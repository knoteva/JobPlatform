namespace SpiceHouse.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "SpiceHouse";

        public const string AdministratorRoleName = "Administrator";
        public const string CustomerRoleName = "Customer";

        public const string DefaultFoodImage = "default_food.png";

        public const string SsCarItemsCount = "ssCarItemsCount";

        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
