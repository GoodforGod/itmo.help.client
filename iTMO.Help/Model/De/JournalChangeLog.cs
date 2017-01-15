using System;
using System.Collections.Generic;

namespace iTMO.Help.Model
{
    public class Var
    {
        public string Name      { get; set; }
        public string Min       { get; set; }
        public string Max       { get; set; }
        public string Threshold { get; set; }
    }

    public class JournalChangeLog
    {
        public string Subject   { get; set; }
        public Var    Var       { get; set; }
        public string Value     { get; set; }
        public string Date      { get; set; }
        public string Sign      { get; set; }
    }
}
