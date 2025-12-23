namespace Suprema_Api_Using_Protos.DTOs
{
    public class LookUpsDTO<T>
    {
        public string Name { get; init; } = string.Empty;
        public T Value { get; init; }
    }
}
