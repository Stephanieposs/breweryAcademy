using System.ComponentModel;

namespace YMS.Enums
{
	public static class EnumHelper
	{
		public static bool IsValidEnumDescription<TEnum>(string description) where TEnum : Enum
		{
			// Get the descriptions of all enum values
			var enumDescriptions = Enum.GetValues(typeof(TEnum))
				.Cast<TEnum>()
				.Select(e => GetEnumDescription(e))
				.ToList();

			return enumDescriptions.Contains(description);
		}

		private static string GetEnumDescription(Enum value)
		{
			var field = value.GetType().GetField(value.ToString());
			var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))!;
			return attribute?.Description ?? value.ToString(); // Fallback to enum name if no description is found
		}
	}
}
