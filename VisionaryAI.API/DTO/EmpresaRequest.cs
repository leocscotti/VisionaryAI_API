namespace VisionaryAI.API.DTO
{
    public class EmpresaRequest
    {
        public string Cnpj { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Descricao { get; set; }
        public int CidadeId { get; set; }
    }
}
