using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondTrySecondTry.Data
{
    public static class TextHelper
    {
        public static string ON = "ON";
        public static string OFF = "OFF";

        public static Dictionary<string, int> WORDS = new Dictionary<string, int>()
        {
            { "one", 2 },
            { "ate", 9 },
            { "eight", 9 },
            { "for", 5 },
            { "ten", 11 },
            { "four", 5 },
            { "two", 3 },
            { "too", 3 },
            { "three", 4 },
            { "tree", 4 },
            { "five", 6 },
            { "to", 3 }
        };

        public static List<string> SUGGESTIONS = new List<string>
        {
            "Read a Book.",
            "Friends, Friends, Friends.",
            "Family Time.",
            "Go to a Museum.",
            "Cook Food.",
            "Go to the Zoo.",
            "Stop and smell the roses.",        
            "Sit outside in the sun and relax.",
            "Make a small campfire in the backyard.",
            "Doodle.",
            "Go to the park.", 
            "Paint with watercolors.", 
            "Go for a bike ride"
        };

        public static Dictionary<string, string> QUIZ = new Dictionary<string, string>()
        {
            { "Quiz: 2 + 2 = ?", "4" },
            { "Capital of Latvia?", "riga"},
            { "Do you love to code?", "yes"},
            { "Quiz: Which year are today?", "2022" }
        };

        public static string GetRandomSuggestion(ulong authorId)
        {
            Random rnd = new Random();
            var index = rnd.Next(0, TextHelper.SUGGESTIONS.Count - 1);

            return $"<@{authorId}> " + TextHelper.SUGGESTIONS[index];
        }
    }
}

