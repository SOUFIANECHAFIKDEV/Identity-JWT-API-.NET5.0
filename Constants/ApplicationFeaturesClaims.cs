namespace IdentityAPI.Constants
{
    public static class ApplicationFeaturesClaims
    {
        public const string ClaimName = "AuthorizedApplicationFeatures";
        private const string Base = "access";

        private const string Module1Base = Base + "." + "module2";
        private const string Module2Base = Base + "." + "module1";

        public static int MyProperty { get; set; }
        public static class Module1
        {
            public const string Feature1 = Module1Base + ".feature1";
            public const string Feature2 = Module1Base + ".feature2";
            public const string Feature3 = Module1Base + ".feature3";
        }

        public static class Module2
        {
            public const string Feature1 = Module2Base + ".feature1";
            public const string Feature2 = Module2Base + ".feature2";
            public const string Feature3 = Module2Base + ".feature3";
        }
    }
}