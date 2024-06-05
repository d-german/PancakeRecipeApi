namespace PancakeRecipe;

public class ButtermilkPancakeRecipe
{
    private const decimal EggPerPancake = 1m/6m;
    private const decimal CupsButterMilkPerPancake = 1m/6m;
    private const decimal CupsOilPerPancake = 1m/24m;
    private const decimal TspsBakinPowerSodaPercake = 1m/6m;
    private const decimal CupsFlourPerPancake = 1m/6m;
    private const decimal CupsSugerPerPancake = 1m/24m;
    private const decimal NumberTspsPerCup = 48m;
    private const decimal NumPoundsPerCupFlour = 80m/250m;
    private const decimal NumPoundsPerCupSugar = 0.54283m;
    private const string FlOzStr = " ( {0} fl oz )";
    private const string PoundsStr = " ( {0} lbs )";

    private static readonly decimal[] CommonFracs = new[]
    {
        0m,
        1m/8m,
        1m/4m,
        1m/3m,
        1m/2m,
        2m/3m,
        3m/4m,
        1m
    };

    private readonly VolumeAmountResult _galsAndCupsResult;
    private readonly decimal _numPancakes;
    private readonly VolumeAmountResult _tspAndTbspResult;

    public ButtermilkPancakeRecipe(decimal numPancakes = 0m)
    {
        _numPancakes = numPancakes;
        _tspAndTbspResult = new VolumeAmountResult {NameU1 = "tbsp", NameU2 = "tsp"};
        _galsAndCupsResult = new VolumeAmountResult {NameU1 = "gallon", NameU2 = "cup"};
        CalcRecipe();
    }

    public string GetRecipeHtml()
    {
        return $"For {_numPancakes} pancakes, you will need:<br/>" +
            $"- {GetEggsAmount()} of eggs<br/>" +
            $"- {GetButtermilkAmount()} of buttermilk<br/>" +
            $"- {GetOilAmount()} of oil<br/>" +
            $"- {GetBakingPowderAmount()} of baking powder<br/>" +
            $"- {GetBakingSodaAmount()} of baking soda<br/>" +
            $"- {GetFlourAmount()} of flour<br/>" +
            $"- {GetSugarAmount()} of sugar";
    }

    public decimal NumEggs { get; set; }
    public decimal NumCupsButtermilk { get; set; }
    public decimal NumCupsOil { get; set; }
    public decimal NumCupsFlower { get; set; }
    public decimal NumTspBakingSoda { get; set; }
    public decimal NumTspBakingPowder { get; set; }
    public decimal NumCupsSugar { get; set; }

    public string Recipe => GetRecipeHtml();

    public string GetEggsAmount()
    {
        return Eggs.GetQuantity((int) NumEggs);
    }

    public string GetBakingSodaAmount()
    {
        return GetTeaspoonQuantityString(NumTspBakingSoda);
    }

    public string GetBakingPowderAmount()
    {
        return GetTeaspoonQuantityString(NumTspBakingPowder);
    }

    public string GetButtermilkAmount()
    {
        return GetCupsAmountString(NumCupsButtermilk);
    }

    public string GetSugarAmount()
    {
        return GetCupsAmountString(NumCupsSugar) + GetPoundsSugarAmount(NumCupsSugar);
    }

    private static string GetPoundsSugarAmount(decimal numCupsSugar)
    {
        return string.Format(PoundsStr, (numCupsSugar*NumPoundsPerCupSugar).Round(2));
    }

    public string GetFlourAmount()
    {
        //http://www.traditionaloven.com/conversions_of_measures/flour_volume_weight.html
        return GetCupsAmountString(NumCupsFlower) + GetPoundsFlourAmount(NumCupsFlower);
    }

    private static string GetPoundsFlourAmount(decimal numCupsFlour)
    {
        return string.Format(PoundsStr, (numCupsFlour*NumPoundsPerCupFlour).Round(1));
    }

    public string GetOilAmount()
    {
        return GetCupsAmountString(NumCupsOil) + GetFlOz(NumCupsOil);
    }

    private static string GetFlOz(decimal numCups)
    {
        return string.Format(FlOzStr, (numCups*8m).Round(1));
    }

    public string GetCupsAmountString(decimal numCups)
    {
        var galsAndCups = new CupsAndGallons(new Cups(numCups));
        var gals = galsAndCups.CalculatedGals;
        var cups = galsAndCups.CalculatedCups;
        var teaspoon = string.Empty;

        var fp = cups.FractionalPart();

        if (fp is < 0.25m and > 0)
        {
            cups = cups.IntegralPart();
            teaspoon = GetTeaspoonQuantityString(fp*NumberTspsPerCup);
        }

        var cupsResult = _galsAndCupsResult.GetVolumnAmount(
            GetCommonFracMeasure(gals),
            GetCommonFracMeasure(cups));

        if (teaspoon.Equals("0"))
        {
            teaspoon = string.Empty;
        }

        if (cupsResult.Equals("0"))
        {
            cupsResult = string.Empty;
        }

        return (cupsResult + " " + teaspoon).Trim();
    }

    public string GetTeaspoonQuantityString(decimal teaspoonQuantity)
    {
        var cupAmount = string.Empty;
        const decimal oneForth = 0.25m;
        const int numTspsPerOneForthCup = 12;

        if (teaspoonQuantity >= numTspsPerOneForthCup)
        {
            var numCups = teaspoonQuantity/NumberTspsPerCup;
            var calculatedNumCups = 0m;

            while (numCups > oneForth)
            {
                numCups -= oneForth;
                calculatedNumCups += oneForth;
            }

            cupAmount = GetCupsAmountString(calculatedNumCups);
            teaspoonQuantity = numCups*NumberTspsPerCup;
        }

        var tspAndTbsp = new TspAndTbsp(new Tsp(teaspoonQuantity));

        var tspAmount = _tspAndTbspResult.GetVolumnAmount(GetCommonFracMeasure(tspAndTbsp.CalculatedTbsps), GetCommonFracMeasure(tspAndTbsp.CalculatedTsps));

        if (tspAmount.Equals("0"))
        {
            tspAmount = string.Empty;
        }

        if (cupAmount.Equals("0"))
        {
            cupAmount = string.Empty;
        }

        return (cupAmount + " " + tspAmount).Trim();
    }

    public static Tsp ToTsp(Cups cup)
    {
        return new Tsp(cup.Value*NumberTspsPerCup);
    }

    private void CalcRecipe()
    {
        NumEggs = EggPerPancake*_numPancakes;
        NumCupsButtermilk = _numPancakes*CupsButterMilkPerPancake;
        NumCupsOil = _numPancakes*CupsOilPerPancake;
        NumTspBakingPowder = _numPancakes*TspsBakinPowerSodaPercake;
        NumTspBakingSoda = _numPancakes*TspsBakinPowerSodaPercake;
        NumCupsFlower = _numPancakes*CupsFlourPerPancake;
        NumCupsSugar = _numPancakes*CupsSugerPerPancake;
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
        var closest = CommonFracs.Select(n => new {n, distance = Math.Abs(n - target)})
            .OrderBy(p => p.distance)
            .First().n;
        return closest;
    }
}