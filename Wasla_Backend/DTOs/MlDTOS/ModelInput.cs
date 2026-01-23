
namespace Wasla_Backend.DTOs.MlDTOS
{
    public class ModelInput
    {
        [LoadColumn(0)]
        public string Tweet { get; set; } 

        [LoadColumn(1)]
        public string Class { get; set; }
    }
}
