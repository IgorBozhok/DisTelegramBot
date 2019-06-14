using DisBotTelegram.BLL.DTO;
using DisBotTelegram.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisBotTelegram.BLL.Interfaces
{
    public interface IClientMessageService
    {
        ResultOperationInfo<IEnumerable<ClientMessageInfo>> GetAll();
        ResultOperationInfo<ClientMessageInfo> GetId(int itemId);

        ResultOperationInfo Add(ClientMessageInfo item);
        ResultOperationInfo Update(ClientMessageInfo item);
        ResultOperationInfo Delete(int itemId);

        ResultOperationInfo<ClientMessageInfo> Create(ClientMessageInfo itemInfo);
    }
}
