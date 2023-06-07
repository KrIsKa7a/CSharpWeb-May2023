namespace TaskBoardApp.Common
{
    public static class EntityValidationConstants
    {
        public static class Task
        {
            public const int TitleMinLength = 5;
            public const int TitleMaxLength = 70;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 1000;
        }

        public static class Board
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
        }
    }
}