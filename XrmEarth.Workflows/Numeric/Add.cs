using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Numeric
{
    public class Add : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            decimal number1 = Number1.Get(activityHelper.CodeActivityContext);
            decimal number2 = Number2.Get(activityHelper.CodeActivityContext);
            int roundDecimalPlaces = RoundDecimalPlaces.Get(activityHelper.CodeActivityContext);

            decimal sum = number1 + number2;

            if (roundDecimalPlaces != -1)
                sum = Math.Round(sum, roundDecimalPlaces);

            Sum.Set(activityHelper.CodeActivityContext, sum);
        }

        [RequiredArgument]
        [Input("Number 1")]
        public InArgument<decimal> Number1 { get; set; }

        [RequiredArgument]
        [Input("Number 2")]
        public InArgument<decimal> Number2 { get; set; }

        [RequiredArgument]
        [Input("Round Decimal Places")]
        [Default("-1")]
        public InArgument<int> RoundDecimalPlaces { get; set; }

        [Output("Sum")]
        public OutArgument<decimal> Sum { get; set; }
    }
}