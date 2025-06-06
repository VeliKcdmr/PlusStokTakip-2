using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class BankTransactionsManager: IBankTransactionsService
    {
        private readonly IBankTransactionsDal _bankTransactionsDal;
        public BankTransactionsManager(IBankTransactionsDal bankTransactionsDal)
        {
            _bankTransactionsDal = bankTransactionsDal;
        }
        public void TDelete(BankTransactions entity)
        {
            _bankTransactionsDal.Delete(entity);
        }
        public List<BankTransactions> TGetAll()
        {
            return _bankTransactionsDal.GetAll();
        }
        public BankTransactions TGetById(int id)
        {
            return _bankTransactionsDal.GetById(id);
        }
        public void TInsert(BankTransactions entity)
        {
            _bankTransactionsDal.Insert(entity);
        }
        public void TUpdate(BankTransactions entity)
        {
            _bankTransactionsDal.Update(entity);
        }
    }
}
