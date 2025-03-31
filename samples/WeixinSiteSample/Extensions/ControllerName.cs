using Microsoft.AspNetCore.Mvc;
using System;

using Myvas.AspNetCore.Weixin;

public static class ControllerName
{
    private const string ControllerSuffix = "Controller";
    private const int ControllerSuffixLength = 10;

    /// <summary>
    /// Gets the friendly name of a controller (without "Controller" suffix)
    /// </summary>
    /// <example>
    /// string name = ControllerName.Of<HomeController>(); // returns "Home"
    /// </example>
    public static string Of<T>() where T : Controller
    {
        return TrimSuffix(typeof(T).Name);
    }

    /// <summary>
    /// Gets the friendly name of a controller from its type
    /// </summary>
    public static string Of(Type controllerType)
    {
        if (!typeof(Controller).IsAssignableFrom(controllerType))
        {
            throw new ArgumentException($"Type must inherit from {nameof(Controller)}", nameof(controllerType));
        }
        return TrimSuffix(controllerType.Name);
    }

    private static string TrimSuffix(ReadOnlySpan<char> name)
    {
        return name.EndsWith(ControllerSuffix, StringComparison.Ordinal)
            ? name.Slice(0, name.Length - ControllerSuffixLength).ToString()
            : name.ToString();
    }
}