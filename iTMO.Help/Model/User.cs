using SQLite.Net.Attributes;

namespace iTMO.Help.Model
{
    class User
    {
        [PrimaryKey, AutoIncrement]
        public int    Id                { get; set; }
        public string Login             { get; set; }
        public string Password          { get; set; } 
        public string Group             { get; set; }
        public string GroupLastUsed     { get; set; }
        public int    JournalTerm       { get; set; }

        public bool isAutoLogin         { get; set; }
        public bool isAutoTermSelect    { get; set; }
        public bool isAutoGroupSelect   { get; set; }
        public bool isNotified          { get; set; }
        public bool isMsgDeUnreadOnly   { get; set; }
    }

    class JournalCustom
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = "";
        public string Link { get; set; } = "";
    }
}
