using System;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinUserInfoJsonExtensions
{
    public static DateTime? SubscribeTimeAsDateTime(this WeixinUserInfoJson obj)
    {
        return obj.SubscribeTime == null ? null : new UnixDateTime(obj.SubscribeTime!.Value);
    }

    public static WeixinUserInfoJson SubscriberTimeFromDateTime(this WeixinUserInfoJson obj, DateTime? value)
    {
        obj.SubscribeTime = value.ToUnixTime();
        return obj;
    }

    public static WeixinGender GenderAsWeixinGender(this WeixinUserInfoJson obj)
    {
        return WeixinGenderHelper.FromInt(obj.Sex);
    }

    public static WeixinUserInfoJson GenderFromWeixinGender(this WeixinUserInfoJson obj, WeixinGender value)
    {
        obj.Sex = value.ToInt();
        return obj;
    }

    public static WeixinLanguage LanguageAsWeixinLanguage(this WeixinUserInfoJson obj)
    {
        return new WeixinLanguage(obj.Language);
    }

    public static WeixinUserInfoJson LanguageFromWeixinLanguage(this WeixinUserInfoJson obj, WeixinLanguage value)
    {
        obj.Language = value.Code;
        return obj;
    }

    public static bool SubscribeAsBool(this WeixinUserInfoJson obj)
    {
        return obj.Subscribe == 1;
    }

    public static WeixinUserInfoJson SubscriberFromBool(this WeixinUserInfoJson obj, bool value)
    {
        obj.Subscribe = (value ? 1 : 0);
        return obj;
    }

}
