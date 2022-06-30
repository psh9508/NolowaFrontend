﻿using Microsoft.AspNetCore.SignalR.Client;
using NolowaFrontend.Core;
using NolowaFrontend.Extensions;
using NolowaFrontend.Servicies;
using NolowaFrontend.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NolowaFrontend.ViewModels
{
    public class DirectMessageDialogItem
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public bool IsMine { get; set; }
    }

    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OthersTemplate { get; set; }
        public DataTemplate MineTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var message = (DirectMessageDialogItem)item;

            return message.IsMine ? MineTemplate : OthersTemplate;
        }
    }

    public class DirectMessageSendVM : ViewModelBase
    {
        private readonly HubConnection _hubConnection;
        private readonly ISignalRService _signalRService;

        private ObservableCollection<DirectMessageDialogItem> _dialog = new ObservableCollection<DirectMessageDialogItem>();

        public ObservableCollection<DirectMessageDialogItem> Dialog
        {
            get { return _dialog; }
            set { _dialog = value; OnPropertyChanged(); }
        }

        private string _mseeage = string.Empty;

        public string Message
        {
            get { return _mseeage; }
            set { _mseeage = value; OnPropertyChanged(); }
        }

        private bool _isHide = false;

        public bool IsHide
        {
            get { return _isHide; }
            set { _isHide = value; OnPropertyChanged(); }
        }

        #region Commands
        private ICommand _loadedCommand;

        public ICommand LoadedCommand
        {
            get
            {
                return GetRelayCommand(ref _loadedCommand, async _ =>
                {
                    var dialogResponse = await _signalRService.GetDialog(AppConfiguration.LoginUser.Id, 3);

                    if (dialogResponse.Count() > 0)
                    {
                        var dialogData = dialogResponse.Select(x => new DirectMessageDialogItem()
                        {
                            SenderId = x.SenderId,
                            ReceiverId = x.ReceiverId,
                            Message = x.Message,
                            Time = x.Time,
                            IsMine = x.ReceiverId == AppConfiguration.LoginUser.Id,
                        }).ToObservableCollection();

                        Dialog = dialogData;
                    }
                });
            }
        }

        private ICommand _sendDirectMessageCommand;

        public ICommand SendDirectMessageCommand
        {
            get
            {
                return GetRelayCommand(ref _sendDirectMessageCommand, async _ =>
                {
                    if (Message.IsNotVaild())
                        return;

                    string message = Message;
                    Message = string.Empty;

                    if (_hubConnection.State != HubConnectionState.Connected)
                    {
                        await _hubConnection.StartAsync();
                        await _hubConnection.SendAsync("Login", AppConfiguration.LoginUser.Id);
                    }

                    await _hubConnection.SendAsync("SendMessage", AppConfiguration.LoginUser.Id, 3, message);
                });
            }
        }

        private ICommand _backButtonCommand;

        public ICommand BackButtonCommand
        {
            get
            {
                return GetRelayCommand(ref _backButtonCommand, async _ =>
                {
                    IsHide = true;
                });
            }
        } 
        #endregion

        public DirectMessageSendVM()
        {
            _signalRService = new SignalRService();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/DirectMessage")
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On("ReceiveDirectMessage", (long senderId, long receiveId, string message, string time) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => {
                    Dialog.Add(new DirectMessageDialogItem()
                    {
                        SenderId = senderId,
                        ReceiverId = receiveId,
                        Message = message,
                        Time = time,
                        IsMine = senderId == AppConfiguration.LoginUser.Id,
                    });
                });
            });
        }
    }
}
