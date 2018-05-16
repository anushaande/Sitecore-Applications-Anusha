using Sitecore;
using Sitecore.Shell.Framework;
using Sitecore.Shell.Framework.Commands;
using System;

namespace HealthIS.Apps.MVC.UserActivityLog
{
    public class UserLog : Command
    {
        public override void Execute(CommandContext context)
        {
            Windows.RunApplication("User Log");
        }
        public override CommandState QueryState(CommandContext context)
        {
            if (Context.Database.GetItem("/sitecore/content/Applications/User Log") == null)
                return CommandState.Hidden;
            return base.QueryState(context);
        }
    }
}
