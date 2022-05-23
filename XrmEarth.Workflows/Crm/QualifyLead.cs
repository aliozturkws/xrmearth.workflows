using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    // Not Supported v9

    //public class QualifyLead : BaseCodeActivity
    //{
    //    protected override void OnExecute(CodeActivityHelper activityHelper)
    //    {
    //        var lead = Lead.Get<EntityReference>(activityHelper.CodeActivityContext);
    //        var leadStatus = LeadStatus.Get<int>(activityHelper.CodeActivityContext);
    //        var createAccount = CreateAccount.Get<bool>(activityHelper.CodeActivityContext);
    //        var createContact = CreateContact.Get<bool>(activityHelper.CodeActivityContext);
    //        var createOpportunity = CreateOpportunity.Get<bool>(activityHelper.CodeActivityContext);
    //        var account = Account.Get<EntityReference>(activityHelper.CodeActivityContext);
    //        var transactionCurrency = TransactionCurrency.Get<EntityReference>(activityHelper.CodeActivityContext);

    //        WorkflowHelper.QualifyLead(activityHelper.OrganizationService, lead.Id, leadStatus, createAccount, createContact, createOpportunity, account, transactionCurrency);
    //    }

    //    [RequiredArgument]
    //    [Input("Lead")]
    //    [ReferenceTarget(EntityNames.Lead)]
    //    public InArgument<EntityReference> Lead { get; set; }

    //    [RequiredArgument]
    //    [Input("Lead Status")]
    //    public InArgument<int> LeadStatus { get; set; }

    //    [Input("Create Contact")]
    //    public InArgument<bool> CreateContact { get; set; }

    //    [Input("Create Account")]
    //    public InArgument<bool> CreateAccount { get; set; }

    //    [Input("Create Opportunity")]
    //    public InArgument<bool> CreateOpportunity { get; set; }

    //    [Input("Account")]
    //    [ReferenceTarget(EntityNames.Account)]
    //    public InArgument<EntityReference> Account { get; set; }

    //    [Input("Transaction Currency")]
    //    [ReferenceTarget(EntityNames.TransactionCurrency)]
    //    public InArgument<EntityReference> TransactionCurrency { get; set; }
    //}
}
