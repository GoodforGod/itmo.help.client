using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iTMO.Help.Model
{
    /// <summary>
    /// Data object inteface, may be usefull in future, or.. true OOP
    /// </summary>
    interface IData
    {
        bool    isValid { get; set; }        
        string  Message { get; set; }
    }

    /// <summary>
    /// Common generic for data objects
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    class CommonData<TValue> : IData
    {
        public TValue   Data    { get; set; }
        public bool     isValid { get; set; } = false;
        public string   Message { get; set; } = "";
    }

    /// <summary>
    /// HttpController response object
    /// </summary>
    class HttpData<TValue> : CommonData<TValue>
    {
        public HttpStatusCode   Code    { get; set; } = HttpStatusCode.Forbidden;
    }

    /// <summary>
    /// Serialisation response with valid state and exception method
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    class SerializeData<TValue> : CommonData<TValue> { }

    /// <summary>
    /// Used as response from methods which try to CheckOrAndRemember user's info
    /// </summary>
    class UserData : CommonData<UserAccount>
    {
        public bool isRemember  { get; set; } = false;

        public UserData()
        {
            Data = new UserAccount();
        }

        public UserData(List<string> opts)
        {
            this.Data.Opts = opts;
        }

        public UserData(string group) : this()
        {
            Data.Group = group;
        }

        public UserData(string login, string password) : this()
        {
            Data.Login = login;
            Data.Password = password;
        }

        public UserData(string login, string password, string group) : this(login, password)
        {
            Data.Group = group;
        }
    }

    /// <summary>
    /// Used for Main menu <see cref="MainPage"/>
    /// </summary>
    class MenuItem
    {
        public string Title { get; set; } = "";
        public string Icon  { get; set; } = "";
        public Type   Page  { get; set; }
    }

    /// <summary>
    /// Used as Term select item on the <see cref="JournalHub"/> term ComboBox
    /// </summary>
    class TermItem
    {
        public string Term { get; set; } = "";
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
}
