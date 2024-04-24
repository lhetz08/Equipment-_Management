using Caliburn.Micro;
using Equipment__Management.Commands;
using Equipment__Management.Models;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Equipment__Management.ViewModels
{
    internal class SiteMaintenanceViewModel : Screen
    {        
        public ICommand EditCommand { get; private set; }

        public SiteMaintenanceViewModel() {
            
            BindableCollection<User> usersCollection = new BindableCollection<User>();
            LoadSitesToGrid();
            LoadUsersToCombo();

            EditCommand = new RelayCommand(EditItem);
        }

        private void EditItem(object parameter)
        {
            Site item = parameter as Site;
            if (item != null)
            {
                LoadSitesToGrid();
                SelectedUser = item.User;
                Description = item.Description;
                IsActive = item.Active;
                SiteId = item.Site_Id;
                // Perform actions to save the data
                // For example:
                // Update database, save changes, etc.
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        void LoadUsersToCombo()
        {
            SelectedUser = null;
            using (EMDBContext context = new EMDBContext())
            {
                Users = new ObservableCollection<User>(context.User.ToList());
            }
        }

        void LoadSitesToGrid()
        {
            using (EMDBContext context = new EMDBContext())
            {
                DataGrid_SiteList = new BindableCollection<Site>(
                    context.Site.Join(context.User, site => site.User_Id, user => user.User_Id,
                    (site, user) => new Site { Site_Id = site.Site_Id, Description = site.Description, Active = site.Active, User = user }).ToList());
            }

            NotifyOfPropertyChange(() => Users);
        }

        private int _siteId = 0;

        public int SiteId
        {
            get { return _siteId; }
            set { 
                _siteId = value; 
                NotifyOfPropertyChange(() => SiteId);
            }
        }



        private string _description;

        public string Description
        {
            get { return _description; }
            set {
                _description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }

        private bool _isActive = true;

        public bool IsActive
        {
            get { return _isActive; }
            set {
                _isActive = value;
                NotifyOfPropertyChange(() => IsActive);
            }
        }



        private ObservableCollection<User> _users = new ObservableCollection<User>();
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        private User _selectedUser;

        public User SelectedUser
        {
            get { return _selectedUser; }
            set {
                _selectedUser = value;
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }


        private ObservableCollection<Site> _dataGrid_SiteList = new ObservableCollection<Site>();

        public ObservableCollection<Site> DataGrid_SiteList
        {
            get { return _dataGrid_SiteList; }
            set {
                _dataGrid_SiteList = value;
                NotifyOfPropertyChange(() => DataGrid_SiteList);
            }
        }

        void clickTets() { }        

        public bool CanSave(User selectedUser, string description)
        {
            return !(_selectedUser is null) && !string.IsNullOrWhiteSpace(_description);           
        }
        public async void Save(User selectedUser, string description)
        {
            StringBuilder err = new StringBuilder();
            if(_selectedUser == null)
            {
                err.AppendLine("User is required.");
            }
            if(_description.Trim().Length <=0)
            {
                err.AppendLine("Description is required");
            }

            if(err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Issue found.");
            }
            try
            {
                using (EMDBContext context = new EMDBContext())
                {
                    Site site = new Site();
                    if (SiteId > 0)
                    {
                        site = await context.Site.FindAsync(SiteId);
                        site.User_Id = _selectedUser.User_Id;
                        site.Description = _description;
                        site.Active = _isActive;

                        var userSiteExists = context.Site.Any(s => s.User.User_Id == _selectedUser.User_Id && s.Description == _description && s.Site_Id != site.Site_Id);
                        if (userSiteExists)
                        {
                            MessageBox.Show("User already assigned to this site", "Issue found.");
                            return;
                        }
                        context.Site.Update(site);
                    }
                    else
                    {
                        var userSiteExists = context.Site.Any(s => s.User.User_Id == _selectedUser.User_Id && s.Description == _description);

                        if (userSiteExists)
                        {
                            MessageBox.Show("User already assigned to this site", "Issue found.");
                            return;
                        }

                        site = new Site() { Site_Id = SiteId, User_Id = _selectedUser.User_Id, Description = _description, Active = _isActive };
                        context.Site.Add(site);
                    }
                                        

                    if(context.SaveChanges() > 0)
                    {
                        if(SiteId>0) MessageBox.Show("Record successfully updated", "Save");
                        else MessageBox.Show("Record successfully saved", "Save");

                        SiteId = 0;
                        SelectedUser = null;
                        Description = string.Empty;
                        IsActive = true;
                    }

                    LoadSitesToGrid();
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

    }
}
