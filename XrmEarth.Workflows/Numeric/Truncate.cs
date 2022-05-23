using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Numeric
{
    public class Truncate : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            decimal numberToTruncate = NumberToTruncate.Get(activityHelper.CodeActivityContext);
            int decimalPlaces = DecimalPlaces.Get(activityHelper.CodeActivityContext);

            if (decimalPlaces < 0)
                decimalPlaces = 0;

            decimal step = (decimal)Math.Pow(10, decimalPlaces);
            int temp = (int)Math.Truncate(step * numberToTruncate);

            decimal truncatedNumber = temp / step;

            TruncatedNumber.Set(activityHelper.CodeActivityContext, truncatedNumber);
        }

        [RequiredArgument]
        [Input("Number To Truncate")]
        public InArgument<decimal> NumberToTruncate { get; set; }

        [RequiredArgument]
        [Input("Decimal Places")]
        public InArgument<int> DecimalPlaces { get; set; }

        [Output("Truncated Number")]
        public OutArgument<decimal> TruncatedNumber { get; set; }
    }
}
