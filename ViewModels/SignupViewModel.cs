using Caliburn.Micro;
using Equipment__Management.Commands;
using Equipment__Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Equipment__Management.ViewModels
{
    internal class SignupViewModel : Screen
    {
        private User _user;
        private string firstName;
        private string lastName;
        private BindableCollection<UserTypeModel> _userTypes = new BindableCollection<UserTypeModel>();
        private UserTypeModel _selectedUserType;

        public ICommand RegisterCommand { get; }

        public SignupViewModel()
        {
            _user = new User();
            UserTypes.Add(new UserTypeModel { Description = "Admin" });
            UserTypes.Add(new UserTypeModel { Description = "SuperAdmin" });

            RegisterCommand = new RelayCommand((param) => Register(param));
        }

        private void Register(object param)
        {
            using (EMDBContext context = new EMDBContext())
            {
                StringBuilder err = new StringBuilder();
                if (string.IsNullOrEmpty(FirstName))
                {
                    err.AppendLine("First Name is required.");
                }
                if (string.IsNullOrEmpty(SelectedUserType.Description))
                {
                    err.AppendLine("User Type is required.");
                }
                if (string.IsNullOrEmpty(UserName))
                {
                    err.AppendLine("User Name is required.");
                }
                if (string.IsNullOrEmpty(Password))
                {
                    err.AppendLine("Password is required.");
                }
                //if (string.IsNullOrEmpty(ConfirmPassword))
                //{
                //    err.AppendLine("Confirm Password is required.");
                //}

                if (err.Length > 0)
                {
                    MessageBox.Show(err.ToString(), "Issue Found!");
                    return;
                }

                if (!_user.Password.Trim().Equals(ConfirmPassword.Trim()))
                {
                    err.AppendLine("Password not match.");
                }

                if (err.Length > 0)
                {
                    MessageBox.Show(err.ToString(), "Issue Found!");
                    return;
                }

                bool userNameExists = context.User.Any(u => u.User_Name == _user.User_Name.Trim());

                if (userNameExists)
                {
                    MessageBox.Show("Username already exist");
                    return;
                }

                bool userInfoExists = context.User.Any(u => u.First_Name == _user.First_Name.Trim() && u.Last_Name == _user.Last_Name.Trim());
                if (userInfoExists)
                {
                    MessageBox.Show("User already exists!");
                    return;
                }
                _user.User_Type

                context.Add(_user);
                if(context.SaveChanges() > 0)
                {
                    MessageBox.Show("User successfully updated", "Save");
                }

                _user = new User();
            }
        }

        public string UserType
        {
            get => _user.User_Type;
            set
            {
                _user.User_Type = value;
                NotifyOfPropertyChange(() => UserType);
            }
        }
        public string FirstName { get => _user.First_Name;
            set { 
                _user.First_Name = value; 
                NotifyOfPropertyChange(()=> FirstName);
            }
        }
       
        public string LastName
        {
            get => _user.Last_Name;
            set
            {
                _user.Last_Name = value;
                NotifyOfPropertyChange(()=> LastName);
            }
        }

        public string EmailAddress
        {
            get { return _user.Email_Address; }
            set {
                _user.Email_Address = value; 
                NotifyOfPropertyChange(() => EmailAddress);
            }
        }

        public string UserName
        {
            get { return _user.User_Name; }
            set {
                _user.User_Name = value; 
                NotifyOfPropertyChange(()=> UserName);
            }
        }

        public string Password
        {
            get { return _user.Password; }
            set {
                _user.Password = value; 
                NotifyOfPropertyChange(()=> Password);
            }
        }

        public string ConfirmPassword
        {
            get;
            set;
        }

        

        public BindableCollection<UserTypeModel> UserTypes
        {
            get { return _userTypes; }
            set { _userTypes = value; }
        }

        

        public UserTypeModel SelectedUserType
        {
            get { return _selectedUserType; }
            set { 
                _selectedUserType = value; 
                NotifyOfPropertyChange(()=> SelectedUserType);
            }
        }


    }
}

