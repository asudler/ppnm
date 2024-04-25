import scipy.integrate as integrate
import numpy as np

def f1(x):
    return 1/np.sqrt(x)

def f2(x):
    return np.log(x)/np.sqrt(x)

err=1E-4 # same errors as c-sharp code
sol=integrate.quad(f1, 0, 1, full_output=1, epsabs=err, epsrel=err)
print('### Python Implementation ###')
print(
    'abserr = {:},'.format(err),
    'relerr = {:}'.format(err)
)
print(
    'scipy.integrate.quad ({:} calls):'.format(next(iter(sol[2].values()))), 
    '∫[0,1] dx 1/√(x) = {:},'.format(sol[0]),
    'error estimate = {:}'.format(sol[1])
)
sol=integrate.quad(f2, 0, 1, full_output=1, epsabs=err, epsrel=err)
print(
    'scipy.integrate.quad ({:} calls):'.format(next(iter(sol[2].values()))),
    '∫[0,1] dx ln(x)/√(x) = {:},'.format(sol[0]),
    'error estimate = {:}'.format(sol[1])
)
