using Microsoft.ML.Data;

namespace VisionaryAI.API.Models
{
    public class ResultadoPrevisao
    {
        [ColumnName("Score")]
        public float Tendencia { get; set; }
    }
}
