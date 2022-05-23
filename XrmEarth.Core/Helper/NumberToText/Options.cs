﻿namespace XrmEarth.Core.NumberToText
{
    public struct Options
    {
        public bool MainUnitNotConvertedToText { get; set; }
        public bool SubUnitNotConvertedToText { get; set; }
        public bool SubUnitZeroNotDisplayed { get; set; }
        public bool MainUnitFirstCharUpper { get; set; }
        public bool SubUnitFirstCharUpper { get; set; }
        public bool CurrencyFirstCharUpper { get; set; }
        public bool AddAndBetweenMainUnitAndSubUnits { get; set; }
        public bool UseShortenedUnits { get; set; }
    }
}
