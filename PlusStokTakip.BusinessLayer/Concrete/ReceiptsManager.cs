using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class ReceiptsManager : IReceiptsService
    {
        private readonly IReceiptsDal _receiptsDal;

        public ReceiptsManager(IReceiptsDal receiptsDal)
        {
            _receiptsDal = receiptsDal;
        }

        public void TDelete(Receipts entity)
        {
            _receiptsDal.Delete(entity);
        }

        public List<Receipts> TGetAll()
        {
            return _receiptsDal.GetAll();
        }

        public Receipts TGetById(int id)
        {
            return _receiptsDal.GetById(id);
        }

        public void TInsert(Receipts entity)
        {
            _receiptsDal.Insert(entity);
        }

        public void TUpdate(Receipts entity)
        {
            _receiptsDal.Update(entity);
        }
    }
}
