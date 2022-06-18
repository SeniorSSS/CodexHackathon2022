using System;
using Discord;
using Discord.WebSocket;
using System.Threading;
using System.Threading.Tasks;
using SecondTrySecondTry.Data;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
//using System.Linq;

namespace SecondTrySecondTry
{
    class Program
    {
        DiscordSocketClient client;

        SocketMessage lastmsg;

        bool pLangOn = false;

        bool suggestionsOn = false;

        bool quizOn = false;

        bool breathingOn = false;
        bool breathingStarted = false;

        Dictionary<ulong, string> quizUsers = new Dictionary<ulong, string>();

        string botText = string.Empty;
        string message = string.Empty;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            client =  new DiscordSocketClient();
            client.MessageReceived += CommandsHandler;
            client.Log += Log;

           //get your own token
            var token = "some token";

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            var suggestionTimer = new System.Timers.Timer(30 * 60 * 1000);
            suggestionTimer.Elapsed += new ElapsedEventHandler(OnSugestionTimedEvent);
            suggestionTimer.Start();

            var quizTimer = new System.Timers.Timer(60 * 1000);
            quizTimer.Elapsed += new ElapsedEventHandler(OnQuizTimedEvent);
            quizTimer.Start();

            Console.ReadLine();
        }

        private void OnQuizTimedEvent(object sender, ElapsedEventArgs e)
        {
            var userList = new List<ulong>();

            if (quizOn)
            {
                if (lastmsg != null)
                {
                    Random rnd = new Random();
                    var users = lastmsg.Channel.GetUsersAsync().ToListAsyncBetter();                    
                    var userName = users.Result[0];

                    foreach (var user in userName)
                    {
                        if (!user.IsBot && !quizUsers.ContainsKey(user.Id)) 
                        {
                            userList.Add(user.Id); 
                        }
                    }

                    if (userList.Count > 0)
                    {
                        var userIndex = rnd.Next(0, userList.Count);                     
                        var quizIndex = rnd.Next(0, TextHelper.QUIZ.Count);
                        quizUsers.Add(userList[userIndex], TextHelper.QUIZ.ElementAt(quizIndex).Value);

                        lastmsg.Channel.SendMessageAsync($"<@{userList[userIndex]}> " + TextHelper.QUIZ.ElementAt(quizIndex).Key);
                    }
                }
            }
        }

        private void OnSugestionTimedEvent(object sender, ElapsedEventArgs e)
        {
            var userList = new List<ulong>();

            if (suggestionsOn)
            {
                if (lastmsg != null)
                {
                    Random rnd = new Random();
                    var index = rnd.Next(0, TextHelper.SUGGESTIONS.Count - 1);
                    var users = lastmsg.Channel.GetUsersAsync().ToListAsyncBetter();                    
                    var userName = users.Result[0];

                    foreach (var user in userName)
                    {
                        if (!user.IsBot) 
                        {
                            userList.Add(user.Id); 
                        }
                    }

                    var userIndex = rnd.Next(0, userList.Count);
                       
                    lastmsg.Channel.SendMessageAsync($"<@{userList[userIndex]}> " + TextHelper.SUGGESTIONS[index]);
                }
            }
        }
        

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task<Task> CommandsHandler(SocketMessage msg)
        {
            lastmsg = msg;

            if (!msg.Author.IsBot)
            {
                if (breathingStarted)
                {
                    await msg.Channel.DeleteMessageAsync(msg);
                    return Task.CompletedTask;
                }

               botText = string.Empty;
               message = msg.Content.ToLower();

                if (quizOn && quizUsers.ContainsKey(msg.Author.Id))
                {
                    if (message != quizUsers[msg.Author.Id])
                    {
                        await msg.Channel.DeleteMessageAsync(msg);
                        botText = $"<@{msg.Author.Id}> Sorry answer {message} is not correct! \n Try again!";
                    }
                    else
                    {
                        botText = $"<@{msg.Author.Id}> That is correct answer!!! You can continue to chat :)";
                        quizUsers.Remove(msg.Author.Id);
                    }
                }
                else
                {
                     switch (message)
                        {
                            case "!commands":
                                botText = "Available commands: \n !status - gives status of bot variables \n !planguage switch - switch ON&OFF P language translator for all messages \n !suggestions switch - switch ON&OFF suggestions to random user \n what to do today? - makes suggestion for activity \n !quiz switch - switch quiz ON&OFF for random user \n !breath - start breathing exercise";
                                break;
                            case "!status":
                                botText = $"P language interpretator is {GetPLangStatus()}";
                                botText += "\n";
                                botText += $"Suggestions is {GetSuggestionStatus()} with frequency once in 30 minutes";
                                botText += "\n";
                                botText += $"Quiz is {GetQuiztatus()}";
                                break;
                           case "!planguage switch":
                                pLangOn = !pLangOn;
                                botText = $"";
                                break;
                           case "!suggestions switch":
                                suggestionsOn = !suggestionsOn;
                                botText = $"";
                                break;
                           case "!quiz switch":
                                quizOn = !quizOn;
                                botText = $"";
                                break;
                           case "!breathing switch":
                                breathingOn = !breathingOn;
                                break;
                           case "what to do today?":
                                botText = TextHelper.GetRandomSuggestion(msg.Author.Id);    
                                break;
                           case "!breath":
                                breathingStarted = true;
                                var botImage = await lastmsg.Channel.SendFileAsync("C:\\work\\temp\\Fun\\breathing-daisy.mp4.gif", "Breathing exercise");                             
                                Thread.Sleep(15000);
                                await botImage.DeleteAsync();
                                breathingStarted = false;
                                break;
                        }

                        //p land variator
                        if (pLangOn)
                        {
                            await msg.Channel.DeleteMessageAsync(msg);

                            var pMessage = message;
                            foreach (var (word, number) in TextHelper.WORDS)
                            {
                                pMessage = pMessage.Replace(word, number.ToString());
                            }
       
                            var uNick = (msg.Author as SocketGuildUser).DisplayName;

                            await msg.Channel.SendMessageAsync($"[{uNick}]: " + pMessage);                  
                        }
                }


                //bot helper commands
                if (!string.IsNullOrEmpty(botText))
                {
                    await msg.DeleteAsync();
                    await msg.Channel.SendMessageAsync($"======================================================= \n {botText} \n ----------------------------------------------------------------------------");
                    return Task.CompletedTask;
                }


            }
            return Task.CompletedTask;
        }

        private string GetPLangStatus()
        {
            return pLangOn ? TextHelper.ON : TextHelper.OFF;
        }

        private string GetSuggestionStatus()
        {
            return suggestionsOn ? TextHelper.ON : TextHelper.OFF;
        }

        private string GetQuiztatus()
        {
            return quizOn ? TextHelper.ON : TextHelper.OFF;
        }
    }
}
