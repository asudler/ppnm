### Final Examination Project
# LU Decomposition of a real square matrix

_Implement the LU-decomposition of a real square matrix. Implement also the corresponding functions to solve linear equations, calculate the determinant, and calculate the inverse matrix. Is your LU faster than your QR?_

**TASK A:** The lu-decomposition routine can be found in lu.cs. I followed the 2024 textbook p. 23-24. It uses the matrix.cs and vector.cs libraries which Dmitri provided to us at the beginning of the semester. The results of my checks of the algorithm can be found in the output file out.debug.log.

**TASK B:** The corresponding functions to solve linear equations, calculate the determinant, and to calculate the inverse are also found in the output file lu.cs. The checks are in out.debug.log.

**TASK C:** I compared the speed of my QRGS decomposition algorithm with my LU decomposition algorithm. Using the script in in.time.gpi, the time data are plotted in out.time.png, and the fit parameters from gnuplot are given in out.qrfit.log and out.lufit.log. 

The LU decomposition routine is faster than the QRGS decomposition routine. Comparing the fit parameters, it looks like the LU routine takes 1/3 the time of the QRGS routine. This makes sense since the complexity of the QRGS routine for real square matrices is $O(n^3)$ and for LU decomposition it is $\frac{1}{3} O(n^3)$ when counted in the same way.

**SELF-EVAL:** 9 out of 10. I answered every part of the examination prompt adequately, no problems there. However, I did not implement the row and column pivoting as described in the textbook (and Wikipedia), so my algorithm may fail for specific edge cases. But it works very well otherwise.

note: some of the checks fail for larger n due to rounding errors

