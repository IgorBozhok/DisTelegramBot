using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisBotTelegram.PL.Desktop.Model
{
    public class MessageAll
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }

        public override string ToString()
        {
            return $"{DateTime} [{UserName}] : {Message} ";
        }
    }
}
