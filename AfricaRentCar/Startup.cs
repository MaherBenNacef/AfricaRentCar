using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AfricaRentCar.Startup))]
namespace AfricaRentCar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
