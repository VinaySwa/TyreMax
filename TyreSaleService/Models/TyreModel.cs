using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TyreSaleService.Models
{
    /// <summary>
    /// Represents a tyre model with its properties.
    /// </summary>
    public class TyreModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }

        [JsonIgnore]
        [BindNever]
        [ValidateNever]
        public TyreCompany Company { get; set; }
        public List<Tyre> Tyres { get; set; } = new();
    }
}
