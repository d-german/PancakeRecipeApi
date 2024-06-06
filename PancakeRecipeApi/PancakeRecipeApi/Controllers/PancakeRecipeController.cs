using Microsoft.AspNetCore.Mvc;
using PancakeRecipe;

namespace PancakeRecipeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PancakeRecipeController : Controller
{
	[HttpGet(Name = "GetPancakeRecipe")]
	public IActionResult Get(int num = 6)
	{
		var res = new ButtermilkPancakeRecipe(num);
		return Content(res.GetRecipeHtml(), "text/html");
	}
}