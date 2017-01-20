namespace iTMO.Help.Model
{
    class MessageDe
    {
        public int      Id              { get; set; } = -1;
        public string   Sender          { get; set; } = "";
        public string   Topic           { get; set; } = "";
        public string   Text            { get; set; } = "";
        public string   Date            { get; set; } = "";
        public bool     isRead          { get; set; } = false;
        public bool     isFileAttached  { get; set; } = false;
        public string   FileName        { get; set; } = "";
    }
}
