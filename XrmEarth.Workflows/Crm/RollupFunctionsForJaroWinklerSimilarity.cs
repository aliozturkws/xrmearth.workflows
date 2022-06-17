using Microsoft.Xrm.Sdk.Workflow;
using Newtonsoft.Json;
using System.Activities;
using System.Collections.Generic;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Model;

namespace XrmEarth.Workflows.Crm
{
    public class RollupFunctionsForJaroWinklerSimilarity : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var compareAttributeName = CompareAttributeName.Get<string>(activityHelper.CodeActivityContext);
            var compareValue = CompareValue.Get<string>(activityHelper.CodeActivityContext);
            var maxSimilarity = MaxSimilarity.Get<double>(activityHelper.CodeActivityContext);
            var recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);
            var recordUrl2 = RecordUrl2.Get<string>(activityHelper.CodeActivityContext);
            var inputText1 = InputText1.Get<string>(activityHelper.CodeActivityContext);
            var inputText2 = InputText2.Get<string>(activityHelper.CodeActivityContext);
            var fetchXml = FetchXML.Get<string>(activityHelper.CodeActivityContext);

            List<RollupFunctionsForJaroWinklerSimilarityModel> similarityList = JaroWinklerHelper.RollupFunctionsForJaroWinklerSimilarity(activityHelper.OrganizationService, compareAttributeName, compareValue, maxSimilarity, fetchXml, recordUrl, recordUrl2, inputText1, inputText2);

            if (similarityList.Count == 0)
            {
                IsSimilarity.Set(activityHelper.CodeActivityContext, false);
            }
            else if (similarityList.Count > 0)
            {
                IsSimilarity.Set(activityHelper.CodeActivityContext, true);
                SimilarityDetails.Set(activityHelper.CodeActivityContext, JsonConvert.SerializeObject(similarityList));
            }
        }

        [RequiredArgument]
        [Input("FetchXML")]
        public InArgument<string> FetchXML { get; set; }

        [RequiredArgument]
        [Input("Compare Attribute Name")]
        public InArgument<string> CompareAttributeName { get; set; }

        [RequiredArgument]
        [Input("Compare Value")]
        public InArgument<string> CompareValue { get; set; }

        [RequiredArgument]
        [Input("Max Similarity (Between 0 and 1)")]
        public InArgument<double> MaxSimilarity { get; set; }

        [Input("RecordUrl")]
        public InArgument<string> RecordUrl { get; set; }

        [Input("RecordUrl2")]
        public InArgument<string> RecordUrl2 { get; set; }

        [Input("InputText1")]
        public InArgument<string> InputText1 { get; set; }

        [Input("InputText2")]
        public InArgument<string> InputText2 { get; set; }

        [RequiredArgument]
        [Output("IsSimilarity")]
        public OutArgument<bool> IsSimilarity { get; set; }

        [Output("Similarity Details")]
        public OutArgument<string> SimilarityDetails { get; set; }
    }
}
