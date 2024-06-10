namespace Razor.Models
{
    public class AddProductViewModel
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public string Subtitle { get; set; }
        public int AmountOf { get; set; }
        public decimal Price { get; set; }
        public int UserId { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<string> Photos { get; set; }
    }
}
