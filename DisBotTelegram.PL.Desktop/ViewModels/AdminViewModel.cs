using DisBotTelegram.BLL.DTO;
using DisBotTelegram.BLL.Interfaces;
using DisBotTelegram.BLL.Services;
using DisBotTelegram.PL.Desktop.Model;
using DisBotTelegram.PL.Desktop.ReleyCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace DisBotTelegram.PL.Desktop.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        #region Fields        
        private UnityContainer _container;
        private ModelUserService _modelUserService;
        private ModelClientService _modelClientService;
        private ModelClientMessageService _modelClientMessageService;
        private ModelDespatcherMessageService _modelDespatcherMessageService;

        private ObservableCollection<MessageAll> _messages;
        private ObservableCollection<UserInfo> _users;
        private ObservableCollection<ClientInfo> _clients;
        private ObservableCollection<ClientMessageInfo> _clentMessages;
        private ObservableCollection<DispatcherMessageInfo> _dispatcherMessages;

        private UserInfo _userLinq;
        private ClientInfo _clientLinq;
        private DateTime _fromDate;
        private DateTime _toDate;
        private string _login;
        private string _password;
        private string _repeatPassword;
        private string _firstName;
        private string _lastName;
        private bool _isChackLogin;
        private string _checkLoginText;
        private string _checkPaswwordText;
        #endregion

        #region Properties
        public ObservableCollection<MessageAll> Messages
        {
            get { return _messages; }
            set { _messages = value; OnPropertyChanged(); }
        }
        public ObservableCollection<UserInfo> Users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ClientInfo> Clients
        {
            get { return _clients; }
            set { _clients = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ClientMessageInfo> ClentMessages
        {
            get { return _clentMessages; }
            set { _clentMessages = value; OnPropertyChanged(); }
        }
        public ObservableCollection<DispatcherMessageInfo> DispatcherMessages
        {
            get { return _dispatcherMessages; }
            set { _dispatcherMessages = value; OnPropertyChanged(); }
        }
        public UserInfo UserLinq
        {
            get { return _userLinq; }
            set { _userLinq = value; OnPropertyChanged(); }
        }
        public ClientInfo ClientLinq
        {
            get { return _clientLinq; }
            set { _clientLinq = value; OnPropertyChanged(); }
        }
        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; OnPropertyChanged(); }
        }
        public DateTime ToDate
        {
            get { return _toDate; }
            set { _toDate = value; OnPropertyChanged(); }
        }
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                foreach (var item in Users)
                {
                    if (item.UserLogin.Equals(Login))
                    {

                        IsCheckLogin = false;
                        CheckLoginText = "This login exists!";
                        break;
                    }
                    else if (String.IsNullOrEmpty(Login))
                    {
                        IsCheckLogin = false;
                        CheckLoginText = "Enter Login!";
                    }
                    else
                    {
                        IsCheckLogin = true;
                        CheckLoginText = "This login don`t exists!";
                    }

                }
                OnPropertyChanged();
                OnPropertyChanged("IsCorrectPassword");
            }
        }
        public string Paswword
        {
            get { return _password; }
            set
            {
                _password = value; OnPropertyChanged();
                OnPropertyChanged("IsCorrectPassword");
            }
        }
        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set
            {
                _repeatPassword = value;
                if (Paswword.Equals(RepeatPassword))
                {
                    // IsCorrectPassword = true;
                    CheckPaswwordText = "Password is correct";
                }
                else if (String.IsNullOrEmpty(RepeatPassword))
                {
                    // IsCorrectPassword = false;
                    CheckPaswwordText = "Password entry is`t correct";
                }
                else
                {
                    // IsCorrectPassword = false;
                    CheckPaswwordText = "Enter сonfirm зassword";
                }
                OnPropertyChanged();
                OnPropertyChanged("IsCorrectPassword");
            }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged(); }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged(); }
        }
        public string CheckLoginText
        {
            get { return _checkLoginText; }
            set { _checkLoginText = value; OnPropertyChanged(); }
        }
        public string CheckPaswwordText
        {
            get { return _checkPaswwordText; }
            set { _checkPaswwordText = value; OnPropertyChanged(); }
        }
        public bool IsCheckLogin
        {
            get { return _isChackLogin; }
            set { _isChackLogin = value; OnPropertyChanged(); }
        }
        public bool IsCorrectPassword
        {
            get { return !String.IsNullOrWhiteSpace(Paswword) && !String.IsNullOrWhiteSpace(RepeatPassword) && Paswword.Equals(RepeatPassword); }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand OkCommandLinq { get; set; }
        #endregion

        #region Ctor
        public AdminViewModel()
        {
            _container = new UnityContainer();
            _container.RegisterType<IUserService, UserService>();
            _container.RegisterType<IClientService, ClientService>();
            _container.RegisterType<IClientMessageService, ClientMessageService>();
            _container.RegisterType<IDispatcherMessageService, DispatcherMessageService>();
            _modelUserService = new ModelUserService(_container);
            _modelClientService = new ModelClientService(_container);
            _modelClientMessageService = new ModelClientMessageService(_container);
            _modelDespatcherMessageService = new ModelDespatcherMessageService(_container);

            _messages = new ObservableCollection<MessageAll>();

            _users = _modelUserService.GetUsers();
            _clients = _modelClientService.GetClients();
            _clentMessages = _modelClientMessageService.GetClientMessage();
            _dispatcherMessages = _modelDespatcherMessageService.GetDispatcharMessage();

            _fromDate = new DateTime(2019, 05, 01);
            _toDate = DateTime.Now;
            _checkLoginText = "Enter Login!";

            OkCommandLinq = new RelayCommand(OnCommandLinq);
            SaveCommand = new RelayCommand(OnSaveCommand, CanOnSaveExecute);
        }
        #endregion

        #region Commands
        private bool CanOnSaveExecute(object obj)
        {
            return IsCorrectPassword && IsCheckLogin;
        }
        private void OnSaveCommand(object obj)
        {
            if (IsCheckLogin)
            {
                UserInfo tmpUserinfo = new UserInfo()
                {
                    UserLogin = Login,
                    Password = this.Paswword,
                    FirstName = this.FirstName,
                    LastName = this.LastName

                };
                _modelUserService.Add(tmpUserinfo);
                _users.Add(tmpUserinfo);
                Login = String.Empty;
                Paswword = String.Empty;
                RepeatPassword = String.Empty;
                FirstName = String.Empty;
                LastName = String.Empty;
                return;
            }
            else
            {
                MessageBox.Show("Erorr");
            }
        }
        private void OnCommandLinq(object obj)
        {

            Messages.Clear();
            _toDate += new TimeSpan(23, 59, 59);

            var clientMessagesAll = Clients.Join(ClentMessages, c => c.Id, d => d.ClientId, (c, d) => new { client = c, clientMessage = d }).Select(p => new { userName = p.client.TelegramId, dateTime = p.clientMessage.TimeMassage, message = p.clientMessage.MessageClient, userId = p.clientMessage.UserId, clientId = p.clientMessage.ClientId });
            var dispatcharMessagesAll = Users.Join(DispatcherMessages, c => c.Id, d => d.UserId, (c, d) => new { user = c, dispatcharMessage = d }).Select(p => new { userName = p.user.UserLogin, dateTime = p.dispatcharMessage.TimeMassage, message = p.dispatcharMessage.MessageDispather, userId = p.dispatcharMessage.UserId, clientId = p.dispatcharMessage.ClientId });

            if (UserLinq != null && ClientLinq != null)
            {

                var messageUser = Users.Where(user => user.Id == UserLinq.Id).SelectMany(a => a.Messages).Select(p => new { name = UserLinq.UserLogin, dt = p.TimeMassage, message = p.MessageDispather, userId = p.UserId, clientId = p.ClientId });
                var clientMessages = Clients.Where(client => client.Id == ClientLinq.Id).SelectMany(a => a.Messages).Select(p => new { name = ClientLinq.TelegramId, dt = p.TimeMassage, message = p.MessageClient, userId = p.UserId, clientId = p.ClientId });
                var allMessage = messageUser.Union(clientMessages).Where(p => p.name == _userLinq.UserLogin || p.name == _clientLinq.TelegramId).Where(p => p.dt >= _fromDate && p.dt <= _toDate).Where(p => p.userId == _userLinq.Id && p.clientId == _clientLinq.Id).OrderBy(x => x.dt).ToList();

                foreach (var item in allMessage)
                {
                    MessageAll tempMessage = new MessageAll()
                    {
                        DateTime = item.dt,
                        Message = item.message,
                        UserName = item.name
                    };
                    Messages.Add(tempMessage);
                }
            }
            else if (ClientLinq != null && UserLinq == null)
            {
                MessageBox.Show("Не верно введены данные!!!");
                return;
            }
            else if (ClientLinq == null && UserLinq != null)
            {
                MessageBox.Show("Не верно введены данные!!!");
                return;
            }
            else
            {
                MessageBox.Show("Не верно введены данные!!!");
                return;
            }
        }
        #endregion
    }
}
