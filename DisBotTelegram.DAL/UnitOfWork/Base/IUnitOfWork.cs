using DisBotTelegram.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisBotTelegram.DAL.UnitOfWork.Base
{
    internal interface IUnitOfWork : IDisposable
    {
        void Commit();
        IRepository<T> GetRepository<T>() where T : class, IBaseEntity;
    }
}
