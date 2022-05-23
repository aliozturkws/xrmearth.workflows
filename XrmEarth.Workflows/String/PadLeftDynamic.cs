using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class PadLeftDynamic : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToPad = StringToPad.Get(activityHelper.CodeActivityContext);
            string padCharacter = PadCharacter.Get(activityHelper.CodeActivityContext);
            int length = Length.Get(activityHelper.CodeActivityContext);

            string paddedString = stringToPad;

            while (paddedString.Length < length)
            {
                paddedString = padCharacter + paddedString;
            }

            PaddedString.Set(activityHelper.CodeActivityContext, paddedString);
        }

        [RequiredArgument]
        [Input("String To Pad")]
        public InArgument<string> StringToPad { get; set; }

        [RequiredArgument]
        [Input("Pad Character")]
        public InArgument<string> PadCharacter { get; set; }

        [RequiredArgument]
        [Input("Length")]
        public InArgument<int> Length { get; set; }

        [Output("Padded String")]
        public OutArgument<string> PaddedString { get; set; }
    }
}
