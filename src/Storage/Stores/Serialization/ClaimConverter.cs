﻿using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin.Stores.Serialization;

#pragma warning disable 1591

public class ClaimConverter : JsonConverter<Claim>
{
    public override Claim Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var source = JsonSerializer.Deserialize<ClaimLite>(ref reader, options);
        var target = new Claim(source.Type, source.Value, source.ValueType);
        return target;
    }

    public override void Write(Utf8JsonWriter writer, Claim value, JsonSerializerOptions options)
    {
        var target = new ClaimLite
        {
            Type = value.Type,
            Value = value.Value,
            ValueType = value.ValueType
        };
        if (target.ValueType == ClaimValueTypes.String)
        {
            target.ValueType = null;
        }

        JsonSerializer.Serialize(writer, target, options);
    }
}