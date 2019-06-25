﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisBotTelegram.BLL.DTO
{
    public class ClientInfo
    {
        public int Id { get; set; }
        public string TelegramId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<ClientMessageInfo> Messages { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} {TelegramId}";
        }
    }
}
