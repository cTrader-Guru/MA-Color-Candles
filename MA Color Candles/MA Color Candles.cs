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

namespace cAlgo
{

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

        #endregion

        #region Identity

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "MA Color Candles";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.2";

        #endregion

        #region Params

        /// <summary>
        /// Identità del prodotto nel contesto di ctrader.guru
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/ma-color-candles/")]
        public string ProductInfo { get; set; }

        [Parameter("Periods", Group = "Params", DefaultValue = 24)]
        public int MAPeriod { get; set; }

        [Parameter("Moving Average", Group = "Params", DefaultValue = MovingAverageType.Weighted)]
        public MovingAverageType MAType { get; set; }

        [Parameter("Candle width", Group = "Styles", DefaultValue = 5)]
        public int CandleWidth { get; set; }

        [Parameter("Wick width", Group = "Styles", DefaultValue = 1)]
        public int WickWidth { get; set; }

        [Parameter("Flat color", Group = "Styles", DefaultValue = MyColors.Orange)]
        public MyColors FlatColor { get; set; }

        [Parameter("Above down color", Group = "Styles", DefaultValue = MyColors.LimeGreen)]
        public MyColors AboveDownColor { get; set; }

        [Parameter("Below down color", Group = "Styles", DefaultValue = MyColors.Red)]
        public MyColors BelowDownColor { get; set; }

        [Output("Moving Average", LineColor = "LightGray", LineStyle = LineStyle.Solid)]
        public IndicatorDataSeries Result { get; set; }

        #endregion

        #region Property

        private MovingAverage MA;
        private ParabolicSAR SAR;

        #endregion

        #region Indicator Events

        /// <summary>
        /// Viene generato all'avvio dell'indicatore, si inizializza l'indicatore
        /// </summary>
        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            MA = Indicators.MovingAverage(Bars.ClosePrices, MAPeriod, MAType);
            SAR = Indicators.ParabolicSAR(0.02, 0.2);

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

            double MyMA = MA.Result[index];

            Result[index] = MyMA;

            if (MyMA < close)
            {
                color = (close > SAR.Result.LastValue) ? AboveDownColor : FlatColor;
            }
            else
            {
                color = (close < SAR.Result.LastValue) ? BelowDownColor : FlatColor;

            }

            Chart.DrawTrendLine("candle" + index, index, open, index, close, Color.FromName( color.ToString( "G" ) ), CandleWidth, LineStyle.Solid);
            Chart.DrawTrendLine("line" + index, index, high, index, low, Color.FromName(color.ToString("G")), WickWidth, LineStyle.Solid);

        }

        #endregion

        #region Private Methods

        // --> Seguiamo la signature con underscore "_mioMetodo()"

        #endregion

    }

}