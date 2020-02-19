using System.Reflection;
using System.Runtime.Serialization;

namespace System.Text.Json.Serialization
{
	/// <summary>
	/// <see cref="JsonConverterFactory"/> to convert enums to and from strings, respecting <see cref="EnumMemberAttribute"/> decorations. Supports nullable enums.
	/// </summary>
	internal class JsonStringEnumMemberConverter : JsonConverterFactory
	{
		private readonly JsonNamingPolicy? namingPolicy;
		private readonly bool allowIntegerValues;

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonStringEnumMemberConverter"/> class.
		/// </summary>
		public JsonStringEnumMemberConverter()
			: this(namingPolicy: null, allowIntegerValues: true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonStringEnumMemberConverter"/> class.
		/// </summary>
		/// <param name="namingPolicy">
		/// Optional naming policy for writing enum values.
		/// </param>
		/// <param name="allowIntegerValues">
		/// True to allow undefined enum values. When true, if an enum value isn't
		/// defined it will output as a number rather than a string.
		/// </param>
		public JsonStringEnumMemberConverter(JsonNamingPolicy? namingPolicy = null, bool allowIntegerValues = true)
		{
			this.namingPolicy = namingPolicy;
			this.allowIntegerValues = allowIntegerValues;
		}

		/// <inheritdoc/>
		public override bool CanConvert(Type typeToConvert)
		{
			// Don't perform a typeToConvert == null check for performance. Trust our callers will be nice.
#pragma warning disable CA1062 // Validate arguments of public methods
			return typeToConvert.IsEnum || (typeToConvert.IsGenericType && TestNullableEnum(typeToConvert).IsNullableEnum);
#pragma warning restore CA1062 // Validate arguments of public methods
		}

		/// <inheritdoc/>
		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		{
			(var IsNullableEnum, var UnderlyingType) = TestNullableEnum(typeToConvert);

			return (JsonConverter)Activator.CreateInstance(
				typeof(JsonStringEnumMemberConverter<>).MakeGenericType(typeToConvert),
				BindingFlags.Instance | BindingFlags.Public,
				binder: null,
				args: new object?[] { namingPolicy, allowIntegerValues, IsNullableEnum ? UnderlyingType : null },
				culture: null);
		}

		private (bool IsNullableEnum, Type? UnderlyingType) TestNullableEnum(Type typeToConvert)
		{
			var underlyingType = Nullable.GetUnderlyingType(typeToConvert);
			return (underlyingType?.IsEnum ?? false, underlyingType);
		}
	}
}