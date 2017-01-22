namespace iTMO.Help.Model
{
    class MessageDe
    {
        public int      id              { get; set; } = -1;
        public string   sender          { get; set; } = "";
        public string   topic           { get; set; } = "";
        public string   text            { get; set; } = "";
        public string   date            { get; set; } = "";
        public bool     isRead          { get; set; } = false;
        public bool     isFileAttached  { get; set; } = false;
        public string   fileName        { get; set; } = "";
    }
}
