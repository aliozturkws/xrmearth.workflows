using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Microsoft.Xrm.Sdk;

namespace XrmEarth.Core
{
    public class RecordUrlHelper
    {
        public static string ChangeEtcToEtnUrl(string recordUrl, IOrganizationService service)
        {
            if (!string.IsNullOrEmpty(recordUrl))
            {
                var _recordUrlArray = recordUrl.Split('?');
                if (_recordUrlArray != null && _recordUrlArray.Length > 0)
                {
                    string changeRecordUrl = _recordUrlArray[0] + "?";

                    NameValueCollection nvm = ParseQueryString(_recordUrlArray[1]);

                    foreach (var n in nvm.AllKeys)
                    {
                        if (n == "etc")
                        {
                            var entityReference = GetEntityReference(recordUrl, service);
                            changeRecordUrl += $"etn={entityReference.LogicalName}&";
                        }
                        else
                        {
                            changeRecordUrl += $"{n}={nvm[n]}&";
                        }
                    }

                    return changeRecordUrl.Remove(changeRecordUrl.Length - 1);
                }
                else
                {
                    throw new Exception("Can Not Be Null RecordUrl !");
                }
            }
            else
            {
                throw new Exception("Can Not Be Null RecordUrl !");
            }
        }

        public static Guid GetIdByRecordUrl(string recordUrl)
        {
            if (!string.IsNullOrEmpty(recordUrl))
            {
                var _recordUrlArray = recordUrl.Split('?');
                if (_recordUrlArray != null && _recordUrlArray.Length > 0)
                {
                    var nvm = ParseQueryString(_recordUrlArray[1]);
                    return new Guid(nvm["id"]);
                }
                else
                {
                    throw new Exception("Can Not Be Null RecordUrl !");
                }
            }
            else
            {
                throw new Exception("Can Not Be Null RecordUrl !");
            }
        }

        public static int GetEtcByRecordUrl(string recordUrl)
        {
            if (!string.IsNullOrEmpty(recordUrl))
            {
                var _recordUrlArray = recordUrl.Split('?');
                if (_recordUrlArray != null && _recordUrlArray.Length > 0)
                {
                    var nvm = ParseQueryString(_recordUrlArray[1]);
                    return Convert.ToInt32(nvm["etc"]);
                }
                else
                {
                    throw new Exception("Can Not Be Null RecordUrl !");
                }
            }
            else
            {
                throw new Exception("Can Not Be Null RecordUrl !");
            }
        }

        public static EntityReference GetEntityReference(string recordUrl, IOrganizationService service)
        {
            var entityId = GetIdByRecordUrl(recordUrl);
            var entityEtc = GetEtcByRecordUrl(recordUrl);
            var entityName = CrmHelper.GetEntityLogicalName(service, entityEtc);

            return new EntityReference(entityName, entityId);
        }

        public static NameValueCollection ParseQueryString(string s)
        {
            var nvc = new NameValueCollection();
            // remove anything other than query string from url
            if (s.Contains("?"))
            {
                s = s.Substring(s.IndexOf('?') + 1);
            }
            foreach (string vp in Regex.Split(s, "&"))
            {
                string[] singlePair = Regex.Split(vp, "=");
                if (singlePair.Length == 2)
                {
                    nvc.Add(singlePair[0], singlePair[1]);
                }
                else
                {
                    // only one key with no value specified in query string
                    nvc.Add(singlePair[0], string.Empty);
                }
            }
            return nvc;
        }
    }
}
