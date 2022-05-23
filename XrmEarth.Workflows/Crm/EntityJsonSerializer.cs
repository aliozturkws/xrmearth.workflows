using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using XrmEarth.Core;
using System;
using System.Activities;
using System.Text;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class EntityJsonSerializer : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string serializingRecordUrl = SerializingRecordUrl.Get(activityHelper.CodeActivityContext);

            var dup = new DynamicUrlParser(serializingRecordUrl);
            string entityName = dup.GetEntityLogicalName(activityHelper.OrganizationService);

            var retrievedObject = activityHelper.OrganizationService.Retrieve(entityName, dup.Id, new ColumnSet(allColumns: true));

            string primaryIdAttribute = "";
            var atts = CrmHelper.GetEntityAttributesToClone(entityName, activityHelper.OrganizationService, ref primaryIdAttribute);

            var sJson = new StringBuilder("{\"" + entityName + "\": {");

            sJson.Append("\"" + primaryIdAttribute + "\": \"" + dup.Id + "\"");
            foreach (string att in atts)
            {
                if (retrievedObject.Attributes.Contains(att))
                {
                    sJson.Append(",");

                    Type t = retrievedObject.Attributes[att].GetType();

                    if (t == typeof(string))
                    {
                        sJson.Append("\"" + att + "\" : \"" + retrievedObject.Attributes[att].ToString() + "\"");
                    }
                    else if (t == typeof(bool))
                    {
                        sJson.Append("\"" + att + "\" : " + retrievedObject.Attributes[att].ToString().ToLower() + "");
                    }
                    else if (t == typeof(OptionSetValue))
                    {
                        var obj = (OptionSetValue)retrievedObject.Attributes[att];
                        sJson.Append("\"" + att + "\" : " + obj.Value);
                    }
                    else if (t == typeof(Money))
                    {
                        var obj = (Money)retrievedObject.Attributes[att];
                        sJson.Append("\"" + att + "\" : " + obj.Value);
                    }
                    else if (t == typeof(decimal))
                    {
                        sJson.Append("\"" + att + "\" : " + (decimal)retrievedObject.Attributes[att]);
                    }
                    else if (t == typeof(DateTime))
                    {
                        var dateTime = "\"" + ((DateTime)retrievedObject.Attributes[att]).ToUniversalTime().ToString("s") + "Z" + "\"";
                        sJson.Append("\"" + att + "\" : " + dateTime);

                    }
                    else if (t == typeof(EntityReference))
                    {
                        var obj = (EntityReference)retrievedObject.Attributes[att];
                        sJson.Append("\"" + att + "\" : { \"typename\" : \"" + obj.LogicalName.ToLower() + "\", \"id\" :\"" + obj.Id.ToString() + "\", \"name\":\"" + obj.Name + "\" }");
                    }
                    else
                    {
                        sJson.Append("\"" + att + "\" : " + retrievedObject.Attributes[att]);
                    }
                }
            }
            sJson.Append("}}");
            OutputJson.Set(activityHelper.CodeActivityContext, sJson.ToString());
        }

        [RequiredArgument]
        [Input("Serializing Record URL")]
        public InArgument<string> SerializingRecordUrl { get; set; }

        [Output("Output Json")]
        public OutArgument<string> OutputJson { get; set; }
    }
}
