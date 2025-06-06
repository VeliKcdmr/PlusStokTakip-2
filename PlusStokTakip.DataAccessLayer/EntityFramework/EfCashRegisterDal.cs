
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Repositories;
using PlusStokTakip.EntityLayer.EntityModel;

namespace PlusStokTakip.DataAccessLayer.EntityFramework
{
    public class EfCashRegisterDal:GenericRepository<CashRegister>, ICashRegisterDal
    {
    }
}
