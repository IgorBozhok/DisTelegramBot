using DisBotTelegram.DAL.Entities;
using DisBotTelegram.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisBotTelegram.DAL.Repositories
{
    internal class DispatcherMessageRepository: GenericRepository<DispatherMessage>
    {
        public DispatcherMessageRepository(DbContext db) : base(db)
        {

        }
    }
}
