using System.Collections.Generic;

namespace iTMO.Help.Model
{
    /// <summary>
    /// JSON serialized object of "www.ifmo.ru/ru/" API
    /// </summary>
    public class Lesson
    {
        public string   subject     { get; set; }
        public string   type        { get; set; }
        public string   type_name   { get; set; }
        public string   time_start  { get; set; }
        public string   time_end    { get; set; }
        public int      parity      { get; set; }
        public string   parity_text { get; set; }
        public string   date_start  { get; set; }
        public string   date_end    { get; set; }

        public List<string>     dates       { get; set; }
        public List<Teacher>    teachers    { get; set; }
        public List<Auditory>   auditories  { get; set; }
    }

    public class StudySchedule
    {
        public int          weekday { get; set; }
        public List<Lesson> lessons { get; set; }
    }

    public class Schedule
    {
        public List<Faculty> faculties { get; set; }
    }
}
