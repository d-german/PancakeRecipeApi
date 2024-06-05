using Microsoft.AspNetCore.Mvc;
using PancakeRecipe;

namespace PancakeRecipeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PancakeRecipeController : Controller
{
	[HttpGet(Name = "GetPancakeRecipe")]
	public IActionResult Get(int numPancakes = 6)
	{
		var res = new ButtermilkPancakeRecipe(numPancakes);
		var rec = res.GetRecipeHtml();
		return Content(rec, "text/html");
	}
}