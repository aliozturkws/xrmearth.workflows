using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class Substring : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToParse = StringToParse.Get(activityHelper.CodeActivityContext);
            int startPosition = StartPosition.Get(activityHelper.CodeActivityContext);
            int length = Length.Get(activityHelper.CodeActivityContext);

            if (startPosition < 0)
                startPosition = 0;

            if (startPosition > stringToParse.Length)
            {
                activityHelper.TracingService.Trace("Specified start position [" + startPosition + "] is after end is string [" + stringToParse + "]");
                PartialString.Set(activityHelper.CodeActivityContext, null);
            }

            if (length > stringToParse.Length)
                length = stringToParse.Length - startPosition;

            string partialString = (length == 0)
                ? stringToParse.Substring(startPosition)
                : stringToParse.Substring(startPosition, length);

            PartialString.Set(activityHelper.CodeActivityContext, partialString);
        }

        [RequiredArgument]
        [Input("String To Parse")]
        public InArgument<string> StringToParse { get; set; }

        [RequiredArgument]
        [Input("Start Position")]
        public InArgument<int> StartPosition { get; set; }

        [Input("Length")]
        public InArgument<int> Length { get; set; }

        [Output("Partial String")]
        public OutArgument<string> PartialString { get; set; }
    }
}
