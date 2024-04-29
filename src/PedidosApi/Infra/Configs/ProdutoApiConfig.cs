namespace PedidosApi.Infra.Configs
{
    public record ProdutoApiConfig
    {
        public string Protocol { get; set; }
        public string BaseUrl { get; set; }
        public string Port { get; set; }
        public int ParallelCallCount { get; set; }
        public string Token { get; set; }
    }
}
