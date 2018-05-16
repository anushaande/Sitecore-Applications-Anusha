using System;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.ProductMarketing.Rules;

namespace Sitecore.ProductMarketing.Rules
{
    public class WhenRole<T> : WhenCondition<T> where T : RuleContext
    {
        public string Role { get; set; }
        protected override bool Execute(T ruleContext)
        {
            if (!string.IsNullOrEmpty(this.Role))
            {
                
                return Sitecore.Context.User.IsInRole(this.Role);
            }
            return false;
        }
    }
}
