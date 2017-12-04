using CD4_Server.Connection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace CD4_Server.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Server server;
        private const int port = 8090;
        private const string ip = "127.0.0.1";
        private bool isConnected = false;

        public RelayCommand StartBtnClickCmd { get; set; }
        public RelayCommand StopBtnClickCmd { get; set; }
        public RelayCommand DropClientBtnClickCmd { get; set; }
        public RelayCommand SaveToLogBtnClickCmd { get; set; }
        public ObservableCollection<string> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }

        public string SelectedUser { get; set; }

        public int NoOfReceivedMessages
        {
            get
            {
                return Messages.Count;
            }
        }

        public MainViewModel()
        {
            Messages = new ObservableCollection<string>();
            Users = new ObservableCollection<string>();

            //set command for start button
            StartBtnClickCmd = new RelayCommand(
                () =>
                {
                    server = new Server(ip, port, UpdateGuiWithNewMessage);
                    server.StartAccepting();
                    isConnected = true;
                },
                () => { return !isConnected; });

            //set command for stop button
            StopBtnClickCmd = new RelayCommand(
                //action for execute
                () =>
                {
                    server.StopAccepting();
                    isConnected = false;
                },
                //can execute
                () => { return isConnected; });

            //init Command for Drop button with CanExecute statement
            DropClientBtnClickCmd = new RelayCommand(() =>
            {
                server.DisconnectSpecificClient(SelectedUser);
                Users.Remove(SelectedUser); // remove from GUI listbox
            },
                () => { return (SelectedUser != null); });
        }

        public void UpdateGuiWithNewMessage(string message)
        {
            //switch thread to GUI thread to write to GUI
            App.Current.Dispatcher.Invoke(() =>
            {
                string name = message.Split(':')[0];
                if (!Users.Contains(name))
                {//not in list => add it
                    Users.Add(name);
                }
                //write message
                Messages.Add(message);
                //do this to inform the GUI about the update of the received message counter!
                RaisePropertyChanged("NoOfReceivedMessages");
            });
        }
    }
}