using Caliburn.Micro;
using Equipment__Management.Models;
using Equipment__Management.Views;
using System.Windows;
using System.Windows.Controls;

namespace Equipment__Management.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private string _username;
        private string _password;
   
        public LoginViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => UserName);                
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);                
            }
        }

        public void PasswordChangedAction(PasswordBox source)
        {
            Password = source.Password;
        }

        #region commands
        public async void Register()
        {
            //SignupView view = new SignupView();
            //view.Show();
           await  _eventAggregator.PublishOnUIThreadAsync(new RegisterEvent());
        }

        public bool CanLogin(string userName, string password)
        {
            return !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password);
        }

        public async void Login(string userName, string password)
        {
            using (EMDBContext context = new EMDBContext())
            {
                bool userFound = context.User.Any(u => u.User_Name == UserName && u.Password == Password);

                if (userFound)
                {
                    MessageBox.Show("Logged in successfully!");

                    var _user = context.User.Where(x => x.User_Name == UserName && x.Password == Password).FirstOrDefault();

                    //// Example: passing "Hello from LoginViewModel" as the parameter
                    ////await IoC.Get<IEventAggregator>().PublishOnUIThreadAsync(new UserLoggedInEvent(_user));

                    ////var shellViewModel = IoC.Get<ShellViewModel>();
                    ////shellViewModel.ActivateAsync();

                    //////shellViewModel.ShowMainView();
                    //////shellViewModel.ShowMainView(new UserViewModel(_user);
                    //////ShellView view = new ShellView(this);
                    //////view.Show();
                    ////this.TryCloseAsync();

                    //// Publish an event when login is successful
                    ////_eventAggregator.PublishAsync(new LoginSuccessfulEvent()).ConfigureAwait(false);
                   //await this.TryCloseAsync();
                   //await  _eventAggregator.PublishAsync(new LoginSuccessfulEvent());
                   await _eventAggregator.PublishOnUIThreadAsync(new LoginSuccessfulEvent(_user.User_Type));

                    //var viewModel = IoC.Get<SiteMaintenanceViewModel>();
                    //await ActivateItemAsync(viewModel, new CancellationToken());
                }
                else
                {
                    MessageBox.Show("Log-in Failed!");
                }
            }
        }
        #endregion           
    }

    public class LoginSuccessfulEvent {
        private string _userType;

        public string UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        public LoginSuccessfulEvent(string UserType)
        {
            _userType = UserType;
        }
    }

    public class RegisterEvent
    {
        public RegisterEvent()
        {
            
        }
    }

    public class UserLoggedInEvent
    {
        public User User { get; }

        public UserLoggedInEvent(User user)
        {
            User = user;
        }
    }

    public static class EventAggregatorExtensions
    {
        public static async Task PublishAsync<TEvent>(this IEventAggregator eventAggregator, TEvent message)
        {
            
        }
    }
}
