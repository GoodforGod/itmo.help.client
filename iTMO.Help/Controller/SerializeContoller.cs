using iTMO.Help.Model;
using iTMO.Help.Model.ViewReady;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace iTMO.Help.Controller
{
    class SerializeContoller
    {
        public static List<ExamVR> ToExamViewReady(string data)
        {
            ScheduleExam exams      = null;
            List<ExamVR> listExamVR = new List<ExamVR>();

            try
            {
                exams = JsonConvert.DeserializeObject<ScheduleExam>(data);

                if (exams.faculties != null
                    && exams.faculties[0].departments != null
                    && exams.faculties[0].departments[0].groups != null
                    && exams.faculties[0].departments[0].groups[0].exams_schedule != null)
                {
                    var examScheduleList = exams.faculties[0].departments[0].groups[0].exams_schedule;
                    foreach (ExamsSchedule exam in examScheduleList)
                    {
                        try
                        {
                            listExamVR.Add(new ExamVR
                            {
                                DateAdviceStr   = exam.advice_date,
                                DateExamStr     = exam.exam_date,
                                TimeAdvice      = exam.advice_time,
                                TimeExam        = exam.exam_time,
                                Subject         = exam.subject,
                                WeekdayAdvice   = exam.advice_day_text,
                                WeekdayExam     = exam.exam_day_text,
                                Teacher         = exam.teachers[0].teacher_name,
                                RoomAdvice      = exam.auditories[1].auditory_name,
                                RoomExam        = exam.auditories[0].auditory_name,
                            });
                        }
                        catch(ArgumentNullException ex) { listExamVR.Add(new ExamVR { Subject = "INVALID" }); }
                        catch(FormatException ex)       { listExamVR.Add(new ExamVR { Subject = "INVALID" }); }
                    }
                }
                else throw new ArgumentNullException("Somethink is missing in JSON Response! ITMO API STOP IT PLEASE!");
            }
            catch(JsonSerializationException ex)    { new List<ExamVR>() { new ExamVR() { Subject = "JSON PARSE ERROR" } }; }

            return listExamVR;
        }

        public static List<ScheduleVR> ToScheduleViewReady(string data)
        {
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
            return listScheduleVR;
        }

        public static Journal ToJournalView(string data)
        {
            Journal journal = null;

            try
            {
                journal = JsonConvert.DeserializeObject<Journal>(data);
            }
            catch (JsonSerializationException ex)
            {

            }
            return journal;
        }

        public static JournalChangeLog ToJournalChangeLogView(string data)
        {
            JournalChangeLog journalChangeLog = null;

            try
            {
                journalChangeLog = JsonConvert.DeserializeObject<JournalChangeLog>(data);
            }
            catch (JsonSerializationException ex)
            {

            }
            return journalChangeLog;
        }

        public static MessageDe ToMessageDeView(string data)
        {
            MessageDe messages = null;

            try
            {
                messages = JsonConvert.DeserializeObject<MessageDe>(data);
            }
            catch (JsonSerializationException ex)
            {

            }
            return messages;
        }
    }
}
