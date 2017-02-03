using System.Collections.Generic;

namespace iTMO.Help.Model
{
    /// <summary>
    /// Attestation of the 101 Chair, site link "de.ifmo.ru"
    /// Contains subjects <see cref="AttesatationSubjectDe"/>
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

            this.Schedule = new AttestationSchedule(lang);
            this.Title = title;
        }

        public string                      Title    { get; set; }
        public List<AttesatationSubjectDe> Subjects { get; set; } = new List<AttesatationSubjectDe>();
        public AttestationSchedule         Schedule { get; set; } 
    }

    /// <summary>
    /// Subject which has attestations in the semester
    /// Contains test that will be availiable during semester <see cref="AttestationTestDe"/>
    /// </summary>
    class AttesatationSubjectDe
    {
        public string                  Name  { get; set; } = "";
        public List<AttestationTestDe> Tests { get; set; } = new List<AttestationTestDe>();
    }

    /// <summary>
    /// Subject's test in the semester
    /// <see cref="AttesatationSubjectDe"/>
    /// </summary>
    class AttestationTestDe
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

        public AttestationSemesters First { get; set; } = new AttestationSemesters();
        public AttestationSemesters Last  { get; set; } = new AttestationSemesters();
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
        public List<AttestationTimeTable> TimeTable { get; set; } = new List<AttestationTimeTable>();
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
        public string Link      { get; set; } = "";
    }
}
