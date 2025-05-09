using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class ModelsManager : IModelsService
    {
        private readonly IModelsDal _modelsDal;

        public ModelsManager(IModelsDal modelsDal)
        {
            _modelsDal = modelsDal;
        }

        public void TDelete(Models entity)
        {
            _modelsDal.Delete(entity);
        }

        public List<Models> TGetAll()
        {
            return _modelsDal.GetAll();
        }

        public Models TGetById(int id)
        {
            return _modelsDal.GetById(id);
        }

        public void TInsert(Models entity)
        {
            _modelsDal.Insert(entity);
        }

        public void TUpdate(Models entity)
        {
            _modelsDal.Update(entity);
        }
    }
}
