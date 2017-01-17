using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTMO.Help.Model.ViewReady
{
    class JournalVR
    {
        public string Subject    { get; set; }
        public string Term       { get; set; }
        public string Total      { get; set; }
        public string Type       { get; set; }

        public bool IsMinReached { get; set; }

        List<JournalItemVR> Items { get; set; }
    }

    class JournalItemVR
    {
        public string Name          { get; set; }
        public string MaxPoints     { get; set; }
        public string MinPoints     { get; set; }
        public string EarnedPoints  { get; set; }

        public bool IsMinReached    { get; set; }
    }
}
