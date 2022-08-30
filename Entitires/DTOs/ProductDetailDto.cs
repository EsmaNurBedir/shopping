using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class ProductDetailDto:IDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int ColorId { get; set; }
        public string CategoryName { get; set; }
        public string ColorName { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
