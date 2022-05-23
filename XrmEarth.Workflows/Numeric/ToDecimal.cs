using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Numeric
{
    public class ToDecimal : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string textToConvert = TextToConvert.Get(activityHelper.CodeActivityContext);

            if (string.IsNullOrEmpty(textToConvert))
            {
                IsValid.Set(activityHelper.CodeActivityContext, false);
                return;
            }

            decimal convertedNumber;
            bool isNumber = decimal.TryParse(textToConvert, out convertedNumber);

            if (isNumber)
            {
                ConvertedNumber.Set(activityHelper.CodeActivityContext, convertedNumber);
                IsValid.Set(activityHelper.CodeActivityContext, true);
            }
            else
                IsValid.Set(activityHelper.CodeActivityContext, false);
        }

        [RequiredArgument]
        [Input("Text To Convert")]
        public InArgument<string> TextToConvert { get; set; }

        [Output("Converted Number")]
        public OutArgument<decimal> ConvertedNumber { get; set; }

        [Output("Is Valid Decimal")]
        public OutArgument<bool> IsValid { get; set; }
    }
}
