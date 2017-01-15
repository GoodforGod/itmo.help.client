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
            ScheduleExam exams = null;
            List<ExamVR> listExamVr = new List<ExamVR>();

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
                            listExamVr.Add(new ExamVR
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
                        catch(ArgumentNullException ex)
                        {
                            
                        }
                        catch(FormatException ex)
                        {

                        }
                    }
                }
                else throw new ArgumentNullException("Somethink is missing in JSON Response! ITMO API STOP IT PLEASE!");

                return listExamVr;
            }
            catch(JsonSerializationException ex)
            {
                new List<ExamVR>() { new ExamVR() { Subject = "JSON PARSE ERROR" } };
            }
            catch(Exception ex)
            {
                new List<ExamVR>() { new ExamVR() { Subject = "UNKNOWN PARSE ERROR" } };
            }
            return listExamVr;
        }

        public static ScheduleVR ToScheduleViewReady(string data)
        {
            Schedule schedule = null;

            try
            {

            }
            catch (JsonSerializationException ex)
            {

            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
