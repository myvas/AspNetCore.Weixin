using System;
using System.ComponentModel.DataAnnotations;
using Myvas.AspNetCore.Weixin.Api.Properties;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Gender (Defined by Tencent)
/// </summary>
public enum WeixinGender
{
    /// <summary>
    /// Male
    /// </summary>
    [Display(Name = "Male")]
    Male = 1,

    /// <summary>
    /// Female
    /// </summary>
    [Display(Name = "Female")]
    Female = 2,

    /// <summary>
    /// Unknown
    /// </summary>
    [Display(Name = "Unknown")]
    Unknown = 0
}

public static class WeixinGenderExtensions
{
    /// <summary>
    /// The <see cref="int"/> value defined to store in the database
    /// </summary>
    /// <param name="value">The value defined by Tencent, which male is 1, female is 2, unknown is 0</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static int ToInt(this WeixinGender value)
    {
        return (int)value;
    }

    public static string ToDisplayName(this WeixinGender value)
    {
        return value switch
        {
            WeixinGender.Male => Resources.GenderMale,
            WeixinGender.Female => Resources.GenderFemale,
            WeixinGender.Unknown => Resources.GenderUnknown,
            _ => throw new NotSupportedException(),
        };
    }
}

// NOTE: Do not use extensions for standard data type
public static class WeixinGenderHelper
{
    public static WeixinGender FromInt(int? value)
    {
        if (value == null) return WeixinGender.Unknown;
        return value!.Value switch
        {
            1 => WeixinGender.Male,
            2 => WeixinGender.Female,
            _ => WeixinGender.Unknown,
        };
    }
}

