using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;

namespace CodingDojo4.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool isConnected = false;

        public string ChatName { get; set; }
        public string Message { get; set; }
        public ObservableCollection<string> RecievedMessages { get; set; }
        public RelayCommand ConnectBtnClickedCmd { get; set; }
        public RelayCommand SendBtnClickCmd { get; set; }


        public MainViewModel()
        {

        }
    }
}