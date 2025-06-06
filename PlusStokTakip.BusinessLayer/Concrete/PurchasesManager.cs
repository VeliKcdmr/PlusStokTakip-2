
using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class PurchasesManager : IPurchasesService
    {
        private readonly IPurchasesDal _purchasesDal;
        public PurchasesManager(IPurchasesDal purchasesDal)
        {
            _purchasesDal = purchasesDal;
        }
        public void TDelete(Purchases entity)
        {
            _purchasesDal.Delete(entity);
        }
        public List<Purchases> TGetAll()
        {
            return _purchasesDal.GetAll();
        }
        public Purchases TGetById(int id)
        {
            return _purchasesDal.GetById(id);
        }
        public void TInsert(Purchases entity)
        {
            _purchasesDal.Insert(entity);
        }
        public void TUpdate(Purchases entity)
        {
            _purchasesDal.Update(entity);
        }
    }
}
