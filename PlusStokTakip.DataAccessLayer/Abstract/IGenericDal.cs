﻿using System;
using System.Collections.Generic;

namespace PlusStokTakip.DataAccessLayer.Abstract
{
    public interface IGenericDal<T> where T : class
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetById(int id);
        List<T> GetAll();       
    }    
}
