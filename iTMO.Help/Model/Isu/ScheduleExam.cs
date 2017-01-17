using System;
using System.Collections.Generic;

namespace iTMO.Help.Model
{
    public class Teacher
    {
        public string teacher_name  { get; set; } = "";
        public object teacher_id    { get; set; }
    }

    public class Auditory
    {
        public string type              { get; set; } = "";
        public string auditory_name     { get; set; } = "";
        public object auditory_address  { get; set; }
    }

    public class ExamsSchedule
    {
        public string   subject         { get; set; } = "";
        public int      type            { get; set; }
        public string   type_name       { get; set; } = "";
        public string   exam_time       { get; set; } = "";
        public string   exam_date       { get; set; } = "";
        public int      exam_day        { get; set; }
        public string   exam_day_text   { get; set; } = "";
        public string   advice_time     { get; set; } = "";
        public string   advice_date     { get; set; } = "";
        public int      advice_day      { get; set; }
        public string   advice_day_text { get; set; } = "";

        public List<object>     dates      { get; set; }
        public List<Teacher>    teachers   { get; set; }
        public List<Auditory>   auditories { get; set; }
    }

    public class Group
    {
        public string               group_name      { get; set; } = "";

        public List<object>         study_schedule  { get; set; }
        public List<ExamsSchedule>  exams_schedule  { get; set; }
    }

    public class Department
    {
        public string   department_name { get; set; } = "";
        public int      department_id   { get; set; }

        public List<Group> groups       { get; set; }
    }

    public class Faculty
    {
        public string   faculty_name    { get; set; } = "";
        public int      faculty_id      { get; set; }

        public List<Department> departments { get; set; }
    }

    public class ScheduleExam
    {
        public List<Faculty> faculties { get; set; }
    }
}
