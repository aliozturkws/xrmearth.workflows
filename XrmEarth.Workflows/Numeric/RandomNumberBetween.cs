using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Numeric
{
    public class RandomNumberBetween : BaseCodeActivity
	{
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            int minValue = MinValue.Get(activityHelper.CodeActivityContext);
            int maxValue = MaxValue.Get(activityHelper.CodeActivityContext);

            if (minValue < 1)
                minValue = 0;

            if (maxValue < 1)
                maxValue = 1;

            if (maxValue < minValue)
                throw new InvalidPluginExecutionException("Max Value must be greater than Min Value.");

            if (maxValue == minValue)
            {
                GeneratedNumber.Set(activityHelper.CodeActivityContext, maxValue);
                return;
            }

            Random random = new Random();
            int generatedNumber = random.Next(minValue, maxValue);

            GeneratedNumber.Set(activityHelper.CodeActivityContext, generatedNumber);
        }

        [RequiredArgument]
	    [Input("Min Value")]
	    public InArgument<int> MinValue { get; set; }

	    [RequiredArgument]
	    [Input("Max Value")]
	    public InArgument<int> MaxValue { get; set; }

	    [Output("Generated Number")]
	    public OutArgument<int> GeneratedNumber { get; set; }
    }
}
