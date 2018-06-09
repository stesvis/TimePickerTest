using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimePickerTest.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private List<DateTime> _availableDates;
        public List<DateTime> AvailableDates
        {
            get { return _availableDates; }
            set { SetProperty(ref _availableDates, value); }
        }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
        }
    }
}