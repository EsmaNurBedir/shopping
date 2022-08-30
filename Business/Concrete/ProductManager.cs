using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Contants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Utilities.Bussiness;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [CacheAspect]
        //[SecuredOperation("admin")]
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            //iş kuralları

          IResult result=  BusinessRules.Run(CheckIfProductCountCategoryCorrect(product.CategoryId)
              ,CheckIfCategoryLimitExceded());
            if (result != null)
            {
                return result;
            }

           _productDal.Add(product);
           return new SuccessResult(Messages.Added);

        }

        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(Messages.Deleted);
        }

        //[CacheRemoveAspect(IProductService.Get)] => Bunları siler...
        //[PerformanceAspect(5)]
        public IDataResult<List<Product>> GetAll()
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.Listed);
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>
            (_productDal.Get(p => p.ProductId == productId), Messages.ListedId);
        }

        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.Updated);
        }

        //Bir kategoride en fazla 10 ürün olabilir//
        private IResult CheckIfProductCountCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result>=15)
            {
                return new ErrorResult("En fazla 15 kategori eklenebilir.");
            }
            return new SuccessResult();
        }

        //Eger mevcut kategorıy sayısı 15 şi geçtiyse sisteme yeni ürün eklenemez.
        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult("Kategori limiti aşıldıgı için yeni ürün eklenemiyor.");
            }
            return new SuccessResult();
        }

        public IDataResult<List<Product>> GetByCategoryId(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == categoryId).ToList());
        }

        public IDataResult<List<Product>> GetByColorId(int colorId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.ColorId == colorId).ToList());
        }





    }
}
