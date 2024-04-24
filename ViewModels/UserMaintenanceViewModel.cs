using Caliburn.Micro;
using Equipment__Management.Commands;
using Equipment__Management.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Equipment__Management.ViewModels
{
    internal class UserMaintenanceViewModel : Screen
    {
        public ICommand EditCommand { get; private set; }

        public UserMaintenanceViewModel()
        {
            EditCommand = new RelayCommand(EditItem);
            
            UserTypes.Add(new ComboboxItemSource() { Object = 0, Description = "All", Value="" });
            UserTypes.Add(new ComboboxItemSource() { Object = 1, Description = "Admin", Value="Admin" });
            UserTypes.Add(new ComboboxItemSource() { Object = 2, Description = "SuperAdmin", Value = "SuperAdmin" });
        }
        private ObservableCollection<User> _dataGrid_UserList;

		public ObservableCollection<User> DataGrid_UserList
        {
			get { return _dataGrid_UserList; }
			set { 
				_dataGrid_UserList = value; 
				NotifyOfPropertyChange(() => DataGrid_UserList);
			}
		}

        private int _userId;

        public int UserId
        {
            get { return _userId; }
            set { 
                _userId = value; 
                NotifyOfPropertyChange(() => UserId);
            }
        }


        private string _userType;

        public string UserType
        {
            get { return _userType; }
            set
            {
                _userType = value;
                NotifyOfPropertyChange(() => UserType);
            }
        }

        
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { 
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }


        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { 
                _firstName = value; 
                NotifyOfPropertyChange(() => FirstName);
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                NotifyOfPropertyChange(() => LastName);
            }
        }


        private string _emailAddress;

        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                NotifyOfPropertyChange(() => EmailAddress);
            }
        }

        private ComboboxItemSource _selectedUserType;

        public ComboboxItemSource SelectedUserType
        {
            get { return _selectedUserType; }
            set
            {
                _selectedUserType = value;                
                bool isEnable = false;

                if (_selectedUserType is not null)
                {
                    UserType = _selectedUserType.Value;
                    if (!string.IsNullOrWhiteSpace(_selectedUserType.Value))
                    {
                        isEnable = true;
                    }

                    LoadUsersToGrid();
                }

                ClearFields();
                EnableFields(isEnable);
                NotifyOfPropertyChange(() => SelectedUserType);
            }
        }

        private ObservableCollection<ComboboxItemSource> _userTypes = new ObservableCollection<ComboboxItemSource>();

        public ObservableCollection<ComboboxItemSource> UserTypes
        {
            get { return _userTypes; }
            set { 
                _userTypes = value; 
                NotifyOfPropertyChange(()=> UserTypes);
            }
        }




        void LoadUsersToGrid()
		{
            string userType = string.Empty;

            if (_selectedUserType is not null)
            {
                if (_selectedUserType.Object is not null)
                    userType = _selectedUserType.Value;
            }

            using (EMDBContext context = new EMDBContext())
            {
                DataGrid_UserList = new ObservableCollection<User>(
                    context.User.Where(x=>x.User_Type == (userType == "" ? x.User_Type : userType)).ToList());
            }

            NotifyOfPropertyChange(() => DataGrid_UserList);
        }

        private bool _isUserNameEnable;

        public bool IsUserNameEnable
        {
            get { return _isUserNameEnable; }
            set { 
                _isUserNameEnable = value; 
                NotifyOfPropertyChange(()=> IsUserNameEnable);
            }
        }

        private bool _isFirstNameEnable;
        public bool IsFirstNameEnable
        {
            get { return _isFirstNameEnable; }
            set { 
                _isFirstNameEnable = value; 
                NotifyOfPropertyChange(()=> IsFirstNameEnable);
            }
        }

        private bool _isLastNameEnable;
        public bool IsLasttNameEnable
        {
            get { return _isLastNameEnable; }
            set { 
                _isLastNameEnable = value;
                NotifyOfPropertyChange(() => IsLasttNameEnable);
            }
        }

        private bool _isEmailAddressEnable;

        public bool IsEmailAddressEnable
        {
            get { return _isEmailAddressEnable; }
            set { 
                _isEmailAddressEnable = value; 
                NotifyOfPropertyChange(() => IsEmailAddressEnable);
            }
        }



        #region Event
        public bool CanNewUser(string userType)
        {
            return !string.IsNullOrWhiteSpace(userType);
        }
        public void NewUser(string userType)
        {

            ClearFields();
            EnableFields(true);
        }

        void ClearFields()
        {
            UserId = 0;
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            EmailAddress = string.Empty;
        }

        private void EnableFields(bool enable)
        {
            IsUserNameEnable = enable;
            IsLasttNameEnable = enable;
            IsFirstNameEnable = enable;
            IsEmailAddressEnable = enable;
        }

        public bool CanSave(string userName, string firstName, string lastName) { 
            
            return SelectedUserType is not null && !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(firstName) ;
        }
        public async void Save(string userName, string firstName, string lastName)
        {
            StringBuilder err = new StringBuilder();
            if (_selectedUserType == null)
            {
                err.AppendLine("User Type is required.");
            }
            if (_userName.Trim().Length <=0)
            {
                err.AppendLine("Username is required");
            }
            if (_firstName.Trim().Length <=0)
            {
                err.AppendLine("First Name is required");
            }

            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Issue found.");
            }
            try
            {
                using (EMDBContext context = new EMDBContext())
                {
                    User user = new User();
                    if (UserId > 0)
                    {
                        user = await context.User.FindAsync(UserId);                                                

                        var userExists = context.User.Any(s => s.First_Name == _firstName && s.Last_Name == _lastName && s.User_Id != user.User_Id);
                        if (userExists)
                        {
                            MessageBox.Show("User information already exists", "Issue found.");
                            return;
                        }

                        var userEmailExists = context.User.Any(s => s.Email_Address == _emailAddress && s.User_Id != user.User_Id);
                        if (userEmailExists)
                        {
                            MessageBox.Show("Email Address already exists", "Issue found.");
                            return;
                        }

                        user.User_Type = _selectedUserType.Description;
                        user.First_Name = _firstName;
                        user.Last_Name = _lastName;
                        user.Email_Address = _emailAddress;
                        context.User.Update(user);
                    }
                    else
                    {
                        var userNameExists = context.User.Any(s => s.User_Name == _userName);
                        if (userNameExists)
                        {
                            MessageBox.Show("User Name already exists", "Issue found.");
                            return;
                        }

                        var userExists = context.User.Any(s => s.First_Name ==_firstName && s.Last_Name == _lastName);
                        if (userExists)
                        {
                            MessageBox.Show("User already assigned to this site", "Issue found.");
                            return;
                        }

                        var userEmailExists = context.User.Any(s => s.Email_Address == _emailAddress);
                        if (userEmailExists)
                        {
                            MessageBox.Show("Email Address already exists", "Issue found.");
                            return;
                        }

                        user = new User() { First_Name = _firstName, Last_Name = _lastName, Email_Address = _emailAddress, User_Name = _userName, User_Type = _selectedUserType.Description, Password = "12345"};
                        context.User.Add(user);
                    }


                    if (context.SaveChanges() > 0)
                    {
                        if (UserId>0) MessageBox.Show("Record successfully updated", "Save");
                        else MessageBox.Show("Record successfully saved", "Save");

                        ClearFields();                       
                        EnableFields(true);
                    }

                    LoadUsersToGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void EditItem(object parameter)
        {
            User item = parameter as User;
            if (item != null)
            {
                UserId = item.User_Id;                
                UserType = item.User_Type;
                UserName = item.User_Name;
                FirstName = item.First_Name;
                LastName = item.Last_Name;
                EmailAddress = item.Email_Address;

                EnableFields(true);
                IsUserNameEnable = false;
            }
        }
        #endregion
    }
}
