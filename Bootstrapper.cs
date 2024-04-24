using Autofac;
using Caliburn.Micro;
using Equipment__Management.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Equipment__Management
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer container;
        public Bootstrapper()
        {
            Initialize();
        }

        protected async override void OnStartup(object sender, StartupEventArgs e)
        {
            LogManager.GetLog = type => new DebugLog(type);
            await DisplayRootViewForAsync<ShellViewModel>();            
        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();
            container = new SimpleContainer();

            //    // Register your ViewModels and other classes with Autofac
            //builder.RegisterType<EMDBContext>().AsSelf().SingleInstance();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();            
            container.PerRequest<LoginViewModel>();
            container.PerRequest<ShellViewModel>();

          
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}
