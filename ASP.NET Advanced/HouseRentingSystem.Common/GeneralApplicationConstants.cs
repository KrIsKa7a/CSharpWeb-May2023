namespace HouseRentingSystem.Common
{
    public static class GeneralApplicationConstants
    {
        public const int ReleaseYear = 2023;

        public const int DefaultPage = 1;
        public const int EntitiesPerPage = 3;

        public const string AdminAreaName = "Admin";
        public const string AdminRoleName = "Administrator";
        public const string DevelopmentAdminEmail = "admin@houserenting.bg";

        public const string UsersCacheKey = "UsersCache";
        public const string RentsCacheKey = "RentsCache";
        public const int UsersCacheDurationMinutes = 5;
        public const int RentsCacheDurationMinutes = 10;

        public const string OnlineUsersCookieName = "IsOnline";
        public const int LastActivityBeforeOfflineMinutes = 10;
    }
}