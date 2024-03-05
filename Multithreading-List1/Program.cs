// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Multithreading_List1;
using ThreadState = System.Threading.ThreadState;

Console.WriteLine("Select: ");
Console.WriteLine("1. All factors\n2. Laplace expansion");

var key = Console.ReadKey();

switch (key.Key) {
    case ConsoleKey.D1:
        RunCalcFactors();
        break;
    case ConsoleKey.D2:
        RunLaplaceExpansion();
        break;
    case ConsoleKey.Escape:
        break;
}

return;

void RunCalcFactors() {
    var threadCount = 1U;
    var testNumber = 0L;

    Console.WriteLine("\nChange thread count? (current threads: 1) Y/N (Yes/No)");
    if (Console.ReadKey().Key == ConsoleKey.Y) {
        Console.WriteLine($"\nEnter new thread number (max: {Environment.ProcessorCount}): ");

        Console.WriteLine(
            uint.TryParse(Console.ReadLine(), out threadCount)
                ? $"Thread number changed, new thread count: {threadCount}."
                : $"Thread number unchanged, thread count: {threadCount}."
        );
    }

    Console.WriteLine($"\nChange test number? (current number: {testNumber}) Y/N (Yes/no)");
    if (Console.ReadKey().Key == ConsoleKey.Y) {
        Console.WriteLine("\nEnter new test number: ");

        Console.WriteLine(
            long.TryParse(Console.ReadLine(), out testNumber)
                ? $"Test number changed, new test number: {testNumber}."
                : $"Test number unchanged, test number: {testNumber}."
        );
    }

    var range = testNumber / threadCount + testNumber % threadCount;
    var threads = new List<Thread>();
    var factors = new List<long>();

    var sw = new Stopwatch();
    sw.Start();

    foreach (var threadId in Enumerable.Range(0, (int)threadCount)) {
        var lowThreshold = range * threadId;
        var highThreshold = range * threadId + range;

        var nf = new NumberFactors(lowThreshold, highThreshold, testNumber);
        var thread = new Thread(
            () => factors.AddRange(nf.GetFactors())
        );

        thread.Start();
        threads.Add(thread);
    }

    while (threads.Count > 0) {
        var thread = threads.LastOrDefault();

        if (thread is null || thread.ThreadState != ThreadState.Stopped) {
            continue;
        }

        thread.Join();
        threads.Remove(thread);
    }

    sw.Stop();
    Console.WriteLine($"\nExecution time (ms): {sw.ElapsedMilliseconds}.");

    foreach (var factor in factors) {
        Console.WriteLine(factor);
    }

    Console.WriteLine("Press any key to exit");
    Console.ReadKey();
}

void RunLaplaceExpansion() {
    // var le = new LaplaceExpansion([
    //     1, -2, 0, 3,
    //     1, 0, -4, 5,
    //     5, 2, 0, 2,
    //     1, -5, -2, 1
    // ], 1);
    
    // var le = new LaplaceExpansion([
    //     1, 0, 0, 0, 0, 0,
    //     0, 1, 0, 0, 0, 0,
    //     0, 0, 1, 0, 0, 0,
    //     0, 0, 0, 1, 0, 0,
    //     0, 0, 0, 0, 1, 0,
    //     0, 0, 0, 0, 0, 1,
    // ]);

    var le = new LaplaceExpansion([
        1, 0, 0,
        0, 1, 0,
        0, 0, 1
    ]);

    var sw = new Stopwatch();
    sw.Start();
    
    var result = le.UseLaplaceExpansion(le.RefMatrix);
    
    sw.Stop();
    Console.WriteLine($"\nExecution time (ms): {sw.ElapsedMilliseconds}.");
    
    Console.WriteLine($"Matrix det: {result}");
}