# 2024-03-18

qrgs.cs: from prior homework "lineq";
matrix.cs: from prior homework "evd" (originally provided by Dmitri);
vector.cs: from prior homework "evd" (originally provided by Dmitri).

# 2024-03-29: Answering the HW Questions

_The uncertainty of the logarithm of a dataset (from [UMass](https://openbooks.library.umass.edu/p132-lab-manual/chapter/uncertainty-for-natural-logarithms/)):_

Given a function of a single variable $f(y)$, the uncertainty on $f$, called $\delta_f$, is related to the uncertainty on $y$, called $\delta_y$, through the formula $$\delta_f = \sqrt{\left(\frac{df}{dy}\right)^2 \delta_y^2} \ .$$ Since we have $f = \ln y$, $$\delta_f = \sqrt{\left(\frac{1}{y}\right)^2 \delta_y^2} = \frac{\delta_y}{y} \ .$$

_Determining half-life time from fit_:

The fit gives $\lambda=0.171 \ \text{days}^{-1}$, so the half-life time $T_{\frac{1}{2}} = \frac{\ln 2}{\lambda} \approx 4.05 \ \text{days}$. The [accepted value for the half-life of Radium-224](https://en.wikipedia.org/wiki/Isotopes_of_radium) is 3.63 days, so we're a bit off from what's currently accepted.

_Estimating the uncertainty in half-life value_:

We will have to use [error propagation](https://en.wikipedia.org/wiki/Propagation_of_uncertainty) to solve this question. The calculated uncertainty in $\lambda$ is 0.00716. Then the uncertainty in our half-life measurement is (hopefully) given by $\left|\frac{\ln 2}{\lambda}\right|\frac{\delta_{\lambda}}{\lambda},$ which is 0.17 days. This is significantly higher than the modern value for estimated uncertainty (see the link to the Radium-224 Wikipedia page above).

