﻿using System.Globalization;

namespace PancakeRecipe;

public static class Extensions
{
	public static decimal IntegralPart(this decimal value)
	{
		return Math.Truncate(value);
	}

	public static decimal FractionalPart(this decimal value)
	{
		return value - value.IntegralPart();
	}

	public static string ToNumericalString(this decimal value)
	{
		var ip = value.IntegralPart();
		var fp = value.FractionalPart();

		switch (ip)
		{
			case 0 when fp == 0:
				return "0";
			case 0 when fp != 0:
				return FracMethod.Dec2Frac(fp);
		}

		if (ip != 0 && fp == 0)
		{
			return ip.ToString(CultureInfo.CurrentCulture);
		}

		if (ip != 0 && fp != 0)
		{
			return $"{ip} and {FracMethod.Dec2Frac(fp)}";
		}

		return null!;
	}

	public static decimal Round(this decimal value, int numPlaces = 5)
	{
		return Math.Round(value, numPlaces);
	}
}