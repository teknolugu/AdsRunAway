using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AdsRunAway.Helper
{
    class ThemeManager
    {
        public string GetTheme()
        {
            string currentTheme;
            if (Properties.Settings.Default.Dark)
            {
                currentTheme = "Dark";
            }
            else
            {
                currentTheme = "Light";
            }
            return currentTheme;
        }

        public List<Color> ApplyTheme(ThemeType theme)
        {
            List<Color> themeColor = new List<Color>();
            switch (theme)
            {
                case ThemeType.Light:
                    {

                        break;
                    }
                case ThemeType.Dark:
                    {
                        themeColor.Add(ColorTranslator.FromHtml("#1E1E1E"));
                        themeColor.Add(ColorTranslator.FromHtml("#333333"));
                        
                        break;
                    }
            }
            return themeColor;
        }
    }
}
