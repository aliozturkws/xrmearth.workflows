using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class TextToQRCode : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string text = Text.Get(activityHelper.CodeActivityContext);

            Base64Text.Set(activityHelper.CodeActivityContext, QRCodeCreator.Create(text));
        }

        [RequiredArgument]
        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Output("Base64Text")]
        public OutArgument<string> Base64Text { get; set; }
    }
}
