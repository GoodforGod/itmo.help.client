using System.Collections.Generic;

namespace iTMO.Help.Model
{
    /// <summary>
    /// JSON serialized object of "www.ifmo.ru/ru/" API
    /// </summary>
    public class Lesson
    {
        public string subject       { get; set; } = "";
        public string type          { get; set; } = "";
        public string type_name     { get; set; } = "";
        public string time_start    { get; set; } = "";
        public string time_end      { get; set; } = "";
        public string parity        { get; set; } = "";
        public string parity_text   { get; set; } = "";
        public string date_start    { get; set; } = "";
        public string date_end      { get; set; } = "";

        public List<string> dates { get; set; } = new List<string>();
        public List<Teacher> teachers { get; set; } = new List<Teacher>();
        public List<Auditory> auditories { get; set; } = new List<Auditory>(); 
    }

    public class StudySchedule
    {
        public string       weekday { get; set; }
        public List<Lesson> lessons { get; set; }
    }

    public class Schedule
    {
        public List<Faculty> faculties { get; set; }
    }
}
