namespace SpiceHouse.Web.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class EnumerableExtension
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                select new SelectListItem
                {
                    Text = item.GetPropertyValue("Name"),
                    Value = item.GetPropertyValue("Id"),
                    Selected = item.GetPropertyValue("Id").Equals(selectedValue.ToString()),
                };
        }
    }
}
