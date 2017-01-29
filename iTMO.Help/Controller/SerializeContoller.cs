using iTMO.Help.Model;
using iTMO.Help.Model.ViewReady;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace iTMO.Help.Controller
{
    class SerializeData<TValue>
    {
        public TValue   Data    { get; set; }
        public bool     IsValid { get; set; } = false;
        public string   Message { get; set; } = "";
    }

    class SerializeContoller
    {
        public static SerializeData<TValue> ToViewReady<TValue>(string data)
        {

            return new SerializeData<TValue>();
        }
        public static SerializeData<List<ExamVR>> ToExamView(string data)
        {
            SerializeData<List<ExamVR>> serializedData = new SerializeData<List<ExamVR>>() { Data = new List<ExamVR>() };
            ScheduleExam exams = null;

            try
            {
                exams = JsonConvert.DeserializeObject<ScheduleExam>(data);

                if (exams.faculties != null 
                    && exams.faculties.Capacity != 0 
                    && exams.faculties[0].departments != null
                    && exams.faculties[0].departments.Capacity != 0
                    && exams.faculties[0].departments[0].groups != null
                    && exams.faculties[0].departments[0].groups.Capacity != 0
                    && exams.faculties[0].departments[0].groups[0].exams_schedule != null)
                {
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
                                DateAdvice      = exam.advice_date.Substring(0, exam.advice_date.LastIndexOf('.')),
                                DateExam        = exam.exam_date.Substring(0, exam.exam_date.LastIndexOf('.')),
                                DayExam         = realDayExam,
                                DayAdvice       = exam.advice_day_text,
                                TimeAdvice      = exam.advice_time,
                                TimeExam        = exam.exam_time,

                                Subject         = exam.subject,
                                Teacher         = exam.teachers[0].teacher_name,
                                TeacherId       = exam.teachers[0].teacher_id,
                                RoomAdvice      = exam.auditories[1].auditory_name,
                                RoomExam        = exam.auditories[0].auditory_name,
                            });
                        }
                        catch (ArgumentNullException ex) { }
                        catch (FormatException ex)       { }
                    }
                    serializedData.IsValid = true;
                }
                else throw new ArgumentNullException("Group number is probably Invalid!");
            }
            catch (JsonSerializationException ex)   { serializedData.Message = "Json Parse Error"; }
            catch (ArgumentNullException ex)        { serializedData.Message = ex.Message; }
            catch (Exception ex)                    { serializedData.Message = "Unexpected Server Response"; }

            return serializedData;
        }

        public static SerializeData<List<ScheduleVR>> ToScheduleView(string data)
        {
            SerializeData<List<ScheduleVR>> serializedData = new SerializeData<List<ScheduleVR>>();
            Schedule            schedule        = null;
            List<ScheduleVR>    listScheduleVR  = new List<ScheduleVR>();

            try
            {
                schedule = JsonConvert.DeserializeObject<Schedule>(data);

                if(schedule == null)
                {

                }
            }
            catch (JsonSerializationException ex)
            {

            }
            return serializedData;
        }

        public static SerializeData<Journal> ToJournalView(string data)
        {
            SerializeData<Journal> serializedData = new SerializeData<Journal>() { IsValid = true };

            try
            {
                if ((serializedData.Data = JsonConvert.DeserializeObject<Journal>(data)) == null
                    || serializedData.Data.years == null
                    || serializedData.Data.years.Count == 0)
                {
                    serializedData.Message = "Empty";
                    serializedData.IsValid = false;
                }
                else
                {
                    foreach(Year year in serializedData.Data.years)
                    {
                        var evenSem = new List<Subject>();
                        var oddSem = new List<Subject>();

                        foreach (Subject subject in year.subjects)
                        {
                            if (subject.marks == null || subject.marks.Count == 0)
                                subject.marks = new List<Mark>() { new Mark() };
                            if (subject.points == null || subject.points.Count == 0)
                                subject.points = new List<Points>() { new Points() { value = "0" } };

                            // Sort by even & odd semesters, cause API doesnt garantie even the order of the subjects...
                            var semCheck = 0;
                            int.TryParse(subject.semester, out semCheck);

                            if(semCheck % 2 == 0)
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
                serializedData.IsValid = true;
            }
            catch (JsonSerializationException ex) { serializedData.Message = ex.Message; }
            return serializedData;
        }

        public static SerializeData<List<MessageDe>> ToMessageDeView(string data)
        {
            SerializeData<List<MessageDe>> serializedData = new SerializeData<List<MessageDe>>();

            try
            {
                if ((serializedData.Data = JsonConvert.DeserializeObject<List<MessageDe>>(data)) != null
                    && serializedData.Data.Count != 0)
                        serializedData.IsValid = true;
                else
                    serializedData.Message = "Empty";
            }
            catch (JsonSerializationException ex) { serializedData.Message = ex.Message; }
            catch (Exception ex)                  { serializedData.Message = ex.Message; }    
            return serializedData;
        }
    }
}
