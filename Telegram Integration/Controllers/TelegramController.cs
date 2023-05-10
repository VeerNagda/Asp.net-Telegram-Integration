using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegram_Integration.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TelegramLoginController : ControllerBase
{
    [HttpGet]
    public static async Task<User> Login()
    {
        var botClient = new TelegramBotClient("5738117370:AAHDcN7Fs7y69JmM-VLl5jQd3Rqw_puzdUE");
        
        var me = await botClient.GetMeAsync();

        return me;
    }
}