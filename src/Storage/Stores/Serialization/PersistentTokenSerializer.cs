using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin.Stores.Serialization;

/// <summary>
/// JSON-based persisted token serializer
/// </summary>
/// <seealso cref="IPersistentTokenSerializer" />
public class PersistentTokenSerializer : IPersistentTokenSerializer
{
    private static readonly JsonSerializerOptions Settings;

    private readonly PersistentGrantOptions _options;
    private readonly IDataProtector _provider;

    static PersistentTokenSerializer()
    {
        Settings = new JsonSerializerOptions
        {
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        Settings.Converters.Add(new ClaimConverter());
        Settings.Converters.Add(new ClaimsPrincipalConverter());
    }

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="dataProtectionProvider"></param>
    public PersistentTokenSerializer(PersistentGrantOptions options = null, IDataProtectionProvider dataProtectionProvider = null)
    {
        _options = options;
        _provider = dataProtectionProvider?.CreateProtector(nameof(PersistentTokenSerializer));
    }

    bool ShouldDataProtect => _options?.DataProtectData == true && _provider != null;

    /// <summary>
    /// Serializes the specified value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public string Serialize<T>(T value)
    {
        var payload = JsonSerializer.Serialize(value, Settings);

        if (ShouldDataProtect)
        {
            payload = _provider.Protect(payload);
        }

        var data = new PersistentTokenDataContainer
        {
            PersistentTokenDataContainerVersion = 1,
            DataProtected = ShouldDataProtect,
            Payload = payload,
        };

        return JsonSerializer.Serialize(data, Settings);
    }

    /// <summary>
    /// Deserializes the specified string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json">The json.</param>
    /// <returns></returns>
    public T Deserialize<T>(string json)
    {
        var container = JsonSerializer.Deserialize<PersistentTokenDataContainer>(json, Settings);

        if (container.PersistentTokenDataContainerVersion == 0)
        {
            return JsonSerializer.Deserialize<T>(json, Settings);
        }

        if (container.PersistentTokenDataContainerVersion == 1)
        {
            var payload = container.Payload;

            if (container.DataProtected)
            {
                if (_provider == null)
                {
                    throw new Exception("No IDataProtectionProvider configured.");
                }

                payload = _provider.Unprotect(container.Payload);
            }

            return JsonSerializer.Deserialize<T>(payload, Settings);
        }

        throw new Exception($"Invalid version in persisted token data: '{container.PersistentTokenDataContainerVersion}'.");
    }
}

class PersistentTokenDataContainer
{
    public int PersistentTokenDataContainerVersion { get; set; }
    public bool DataProtected { get; set; }
    public string Payload { get; set; }
}
