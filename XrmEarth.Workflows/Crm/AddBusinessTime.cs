using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class AddBusinessTime : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            #region | Params |
            var date = Date.Get(activityHelper.CodeActivityContext);
            var durationType = DurationType.Get(activityHelper.CodeActivityContext);
            var duration = Duration.Get(activityHelper.CodeActivityContext);
            var businessStartTime = BusinessStartTime.Get(activityHelper.CodeActivityContext);
            var businessEndTime = BusinessEndTime.Get(activityHelper.CodeActivityContext);
            var noonHourStartTime = NoonHourStartTime.Get(activityHelper.CodeActivityContext);
            var noonHourEndTime = NoonHourEndTime.Get(activityHelper.CodeActivityContext);
            var exitsTheWeekend = ExitsTheWeekend.Get(activityHelper.CodeActivityContext);
            var exitsTheBusinessClosed = ExitsTheBusinessClosed.Get(activityHelper.CodeActivityContext);
            #endregion | Params |

            #region | Calendar |

            EntityCollection calendar = null;
            EntityCollection calendarRules = null;

            if (exitsTheBusinessClosed)
            {
                var calendarId = CrmHelper.GetBusinessClosureCalendarId(activityHelper.OrganizationService, activityHelper.Context.OrganizationId);
                calendar = CrmHelper.GetCalendarList(activityHelper.OrganizationService, calendarId);
            }

            if (calendar != null && calendar.Entities.Count > 0)
                calendarRules = CrmHelper.GetCalendarRuleList(activityHelper.OrganizationService, calendar[0]);

            #endregion | Calendar |

            #region | TimeSpan |
            var businessStartTimeSpan = StringHelper.GetTimeSpan(businessStartTime);
            var businessEndTimeSpan = StringHelper.GetTimeSpan(businessEndTime);
            var noonHourStartTimeSpan = StringHelper.GetTimeSpan(noonHourStartTime);
            var noonHourEndTimeSpan = StringHelper.GetTimeSpan(noonHourEndTime);
            #endregion | TimeSpan |

            var helper = new AddBusinessTimeHelper()
            {
                Date = date,
                DurationType = durationType,
                Duration = duration,
                ExitsTheWeekend = exitsTheWeekend,
                ExitsTheBusinessClosed = exitsTheBusinessClosed,
                BusinessStartTimeSpan = businessStartTimeSpan,
                BusinessEndTimeSpan = businessEndTimeSpan,
                NoonHourStartTimeSpan = noonHourStartTimeSpan,
                NoonHourEndTimeSpan = noonHourEndTimeSpan,
                CalendarRules = calendarRules,
                service = activityHelper.OrganizationService
            };

            CalculatedDate.Set(activityHelper.CodeActivityContext, helper.CalculatedDate());
        }

        [RequiredArgument]
        [Input("Date")]
        public InArgument<DateTime> Date { get; set; }

        [RequiredArgument]
        [Input("Duration Type (Day,Hour or Minute)")]
        [Default("Hour")]
        public InArgument<string> DurationType { get; set; }

        [RequiredArgument]
        [Input("Duration")]
        public InArgument<decimal> Duration { get; set; }

        [RequiredArgument]
        [Input("Business Start Time")]
        [Default("")]
        public InArgument<string> BusinessStartTime { get; set; }

        [RequiredArgument]
        [Input("Business End Time")]
        [Default("")]
        public InArgument<string> BusinessEndTime { get; set; }

        [Input("Noon Hour Start Time")]
        [Default("")]
        public InArgument<string> NoonHourStartTime { get; set; }

        [Input("Noon Hour End Time")]
        [Default("")]
        public InArgument<string> NoonHourEndTime { get; set; }

        [RequiredArgument]
        [Input("Exits The Weekend")]
        public InArgument<bool> ExitsTheWeekend { get; set; }

        [RequiredArgument]
        [Input("Exits The Business Closed")]
        public InArgument<bool> ExitsTheBusinessClosed { get; set; }

        [Output("Calculated Date")]
        public OutArgument<DateTime> CalculatedDate { get; set; }
    }
}
