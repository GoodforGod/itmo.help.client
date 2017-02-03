using System.Collections.Generic;

namespace iTMO.Help.Model
{
    /// <summary>
    /// Attestation of the 101 Chair, site link "de.ifmo.ru"
    /// Contains subjects <see cref="SubjectDe"/>
    /// And all avaliable dates/weeks for semesters <see cref="AttestationSemesters"/>
    /// 
    /// Lang option is used to localize application for used <see cref="LanguageOption"/>
    /// </summary>
    class AttestationDe
    {
        public AttestationDe(LanguageOption lang)
        {
            string title = "График аттестаций группы";

            switch (lang)
            {
                case LanguageOption.RU: break;
                case LanguageOption.EN: title = "Group's attestation schedule"; break;
                case LanguageOption.CN: break;
                default: break;
            }

            this.Title = title;
        }

        public string               Title               { get; set; } = "";
        public List<SubjectDe>      Subjects            { get; set; }
        public AttestationSemesters AttestationSemester { get; set; } = new AttestationSemesters();
    }

    /// <summary>
    /// Subject which has attestations in the semester
    /// Contains test that will be availiable during semester <see cref="TestDe"/>
    /// </summary>
    class SubjectDe
    {
        public string        Name    { get; set; } = "";
        public List<TestDe>  Tests   { get; set; }
    }

    /// <summary>
    /// Subject's test in the semester
    /// <see cref="SubjectDe"/>
    /// </summary>
    class TestDe
    {
        public string Name          { get; set; } = "";
        public string DateAndWeek   { get; set; } = "";
    }
    
    /// <summary>
    /// Contains two semesters of the year <see cref="AttestationSemesters"/>
    /// 
    /// Lang option is used to localize application for used <see cref="LanguageOption"/>
    /// </summary>
    class AttestationSchedule
    {
        public AttestationSchedule(LanguageOption lang)
        {
            string firstName = "Autumn";
            string lastName = "Spring";
            string term = "Term";

            switch (lang)
            {
                case LanguageOption.RU:
                    firstName = "Осенний";
                    lastName = "Весенний";
                    break;

                case LanguageOption.EN: break;
                case LanguageOption.CN: break;
                default: break;
            }

            First = new AttestationSemesters() { Name = firstName + term };
            Last = new AttestationSemesters()  { Name = lastName  + term };
        }

        public AttestationSemesters First { get; set; }
        public AttestationSemesters Last  { get; set; }
    }

    /// <summary>
    /// 
    /// Осенний семестр             <see cref="Name"/>
    /// Неделя  19  09.01 - 14.01   <see cref="AttestationTimeTable"/> 
    /// ...
    /// 
    /// </summary>
    class AttestationSemesters
    {
        public string                     Name      { get; set; } = "";
        public List<AttestationTimeTable> TimeTable { get; set; }
    }

    /// <summary>
    /// EXAMPLE
    /// 
    /// Неделя 19   09.01 - 14.01
    /// Неделя 20   16.01 - 21.01
    /// 
    /// </summary>
    class AttestationTimeTable
    {
        public string Week      { get; set; } = "";
        public string Interval  { get; set; } = "";
    }
}
