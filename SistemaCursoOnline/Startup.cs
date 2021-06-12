using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistemaCursoOnline.Startup))]
namespace SistemaCursoOnline
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
