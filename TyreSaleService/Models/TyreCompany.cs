namespace TyreSaleService.Models
{
    /// <summary>
    /// Represents a tyre company with its properties.
    /// </summary>
    public class TyreCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TyreModel> Models { get; set; } = new();
    }
}
