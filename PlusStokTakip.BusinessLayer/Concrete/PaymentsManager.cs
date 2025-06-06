
using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class PaymentsManager: IPaymentsService
    {
        private readonly IPaymentsDal _paymentsDal;
        public PaymentsManager(IPaymentsDal paymentsDal)
        {
            _paymentsDal = paymentsDal;
        }
        public void TDelete(Payments entity)
        {
            _paymentsDal.Delete(entity);
        }
        public List<Payments> TGetAll()
        {
            return _paymentsDal.GetAll();
        }
        public Payments TGetById(int id)
        {
            return _paymentsDal.GetById(id);
        }
        public void TInsert(Payments entity)
        {
            _paymentsDal.Insert(entity);
        }
        public void TUpdate(Payments entity)
        {
            _paymentsDal.Update(entity);
        }
    }    
}
