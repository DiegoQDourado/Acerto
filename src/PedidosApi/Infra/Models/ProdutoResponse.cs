namespace PedidosApi.Infra.Models
{
    public class ProdutoResponse
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}
