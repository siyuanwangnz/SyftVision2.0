using System.Windows;
using System.Xml.Linq;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Public.Global;

namespace SyftVision.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            //Subscribe event message
            _eventAggregator.GetEvent<Public.Event.MessageEvent>().Subscribe((m) => TaskProcess = m / 100);
            //Set dialog title
            Title = $"Syft Vision V{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
            //Get user name
            try
            {
                XElement RootNode = XElement.Load($"./Config/User_Config.xml");

                if (RootNode.Value != "")
                {
                    UserName = RootNode.Value;
                    IsUserNameSaved = true;
                }
                else
                {
                    IsUserNameSaved = false;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR");
                IsUserNameSaved = false;
            }
        }
        private readonly IEventAggregator _eventAggregator;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private double _taskProcess;
        public double TaskProcess
        {
            get { return _taskProcess; }
            set { SetProperty(ref _taskProcess, value); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value);
                Global.UserName = _userName;
                IsUserNameSaved = false;
            }
        }

        private bool _isUserNameSaved;
        public bool IsUserNameSaved
        {
            get { return _isUserNameSaved; }
            set { SetProperty(ref _isUserNameSaved, value); }
        }

        public DelegateCommand UserNameSaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        //Save user name
                        XElement RootNode = XElement.Load($"./Config/User_Config.xml");
                        if (UserName != "")
                        {
                            RootNode.SetValue(UserName);
                            RootNode.Save($"./Config/User_Config.xml");
                            IsUserNameSaved = true;
                        }
                        //Close popup dialog
                        PopupBox.ClosePopupCommand.Execute(new object(), null);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR");
                        IsUserNameSaved = false;
                    }
                });
            }
        }
    }
}
