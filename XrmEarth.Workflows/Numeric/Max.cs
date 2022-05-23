using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Numeric
{
    public class Max : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            decimal number1 = Number1.Get(activityHelper.CodeActivityContext);
            decimal number2 = Number2.Get(activityHelper.CodeActivityContext);

            decimal maxValue = number1 >= number2 ? number1 : number2;

            MaxValue.Set(activityHelper.CodeActivityContext, maxValue);
        }

        [RequiredArgument]
        [Input("Number 1")]
        public InArgument<decimal> Number1 { get; set; }

        [RequiredArgument]
        [Input("Number 2")]
        public InArgument<decimal> Number2 { get; set; }

        [Output("Max Value")]
        public OutArgument<decimal> MaxValue { get; set; }
    }
}