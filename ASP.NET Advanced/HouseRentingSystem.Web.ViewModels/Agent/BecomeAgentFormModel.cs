namespace HouseRentingSystem.Web.ViewModels.Agent
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.Agent;

    public class BecomeAgentFormModel
    {
        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Phone]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; } = null!;
    }
}
