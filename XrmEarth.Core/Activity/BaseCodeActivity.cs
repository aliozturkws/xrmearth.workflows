using Microsoft.Xrm.Sdk;
using System;
using System.Activities;

namespace XrmEarth.Core.Activity
{
    public abstract class BaseCodeActivity : CodeActivity
    {
        protected abstract void OnExecute(CodeActivityHelper activityHelper);

        protected override void Execute(CodeActivityContext codeActivityContext)
        {
            var activityHelper = new CodeActivityHelper(codeActivityContext);
            try
            {
                activityHelper.Trace(string.Concat("Started."));
                OnExecute(activityHelper);
                activityHelper.Trace(string.Concat("Ended."));
            }
            catch (InvalidPluginExecutionException ex)
            {
                activityHelper.Trace(string.Concat("Error : ", ex.ToString()));
                throw;
            }
            catch (Exception ex)
            {
                activityHelper.Trace(string.Concat("Error : ", ex.ToString()));
                throw;
            }
        }
    }
}
