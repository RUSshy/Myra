﻿using System;

namespace Myra.Editor.Utils
{
	public static class Reflection
	{
		public static Type FindGenericType(this Type givenType, Type genericType)
		{
			var interfaceTypes = givenType.GetInterfaces();

			foreach (var it in interfaceTypes)
			{
				if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
					return it;
			}

			if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
				return givenType;

			Type baseType = givenType.BaseType;
			if (baseType == null) return null;

			return FindGenericType(baseType, genericType);
		}

		public static bool IsNullablePrimitive(this Type type)
		{
			return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>) &&
			        type.GetGenericArguments()[0].IsPrimitive);
		}


		public static Type GetNullableType(this Type type)
		{
			return type.GetGenericArguments()[0];
		}

		public static bool IsNumericInteger(this Type t)
		{
			switch (Type.GetTypeCode(t))
			{
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
					return true;
				default:
					return false;
			}
		}

		public static bool IsNumericType(this Type t)
		{
			if (IsNumericInteger(t))
			{
				return true;
			}

			switch (Type.GetTypeCode(t))
			{
				case TypeCode.Decimal:
				case TypeCode.Double:
				case TypeCode.Single:
					return true;
				default:
					return false;
			}
		}
	}
}