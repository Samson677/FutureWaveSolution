namespace FutureWave.Api.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public byte[]? Image { get; set; }
        public int CategoryId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);


    }
}
