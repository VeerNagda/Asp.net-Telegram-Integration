using Microsoft.AspNetCore.Mvc;
namespace Telegram_Integration.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TelegramLogin : Controller
{
    private static TelegramController? _telegram = null;
    [HttpGet("number")]
    public static async void Login(string number)
    {
        _telegram ??= new TelegramController();
        await _telegram.DoLogin(number); // initial call with user's phone_number
    }
}