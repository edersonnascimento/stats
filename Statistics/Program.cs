﻿using Statistics.classes;

const int LENGTH = 4;
const int TOP = 30;

var arguments = Environment.GetCommandLineArgs();
string fileName = arguments == null || arguments.Length < 2 ? "./resources/combinations-15-big-collection.csv" : arguments[1];


if (File.Exists(fileName)) {
    Dictionary<long, int> pairs = new Dictionary<long, int>();
    var collection = Transform.CreateBitmapRange(1, LENGTH, 25);

    var lines = File.ReadAllLines(fileName);
    var csv = new ParseCsv(lines);

    var current = 0;
    var values = new Int64[lines.Length];

    while (csv.GetNextLine() is string[] line) {
        values[current] = Transform.ToBitmap(line);
        current++;
    }

    collection.ForEach((test) => {
        for (int j = 0; j < values.Length; j++) {
            if ((test & values[j]) == test) {
                if (pairs.ContainsKey(test))
                    pairs[test]++;
                else
                    pairs.Add(test, 1);
            }
        }
    });
    
    pairs = pairs.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

    Console.WriteLine($"Total lines {lines.Length}");
    Console.WriteLine("".PadRight(60, '-'));

    int top = (pairs.Count() > TOP) ? TOP : pairs.Count();
    long sum = 0;
    for(int i = 0; i < top; i++) {
        var pair = pairs.ElementAt(i);
        sum |= pair.Key;
        Console.WriteLine($"{i} - Combination {Transform.GetSequence(pair.Key, 25)} - Occours {pair.Value} times ({Math.Round(((decimal)pair.Value / (decimal)lines.Length) * 100, 2)}%)");
    }
    
    var scomb = Transform.GetSequence(sum, 25);
    var count = scomb.Split(';').Length;

    Console.WriteLine("".PadRight(60, '-'));
    Console.WriteLine($"Summary {scomb} => {count} numbers");
} else {
    Console.WriteLine("File not found");
    return;
}
