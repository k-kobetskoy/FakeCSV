using System.ComponentModel.DataAnnotations;

namespace FakeCSV.Domain.Models
{
    public enum QuotationMark 
    {
        [Display(Name = "Double-quote (\")")]
        DoubleQuote,
        [Display(Name = "Single-quote (\')")]
        SiningleQuote,
        [Display(Name = "Back - quote(`)")]
        BackQuote
    }
}