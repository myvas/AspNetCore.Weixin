using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin;

public class WeixinDailyStat
{
    public List<DeviceStat> statList = new List<DeviceStat>();
}

public class DeviceStat
{
    /// <summary>
    /// AP设备编号
    /// <para>例如：“deviceno1”</para>
    /// </summary>
    public string deviceNo;

    /// <summary>
    /// 手机端 302传统认证页面的UV
    /// <para>例如：10</para>
    /// </summary>
    public int phoneUv;

    /// <summary>
    /// 手机端 302传统认证页面的PV
    /// <para>例如：20</para>
    /// </summary>
    public int phonePv;

    /// <summary>
    /// 手机端传统方式上网的登录人数
    /// <para>例如：10</para>
    /// </summary>
    public int phoneLoginPersons;

    /// <summary>
    /// 手机端传统方式上网的登录次数
    /// <para>例如：12</para>
    /// </summary>
    public int phoneLoginTimes;

    /// <summary>
    /// PC端 302传统认证页面的UV
    /// <para>例如：10</para>
    /// </summary>
    public int pcUv;

    /// <summary>
    /// PC端 302传统认证页面的PV
    /// <para>例如：20</para>
    /// </summary>
    public int pcPv;

    /// <summary>
    /// PC端传统方式上网的登录人数
    /// <para>例如：10</para>
    /// </summary>
    public int pcLoginPersons;

    /// <summary>
    /// PC端传统方式上网的登录次数
    /// <para>例如：12</para>
    /// </summary>
    public int pcLoginTimes;

    /// <summary>
    /// AP故障持续时间，单位：秒
    /// </summary>
    public int failureTimes;
}
