### LU Decomposition of a real square matrix

_Implement the LU-decomposition of a real square matrix. Implement also the corresponding functions to solve linear equations, calculate the determinant, and calculate the inverse matrix. Is your LU faster than your QR?_

**TASK A:** See _out.debug.log_ and _out.debug_pivots.log_. The lu-decomposition routine with pivoting can be found in lu.cs. I followed the 2024 textbook p. 23-24. It uses the matrix.cs and vector.cs libraries which Dmitri provided to us at the beginning of the semester. The pivoting mechanism is only triggered for matrices where the (0,0) element is approximately equal to 0.

**TASK B:** See _out.debug.log_ and _out.debug_pivots.log_. The corresponding functions to solve linear equations, calculate the determinant, and to calculate the inverse are also found in the file lu.cs.

**TASK C:** See _out.time.png_. I compared the speed of my QRGS decomposition algorithm with my LU decomposition algorithm. Using the script in in.time.gpi, the fit parameters from gnuplot are given in out.qrfit.log and out.lufit.log. 

The LU decomposition routine is faster than the QRGS decomposition routine. Comparing the fit parameters, it looks like the LU routine takes 1/3 the time of the QRGS routine. This makes sense since the complexity of the QRGS routine for real square matrices is $O(n^3)$ and for LU decomposition it is $\frac{1}{3} O(n^3)$ when counted in the same way.

**SELF-EVAL:** 9 out of 10. I answered every part of the examination prompt thoroughly, no problems there. My algorithm even accounts for edge cases where row pivoting might be necessary. But I am sure the algorithm could be improved or tested even more rigorously, and it also feels a bit conceited to give myself a 10. Thus I will be content with a 9!

note: some of the checks fail for larger n due to machine rounding errors

