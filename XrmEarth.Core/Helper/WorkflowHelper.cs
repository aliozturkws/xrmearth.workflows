using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using XrmEarth.Data.Const;
using XrmEarth.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XrmEarth.Core
{
    public class WorkflowHelper
    {
        public static string GetAppRecordUrl(IOrganizationService service, string recordUrl, string appModuleUniqueName)
        {
            EntityCollection appmodules = CrmHelper.GetAppModules(service, appModuleUniqueName);

            if (appmodules.Entities.Count > 0)
            {
                string appModuleId = appmodules.Entities.First()["appmoduleid"].ToString();
                return recordUrl + "&appid=" + appModuleId;
            }
            return string.Empty;
        }

        public static int RetrieveUserUILanguageCode(IOrganizationService service, Guid userId)
        {
            var userSettingsQuery = new QueryExpression(EntityNames.UserSettings);
            userSettingsQuery.ColumnSet.AddColumns("uilanguageid", "systemuserid");
            userSettingsQuery.Criteria.AddCondition("systemuserid", ConditionOperator.Equal, userId);
            var userSettings = service.RetrieveMultiple(userSettingsQuery);
            if (userSettings.Entities.Count > 0)
            {
                return (int)userSettings.Entities[0]["uilanguageid"];
            }
            return -1;
        }

        public static bool CheckUserInRole(IOrganizationService service, Guid systemUserId, string roleName)
        {
            var query = new QueryExpression(EntityNames.SystemUserRoles);
            query.ColumnSet.AllColumns = true;
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition("systemuserid", ConditionOperator.Equal, systemUserId);
            query.AddLink(EntityNames.Role, "roleid", "roleid").
                    LinkCriteria.AddCondition("name", ConditionOperator.Equal, roleName);

            var result = service.RetrieveMultiple(query);

            if (result != null && result.Entities.Count > 0)
                return true;
            else
                return false;
        }

        public static bool CheckUserInTeam(IOrganizationService service, Guid userId, EntityReference team)
        {
            var query = new QueryExpression(EntityNames.TeamMembership);
            query.ColumnSet.AllColumns = true;
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition("teamid", ConditionOperator.Equal, team.Id);
            query.Criteria.AddCondition("systemuserid", ConditionOperator.Equal, userId);

            var result = service.RetrieveMultiple(query);

            if (result != null && result.Entities.Count > 0)
                return true;
            else
                return false;
        }

        public static bool CheckTeamInRole(IOrganizationService service, Guid teamId, string roleName)
        {
            var query = new QueryExpression(EntityNames.TeamRoles);
            query.ColumnSet.AllColumns = true;
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition("teamid", ConditionOperator.Equal, teamId);
            query.AddLink(EntityNames.Role, "roleid", "roleid").
                    LinkCriteria.AddCondition("name", ConditionOperator.Equal, roleName);

            var result = service.RetrieveMultiple(query);

            if (result != null && result.Entities.Count > 0)
                return true;
            else
                return false;
        }

        public static void RevokePrivileges(IOrganizationService service, EntityReference target, EntityReference user)
        {
            var revokeRequest = new RevokeAccessRequest
            {
                Revokee = user,
                Target = target
            };
            service.Execute(revokeRequest);
        }

        public static void CloneEntity(IOrganizationService service, string recordUrl)
        {
            var id = RecordUrlHelper.GetIdByRecordUrl(recordUrl);
            var etc = RecordUrlHelper.GetEtcByRecordUrl(recordUrl);
            var logicalName = CrmHelper.GetEntityLogicalName(service, etc);
            var entity = service.Retrieve(logicalName, id, new ColumnSet(true));

            if (entity != null && entity.Id != Guid.Empty)
            {
                entity.Id = Guid.Empty;
                service.Create(entity);
            }
        }

        public static void AddUserToTeam(IOrganizationService service, Guid teamId, Guid userId)
        {
            var addRequest = new AddMembersTeamRequest();
            addRequest.TeamId = teamId;
            addRequest.MemberIds = new Guid[] { userId }; ;
            service.Execute(addRequest);
        }

        public static void RemoveUserFromTeam(IOrganizationService service, Guid teamId, Guid userId)
        {
            var addRequest = new RemoveMembersTeamRequest();
            addRequest.TeamId = teamId;
            addRequest.MemberIds = new Guid[] { userId };
            service.Execute(addRequest);
        }

        public static void SetState(IOrganizationService service, int stateCode, int statusCode, string recordUrl)
        {
            var id = RecordUrlHelper.GetIdByRecordUrl(recordUrl);
            var etc = RecordUrlHelper.GetEtcByRecordUrl(recordUrl);
            var logicalName = CrmHelper.GetEntityLogicalName(service, etc);

            CrmHelper.SetState(service, logicalName, id, stateCode, statusCode);
        }

        public static void ExecuteWorkflow(IOrganizationService service, Guid workflowId, string recordUrl)
        {
            var id = RecordUrlHelper.GetIdByRecordUrl(recordUrl);

            var request = new ExecuteWorkflowRequest()
            {
                WorkflowId = workflowId,
                EntityId = id
            };
            var response = (ExecuteWorkflowResponse)service.Execute(request);
        }

        public static double DateDifference(DateTime date1, DateTime date2, string dateInterval)
        {
            bool checkInterval = false;
            double result = 0;
            foreach (DateTimeHelper.DateInterval item in Enum.GetValues(typeof(DateTimeHelper.DateInterval)))
            {
                if (item.ToString() == dateInterval)
                {
                    checkInterval = true;
                    result = DateTimeHelper.DateDiff(item, date1, date2);
                }
            }

            if (!checkInterval)
                throw new Exception("Error Value : DateInterval!");

            return result;
        }

        public static int UpdateChildRecord(IOrganizationService service,
                                             IWorkflowContext context,
                                             string relationEntityName,
                                             string relationEntityFieldName,
                                             string relationEntityUpdatedFieldName,
                                             string valueToSet,
                                             string valueToSetLogicalName,
                                             string mapFieldName)
        {
            if ((!string.IsNullOrEmpty(valueToSet) && !string.IsNullOrEmpty(mapFieldName))
                ||
                (string.IsNullOrEmpty(valueToSet) && string.IsNullOrEmpty(mapFieldName)))
            {
                throw new InvalidPluginExecutionException("Value To Set OR Map Field Name Not Null!");
            }

            var relationEntityQuery = new QueryExpression(relationEntityName);
            relationEntityQuery.ColumnSet.AddColumn(relationEntityUpdatedFieldName);
            relationEntityQuery.Criteria.AddCondition(relationEntityFieldName, ConditionOperator.Equal, context.PrimaryEntityId);
            var relationEntityList = service.RetrieveMultiple(relationEntityQuery);

            var attributeType = CrmHelper.GetAttributeType(service, relationEntityName, relationEntityUpdatedFieldName);
            var processCount = 0;

            foreach (var entity in relationEntityList.Entities)
            {
                if (!string.IsNullOrEmpty(valueToSet))
                {
                    if (attributeType == AttributeTypeCode.Lookup || attributeType == AttributeTypeCode.Customer)
                    {
                        entity[relationEntityUpdatedFieldName] = new EntityReference(valueToSetLogicalName, ConvertHelper.CustomConvert<Guid>(valueToSet));
                    }
                    else if (attributeType == AttributeTypeCode.Picklist)
                    {
                        entity[relationEntityUpdatedFieldName] = new OptionSetValue(ConvertHelper.CustomConvert<int>(valueToSet));
                    }
                    else if (attributeType == AttributeTypeCode.String)
                    {
                        entity[relationEntityUpdatedFieldName] = valueToSet;
                    }
                    else if (attributeType == AttributeTypeCode.Integer)
                    {
                        entity[relationEntityUpdatedFieldName] = ConvertHelper.CustomConvert<int>(valueToSet);
                    }
                    else if (attributeType == AttributeTypeCode.Decimal)
                    {
                        entity[relationEntityUpdatedFieldName] = ConvertHelper.CustomConvert<decimal>(valueToSet);
                    }
                    else if (attributeType == AttributeTypeCode.Money)
                    {
                        entity[relationEntityUpdatedFieldName] = new Money(ConvertHelper.CustomConvert<decimal>(valueToSet));
                    }
                    else if (attributeType == AttributeTypeCode.Boolean)
                    {
                        entity[relationEntityUpdatedFieldName] = ConvertHelper.CustomConvert<bool>(valueToSet);
                    }
                    else if (attributeType == AttributeTypeCode.Status)
                    {
                        var statusCode = ConvertHelper.CustomConvert<int>(valueToSet);
                        var stateCode = CrmHelper.GetStateCode(service, relationEntityName, statusCode);
                        CrmHelper.SetState(service, entity.LogicalName, entity.Id, stateCode, statusCode);
                        processCount++;
                    }
                }
                else
                {
                    var result = service.Retrieve(context.PrimaryEntityName, context.PrimaryEntityId, new ColumnSet(mapFieldName));

                    if (result != null && result.Contains(mapFieldName))
                    {
                        entity[relationEntityUpdatedFieldName] = result[mapFieldName];
                    }
                }

                if (attributeType != AttributeTypeCode.Status)
                {
                    service.Update(entity);
                    processCount++;
                }
            }

            return processCount;
        }

        public static void UpdateEntity(IOrganizationService service,
            IWorkflowContext context,
            string recordUrl,
            string entityName,
            string entityId,
            string updateFieldName,
            string valueToSet,
            string valueToSetLogicalName)
        {
            if (string.IsNullOrEmpty(entityId) && string.IsNullOrEmpty(entityName) && string.IsNullOrEmpty(recordUrl))
            {
                throw new Exception("EntityId and EntityName, Or RecordUrl Can Not Be Null");
            }

            if (!string.IsNullOrEmpty(recordUrl))
            {
                entityId = RecordUrlHelper.GetIdByRecordUrl(recordUrl).ToString();
                var entityEtc = RecordUrlHelper.GetEtcByRecordUrl(recordUrl);
                entityName = CrmHelper.GetEntityLogicalName(service, entityEtc);
            }

            var entity = new Entity(entityName, ConvertHelper.CustomConvert<Guid>(entityId));

            var attributeType = CrmHelper.GetAttributeType(service, entityName, updateFieldName);

            if (attributeType == AttributeTypeCode.Lookup || attributeType == AttributeTypeCode.Customer)
            {
                entity[updateFieldName] = new EntityReference(valueToSetLogicalName, ConvertHelper.CustomConvert<Guid>(valueToSet));
            }
            else if (attributeType == AttributeTypeCode.Picklist)
            {
                entity[updateFieldName] = new OptionSetValue(ConvertHelper.CustomConvert<int>(valueToSet));
            }
            else if (attributeType == AttributeTypeCode.String)
            {
                entity[updateFieldName] = valueToSet;
            }
            else if (attributeType == AttributeTypeCode.Integer)
            {
                entity[updateFieldName] = ConvertHelper.CustomConvert<int>(valueToSet);
            }
            else if (attributeType == AttributeTypeCode.Decimal)
            {
                entity[updateFieldName] = ConvertHelper.CustomConvert<decimal>(valueToSet);
            }
            else if (attributeType == AttributeTypeCode.Money)
            {
                entity[updateFieldName] = new Money(ConvertHelper.CustomConvert<decimal>(valueToSet));
            }
            else if (attributeType == AttributeTypeCode.Boolean)
            {
                entity[updateFieldName] = ConvertHelper.CustomConvert<bool>(valueToSet);
            }
            else if (attributeType == AttributeTypeCode.Status)
            {
                var statusCode = ConvertHelper.CustomConvert<int>(valueToSet);
                var stateCode = CrmHelper.GetStateCode(service, context.PrimaryEntityName, statusCode);
                CrmHelper.SetState(service, context.PrimaryEntityName, context.PrimaryEntityId, stateCode, statusCode);
            }

            if (attributeType != AttributeTypeCode.Status)
            {
                service.Update(entity);
            }
        }

        public static void SharePrivileges(IOrganizationService service,
                                           EntityReference target,
                                           EntityReference principal,
                                           bool read, bool write, bool append, bool delete, bool assign, bool share)
        {
            AccessRights accessRights = AccessRights.None;

            if (read)
                accessRights = AccessRights.ReadAccess;

            if (write)
                if (accessRights == AccessRights.None)
                    accessRights = AccessRights.WriteAccess;
                else
                    accessRights = accessRights | AccessRights.WriteAccess;

            if (append)
                if (accessRights == AccessRights.None)
                    accessRights = AccessRights.AppendToAccess | AccessRights.AppendAccess;
                else
                    accessRights = accessRights | AccessRights.AppendToAccess | AccessRights.AppendAccess;

            if (delete)
                if (accessRights == AccessRights.None)
                    accessRights = AccessRights.DeleteAccess;
                else
                    accessRights = accessRights | AccessRights.DeleteAccess;

            if (assign)
                if (accessRights == AccessRights.None)
                    accessRights = AccessRights.ReadAccess | AccessRights.AssignAccess;
                else
                    accessRights = accessRights | AccessRights.AssignAccess;

            if (share)
                if (accessRights == AccessRights.None)
                    accessRights = AccessRights.ReadAccess | AccessRights.ShareAccess;
                else
                    accessRights = accessRights | AccessRights.ShareAccess;

            var grantAccess = new GrantAccessRequest
            {
                PrincipalAccess = new PrincipalAccess
                {
                    AccessMask = accessRights,
                    Principal = principal
                },
                Target = target
            };
            service.Execute(grantAccess);
        }

        public static void Associate(IOrganizationService service, string targetRecordUrl, string relatedRecordUrl, string relationShipName, string relationShipEntityName)
        {
            var relatedEntityId = RecordUrlHelper.GetIdByRecordUrl(relatedRecordUrl);
            var relatedEntityEtc = RecordUrlHelper.GetEtcByRecordUrl(relatedRecordUrl);
            var relatedEntityName = CrmHelper.GetEntityLogicalName(service, relatedEntityEtc);

            var targetEntityId = RecordUrlHelper.GetIdByRecordUrl(targetRecordUrl);
            var targetEntityEtc = RecordUrlHelper.GetEtcByRecordUrl(targetRecordUrl);
            var targetEntityName = CrmHelper.GetEntityLogicalName(service, targetEntityEtc);

            if (!CrmHelper.CheckAssociate(service, relationShipEntityName, targetEntityId, targetEntityName, relatedEntityId, relatedEntityName))
            {
                CrmHelper.Associate(service, targetEntityId, targetEntityName, relatedEntityId, relatedEntityName, relationShipName);
            }
        }

        public static void Diassociate(IOrganizationService service, string targetRecordUrl, string relatedRecordUrl, string relationShipName, string relationShipEntityName)
        {
            var relatedEntityId = RecordUrlHelper.GetIdByRecordUrl(relatedRecordUrl);
            var relatedEntityEtc = RecordUrlHelper.GetEtcByRecordUrl(relatedRecordUrl);
            var relatedEntityName = CrmHelper.GetEntityLogicalName(service, relatedEntityEtc);

            var targetEntityId = RecordUrlHelper.GetIdByRecordUrl(targetRecordUrl);
            var targetEntityEtc = RecordUrlHelper.GetEtcByRecordUrl(targetRecordUrl);
            var targetEntityName = CrmHelper.GetEntityLogicalName(service, targetEntityEtc);

            if (CrmHelper.CheckAssociate(service, relationShipEntityName, targetEntityId, targetEntityName, relatedEntityId, relatedEntityName))
            {
                CrmHelper.Disassociate(service, targetEntityId, targetEntityName, relatedEntityId, relatedEntityName, relationShipName);
            }
        }

        public static bool CheckAssociate(IOrganizationService service, string targetRecordUrl, string relatedRecordUrl, string relationShipName, string relationShipEntityName)
        {
            var relatedEntityId = RecordUrlHelper.GetIdByRecordUrl(relatedRecordUrl);
            var relatedEntityEtc = RecordUrlHelper.GetEtcByRecordUrl(relatedRecordUrl);
            var relatedEntityName = CrmHelper.GetEntityLogicalName(service, relatedEntityEtc);

            var targetEntityId = RecordUrlHelper.GetIdByRecordUrl(targetRecordUrl);
            var targetEntityEtc = RecordUrlHelper.GetEtcByRecordUrl(targetRecordUrl);
            var targetEntityName = CrmHelper.GetEntityLogicalName(service, targetEntityEtc);

            return CrmHelper.CheckAssociate(service, relationShipEntityName, targetEntityId, targetEntityName, relatedEntityId, relatedEntityName);
        }

        public static RollupFunctionsResponse RollupFunctions(IOrganizationService service, string fetchXML, string recordUrl, string recordUrl2 = null)
        {
            Guid entityId2 = Guid.Empty;

            if (!string.IsNullOrEmpty(recordUrl2))
            {
                entityId2 = RecordUrlHelper.GetIdByRecordUrl(recordUrl2);

                if (fetchXML.Contains("RecordId2"))
                    fetchXML = fetchXML.Replace("RecordId2", entityId2.ToString());
            }

            var entityId = RecordUrlHelper.GetIdByRecordUrl(recordUrl);

            if (fetchXML.Contains("RecordId"))
                fetchXML = fetchXML.Replace("RecordId", entityId.ToString());

            var fetchExpriession = new FetchExpression(fetchXML);

            var crmResult = service.RetrieveMultiple(fetchExpriession);

            var response = new RollupFunctionsResponse();

            if (crmResult != null && crmResult.Entities.Count > 0)
            {
                var firstAttributes = crmResult[0].Attributes;

                if (firstAttributes.Contains("max"))
                    response.Max = ConvertHelper.ConvertRollupFieldToDecimal(((AliasedValue)firstAttributes["max"]).Value);
                if (firstAttributes.Contains("min"))
                    response.Min = ConvertHelper.ConvertRollupFieldToDecimal(((AliasedValue)firstAttributes["min"]).Value);
                if (firstAttributes.Contains("avg"))
                    response.Avg = ConvertHelper.ConvertRollupFieldToDecimal(((AliasedValue)firstAttributes["avg"]).Value);
                if (firstAttributes.Contains("sum"))
                    response.Sum = ConvertHelper.ConvertRollupFieldToDecimal(((AliasedValue)firstAttributes["sum"]).Value);
                if (firstAttributes.Contains("count"))
                    response.Count = ConvertHelper.ConvertRollupFieldToDecimal(((AliasedValue)firstAttributes["count"]).Value);
            }

            return response;
        }

        public static void AddRoleToUser(IOrganizationService service, Guid systemUserId, string roleName)
        {
            var businessUnitId = CrmHelper.GetBusinessUnitId(service, EntityNames.SystemUser, systemUserId);

            var roleId = CrmHelper.GetRoleId(service, roleName, businessUnitId);

            if (!CrmHelper.CheckAssociate(service, EntityNames.SystemUserRoles, systemUserId, EntityNames.SystemUser, roleId, EntityNames.Role))
            {
                CrmHelper.Associate(service, systemUserId, EntityNames.SystemUser, roleId, EntityNames.Role, RelationShipNames.systemuserroles);
            }
        }

        public static void AddRoleToTeam(IOrganizationService service, Guid teamId, string roleName)
        {
            var businessUnitId = CrmHelper.GetBusinessUnitId(service, EntityNames.Team, teamId);

            var roleId = CrmHelper.GetRoleId(service, roleName, businessUnitId);

            if (!CrmHelper.CheckAssociate(service, EntityNames.TeamRoles, teamId, EntityNames.Team, roleId, EntityNames.Role))
            {
                CrmHelper.Associate(service, teamId, EntityNames.Team, roleId, EntityNames.Role, RelationShipNames.teamroles);
            }
        }

        public static void RemoveRoleFromUser(IOrganizationService service, Guid systemUserId, string roleName)
        {
            var businessUnitId = CrmHelper.GetBusinessUnitId(service, EntityNames.SystemUser, systemUserId);

            var roleId = CrmHelper.GetRoleId(service, roleName, businessUnitId);

            if (CrmHelper.CheckAssociate(service, EntityNames.SystemUserRoles, systemUserId, EntityNames.SystemUser, roleId, EntityNames.Role))
            {
                CrmHelper.Disassociate(service, systemUserId, EntityNames.SystemUser, roleId, EntityNames.Role, RelationShipNames.systemuserroles);
            }
        }

        public static void RemoveRoleFromTeam(IOrganizationService service, Guid teamId, string roleName)
        {
            var businessUnitId = CrmHelper.GetBusinessUnitId(service, EntityNames.Team, teamId);

            var roleId = CrmHelper.GetRoleId(service, roleName, businessUnitId);

            if (CrmHelper.CheckAssociate(service, EntityNames.TeamRoles, teamId, EntityNames.SystemUser, roleId, EntityNames.Role))
            {
                CrmHelper.Disassociate(service, teamId, EntityNames.Team, roleId, EntityNames.Role, RelationShipNames.teamroles);
            }
        }

        public static GeoCodeResponse GetGeoCode(string name, string address, string apiKey)
        {
            if (name == "google")
                return GetGeoCodeByGoogle(address, apiKey);
            else if (name == "bing")
                return GetGeoCodeByBing(address, apiKey);
            else
                throw new Exception("GeoCode Name Not Exist!");
        }

        private static GeoCodeResponse GetGeoCodeByGoogle(string address, string apiKey)
        {
            var response = new GeoCodeResponse();
            var requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key={1}", Uri.EscapeDataString(address), apiKey);
            var xDoc = XDocument.Load(requestUri);
            if (xDoc != null)
            {
                var result = xDoc.Element("GeocodeResponse").Element("result");
                if (result != null)
                {
                    var locationElement = result.Element("geometry").Element("location");
                    if (locationElement != null)
                    {
                        response.Latitude = Convert.ToDouble(locationElement.Element("lat").Value);
                        response.Longitude = Convert.ToDouble(locationElement.Element("lng").Value);
                    }
                }
            }
            return response;
        }

        private static GeoCodeResponse GetGeoCodeByBing(string address, string apiKey)
        {
            var response = new GeoCodeResponse();
            var requestUri = string.Format("https://dev.virtualearth.net/REST/v1/Locations?o=xml&q={0}&key={1}", Uri.EscapeDataString(address), apiKey);
            var xDoc = XDocument.Load(requestUri);
            if (xDoc != null)
            {
                var ns = xDoc.Root.GetDefaultNamespace();
                var lat = xDoc.Descendants(ns + "GeocodePoint").Elements(ns + "Latitude").FirstOrDefault();
                var lng = xDoc.Descendants(ns + "GeocodePoint").Elements(ns + "Longitude").FirstOrDefault();
                if (lat != null)
                    response.Latitude = Convert.ToDouble(lat.Value);
                if (lng != null)
                    response.Longitude = Convert.ToDouble(lng.Value);
            }
            return response;
        }

        public static void QualifyLead(IOrganizationService service, Guid leadId, int leadStatus, bool createAccount, bool createContact, bool createOpportunity, EntityReference account, EntityReference transactionCurrency)
        {
            var request = new QualifyLeadRequest();
            request.CreateAccount = createAccount;
            request.CreateContact = createContact;
            request.CreateOpportunity = createOpportunity;
            request.LeadId = new EntityReference(EntityNames.Lead, leadId);
            request.Status = new OptionSetValue(leadStatus);

            if (request.CreateOpportunity)
            {
                if (transactionCurrency != null && transactionCurrency.Id != Guid.Empty)
                    request.OpportunityCurrencyId = transactionCurrency;
                else
                    throw new InvalidPluginExecutionException("Transaction Currency Id Not Exist!");

                if (account != null && account.Id != Guid.Empty)
                    request.OpportunityCustomerId = account;
            }

            var response = (QualifyLeadResponse)service.Execute(request);
        }

        public static void SetProcessStage(IOrganizationService service, EntityReference process, string recordUrl, string processStage)
        {
            var recordEntityReference = RecordUrlHelper.GetEntityReference(recordUrl, service);

            var queryStage = new QueryExpression(EntityNames.ProcessStage);
            queryStage.ColumnSet = new ColumnSet();
            queryStage.Criteria.AddCondition(new ConditionExpression(
                "stagename",
                ConditionOperator.Equal,
                processStage));

            queryStage.Criteria.AddCondition(new ConditionExpression(
                "processid",
                ConditionOperator.Equal,
                process.Id));

            var stageReference = service.RetrieveMultiple(queryStage).Entities.FirstOrDefault();
            if (stageReference == null)
                throw new InvalidPluginExecutionException("Process Stage " + processStage + " Not Found!");

            var entity = new Entity(recordEntityReference.LogicalName, recordEntityReference.Id);
            entity["processid"] = process.Id;
            entity["stageid"] = stageReference.Id;
            service.Update(entity);
        }

        public static string GetNextProcessStage(IOrganizationService service, EntityReference process, string processStage)
        {
            string nextProcessStage = string.Empty;

            var processStageList = CrmHelper.GetProcessStageList(service, process);

            if (processStageList.Entities.Count > 0)
            {
                var processStageEntity = processStageList.Entities.FirstOrDefault(x => x.Contains("stagename") && x["stagename"].ToString() == processStage);

                if (processStageEntity != null)
                {
                    var processStageCategory = processStageEntity.Contains("stagecategory") ? ((OptionSetValue)processStageEntity["stagecategory"]).Value : -1;

                    if (processStageCategory > -1)
                    {
                        foreach (var entity in processStageList.Entities)
                        {
                            if (entity.Contains("stagecategory") && ((OptionSetValue)entity["stagecategory"]).Value > processStageCategory)
                            {
                                nextProcessStage = entity.Contains("stagename") ? entity["stagename"].ToString() : string.Empty;
                                break;
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException(processStage + " Process StageCategory Not Found!");
                    }
                }
                else
                {
                    throw new InvalidPluginExecutionException(processStage + " Process StageName Not Found!");
                }
            }

            return nextProcessStage;
        }

        public static string GetPreviousProcessStage(IOrganizationService service, EntityReference process, string processStage)
        {
            string nextProcessStage = string.Empty;

            var processStageList = CrmHelper.GetProcessStageList(service, process);

            if (processStageList.Entities.Count > 0)
            {
                var processStageEntity = processStageList.Entities.FirstOrDefault(x => x.Contains("stagename") && x["stagename"].ToString() == processStage);

                if (processStageEntity != null)
                {
                    var processStageCategory = processStageEntity.Contains("stagecategory") ? ((OptionSetValue)processStageEntity["stagecategory"]).Value : -1;

                    if (processStageCategory > -1)
                    {
                        foreach (var entity in processStageList.Entities)
                        {
                            if (entity.Contains("stagecategory") && ((OptionSetValue)entity["stagecategory"]).Value < processStageCategory)
                            {
                                nextProcessStage = entity.Contains("stagename") ? entity["stagename"].ToString() : string.Empty;
                                break;
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException(processStage + " Process StageCategory Not Found!");
                    }
                }
                else
                {
                    throw new InvalidPluginExecutionException(processStage + " Process StageName Not Found!");
                }
            }

            return nextProcessStage;
        }

        public static bool SendEmailToUsersInRole(IOrganizationService service, EntityReference securityEntityReference, EntityReference emailEntityReference)
        {
            var userList = service.RetrieveMultiple(new FetchExpression(CrmHelper.BuildFetchXml(securityEntityReference.Id)));

            var emailEnt = new Entity(EntityNames.Email, emailEntityReference.Id);

            var to = new EntityCollection();

            foreach (Entity user in userList.Entities)
            {
                var userId = user.Id;
                Entity to1 = new Entity("activityparty");
                to1["partyid"] = new EntityReference("systemuser", userId);
                to.Entities.Add(to1);
            }

            emailEnt["to"] = to;
            service.Update(emailEnt);

            var req = new SendEmailRequest();
            req.EmailId = emailEntityReference.Id;

            var res = (SendEmailResponse)service.Execute(req);
            return true;
        }

        public static bool SendEmailFromTemplateToUsersInRole(IOrganizationService service, EntityReference securityRoleLookup, EntityReference emailTemplateLookup)
        {
            var userList = service.RetrieveMultiple(new FetchExpression(CrmHelper.BuildFetchXml(securityRoleLookup.Id)));

            foreach (var user in userList.Entities)
            {
                try
                {
                    bool sent = SendEmailFromTemplate(service, emailTemplateLookup, user.Id);
                }
                catch
                {
                    // ignored
                }
            }
            return true;
        }

        public static bool SendEmailFromTemplate(IOrganizationService service, EntityReference template, Guid userId)
        {
            var toEntities = new List<Entity>();
            var activityParty = new Entity();
            activityParty.LogicalName = EntityNames.ActivityParty;
            activityParty.Attributes["partyid"] = new EntityReference(EntityNames.SystemUser, userId);
            toEntities.Add(activityParty);

            var email = new Entity(EntityNames.Email);
            email.Attributes["to"] = toEntities.ToArray();

            var emailUsingTemplateReq = new SendEmailFromTemplateRequest
            {
                Target = email,
                TemplateId = template.Id,
                RegardingId = userId,
                RegardingType = EntityNames.SystemUser
            };

            var emailUsingTemplateResp = (SendEmailFromTemplateResponse)service.Execute(emailUsingTemplateReq);

            return true;
        }
    }
}
