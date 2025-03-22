/*  CTRADER GURU --> Indicator Template 1.0.8

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/ctrader-guru

*/

using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System;

namespace cAlgo
{


    #region Extensions


    public static class SymbolExtensions
    {


        public static double DigitsToPips(this Symbol MySymbol, double Pips)
        {

            return Math.Round(Pips / MySymbol.PipSize, 2);

        }


        public static double PipsToDigits(this Symbol MySymbol, double Pips)
        {

            return Math.Round(Pips * MySymbol.PipSize, MySymbol.Digits);

        }

    }


    public static class BarsExtensions
    {


        public static int GetIndexByDate(this Bars MyBars, DateTime MyTime)
        {

            for (int i = MyBars.ClosePrices.Count - 1; i >= 0; i--)
            {

                if (MyTime == MyBars.OpenTimes[i])
                    return i;

            }

            return -1;

        }

    }

    #endregion

    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class MAColorCandles : Indicator
    {

        #region Enums

        public enum Mode
        {

            DeTrended,
            MovingAverage

        }

        #endregion

        #region Identity


        public const string NAME = "MA Color Candles";


        public const string VERSION = "1.7.3";

        #endregion

        #region Params


        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://www.google.com/search?q=ctrader+guru+ma+color+candles")]
        public string ProductInfo { get; set; }

        [Parameter("Indicator", Group = "Mode", DefaultValue = Mode.DeTrended)]
        public Mode ModeType { get; set; }

        [Parameter("Ignore Mid Period", Group = "Mode", DefaultValue = true)]
        public bool IgnoreMid { get; set; }

        [Parameter("Period", Group = "Params", DefaultValue = 21, MinValue = 3)]
        public int Period { get; set; }

        [Parameter("Smooth", Group = "Params", DefaultValue = 3, MinValue = 1)]
        public int Smooth { get; set; }

        [Parameter("MA Type", Group = "Params", DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MAType { get; set; }

        [Parameter("K% (zero = auto)", Group = "DeTrended", DefaultValue = 0, MinValue = 0, Step = 0.1)]
        public double K { get; set; }

        [Parameter("Auto Period (bars)", Group = "DeTrended", DefaultValue = 300, MinValue = 1, Step = 1)]
        public int AutoPeriod { get; set; }

        [Parameter("Bullish Color", Group = "Styles", DefaultValue = "LimeGreen")]
        public Color BullishColor { get; set; }

        [Parameter("Mid Bullish Color", Group = "Styles", DefaultValue = "LightGray")]
        public Color MidBullishColor { get; set; }

        [Parameter("Bearish Color", Group = "Styles", DefaultValue = "Red")]
        public Color BearishColor { get; set; }

        [Parameter("Mid Bearish Color", Group = "Styles", DefaultValue = "LightGray")]
        public Color MidBearishColor { get; set; }

        #endregion

        #region Property

        private DetrendedPriceOscillator DeTrended;
        private ExponentialMovingAverage EMASmooth;
        private MovingAverage MA;
        private ParabolicSAR SAR;
        private AverageTrueRange ATR;

        private Color LastColor;

        #endregion

        #region Indicator Events


        protected override void Initialize()
        {

            Print("{0} : {1}", NAME, VERSION);

            DeTrended = Indicators.DetrendedPriceOscillator(Bars.ClosePrices, Period, MAType);
            EMASmooth = Indicators.ExponentialMovingAverage(DeTrended.Result, Smooth);
            MA = Indicators.MovingAverage(Bars.ClosePrices, Period, MAType);
            SAR = Indicators.ParabolicSAR(0.02, 0.2);
            ATR = Indicators.AverageTrueRange(Period, MAType);

            K = Symbol.PipsToDigits(K);

            LastColor = Color.Gray;

        }

        public override void Calculate(int index)
        {

            double open = Bars.OpenPrices[index];
            double high = Bars.HighPrices[index];
            double low = Bars.LowPrices[index];
            double close = Bars.ClosePrices[index];

            Color color;

            switch (ModeType)
            {

                case Mode.MovingAverage:

                    double MyMA = MA.Result[index];

                    if (MyMA < close)
                    {

                        color = (close > SAR.Result.LastValue) ? BullishColor : (IgnoreMid) ? LastColor : MidBullishColor;

                    }
                    else
                    {

                        color = (close < SAR.Result.LastValue) ? BearishColor : (IgnoreMid) ? LastColor : MidBearishColor;

                    }

                    break;
                default:


                    double MyDeTrended = EMASmooth.Result[index];
                    double KK = (K > 0) ? K : ATR.Result.Maximum(AutoPeriod);

                    if (MyDeTrended < 0)
                    {

                        color = (MyDeTrended < -KK) ? BearishColor : (IgnoreMid) ? LastColor : MidBearishColor;

                    }
                    else
                    {

                        color = (MyDeTrended > KK) ? BullishColor : (IgnoreMid) ? LastColor : MidBullishColor;

                    }

                    break;

            }

            LastColor = color;

            Chart.SetBarColor(index, color);

        }

        #endregion

    }

}
