using iTMO.Help.Model;
using iTMO.Help.Model.ViewReady;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        private DatabaseController()
        {
            DataBaseConnection.CreateTable<User>();
            DataBaseConnection.CreateTable<JournalCustom>();

            if (DUser == null)
                DataBaseConnection.InsertOrReplace(new User { Id = 1 });
        }

        // Cache/Store Part
        private volatile User _DUser;
        public           User  DUser
        {
            get {
                if (_DUser != null)
                    return _DUser;

                return _DUser = DataBaseConnection.Table<User>().FirstOrDefault();
                    /*(from p in DataBaseConnection.Table<User>()
                                where p.Id == UserTargetId
                                select p).FirstOrDefault();
                                */
            }
            set {
                DataBaseConnection.Update(_DUser = value);
            } }

        public           List<JournalCustom>    DJournalCustom    { get { return _DJournalCustom; } set { _DJournalCustom = value; } }
        private volatile List<JournalCustom>  _DJournalCustom = null;

        public          List<ExamVR>           DExams            { get { return _DExams;     } set { _DExams = value;         } }
        public volatile List<ExamVR>          _DExams = null;

        public          List<ScheduleVR>       DSchedule         { get { return _DSchedule;  } set { _DSchedule = value;      } }
        public volatile List<ScheduleVR>      _DSchedule = null;

        public          Journal                DJournal          { get { return _DJournal;   } set { _DJournal = value;       } }
        public volatile Journal               _DJournal = null;

        public          MessageDe              DMessageDe        { get { return _DMessageDe; } set { _DMessageDe = value;     } }
        public volatile MessageDe             _DMessageDe = null;

        public          List<JournalChangeLog>  DJournalChangeLog { get { return _DJournalChangeLog; } set { _DJournalChangeLog = value; } }
        public volatile List<JournalChangeLog> _DJournalChangeLog = null;

        // DB part
        private static string       PathToBase = Path.Combine(ApplicationData.Current.LocalFolder.Path, "iTMO.db.sqlite");
        private SQLiteConnection    DataBaseConnection { get { return new SQLiteConnection(new SQLitePlatformWinRT(), PathToBase); } }
        private const int           UserTargetId = 1;

        public List<JournalCustom> GetCustomJournals()
        {
            if (DJournalCustom != null && DJournalCustom.Count != 0)
                return DJournalCustom;
            try                             { return DJournalCustom = DataBaseConnection.Table<JournalCustom>().ToList(); }
            catch(ArgumentNullException ex) { return new List<JournalCustom>(); }
        }
        
        public void SaveCustomJournal(JournalCustom newJournal)
        {
            _DJournalCustom.Add(newJournal);
            DataBaseConnection.InsertOrReplace(newJournal);
        }
    }
}
