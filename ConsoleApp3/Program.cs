using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main()
    {
        int n = 10000;//
        int[] array = GenerateUniqueNumbers(n).ToArray();

        // Тестирование SkipList
        var skipList = new SkipList<int, int>();
        Stopwatch stopwatch = Stopwatch.StartNew();
        foreach (var num in array)
            skipList.Add(num, num);
        stopwatch.Stop();
        Console.WriteLine($"SkipList заполнен за {stopwatch.ElapsedMilliseconds} мс");

        // Удаление элементов от n/2 до 3n/4
        stopwatch.Restart();
        for (int i = n / 2; i < 3 * n / 4; i++)
            skipList.Remove(array[i]);
        stopwatch.Stop();
        Console.WriteLine($"SkipList удаление: {stopwatch.ElapsedMilliseconds} мс");

        // Поиск всех элементов
        stopwatch.Restart();
        foreach (var num in array)
            skipList.TryGetValue(num, out _);
        stopwatch.Stop();
        Console.WriteLine($"SkipList поиск: {stopwatch.ElapsedMilliseconds} мс");

        // Тестирование SortedList
        var sortedList = new SortedList<int, int>();
        stopwatch.Restart();
        foreach (var num in array)
            sortedList.Add(num, num);
        stopwatch.Stop();
        Console.WriteLine($"SortedList заполнен за {stopwatch.ElapsedMilliseconds} мс");

        stopwatch.Restart();
        for (int i = n / 2; i < 3 * n / 4; i++)
            sortedList.Remove(array[i]);
        stopwatch.Stop();
        Console.WriteLine($"SortedList удаление: {stopwatch.ElapsedMilliseconds} мс");

        stopwatch.Restart();
        foreach (var num in array)
            sortedList.TryGetValue(num, out _);
        stopwatch.Stop();
        Console.WriteLine($"SortedList поиск: {stopwatch.ElapsedMilliseconds} мс");
    }

    private static HashSet<int> GenerateUniqueNumbers(int count)
    {
        var random = new Random();
        var numbers = new HashSet<int>();
        while (numbers.Count < count)
            numbers.Add(random.Next());
        return numbers;
    }
}