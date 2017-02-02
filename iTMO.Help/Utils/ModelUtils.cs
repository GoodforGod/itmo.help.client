using iTMO.Help.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTMO.Help.Utils
{
    class ModelUtils
    {
        public static bool IsJournalValid(Journal data)
        {
            return data.years != null && data.years.Count != 0;
        }

        public static bool IsJournalCustomValid(JournalCustom data)
        {

            return false;
        }

        public static bool IsJournalChangeLogValid(JournalChangeLog data)
        {

            return false;
        }

        public static bool IsMessageDeValid(List<MessageDe> data)
        {
            return data != null && data.Count != 0;
        }

        public static bool IsScheduleValid(Schedule data)
        {

            return false;
        }

        public static bool IsExamValid(ScheduleExam data)
        {
            return data.faculties != null
                    && data.faculties.Capacity != 0
                    && data.faculties[0].departments != null
                    && data.faculties[0].departments.Capacity != 0
                    && data.faculties[0].departments[0].groups != null
                    && data.faculties[0].departments[0].groups.Capacity != 0
                    && data.faculties[0].departments[0].groups[0].exams_schedule != null;
        }
    }
}
