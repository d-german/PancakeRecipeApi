namespace PancakeRecipe;

public class Eggs
{
	public static string GetQuantity(int numEggs)
	{
		switch (numEggs)
		{
			case 1:
				return "1 egg";
			case < 12:
				return $"{numEggs} eggs";
			case 12:
				return "1 dozen";
		}

		var numDozen = CalcNumDozen(numEggs);

		var leftover = numEggs % 12;

		return leftover switch
		{
			0 => $"{numDozen} dozen",
			1 => $"{numDozen} dozen and 1 egg",
			_ => $"{numDozen} dozen and {leftover} eggs"
		};
	}

	private static int CalcNumDozen(int numItems)
	{
		return numItems switch
		{
			< 12 => 0,
			12 => 1,
			_ => numItems / 12
		};
	}
}