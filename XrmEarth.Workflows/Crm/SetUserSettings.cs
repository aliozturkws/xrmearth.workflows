using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class SetUserSettings : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var userReference = User.Get(activityHelper.CodeActivityContext);
            int pagingLimit = PagingLimit.Get(activityHelper.CodeActivityContext);
            int advancedFindStartupMode = AdvancedFindStartupMode.Get(activityHelper.CodeActivityContext);
            int timeZoneCode = TimeZoneCode.Get(activityHelper.CodeActivityContext);
            int helpLanguageId = HelpLanguageId.Get(activityHelper.CodeActivityContext);
            int uiLanguageId = UILanguageId.Get(activityHelper.CodeActivityContext);
            int defaultCalendarView = DefaultCalendarView.Get(activityHelper.CodeActivityContext);

            var newSettings = new Entity(EntityNames.UserSettings, userReference.Id);

            if (pagingLimit != 0)
                newSettings.Attributes.Add("paginglimit", pagingLimit);
            if (advancedFindStartupMode == 1 || advancedFindStartupMode == 2)
                newSettings.Attributes.Add("advancedfindstartupmode", advancedFindStartupMode);
            if (timeZoneCode != 0)
                newSettings.Attributes.Add("timezonecode", timeZoneCode);
            if (helpLanguageId != 0)
                newSettings.Attributes.Add("helplanguageid", helpLanguageId);
            if (uiLanguageId != 0)
                newSettings.Attributes.Add("uilanguageid", uiLanguageId);
            if (defaultCalendarView == 0 || defaultCalendarView == 1 || defaultCalendarView == 2)
                newSettings.Attributes.Add("defaultcalendarview", defaultCalendarView);
            activityHelper.OrganizationService.Update(newSettings);
        }

        [RequiredArgument]
        [Input("System User")]
        [ReferenceTarget(EntityNames.SystemUser)]
        public InArgument<EntityReference> User { get; set; }

        [RequiredArgument]
        [Input("PagingLimit")]
        [Default("0")]
        public InArgument<int> PagingLimit { get; set; }

        [RequiredArgument]
        [Input("AdvancedFindStartupMode")]
        [Default("1")]
        public InArgument<int> AdvancedFindStartupMode { get; set; }

        [RequiredArgument]
        [Input("TimeZoneCode")]
        [Default("0")]
        public InArgument<int> TimeZoneCode { get; set; }

        [RequiredArgument]
        [Input("HelpLanguageId")]
        [Default("0")]
        public InArgument<int> HelpLanguageId { get; set; }

        [RequiredArgument]
        [Input("UILanguageId")]
        [Default("0")]
        public InArgument<int> UILanguageId { get; set; }

        [RequiredArgument]
        [Input("DefaultCalendarView")]
        [Default("0")]
        public InArgument<int> DefaultCalendarView { get; set; }
    }
}
