namespace Forum.ViewModels.Post
{
    using System.ComponentModel.DataAnnotations;

    using static Forum.Common.Validations.EntityValidations.Post;

    public class PostFormModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; } = null!;
    }
}
