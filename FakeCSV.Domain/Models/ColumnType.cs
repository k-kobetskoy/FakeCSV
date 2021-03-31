using System.ComponentModel.DataAnnotations;

namespace FakeCSV.Domain.Models
{
    public enum ColumnType
    {
        [Display(Name = "Full name")]
        FullName,
        Job,
        Email,
        [Display(Name = "Domain name")]
        DomainName,
        [Display(Name = "Phone number")]
        PhoneNumber,
        [Display(Name = "Company name")]
        CompanyName,
        Text,
        Integer,
        Address,
    }
}