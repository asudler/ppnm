using System;
using static System.Math;
using static System.Console;
using static cmath;
class main{
static int Main(){
	int return_code=0;
	bool test;
	var rnd=new Random();
	int n=9;
	complex[] zs = new complex[n];
	for(int i=0;i<n;i++)
		zs[i]=new complex(2*rnd.NextDouble()-1,2*rnd.NextDouble()-1);

	Write("testing explicit cast from int ... ");
	test=complex.One.approx((complex)1);
	Write($" {(complex)1} ");
	if(test) Write(" ...passed\n");

	Write("testing explicit cast from double ... ");
	test=complex.One.approx((complex)1.0);
	Write($" {(complex)1.0} ");
	if(test) Write(" ...passed\n");

	Write("testing exp(log(z))=z ...");
	test=true;
	for(int i=0;i<n;i++){
		complex z=zs[i];
		test=test && exp(log(z)).approx(z);
	}
	if(test) Write(" ...passed\n");
	else { Write(" ...FAILED\n"); return_code += 1; }

	Write("testing log(exp(z))=z ...");
	test=true;
	for(int i=0;i<n;i++){
		complex z=zs[i];
		test=test && log(exp(z)).approx(z);
	}
	if(test) Write(" ...passed\n");
	else { Write(" ...FAILED\n"); return_code += 1; }

	Write("testing abs(z)^2=z*conjugate(z) ...");
	test=true;
	for(int i=0;i<n;i++){
		complex z=zs[i];
		test=test && abs(z).pow(2).approx(z*~z);
	}
	if(test) Write(" ...passed\n");
	else { Write(" ...FAILED\n"); return_code += 1; }

	Write("testing sqrt(a)*sqrt(a)=a ...");
	test=true;
	for(int i=0;i<n;i++){
		complex z=zs[i];
		test=test && z.approx(sqrt(z)*sqrt(z));
	}
	if(test) Write(" ...passed\n");
	else { Write(" ...FAILED\n"); return_code += 1; }

	Write("testing sin(a)^2+cos(a)^2=1 ...");
	test=true;
	for(int i=0;i<n;i++){
		complex z=zs[i];
		test=test &&
			(sin(z).pow(2)+cos(z).pow(2)).approx(1);
	}
	if(test) Write(" ...passed\n");
	else { Write(" ...FAILED\n"); return_code += 1; }

	Write("testing sin(a+b)=sin(a)*cos(b)+cos(a)*sin(b) ...");
	test=true;
	for(int i=0;i<n-1;i++){
		complex a=zs[i],b=zs[i+1];
		test=test &&
			sin(a+b).approx(sin(a)*cos(b)+cos(a)*sin(b));
	}
	if(test) Write(" ...passed\n");
	else { Write(" ...FAILED\n"); return_code += 1; }

	Write("testing (a/b)*b=a ...");
	test=true;
	for(int i=0;i<n-1;i++){
		complex a=zs[i],b=zs[i+1];
		test=test &&
			((a/b)*b).approx(a);
	}
	if(test) Write(" ...passed\n");
	else { Write(" ...FAILED\n"); return_code += 1; }

	Write("testing exp(a+b)=exp(a)*exp(b) ...");
	test=true;
	for(int i=0;i<n-1;i++){
		complex a=zs[i],b=zs[i+1];
		test=test &&
			(exp(a)*exp(b)).approx(exp(a+b));
	}
	if(test) Write(" ...passed\n");
	else { Write(" ...FAILED\n"); return_code += 1; }

	Write("testing cos(a+b)=cos(a)*cos(b)-sin(a)*sin(b) ...");
	test=true;
	for(int i=0;i<n-1;i++){
		complex a=zs[i],b=zs[i+1];
		test=test &&
			cos(a+b).approx(cos(a)*cos(b)-sin(a)*sin(b));
	}
	if(test) Write(" ...passed\n");
	else { Write(" ...FAILED\n"); return_code += 1; }


	Write("testing abs(z)*exp(I*arg(z))=z ...");
	test=true; complex I=complex.I;
	for(int i=0;i<n;i++){
		complex z=zs[i];
		test=test &&
			( abs(z)*exp(I*arg(z)) ).approx(z);
	}
	if(test) Write(" ...passed\n");
	else { Write(" ...FAILED\n"); return_code += 1; }

    WriteLine();
    WriteLine();
    WriteLine("now performing the tests outlined in the exercise description");

    Write("sqrt(-1) ?= 1j ...");
    complex arg=-1;
    complex lhs=sqrt(arg);
    complex rhs=complex.I;
    if(lhs.Re.approx(rhs.Re) && lhs.Im.approx(rhs.Im)) Write("...passed\n");
    else { Write(" ...FAILED\n"); return_code += 1; }

    Write("sqrt(1j) ?= 1/sqrt(2) + 1j/sqrt(2) ...");
    complex larg=complex.I;
    lhs=sqrt(larg);
    rhs=1/sqrt(2.0) + complex.I/sqrt(2.0);
    if(lhs.approx(rhs)) Write("...passed\n");
    else { Write(" ...FAILED\n"); return_code += 1; }
   
    Write("exp(1j) ?= cos(1) + 1j*sin(1) ... ");
    lhs=exp(larg);
    rhs=cos(1.0)+I*sin(1.0);
//    Write($"{lhs.Re} {lhs.Im} {rhs.Re} {rhs.Im}");
    if(lhs.approx(rhs)) Write("...passed\n");
    else { Write(" ...FAILED\n"); return_code += 1; }

    Write("exp(1j*pi) ?= -1 ....");
    lhs=exp(larg*PI);
    rhs=-1.0;
    if(lhs.approx(rhs)) Write("...passed\n");
    else { Write(" ...FAILED\n"); return_code += 1; }

    Write("1j^1j ?= exp(-pi/2) ... ");
    lhs=cmath.pow(larg,larg);
    rhs=exp(-PI/2.0);
    if(lhs.approx(rhs)) Write("...passed\n");
    else { Write(" ...FAILED\n"); return_code += 1; }

    Write("ln(1j) ?= 1j*pi/2 ... ");
    lhs=log(larg);
    rhs=larg*PI/2.0;
    if(lhs.approx(rhs)) Write("...passed\n");
    else { Write(" ...FAILED\n"); return_code += 1; }

    Write("sin(1j*pi) ?= (1/2j)*[exp(-pi)-exp(pi)] ...");
    lhs=sin(larg*PI);
    rhs=(1.0/(2.0*larg))*(exp(-PI)-exp(PI));
    if(lhs.approx(rhs)) Write("...passed\n");
    else { Write(" ...FAILED\n"); return_code += 1; }

    Write("EXTRA: sinh(1j) ?= 1j*sin(1) ... ");
    lhs=cmath.sinh(larg);
    rhs=larg*sin(1.0);
    if(lhs.approx(rhs)) Write("...passed\n");
    else { Write(" ...FAILED\n"); return_code += 1; }

    Write("EXTRA: cosh(1j) ?= cos(1) ... ");
    lhs=cmath.cosh(larg);
    rhs=cos(1.0);
    if(lhs.approx(rhs)) Write("...passed\n");
    else { Write(" ...FAILED\n"); return_code += 1; }

    WriteLine();
if(return_code==0)
	Write("all tests passed :)\n");
else 
	Write("{0} tests FAILED :(\n",return_code);
return return_code;

}//Main
}//main
