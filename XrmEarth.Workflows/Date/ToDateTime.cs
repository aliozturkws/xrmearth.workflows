using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class ToDateTime : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string textToConvert = TextToConvert.Get(activityHelper.CodeActivityContext);

            DateTime convertedDate;
            bool isValid = DateTime.TryParse(textToConvert, out convertedDate);

            ConvertedDate.Set(activityHelper.CodeActivityContext, convertedDate);
            IsValid.Set(activityHelper.CodeActivityContext, isValid);
        }

        [RequiredArgument]
        [Input("Text To Convert")]
        public InArgument<string> TextToConvert { get; set; }

        [Output("Converted Date")]
        public OutArgument<DateTime> ConvertedDate { get; set; }

        [Output("Is Valid Date")]
        public OutArgument<bool> IsValid { get; set; }
    }
}
