using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using PinewoodTechnologies.Code;
using System.ComponentModel.DataAnnotations;

namespace PinewoodTechnologies.Models
{
    // A separate model from the database to allow for validation and additional functions to be added
    // without causing issues with the base model
    public class ViewCustomer
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="*")]
        [RegularExpression(pattern:GlobalVars.name, ErrorMessage = "*Invalid")]
        public string Forename { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression(pattern: GlobalVars.name, ErrorMessage = "*Invalid")]
        public string Surname { get; set; }

        public string? Organisationname { get; set; }

        public string Contactnumber { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression(pattern: GlobalVars.reg_email, ErrorMessage = "*Invalid")]
        public string Email { get; set; }

        // For reusability the address is its own Model
        public ViewAddress address { get; set; }
    }
}
