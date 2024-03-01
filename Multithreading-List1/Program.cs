// See https://aka.ms/new-console-template for more information

using Multithreading_List1;


uint threadCount = 4;
long number = 40;
var threads = new List<Thread>();

switch (args.Length) {
    case 1:
        Console.WriteLine(
            long.TryParse(args.First(), out number)
                ? $"New number = {number}"
                : $"Number unchanged = {number}"
        );
        break;
    case 2:
        Console.WriteLine(
            long.TryParse(args.First(), out number)
                ? $"New number = {number}"
                : $"Number unchanged = {number}"
        );
        Console.WriteLine(
            uint.TryParse(args.Last(), out threadCount)
                ? $"New number of threads = {threadCount}"
                : "Number of threads unchanged"
        );
        break;
}

var numberRange = number / threadCount + number % threadCount;

for (var threadId = 0; threadId < threadCount; ++threadId) {
    Console.WriteLine($"Range for thread {threadId}: l = {1 + threadId * numberRange}, h = {numberRange * (threadId + 1) + 1}");
    var nf = new NumberFactors(
        1 + threadId * numberRange,
        numberRange * (threadId + 1) + 1,
        number
    );
    
    var id = threadId;
    var newThread = new Thread(() => nf.GetFactors(in id));
    
    threads.Add(newThread);
}
foreach (var thread in threads) {
    thread.Start();
}

foreach (var thread in threads) {
    thread.Join();
}

