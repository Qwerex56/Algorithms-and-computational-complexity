// See https://aka.ms/new-console-template for more information

using Multithreading_List1;

var le = new LaplaceExpansion([
    1, -2, 0, 3,
    1, 0, -4, 5,
    5, 2, 0, 2,
    1, -5, -2, 1
]);

var res = le.UseLaplaceExpansion();

Console.WriteLine(
    le.UseLaplaceExpansion()
);

// uint threadCount = 4;
// long number = 2837465;
// var threads = new List<Thread>();
//
// switch (args.Length) {
//     case 1:
//         Console.WriteLine(
//             long.TryParse(args.First(), out number)
//                 ? $"New number = {number}"
//                 : $"Number unchanged = {number}"
//         );
//         break;
//     case 2:
//         Console.WriteLine(
//             long.TryParse(args.First(), out number)
//                 ? $"New number = {number}"
//                 : $"Number unchanged = {number}"
//         );
//         Console.WriteLine(
//             uint.TryParse(args.Last(), out threadCount)
//                 ? $"New number of threads = {threadCount}"
//                 : "Number of threads unchanged"
//         );
//         break;
// }
//
// var numberRange = number / threadCount + number % threadCount;
// var resultingFactors = new List<long>();
//
// for (var threadId = 0; threadId < threadCount; ++threadId) {
//     var nf = new NumberFactors(
//         1 + threadId * numberRange,
//         numberRange * (threadId + 1) + 1,
//         number
//     );
//     
//     var newThread = new Thread(() => resultingFactors.AddRange(nf.GetFactors()));
//     
//     threads.Add(newThread);
//     newThread.Start();
// }
//
// foreach (var thread in threads) {
//     thread.Join();
// }
//
// foreach (var factor in resultingFactors) {
//     Console.Write($"{factor}, ");
// }