using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Reflection;
using UI.Components;
using UI.Models;

namespace UI.Extensions
{
    public static class CategoriesExtesions
    {
        public static double[] ToArray(this CategoriesDistributionModel model)
        {
            if(model != null)
                return new double[] { model.Saves, model.Fees, model.Entertainment };
            else return new double[] { 1,1,98 };
        }

        public static string[] GetPropertyNames(this CategoriesDistributionModel model, IStringLocalizer localizer)
        {
            if (model != null)
            {
                var tab = model.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(prop => localizer[prop.Name] + " ")
                    .ToArray();

                tab[0] +=  Math.Round(model.Saves, 2).ToString() + "%";
                tab[1] += Math.Round(model.Fees, 2).ToString() + "%";
                tab[2] += Math.Round(model.Entertainment, 2).ToString() + "%";

                return tab;
            }
            else return new string[] { localizer["InvalidData"] };
        }
    }
}