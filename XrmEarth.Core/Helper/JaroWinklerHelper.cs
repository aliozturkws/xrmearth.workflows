using F23.StringSimilarity;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using XrmEarth.Data.Model;

namespace XrmEarth.Core
{
    public class JaroWinklerHelper
    {
        public static double Similarity(string value1,string value2)
        {
            JaroWinkler jw = new JaroWinkler();

            return jw.Similarity(value1, value2);
        }

        public static List<RollupFunctionsForJaroWinklerSimilarityModel> RollupFunctionsForJaroWinklerSimilarity(IOrganizationService service, string compareAttributeName, string compareValue, double maxSimilarity, string fetchXml, string recordUrl, string recordUrl2, string inputText1, string inputText2)
        {
            var result = CrmHelper.GetRollupFunctionsForJaroWinklerSimilarity(service, fetchXml, recordUrl, recordUrl2, inputText1, inputText2);

            JaroWinkler jaroWinkler = new JaroWinkler();

            List<RollupFunctionsForJaroWinklerSimilarityModel> similarityList = new List<RollupFunctionsForJaroWinklerSimilarityModel>();

            foreach (var entity in result.Entities)
            {
                if (entity.Contains(compareAttributeName))
                {
                    string value = entity[compareAttributeName].ToString();

                    if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(compareValue))
                        continue;

                    double similarity = jaroWinkler.Similarity(value, compareValue);

                    if (similarity > maxSimilarity)
                    {
                        similarityList.Add(new RollupFunctionsForJaroWinklerSimilarityModel { Id = entity.Id, Similarity = similarity, Text = entity[compareAttributeName].ToString() });
                    }
                }
            }

            return similarityList;
        }
    }
}
