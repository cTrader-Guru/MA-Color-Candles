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

    // --> Estensioni che rendono il codice più leggibile
    #region Extensions

    /// <summary>
    /// Estensione che fornisce metodi aggiuntivi per il simbolo
    /// </summary>
    public static class SymbolExtensions
    {

        /// <summary>
        /// Converte il numero di pips corrente da digits a double
        /// </summary>
        /// <param name="Pips">Il numero di pips nel formato Digits</param>
        /// <returns></returns>
        public static double DigitsToPips(this Symbol MySymbol, double Pips)
        {

            return Math.Round(Pips / MySymbol.PipSize, 2);

        }

        /// <summary>
        /// Converte il numero di pips corrente da double a digits
        /// </summary>
        /// <param name="Pips">Il numero di pips nel formato Double (2)</param>
        /// <returns></returns>
        public static double PipsToDigits(this Symbol MySymbol, double Pips)
        {

            return Math.Round(Pips * MySymbol.PipSize, MySymbol.Digits);

        }

    }

    /// <summary>
    /// Estensione che fornisce metodi aggiuntivi per le Bars
    /// </summary>
    public static class BarsExtensions
    {

        /// <summary>
        /// Converte l'indice di una bar partendo dalla data di apertura
        /// </summary>
        /// <param name="MyTime">La data e l'ora di apertura della candela</param>
        /// <returns></returns>
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

        public enum MyColors
        {

            AliceBlue,
            AntiqueWhite,
            Aqua,
            Aquamarine,
            Azure,
            Beige,
            Bisque,
            Black,
            BlanchedAlmond,
            Blue,
            BlueViolet,
            Brown,
            BurlyWood,
            CadetBlue,
            Chartreuse,
            Chocolate,
            Coral,
            CornflowerBlue,
            Cornsilk,
            Crimson,
            Cyan,
            DarkBlue,
            DarkCyan,
            DarkGoldenrod,
            DarkGray,
            DarkGreen,
            DarkKhaki,
            DarkMagenta,
            DarkOliveGreen,
            DarkOrange,
            DarkOrchid,
            DarkRed,
            DarkSalmon,
            DarkSeaGreen,
            DarkSlateBlue,
            DarkSlateGray,
            DarkTurquoise,
            DarkViolet,
            DeepPink,
            DeepSkyBlue,
            DimGray,
            DodgerBlue,
            Firebrick,
            FloralWhite,
            ForestGreen,
            Fuchsia,
            Gainsboro,
            GhostWhite,
            Gold,
            Goldenrod,
            Gray,
            Green,
            GreenYellow,
            Honeydew,
            HotPink,
            IndianRed,
            Indigo,
            Ivory,
            Khaki,
            Lavender,
            LavenderBlush,
            LawnGreen,
            LemonChiffon,
            LightBlue,
            LightCoral,
            LightCyan,
            LightGoldenrodYellow,
            LightGray,
            LightGreen,
            LightPink,
            LightSalmon,
            LightSeaGreen,
            LightSkyBlue,
            LightSlateGray,
            LightSteelBlue,
            LightYellow,
            Lime,
            LimeGreen,
            Linen,
            Magenta,
            Maroon,
            MediumAquamarine,
            MediumBlue,
            MediumOrchid,
            MediumPurple,
            MediumSeaGreen,
            MediumSlateBlue,
            MediumSpringGreen,
            MediumTurquoise,
            MediumVioletRed,
            MidnightBlue,
            MintCream,
            MistyRose,
            Moccasin,
            NavajoWhite,
            Navy,
            OldLace,
            Olive,
            OliveDrab,
            Orange,
            OrangeRed,
            Orchid,
            PaleGoldenrod,
            PaleGreen,
            PaleTurquoise,
            PaleVioletRed,
            PapayaWhip,
            PeachPuff,
            Peru,
            Pink,
            Plum,
            PowderBlue,
            Purple,
            Red,
            RosyBrown,
            RoyalBlue,
            SaddleBrown,
            Salmon,
            SandyBrown,
            SeaGreen,
            SeaShell,
            Sienna,
            Silver,
            SkyBlue,
            SlateBlue,
            SlateGray,
            Snow,
            SpringGreen,
            SteelBlue,
            Tan,
            Teal,
            Thistle,
            Tomato,
            Transparent,
            Turquoise,
            Violet,
            Wheat,
            White,
            WhiteSmoke,
            Yellow,
            YellowGreen

        }

        public enum Mode
        {

            DeTrended,
            MovingAverage

        }

        #endregion

        #region Identity

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "MA Color Candles";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.5";

        #endregion

        #region Params

        /// <summary>
        /// Identità del prodotto nel contesto di ctrader.guru
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/ma-color-candles/")]
        public string ProductInfo { get; set; }

        [Parameter("Indicator", Group = "Mode", DefaultValue = Mode.DeTrended)]
        public Mode ModeType { get; set; }

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

        [Parameter("Bullish Color", Group = "Styles", DefaultValue = MyColors.LimeGreen)]
        public MyColors BullishColor { get; set; }

        [Parameter("Mid Bullish Color", Group = "Styles", DefaultValue = MyColors.LightGray)]
        public MyColors MidBullishColor { get; set; }

        [Parameter("Bearish Color", Group = "Styles", DefaultValue = MyColors.Red)]
        public MyColors BearishColor { get; set; }

        [Parameter("Mid Bearish Color", Group = "Styles", DefaultValue = MyColors.LightGray)]
        public MyColors MidBearishColor { get; set; }

        #endregion

        #region Property

        private DetrendedPriceOscillator DeTrended;
        private ExponentialMovingAverage EMASmooth;
        private MovingAverage MA;
        private ParabolicSAR SAR;
        private AverageTrueRange ATR;

        private int CandleWidth;
        private int WickWidth;

        #endregion

        #region Indicator Events

        /// <summary>
        /// Viene generato all'avvio dell'indicatore, si inizializza l'indicatore
        /// </summary>
        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            DeTrended = Indicators.DetrendedPriceOscillator(Bars.ClosePrices, Period, MAType);
            EMASmooth = Indicators.ExponentialMovingAverage(DeTrended.Result, Smooth);
            MA = Indicators.MovingAverage(Bars.ClosePrices, Period, MAType);
            SAR = Indicators.ParabolicSAR(0.02, 0.2);
            ATR = Indicators.AverageTrueRange(Period, MAType);

            K = Symbol.PipsToDigits(K);

            // --> Inizializzo i parametri grafici
            _updateCandleSize();

            // --> Ridisegno tutte le candele ogni volta che cambia lo zoom
            Chart.ZoomChanged += _repaint;


        }

        /// <summary>
        /// Generato ad ogni tick, vengono effettuati i calcoli dell'indicatore
        /// </summary>
        /// <param name="index">L'indice della candela in elaborazione</param>
        public override void Calculate(int index)
        {

            double open = Bars.OpenPrices[index];
            double high = Bars.HighPrices[index];
            double low = Bars.LowPrices[index];
            double close = Bars.ClosePrices[index];

            MyColors color;

            switch (ModeType)
            {

                case Mode.MovingAverage:

                    double MyMA = MA.Result[index];

                    if (MyMA < close)
                    {

                        color = (close > SAR.Result.LastValue) ? BullishColor : MidBullishColor;

                    }
                    else
                    {

                        color = (close < SAR.Result.LastValue) ? BearishColor : MidBearishColor;

                    }

                    break;
                default:


                    double MyDeTrended = EMASmooth.Result[index];
                    double KK = (K > 0) ? K : ATR.Result.Maximum(AutoPeriod);

                    if (MyDeTrended < 0)
                    {

                        color = (MyDeTrended < -KK) ? BearishColor : MidBearishColor;

                    }
                    else
                    {

                        color = (MyDeTrended > KK) ? BullishColor : MidBullishColor;

                    }

                    break;

            }

            Chart.DrawTrendLine("candle" + index, index, open, index, close, Color.FromName(color.ToString("G")), CandleWidth, LineStyle.Solid);
            Chart.DrawTrendLine("line" + index, index, high, index, low, Color.FromName(color.ToString("G")), WickWidth, LineStyle.Solid);

        }

        #endregion

        #region Private Methods

        private void _repaint(ChartZoomEventArgs obj = null)
        {

            _updateCandleSize(obj);

            for (int i = 0; i < Bars.Count - 1; i++)
            {

                Calculate(i);

            }

        }

        private void _updateCandleSize(ChartZoomEventArgs obj = null)
        {

            int zoom = (obj == null) ? Chart.ZoomLevel : obj.Chart.ZoomLevel;

            if (zoom <= 5)
            {

                CandleWidth = 1;
                WickWidth = 1;

            }
            else if (zoom <= 10)
            {

                CandleWidth = 2;
                WickWidth = 1;

            }
            else if (zoom <= 30)
            {

                CandleWidth = 3;
                WickWidth = 1;

            }
            else if (zoom <= 40)
            {

                CandleWidth = 5;
                WickWidth = 2;

            }
            else if (zoom <= 60)
            {

                CandleWidth = 7;
                WickWidth = 2;

            }
            else if (zoom <= 75)
            {

                CandleWidth = 9;
                WickWidth = 2;

            }
            else if (zoom <= 90)
            {

                CandleWidth = 11;
                WickWidth = 3;

            }
            else if (zoom <= 105)
            {

                CandleWidth = 13;
                WickWidth = 3;

            }
            else if (zoom <= 120)
            {

                CandleWidth = 15;
                WickWidth = 3;

            }
            else if (zoom <= 150)
            {

                CandleWidth = 19;
                WickWidth = 4;

            }
            else
            {

                CandleWidth = 0;
                WickWidth = 0;

            }

        }

        #endregion

    }

}
