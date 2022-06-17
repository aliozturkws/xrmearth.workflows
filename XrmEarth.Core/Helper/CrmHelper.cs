using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using XrmEarth.Data.Const;
using System;
using System.Collections.Generic;

namespace XrmEarth.Core
{
    public class CrmHelper
    {
        public static int GetStateCode(IOrganizationService orgService, string entityName, int statusCode)
        {
            int stateOptionValue = -1;
            var attributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = "statuscode",
                RetrieveAsIfPublished = true
            };
            var attributeResponse = (RetrieveAttributeResponse)orgService.Execute(attributeRequest);
            var attrMetadata = (AttributeMetadata)attributeResponse.AttributeMetadata;
            var statusMetadata = (StatusAttributeMetadata)attrMetadata;
            foreach (var optionMeta in statusMetadata.OptionSet.Options)
            {
                if (optionMeta.Value == statusCode)
                {
                    stateOptionValue = (int)((StatusOptionMetadata)optionMeta).State;
                }
            }
            if (stateOptionValue < 0)
                throw new InvalidPluginExecutionException("Exist StatusCode!");
            return stateOptionValue;
        }

        public static void SetState(IOrganizationService service, string entityName, Guid entityId, int stateCode, int statusCode)
        {
            var target = new EntityReference(entityName, entityId);
            var request = new SetStateRequest();
            request.EntityMoniker = target;
            request.State = new OptionSetValue(stateCode);
            request.Status = new OptionSetValue(statusCode);
            service.Execute(request);
        }

        public static string GetEntityLogicalName(IOrganizationService service, int etc)
        {
            var entityFilter = new MetadataFilterExpression(LogicalOperator.And);
            entityFilter.Conditions.Add(new MetadataConditionExpression("ObjectTypeCode", MetadataConditionOperator.Equals, etc));
            var entityQueryExpression = new EntityQueryExpression()
            {
                Criteria = entityFilter
            };
            var retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
                ClientVersionStamp = null
            };
            var entityList = (RetrieveMetadataChangesResponse)service.Execute(retrieveMetadataChangesRequest);
            return entityList.EntityMetadata[0].LogicalName;
        }

        public static AssociateResponse Associate(IOrganizationService service, Guid targetEntityId, string targetEntityName, Guid relatedEntityId, string relatedEntityName, string relationShipName)
        {
            var entityCollection = new EntityReferenceCollection();
            entityCollection.Add(new EntityReference(relatedEntityName, relatedEntityId));

            var request = new AssociateRequest()
            {
                RelatedEntities = entityCollection,
                Target = new EntityReference(targetEntityName, targetEntityId),
                Relationship = new Relationship(relationShipName)
            };

            var result = (AssociateResponse)service.Execute(request);
            return result;
        }

        public static DisassociateResponse Disassociate(IOrganizationService service, Guid targetEntityId, string targetEntityName, Guid relatedEntityId, string relatedEntityName, string relationShipName)
        {
            var entityCollection = new EntityReferenceCollection();
            entityCollection.Add(new EntityReference(relatedEntityName, relatedEntityId));

            var request = new DisassociateRequest()
            {
                Target = new EntityReference(targetEntityName, targetEntityId),
                RelatedEntities = entityCollection,
                Relationship = new Relationship(relationShipName)
            };

            var result = (DisassociateResponse)service.Execute(request);
            return result;
        }

        public static bool CheckAssociate(IOrganizationService service, string relationShipEntityName, Guid targetEntityId, string targetEntityName, Guid relatedEntityId, string relatedEntityName)
        {
            var query = new QueryExpression(relationShipEntityName)
            {
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(string.Format("{0}id", targetEntityName), ConditionOperator.Equal, targetEntityId),
                        new ConditionExpression(string.Format("{0}id", relatedEntityName), ConditionOperator.Equal, relatedEntityId),
                    }
                },
            };
            return service.RetrieveMultiple(query).Entities.Count != 0;
        }

        public static Guid GetRoleId(IOrganizationService service, string roleName, Guid businessUnitId)
        {
            var query = new QueryExpression(EntityNames.Role);
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition("name", ConditionOperator.Equal, roleName);
            query.Criteria.AddCondition("businessunitid", ConditionOperator.Equal, businessUnitId);
            var result = service.RetrieveMultiple(query);

            if (result != null && result.Entities.Count > 0)
                return result[0].Id;
            else
                return Guid.Empty;
        }

        public static Guid GetBusinessUnitId(IOrganizationService service, string entityName, Guid entityId)
        {
            var result = service.Retrieve(entityName, entityId, new ColumnSet("businessunitid"));
            if (result != null && result.Contains("businessunitid"))
                return ((EntityReference)result["businessunitid"]).Id;
            else
                return Guid.Empty;
        }

        public static int? RetrieveTimeZoneCode(IOrganizationService service)
        {
            var currentUserSettings = service.RetrieveMultiple(

                new QueryExpression("usersettings")
                {
                    ColumnSet = new ColumnSet("timezonecode"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("systemuserid", ConditionOperator.EqualUserId)
                        }
                    }
                }).Entities[0].ToEntity<Entity>();

            return (int?)currentUserSettings.Attributes["timezonecode"];
        }

        public static DateTime RetrieveLocalTimeFromUtcTime(IOrganizationService service, DateTime utcTime, int? timeZoneCode)
        {
            if (!timeZoneCode.HasValue)
                return DateTime.Now;

            var request = new LocalTimeFromUtcTimeRequest
            {
                TimeZoneCode = timeZoneCode.Value,
                UtcTime = utcTime.ToUniversalTime()
            };

            OrganizationResponse response = service.Execute(request);
            return (DateTime)response.Results["LocalTime"];
        }

        public static bool CheckForAttachment(IOrganizationService service, Guid noteId)
        {
            Entity note = service.Retrieve("annotation", noteId, new ColumnSet("isdocument"));

            object oIsDocument;
            bool hasValue = note.Attributes.TryGetValue("isdocument", out oIsDocument);
            if (!hasValue)
                return false;

            return (bool)oIsDocument;
        }

        public static Entity GetNote(IOrganizationService service, Guid noteId)
        {
            return service.Retrieve("annotation", noteId, new ColumnSet(true));
        }

        public static bool CheckForAttachment(Entity note)
        {
            object oIsAttachment;
            bool hasValue = note.Attributes.TryGetValue("isdocument", out oIsAttachment);
            if (!hasValue)
                return false;

            return (bool)oIsAttachment;
        }

        public static void UpdateNote(IOrganizationService service, Entity note, string notice)
        {
            Entity updateNote = new Entity("annotation");
            updateNote.Id = note.Id;
            if (!string.IsNullOrEmpty(notice))
            {
                string newText = note.GetAttributeValue<string>("notetext");
                if (!string.IsNullOrEmpty(newText))
                    newText += "\r\n";

                updateNote["notetext"] = newText + notice;
            }
            updateNote["isdocument"] = false;
            updateNote["filename"] = null;
            updateNote["documentbody"] = null;
            updateNote["filesize"] = null;

            service.Update(updateNote);
        }

        public static bool ExtensionMatch(IEnumerable<string> extenstons, string filename)
        {
            foreach (string ex in extenstons)
            {
                if (filename.EndsWith("." + ex))
                    return true;
            }
            return false;
        }

        public static List<string> GetEntityAttributesToClone(string entityName, IOrganizationService service, ref string primaryIdAttribute)
        {
            List<string> atts = new List<string>();
            RetrieveEntityRequest req = new RetrieveEntityRequest()
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = entityName
            };

            RetrieveEntityResponse res = (RetrieveEntityResponse)service.Execute(req);
            primaryIdAttribute = res.EntityMetadata.PrimaryIdAttribute;

            foreach (AttributeMetadata attMetadata in res.EntityMetadata.Attributes)
            {
                if ((attMetadata.IsValidForCreate.Value || attMetadata.IsValidForUpdate.Value) && !attMetadata.IsPrimaryId.Value)
                {
                    if (attMetadata.AttributeTypeName.Value.ToLower() == "partylisttype")
                    {
                        atts.Add("partylist-" + attMetadata.LogicalName);
                        //atts.Add(attMetadata.LogicalName);
                    }
                    else
                    {
                        atts.Add(attMetadata.LogicalName);
                    }
                }
            }
            return (atts);
        }

        public static AttributeTypeCode? GetAttributeType(IOrganizationService service, string entityName, string attributeName)
        {
            var request = new RetrieveAttributeRequest();
            request.EntityLogicalName = entityName;
            request.LogicalName = attributeName;
            var response = (RetrieveAttributeResponse)service.Execute(request);
            var attmetadata = response.AttributeMetadata;
            return attmetadata.AttributeType;
        }

        public static EntityCollection GetProcessStageList(IOrganizationService service, EntityReference process)
        {
            var queryStage = new QueryExpression(EntityNames.ProcessStage);
            queryStage.ColumnSet = new ColumnSet("stagename", "stagecategory");
            queryStage.Criteria.AddCondition(new ConditionExpression(
                "processid",
                ConditionOperator.Equal,
                process.Id));

            return service.RetrieveMultiple(queryStage);
        }

        public static EntityCollection GetCalendarList(IOrganizationService service, Guid calendarId)
        {
            var queryStage = new QueryExpression(EntityNames.Calendar);
            queryStage.ColumnSet = new ColumnSet(true);
            queryStage.Criteria.AddCondition(new ConditionExpression(
                "calendarid",
                ConditionOperator.Equal,
                calendarId));

            return service.RetrieveMultiple(queryStage);
        }

        public static Guid GetBusinessClosureCalendarId(IOrganizationService service, Guid organizationId)
        {
            var query = new QueryExpression(EntityNames.Organization);
            query.ColumnSet = new ColumnSet("businessclosurecalendarid");
            query.Criteria.AddCondition(new ConditionExpression(
                "organizationid",
                ConditionOperator.Equal,
                organizationId));

            var result = service.RetrieveMultiple(query);
            if (result != null && result.Entities.Count > 0)
                return result[0].Contains("businessclosurecalendarid") ? (Guid)result[0]["businessclosurecalendarid"] : Guid.Empty;
            else
                return Guid.Empty;
        }

        public static EntityCollection GetCalendarRuleList(IOrganizationService service, Entity calendar)
        {
            return calendar.Contains("calendarrules") ? (EntityCollection)calendar["calendarrules"] : null;
        }

        public static string BuildFetchXml(Guid roleId)
        {
            const string fetchXml =
                @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                      <entity name='systemuser'>
                        <attribute name='systemuserid' />
                            <filter type='and'>
                             <condition attribute='accessmode' operator='eq' value='0' />
                            </filter>
                        <link-entity name='systemuserroles' from='systemuserid' to='systemuserid' visible='false' intersect='true'>
                          <link-entity name='role' from='roleid' to='roleid' alias='aa'>
                            <filter type='and'>
                              <condition attribute='roleid' operator='eq' uitype='role' value='{0}' />
                            </filter>
                          </link-entity>
                        </link-entity>
                      </entity>
                    </fetch>";

            return string.Format(fetchXml, roleId);
        }

        public static EntityCollection GetTeamInUsers(IOrganizationService service, Guid teamId)
        {
            var userQuery = new QueryExpression(EntityNames.SystemUser);
            userQuery.ColumnSet = new ColumnSet(true);
            userQuery.Criteria.AddCondition("isdisabled", ConditionOperator.Equal, false);
            var teamLink = new LinkEntity(EntityNames.SystemUser, EntityNames.TeamMembership, "systemuserid", "systemuserid", JoinOperator.Inner);
            var teamCondition = new ConditionExpression("teamid", ConditionOperator.Equal, teamId);
            teamLink.LinkCriteria.AddCondition(teamCondition);
            userQuery.LinkEntities.Add(teamLink);
            return service.RetrieveMultiple(userQuery);
        }

        public static bool CheckIsMemberOfMarketingList(IOrganizationService service, Guid listId, Guid entityId)
        {
            var query = new QueryExpression { EntityName = EntityNames.List };

            var linkEntity = new LinkEntity
            {
                JoinOperator = JoinOperator.Natural,
                LinkFromEntityName = EntityNames.List,
                LinkFromAttributeName = "listid",
                LinkToEntityName = EntityNames.ListMember,
                LinkToAttributeName = "listid",
                LinkCriteria = new FilterExpression(LogicalOperator.And)
            };

            linkEntity.LinkCriteria.AddCondition("listid", ConditionOperator.Equal, listId);
            linkEntity.LinkCriteria.AddCondition("entityid", ConditionOperator.Equal, entityId);

            query.LinkEntities.Add(linkEntity);

            var collection = service.RetrieveMultiple(query);

            return (collection.Entities.Count > 0);
        }

        public static void CheckMarketingListMemberEntityType(string entityName)
        {
            if (entityName != EntityNames.Account && entityName != EntityNames.Contact && entityName != EntityNames.Lead)
            {
                throw new Exception("Entity type error. Must be account, contact or lead.");
            }
        }

        public static EntityCollection GetListMembers(IOrganizationService service, Guid entityId)
        {
            var query = new QueryExpression
            {
                EntityName = EntityNames.ListMember,
                ColumnSet = new ColumnSet(true),
                Criteria =
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = "entityid",
                            Operator = ConditionOperator.Equal,
                            Values = { entityId }
                        },
                    }
                }
            };

            return service.RetrieveMultiple(query);
        }

        public static EntityCollection GetAppModules(IOrganizationService service, string appModuleUniqueName)
        {
            var query = new QueryExpression
            {
                EntityName = "appmodule",
                ColumnSet = new ColumnSet("appmoduleid", "uniquename"),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression ("uniquename", ConditionOperator.Equal, appModuleUniqueName)
                    }
                }
            };
            return service.RetrieveMultiple(query);
        }

        public static EntityCollection GetAttachments(IOrganizationService service, Guid entityId)
        {
            QueryExpression query = new QueryExpression("annotation");
            query.Criteria = new FilterExpression(LogicalOperator.And);
            query.Criteria.AddCondition(new ConditionExpression("objectid", ConditionOperator.Equal, entityId));
            query.ColumnSet.AllColumns = true;
            return service.RetrieveMultiple(query);
        }

        public static EntityCollection GetAllRecord(IOrganizationService service, QueryExpression query, int pageCount = 5000)
        {
            query.PageInfo = new PagingInfo();
            query.PageInfo.Count = pageCount;
            query.PageInfo.PageNumber = 1;

            EntityCollection totalResults = new EntityCollection();

            while (true)
            {
                EntityCollection results = service.RetrieveMultiple(query);

                if (results.MoreRecords)
                {
                    query.PageInfo.PageNumber++;
                    query.PageInfo.PagingCookie = results.PagingCookie;
                    totalResults.Entities.AddRange(results.Entities);
                }
                else
                {
                    totalResults.Entities.AddRange(results.Entities);
                    break;
                }
            }

            return totalResults;
        }

        public static EntityCollection GetRollupFunctionsForJaroWinklerSimilarity(IOrganizationService service, string fetchXML, string recordUrl, string recordUrl2 = null, string inputText1 = null, string inputText2 = null)
        {
            Guid entityId2 = Guid.Empty;

            if (!string.IsNullOrEmpty(recordUrl2))
            {
                entityId2 = RecordUrlHelper.GetIdByRecordUrl(recordUrl2);

                if (fetchXML.Contains("RecordId2"))
                    fetchXML = fetchXML.Replace("RecordId2", entityId2.ToString());
            }

            if (!string.IsNullOrEmpty(recordUrl))
            {
                var entityId = RecordUrlHelper.GetIdByRecordUrl(recordUrl);

                if (fetchXML.Contains("RecordId"))
                    fetchXML = fetchXML.Replace("RecordId", entityId.ToString());
            }

            if (!string.IsNullOrEmpty(inputText1))
            {
                if (fetchXML.Contains("InputText1"))
                    fetchXML = fetchXML.Replace("InputText1", inputText1);
            }

            if (!string.IsNullOrEmpty(inputText2))
            {
                if (fetchXML.Contains("InputText2"))
                    fetchXML = fetchXML.Replace("InputText2", inputText2);
            }

            FetchXmlToQueryExpressionRequest conversionRequest = new FetchXmlToQueryExpressionRequest
            {
                FetchXml = fetchXML
            };

            FetchXmlToQueryExpressionResponse conversionResponse = (FetchXmlToQueryExpressionResponse)service.Execute(conversionRequest);

            return GetAllRecord(service, conversionResponse.Query);
        }
    }
}
