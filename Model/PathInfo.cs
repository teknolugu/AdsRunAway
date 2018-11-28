using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsRunAway.Model
{
    public static class PathInfo
    {
        private static readonly string hosts = Environment.SystemDirectory + @"\drivers\etc\hosts";
        private static readonly string downloaded = AppDomain.CurrentDomain.BaseDirectory + @"\hosts";
        private static readonly string source = AppDomain.CurrentDomain.BaseDirectory + @"\data\data.json";

        public static string Source { get => source; }
        public static string Downloaded { get => downloaded; }
        public static string Hosts { get => hosts;  }
    }
}
