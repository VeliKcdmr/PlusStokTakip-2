using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class BrandsManager : IBrandsService
    {
        private readonly IBrandsDal _brandsDal;

        public BrandsManager(IBrandsDal brandsDal)
        {
            _brandsDal = brandsDal;
        }

        public void TDelete(Brands entity)
        {
           _brandsDal.Delete(entity);
        }

        public List<Brands> TGetAll()
        {
            return _brandsDal.GetAll();
        }

        public Brands TGetById(int id)
        {
           return _brandsDal.GetById(id);
        }

        public void TInsert(Brands entity)
        {
            _brandsDal.Insert(entity);
        }

        public void TUpdate(Brands entity)
        {
           _brandsDal.Update(entity);
        }
    }
}
