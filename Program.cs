
using CatalogoApi.Data;
using CatalogoApi.Filters;
using CatalogoApi.Models;
using CatalogoApi.Repositories;
using CatalogoApi.Security;
using CatalogoApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace CatalogoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<ApiLoggingFilter>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<JwtUtil>();
            builder.Services.AddScoped<JwtService>();

            builder.Services
             .AddIdentity<User, IdentityRole>()
             .AddEntityFrameworkStores<AppDbContext>() //  Essa linha diz ao Identity para usar o UserDbContext para armazenar os dados do Identity no banco de dados. Ou seja, ele vai usar o UserDbContext para criar tabelas como AspNetUsers (para usu�rios), AspNetRoles (para roles), AspNetUserRoles (associa��o de usu�rios e roles), etc.
             .AddDefaultTokenProviders();

            string? dbConnection = builder.Configuration.GetConnectionString("dbConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(dbConnection, ServerVersion.AutoDetect(dbConnection));
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var jwtUtil = new JwtUtil(builder.Configuration);

            // .AddJwtBearer(): Este m�todo adiciona a autentica��o baseada em JWT ao pipeline de autentica��o. Ele configura os detalhes sobre como o token JWT ser� validado quando enviado nas requisi��es HTTP.
            // ...Quando uma requisi��o chega ao servidor com um token JWT no cabe�alho Authorization, o middleware JwtBearer valida esse token (por exemplo, verificando sua assinatura, se ele est� expirado, se o emissor � v�lido, etc.).
            // Com esta configura��o, o ASP.NET Core sabe que as requisi��es devem ser autenticadas usando tokens JWT e que as requisi��es com tokens no cabe�alho Authorization: Bearer <token> ser�o validadas automaticamente antes de serem passadas para o controlador da API.
            builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // Valida o emissor do token
                    ValidateAudience = true, // Valida o p�blico alvo do token
                    ValidateLifetime = true, // Valida o tempo de expira��o do token
                    ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = jwtUtil.GetSymmetricSecurityKey() // Chave usada para assinar o token
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            }).AddNewtonsoftJson();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // Configurar o esquema de seguran�a JWT
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Informe o token JWT no campo abaixo como: 'Bearer {seu_token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey // Tipo de seguran�a � uma chave API, pois o token ser� enviado como um valor de chave
                });

                // Define a exig�ncia de seguran�a para todas as opera��es da API
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { 
                        // Refer�ncia ao tipo de seguran�a configurado acima, "Bearer"
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {} // N�o h� escopos adicionais necess�rios para o JWT (n�o usamos permiss�es espec�ficas no exemplo)
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Cada middleware no pipeline processa a requisi��o conforme ela passa pelo pipeline. O ASP.NET Core chama o m�todo InvokeAsync automaticamente para cada middleware registrado.
            app.UseMiddleware<Exceptions.ExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
