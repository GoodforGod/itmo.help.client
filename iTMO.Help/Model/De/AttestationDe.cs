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
        public string               Title    { get; set; }

        public AttestationSemesters First    { get; set; } 
        public AttestationSemesters Last     { get; set; } 

        public AttestationDe(LanguageOption lang)
        {
            string title = "Group's attestation schedule";

            string firstName = "Autumn";
            string lastName = "Spring";
            string term = "Term";

            switch (lang)
            {
                case LanguageOption.RU:
                    title = "График аттестаций группы";
                    firstName = "Осенний";
                    lastName = "Весенний";
                    break;

                case LanguageOption.EN:
                    break;

                case LanguageOption.CN:
                    break;

                default:
                    break;
            }

            this.Title = title;
            this.First = new AttestationSemesters() { Name = firstName + term };
            this.Last  = new AttestationSemesters() { Name = lastName  + term };
        }
    }

    /// <summary>
    /// DE atttestation Semester represantation
    /// </summary>
    class AttestationSemesters
    {
        public string                       Name        { get; set; } = "";
        public List<AttesatationSubjectDe>  Subjects    { get; set; } = new List<AttesatationSubjectDe>();
        public List<AttestationTimeTable>   TimeTable   { get; set; } = new List<AttestationTimeTable>();
    }

    /// <summary>
    /// DE Attestation TimeTable represantation
    /// </summary>
    class AttestationTimeTable
    {
        public string Week      { get; set; } = "";
        public string Interval  { get; set; } = "";
        public string Link      { get; set; } = "";
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
        public string Name      { get; set; } = "";
        public string Week      { get; set; } = "";
        public string Interval  { get; set; } = "";
        public string Link      { get; set; } = "";
    }
}
