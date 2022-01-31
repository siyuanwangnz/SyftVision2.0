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
            MainTitle = $"SyftVision V{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
        }
        private readonly IEventAggregator _eventAggregator;

        private string _mainTitle;
        public string MainTitle
        {
            get { return _mainTitle; }
            set { SetProperty(ref _mainTitle, value); }
        }

        private double _taskProcess;
        public double TaskProcess
        {
            get { return _taskProcess; }
            set { SetProperty(ref _taskProcess, value); }
        }
    }
}
