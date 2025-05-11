using System.ComponentModel.DataAnnotations;

public enum PaymentMethod
{
    [Display(Name = "Cash")]
    Cash,

    [Display(Name = "Credit/Debit Card")]
    Card,

    [Display(Name = "Bank Transfer")]
    Bank,

    [Display(Name = "PayPal")]
    PayPal,

    [Display(Name = "Cryptocurrency")]
    Crypto
}
