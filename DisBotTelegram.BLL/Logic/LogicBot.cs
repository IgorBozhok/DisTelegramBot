using DisBotTelegram.BLL.DTO;
using System;
using System.Threading;
using System.Windows;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using DisBotTelegram.BLL.Helper;

namespace DisBotTelegram.BLL.Logic
{
    public class LogicBot
    {
        private System.Threading.SynchronizationContext _synchronizationContext;
        private TelegramBotClient _botclient;

        public Action<DTO.DisBotMessage> Log { get; set; }

        private DisBotMessage _masseges;

        public DisBotMessage Masseges
        {
            get { return _masseges; }
            set { _masseges = value; }
        }

        public LogicBot()
        {
            _masseges = new DisBotMessage();
            _synchronizationContext = SynchronizationContext.Current;
            _botclient = new TelegramBotClient("744399662:AAFJafKh3iNO_h7upw4sfGN27p9YXbDeKbc");
            _botclient.OnMessage += OnMessage;
            My_checkinternet();
        }

        public bool My_checkinternet()
        {
            try
            {
                var me = _botclient.GetMeAsync().Result;
                return true;
            }
            catch
            {
                MessageBox.Show("No internet connection !!", "Notification");
                return false;
            }
        }
        public void ReciveMessage()
        {

            _botclient.StartReceiving(new UpdateType[] { UpdateType.Message });

        }
        public void StopReceiving()
        {
            _botclient.StopReceiving();
            _synchronizationContext.OperationCompleted();
        }
        public async void Bot_Send_Message(string id, string message)
        {

            if (message != string.Empty && id != string.Empty)
            {
                await _botclient.SendTextMessageAsync(chatId: id, text: message);
            }
            if (id == string.Empty)
            {
                MessageBox.Show("No internet connection !!", "Notification");
            }
        }
        private async void OnMessage(object sender, MessageEventArgs e)
        {
            Message msg = e.Message;

            if (msg == null || msg.Type != MessageType.Text)
            {
                return;
            }

            _masseges.Id = e.Message.Chat.Id.ToString();
            _masseges.UserName = e.Message.Chat.Username;
            _masseges.FirstName = e.Message.Chat.FirstName;
            _masseges.LastName = e.Message.Chat.LastName;
            _masseges.Date = DateTime.Now;
            _masseges.Content = e.Message.Text;
            _masseges.Type = DisBotMessage.MessageType.OutMessage;
            _synchronizationContext.Post(obj => Log?.Invoke(_masseges), null);
            await _botclient.SendTextMessageAsync(chatId: e.Message.Chat, text: StaticLogicBot.UserInfo.UserLogin);
        }
    }
}
