using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using iTMO.Help.Controller;
using iTMO.Help.Model;
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
            try
            {
                ScheduleExam exams = JsonConvert.DeserializeObject<ScheduleExam>(data);
                if (ModelUtils.IsExamValid(exams))
                {
                    if (exams.faculties[0].departments[0].groups[0].exams_schedule.Count == 0)
                        throw new ArgumentException();

                    foreach (ExamsSchedule exam in exams.faculties[0].departments[0].groups[0].exams_schedule)
                    {
                        try
                        {
                            ExamsSchedule noNullExam = toNonNullExamSchedule(exam);

                            var realDayExam = noNullExam.exam_day_text;
                            if (realDayExam.Length > 7)
                                realDayExam = realDayExam.Substring(0, 7);

                            var realDayAdvice = noNullExam.advice_day_text;
                            if (realDayAdvice.Length > 7)
                                realDayAdvice = realDayAdvice.Substring(0, 7);

                            serializedData.Data.Add(new ExamVR
                            {
                                DateAdvice  = noNullExam.advice_date,
                                DateExam    = noNullExam.exam_date,
                                DayExam     = realDayExam,
                                DayAdvice   = noNullExam.advice_day_text,
                                TimeAdvice  = noNullExam.advice_time,
                                TimeExam    = noNullExam.exam_time,

                                Subject     = noNullExam.subject,
                                Teacher     = noNullExam.teachers[0].teacher_name,
                                TeacherId   = noNullExam.teachers[0].teacher_id,
                                RoomAdvice  = noNullExam.auditories[1].auditory_name,
                                RoomExam    = noNullExam.auditories[0].auditory_name,
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
            SerializeData<List<ScheduleVR>> serializedData = new SerializeData<List<ScheduleVR>>() { Data = new List<ScheduleVR>() };
            try
            {
                Schedule schedule = JsonConvert.DeserializeObject<Schedule>(data);
                if (ModelUtils.IsScheduleValid(schedule))
                {
                    if (schedule.faculties[0].departments[0].groups[0].study_schedule.Count == 0)
                        throw new ArgumentException();

                    foreach (StudySchedule week in schedule.faculties[0].departments[0].groups[0].study_schedule)
                    {
                        try
                        {
                            var weekText = "Unknown";
                            var weekDayNumber = 0;
                            if (!string.IsNullOrWhiteSpace(week.weekday) && int.TryParse(week.weekday, out weekDayNumber))
                            {
                                switch (weekDayNumber)
                                {
                                    case 1: weekText = "Monday";    break;
                                    case 2: weekText = "Tuesday";   break;
                                    case 3: weekText = "Wednesday"; break;
                                    case 4: weekText = "Thursday";  break;
                                    case 5: weekText = "Friday";    break;
                                    case 6: weekText = "Saturday";  break;
                                    case 7: weekText = "Sunday";    break;
                                    default: break;
                                }
                            }

                            List<LessonVR> weekLessons = new List<LessonVR>();

                            foreach(Lesson lesson in week.lessons)
                            {
                                var nonNullLesson = toNonNullLesson(lesson);

                                weekLessons.Add(new LessonVR()
                                {
                                    Subject     = nonNullLesson.subject,
                                    SubjectType = nonNullLesson.type_name,

                                    Teacher     = nonNullLesson.teachers[0].teacher_name,
                                    TeacherId   = nonNullLesson.teachers[0].teacher_id,
                                    RoomAddress = nonNullLesson.auditories[0].auditory_address,
                                    RoomName    = nonNullLesson.auditories[0].auditory_name,

                                    Parity      = nonNullLesson.parity,
                                    ParityText  = nonNullLesson.parity_text,
                                    DateStart   = nonNullLesson.date_start,
                                    DateEnd     = nonNullLesson.date_end,
                                    TimeStart   = nonNullLesson.time_start,
                                    TimeEnd     = nonNullLesson.time_end
                                });
                            }

                            serializedData.Data.Add(new ScheduleVR() {
                                WeekdayText = weekText,
                                Weekday = week.weekday.ToString(),
                                Lessons = weekLessons
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

        private static Lesson toNonNullLesson(Lesson lesson)
        {
            Lesson nonNullLesson = new Lesson();
            nonNullLesson.auditories.Add(new Auditory());
            nonNullLesson.teachers.Add(new Teacher());

            if (lesson.auditories != null && lesson.auditories.Count != 0)
            {
                if (!string.IsNullOrWhiteSpace(lesson.auditories[0].auditory_address))
                    nonNullLesson.auditories[0].auditory_address = lesson.auditories[0].auditory_address;
                if (!string.IsNullOrWhiteSpace(lesson.auditories[0].auditory_name))
                    nonNullLesson.auditories[0].auditory_name = lesson.auditories[0].auditory_name;

                //if (string.IsNullOrWhiteSpace(lesson.auditories[0].auditory_name)
                //    && string.IsNullOrWhiteSpace(lesson.auditories[0].auditory_address)
                //    && lesson.auditories.Count == 2)
                //{
                //    if (!string.IsNullOrWhiteSpace(lesson.auditories[1].auditory_address))
                //        nonNullLesson.auditories[0].auditory_address = lesson.auditories[1].auditory_address;
                //    if (!string.IsNullOrWhiteSpace(lesson.auditories[1].auditory_name))
                //        nonNullLesson.auditories[0].auditory_name = lesson.auditories[1].auditory_name;
                //}
            }

            if (lesson.teachers != null && lesson.teachers.Count != 0)
            {
                if (!string.IsNullOrWhiteSpace(lesson.teachers[0].teacher_name))
                    nonNullLesson.teachers[0].teacher_name = lesson.teachers[0].teacher_name;
                if (!string.IsNullOrWhiteSpace(lesson.teachers[0].teacher_id))
                    nonNullLesson.teachers[0].teacher_id = lesson.teachers[0].teacher_id;

                //if (string.IsNullOrWhiteSpace(lesson.teachers[0].teacher_name) 
                //    && string.IsNullOrWhiteSpace(lesson.teachers[0].teacher_id)
                //    && lesson.teachers.Count == 2)
                //{
                //    if (!string.IsNullOrWhiteSpace(lesson.teachers[1].teacher_name))
                //        nonNullLesson.teachers[0].teacher_name = lesson.teachers[1].teacher_name;
                //    if (!string.IsNullOrWhiteSpace(lesson.teachers[1].teacher_id))
                //        nonNullLesson.teachers[0].teacher_id = lesson.teachers[1].teacher_id;
                //}
            }

            if (!string.IsNullOrWhiteSpace(lesson.subject))
                nonNullLesson.subject = lesson.subject;
            if(!string.IsNullOrWhiteSpace(lesson.type_name))
                nonNullLesson.type_name = lesson.type_name;
            if(!string.IsNullOrWhiteSpace(lesson.parity))
                nonNullLesson.parity = lesson.parity;
            if(!string.IsNullOrWhiteSpace(lesson.parity_text))
                nonNullLesson.parity_text = lesson.parity_text;
            if(!string.IsNullOrWhiteSpace(lesson.date_start))
                nonNullLesson.date_start = lesson.date_start;
            if(!string.IsNullOrWhiteSpace(lesson.date_end))
                nonNullLesson.date_end = lesson.date_end;
            if(!string.IsNullOrWhiteSpace(lesson.time_start))
                nonNullLesson.time_start = lesson.time_start;
            if(!string.IsNullOrWhiteSpace(lesson.time_end))
                nonNullLesson.time_end= lesson.time_end;

            return nonNullLesson;
        }

        private static ExamsSchedule toNonNullExamSchedule(ExamsSchedule exam)
        {
            ExamsSchedule nonNullExam = new ExamsSchedule();
            nonNullExam.auditories.Add(new Auditory());
            nonNullExam.auditories.Add(new Auditory());
            nonNullExam.teachers.Add(new Teacher());

            // ADVICE
            if (!string.IsNullOrWhiteSpace(exam.advice_date) && exam.advice_date.Contains("."))
                nonNullExam.exam_date = exam.advice_date.Substring(0, exam.advice_date.LastIndexOf('.'));
            if (!string.IsNullOrWhiteSpace(exam.advice_day))
                nonNullExam.advice_day = exam.advice_day;
            if (!string.IsNullOrWhiteSpace(exam.advice_day_text))
                nonNullExam.advice_day_text = exam.advice_day_text;
            if (!string.IsNullOrWhiteSpace(exam.advice_time))
                nonNullExam.advice_time = exam.advice_time;

            // AUDITORIES
            if (exam.auditories.Count != 0)
            {
                if (!string.IsNullOrWhiteSpace(exam.auditories[0].auditory_address))
                    nonNullExam.auditories[0].auditory_address = exam.auditories[0].auditory_address;
                if (!string.IsNullOrWhiteSpace(exam.auditories[0].auditory_name))
                    nonNullExam.auditories[0].auditory_name = exam.auditories[0].auditory_name;
                if (!string.IsNullOrWhiteSpace(exam.auditories[0].type))
                    nonNullExam.auditories[0].type = exam.auditories[0].type;
                //if (exam.auditories.Count == 2)
                //{
                //    nonNullExam.auditories.Add(new Auditory());

                //    if (!string.IsNullOrWhiteSpace(exam.auditories[1].auditory_address))
                //        nonNullExam.auditories[1].auditory_address = exam.auditories[1].auditory_address;
                //    if (!string.IsNullOrWhiteSpace(exam.auditories[1].auditory_name))
                //        nonNullExam.auditories[1].auditory_name = exam.auditories[1].auditory_name;
                //    if (!string.IsNullOrWhiteSpace(exam.auditories[1].type))
                //        nonNullExam.auditories[1].type = exam.auditories[1].type;
                //}
            }
            if(exam.dates.Count != 0)
                nonNullExam.dates = exam.dates;

            // EXAM
            if (!string.IsNullOrWhiteSpace(exam.exam_date) && exam.exam_date.Contains("."))
                nonNullExam.exam_date = exam.exam_date.Substring(0, exam.exam_date.LastIndexOf('.'));
            if(!string.IsNullOrWhiteSpace(exam.exam_day))
                nonNullExam.exam_day = exam.exam_day;
            if(!string.IsNullOrWhiteSpace(exam.exam_day_text))
                nonNullExam.exam_day_text= exam.exam_day_text;
            if(!string.IsNullOrWhiteSpace(exam.exam_time))
                nonNullExam.exam_time= exam.exam_time;

            // TEACHERS & SUBJECT
            if(!string.IsNullOrWhiteSpace(exam.subject))
                nonNullExam.subject= exam.subject;
            if (exam.teachers.Count != 0)
            {
                if (string.IsNullOrWhiteSpace(exam.teachers[0].teacher_id))
                    nonNullExam.teachers[0].teacher_id = exam.teachers[0].teacher_id;
                if (string.IsNullOrWhiteSpace(exam.teachers[0].teacher_name))
                    nonNullExam.teachers[0].teacher_name= exam.teachers[0].teacher_name;
            }
            if(!string.IsNullOrWhiteSpace(exam.type))
                nonNullExam.type= exam.type;
            if(!string.IsNullOrWhiteSpace(exam.type_name))
                nonNullExam.type_name= exam.type_name;

            return nonNullExam;
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
