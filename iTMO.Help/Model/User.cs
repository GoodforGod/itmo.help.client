using SQLite.Net.Attributes;

using iTMO.Help.View;

namespace iTMO.Help.Model
{
    /// <summary>
    /// Main user object
    /// </summary>
    class User
    {
        [PrimaryKey, AutoIncrement]
        public int    Id                { get; set; }
        public string Login             { get; set; }
        public string Password          { get; set; } 
        public string Group             { get; set; }
        public string GroupLastUsed     { get; set; }
        public int    JournalTerm       { get; set; }
        public int    MenuLastSelected  { get; set; } = 0;

        public bool isAutoTermSelect    { get; set; }
        public bool isNotified          { get; set; }
        public bool isMsgDeUnreadOnly   { get; set; }
    }

    /// <summary>
    /// Custom journal object, used to store link to user's custom journals
    /// 
    /// EXAMPLE (Like Google Doc, where tutor stores points/scores instead of DE)
    /// </summary>
    class JournalCustom
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = "";
        public string Link { get; set; } = "";
    }

    /// <summary>
    /// Used to Create/Change objects locale state
    /// </summary>
    enum LanguageOption
    {
        EN = 0,
        RU = 1,
        CN = 2 // Reserved for China, If some Chieese want to help, contact me, lets push chinese dictionary together
    }

    /// <summary>
    /// Used as Term select item on the <see cref="JournalHub"/> term ComboBox
    /// </summary>
    class TermItem
    {
        public string Term { get; set; } = "";
    }
}
