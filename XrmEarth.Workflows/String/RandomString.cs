using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class RandomString : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            bool isString = IsString.Get(activityHelper.CodeActivityContext);
            bool isNumeric = IsNumeric.Get(activityHelper.CodeActivityContext);
            int length = Length.Get(activityHelper.CodeActivityContext);
            string result = string.Empty;

            if (length < 1)
                throw new InvalidPluginExecutionException("Length field must be greater than zero!");

            if (isNumeric && isString)
                result = StringHelper.RandomString(length, StringHelper.RandomStringChars + StringHelper.RandomNumericChars);
            else if (isString)
                result = StringHelper.RandomString(length, StringHelper.RandomStringChars);
            else if (isNumeric)
                result = StringHelper.RandomString(length, StringHelper.RandomNumericChars);
            else
                throw new InvalidPluginExecutionException("One of Is Numeric or Is String fields must be full!");

            Result.Set(activityHelper.CodeActivityContext, result);
        }

        [RequiredArgument]
        [Input("Is String")]
        public InArgument<bool> IsString { get; set; }

        [RequiredArgument]
        [Input("Is Numeric")]
        public InArgument<bool> IsNumeric { get; set; }

        [RequiredArgument]
        [Input("Length")]
        public InArgument<int> Length { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }
    }
}
