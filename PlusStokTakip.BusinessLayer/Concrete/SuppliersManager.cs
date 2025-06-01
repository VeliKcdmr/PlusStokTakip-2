using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class SuppliersManager : ISuppliersService
    {
        private readonly ISuppliersDal _suppliersDal;

        public SuppliersManager(ISuppliersDal suppliersDal)
        {
            _suppliersDal = suppliersDal;
        }

        public void TDelete(Suppliers entity)
        {
           _suppliersDal.Delete(entity);
        }

        public List<Suppliers> TGetAll()
        {
            return _suppliersDal.GetAll();
        }

        public Suppliers TGetById(int id)
        {
            return _suppliersDal.GetById(id);
        }

        public void TInsert(Suppliers entity)
        {
            _suppliersDal.Insert(entity);
        }

        public void TUpdate(Suppliers entity)
        {
            _suppliersDal.Update(entity);
        }
    }
}
