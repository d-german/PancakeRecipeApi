namespace PancakeRecipe;

public class TspAndTbsp
{
	// 3 tsp = 1 tbsp
	// 16 tbsp = 1 cup
	public TspAndTbsp(Tsp numTsp)
	{
		CalcTspsAndTbsps(numTsp.Value);
	}

	private void CalcTspsAndTbsps(decimal numTsps)
	{
		var numtsp = numTsps;

		if (numtsp < 3)
		{
			CalculatedTsps += numtsp;
			return;
		}

		if (numtsp == 3)
		{
			CalculatedTbsps += 1;
			return;
		}

		while (numtsp > 3)
		{
			CalculatedTbsps += 1;
			numtsp -= 3;
		}

		CalcTspsAndTbsps(numtsp);
	}

	public decimal CalculatedTsps { get; private set; }
	public decimal CalculatedTbsps { get; private set; }
}