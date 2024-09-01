using System.ComponentModel;

namespace UI.Models
{
    public enum TransactionCategoryEnum
    {
        [Description("Oszczędności")]
        Saves = 0,
        [Description("Opłaty")]
        Fees = 1,
        [Description("Rozrywka")]
        Entertainment = 2
    }
}
