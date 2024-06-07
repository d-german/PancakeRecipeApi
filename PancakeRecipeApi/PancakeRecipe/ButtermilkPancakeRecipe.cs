using System.Text;

namespace PancakeRecipe;

public class ButtermilkPancakeRecipe
{
	private const decimal EggPerPancake = 1m / 6m;
	private const decimal CupsButterMilkPerPancake = 1m / 6m;
	private const decimal CupsOilPerPancake = 1m / 24m;
	private const decimal TspsBakingPowerSodaPerPancake = 1m / 6m;
	private const decimal CupsFlourPerPancake = 1m / 6m;
	private const decimal CupsSugarPerPancake = 1m / 24m;
	private const decimal NumberTspsPerCup = 48m;
	private const decimal NumPoundsPerCupFlour = 80m / 250m;
	private const decimal NumPoundsPerCupSugar = 0.54283m;
	private const string FlOzStr = " ( {0} fl oz )";
	private const string PoundsStr = " ( {0} lbs )";

	private static readonly decimal[] CommonFractions = {
		0m, 1m / 8m, 1m / 4m, 1m / 3m, 1m / 2m, 2m / 3m, 3m / 4m, 1m
	};

	private readonly VolumeAmountResult _galsAndCupsResult;
	private readonly decimal _numPancakes;
	private readonly VolumeAmountResult _tspAndTbspResult;

	public ButtermilkPancakeRecipe(decimal numPancakes = 0m)
	{
		_numPancakes = numPancakes;
		_tspAndTbspResult = new VolumeAmountResult
		{
			NameU1 = "tbsp",
			NameU2 = "tsp"
		};
		_galsAndCupsResult = new VolumeAmountResult
		{
			NameU1 = "gallon",
			NameU2 = "cup"
		};
		CalcRecipe();
	}

	public string GetRecipeHtml()
	{
		var instructions = new StringBuilder();
		instructions.AppendLine($"For {_numPancakes} pancakes, you will need:<br/>");
		instructions.AppendLine($"- {GetEggsAmount()}<br/>");
		instructions.AppendLine($"- {GetButtermilkAmount()} of buttermilk<br/>");
		instructions.AppendLine($"- {GetBakingPowderAmount()} of vanilla<br/>"); // adding vanilla
		instructions.AppendLine($"- {GetOilAmount()} of oil<br/>");
		instructions.AppendLine($"- {GetBakingPowderAmount()} of baking powder<br/>");
		instructions.AppendLine($"- {GetBakingSodaAmount()} of baking soda<br/>");
		instructions.AppendLine($"- {GetFlourAmount()} of flour<br/>");
		instructions.AppendLine($"- {GetSugarAmount()} of sugar<br/><br/>");

		instructions.AppendLine("Instructions:<br/>");
		instructions.AppendLine("First, mix the wet ingredients well.<br/>");
		instructions.AppendLine("Next, stir in the dry ingredients just until everything is wet. Don't over mix!<br/>");
		instructions.AppendLine("The mixture should be slightly lumpy. Let it sit for a few minutes to allow the batter to expand a little.<br/>");
		instructions.AppendLine("Use a griddle set to 325 - 350 degrees or a skillet set to medium heat.<br/>");
		instructions.AppendLine("The batter is a little thick, so after putting some on the griddle, use a spoon to flatten it out.<br/>");
		instructions.AppendLine("Cook for about 2 minutes on each side or until they look as desired.<br/>");

		return instructions.ToString();
	}

	private decimal NumEggs { get; set; }
	private decimal NumCupsButtermilk { get; set; }
	private decimal NumCupsOil { get; set; }
	private decimal NumCupsFlower { get; set; }
	private decimal NumTspBakingSoda { get; set; }
	private decimal NumTspBakingPowder { get; set; }
	private decimal NumCupsSugar { get; set; }

	private string GetEggsAmount()
	{
		return Eggs.GetQuantity((int)NumEggs);
	}

	private string GetBakingSodaAmount()
	{
		return GetTeaspoonQuantityString(NumTspBakingSoda);
	}

	private string GetBakingPowderAmount()
	{
		return GetTeaspoonQuantityString(NumTspBakingPowder);
	}

	private string GetButtermilkAmount()
	{
		var cups = GetCupsAmountString(NumCupsButtermilk);
		var grams = NumCupsButtermilk * 242m; // Convert cups to grams
		return $"{cups} ( {grams.Round(1)} g )";
	}

	private string GetSugarAmount()
	{
		return GetCupsAmountString(NumCupsSugar) + GetPoundsSugarAmount(NumCupsSugar);
	}

	private static string GetPoundsSugarAmount(decimal numCupsSugar)
	{
		var pounds = numCupsSugar * NumPoundsPerCupSugar;
		var grams = pounds * 453.59237m;
		return string.Format(PoundsStr, pounds.Round(2)) + $" ( {grams.Round(1)} g )";
	}

	private string GetFlourAmount()
	{
		//http://www.traditionaloven.com/conversions_of_measures/flour_volume_weight.html
		return GetCupsAmountString(NumCupsFlower) + GetPoundsFlourAmount(NumCupsFlower);
	}

	private static string GetPoundsFlourAmount(decimal numCupsFlour)
	{
		var pounds = numCupsFlour * NumPoundsPerCupFlour;
		var grams = pounds * 453.59237m;
		return string.Format(PoundsStr, pounds.Round(1)) + $" ( {grams.Round(1)} g )";
	}

	private string GetOilAmount()
	{
		var cups = GetCupsAmountString(NumCupsOil);
		var flOz = GetFlOz(NumCupsOil);
		var grams = NumCupsOil * 190.5m;
		return $"{cups} {flOz} ( {grams.Round(1)} g )";
	}

	private static string GetFlOz(decimal numCups)
	{
		return string.Format(FlOzStr, (numCups * 8m).Round(1));
	}

	private string GetCupsAmountString(decimal numCups)
	{
		var galsAndCups = new CupsAndGallons(new Cups(numCups));
		var gals = galsAndCups.CalculatedGals;
		var cups = galsAndCups.CalculatedCups;
		var tspAmount = string.Empty;

		var fp = cups.FractionalPart();

		if (fp is < 0.25m and > 0)
		{
			cups = cups.IntegralPart();
			tspAmount = GetTeaspoonQuantityString(fp * NumberTspsPerCup);
		}

		var cupsResult = _galsAndCupsResult.GetVolumnAmount(
			GetCommonFracMeasure(gals),
			GetCommonFracMeasure(cups));

		if (tspAmount.Equals("0"))
		{
			tspAmount = string.Empty;
		}

		if (cupsResult.Equals("0"))
		{
			cupsResult = string.Empty;
		}

		return $"{cupsResult} {tspAmount}".Trim();
	}

	private string GetTeaspoonQuantityString(decimal teaspoonQuantity)
	{
		var cupsResult = string.Empty;
		const decimal oneForth = 0.25m;
		const int numTspsPerOneForthCup = 12;

		if (teaspoonQuantity >= numTspsPerOneForthCup)
		{
			var numCups = teaspoonQuantity / NumberTspsPerCup;
			var calculatedNumCups = 0m;

			while (numCups > oneForth)
			{
				numCups -= oneForth;
				calculatedNumCups += oneForth;
			}

			cupsResult = GetCupsAmountString(calculatedNumCups);
			teaspoonQuantity = numCups * NumberTspsPerCup;
		}

		var tspAndTbsp = new TspAndTbsp(new Tsp(teaspoonQuantity));

		var tspAmount = _tspAndTbspResult.GetVolumnAmount(GetCommonFracMeasure(tspAndTbsp.CalculatedTbsps), GetCommonFracMeasure(tspAndTbsp.CalculatedTsps));

		if (tspAmount.Equals("0"))
		{
			tspAmount = string.Empty;
		}

		if (cupsResult.Equals("0"))
		{
			cupsResult = string.Empty;
		}

		return $"{cupsResult} {tspAmount}".Trim();
	}

	public static Tsp ToTsp(Cups cup)
	{
		return new Tsp(cup.Value * NumberTspsPerCup);
	}

	private void CalcRecipe()
	{
		NumEggs = EggPerPancake * _numPancakes;
		NumCupsButtermilk = _numPancakes * CupsButterMilkPerPancake;
		NumCupsOil = _numPancakes * CupsOilPerPancake;
		NumTspBakingPowder = _numPancakes * TspsBakingPowerSodaPerPancake;
		NumTspBakingSoda = _numPancakes * TspsBakingPowerSodaPerPancake;
		NumCupsFlower = _numPancakes * CupsFlourPerPancake;
		NumCupsSugar = _numPancakes * CupsSugarPerPancake;
	}

	private static decimal GetCommonFracMeasure(decimal num)
	{
		var ip = num.IntegralPart();
		var fp = Frac2ClosestMatchFrac(num.FractionalPart());
		return ip + fp;
	}

	private static decimal Frac2ClosestMatchFrac(decimal num)
	{
		var target = num.FractionalPart();
		var closest = CommonFractions.Select(n => new
			{
				n,
				distance = Math.Abs(n - target)
			})
			.OrderBy(p => p.distance)
			.First().n;
		return closest;
	}
}