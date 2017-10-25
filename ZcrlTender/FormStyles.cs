using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ZcrlTender
{
    public class FormStyles
    {
        private static Font moneyTotalsFont;
        private static Color wrongSumColor;
        private static Color rightSumColor;

        public static Font MoneyTotalsFont
        {
            get
            {
                return moneyTotalsFont.Clone() as Font;
            }
        }

        public static Color WrongSumColor
        {
            get
            {
                return wrongSumColor;
            }
        }

        public static Color RightSumColor
        {
            get
            {
                return rightSumColor;
            }
        }

        static FormStyles()
        {
            moneyTotalsFont = new System.Drawing.Font("Microsoft Sans Serif", 8f, FontStyle.Bold);
            wrongSumColor = Color.FromArgb(255, 0, 0);
            rightSumColor = Color.FromArgb(0, 0, 0);
        }
    }
}
