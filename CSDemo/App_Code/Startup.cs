using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CSDemo.Startup))]
namespace CSDemo
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
