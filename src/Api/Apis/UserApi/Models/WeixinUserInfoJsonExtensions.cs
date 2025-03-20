using System;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinUserInfoJsonExtensions
{
    public static DateTime? SubscribeTimeAsDateTime(this WeixinUserInfoJson obj)
    {
        return obj.SubscribedTime == null ? null : new UnixDateTime(obj.SubscribedTime!.Value);
    }

    public static WeixinUserInfoJson SubscriberTimeFromDateTime(this WeixinUserInfoJson obj, DateTime? value)
    {
        obj.SubscribedTime = value.ToUnixTime();
        return obj;
    }

    public static WeixinGender GenderAsWeixinGender(this WeixinUserInfoJson obj)
    {
        return WeixinGenderHelper.FromInt(obj.Gender);
    }

    public static WeixinUserInfoJson GenderFromWeixinGender(this WeixinUserInfoJson obj, WeixinGender value)
    {
        obj.Gender = value.ToInt();
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
        return obj.Subscribed == 1;
    }

    public static WeixinUserInfoJson SubscriberFromBool(this WeixinUserInfoJson obj, bool value)
    {
        obj.Subscribed = (value ? 1 : 0);
        return obj;
    }

}
