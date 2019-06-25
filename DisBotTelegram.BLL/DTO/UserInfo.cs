using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisBotTelegram.BLL.DTO
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<DispatcherMessageInfo> Messages { get; set; }

        public override string ToString()
        {
            return $"{UserLogin} {FirstName}";
        }
    }
}
