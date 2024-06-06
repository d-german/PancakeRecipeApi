using System.Globalization;
using System.Text;

namespace PancakeRecipe;

/// <summary>
/// Class for simplifying decimals to fractions with example demonstration.
/// For testing purposes, I used the pc calculator from which I copied decimal values into the console
/// To paste on console, right click on window, -> edit -> paste
/// 
/// Author: Opata Chibueze
/// </summary>
public class FracMethod
{
	public static string Dec2Frac(decimal dbl)
	{
		var neg = ' ';
		var dblDecimal = dbl;

		if (dblDecimal == (int)dblDecimal) return dblDecimal.ToString(CultureInfo.InvariantCulture); //return no if it's not a decimal

		if (dblDecimal < 0)
		{
			dblDecimal = Math.Abs(dblDecimal);
			neg = '-';
		}

		var whole = (int)Math.Truncate(dblDecimal);
		var decPart = dblDecimal.ToString(CultureInfo.CurrentCulture).Replace(Math.Truncate(dblDecimal) + ".", "");
		var rN = Convert.ToDouble(decPart);
		var rD = Math.Pow(10, decPart.Length);

		var rd = Recur(decPart);
		var rel = Convert.ToInt32(rd);
		if (rel != 0)
		{
			rN = rel;
			rD = (int)Math.Pow(10, rd.Length) - 1;
		}

		//just a few prime factors for testing purposes
		var primes = new[]
		{
			97, 89, 83, 79, 73, 71, 67, 61, 59, 53, 47, 43, 37, 31, 29, 23, 19, 17, 13, 11, 7, 5, 3, 2
		};
		foreach (var i in primes) ReduceNo(i, ref rD, ref rN);

		rN = rN + (whole * rD);
		return $"{neg}{rN}/{rD}".Trim();
	}

	/// <summary>
	/// Finds out the recurring decimal in a specified number
	/// </summary>
	/// <param name="db">Number to check</param>
	/// <returns></returns>
	private static string Recur(string db)
	{
		if (db.Length < 13) return "0";
		var sb = new StringBuilder();
		for (var i = 0; i < 7; i++)
		{
			sb.Append(db[i]);
			var length = (db.Length / sb.ToString().Length);
			var occur = Occurence(sb.ToString(), db);
			if (length == occur || length == occur - sb.ToString().Length)
			{
				return sb.ToString();
			}
		}

		return "0";
	}

	/// <summary>
	/// Checks for number of occurrence of specified no in a number
	/// </summary>
	/// <param name="s">The no to check occurence times</param>
	/// <param name="check">The number where to check this</param>
	/// <returns></returns>
	private static int Occurence(string s, string check)
	{
		var i = 0;
		var d = s.Length;
		var ds = check;
		for (var n = (ds.Length / d); n > 0; n--)
		{
			if (!ds.Contains(s)) continue;
			i++;
			ds = ds.Remove(ds.IndexOf(s, StringComparison.Ordinal), d);
		}

		return i;
	}

	/// <summary>
	/// Reduces a fraction given the numerator and denominator
	/// </summary>
	/// <param name="i">Number to use in an attempt to reduce fraction</param>
	/// <param name="rD">the Denominator</param>
	/// <param name="rN">the Numerator</param>
	private static void ReduceNo(int i, ref double rD, ref double rN)
	{
		//keep reducing until divisibility ends
		while ((rD % i) == 0 && (rN % i) == 0)
		{
			rN = rN / i;
			rD = rD / i;
		}
	}
}