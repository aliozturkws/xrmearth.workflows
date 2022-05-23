using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class GetAvailableNextDay : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            #region | Params |
            var date = Date.Get(activityHelper.CodeActivityContext);
            var hour = Hour.Get(activityHelper.CodeActivityContext);
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
            var hourTimeSpan = StringHelper.GetTimeSpan(hour);
            #endregion | TimeSpan |

            var helper = new GetAvailableNextDayHelper()
            {
                Date = date,
                ExitsTheWeekend = exitsTheWeekend,
                ExitsTheBusinessClosed = exitsTheBusinessClosed,
                HourTimeSpan = hourTimeSpan,
                CalendarRules = calendarRules,
                service = activityHelper.OrganizationService
            };

            CalculatedEndDate.Set(activityHelper.CodeActivityContext, helper.GetAvailableDate());
        }

        [RequiredArgument]
        [Input("Date")]
        public InArgument<DateTime> Date { get; set; }

        [RequiredArgument]
        [Input("Hour")]
        [Default("")]
        public InArgument<string> Hour { get; set; }

        [RequiredArgument]
        [Input("Exits The Weekend")]
        public InArgument<bool> ExitsTheWeekend { get; set; }

        [RequiredArgument]
        [Input("Exits The Business Closed")]
        public InArgument<bool> ExitsTheBusinessClosed { get; set; }

        [Output("CalculatedEndDate")]
        public OutArgument<DateTime> CalculatedEndDate { get; set; }
    }
}
