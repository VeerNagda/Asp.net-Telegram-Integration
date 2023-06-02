using Microsoft.AspNetCore.Mvc;
using Telegram_Integration.Models;

namespace Telegram_Integration.Controllers;

[ApiController]
[Route("[controller]")]
public class TelegramIntegrationController : Controller
{
    private static TelegramController? _telegramController;
    private readonly Models.Telegram _telegram = new();

    [HttpGet("login")]
    public IEnumerable<Models.Telegram> Login(string info)
    {
        _telegramController ??= new TelegramController("sessionToken");
        _telegram.Message = _telegramController.DoLogin(info).Result; // initial call with user's phone_number
        if (_telegram.Message is "PHONE_NUMBER_INVALID" or "Login Successful")
        {
            var temp = _telegram.Message;
            Disconnect();
            _telegram.Message = temp;
        }
           
        yield return _telegram;
    }

    //For testing purpose only given http get, must be used inside other functions
    [HttpGet("reconnect")]
    public Models.Telegram Reconnect()
    {
        try
        {
            //to check if the session token is access or not if yes then session is live
            var fs = System.IO.File.Open("sessionToken", FileMode.Open);
            fs.Close();
            _telegramController = new TelegramController("sessionToken");
            _telegramController.Reconnect();
            _telegram.Message = "Session Created";
            Thread.Sleep(50); // So the reconnection completes before attempting to send message
            return _telegram;
        }
        catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32) //catches if session token is being used
        {
            _telegram.Message = "Session token being used";
            return _telegram;
        }
    }

    //For testing purpose only given http get, must be used inside other functions
    [HttpGet("disconnect")]
    public Models.Telegram Disconnect()
    {
        if (_telegramController == null)
        {
            _telegram.Message = "No session on going";
            return _telegram;
        }

        _telegramController.Disconnect();
        _telegramController = null;
        _telegram.Message = "Session Disconnected";
        return _telegram;
    }

    [HttpPost("message")]
    public async Task<Models.Telegram> Message(TelegramMessage telegramMessage)
    {
        Reconnect();
        if (_telegram.Message is "Session Created" or "Session token being used")
        {
            if (telegramMessage.MobileNumber == null || telegramMessage.Message == null)
            {
             _telegram.Message = "Field_Required";
             return _telegram;
            }
            var x = await _telegramController?.SendMessage(telegramMessage.MobileNumber, telegramMessage.Message)!; // Awaits to send message before disconnection
            Disconnect();
            switch (x)
            {
                case "Success":
                    _telegram.Message = "Message Sent";
                    break;
                case "AUTH_KEY_UNREGISTERED":
                    _telegram.Message = "Please Login Again";
                    _telegramController = null;
                    break;
                case "You must connect to Telegram first":
                    _telegram.Message = "Retry";
                    _telegramController = null;
                    break;
                case "PHONE_NOT_OCCUPIED":
                    _telegram.Message = "Enter a valid number";
                    break;
            }

            return _telegram;
        }

        _telegram.Message = "Some error"; // In case a error is missed 
        Disconnect();
        return _telegram;
    }


    [HttpGet("send-media")]
    public async Task SendMedia()
    {
        Reconnect();
        await _telegramController?.SendMedia()!;
        Disconnect();
    }

    [HttpGet("ScheduleMessage")]
    public async Task ScheduleMessage()
    {
        Reconnect();
        await _telegramController?.ScheduleMessage()!;
        Disconnect();
    }

}