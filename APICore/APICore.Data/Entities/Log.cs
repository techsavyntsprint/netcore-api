using APICore.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace APICore.Data.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public EventTypeEnum EventType { get; set; }
        public LogTypeEnum LogType { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string App { get; set; }
        public string Module { get; set; }
    }
}