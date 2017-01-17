using System;

namespace iTMO.Help.Model.ViewReady
{
    public class ExamVR
    {
        public string DateExamStr   { get; set; } = "";
        public string DateAdviceStr { get; set; } = "";

        public DateTime DateExam    { get; set; }
        public DateTime DateAdvice  { get; set; }
        public string WeekdayExam   { get; set; } = "";
        public string WeekdayAdvice { get; set; } = "";
        public string TimeExam      { get; set; } = "";
        public string TimeAdvice    { get; set; } = "";
        public string RoomExam      { get; set; } = "";
        public string RoomAdvice    { get; set; } = "";
        public string Subject       { get; set; } = "";
        public string Teacher       { get; set; } = "";
    }
}
