using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Repositories;
using PlusStokTakip.EntityLayer.EntityModel;

namespace PlusStokTakip.DataAccessLayer.EntityFramework
{
    public class EfBankTransactionsDal : GenericRepository<BankTransactions>, IBankTransactionsDal
    {
    }
}
