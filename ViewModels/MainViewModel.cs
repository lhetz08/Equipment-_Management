using Caliburn.Micro;
using Equipment__Management.Commands;
using Equipment__Management.Models;
using Equipment__Management.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Equipment__Management.ViewModels
{
    public class MainViewModel : Screen
    {
        private UserViewModel _user;
        //public MainViewModel(UserViewModel user)
        //{
        //    _user = user;
        //}
        //private readonly User _user;
        //public MainViewModel(User user)
        //{
        //    _user = user;
        //    string userType = _user.User_Type.ToUpper();

        //    if (_user.User_Type.ToUpper().Contains("SUPER") && userType.ToUpper().Contains("ADMIN")) MenuItems.Add(new MenuItemViewModel { Header = "User", Command=new RelayCommand(UserMenu) });

        //    BindableCollection<MenuItemViewModel> menus = new BindableCollection<MenuItemViewModel>() {
        //        new MenuItemViewModel{ Header="Site", Command=new RelayCommand(SiteMenu) },
        //        new MenuItemViewModel{ Header="Equipment", Command=new RelayCommand(EquipmentMenu) },
        //    };
        //}

        //private BindableCollection<MenuItemViewModel> _menuItems;
        //public BindableCollection<MenuItemViewModel> MenuItems
        //{
        //    get { return _menuItems; }
        //    set
        //    {
        //        _menuItems = value;
        //        NotifyOfPropertyChange(() => MenuItems);
        //    }
        //}

        //private void UserMenu(object param)
        //{
        //    LoginView loginView = new LoginView();
        //    loginView.Show();
        //}
        //private void SiteMenu(object param)
        //{

        //}
        //private void EquipmentMenu(object param) { }
        //private void LogoutMenu(object param) { }

        //public async Task HandleAsync(UserLoggedInEvent message, CancellationToken cancellationToken)
        //{
        //    // Retrieve user type from the user information
        //    string userType = message.User.User_Type;

        //    // Initialize menu items based on user type
        //    //InitializeMenuItems(userType);
        //    await activateitem(new MainViewModel(message.User));
        //}
    }
}
