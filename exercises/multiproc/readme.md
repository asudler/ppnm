# Pitfalls in Multiprocessesing

Both of my attempts at using built-in parallelization methods not only failed but also yielded the incorrect results. Of course, my implementation could be entirely incorrect, but it seems the point of this exercise was to give us examples of sitatuions where serial for-loop parallelization works and built-in parallelization does not.

The built-in methods run slower likely due to more overhead than our simple parallelization example. For example, the document pages for C# describe how the Parallel.For method must initialize and synchronize during its use. We were able to avoid this slowdown by understanding our problem and developing a simple parallelization fix which does not involve any overhead.

Furthermore, the built-in methods return an incorrect result due to the race conditions that my implementation creates. Each thread may attempt to update the sum asynchroniously, leading to small errors which ultimately yeild an incorrect result.

(if my implementation of the Parallel.For methods is entirely incorrect, please let me know and I will revise my code for this exercise)
