# Parallel.ForEachAsync Exceptions  

Parallel.ForEachAsync is a very useful tool for running code in parallel. But what happens when an exception is thrown inside the loop?  

By default, the following things happen:

1. **If we "await" ForEachAsync, then we get a single exception (even if exceptions are thrown in multiple iterations of the loop).**  
2. **The loop short-circuits -- meaning not all items are processed.**  

This code explores both of these issues in a couple of different ways.  

*For slides and code samples on Parallel.ForEachAsync (and other parallel approaches), you can take a look at the materials from my full-day workshop on asynchronous programming: [https://github.com/jeremybytes/async-workshop-2022](https://github.com/jeremybytes/async-workshop-2022). (These materials use .NET 6.0. Updates for .NET 8.0 are coming in a few months.) For announcements on public workshops, check here: [https://jeremybytes.blogspot.com/p/workshops.html](https://jeremybytes.blogspot.com/p/workshops.html).*  

## Articles

* [Parallel.ForEachAsync and Exceptions](https://jeremybytes.blogspot.com/2024/02/parallelforeachasync-and-exceptions.html)  
* [Getting Multiple Exceptions from Parallel.ForEachAsync](https://jeremybytes.blogspot.com/2024/02/getting-multiple-exceptions-from.html)  
* [Continue Processing with Parallel.ForEachAsync (even when exceptions are thrown)](https://jeremybytes.blogspot.com/2024/02/continue-processing-with.html)  

## Projects

* **original-code**  
[original-code/Program.cs](./ForEachAsyncException/original-code/Program.cs)  
The starting point for the code. Exceptions are thrown inside the Parallel.ForEachAsync loop (every 3rd item throws an exception).  
This exhibits both issues noted above: (1) it only shows a single exception; (2) it short-cicuits the loop.  
*See [Parallel.ForEachAsync and Exceptions](https://jeremybytes.blogspot.com/2024/02/parallelforeachasync-and-exceptions.html)*

* **continuation**  
[continuation/Program.cs](./ForEachAsyncException/continuation/Program.cs)  
Instead of awaiting ForEachAsync, this code uses a continuation to get the AggregateException and shows exceptions for all of the faulted tasks.  
This solves for issue #1 (it shows the inner exceptions), but it does **not** resolve #2 (it still short-circuits).  
*See [Getting Multiple Exceptions from Parallel.ForEachAsync](https://jeremybytes.blogspot.com/2024/02/getting-multiple-exceptions-from.html)*

* **configure-await-options**  
[configure-await-options/Program.cs](./ForEachAsyncException/configure-await-options/Program.cs)  
This code is adapted from Gérald Barré's article ["Getting all exceptions thrown from Parallel.ForEachAsync"](https://www.meziantou.net/getting-all-exceptions-thrown-from-parallel-foreachasync.htm). This uses the "SuppressThrowing" ConfigureAwait option (added in .NET 8) to throw an AggregateException (rather than having "await" unwrap to a single exception).  
This solves for issue #1 (it shows the inner exceptions), but it does **not** resolve #2 (it still short-circuits).  
*See [Getting Multiple Exceptions from Parallel.ForEachAsync](https://jeremybytes.blogspot.com/2024/02/getting-multiple-exceptions-from.html)*

* **doesnt-stop**  
[doesnt-stop/Program.cs](./ForEachAsyncException/doesnt-stop/Program.cs)  
This code catches exceptions **inside** the ForEachAsync loop.  
This solves for both issues: (1) it shows all of the exceptions, (2) it processes all of the items without short-circuiting.  
*See [Continue Processing with Parallel.ForEachAsync (even when exceptions are thrown)](https://jeremybytes.blogspot.com/2024/02/continue-processing-with.html)*

---
