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
    /// <summary>
    /// Used to communicate with SQLLite database as singleton object
    /// </summary>
    class DatabaseController
    {
        private static object syncRoot = new object();
        private volatile static DatabaseController _Me;
        public  static          DatabaseController  Me
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

            var journals = GetCustomJournals();

            if (journals == null || journals.Count == 0)
                SaveCustomJournal(new JournalCustom() { Id = 1, Link = "http://www.ifmo.ru/ru/", Name = "ifmo" });

            if (DUser == null)
                DataBaseConnection.InsertOrReplace(new User { Id = 1 });
        }

        /// <summary>
        /// Application Cache/Store objects, used to cache JSON reprisantation/HTML responses or other DB info
        /// </summary>
        private volatile User _DUser;
        public           User  DUser
        {
            get
            {
                if (_DUser != null)
                    return _DUser;

                return _DUser = DataBaseConnection.Table<User>().FirstOrDefault();
            }
            set {
                DataBaseConnection.Update(_DUser = value);
            }
        }
        
        private volatile AttestationDe _AttestationDe = null;
        public           AttestationDe DAttestationDe {
            get { return _AttestationDe; }
            set { _AttestationDe = value; }
        }

        private volatile LanguageOption _LangOpt = LanguageOption.EN;
        public           LanguageOption LangOpt {
            get { return _LangOpt; }
            set { _LangOpt = value; }
        }

        private volatile int _TermLastSelectedIndex = 0;
        public int            TermLastSelectedIndex {
            get { return _TermLastSelectedIndex; }
            set { _TermLastSelectedIndex = value; }
        }

        private volatile int _GroupLastSelectedIndex = -1;
        public int            GroupLastSelectedIndex {
            get { return _GroupLastSelectedIndex; }
            set { _GroupLastSelectedIndex = value; }
        }

        private volatile List<JournalCustom> _DJournalCustom = null;
        public           List<JournalCustom> DJournalCustom {
            get { return _DJournalCustom; }
            set { _DJournalCustom = value; }
        }

        public volatile List<ExamVR> _DExams = null;
        public          List<ExamVR> DExams {
            get { return _DExams; }
            set { _DExams = value; }
        }

        public volatile List<ScheduleVR> _DSchedule = null;
        public          List<ScheduleVR> DSchedule {
            get { return _DSchedule;  }
            set { _DSchedule = value; }
        }

        public volatile Journal _DJournal = null;
        public          Journal DJournal {
            get { return _DJournal; }
            set { _DJournal = value; }
        }

        public volatile List<MessageDe> _DMessageDe = null;
        public          List<MessageDe> DMessageDe {
            get { return _DMessageDe; }
            set { _DMessageDe = value; }
        }

        public volatile List<JournalChangeLog> _DJournalChangeLog = null;
        public          List<JournalChangeLog> DJournalChangeLog {
            get { return _DJournalChangeLog; }
            set { _DJournalChangeLog = value; }
        }

        /// <summary>
        /// This part, where we communicate with SQLLite database
        /// </summary>
        private const int           UserTargetId = 1;
        private static string       PathToBase = Path.Combine(ApplicationData.Current.LocalFolder.Path, "iTMO.db.sqlite");
        private SQLiteConnection    DataBaseConnection {
            get { return new SQLiteConnection(new SQLitePlatformWinRT(), PathToBase); }
        }

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

        public void DeleteCustomJournal(int id)
        {
            var item = DJournalCustom.Find(temp => temp.Id == id);
            _DJournalCustom.Remove(item);
            DataBaseConnection.Delete(item);
        }
    }
}
