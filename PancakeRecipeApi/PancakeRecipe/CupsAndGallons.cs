﻿namespace PancakeRecipe;

public class CupsAndGallons
{
	private const decimal HalfGallon = 8m;
	private const decimal Gallon = 16m;
	private const decimal Half = 0.5m;
	private const int One = 1;

	public CupsAndGallons(Cups cups)
	{
		CalcCupsAndGallons(cups.Value);
	}

	public decimal CalculatedGals { get; private set; }
	public decimal CalculatedCups { get; private set; }

	private void CalcCupsAndGallons(decimal numCups)
	{
		while (true)
		{
			switch (numCups)
			{
				case < HalfGallon:
					CalculatedCups += numCups;
					return;
				case HalfGallon:
					CalculatedGals += Half;
					return;
				case Gallon:
					CalculatedGals += One;
					return;
				case > HalfGallon and < Gallon:
					CalculatedGals += Half;
					CalculatedCups += numCups - HalfGallon;
					return;
			}

			if (numCups <= Gallon) return;

			CalculatedCups += numCups.FractionalPart();
			var ip = numCups.IntegralPart();

			while (ip > Gallon)
			{
				CalculatedGals += One;
				ip -= Gallon;
			}

			numCups = ip;
		}
	}
}