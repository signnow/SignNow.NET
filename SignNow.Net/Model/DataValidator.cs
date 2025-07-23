using System.Runtime.Serialization;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Field or text tag parameter that specifies data format for the input
    /// </summary>
    public enum DataValidator
    {
        /// <summary>
        /// A date formatted dd/mm/yyyy (e.g. 27/11/2008)
        /// </summary>
        [EnumMember(Value = "059b068ef8ee5cc27e09ba79af58f9e805b7c2b3")]
        DateEU,

        /// <summary>
        /// A date and time formatted mm/dd/yyyy hh:mm:ss AM/PM format (hh:mm:ss AM/PM is optional)
        /// </summary>
        [EnumMember(Value = "06448a0d0eb6a71c7c116ec4754bcb04ebf11da5")]
        DateTimeEU,

        /// <summary>
        /// A formatted DD-MON-YYYY, in all caps and a hyphen between values (e.g. 9-MAY-1981)
        /// </summary>
        [EnumMember(Value = "07c1e60f3da1192b60aca6f7e72d9b17a44539e5")]
        DateCaps,

        /// <summary>
        /// Time formatted hh:mm or h:mm. (e.g. 23:00)
        /// </summary>
        [EnumMember(Value = "09d3bb6a5eb6598edb7bfad02b0143d8c68ad788")]
        TimeOnly,

        /// <summary>
        /// Date formatted DD/MM/YY (e.g. 31/12/75 or 29/02/00)
        /// </summary>
        [EnumMember(Value = "0b61eb6a696da953910f195b30c86e5131f3ae3e")]
        DateShortEU,

        /// <summary>
        /// Date formatted Mmm dd, yyyy (e.g. Jan 1, 2003 or December 12, 1999).
        /// The month must be capitalized, there cannot be a period, and there must be a comma.
        /// </summary>
        [EnumMember(Value = "0f4827a308018f98b11ae3923104685ff0c03070")]
        DateWrittenEU,

        /// <summary>
        /// Date Only (e.g. 09/28/2008)
        /// </summary>
        [EnumMember(Value = "13435fa6c2a17f83177fcbb5c4a9376ce85befeb")]
        DateUS,

        /// <summary>
        /// Number, with or without decimal places or comma separators (e.g. 12345 or 1,234.50 or 1,234.05)
        /// </summary>
        [EnumMember(Value = "1109cfbbb06311a06a4c7f8d04f1f0d5c44103cb")]
        NumberWithDecimals,

        /// <summary>
        /// US phone number including area code (e.g. (123) 456-7890)
        /// </summary>
        [EnumMember(Value = "13cc1d661da456d27b249b73056ed4d1f2e72d8e")]
        PhoneNumberUS,

        /// <summary>
        /// US dollar amount with commas for multiples of 1,000 and a period for the decimal (e.g. $1,234.50 or $1,234.05)
        /// </summary>
        [EnumMember(Value = "150662c7221a6a6ebcbb7c50ca46359d19757f81")]
        CurrencyUS,

        /// <summary>
        /// 5 or 9 digit US zip code formatted xxxxx-xxxx or xxxxx (e.g. 92663 or 92663-1234)
        /// </summary>
        [EnumMember(Value = "1671f4eb87444a24e1e00f149bade8b7cf3af5da")]
        ZipCodeUS,

        /// <summary>
        /// Age in years (e.g. 29 or 68), with a maximum of 122
        /// </summary>
        [EnumMember(Value = "1a203fa91791b0458608be045a454ba90557fb26")]
        AgeInYears,

        /// <summary>
        /// Positive number using only digits (eg. 12345)
        /// </summary>
        [EnumMember(Value = "1f9486ae822d30ba3df2cb8e65303ebfb8c803e8")]
        PositiveNumber,

        /// <summary>
        /// Positive or negative integer (e.g. 1234 or -123 or +123)
        /// </summary>
        [EnumMember(Value = "23a57c29fa089e22bcf85d601c8091bc9c7da570")]
        PositiveOrNegativeNumber,

        /// <summary>
        /// Social Security Number (e.g. 078-05-1120 or 078051120)
        /// </summary>
        [EnumMember(Value = "2cd795fd64ce63b670b52b2e83457d59ac796a39")]
        SocialSecurityNumber,

        /// <summary>
        /// Credit card number with hyphens. Example : xxxx-xxxx-xxxx-xxxx for VISA/Master or xxxx-xxxxxx-xxxxx for AMEX
        /// </summary>
        [EnumMember(Value = "2f1c408bdf2f99fc5d4c342249da88ce5d2a5f02")]
        CreditCardNumber,

        /// <summary>
        /// US state, as you would for the Post Office (e.g. AL or CA)
        /// </summary>
        [EnumMember(Value = "3123849de563f9e14acacc2739467e3d30e426b6")]
        StateUS,

        /// <summary>
        /// Letters and/or numbers, with no special characters (e.g. abc123 or 123abc)
        /// </summary>
        [EnumMember(Value = "3859296fffd39cb8efeaffda5899973c014ce42e")]
        AlphaNumeric,

        /// <summary>
        /// email address (e.g john+1@gmail.com or john123@gmail.com)
        /// </summary>
        [EnumMember(Value = "7cd795fd64ce63b670b52b2e83457d59ac796a39")]
        EmailAddress,

        /// <summary>
        /// Bank Routing Transit Number(e.g.614321634)
        /// </summary>
        [EnumMember(Value = "959b068ef8ee5cc27e09ba79af58f9e805b7c2b3")]
        BankRoutingNumber,

        /// <summary>
        /// Currency amount (e.g. 73,234.55)
        /// </summary>
        [EnumMember(Value = "7ef095fd94ce63b670b52b2e83457d59ac796a39")]
        CurrencyWithDotForDecimals,

        /// <summary>
        /// Currency amount (e.g. 73.234,55)
        /// </summary>
        [EnumMember(Value = "824085fd04ce63b670b11b2e83457d59ac796a39")]
        CurrencyWithCommaForDecimals,

        /// <summary>
        /// Positive or Negative numbers in US Format
        /// </summary>
        [EnumMember(Value = "f031b8b63127c6915949bfc55e3cc81edbb12ecc")]
        PositiveOrNegativeNumberUS,

        /// <summary>
        /// Positive Integers for formula calculations in US Format
        /// </summary>
        [EnumMember(Value = "4cf9dfe7e65eb0a2c0c93e199f1ec1cdc23a267d")]
        PositiveNumberUS,

        /// <summary>
        /// Positive or Negative numbers in EU Format
        /// </summary>
        [EnumMember(Value = "e8b7248b323525d1e8d5240ccf87d8c2926622cd")]
        PositiveOrNegativeNumberEU,

        /// <summary>
        /// Positive Integers for formula calculations in EU Format
        /// </summary>
        [EnumMember(Value = "8937f6d0511167d67c6b21fe07c9ccd3a8116126")]
        PositiveNumberEU,
    }
}
