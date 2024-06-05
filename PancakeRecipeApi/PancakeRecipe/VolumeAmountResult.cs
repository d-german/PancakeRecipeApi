namespace PancakeRecipe;

public class VolumeAmountResult
{
	//TODO: Clean this up
	private const string ZeroU1ZeroU2 = "0";
	private const string ZeroU1OneU2 = "{0} " + U2Token;
	private const string ZeroU1MoreU2 = "{0} " + U2Token + "s";
	private const string OneU1ZeroU2 = "{0} [U1]";
	private const string OneU1OneU2 = "{0} [U1] and {1} [U2]";
	private const string OneU1MoreU2 = "{0} [U1] and {1} [U2]s";
	private const string MoreU1ZeroU2 = "{0} [U1]s";
	private const string MoreU1OneU2 = "{0} [U1]s and {1} [U2]";
	private const string MoreU1MoreU2 = "{0} [U1]s and {1} [U2]s";
	private const string U1Token = "[U1]";
	private const string U2Token = "[U2]";

	public string NameU1 { get; set; }
	public string NameU2 { get; set; }

	private static Cardinality GetCardinality(decimal num)
	{
		if (num == 0m)
		{
			return Cardinality.Zero;
		}

		return num <= 1 ? Cardinality.LessThanEqualToOne : Cardinality.GreaterThanOne;
	}

	public string GetVolumnAmount(decimal u1, decimal u2)
	{
		var u1Card = GetCardinality(u1);
		var u2Card = GetCardinality(u2);

		var result = String.Empty;

		if (u1Card == Cardinality.Zero && u2Card == Cardinality.Zero)
		{
			result = ZeroU1ZeroU2;
		}

		if (u1Card == Cardinality.Zero && u2Card == Cardinality.LessThanEqualToOne)
		{
			result = String.Format(ZeroU1OneU2, u2.ToNumericalString());
		}

		if (u1Card == Cardinality.Zero && u2Card == Cardinality.GreaterThanOne)
		{
			result = String.Format(ZeroU1MoreU2, u2.ToNumericalString());
		}

		////////////////////////

		if (u1Card == Cardinality.LessThanEqualToOne && u2Card == Cardinality.Zero)
		{
			result = String.Format(OneU1ZeroU2, u1.ToNumericalString());
		}

		if (u1Card == Cardinality.LessThanEqualToOne && u2Card == Cardinality.LessThanEqualToOne)
		{
			result = String.Format(OneU1OneU2,
				u1.ToNumericalString(),
				u2.ToNumericalString());
		}

		if (u1Card == Cardinality.LessThanEqualToOne && u2Card == Cardinality.GreaterThanOne)
		{
			result = String.Format(OneU1MoreU2,
				u1.ToNumericalString(),
				u2.ToNumericalString());
		}

		///////////////

		if (u1Card == Cardinality.GreaterThanOne && u2Card == Cardinality.Zero)
		{
			result = String.Format(MoreU1ZeroU2, u1.ToNumericalString());
		}

		if (u1Card == Cardinality.GreaterThanOne && u2Card == Cardinality.LessThanEqualToOne)
		{
			result = String.Format(MoreU1OneU2,
				u1.ToNumericalString(),
				u2.ToNumericalString());
		}

		if (u1Card == Cardinality.GreaterThanOne && u2Card == Cardinality.GreaterThanOne)
		{
			result = String.Format(MoreU1MoreU2,
				u1.ToNumericalString(),
				u2.ToNumericalString());
		}

		return result.Replace(U1Token, NameU1).Replace(U2Token, NameU2);
	}
}