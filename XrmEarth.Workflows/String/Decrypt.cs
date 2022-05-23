using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class Decrypt : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToBeDecrypted = StringToBeDecrypted.Get(activityHelper.CodeActivityContext);
            string password = Password.Get(activityHelper.CodeActivityContext);

            Result.Set(activityHelper.CodeActivityContext, CipherHelper.Decrypt(stringToBeDecrypted, password));
        }

        [RequiredArgument]
        [Input("Password")]
        public InArgument<string> Password { get; set; }

        [RequiredArgument]
        [Input("String To Be Decrypted")]
        public InArgument<string> StringToBeDecrypted { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }
    }
}
