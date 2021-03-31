using System.ComponentModel.DataAnnotations;

namespace FakeCSV.Domain.Models
{
    public enum ColumnSeparator
    {
        [Display(Name = "Comma (,)")]
        Comma,
        [Display(Name = "Semicolon (;)")]
        Semicolon,
    }
}