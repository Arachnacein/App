using System.Reflection;
using UI.Models;

namespace UI.Extensions
{
    public static class CategoriesExtesions
    {
        public static double[] ToArray(this CategoriesDistributionModel model)
        {
            if(model != null)
                return new double[] { model.Saves, model.Fees, model.Enterntainment };
            else return new double[] { 1,1,98 };
        }

        public static string[] GetPropertyNames(this CategoriesDistributionModel model)
        {
            if (model != null)
            {
                var tab = model.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(prop => prop.Name + " ")
                    .ToArray();

                tab[0] +=  Math.Round(model.Saves, 2).ToString() + "%";
                tab[1] += Math.Round(model.Fees, 2).ToString() + "%";
                tab[2] += Math.Round(model.Enterntainment, 2).ToString() + "%";

                return tab;
            }
            else return new string[] { "Invalid data" };
        }
    }
}