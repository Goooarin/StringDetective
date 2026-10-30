using System;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== String Detective ===");
        Console.Write("Введите текст: ");
        string text = Console.ReadLine() ?? "";

        while (true)
        {
            Console.WriteLine("\n1. Подсчитать слова");
            Console.WriteLine("2. Найти самое частое слово");
            Console.WriteLine("3. Удалить знаки препинания");
            Console.WriteLine("4. Показать топ слов");
            Console.WriteLine("5. Ввести новый текст");
            Console.WriteLine("0. Выход");
            Console.Write("Ваш выбор: ");

            string? input = Console.ReadLine();
            if (input is null)
                return;

            string choice = input.Trim();

            switch (choice)
            {
                case "1":
                    Console.WriteLine($"Количество слов: {GetWordCount(text)}");
                    break;
                case "2":
                    Console.WriteLine($"Самое частое слово: {GetMostFrequentWord(text)}");
                    break;
                case "3":
                    Console.WriteLine($"Текст без пунктуации: {RemovePunctuation(text)}");
                    break;
                case "4":
                    foreach (var word in GetTopWords(text, 5))
                        Console.WriteLine($"{word.Word}: {word.Count}");
                    break;
                case "5":
                    Console.Write("Введите новый текст: ");
                    text = Console.ReadLine() ?? "";
                    Console.WriteLine("Текст обновлён.");
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неизвестный пункт меню.");
                    break;
            }
        }
    }

    static int GetWordCount(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        string cleanedText = RemovePunctuation(text);
        string[] words = cleanedText.Split(
            new[] { ' ', '\t', '\n', '\r' },
            StringSplitOptions.RemoveEmptyEntries
        );

        return words.Length;
    }

    static string GetMostFrequentWord(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";

        string cleanedText = RemovePunctuation(text).ToLowerInvariant();
        string[] words = cleanedText.Split(
            new[] { ' ', '\t', '\n', '\r' },
            StringSplitOptions.RemoveEmptyEntries
        );

        return words
            .GroupBy(word => word)
            .OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key)
            .Select(group => group.Key)
            .FirstOrDefault() ?? "";
    }

    static string RemovePunctuation(string text)
    {
        return Regex.Replace(text, @"\p{P}", "");
    }

    static (string Word, int Count)[] GetTopWords(string text, int count)
    {
        if (string.IsNullOrWhiteSpace(text) || count <= 0)
            return Array.Empty<(string Word, int Count)>();

        string cleanedText = RemovePunctuation(text).ToLowerInvariant();
        string[] words = cleanedText.Split(
            new[] { ' ', '\t', '\n', '\r' },
            StringSplitOptions.RemoveEmptyEntries
        );

        return words
            .GroupBy(word => word)
            .Select(group => (Word: group.Key, Count: group.Count()))
            .OrderByDescending(item => item.Count)
            .ThenBy(item => item.Word)
            .Take(count)
            .ToArray();
    }
}
