using System;

namespace APICore.Common.DTO.Response
{
    public class LogResponse
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string LogType { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
    }
}
