using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace iTMO.Help.Model
{
    class User
    {
        [PrimaryKey, Unique]
        public string Login     { get; set; }
        public string Password  { get; set; }
        public string Group     { get; set; }

        public bool isAutoLogin         { get; set; }
        public bool isAutoTermSelect    { get; set; }
        public bool isAutoGroupSelect   { get; set; }
        public bool isNotified          { get; set; }
    }
}
