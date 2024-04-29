namespace PedidosApi.Domain.Extensions
{
    public static class ListExtensions
    {
        public static string ToString<T>(this List<T> list, string separator) =>
            string.Join(separator, list);
    }
}
