
using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class StockMovementsManager: IStockMovementsService
    {
        private readonly IStockMovementsDal _stockMovementsDal;

        public StockMovementsManager(IStockMovementsDal stockMovementsDal)
        {
            _stockMovementsDal = stockMovementsDal;
        }

        public void TDelete(StockMovements entity)
        {
           _stockMovementsDal.Delete(entity);
        }

        public List<StockMovements> TGetAll()
        {
            return _stockMovementsDal.GetAll();
        }

        public StockMovements TGetById(int id)
        {
            return _stockMovementsDal.GetById(id);
        }

        public void TInsert(StockMovements entity)
        {
            _stockMovementsDal.Insert(entity);
        }

        public void TUpdate(StockMovements entity)
        {
            _stockMovementsDal.Update(entity);
        }
    }
}
