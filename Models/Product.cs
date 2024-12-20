﻿using CatalogoApi.Validations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogoApi.Models
{
    [Table("products")]   
    
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        [MaiuscFirstLetter]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double? Price { get; set; }

        [Required]
        public int? QuantityAvaiable { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [Required]
        public DateTime DateRegister { get; set; } = DateTime.UtcNow;

        [Required]
        public int? CategoryId { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
