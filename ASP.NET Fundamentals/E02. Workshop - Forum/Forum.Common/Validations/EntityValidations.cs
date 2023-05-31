namespace Forum.Common.Validations
{
    public static class EntityValidations
    {
        public static class Post
        {
            public const int TitleMinLength = 10;
            public const int TitleMaxLength = 50;
            public const int ContentMinLength = 30;
            public const int ContentMaxLength = 1500;
        }
    }
}
