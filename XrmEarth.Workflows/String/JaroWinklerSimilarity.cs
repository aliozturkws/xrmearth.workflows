using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class JaroWinklerSimilarity : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var value1 = Value1.Get(activityHelper.CodeActivityContext);
            var value2 = Value2.Get(activityHelper.CodeActivityContext);

            Result.Set(activityHelper.CodeActivityContext, JaroWinklerHelper.Similarity(value1, value2));
        }

        [Input("Value 1")]
        public InArgument<string> Value1 { get; set; }

        [Input("Value 2")]
        public InArgument<string> Value2 { get; set; }

        [Output("Result")]
        public OutArgument<double> Result { get; set; }
    }
}
