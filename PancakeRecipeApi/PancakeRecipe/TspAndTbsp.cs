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
		while (true)
		{
			var calculatedTsps = numTsps;

			switch (calculatedTsps)
			{
				case < 3:
					CalculatedTsps += calculatedTsps;
					return;
				case 3:
					CalculatedTbsps += 1;
					return;
			}

			while (calculatedTsps > 3)
			{
				CalculatedTbsps += 1;
				calculatedTsps -= 3;
			}

			numTsps = calculatedTsps;
		}
	}

	public decimal CalculatedTsps { get; private set; }
	public decimal CalculatedTbsps { get; private set; }
}