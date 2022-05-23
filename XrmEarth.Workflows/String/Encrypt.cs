using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class Encrypt : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToBeEncrypted = StringToBeEncrypted.Get(activityHelper.CodeActivityContext);
            string password = Password.Get(activityHelper.CodeActivityContext);

            Result.Set(activityHelper.CodeActivityContext, CipherHelper.Encrypt(stringToBeEncrypted, password));
        }

        [RequiredArgument]
        [Input("Password")]
        public InArgument<string> Password { get; set; }

        [RequiredArgument]
        [Input("String To Be Encrypted")]
        public InArgument<string> StringToBeEncrypted { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }
    }
}
