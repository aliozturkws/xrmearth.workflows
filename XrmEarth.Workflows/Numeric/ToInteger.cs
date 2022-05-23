using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Numeric
{
    public class ToInteger : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string textToConvert = TextToConvert.Get(activityHelper.CodeActivityContext);

            if (string.IsNullOrEmpty(textToConvert))
            {
                IsValid.Set(activityHelper.CodeActivityContext, false);
                return;
            }

            int convertedNumber;
            bool isNumber = int.TryParse(textToConvert, out convertedNumber);

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
        public OutArgument<int> ConvertedNumber { get; set; }

        [Output("Is Valid Integer")]
        public OutArgument<bool> IsValid { get; set; }
    }
}
