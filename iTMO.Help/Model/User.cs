using SQLite.Net.Attributes;

namespace iTMO.Help.Model
{
    class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id                   { get; set; }
        [Unique]
        public string Login             { get; set; }
        public string Password          { get; set; }
        public string Group             { get; set; }
        public string GroupLastUsed     { get; set; }

        public bool isAutoLogin         { get; set; }
        public bool isAutoTermSelect    { get; set; }
        public bool isAutoGroupSelect   { get; set; }
        public bool isNotified          { get; set; }
    }
}
