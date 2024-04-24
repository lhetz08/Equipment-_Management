using Caliburn.Micro;
using Equipment__Management.Commands;
using Equipment__Management.Models;
using Equipment__Management.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Equipment__Management.ViewModels
{
    public class ShellViewModel : Conductor<object>.Collection.OneActive, IHandle<LoginSuccessfulEvent>, IHandle<RegisterEvent>
    {        
        private readonly IEventAggregator _eventAggregator;

        public ShellViewModel(IEventAggregator eventAggregator)
        {            
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);           
        }

        protected async override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            //await UserLogin();
            //var viewModel = IoC.Get<HomeViewModel>();
            ActivateItem(new HomeViewModel());

            UserType = "";
        }

        //private readonly IWindowManager _windowManager;
        //private MainViewModel _mainViewModel;
        private ObservableCollection<MenuItemViewModel> _menuItems = new ObservableCollection<MenuItemViewModel>();
        public ObservableCollection<MenuItemViewModel> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                NotifyOfPropertyChange(() => MenuItems);
            }
        }

        private string _userType;
        public string UserType
        {
            get { return _userType; }
            set { 
                _userType = value;

                _menuItems = new ObservableCollection<MenuItemViewModel>();

                MenuItems.Add(new MenuItemViewModel { Header = "Home", Command=new RelayCommand(HomeMenu) });
                if (!string.IsNullOrEmpty(_userType)) {
                    if (_userType.ToUpper().Contains("SUPER") && _userType.ToUpper().Contains("ADMIN")) 
                        MenuItems.Add(new MenuItemViewModel { Header = "User Maintenance", Command=new RelayCommand(UserMenu) });

                    //BindableCollection<MenuItemViewModel> menus = new BindableCollection<MenuItemViewModel>() {
                    //        new MenuItemViewModel{ Header="Site", Command=new RelayCommand(SiteMenu) },
                    //        new MenuItemViewModel{ Header="Equipment", Command=new RelayCommand(EquipmentMenu) },
                    //    };

                    MenuItems.Add(new MenuItemViewModel { Header = "Site Maintenance", Command=new RelayCommand(SiteMenu) });
                    MenuItems.Add(new MenuItemViewModel { Header = "Equipment Maintenance", Command=new RelayCommand(EquipmentMenu) });
                    MenuItems.Add(new MenuItemViewModel { Header = "Site Equipment", Command=new RelayCommand(SiteEquipmentMenu) });
                    MenuItems.Add(new MenuItemViewModel { Header = "Logout", Command=new RelayCommand(LogoutMenu) });
                }
                else
                {
                    MenuItems.Add(new MenuItemViewModel { Header = "Login", Command=new RelayCommand(LoginMenu) });
                }
                MenuItems.Add(new MenuItemViewModel { Header = "Close Application", Command=new RelayCommand(CloseMenu) });

                NotifyOfPropertyChange(() => UserType);
                NotifyOfPropertyChange(() => MenuItems);
            }
        }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { 
                _currentView = value; 
                NotifyOfPropertyChange(() => CurrentView);
            }
        }

        private void HomeMenu(object param)
        {
            ActivateItem(new HomeViewModel());
        }

        private void UserMenu(object param)
        {
            ActivateItem(new UserMaintenanceViewModel());
        }
        private void SiteMenu(object param)
        {            
            ActivateItem(new SiteMaintenanceViewModel());
        }
        private void EquipmentMenu(object param) 
        {
            ActivateItem(new EquipmentViewModel());
        }
        private void SiteEquipmentMenu(object param)
        {            
            ActivateItem(new SiteEquipmentMaintenanceViewModel());
        }
        private void LoginMenu(object param)
        {
            var viewModel = IoC.Get<LoginViewModel>();
            ActivateItem(viewModel);
            UserType = "";
        }
        private void LogoutMenu(object param) {

            if(MessageBox.Show("Continue to Logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var viewModel = IoC.Get<LoginViewModel>();
                ActivateItem(viewModel);
                UserType = "";
            }            
        }

        private async void CloseMenu(object param)
        {
            if (MessageBox.Show("Continue to Close Application?", "Exit Application", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await TryCloseAsync();
            }
            
        }

        public async Task HandleAsync(LoginSuccessfulEvent message, CancellationToken cancellationToken)
        {            
            UserType = message.UserType;            
            ActivateItem(new HomeViewModel());
        }

        public async void ActivateItem(object item)
        {
            if (item != null && item.Equals(ActiveItem))
            {
                OnActivationProcessed(item, true);
                return;
            }

            await ChangeActiveItemAsync(item, false);
        }

        public async Task HandleAsync(RegisterEvent message, CancellationToken cancellationToken)
        {
            ActivateItem(new SignupViewModel());
        }
    }

    public class UserViewModel : Screen
    {
        public User User { get; }

        public UserViewModel()
        {
            
        }
        public UserViewModel(User user)
        {
            User = user;
        }
    }
    

    public class MenuItemViewModel
    {
        public string Header { get; set; }
        public ICommand Command { get; set; }
    }

   
}
