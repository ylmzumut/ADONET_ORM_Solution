﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADONET_ORM_Common
{
    public interface IORM<T> where T : class, new()
    {

        bool Insert(T Entity);
        bool Update(T Entity);
        bool Delete(T Entity);
        List<T> Select();

    }
}
