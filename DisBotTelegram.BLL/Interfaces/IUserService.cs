using DisBotTelegram.BLL.DTO;
using DisBotTelegram.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisBotTelegram.BLL.Interfaces
{
    internal interface IUserService
    {
        ResultOperationInfo<IEnumerable<UserInfo>> GetAll();
        ResultOperationInfo<UserInfo> GetId(int itemId);
        ResultOperationInfo Add(UserInfo item);
        ResultOperationInfo Update(UserInfo item);
        ResultOperationInfo Delete(int itemId);
        ResultOperationInfo<UserInfo> Create(UserInfo itemInfo);
    }
}
