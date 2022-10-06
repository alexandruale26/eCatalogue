﻿using System.ComponentModel.DataAnnotations;

namespace EStudentsManager.DTOs
{
    public class StudentToCreate
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(15, 100)]
        public int Age { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int? StreetNumber { get; set; }
    }
}
