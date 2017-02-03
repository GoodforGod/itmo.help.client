using SQLite.Net.Attributes;

using iTMO.Help.View;
using System.Collections.Generic;

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
    /// User account object, to use in HttpRequest proceed
    /// </summary>
    class UserAccount
    {
        public string       Login    { get; set; } = "";
        public string       Password { get; set; } = "";
        public string       Group    { get; set; } = "";
        public List<string> Opts     { get; set; } = new List<string>();
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
}
