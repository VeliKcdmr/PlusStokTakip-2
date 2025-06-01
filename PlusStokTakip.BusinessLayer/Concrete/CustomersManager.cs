using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class CustomersManager: ICustomersServices
    {
        private readonly ICustomersDal _customersDal;

        public CustomersManager(ICustomersDal customersDal)
        {
            _customersDal = customersDal;
        }

        public void TDelete(Customers entity)
        {
          _customersDal.Delete(entity);
        }

        public List<Customers> TGetAll()
        {
            return _customersDal.GetAll();
        }

        public Customers TGetById(int id)
        {
           return _customersDal.GetById(id);
        }

        public void TInsert(Customers entity)
        {
            _customersDal.Insert(entity);
        }

        public void TUpdate(Customers entity)
        {
           _customersDal.Update(entity);
        }
    }
}
