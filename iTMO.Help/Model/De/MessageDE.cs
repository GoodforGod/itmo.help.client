﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTMO.Help.Model
{
    class MessageDe
    {
        public int      Id              { get; set; }
        public string   Sender          { get; set; }
        public string   Topic           { get; set; }
        public string   Text            { get; set; }
        public string   Date            { get; set; }
        public bool     isRead          { get; set; }
        public bool     isFileAttached  { get; set; }
        public string   FileName        { get; set; }
    }
}