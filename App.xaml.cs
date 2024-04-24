using System.Data.Common;
using System.Data.SQLite;
using System.Windows;


namespace Equipment__Management
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnStartup(StartupEventArgs e)
        {
            //DatabaseFacade facade = new DatabaseFacade(new EMDBContext());
            //facade.EnsureCreated();

            // Manually register SQLite ADO.NET provider
            //DbProviderFactories.RegisterFactory("System.Data.SQLite", SQLiteFactory.Instance);

            base.OnStartup(e);
        }
    }

}
