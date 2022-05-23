using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Globalization;
using XrmEarth.Core.Activity;
using XrmEarth.Core.NumberToText;

namespace XrmEarth.Workflows.String
{
    public class DecimalToMoneyText : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var language = Language.Get<string>(activityHelper.CodeActivityContext);
            var currency = Currency.Get<string>(activityHelper.CodeActivityContext);
            var decimalValue = DecimalValue.Get<decimal>(activityHelper.CodeActivityContext);
            var digit = Digit.Get<int>(activityHelper.CodeActivityContext);

            var options = new Options
            {
                MainUnitNotConvertedToText = MainUnitNotConvertedToText.Get<bool>(activityHelper.CodeActivityContext),
                SubUnitNotConvertedToText = SubUnitNotConvertedToText.Get<bool>(activityHelper.CodeActivityContext),
                MainUnitFirstCharUpper = MainUnitFirstCharUpper.Get<bool>(activityHelper.CodeActivityContext),
                SubUnitFirstCharUpper = SubUnitFirstCharUpper.Get<bool>(activityHelper.CodeActivityContext),
                CurrencyFirstCharUpper = CurrencyFirstCharUpper.Get<bool>(activityHelper.CodeActivityContext),
                SubUnitZeroNotDisplayed = SubUnitZeroNotDisplayed.Get<bool>(activityHelper.CodeActivityContext),
            };

            var sensitivityFormat = string.Empty;

            for (int i = 0; i < digit; i++)
                sensitivityFormat += "#";

            var formattedDecimalValue = decimalValue.ToString("0." + sensitivityFormat);

            var enCultureInfo = new CultureInfo("en-US");

            var result = Convert.ToDecimal(formattedDecimalValue, enCultureInfo).ToText(currency, language, options);

            Result.Set(activityHelper.CodeActivityContext, result);
        }

        [RequiredArgument]
        [Input("Decimal")]
        public InArgument<decimal> DecimalValue { get; set; }

        [RequiredArgument]
        [Input("Language")]
        public InArgument<string> Language { get; set; }

        [RequiredArgument]
        [Input("Currency")]
        public InArgument<string> Currency { get; set; }

        [RequiredArgument]
        [Input("Digit")]
        public InArgument<int> Digit { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }

        #region | Options |
        [RequiredArgument]
        [Input("Main Unit Not Converted To Text")]
        public InArgument<bool> MainUnitNotConvertedToText { get; set; }

        [RequiredArgument]
        [Input("Sub Unit Not Converted To Text")]
        public InArgument<bool> SubUnitNotConvertedToText { get; set; }

        [RequiredArgument]
        [Input("Main Unit First Char Upper")]
        public InArgument<bool> MainUnitFirstCharUpper { get; set; }

        [RequiredArgument]
        [Input("Sub Unit First Char Upper")]
        public InArgument<bool> SubUnitFirstCharUpper { get; set; }

        [RequiredArgument]
        [Input("Currency First Char Upper")]
        public InArgument<bool> CurrencyFirstCharUpper { get; set; }

        [RequiredArgument]
        [Input("Sub Unit Zero Not Displayed")]
        public InArgument<bool> SubUnitZeroNotDisplayed { get; set; }

        #endregion
    }
}
