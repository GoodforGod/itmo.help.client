using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTMO.Help.Model
{
    public class Mark
    {
        public string MarkPoint { get; set; }
        public string Markdate  { get; set; }
        public string Worktype  { get; set; }
    }

    public class Subject
    {
        public string Name          { get; set; }
        public string Semester      { get; set; }
        public List<Mark> Marks     { get; set; }
        public List<object> Points  { get; set; }
    }

    public class Year
    {
        public string Group             { get; set; }
        public string StudyYear         { get; set; }
        public List<Subject> Subjects   { get; set; }
    }

    public class Journal
    {
        public List<Year> Years { get; set; }
    }
}
