﻿namespace ECatalogueManager.DTOs
{
    public class StudentToGet
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int? StreetNumber { get; set; }
    }
}
