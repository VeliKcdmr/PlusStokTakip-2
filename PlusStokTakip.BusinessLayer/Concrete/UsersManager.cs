using PlusStokTakip.BusinessLayer.Abstract;
using PlusStokTakip.DataAccessLayer.Abstract;
using PlusStokTakip.EntityLayer.EntityModel;
using System;
using System.Collections.Generic;

namespace PlusStokTakip.BusinessLayer.Concrete
{
    public class UsersManager : IUsersService
    {
        private readonly IUserDal _usersDal;

        public UsersManager(IUserDal usersDal)
        {
            _usersDal = usersDal;
        }

        public void TDelete(Users entity)
        {
           _usersDal.Delete(entity);
        }

        public List<Users> TGetAll()
        {
            return _usersDal.GetAll();
        }

        public Users TGetById(int id)
        {
            return _usersDal.GetById(id);
        }

        public void TInsert(Users entity)
        {
            _usersDal.Insert(entity);
        }

        public void TUpdate(Users entity)
        {
           _usersDal.Update(entity);
        }
    }
}
