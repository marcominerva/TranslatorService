using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;

namespace System.Text.Json.Serialization
{
#pragma warning disable CA1812 // Remove class never instantiated
    internal class JsonStringEnumMemberConverter<T> : JsonConverter<T>
#pragma warning restore CA1812 // Remove class never instantiated
    {
        private static readonly string[] s_Split = new string[] { ", " };

        private class EnumInfo
        {
#pragma warning disable SA1401 // Fields should be private
            public string Name;
            public Enum EnumValue;
            public ulong RawValue;
#pragma warning restore SA1401 // Fields should be private

            public EnumInfo(string name, Enum enumValue, ulong rawValue)
            {
                Name = name;
                EnumValue = enumValue;
                RawValue = rawValue;
            }
        }

        private readonly bool allowIntegerValues;
        private readonly Type? underlyingType;
        private readonly Type enumType;
        private readonly TypeCode enumTypeCode;
        private readonly bool isFlags;
        private readonly Dictionary<ulong, EnumInfo> rawToTransformed;
        private readonly Dictionary<string, EnumInfo> transformedToRaw;

        public JsonStringEnumMemberConverter(JsonNamingPolicy? namingPolicy, bool allowIntegerValues, Type? underlyingType)
        {
            Debug.Assert(
                (typeof(T).IsEnum && underlyingType == null)
                || (Nullable.GetUnderlyingType(typeof(T)) == underlyingType),
                "Generic type is invalid.");

            this.allowIntegerValues = allowIntegerValues;
            this.underlyingType = underlyingType;
            enumType = this.underlyingType ?? typeof(T);
            enumTypeCode = Type.GetTypeCode(enumType);
            isFlags = enumType.IsDefined(typeof(FlagsAttribute), true);

            var builtInNames = enumType.GetEnumNames();
            var builtInValues = enumType.GetEnumValues();

            rawToTransformed = new Dictionary<ulong, EnumInfo>();
            transformedToRaw = new Dictionary<string, EnumInfo>();

            for (var i = 0; i < builtInNames.Length; i++)
            {
                var enumValue = (Enum)builtInValues.GetValue(i);
                var rawValue = GetEnumValue(enumValue);

                var name = builtInNames[i];

                string transformedName;
                if (namingPolicy == null)
                {
                    var field = enumType.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)!;
                    var enumMemberAttribute = field.GetCustomAttribute<EnumMemberAttribute>(true);
                    transformedName = enumMemberAttribute?.Value ?? name;
                }
                else
                {
                    transformedName = namingPolicy.ConvertName(name) ?? name;
                }

                rawToTransformed[rawValue] = new EnumInfo(transformedName, enumValue, rawValue);
                transformedToRaw[transformedName] = new EnumInfo(name, enumValue, rawValue);
            }
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var token = reader.TokenType;

            // Note: There is no check for token == JsonTokenType.Null becaue Json serializer won't call the converter in that case.
            if (token == JsonTokenType.String)
            {
                var enumString = reader.GetString();

                // Case sensitive search attempted first.
                if (transformedToRaw.TryGetValue(enumString, out var enumInfo))
                {
                    return (T)Enum.ToObject(enumType, enumInfo.RawValue);
                }

                if (isFlags)
                {
                    ulong calculatedValue = 0;
                    var flagValues = enumString.Split(s_Split, StringSplitOptions.None);

                    foreach (var flagValue in flagValues)
                    {
                        // Case sensitive search attempted first.
                        if (transformedToRaw.TryGetValue(flagValue, out enumInfo))
                        {
                            calculatedValue |= enumInfo.RawValue;
                        }
                        else
                        {
                            // Case insensitive search attempted second.
                            var matched = false;
                            foreach (var enumItem in transformedToRaw)
                            {
                                if (string.Equals(enumItem.Key, flagValue, StringComparison.OrdinalIgnoreCase))
                                {
                                    calculatedValue |= enumItem.Value.RawValue;
                                    matched = true;
                                    break;
                                }
                            }

                            if (!matched)
                            {
                                throw new NotSupportedException();
                            }
                        }
                    }

                    return (T)Enum.ToObject(enumType, calculatedValue);
                }
                else
                {
                    // Case insensitive search attempted second.
                    foreach (var enumItem in transformedToRaw)
                    {
                        if (string.Equals(enumItem.Key, enumString, StringComparison.OrdinalIgnoreCase))
                        {
                            return (T)Enum.ToObject(enumType, enumItem.Value.RawValue);
                        }
                    }
                }

                throw new NotSupportedException();
            }

            if (token != JsonTokenType.Number || !allowIntegerValues)
            {
                throw new NotSupportedException();
            }

            // Switch cases ordered by expected frequency.
            var retValue = enumTypeCode switch
            {
                TypeCode.Int32 when reader.TryGetInt32(out var int32) => (T)Enum.ToObject(enumType, int32),
                TypeCode.UInt32 when reader.TryGetUInt32(out var uint32) => (T)Enum.ToObject(enumType, uint32),
                TypeCode.UInt64 when reader.TryGetUInt64(out var uint64) => (T)Enum.ToObject(enumType, uint64),
                TypeCode.Int64 when reader.TryGetInt64(out var int64) => (T)Enum.ToObject(enumType, int64),
                TypeCode.SByte when reader.TryGetSByte(out var byte8) => (T)Enum.ToObject(enumType, byte8),
                TypeCode.Byte when reader.TryGetByte(out var ubyte8) => (T)Enum.ToObject(enumType, ubyte8),
                TypeCode.Int16 when reader.TryGetInt16(out var int16) => (T)Enum.ToObject(enumType, int16),
                TypeCode.UInt16 when reader.TryGetUInt16(out var uint16) => (T)Enum.ToObject(enumType, uint16),
                _ => throw new NotSupportedException()
            };

            return retValue;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // Note: There is no check for value == null becaue Json serializer won't call the converter in that case.
            var rawValue = GetEnumValue(value!);

            if (rawToTransformed.TryGetValue(rawValue, out var enumInfo))
            {
                writer.WriteStringValue(enumInfo.Name);
                return;
            }

            if (isFlags)
            {
                ulong calculatedValue = 0;

                var Builder = new StringBuilder();
                foreach (var enumItem in rawToTransformed)
                {
                    enumInfo = enumItem.Value;
                    if (!(value as Enum)!.HasFlag(enumInfo.EnumValue)
                        || enumInfo.RawValue == 0) // Definitions with 'None' should hit the cache case.
                    {
                        continue;
                    }

                    // Track the value to make sure all bits are represented.
                    calculatedValue |= enumInfo.RawValue;

                    if (Builder.Length > 0)
                    {
                        Builder.Append(", ");
                    }

                    Builder.Append(enumInfo.Name);
                }
                if (calculatedValue == rawValue)
                {
                    writer.WriteStringValue(Builder.ToString());
                    return;
                }
            }

            if (!allowIntegerValues)
            {
                throw new NotSupportedException();
            }

            switch (enumTypeCode)
            {
                case TypeCode.Int32:
                    writer.WriteNumberValue((int)rawValue);
                    break;

                case TypeCode.UInt32:
                    writer.WriteNumberValue((uint)rawValue);
                    break;

                case TypeCode.UInt64:
                    writer.WriteNumberValue(rawValue);
                    break;

                case TypeCode.Int64:
                    writer.WriteNumberValue((long)rawValue);
                    break;

                case TypeCode.Int16:
                    writer.WriteNumberValue((short)rawValue);
                    break;

                case TypeCode.UInt16:
                    writer.WriteNumberValue((ushort)rawValue);
                    break;

                case TypeCode.Byte:
                    writer.WriteNumberValue((byte)rawValue);
                    break;

                case TypeCode.SByte:
                    writer.WriteNumberValue((sbyte)rawValue);
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        private ulong GetEnumValue(object value)
        {
            return enumTypeCode switch
            {
                TypeCode.Int32 => (ulong)(int)value,
                TypeCode.UInt32 => (uint)value,
                TypeCode.UInt64 => (ulong)value,
                TypeCode.Int64 => (ulong)(long)value,
                TypeCode.SByte => (ulong)(sbyte)value,
                TypeCode.Byte => (byte)value,
                TypeCode.Int16 => (ulong)(short)value,
                TypeCode.UInt16 => (ushort)value,
                _ => throw new NotSupportedException(),
            };
        }
    }
}