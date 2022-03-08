# Vitalograph Assessment - Caesar Shift
-----

## Overview of improvements
As this program relies on user input, I've focused my improvements on usability, with only a fraction of performance tradeoff (which is negligible in a real world application - see [BenchMarks](#benchmarks)).  
I've given the user more prompts for input and the program will continuously prompt them in the event their input is invalid.  
I've also given the developer(s) the option to pass values directly to the `CaesarShift` function which can now be used in a library. This removes it from the `Main` entry point and provides more expandability.  
The code is now fully commented which will allow other developers working on the project a good idea of what each line does. This is exceptionally important within group projects as every developer will have a slightly different way of coding and logic; code for other's, not just for yourself.  
Another way to increase usability, expandability and maintainability is by taking the *magic* numbers out of the function scope and creating constant variables which makes it easier to upgrade the program as anyone will be able to see them easily at the top of the class.  

## BenchMarks
Below are the results from the benchmarking done on both functions over 10,000 iterations:
```
Old CaesarShift:
Average Time Elapsed: 0.00023244 ms over 10000 iterations

Improved CaesarShift:
Average Time Elapsed: 0.0003316 ms over 10000 iterations
```
As you can see, there is a very minimal loss in performance which has been sacrificed in order to provide better usability for both the end user and the developer(s).

BenchMarking code used:
```cs
private static double Profiler(string description, Action function, int iterations = 10000)
{
	// set process and thread priority
	Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
	Thread.CurrentThread.Priority = ThreadPriority.Highest;

	// warm up function call
	function();

	var stopWatch = new Stopwatch();

	// garbage collection clean up
	GC.Collect();
	GC.WaitForPendingFinalizers();
	GC.Collect();

	// start benchmarking
	stopWatch.Start();
	for (var i = 0; i < iterations; i++)
	{
		function();
	}

	// finish benchmarking
	stopWatch.Stop();
	Console.WriteLine(description);
	var average = (double)(stopWatch.Elapsed.TotalMilliseconds / iterations);
	Console.WriteLine($"Average Time Elapsed: {average} ms over {iterations} iterations");
	return stopWatch.Elapsed.TotalMilliseconds;
}
```