using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class ProductsManager : IProductsService
    {
        private readonly IProductsDal _productsDal;

        public ProductsManager(IProductsDal productsDal)
        {
            _productsDal = productsDal;
        }

        public void TDelete(Products entity)
        {
            _productsDal.Delete(entity);
        }

        public List<Products> TGetAll()
        {
            return _productsDal.GetAll();
        }

        public Products TGetById(int id)
        {
            return _productsDal.GetById(id);
        }

        public void TInsert(Products entity)
        {
            _productsDal.Insert(entity);
        }

        public void TUpdate(Products entity)
        {
            _productsDal.Update(entity);
        }
    }
}
