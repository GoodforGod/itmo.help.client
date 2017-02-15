using System;
using System.Collections.Generic;

namespace iTMO.Help.Model.ViewReady
{
    /// <summary>
    /// Object used to struct compact representation of the API's object <see cref="Schedule/>
    /// </summary>
    public class ScheduleVR
    {
        public string           Weekday     { get; set; } = "";
        public string           WeekdayText { get; set; } = "";
        public List<LessonVR>   Lessons     { get; set; } = new List<LessonVR>();
    }

    public class LessonVR
    {
        public string TimeStart { get; set; } = "";
        public string TimeEnd   { get; set; } = "";
        public string DateStart { get; set; } = "";
        public string DateEnd   { get; set; } = "";

        public string Parity        { get; set; } = "";
        public string ParityText    { get; set; } = "";

        public string RoomName      { get; set; } = "";
        public string RoomAddress   { get; set; } = "";

        public string Subject       { get; set; } = "";
        public string SubjectType   { get; set; } = "";

        public string Teacher       { get; set; } = "";
        public string TeacherId     { get; set; } = "-1";

        public override string ToString()
        {
            return "Subject : "     + Subject
                + ", Teacher : "    + Teacher
                + "\n[ On : "       + ParityText
                + ", starts in "    + TimeStart
                + ", ends in "      + TimeEnd + " ] "
                + "\n[ Address : "  + RoomAddress
                + ", in room"       + RoomName + " ]";
        }
    }
}
