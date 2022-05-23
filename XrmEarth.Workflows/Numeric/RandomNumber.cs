using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Numeric
{
    public class RandomNumber : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            int maxValue = MaxValue.Get(activityHelper.CodeActivityContext);

            if (maxValue < 1)
                maxValue = 1;

            Random random = new Random();
            int generatedNumber = random.Next(maxValue);

            GeneratedNumber.Set(activityHelper.CodeActivityContext, generatedNumber);
        }

        [RequiredArgument]
        [Input("Max Value")]
        public InArgument<int> MaxValue { get; set; }

        [Output("Generated Number")]
        public OutArgument<int> GeneratedNumber { get; set; }
    }
}
