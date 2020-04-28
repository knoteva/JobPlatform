namespace SpiceHouse.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "SpiceHouse";

        public const string AdministratorRoleName = "Administrator";
        public const string CustomerRoleName = "Customer";

        public const string DefaultFoodImage = "default_food.png";

        public const string DefaultCouponImage = "default_coupon.png";

        public const string SsCarItemsCount = "ssCarItemsCount";

        public static string ConvertToRawHtml(string source)
        {
            if (source == null)
            {
                source = string.Empty;
            }

            var array = new char[source.Length];
            var arrayIndex = 0;
            var inside = false;

            foreach (var @let in source)
            {
                switch (@let)
                {
                    case '<':
                        inside = true;
                        continue;
                    case '>':
                        inside = false;
                        continue;
                }

                if (!inside)
                {
                    array[arrayIndex] = @let;
                    arrayIndex++;
                }
            }

            return new string(array, 0, arrayIndex);
        }
    }
}
