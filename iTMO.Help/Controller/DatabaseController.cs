using iTMO.Help.Model;
using iTMO.Help.Model.ViewReady;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace iTMO.Help.Controller
{
    class DatabaseController
    {
        private static object syncRoot = new object();
        private volatile static DatabaseController _Me;
        public static           DatabaseController  Me
        {
            get {
                    if (_Me == null)
                    {
                        lock (syncRoot)
                        {
                            if (_Me == null)
                                _Me = new DatabaseController();
                        }
                    }
                    return _Me;
            } }

        private DatabaseController() { DataBaseConnection.CreateTable<User>(); }

        // Store Part
        private volatile User _DUser;
        public           User  DUser
        {
            get {
                if (_DUser != null)
                    return _DUser;

                return _DUser = (from p in DataBaseConnection.Table<User>()
                                where p.Id == TargetId
                                select p).FirstOrDefault();
            }
            set {
                if (DUser == null)
                    DataBaseConnection.InsertOrReplace(_DUser = value);
                else
                    DataBaseConnection.Update(_DUser = value);
            } }

        public          List<ExamVR>            DExams            { get { return _DExams;     }        set { _DExams = value;         } }
        public volatile List<ExamVR>           _DExams; 
        public          List<ScheduleVR>        DSchedule         { get { return _DSchedule;  }        set { _DSchedule = value;      } }
        public volatile List<ScheduleVR>       _DSchedule;
        public          Journal                 DJournal          { get { return _DJournal;   }        set { _DJournal = value;       } }
        public volatile Journal                _DJournal;
        public          MessageDe               DMessageDe        { get { return _DMessageDe; }        set { _DMessageDe = value;     } }
        public volatile MessageDe              _DMessageDe;
        public          List<JournalChangeLog>  DJournalChangeLog { get { return _DJournalChangeLog; } set { _DJournalChangeLog = value; } }
        public volatile List<JournalChangeLog> _DJournalChangeLog;

        // DB part
        private static string       PathToBase = Path.Combine(ApplicationData.Current.LocalFolder.Path, "iTMO.db.sqlite");
        private SQLiteConnection    DataBaseConnection { get { return new SQLiteConnection(new SQLitePlatformWinRT(), PathToBase); } }
        private const int           TargetId = 1;
    }
}
