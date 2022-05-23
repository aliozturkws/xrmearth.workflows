using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class IsBetween : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime startingDate = StartingDate.Get(activityHelper.CodeActivityContext);
            DateTime dateToValidate = DateToValidate.Get(activityHelper.CodeActivityContext);
            DateTime endingDate = EndingDate.Get(activityHelper.CodeActivityContext);

            var between = dateToValidate > startingDate && dateToValidate < endingDate;

            Between.Set(activityHelper.CodeActivityContext, between);
        }

        [RequiredArgument]
        [Input("Starting Date")]
        public InArgument<DateTime> StartingDate { get; set; }

        [RequiredArgument]
        [Input("Date To Validate")]
        public InArgument<DateTime> DateToValidate { get; set; }

        [RequiredArgument]
        [Input("Ending Date")]
        public InArgument<DateTime> EndingDate { get; set; }

        [Output("Between")]
        public OutArgument<bool> Between { get; set; }
    }
}
