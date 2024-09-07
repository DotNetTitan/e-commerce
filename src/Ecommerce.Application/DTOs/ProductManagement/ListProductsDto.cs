﻿namespace Ecommerce.Application.DTOs.ProductManagement
{
    public class ListProductsDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
    }
}