using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DisBotTelegram.DAL.Contexts;
using DisBotTelegram.DAL.Entities;

namespace DisBotTelegram.DAL.Initializers
{
     internal class BotTelegramInitializater : CreateDatabaseIfNotExists<BotTelegramContext>
    {
        protected override void Seed(BotTelegramContext db)
        {
            User admin = new User()
            {
                UserLogin = "Admin",
                Password = "123"
            };
            db.Users.Add(admin);
            db.SaveChanges();
        }
    }
}
