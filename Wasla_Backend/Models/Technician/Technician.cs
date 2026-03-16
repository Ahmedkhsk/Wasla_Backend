namespace Wasla_Backend.Models.technician
{
    public class Technician : ServiceProvider
    {
        public TechnicianSpecialty? Specialty { get; set; }
        public int? ExperienceYears { get; set; }
        public bool IsAvailable { get; set; }
        public string? DocumentsJson { get; set; }

        [NotMapped]
        public List<string>? Documents
        {
            get => DocumentsJson == null
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(DocumentsJson);

            set => DocumentsJson = JsonSerializer.Serialize(value);
        }
    }
}
