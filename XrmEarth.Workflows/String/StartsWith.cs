using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public  class StartsWith : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToSearch = StringToSearch.Get(activityHelper.CodeActivityContext);
            string searchFor = SearchFor.Get(activityHelper.CodeActivityContext);
            bool caseSensitive = CaseSensitive.Get(activityHelper.CodeActivityContext);

            bool startsWithString = stringToSearch.StartsWith(searchFor,
                (caseSensitive) ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            StartsWithString.Set(activityHelper.CodeActivityContext, startsWithString);
        }

        [RequiredArgument]
        [Input("String To Search")]
        public InArgument<string> StringToSearch { get; set; }

        [RequiredArgument]
        [Input("Search For")]
        public InArgument<string> SearchFor { get; set; }

        [RequiredArgument]
        [Input("Case Sensitive")]
        [Default("false")]
        public InArgument<bool> CaseSensitive { get; set; }

        [Output("Starts With String")]
        public OutArgument<bool> StartsWithString { get; set; }
    }
}
