using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using iTMO.Help.Controller;
using iTMO.Help.Model;
using iTMO.Help.Model.ViewReady;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace iTMO.Help.Utils
{
    /// <summary>
    /// Used to serialize objects from JSON format to Object represantaton
    /// </summary>
    class SerializeUtils
    {
        public static SerializeData<List<ExamVR>>           ToExamView(string data)
        {
            SerializeData<List<ExamVR>> serializedData = new SerializeData<List<ExamVR>>() { Data = new List<ExamVR>() };
            ScheduleExam exams = null;

            try
            {
                exams = JsonConvert.DeserializeObject<ScheduleExam>(data);

                if (ModelUtils.IsExamValid(exams))
                {
                    if (exams.faculties[0].departments[0].groups[0].exams_schedule.Count == 0)
                        throw new ArgumentException();

                    foreach (ExamsSchedule exam in exams.faculties[0].departments[0].groups[0].exams_schedule)
                    {
                        try
                        {
                            var realDayExam = exam.exam_day_text;
                            if (realDayExam.Length > 7)
                                realDayExam = realDayExam.Substring(0, 7);

                            var realDayAdvice = exam.advice_day_text;
                            if (realDayAdvice.Length > 7)
                                realDayAdvice = realDayAdvice.Substring(0, 7);

                            serializedData.Data.Add(new ExamVR
                            {
                                DateAdvice  = exam.advice_date.Substring(0, exam.advice_date.LastIndexOf('.')),
                                DateExam    = exam.exam_date.Substring(0, exam.exam_date.LastIndexOf('.')),
                                DayExam     = realDayExam,
                                DayAdvice   = exam.advice_day_text,
                                TimeAdvice  = exam.advice_time,
                                TimeExam    = exam.exam_time,

                                Subject     = exam.subject,
                                Teacher     = exam.teachers[0].teacher_name,
                                TeacherId   = exam.teachers[0].teacher_id,
                                RoomAdvice  = exam.auditories[1].auditory_name,
                                RoomExam    = exam.auditories[0].auditory_name,
                            });
                        }
                        catch (ArgumentNullException ex) { }
                        catch (FormatException ex) { }
                    }
                    serializedData.isValid = true;
                }
                else throw new ArgumentNullException();
            }
            catch (JsonSerializationException ex)   { serializedData.Message = "Json Parse Error"; }
            catch (ArgumentNullException ex)        { serializedData.Message = "Group number is probably Invalid!"; }
            catch (ArgumentException ex)            { serializedData.Message = "Empty"; }
            catch (Exception ex)                    { serializedData.Message = "Unexpected Server Response"; }

            return serializedData;
        }

        public static SerializeData<List<ScheduleVR>>       ToScheduleView(string data)
        {
            SerializeData<List<ScheduleVR>> serializedData = new SerializeData<List<ScheduleVR>>();
            Schedule schedule = null;
            try
            {
                if ((schedule = JsonConvert.DeserializeObject<Schedule>(data)) == null)
                {

                }
            }
            catch (JsonSerializationException ex)
            {

            }
            return serializedData;
        }

        public static SerializeData<Journal>                ToJournalView(string data)
        {
            SerializeData<Journal> serializedData = new SerializeData<Journal>() { isValid = true };

            try
            {
                if ((serializedData.Data = JsonConvert.DeserializeObject<Journal>(data)) == null 
                    || !ModelUtils.IsJournalValid(serializedData.Data))
                {
                    serializedData.Message = "Empty";
                    serializedData.isValid = false;
                }
                else
                {
                    foreach (Year year in serializedData.Data.years)
                    {
                        var evenSem = new List<Subject>();
                        var oddSem = new List<Subject>();

                        foreach (Subject subject in year.subjects)
                        {
                            if (subject.marks == null || subject.marks.Count == 0)
                                subject.marks = new List<Mark>() { new Mark() };

                            if (subject.points == null || subject.points.Count == 0)
                                subject.points = new List<Points>() { new Points() { value = "0" } };
                            else
                            {
                                subject.value    = subject.points[0].value;
                                subject.max      = subject.points[0].max;
                                subject.variable = subject.points[0].variable;
                                subject.limit    = subject.points[0].limit;
                                subject.points.RemoveAt(0);
                            }

                            // Sort by even & odd semesters, cause API doesnt garantie even the order of the subjects...
                            var semCheck = 0;
                            int.TryParse(subject.semester, out semCheck);

                            if (semCheck % 2 == 0)
                                evenSem.Add(subject);
                            else
                                oddSem.Add(subject);
                        }
                        year.subjects.Clear();
                        year.subjects.AddRange(oddSem);
                        year.subjects.AddRange(evenSem);
                    }
                }
            }
            catch (JsonSerializationException ex)   { serializedData.Message = ex.Message; }
            catch (Exception ex)                    { serializedData.Message = "Unexpected Server Response"; }
            return serializedData;
        }

        public static SerializeData<List<JournalChangeLog>> ToJournalChangeLogView(string data)
        {
            SerializeData<List<JournalChangeLog>> serializedData = new SerializeData<List<JournalChangeLog>>();

            try
            {
                serializedData.Data = JsonConvert.DeserializeObject<List<JournalChangeLog>>(data);
                serializedData.isValid = true;
            }
            catch (JsonSerializationException ex) { serializedData.Message = ex.Message; }
            return serializedData;
        }

        public static SerializeData<List<MessageDe>>        ToMessageDeView(string data)
        {
            SerializeData<List<MessageDe>> serializedData = new SerializeData<List<MessageDe>>();

            try
            {
                if ((serializedData.Data = JsonConvert.DeserializeObject<List<MessageDe>>(data)) != null
                    && ModelUtils.IsMessageDeValid(serializedData.Data))
                {
                    for (int i = 0; i < serializedData.Data.Count; i++)
                    {
                        var item = serializedData.Data[i];
                        item.text = CommonUtils.HtmlToPlainText(item.text);
                        item.positionInList = i;
                    }
                    serializedData.isValid = true;
                }
                else
                    serializedData.Message = "Empty";
            }
            catch (JsonSerializationException ex) { serializedData.Message = ex.Message; }
            catch (Exception ex) { serializedData.Message = ex.Message; }
            return serializedData;
        }

        public static SerializeData<AttestationDe>          ToAttestationDeView(string data, int semId)
        {
            SerializeData<AttestationDe> serializedData = new SerializeData<AttestationDe>();

            try
            {
                var dom = new HtmlParser().Parse(data);

                AttestationDe storedAttestationDe = DatabaseController.Me.DAttestationDe;

                if (storedAttestationDe == null)
                    storedAttestationDe = AtteScheduleFillStrategy(ToAttestationSchedule(dom));

                if (semId == 1 && storedAttestationDe.First.Subjects.Count == 0)
                {
                    storedAttestationDe.First.Subjects = ToAttestationSubjectList(dom);
                    storedAttestationDe.First = FillAttestationSemester(storedAttestationDe.First);
                }
                else if (semId == 2 && storedAttestationDe.Last.Subjects.Count == 0)
                {
                    storedAttestationDe.Last.Subjects = ToAttestationSubjectList(dom);
                    storedAttestationDe.Last = FillAttestationSemester(storedAttestationDe.Last);
                }

                DatabaseController.Me.DAttestationDe = serializedData.Data = storedAttestationDe;
                serializedData.isValid = true;
            }
            catch (Exception ex) { serializedData.Message = ex.ToString(); }

            return serializedData;
        }

        /// <summary>
        /// Used DOM document structure to parse & fill subjects
        /// </summary>
        private static List<AttesatationSubjectDe> ToAttestationSubjectList(IHtmlDocument dom)
        {
            var subjectList = new List<AttesatationSubjectDe>();

            //div with class "p-inner nobt" - table of subjects
            string SubjectDivClassName = "div.c-page";
            // h4 - subject start and its title
            string SubjectElementTagName = "h4";
            // table with class "table-shedule-group" - subject test's
            string TestTableClassName = "table.table-shedule-group";

            try
            {
                var divSubjects = dom.QuerySelector(SubjectDivClassName);
                var table = dom.QuerySelectorAll(TestTableClassName);

                foreach (IElement subject in divSubjects.QuerySelectorAll(SubjectElementTagName))
                    subjectList.Add(new AttesatationSubjectDe() { Name = subject.TextContent });

                int i = 0;
                foreach (IElement elem in table)
                {
                    foreach (IElement tr in elem.QuerySelectorAll("tr"))
                    {
                        var test = new AttestationTestDe();
                        foreach (IElement td in tr.QuerySelectorAll("td"))
                        {
                            if (td.ChildElementCount > 0)
                                test.Name = td.QuerySelector("li").TextContent;
                            else
                                test.Week = td.TextContent;
                        }
                        subjectList[i].Tests.Add(test);
                    }
                    i++;
                }
            }
            catch (Exception ex) { subjectList.Add(new AttesatationSubjectDe() { Name = "PARSE ERROR" }); }
            return subjectList;
        }

        /// <summary>
        /// Used DOM document structure to fill parse & AttestationSchedule
        /// </summary>
        private static List<AttestationTimeTable> ToAttestationSchedule(IHtmlDocument dom)
        {
            // div with class 
            string scheduleDivClassName = "div.f-block";
            // week name value
            string weekDivClassName     = "div.pull-left";
            // value period dates
            string intervalDivClassName = "div.pull-right";
            // semester href
            string linkSemesterClassName = "active";
            // week href
            string linkWeekClassName     = "sub";

            List<AttestationTimeTable> weeks = new List<AttestationTimeTable>();

            try
            {
                var divWithWeeks = dom.QuerySelectorAll(scheduleDivClassName)[1];
                var weeksTimeTable = divWithWeeks.QuerySelectorAll("a");
                foreach (IElement weekAndTime in weeksTimeTable) 
                {
                    if (weekAndTime.ClassName != null)
                    {
                        var className = Regex.Replace(weekAndTime.ClassName.ToString(), @"\s+", "");
                        if (className.ToString().Equals(linkWeekClassName))
                        {
                            weeks.Add(new AttestationTimeTable()
                            {
                                Interval = weekAndTime.QuerySelector(intervalDivClassName).TextContent,
                                Week = weekAndTime.QuerySelector(weekDivClassName).TextContent,
                                Link = weekAndTime.GetAttribute("href")
                            });
                        }
                    }
                }
            }
            catch(Exception ex) { var str = ex.ToString(); }

            return weeks;
        }

        /// <summary>
        /// Correctly splits all <see cref="AttestationTimeTable"/> objects 
        /// to fill <see cref="AttestationSchedule.First"/> & <see cref="AttestationSchedule.Last"/> Semesters
        /// </summary>
        private static AttestationDe AtteScheduleFillStrategy( List<AttestationTimeTable> timeTable)
        {
            string firstWeekName = "1";

            var attestation = new AttestationDe(DatabaseController.Me.LangOpt);

            // Can't find by "Неделя 1", may be due to UTF-8 or... Who knows
            var lastSemFirstWeekIndex = timeTable.FindLastIndex(week => week.Week.Split(' ')[1] == firstWeekName);

            if (lastSemFirstWeekIndex != -1)
            {
                attestation.First.TimeTable.AddRange(timeTable.GetRange(0, lastSemFirstWeekIndex));
                attestation.Last.TimeTable.AddRange(timeTable.GetRange(lastSemFirstWeekIndex, timeTable.Count - lastSemFirstWeekIndex));
            }

            return attestation;
        }

        /// <summary>
        /// Sorts and Fills missing semester's & AtteTimeTable anr others fields
        /// </summary>
        private static AttestationSemesters FillAttestationSemester(AttestationSemesters semester)
        {
            var filledSemester = semester;
            foreach(AttesatationSubjectDe subject in filledSemester.Subjects)
            {
                foreach(AttestationTestDe test in subject.Tests)
                {
                    var week = filledSemester.TimeTable.Find((w => w.Week.Split(' ')[1] == test.Week.Split(' ')[0]));
                    test.Link = week.Link;
                    test.Week = week.Week;
                    test.Interval = week.Interval;
                }
            }

            return filledSemester;
        }
    }
}
