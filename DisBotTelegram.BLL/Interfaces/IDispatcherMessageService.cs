using DisBotTelegram.BLL.DTO;
using DisBotTelegram.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisBotTelegram.BLL.Interfaces
{
    internal interface IDispatcherMessageService
    {
        ResultOperationInfo<IEnumerable<DispatcherMessageInfo>> GetAll();
        ResultOperationInfo<DispatcherMessageInfo> GetId(int itemId);
        ResultOperationInfo Add(DispatcherMessageInfo item);
        ResultOperationInfo Update(DispatcherMessageInfo item);
        ResultOperationInfo Delete(int itemId);
        ResultOperationInfo<DispatcherMessageInfo> Create(DispatcherMessageInfo itemInfo);
    }
}
