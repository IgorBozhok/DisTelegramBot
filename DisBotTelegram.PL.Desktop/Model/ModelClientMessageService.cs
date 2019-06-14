using DisBotTelegram.BLL.DTO;
using DisBotTelegram.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace DisBotTelegram.PL.Desktop.Model
{
    public class ModelClientMessageService
    {
        private readonly IClientMessageService _clientMessageService;
        private ObservableCollection<ClientMessageInfo> _clientsMessages;

        public ModelClientMessageService(IUnityContainer container)
        {
            _clientMessageService = container.Resolve<IClientMessageService>();
            _clientsMessages = new ObservableCollection<ClientMessageInfo>();
        }

        public ObservableCollection<ClientMessageInfo> GetClientMessage()
        {
            if (_clientsMessages != null)
            {
                var result = _clientMessageService.GetAll();
                if (result.IsSuccess)
                {
                    return new ObservableCollection<ClientMessageInfo>(result.Value);
                }
            }
            return _clientsMessages;
        }


        public void Add(ClientMessageInfo itemInfo)
        {
            var result = _clientMessageService.Add(itemInfo);
            if (result.IsSuccess)
            {
                return;
            }
            throw new Exception(result.Message);
        }

    }
}
