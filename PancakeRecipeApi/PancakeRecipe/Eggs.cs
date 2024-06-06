namespace PancakeRecipe;

public class Eggs
{
	public static string GetQuantity(int numEggs)
	{
		if (numEggs == 1)
		{
			return "1 egg";
		}

		if (numEggs < 12)
		{
			return $"{numEggs} eggs";
		}

		if (numEggs == 12)
		{
			return "1 dozen";
		}

		int numDozen = CalcNumDozen(numEggs);

		int leftover = numEggs % 12;

		if (leftover == 0)
		{
			return $"{numDozen} dozen";
		}

		if (leftover == 1)
		{
			return $"{numDozen} dozen and 1 egg";
		}

		return $"{numDozen} dozen and {leftover} eggs";
	}

	public static int CalcNumDozen(int numItems)
	{
		if (numItems < 12) return 0;

		if (numItems == 12)
		{
			return 1;
		}

		return numItems / 12;
	}
}