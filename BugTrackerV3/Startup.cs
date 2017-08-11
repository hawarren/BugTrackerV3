using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BugTrackerV3.Startup))]
namespace BugTrackerV3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
