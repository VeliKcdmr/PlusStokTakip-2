using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System.Collections.Generic;


namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class CategoriesManager : ICategoriesService
    {
        private readonly ICategoriesDal _categoriesDal;

        public CategoriesManager(ICategoriesDal categoriesDal)
        {
            _categoriesDal = categoriesDal;
        }

        public void TDelete(Categories entity)
        {
            _categoriesDal.Delete(entity);
        }

        public List<Categories> TGetAll()
        {
            return _categoriesDal.GetAll();
        }

        public Categories TGetById(int id)
        {
            return _categoriesDal.GetById(id);
        }

        public void TInsert(Categories entity)
        {
            _categoriesDal.Insert(entity);
        }

        public void TUpdate(Categories entity)
        {
           _categoriesDal.Update(entity);
        }
    }
}
