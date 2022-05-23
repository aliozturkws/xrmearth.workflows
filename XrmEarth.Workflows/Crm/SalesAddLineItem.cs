using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    // Not Supported v9

    //public class SalesAddLineItem : BaseCodeActivity
    //{
    //    protected override void OnExecute(CodeActivityHelper activityHelper)
    //    {
    //        var context = activityHelper.CodeActivityContext;

    //        string recordUrl = RecordUrl.Get(activityHelper.CodeActivityContext);
    //        Guid recordId = RecordUrlHelper.GetIdByRecordUrl(recordUrl);
    //        int recordEtc = RecordUrlHelper.GetEtcByRecordUrl(recordUrl);
    //        string recordLogicalName = CrmHelper.GetEntityLogicalName(activityHelper.OrganizationService, recordEtc);

    //        string[] validEntityList = new string[] {
    //            EntityNames.SalesOrder,
    //            EntityNames.Opportunity,
    //            EntityNames.Quote,
    //            EntityNames.Invoice };

    //        if (Array.IndexOf(validEntityList, recordLogicalName) < 0)
    //            throw new ArgumentNullException("You need to specify either an Opportunity, Quote, Order or Invoice");

    //        if ((Product.Get(context) == null || UOM.Get(context) == null) && (ProductName.Get(context) == null || PricePerUnit.Get(context) == null))
    //            throw new ArgumentNullException("You need to specify either a Product and Unit, or Product Name and Price per Unit");

    //        Entity detail = GetDetailEntity(recordLogicalName, recordId, context);

    //        activityHelper.OrganizationService.Create(detail);

    //        Successful.Set(context, true);
    //    }

    //    public Entity GetDetailEntity(string recordLogicalName, Guid recordId, CodeActivityContext context)
    //    {
    //        Entity detail = null;

    //        if (recordLogicalName == EntityNames.Invoice)
    //        {
    //            detail = new Entity(EntityNames.InvoiceDetail);
    //            detail["invoiceid"] = new EntityReference(EntityNames.Invoice, recordId);
    //        }
    //        else if (recordLogicalName == EntityNames.Opportunity)
    //        {
    //            detail = new Entity(EntityNames.OpportunityProduct);
    //            detail["opportunityid"] = new EntityReference(EntityNames.Opportunity, recordId);
    //        }
    //        else if (recordLogicalName == EntityNames.SalesOrder)
    //        {
    //            detail = new Entity(EntityNames.SalesOrderDetail);
    //            detail["salesorderid"] = new EntityReference(EntityNames.SalesOrder, recordId);
    //        }
    //        else if (recordLogicalName == EntityNames.Quote)
    //        {
    //            detail = new Entity(EntityNames.QuoteDetail);
    //            detail["quoteid"] = new EntityReference(EntityNames.Quote, recordId);
    //        }

    //        if ((recordLogicalName == EntityNames.SalesOrder || recordLogicalName == EntityNames.Quote) && (RequestDeliveryBy.Get(context) != DateTime.MinValue))
    //        {
    //            detail["requestdeliveryby"] = RequestDeliveryBy.Get(context);
    //        }

    //        detail["quantity"] = Quantity.Get(context);
    //        detail["description"] = Description.Get(context);

    //        if (Product.Get(context) != null && UOM.Get(context) != null)
    //        {
    //            detail["isproductoverridden"] = false;
    //            detail["productid"] = new EntityReference("product", Product.Get(context).Id);
    //            detail["uomid"] = new EntityReference("uom", UOM.Get(context).Id);
    //            if (PricePerUnit.Get(context) != null)
    //            {
    //                detail["ispriceoverridden"] = true;
    //                detail["priceperunit"] = PricePerUnit.Get(context);
    //            }
    //        }
    //        else
    //        {
    //            detail["isproductoverridden"] = true;
    //            detail["productdescription"] = ProductName.Get(context);
    //            detail["priceperunit"] = PricePerUnit.Get(context);
    //        }

    //        if (ManualDiscount.Get(context) != null)
    //        {
    //            detail["manualdiscountamount"] = ManualDiscount.Get(context);
    //        }
    //        if (Tax.Get(context) != null)
    //        {
    //            detail["tax"] = Tax.Get(context);
    //        }

    //        return detail;
    //    }

    //    [RequiredArgument]
    //    [Input("Record Url (opportunity,quote,salesorder,invoice)")]
    //    public InArgument<string> RecordUrl { get; set; }

    //    [Input("Product")]
    //    [ReferenceTarget("product")]
    //    public InArgument<EntityReference> Product { get; set; }

    //    [Input("Unit of Measure")]
    //    [ReferenceTarget("uom")]
    //    public InArgument<EntityReference> UOM { get; set; }

    //    [Input("or Write-In Product Name and")]
    //    [ReferenceTarget("product")]
    //    public InArgument<string> ProductName { get; set; }

    //    [Input("Price per Unit")]
    //    public InArgument<Money> PricePerUnit { get; set; }

    //    [Input("Quantity")]
    //    [RequiredArgument()]
    //    public InArgument<decimal> Quantity { get; set; }

    //    [Input("Manual Discount")]
    //    public InArgument<Money> ManualDiscount { get; set; }

    //    [Input("Tax")]
    //    public InArgument<Money> Tax { get; set; }

    //    [Input("Description")]
    //    public InArgument<string> Description { get; set; }

    //    [Input("Request Delivery by (quote or order)")]
    //    public InArgument<DateTime> RequestDeliveryBy { get; set; }

    //    [Output("Product has been Added")]
    //    public OutArgument<bool> Successful { get; set; }
    //}
}
