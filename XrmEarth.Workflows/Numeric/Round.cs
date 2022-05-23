using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Numeric
{
    public class Round : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            decimal numberToRound = NumberToRound.Get(activityHelper.CodeActivityContext);
            int decimalPlaces = DecimalPlaces.Get(activityHelper.CodeActivityContext);

            if (decimalPlaces < 0)
                decimalPlaces = 0;

            decimal roundedNumber = Math.Round(numberToRound, decimalPlaces);

            RoundedNumber.Set(activityHelper.CodeActivityContext, roundedNumber);
        }

        [RequiredArgument]
        [Input("Number To Round")]
        public InArgument<decimal> NumberToRound { get; set; }

        [RequiredArgument]
        [Input("Decimal Places")]
        public InArgument<int> DecimalPlaces { get; set; }

        [Output("Rounded Number")]
        public OutArgument<decimal> RoundedNumber { get; set; }
    }
}
