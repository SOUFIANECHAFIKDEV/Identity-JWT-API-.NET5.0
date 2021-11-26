namespace IdentityAPI.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "api";

        public const string Base = "api";

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string Refresh = Base + "/identity/refresh";
            public const string AddUserToRoles = Base + "/identity/AddUserToRoles";
            public const string ConfirmEmail = Base + "/identity/ConfirmEmail";
            public const string ChangeEmail = Base + "/identity/ChangeEmail";
            public const string SendChangeEmailConfirmation = Base + "/identity/SendChangeEmailConfirmation";
            public const string ResetPassword = Base + "/identity/ResetPassword";
            public const string SendResetPasswordConfirmationEmail = Base + "/identity/SendResetPasswordConfirmationEmail";
            public const string GetAllUsers = Base + "/identity/GetAllUsers";
            public const string GetUserById = Base + "/identity/GetUserById/{userId}";
            public const string UpdateUserProfile = Base + "/identity/UpdateUserProfile/{userId}";
            public const string DeleteUserProfile = Base + "/identity/DeleteUserProfile/{userId}";
            public const string GetRolesList = Base + "/identity/GetRolesList";
            public const string GetUserRolesById = Base + "/identity/GetUserRolesById/{userId}";
            public const string GetApplicationFeatures = Base + "/identity/GetApplicationFeatures";
            public const string GetUserAuthorizedFeatures = Base + "/identity/GetUserAuthorizedFeatures";
            public const string UpdateAuthorizedFeatures = Base + "/identity/UpdateAuthorizedFeatures";
            public const string ConfirmPhoneNumber = Base + "/identity/ConfirmPhoneNumber";
            public const string SendPhoneVerification = Base + "/identity/SendPhoneVerification";
            public const string GetUserOwnsProfile = Base + "/identity/GetUserOwnsProfile";
            public const string UpdateUserOwnsProfile = Base + "/identity/UpdateUserOwnsProfile";
        }
    }
}
