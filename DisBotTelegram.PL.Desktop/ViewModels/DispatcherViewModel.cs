using DisBotTelegram.BLL;
using DisBotTelegram.BLL.DTO;
using DisBotTelegram.BLL.Interfaces;
using DisBotTelegram.BLL.Services;
using DisBotTelegram.PL.Desktop.Model;
using DisBotTelegram.PL.Desktop.ReleyCommand;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace DisBotTelegram.PL.Desktop.ViewModels
{
    public class DispatcherViewModel : BaseViewModel
    {
        #region Fields
        private BotLogic _botLogic;
        private UnityContainer _container;
        private ModelClientMessageService _modelClientMessageService;
        private ModelClientService _modelClientService;
        private ModelDespatcherMessageService _modelDespatcherMessageService;
        private ModelWorkTimeDispatcherService _modelWorkTimeDispatcherService;

        private bool _isConnect;
        private bool _isDisconnect;
        private bool _isSendMessage;

        private ObservableCollection<ClientInfo> _clients;
        private ObservableCollection<WorkTimeDispatcherInfo> _workTimeDispatchers;

        private string _userName;
        private string _message;
        private UserInfo _disInfo;
        private ObservableCollection<ClientInfo> _clientsChat;

        private ClientInfo _сhoiceClient;

        #endregion

        #region Properties
        public ICommand SendMessageCommand { get; set; }
        public ICommand ConnectCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }
        public ObservableCollection<DisBotMessage> Messages { get; set; }

        public ClientInfo ChoiceClient
        {
            get { return _сhoiceClient; }
            set
            {
                _сhoiceClient = value;
                OnPropertyChanged();
                if (!String.IsNullOrEmpty(ChoiceClient.Username))
                {
                    UserName = ChoiceClient.Username;
                }
                else if (!String.IsNullOrEmpty(ChoiceClient.LastName))
                {
                    UserName = ChoiceClient.LastName;
                }
                else if (!String.IsNullOrEmpty(ChoiceClient.FirstName))
                {
                    UserName = ChoiceClient.FirstName;
                }
                else
                    UserName = "Unknown name";
            }
        }
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }
        public string MessageChat
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }

        public bool IsConnect
        {
            get { return _isConnect; }
            set { _isConnect = value; OnPropertyChanged(); }
        }

        public bool IsDisconnect
        {
            get { return _isDisconnect; }
            set { _isDisconnect = value; OnPropertyChanged(); }
        }

        public bool IsSendMessage
        {
            get { return _isSendMessage; }
            set { _isSendMessage = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ClientInfo> ClientsChat
        {
            get { return _clientsChat; }
            set { _clientsChat = value; OnPropertyChanged(); }
        }

        #endregion

        #region Ctor
        public DispatcherViewModel()
        {
            _botLogic = new BotLogic();
            _container = new UnityContainer();
            _container.RegisterType<IClientMessageService, ClientMessageService>();
            _container.RegisterType<IClientService, ClientService>();
            _container.RegisterType<IDispatcherMessageService, DispatcherMessageService>();
            _container.RegisterType<IWorkTimeDispatcherService, WorkTimeDispatcherService>();
            _modelClientMessageService = new ModelClientMessageService(_container);
            _modelClientService = new ModelClientService(_container);
            _modelDespatcherMessageService = new ModelDespatcherMessageService(_container);
            _modelWorkTimeDispatcherService = new ModelWorkTimeDispatcherService(_container);

            _clients = _modelClientService.GetClients();
            _clientsChat = new ObservableCollection<ClientInfo>();


            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                var window = Application.Current.Windows[i];
                if (window.Title.Equals("DispatcherWindow"))
                {
                    Application.Current.Windows[i].Closing += DispatcherViewModel_Closed;
                    break;
                }
            }

            Messages = new ObservableCollection<DisBotMessage>();
            _сhoiceClient = new ClientInfo();
            _isConnect = true;
            SendMessageCommand = new RelayCommand(OnSendMessageCommandExecute);
            ConnectCommand = new RelayCommand(OnConnectCommand);
            DisconnectCommand = new RelayCommand(OnDisconnectCommand);
            EventPost();
        }

        private void DispatcherViewModel_Closed(object sender, EventArgs e)
        {
            if (IsDisconnect)
            {
                _botLogic.StopReceiving();
            }
        }

        private void OnDisconnectCommand(object obj)
        {
            _botLogic.StopReceiving();
            IsConnect = true;
            IsDisconnect = false;
            IsSendMessage = false;
        }
        #endregion

        #region Commands
        private void OnConnectCommand(object obj)
        {
            _botLogic.ReciveMessage();
            IsConnect = false;
            IsDisconnect = true;
        }

        private void EventPost()
        {
            _workTimeDispatchers = _modelWorkTimeDispatcherService.GetWokrsTime();
            _disInfo = new UserInfo()
            {
                Id = _workTimeDispatchers.Last().UserId,
                UserLogin = _workTimeDispatchers.Last().Login
            };
            _botLogic.Log += buffer =>
            {
                var countClientDB = 0;
                var coutClientList = 0;

                if (_clients.Count == 0)
                {
                    var addClient = new ClientInfo()
                    {
                        FirstName = _botLogic.Masseges.FirstName,
                        LastName = _botLogic.Masseges.LastName,
                        TelegramId = _botLogic.Masseges.Id,
                        Username = _botLogic.Masseges.UserName,
                    };

                    _modelClientService.Add(addClient);
                    _clients.Add(addClient);
                    _clientsChat.Add(addClient);
                }

                foreach (var client in _clients)
                {

                    if (client.TelegramId.Equals(_botLogic.Masseges.Id))
                    {
                        break;
                    }
                    else if (!string.IsNullOrEmpty(client.TelegramId))
                    {
                        countClientDB++;
                    }

                    if (_clients.Count == countClientDB)
                    {
                        var addClient = new ClientInfo()
                        {
                            FirstName = _botLogic.Masseges.FirstName,
                            LastName = _botLogic.Masseges.LastName,
                            TelegramId = _botLogic.Masseges.Id,
                            Username = _botLogic.Masseges.UserName,
                        };
                        _modelClientService.Add(addClient);
                        _clients.Add(addClient);
                        _clientsChat.Add(addClient);
                        break;
                    }
                }

                var addClientList = new ClientInfo()
                {
                    FirstName = _botLogic.Masseges.FirstName,
                    LastName = _botLogic.Masseges.LastName,
                    TelegramId = _botLogic.Masseges.Id,
                    Username = _botLogic.Masseges.UserName,
                };
                ;
                if (_clientsChat.Count == 0)
                {
                    _clientsChat.Add(addClientList);
                }

                foreach (var item in _clientsChat)
                {
                    if (item.TelegramId.Equals(_botLogic.Masseges.Id))
                    {
                        break;
                    }
                    else if (!item.TelegramId.Equals(_botLogic.Masseges.Id))
                    {
                        coutClientList++;
                    }

                    if (coutClientList == _clientsChat.Count())
                    {
                        _clientsChat.Add(addClientList);
                        break;
                    }
                }

                Messages.Add(buffer);
                var tmpId = 0;
                foreach (var item in _clients)
                {
                    if (item.TelegramId.Equals(_botLogic.Masseges.Id))
                    {
                        tmpId = item.Id;
                        break;
                    }
                }

                CheckUserName();


                var messageDB = new ClientMessageInfo()
                {
                    MessageClient = _botLogic.Masseges.Content,
                    TimeMassage = _botLogic.Masseges.Date,
                    UserId = _workTimeDispatchers.Last().UserId,
                    Id = tmpId,
                    ClientId = tmpId
                };
                _modelClientMessageService.Add(messageDB);
                if (!String.IsNullOrEmpty(UserName))
                {
                    IsSendMessage = true;
                }
            };
        }
        private void OnSendMessageCommandExecute(object obj)
        {
            if (String.IsNullOrEmpty(UserName))
            {
                return;
            }
            if (!String.IsNullOrEmpty(_сhoiceClient.TelegramId))
            {
                _botLogic.Bot_Send_Message(_сhoiceClient.TelegramId, _message);
                CheckUserName();
            }
            else
                _botLogic.Bot_Send_Message(_botLogic.Masseges.Id, _message);
            var tmpId = 0;
            foreach (var item in _clients)
            {
                if (item.TelegramId.Equals(_botLogic.Masseges.Id))
                {
                    tmpId = item.Id;
                    break;
                }
            }
            var message = new DisBotMessage()
            {
                Content = _message,
                Date = DateTime.Now,
                UserName = _disInfo.UserLogin,
                Type = DisBotMessage.MessageType.InMessage,
            };
            Messages.Add(message);

            var addDispatcherMessage = new DispatcherMessageInfo()
            {
                ClientId = tmpId,
                MessageDispather = _message,
                TimeMassage = DateTime.Now,
                UserId = _workTimeDispatchers.Last().UserId,
                Id = _workTimeDispatchers.Last().UserId

            };
            _modelDespatcherMessageService.Add(addDispatcherMessage);
        }

        private void CheckUserName()
        {
            if (!String.IsNullOrEmpty(_botLogic.Masseges.UserName))
            {
                UserName = _botLogic.Masseges.UserName;
            }
            else if (!String.IsNullOrEmpty(_botLogic.Masseges.LastName))
            {
                UserName = _botLogic.Masseges.LastName;
            }
            else if (!String.IsNullOrEmpty(_botLogic.Masseges.FirstName))
            {
                UserName = _botLogic.Masseges.FirstName;
            }
            else
                UserName = "Unknown name";
        }
        #endregion
    }
}
