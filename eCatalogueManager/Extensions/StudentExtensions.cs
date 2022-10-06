﻿using Data.Models;
using EStudentsManager.DTOs;

namespace eCatalogueManager.Extensions
{
    public static class StudentExtensions
    {
        public static StudentToGet ToDto(this Student student)
        {
            return new StudentToGet
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age,
                City = student.Address.City,
                Street = student.Address.Street,
                StreetNumber = student.Address.StreetNumber
            };
        }
    }
}
