using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestAssignment.Startup))]
namespace TestAssignment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
