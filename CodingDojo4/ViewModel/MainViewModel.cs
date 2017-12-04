using CodingDojo4.Connection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CodingDojo4.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool isConnected = false;
        private Client clientcom;
        public string ChatName { get; set; }
        public string Message { get; set; }
        public ObservableCollection<string> ReceivedMessages { get; set; }
        public RelayCommand ConnectBtnClickCmd { get; set; }
        public RelayCommand SendBtnClickCmd { get; set; }


        public MainViewModel()
        {
            Message = "";
            ReceivedMessages = new ObservableCollection<string>();
            //init Command for Connect button with canExecute statement
            ConnectBtnClickCmd = new RelayCommand(
                () =>
                {
                    isConnected = true;
                    clientcom = new Client("127.0.0.1", 8090, new Action<string>(NewMessageReceived), ClientDissconnected);

                },
                () =>
                {
                    return (!isConnected);
                });
            //init Command for Send button with CanExecute statement
            SendBtnClickCmd = new RelayCommand(
                () => {
                    clientcom.Send(ChatName + ": " + Message);
                    //write own message to GUI
                    ReceivedMessages.Add("YOU: " + Message);
                }, () => { return (isConnected && Message.Length >= 1); });
        }

        private void ClientDissconnected()
        {
            isConnected = false;
            //do this to force the update of the button visibility otherwise change is done after focus change (clicking into gui)
            CommandManager.InvalidateRequerySuggested();
        }

        private void NewMessageReceived(string message)
        {
            //write new message in Collection to display in GUI
            //switch thread to GUI thread to avoid problems
            App.Current.Dispatcher.Invoke(() =>
            {
                ReceivedMessages.Add(message);
            });
        }
    }
}