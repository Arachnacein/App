﻿using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models
{
    public class Income
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}