using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace XrmEarth.Core
{
    public class DynamicUrlParser
	{
		public string Url { get; set; }
		public int EntityTypeCode { get; set; }
		public Guid Id { get; set; }

		public DynamicUrlParser(string url)
		{
			try
			{
				Url = url;
				var uri = new Uri(url);
				int found = 0;

				string[] parameters = uri.Query.TrimStart('?').Split('&');
				foreach (string param in parameters)
				{
					var nameValue = param.Split('=');
					switch (nameValue[0])
					{
						case "etc":
							EntityTypeCode = int.Parse(nameValue[1]);
							found++;
							break;
						case "id":
							Id = new Guid(nameValue[1]);
							found++;
							break;
					}
					if (found > 1) break;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(System.String.Format("Url '{0}' is incorrectly formated for a Dynamics CRM Dynamics Url", url), ex);
			}
		}

		public string GetEntityLogicalName(IOrganizationService service)
		{
			var entityFilter = new MetadataFilterExpression(LogicalOperator.And);
			entityFilter.Conditions.Add(new MetadataConditionExpression("ObjectTypeCode ", MetadataConditionOperator.Equals, this.EntityTypeCode));
			var propertyExpression = new MetadataPropertiesExpression { AllProperties = false };
			propertyExpression.PropertyNames.Add("LogicalName");
			var entityQueryExpression = new EntityQueryExpression()
			{
				Criteria = entityFilter,
				Properties = propertyExpression
			};

			var retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest()
			{
				Query = entityQueryExpression
			};

			var response = service.Execute(retrieveMetadataChangesRequest);

			EntityMetadataCollection metadataCollection = (EntityMetadataCollection)response.Results["EntityMetadata"];
			
			if (metadataCollection.Count == 1)
			{
				return metadataCollection[0].LogicalName;
			}
			return null;
		}
	}
}