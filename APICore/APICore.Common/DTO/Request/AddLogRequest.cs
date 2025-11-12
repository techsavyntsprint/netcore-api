namespace APICore.Common.DTO.Request
{
    public class AddLogRequest
    {        
        public int EventType { get; set; }
        public int LogType { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string App { get; set; }
        public string Module { get; set; }       
    }
}
