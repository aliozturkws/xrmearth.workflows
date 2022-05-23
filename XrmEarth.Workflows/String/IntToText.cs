using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;
using XrmEarth.Core.NumberToText;

namespace XrmEarth.Workflows.String
{
    public class IntToText : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var intValue = IntValue.Get<int>(activityHelper.CodeActivityContext);
            var language = Language.Get<string>(activityHelper.CodeActivityContext);
            var result = intValue.ToText(language);

            Result.Set(activityHelper.CodeActivityContext, result);
        }

        [RequiredArgument]
        [Input("Int")]
        public InArgument<int> IntValue { get; set; }

        [RequiredArgument]
        [Input("Language")]
        public InArgument<string> Language { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }
    }
}
