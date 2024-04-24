using Caliburn.Micro;
using Equipment__Management.Commands;
using Equipment__Management.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Input.Manipulations;
using System.Windows.Media.Media3D;

namespace Equipment__Management.ViewModels
{
    public class SiteEquipmentMaintenanceViewModel : Screen
    {
        public ICommand EditCommand { get; private set; }
        public SiteEquipmentMaintenanceViewModel()
        {
            EditCommand = new RelayCommand(EditItem);
            LoadSiteToCombo();
        }

        #region property
        private BindableCollection<Registered_Equipment> _dataGrid_RegisteredEquipmentList;

        public BindableCollection<Registered_Equipment> DataGrid_RegisteredEquipmentList
        {
            get { return _dataGrid_RegisteredEquipmentList; }
            set { 
                _dataGrid_RegisteredEquipmentList = value; 
                NotifyOfPropertyChange(() => DataGrid_RegisteredEquipmentList);
            }
        }


        private BindableCollection<ComboboxItemSource> _sites = new BindableCollection<ComboboxItemSource>();
        public BindableCollection<ComboboxItemSource> Sites
        {
            get { return _sites; }
            set { 
                _sites = value; 
                NotifyOfPropertyChange(() => Sites);
            }
        }

        private BindableCollection<ComboboxItemSource> _equipments;

        public BindableCollection<ComboboxItemSource> Equipments
        {
            get { return _equipments; }
            set { 
                _equipments = value; 
                NotifyOfPropertyChange(() => Equipments);
            }
        }


        private ComboboxItemSource _selectedSite;
        public ComboboxItemSource SelectedSite
        {
            get { return _selectedSite; }
            set {
                Site = string.Empty;
                _selectedSite = value;
                NotifyOfPropertyChange(() => SelectedSite);
                Equipments = new BindableCollection<ComboboxItemSource>();
                if(_selectedSite is not null)
                {
                    if (_selectedSite.Value != string.Empty)
                    {                        
                        Site = _selectedSite.Value;
                        LoadEquipmentToCombo();                        
                    }

                    LoadRegistedToGrid();
                }                
                ClearFields();
                
            }
        }

        private ComboboxItemSource _selectedEquipment;

        public ComboboxItemSource SelectedEquipment
        {
            get { return _selectedEquipment; }
            set { 
                _selectedEquipment = value;

                NotifyOfPropertyChange(() => SelectedEquipment);
                Equipment = string.Empty;
                if(_selectedEquipment is not null)
                {
                    var item = ((Equipment)_selectedEquipment.Object);
                    SerialNumber = item.Serial_Number;
                    Description = item.Description;
                    Condition = item.Condition;

                    Equipment = _selectedEquipment.Value;
                }
                
                
            }
        }

        private int _id;                
        public int Id
        {
            get { return _id; }
            set { 
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }


        private string _site;
        public string Site
        {
            get { return _site; }
            set { 
                _site = value;
                NotifyOfPropertyChange(() => Site);
            }
        }

        private string _equipment;
        public string Equipment
        {
            get { return _equipment; }
            set { 
                _equipment = value;
                NotifyOfPropertyChange(() => Equipment);
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

        private string _condition;

        public string Condition
        {
            get { return _condition; }
            set { 
                _condition = value; 
                NotifyOfPropertyChange(() => Condition);
            }
        }

        private bool _isEquipmentEnable;

        public bool IsEquipmentEnable
        {
            get { return _isEquipmentEnable; }
            set { 
                _isEquipmentEnable = value; 
                NotifyOfPropertyChange(() => IsEquipmentEnable);
            }
        }


        #endregion

        #region method

        void EnableFields(bool enable)
        {

        }

        void ClearFields()
        {
            SelectedEquipment = null;
            SerialNumber = string.Empty;
            Description = string.Empty;
            Condition = string.Empty;
        }
        void LoadSiteToCombo()
        {            
            Sites.Add(new ComboboxItemSource() { Object = null, Description = "All", Value = "" });
            using (EMDBContext context = new EMDBContext())
            {
                var items = context.Site.ToList();
                foreach (var item in items)
                {
                    Sites.Add(new ComboboxItemSource() { Object = item, Description = item.Description, Value = item.Description });
                }                
            }
        }

        void LoadEquipmentToCombo()
        {
            Equipments = new BindableCollection<ComboboxItemSource>();
            using (EMDBContext context = new EMDBContext())
            {
                var items = context.Equipment.Where(x => !context.Registered_Equipment.Any(x2 => x2.Equipment_Id == x.Equipment_Id)).ToList();
                foreach (var item in items)
                {
                    Equipments.Add(new ComboboxItemSource() { Object = item, Description = item.Description, Value = item.Description });
                }
            }
        }

        void LoadRegistedToGrid()
        {
            int siteId = 0;

            if (_selectedSite is not null)
            {
                if (_selectedSite.Object is not null)
                    siteId = ((Models.Site)_selectedSite.Object).Site_Id;
            }

            using (EMDBContext context = new EMDBContext())
            {
                //var recData = context.Registered_Equipment.Where(x => x.Site_Id == (siteId == 0 ? x.Site_Id : siteId)).Join(context.Equipment, regEquip => regEquip.Equipment_Id, equip => equip.Equipment_Id,
                //       (regEquip, equip) => new Registered_Equipment { Id = regEquip.Id, Site_Id = regEquip.Site_Id, Equipment_Id = regEquip.Equipment_Id, Equipment = equip }).ToList();
                var recData = context.Registered_Equipment.Where(x => x.Site_Id == (siteId == 0 ? x.Site_Id : siteId))
                    .Join(context.Equipment, regEquip => regEquip.Equipment_Id, equip => equip.Equipment_Id,
                       (regEquip, equip) => new { table1 = regEquip, table2 = equip })
                    .Join(context.Site, rEquip=>rEquip.table1.Site_Id, site=>site.Site_Id, (t1, t2) => new Registered_Equipment() { Id = t1.table1.Id, Site_Id = t2.Site_Id, Site = t2, Equipment_Id = t1.table1.Equipment_Id, Equipment = t1.table2 }).ToList();
                DataGrid_RegisteredEquipmentList = new BindableCollection<Registered_Equipment>(recData);
            }
        }

        public bool CanNewRegSite(string site)
        {
            return !string.IsNullOrWhiteSpace(site);
        }
        public void NewRegSite(string site)
        {

            ClearFields();
            EnableFields(true);
        }

        public bool CanSave(string site, string equipment)
        {
            return !string.IsNullOrWhiteSpace(site) && !string.IsNullOrWhiteSpace(_equipment);
        }
        public async void Save(string site, string equipment)
        {
            StringBuilder err = new StringBuilder();
            if (_selectedSite == null)
            {
                err.AppendLine("Site is required.");
            }
            if (_selectedEquipment == null)
            {
                err.AppendLine("Equipment is required.");
            }
            

            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Issue found.");
            }
            try
            {
                using (EMDBContext context = new EMDBContext())
                {
                    Registered_Equipment regEquipment = new Registered_Equipment();
                    int siteId = ((Models.Site)_selectedSite.Object).Site_Id;
                    int equipmentId = ((Equipment)_selectedEquipment.Object).Equipment_Id;

                    if (Id > 0)
                    {
                        regEquipment = await context.Registered_Equipment.FindAsync(Id);                        
                                                
                        var itemExists = context.Registered_Equipment.Any(s => s.Site_Id == siteId && s.Equipment_Id == equipmentId && s.Id != s.Id);
                        if (itemExists)
                        {
                            MessageBox.Show("Equipment already assigned to this site", "Issue found.");
                            return;
                        }

                        regEquipment.Site_Id = siteId;
                        regEquipment.Equipment_Id = equipmentId;
                        context.Registered_Equipment.Update(regEquipment);
                    }
                    else
                    {
                        var itemExists = context.Registered_Equipment.Any(s => s.Site_Id == siteId && s.Equipment_Id == equipmentId);

                        if (itemExists)
                        {
                            MessageBox.Show("Equipment already assigned to this site", "Issue found.");
                            return;
                        }

                        regEquipment = new Registered_Equipment() { Site_Id = siteId, Equipment_Id = equipmentId };
                        context.Registered_Equipment.Add(regEquipment);
                    }


                    if (context.SaveChanges() > 0)
                    {
                        if (Id > 0) MessageBox.Show("Record successfully updated", "Save");
                        else MessageBox.Show("Record successfully saved", "Save");

                        ClearFields();
                    }

                    LoadRegistedToGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private async void EditItem(object parameter)
        {
            Registered_Equipment item = parameter as Registered_Equipment;
            if (item != null)
            {
                using(EMDBContext context = new EMDBContext())
                {
                    var recData = context.Registered_Equipment.Where(x => x.Id == item.Id)
                    .Join(context.Equipment, regEquip => regEquip.Equipment_Id, equip => equip.Equipment_Id,
                       (regEquip, equip) => new { table1 = regEquip, table2 = equip })
                    .Join(context.Site, rEquip => rEquip.table1.Site_Id, site => site.Site_Id, (t1, t2) => new Registered_Equipment() { Id = t1.table1.Id, Site_Id = t2.Site_Id, Site = t2, Equipment_Id = t1.table1.Equipment_Id, Equipment = t1.table2 }).FirstOrDefault();

                    Id = recData.Id;
                    SelectedEquipment = new ComboboxItemSource() { Object = recData.Equipment, Description = recData.Equipment.Description, Value = recData.Equipment.Description };
                    SerialNumber = recData.Equipment.Serial_Number;
                    Description = recData.Equipment.Description;
                    Condition = recData.Equipment.Condition;
                    
                    EnableFields(true);                    
                }                
            }
        }
        #endregion
    }
}
