using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Requests;
using System.Timers;

string token = Console.ReadLine();
var botClient = new TelegramBotClient(token);
using var cts = new CancellationTokenSource();

bool buf = true;
bool cooling = false;
bool opened = false;
string clock = "";
int _Id3 = 0;
int _Id2 = 0;
int _Id = 0;
int _m;
int _s;

InlineKeyboardMarkup Open1 = new(new[]
                                            {
                            // first row
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData(text: "в лево", callbackData: "Open1Left"),
                                InlineKeyboardButton.WithCallbackData(text: "в право", callbackData: "Open1Right"),

                            },

                        });
InlineKeyboardMarkup Open2 = new(new[]
                                            {
                            // first row
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData(text: "в лево2", callbackData: "Open2Left"),
                                InlineKeyboardButton.WithCallbackData(text: "в право2", callbackData: "Open2Right"),

                            },

                        });
InlineKeyboardMarkup Open3 = new(new[]
                                            {
                            // first row
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData(text: "в лево3", callbackData: "Open3Left"),
                                InlineKeyboardButton.WithCallbackData(text: "в право3", callbackData: "Open3Right"),

                            },

                        });






var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { } // receive all update types
};
botClient.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token);






var me = await botClient.GetMeAsync();
Console.WriteLine($"Start listening for @{me.Username}");
while (true)
{
    if (Console.ReadLine() == "Stop")
    {
        cts.Cancel();
        break;
    }
}
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
      
    switch (update.Type)
    {
        case UpdateType.Message:
            if (update.Message.Type == MessageType.Text)
            {
                var username = update.Message.From.Username;
                var chatId = update.Message.Chat.Id;

                var messageText = update.Message.Text;


                Console.WriteLine($"Received a '{messageText}' message in chat {chatId} with a {username}.");
                switch (update.Message.Text)
                {
                    case "/start": var sentMessage = await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Терминал №5571.\r\nДоступные вам команды:\r\n/control - текущее состояние гермоворот; \r\n /open - открытие гермоворот"); break;
                    case "/open": if (cooling | opened) {await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Проверьте состояние гермоворот"); } 
                        else { sentMessage = await botClient.SendPhotoAsync(update.Message.Chat.Id, "https://disk.yandex.ru/i/DMVrrLVxXsjCQA", caption: "Необходимо определить, в какую сторону будет вращаться выделенная шестерёнка", replyMarkup: Open1); _Id = sentMessage.MessageId; } break;

                    case "/control": if (!(cooling|opened)) { sentMessage = await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Состояние гермоворот стабильное"); } 
                     if (cooling) { sentMessage = await botClient.SendTextMessageAsync(update.Message.Chat.Id, "До стабильного состояния гермоворот: "+clock); };
                        if (opened) { sentMessage = await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Гермоворота открыты");};
                            ; break;


          default: break;
                }
            }
            break;
        case UpdateType.CallbackQuery:
            {
                CallbackQuery callbackQuery = update.CallbackQuery;
                await BotOnCallbackQueryReceived(botClient, callbackQuery);
            }
            break;
        default: break;
    }
    // Echo received message text
    
}

async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    if (callbackQuery.Data.StartsWith("Open1"))
    {
        switch (callbackQuery.Data.Substring(5))
        {
            case "Left":
                await botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: ""); if (buf) { buf = true; };
                break;
            case "Right":
                await botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: ""); if (buf) { buf = false; };
                break;
            default: break;
        }
        await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, _Id);
        var beb = await botClient.SendPhotoAsync(callbackQuery.Message.Chat.Id, "https://disk.yandex.ru/i/xqyqars4FSyHiw",caption: "Необходимо определить, в какую сторону будет вращаться выделенная шестерёнка", replyMarkup: Open2);
        _Id2 = beb.MessageId;
    }
    if (callbackQuery.Data.StartsWith("Open2"))
    {
        switch (callbackQuery.Data.Substring(5))
        {
            case "Left":
                await botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: ""); if (buf) { buf = false; };
                break;
            case "Right":
                await botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: ""); if (buf) { buf = true; };
                break;
            default: break;
        }
        await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, _Id2);
        var beb = await botClient.SendPhotoAsync(callbackQuery.Message.Chat.Id, "https://disk.yandex.ru/i/ycGxz1JkK0PgKg",caption: "Необходимо выбрать зубчатую передачу, которая обеспечит оси, прикреплённой к выделенной шестерёнке, максимальную скорость вращения", replyMarkup: Open3);
        _Id3 = beb.MessageId;
    }
    if (callbackQuery.Data.StartsWith("Open3"))
    {
        switch (callbackQuery.Data.Substring(5))
        {
            case "Small":
                 if (buf) { buf = true; };
                break;
            case "Big":
                 if (buf) { buf = false; };
                break;
            default: break;
        }
        
        await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, _Id3); 
        if (buf) { opened = true; var sentMessage = await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Гермоворота открыты"); }
        else { cooling = true; var sentMessage = await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Вам не удалось открыть гермоворота. Во избежание серьезной поломки гермоворот вы можете повторить попытку через 25 минут"); Thread thread = new Thread(new ThreadStart(timeStart)); thread.Start(); };
        await botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: "");
    }


}
void timerCallback(Object stateInfo)
{
    AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
    if (_s > 0) { _s--; }
    else if (_m > 0) { _m--; _s = 59; }
    else { cooling = false; autoEvent.Set(); };
   
    if (_m < 10) { var _m1 = "0" + _m.ToString(); clock = _m1 + ":"; }
    else { clock = _m.ToString()+":"; }
    if (_s < 10) { var _s1 = "0" + _s.ToString(); clock += _s1; }
    else { clock += _s.ToString(); }
    Console.WriteLine(clock);
}


Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

void timeStart()
{
    
    var autoEvent = new AutoResetEvent(false);
    _m = 1;
    _s = 0; ;
    System.Threading.Timer timer = new System.Threading.Timer(callback: timerCallback, autoEvent, 0, 1000);
    autoEvent.WaitOne();
    timer.Dispose();
}