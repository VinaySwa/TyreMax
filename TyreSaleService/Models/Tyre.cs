using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TyreSaleService.Models
{
    /// <summary>
    /// Represents a tyre with its properties.
    /// </summary>
    public class Tyre
    {
        public int Id { get; set; }
        public TyreDimensions Dimensions { get; set; }
        public int ModelId { get; set; }

        [JsonIgnore]
        [BindNever]
        [ValidateNever]
        public TyreModel Model { get; set; }
        public decimal Price { get; set; }
        public double DiscountPercentage { get; set; }
        public int LoadIndex { get; set; }
        public string SpeedIndex { get; set; }
        public bool Availability { get; set; }
    }
}
