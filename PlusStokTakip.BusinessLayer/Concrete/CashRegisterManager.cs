
using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class CashRegisterManager:ICashRegisterService
    {
        private readonly ICashRegisterDal _cashRegisterDal;
        public CashRegisterManager(ICashRegisterDal cashRegisterDal)
        {
            _cashRegisterDal = cashRegisterDal;
        }
        public void TDelete(CashRegister entity)
        {
            _cashRegisterDal.Delete(entity);
        }
        public List<CashRegister> TGetAll()
        {
            return _cashRegisterDal.GetAll();
        }
        public CashRegister TGetById(int id)
        {
            return _cashRegisterDal.GetById(id);
        }
        public void TInsert(CashRegister entity)
        {
            _cashRegisterDal.Insert(entity);
        }
        public void TUpdate(CashRegister entity)
        {
            _cashRegisterDal.Update(entity);
        }
    }
}
