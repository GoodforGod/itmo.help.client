﻿using iTMO.Help.Controller;
using iTMO.Help.Model;
using iTMO.Help.Model.ViewReady;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace iTMO.Help.Utils
{
    /// <summary>
    /// Serialisation response with valid state and exception method
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    class SerializeData<TValue>
    {
        public TValue   Data    { get; set; }
        public bool     IsValid { get; set; } = false;
        public string   Message { get; set; } = "";
    }

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
                    serializedData.IsValid = true;
                }
                else throw new ArgumentNullException("Group number is probably Invalid!");
            }
            catch (JsonSerializationException ex)   { serializedData.Message = "Json Parse Error"; }
            catch (ArgumentNullException ex)        { serializedData.Message = ex.Message; }
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
            SerializeData<Journal> serializedData = new SerializeData<Journal>() { IsValid = true };

            try
            {
                if ((serializedData.Data = JsonConvert.DeserializeObject<Journal>(data)) == null 
                    || !ModelUtils.IsJournalValid(serializedData.Data))
                {
                    serializedData.Message = "Empty";
                    serializedData.IsValid = false;
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
                serializedData.IsValid = true;
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
                    serializedData.IsValid = true;

                    for (int i = 0; i < serializedData.Data.Count; i++)
                    {
                        var item = serializedData.Data[i];
                        item.text = CommonUtils.HtmlToPlainText(item.text);
                        item.positionInList = i;
                    }
                }
                else
                    serializedData.Message = "Empty";
            }
            catch (JsonSerializationException ex) { serializedData.Message = ex.Message; }
            catch (Exception ex) { serializedData.Message = ex.Message; }
            return serializedData;
        }
    }
}