using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Numeric
{
    public class Min : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            decimal number1 = Number1.Get(activityHelper.CodeActivityContext);
            decimal number2 = Number2.Get(activityHelper.CodeActivityContext);

            decimal minValue = number1 <= number2 ? number1 : number2;

            MinValue.Set(activityHelper.CodeActivityContext, minValue);
        }

        [RequiredArgument]
        [Input("Number 1")]
        public InArgument<decimal> Number1 { get; set; }

        [RequiredArgument]
        [Input("Number 2")]
        public InArgument<decimal> Number2 { get; set; }

        [Output("Min Value")]
        public OutArgument<decimal> MinValue { get; set; }
    }
}