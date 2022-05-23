using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Numeric
{
    public class IntToDecimal : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            try
            {
                int intToConvert = IntToConvert.Get(activityHelper.CodeActivityContext);

                decimal convertedNumber;
                bool isNumber = decimal.TryParse(intToConvert.ToString(), out convertedNumber);

                if (isNumber)
                {
                    ConvertedNumber.Set(activityHelper.CodeActivityContext, convertedNumber);
                    IsValid.Set(activityHelper.CodeActivityContext, true);
                }
                else
                    IsValid.Set(activityHelper.CodeActivityContext, false);
            }
            catch (Exception ex)
            {
                activityHelper.TracingService.Trace("Exception: {0}", ex.ToString());
            }
        }

        [RequiredArgument]
        [Input("Int To Convert")]
        public InArgument<int> IntToConvert { get; set; }

        [Output("Converted Number")]
        public OutArgument<decimal> ConvertedNumber { get; set; }

        [Output("Is Valid Decimal")]
        public OutArgument<bool> IsValid { get; set; }
    }
}
