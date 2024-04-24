using Caliburn.Micro;
using Equipment__Management.Commands;
using Equipment__Management.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Equipment__Management.ViewModels
{
    internal class EquipmentViewModel : Screen
    {

		private BindableCollection<ComboboxItemSource> _users;
		private ComboboxItemSource _selectedUser;
		private BindableCollection<Equipment> _dataGrid_EquipmentList;
        public ICommand EditCommand { get; }


        public EquipmentViewModel()
        {
			EditCommand = new RelayCommand(EditItem);
			Users = new BindableCollection<ComboboxItemSource>(GetAllUsers());

			Conditions = new BindableCollection<ComboboxItemSource>(AllCondition());
            			
            //LoadEquipmentsToGrid();

			EnableFields(false);
        }

        List<ComboboxItemSource> GetAllUsers()
        {
            using (EMDBContext context = new EMDBContext())
            {
                List<ComboboxItemSource> list = new List<ComboboxItemSource>();
                var users = context.User.ToList();
                foreach (var user in users)
                {
                    list.Add(new ComboboxItemSource() { Object = user, Description = user.FullName, Value = user.FullName });
                }
                list.Add(new ComboboxItemSource() { Object = null, Description = "All", Value="" });
				list = list.OrderBy(o=>o.Description).ToList();
                return list;
            }
        }

        void LoadEquipmentsToGrid()
		{
			int userId = 0;

            if (_selectedUser is not null) {
				if(_selectedUser.Object is not null)
					userId = ((User)_selectedUser.Object).User_Id;
            }
			
			using(EMDBContext context = new EMDBContext())
			{
				DataGrid_EquipmentList = new BindableCollection<Equipment>(
					context.Equipment.Where(x=>x.User_Id == (userId == 0 ? x.User_Id : userId)).Join(context.User, equipment => equipment.User_Id, user => user.User_Id, 
					(equipment, user) => new Equipment { Equipment_Id = equipment.User_Id, Serial_Number = equipment.Serial_Number, Description = equipment.Description, Condition = equipment.Condition, User = user }).ToList());
            }			
		}			

		List<ComboboxItemSource> AllCondition()
		{
			List<ComboboxItemSource> list = new List<ComboboxItemSource>();
			list.Add(new ComboboxItemSource() { Object = new ComboboxItemSource() { Description = "Good" }, Description = "Good" });
            list.Add(new ComboboxItemSource() { Object = new ComboboxItemSource() { Description = "Bad" }, Description = "Bad" });

			return list;
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


		public BindableCollection<ComboboxItemSource> Users
		{
			get
			{
				return _users;
			}
			set
			{
                _users = value;				
				NotifyOfPropertyChange(() => Users);
			}
		}		

		public ComboboxItemSource SelectedUser
		{
			get { return _selectedUser; }
			set { 
				_selectedUser = value;
				NewEquipment(UserName);

                NotifyOfPropertyChange(() => SelectedUser);

				bool isEnable = false;

                if (_selectedUser is not null)
				{
                    if (!string.IsNullOrWhiteSpace(_selectedUser.Value))
                    {
						isEnable = true;                        
                    }

                    LoadEquipmentsToGrid();
                }
											                				
				EnableFields(isEnable);
            }
		}		

		public BindableCollection<Equipment> DataGrid_EquipmentList
        {
			get { return _dataGrid_EquipmentList; }
			set { 
				_dataGrid_EquipmentList = value;
                NotifyOfPropertyChange(() => DataGrid_EquipmentList);
            }
		}

		private int _equipmentId;

		public int EquipmentId
		{
			get { return _equipmentId; }
			set { 
				_equipmentId = value; 
				NotifyOfPropertyChange(() => EquipmentId);
			}
		}

		private string _serialNumber;

		public string SerialNumber
		{
			get { return _serialNumber; }
			set { 
				_serialNumber = value; 
				NotifyOfPropertyChange(() => SerialNumber);
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

		private BindableCollection<ComboboxItemSource> _conditions;

		public BindableCollection<ComboboxItemSource> Conditions
		{
			get { 
				return _conditions; 
			}
			set { 
				_conditions = value; 
				NotifyOfPropertyChange(() => Conditions);
			}
		}

		private string _condition;

		public string Condition
		{
			get { return _condition; }
			set { 
				_condition = value;
				NotifyOfPropertyChange(() => Condition);
			}
		}


		private ComboboxItemSource _selectedCondition;

		public ComboboxItemSource SelectedCondition
		{
			get { return _selectedCondition; }
			set { 
				_selectedCondition = value;				
				NotifyOfPropertyChange(()=> SelectedCondition);
            }
		}

		private Equipment _selectedEquipment;

		public Equipment SelectedEquipment
		{
			get { return _selectedEquipment; }
			set { 
				_selectedEquipment = value; 
				NotifyOfPropertyChange(nameof(SelectedEquipment));
			}
		}

		private bool _isSerialNumberEnable;

		public bool IsSerialNumberEnable
        {
			get { return _isSerialNumberEnable; }
			set { 
				_isSerialNumberEnable = value; 
				NotifyOfPropertyChange(()=> IsSerialNumberEnable);
			}
		}

		private bool _isDescriptionEnable;

		public bool IsDescriptionEnable
		{
			get { return _isDescriptionEnable; }
			set { 
				_isDescriptionEnable = value; 
				NotifyOfPropertyChange(()=> IsDescriptionEnable);
			}
		}

		private bool _isConditionEnable;

		public bool IsConditionEnable
		{
			get { return _isConditionEnable; }
			set { 
				_isConditionEnable = value; 
				NotifyOfPropertyChange(()=>IsConditionEnable);
			}
		}

		public bool CanNewEquipment(string userName)
		{
			return !string.IsNullOrWhiteSpace(userName);
		}


        public void NewEquipment(string userName)
        {
			EquipmentId = 0;
			SerialNumber = string.Empty;
			Description = string.Empty;
			SelectedCondition = null;

			UserName = string.Empty;
			Condition = string.Empty;

			EnableFields(true);
        }

		private void EnableFields(bool enable)
		{
			IsSerialNumberEnable = enable;
			IsDescriptionEnable = enable;
			IsConditionEnable = enable;
		}



        public bool CanSave(string userName, string serialNumber, string description, string condition)
		{
			return !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(serialNumber) && !string.IsNullOrWhiteSpace(description) && !string.IsNullOrWhiteSpace(condition);
		}

        public void Save(string userName, string serialNumber, string description, string condition)
        {
			using(EMDBContext context = new EMDBContext())
			{
				if(_equipmentId > 0)
				{
					var equipment = context.Equipment.Find(_equipmentId);

					context.Equipment.Update(equipment);
				}
				else
				{					
					int userId = ((User)SelectedUser.Object).User_Id;
					Equipment equipment = new Equipment() { User_Id = userId, Serial_Number = _serialNumber, Description = _description, Condition = _selectedCondition.Description };
					context.Equipment.Add(equipment);
				}

				if(context.SaveChanges() > 0)
				{
                    if (_equipmentId>0) MessageBox.Show("Record successfully updated", "Save");
                    else MessageBox.Show("Record successfully saved", "Save");

					NewEquipment(UserName);
                }
			}            

			LoadEquipmentsToGrid();
        }

        private void EditItem(object parameter)
        {
            Equipment item = parameter as Equipment;
            if (item != null)
            {
				EquipmentId = item.Equipment_Id;
                SelectedEquipment = item;
                SerialNumber = item.Serial_Number;
                Description = item.Description;
                SelectedCondition = new ComboboxItemSource() { Description = item.Condition };
                // Perform actions to save the data
                // For example:
                // Update database, save changes, etc.                
            }
        }


    }
}
