using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public  class Contains : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToSearch = StringToSearch.Get(activityHelper.CodeActivityContext);
            string searchFor = SearchFor.Get(activityHelper.CodeActivityContext);
            bool caseSensitive = CaseSensitive.Get(activityHelper.CodeActivityContext);

            if (!caseSensitive)
            {
                stringToSearch = stringToSearch.ToUpper();
                searchFor = searchFor.ToUpper();
            }

            bool containsString = stringToSearch.Contains(searchFor);

            ContainsString.Set(activityHelper.CodeActivityContext, containsString);
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

        [Output("Contains String")]
        public OutArgument<bool> ContainsString { get; set; }
    }
}