﻿namespace iTMO.Help.Model
{
    public class Var
    {
        public string name      { get; set; } = "";
        public string min       { get; set; } = "";
        public string max       { get; set; } = "";
        public string threshold { get; set; } = "";
    }

    /// <summary>
    /// JSON serialized object of "de.ifmo.ru" API
    /// </summary>
    public class JournalChangeLog
    {
        public string subject   { get; set; } = "";
        public Var    var       { get; set; } = new Var();
        public string value     { get; set; } = "";
        public string date      { get; set; } = "";
        public string sign      { get; set; } = "";
    }
}
