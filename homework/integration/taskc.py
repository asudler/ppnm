import scipy.integrate as integrate
from numpy import inf
import numpy as np

def f1(x):
    return 1/x**2

def f2(x):
    return 2/(1 + x**2)

def f3(x):
    return np.exp(-x**2)

print('### Python Implementation ###')
err=1E-4 # same errors as c-sharp code
print(
    'abserr = {:},'.format(err),
    'relerr = {:}'.format(err)
)
sol=integrate.quad(f1, 1, inf, full_output=1, epsabs=err, epsrel=err)
print(
    'scipy.integrate.quad ({:} calls):'.format(next(iter(sol[2].values()))), 
    '∫[1,inf] dx 1/x/x = {:},'.format(sol[0]),
    'error estimate = {:}'.format(sol[1])
)
sol=integrate.quad(f2, -inf, 0, full_output=1, epsabs=err, epsrel=err)
print(
    'scipy.integrate.quad ({:} calls):'.format(next(iter(sol[2].values()))),
    '∫[-inf,0] dx 1/(1+x*x) = {:},'.format(sol[0]),
    'error estimate = {:}'.format(sol[1])
)
sol=integrate.quad(f3, -inf, inf, full_output=1, epsabs=err, epsrel=err)
print(
    'scipy.integrate.quad ({:} calls):'.format(next(iter(sol[2].values()))),
    '∫[-inf,inf] dx exp(-x*x) = {:},'.format(sol[0]),
    'error estimate = {:}'.format(sol[1])
)

