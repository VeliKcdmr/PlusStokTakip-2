using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class BankAccountsManager : IBankAccountsService
    {
        private readonly IBankAccountsDal _bankAccountsDal;

        public BankAccountsManager(IBankAccountsDal bankAccountsDal)
        {
            _bankAccountsDal = bankAccountsDal;
        }

        public void TDelete(BankAccounts entity)
        {
            _bankAccountsDal.Delete(entity);
        }

        public List<BankAccounts> TGetAll()
        {
            return _bankAccountsDal.GetAll();
        }

        public BankAccounts TGetById(int id)
        {
            return _bankAccountsDal.GetById(id);
        }

        public void TInsert(BankAccounts entity)
        {
           _bankAccountsDal.Insert(entity);
        }

        public void TUpdate(BankAccounts entity)
        {
            _bankAccountsDal.Update(entity);
        }
    }
}
