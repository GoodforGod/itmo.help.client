using System.Collections.Generic;

namespace iTMO.Help.Model
{
    public class Teacher
    {
        public string teacher_name  { get; set; } = "";
        public string teacher_id    { get; set; } = "-1";
    }

    public class Auditory
    {
        public string type              { get; set; } = "";
        public string auditory_name     { get; set; } = "";
        public string auditory_address  { get; set; } = "";
    }

    public class ExamsSchedule
    {
        public string   subject         { get; set; } = "";
        public string   type            { get; set; } = "-1";
        public string   type_name       { get; set; } = "";
        public string   exam_time       { get; set; } = "";
        public string   exam_date       { get; set; } = "";
        public string   exam_day        { get; set; } = "-1";
        public string   exam_day_text   { get; set; } = "";
        public string   advice_time     { get; set; } = "";
        public string   advice_date     { get; set; } = "";
        public string   advice_day      { get; set; } = "";
        public string   advice_day_text { get; set; } = "";

        public List<object>     dates       { get; set; } = new List<object>();
        public List<Teacher>    teachers    { get; set; } = new List<Teacher>();
        public List<Auditory>   auditories  { get; set; } = new List<Auditory>();
    }

    public class Group
    {
        public string              group_name     { get; set; } = "";

        public List<StudySchedule> study_schedule { get; set; } = new List<StudySchedule>();

        // Think that when this list is empty in JSON response, library can't parse it
        public List<ExamsSchedule> exams_schedule { get; set; } = new List<ExamsSchedule>();
    }

    public class Department
    {
        public string   department_name { get; set; } = "";
        public int      department_id   { get; set; } = -1;

        public List<Group> groups { get; set; } = new List<Group>();
    }

    public class Faculty
    {
        public string   faculty_name    { get; set; } = "";
        public int      faculty_id      { get; set; } = -1;

        public List<Department> departments { get; set; }
    }

    /// <summary>
    /// JSON serialized object of "www.ifmo.ru/ru/" API
    /// </summary>
    public class ScheduleExam
    {
        public List<Faculty> faculties { get; set; }
    }
}
