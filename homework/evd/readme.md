# 2024-03-16

Attempting homework "EVD" (eigenvalue decomposition) via the implementation of the Jacobi eigenvalue algorithm. I will use the matrix and vector classes from lineq, which were originally provided by Dmitri.

(b) It appears the converged groundstate energy is 0.5 Hartrees, which we can get pretty close to by setting dr = 0.05 and rmax = 20.

(c) My implementation shows that the number of operations for my eigenvalue decomposition algorithm indeed scales as $n^3$; however, I had to cheat a little to implement the parallelization properly... if I want sufficient points to fit (>50), then I cannot simply run everything in the background as a way of parallelizing since my computer only has 16 processors. I would need to "batch" my jobs such that I only run 16 at a time, but I could not figure out how to do this (yet)... so instead, I force the for loop to wait for 0.1s before beginning the next job, which allows the last few jobs to run in parallel while not competing for computer resources. In the future, I hope to learn how to properly batch my jobs!
 
