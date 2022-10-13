﻿using Data.Models;

namespace ECatalogueManager.DTOs
{
    public class StudentToGet
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }
    }
}
