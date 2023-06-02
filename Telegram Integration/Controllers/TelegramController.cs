using Microsoft.AspNetCore.Mvc;
using TL;
using WTelegram;

namespace Telegram_Integration.Controllers;

public class TelegramController : Controller
{
    private readonly Client? _client;
    private const int ApiId = 26976025;
    private const string ApiHash = "1696dfd0cab794b90407367f85c9c9cd";

    public TelegramController(string sessionPathname)
    {
        //sessionPathname stores session token in a file. If needed can be saved on database.
        //Ref: https://wiz0u.github.io/WTelegramClient/EXAMPLES#store-session-data-to-database-or-elsewhere-instead-of-files
        _client = new Client(ApiId, ApiHash, sessionPathname);
        _client.PingInterval = 600000; // in milliseconds
    }


    public async Task<string?> DoLogin(string loginInfo) // (add this method to your code)
    {
        string x;
        try
        {
            //tries to login with on info at a time. If first step successful then will ask for next step in message.
            x = await _client?.Login(loginInfo)!;
            if (x == null)
            {
                //when null means login completed
                Console.WriteLine($"We are logged-in as {_client.User} (id {_client.User.id})");
                //Immediate dispose so no unnecessary login
                x = "Login Successful";
            }
        }
        catch (RpcException e)
        {
            x = e.Message;
            switch (x)
            {
                case "PHONE_NUMBER_INVALID":
                    _client?.Dispose();
                    break;
                case "Wrong verification code!":
                    break;
                case "Wrong password":
                    break;
            }
        }

        return x;
    }

    public Task Disconnect()
    {
        _client?.Dispose();
        return Task.CompletedTask;
    }

    public Task Reconnect()
    {
        _client?.LoginUserIfNeeded();
        return Task.CompletedTask;
    }

    public async Task<string?> SendMessage(string mobileNumber, string message)
    {
        try
        {
            //when chat id is not know Contacts_ResolvePhone function is called
            var user = await _client.Contacts_ResolvePhone(mobileNumber);
            //can send only one message at a time so loop is necessary to send multiple
            await _client?.SendMessageAsync(user, message)!;
            return "Success";
        }
        catch (RpcException e)
        {
            switch (e.Message)
            {
                case "AUTH_KEY_UNREGISTERED":
                    return e.Message;
                case "PHONE_NOT_OCCUPIED":
                    return e.Message;
            }
        }
        catch (WTException ex) when (ex.Message is "You must connect to Telegram first"
                                         or "Could not read payload length : Connection shut down")
        {
            return ex.Message;
        }

        return null;
    }

    public async Task SendMedia()
    {
        var peer = await _client.Contacts_ResolvePhone("+919819600116");
        var inputFile = await _client?.UploadFileAsync("Media/download.jpg")!;
        await _client.SendMediaAsync(peer, "Here is the photo", inputFile);
    }
    
    public async Task ScheduleMessage()
    {
        var peer = await _client.Contacts_ResolvePhone("+919819600116");
        var when = DateTime.UtcNow.AddMinutes(2);
        await _client?.SendMessageAsync(peer, "Here is the photo", schedule_date: when)!;
    }

    
}