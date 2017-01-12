using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTMO.Help.Model
{
    public class Teacher
    {
        public string TeacherName   { get; set; }
        public object TeacherId     { get; set; }
    }

    public class Auditory
    {
        public string Type              { get; set; }
        public string AuditoryName      { get; set; }
        public object AuditoryAddress   { get; set; }
    }

    public class ExamsSchedule
    {
        public string           Subject     { get; set; }
        public int              Type        { get; set; }
        public string           TypeName    { get; set; }
        public string           ExamTime    { get; set; }
        public string           ExamDate    { get; set; }
        public int              ExamDay     { get; set; }
        public string           ExamDayText { get; set; }
        public string           AdviceTime  { get; set; }
        public string           AdviceDate  { get; set; }
        public int              AdviceDay   { get; set; }
        public string           AdviceDayText { get; set; }
        public List<object>     Dates       { get; set; }
        public List<Teacher>    Teachers    { get; set; }
        public List<Auditory>   Auditories  { get; set; }
    }

    public class Group
    {
        public string               GroupName       { get; set; }
        public List<object>         StudySchedule   { get; set; }
        public List<ExamsSchedule>  ExamSchedule    { get; set; }
    }

    public class Department
    {
        public string       DepartmentName { get; set; }
        public int          DepartmeneId   { get; set; }
        public List<Group>  Groups         { get; set; }
    }

    public class Faculty
    {
        public string           FacultyName { get; set; }
        public int              FacultyId   { get; set; }
        public List<Department> Departments { get; set; }
    }

    public class ScheduleExam
    {
        public List<Faculty> Faculties { get; set; }
    }
}
