using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class SalesManager: ISalesService
    {
        private readonly ISalesDal _salesDal;

        public SalesManager(ISalesDal salesDal)
        {
            _salesDal = salesDal;
        }

        public void TDelete(Sales entity)
        {
          _salesDal.Delete(entity);
        }

        public List<Sales> TGetAll()
        {
            return _salesDal.GetAll();
        }

        public Sales TGetById(int id)
        {
            return _salesDal.GetById(id);
        }

        public void TInsert(Sales entity)
        {
            _salesDal.Insert(entity);
        }

        public void TUpdate(Sales entity)
        {
            _salesDal.Update(entity);
        }

        public void TUpdate(int saleId)
        {
            throw new NotImplementedException();
        }
    }
}
