namespace APICore.Common.DTO.Response
{
    public class HealthCheckResponse
    {
        public int ServiceStatus { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Exception { get; set; }
        public string Duration { get; set; }
    }
}