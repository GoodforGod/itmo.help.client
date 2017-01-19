using System.Collections.Generic;

namespace iTMO.Help.Model
{
    //variable":"Семестр 1","max":"100","limit":"60","value

    public class Points
    {
        public string variable  { get; set; } = "";
        public string max       { get; set; } = "";
        public string limit     { get; set; } = "";
        public string value     { get; set; } = "";
    }

    public class Mark
    {
        public string mark      { get; set; } = "";
        public string markdate  { get; set; } = "";
        public string worktype  { get; set; } = "";
    }

    public class Subject
    {
        public string       name     { get; set; } = "";
        public string       semester { get; set; } = "";
        public List<Mark>   marks    { get; set; }
        public List<Points> points   { get; set; }
    }

    public class Year
    {
        public string           group     { get; set; } = "";
        public string           studyyear { get; set; } = "";
        public List<Subject>    subjects  { get; set; }
    }

    public class Journal 
    {
        public List<Year> years { get; set; }
    }
}
