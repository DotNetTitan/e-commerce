﻿namespace Ecommerce.Application.DTOs.Categories
{
    public class CreateCategoryDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}