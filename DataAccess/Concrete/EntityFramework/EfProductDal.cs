using Core.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, CarContext>, IProductDal
    {
        public List<ProductDetailDto> GetProductDetailDtos(Expression<Func<Product, bool>> filter = null)
        {
            
                using (var context = new CarContext())
                {
                    IQueryable<ProductDetailDto> result =
                        from product in filter is null ? context.Products : context.Products.Where(filter)
                        join category in context.Categories
                        on product.CategoryId equals category.CategoryId
                        join color in context.Colors
                        on product.ColorId equals color.ColorId
                        select new ProductDetailDto
                        {
                            ProductId = product.ProductId,
                            CategoryName = category.CategoryName,
                            ColorName = color.ColorName,
                            UnitPrice = product.UnitPrice,
                            Description = product.Description,
                            CategoryId = product.CategoryId,
                            ColorId = color.ColorId,
                            //ImagePath = (from i in context.CarImages where i.CarId == car.CarId select i.ImagePath).FirstOrDefault()
                        };
                    return result.ToList();

                }
            
        }

       
    
    }
}
