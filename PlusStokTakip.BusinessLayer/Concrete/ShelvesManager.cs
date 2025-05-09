using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class ShelvesManager : IShelvesService
    {
        private readonly IShelvesDal _shelvesDal;

        public ShelvesManager(IShelvesDal shelvesDal)
        {
            _shelvesDal = shelvesDal;
        }

        public void TDelete(Shelves entity)
        {
            _shelvesDal.Delete(entity);
        }

        public List<Shelves> TGetAll()
        {
            return _shelvesDal.GetAll();
        }

        public Shelves TGetById(int id)
        {
            return _shelvesDal.GetById(id);
        }

        public void TInsert(Shelves entity)
        {
           _shelvesDal.Insert(entity);
        }

        public void TUpdate(Shelves entity)
        {
           _shelvesDal.Update(entity);
        }
    }
}
