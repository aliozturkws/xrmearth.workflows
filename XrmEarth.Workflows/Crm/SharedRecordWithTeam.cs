using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class SharedRecordWithTeam : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var target = new EntityReference(activityHelper.Context.PrimaryEntityName, activityHelper.Context.PrimaryEntityId);
            var user = Team.Get<EntityReference>(activityHelper.CodeActivityContext);
            var read = Read.Get<bool>(activityHelper.CodeActivityContext);
            var write = Write.Get<bool>(activityHelper.CodeActivityContext);
            var append = Append.Get<bool>(activityHelper.CodeActivityContext);
            var delete = Delete.Get<bool>(activityHelper.CodeActivityContext);
            var assign = Assign.Get<bool>(activityHelper.CodeActivityContext);
            var share = Share.Get<bool>(activityHelper.CodeActivityContext);
            var recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);

            if (!string.IsNullOrEmpty(recordUrl))
                target = RecordUrlHelper.GetEntityReference(recordUrl, activityHelper.OrganizationService);

            WorkflowHelper.SharePrivileges(activityHelper.OrganizationService,
                                           target,
                                           user,
                                           read,
                                           write,
                                           append,
                                           delete,
                                           assign,
                                           share);
        }

        [RequiredArgument]
        [Input("Team")]
        [ReferenceTarget(EntityNames.Team)]
        public InArgument<EntityReference> Team { get; set; }

        [RequiredArgument]
        [Input("Read")]
        public InArgument<bool> Read { get; set; }

        [RequiredArgument]
        [Input("Write")]
        public InArgument<bool> Write { get; set; }

        [RequiredArgument]
        [Input("Append")]
        public InArgument<bool> Append { get; set; }

        [RequiredArgument]
        [Input("Delete")]
        public InArgument<bool> Delete { get; set; }

        [RequiredArgument]
        [Input("Assign")]
        public InArgument<bool> Assign { get; set; }

        [RequiredArgument]
        [Input("Share")]
        public InArgument<bool> Share { get; set; }

        [Input("RecordUrl")]
        public InArgument<string> RecordUrl { get; set; }
    }
}
