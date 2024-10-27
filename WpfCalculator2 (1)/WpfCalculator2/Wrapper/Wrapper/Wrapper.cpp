#include "pch.h"
#include "math.h"
#include "Wrapper.h"

namespace Wrapper
{
	double MyArithmetic::Add(double a, double b)
	{
		return a + b;
	}

	double MyArithmetic::Subtract(double a, double b)
	{
		return a - b;
	}

	double MyArithmetic::Multiply(double a, double b)
	{
		return a * b;
	}

	double MyArithmetic::Divide(double a, double b)
	{
		return a / b;
	}

	double MyArithmetic::Exponent(double a)
	{
		return 1 / a;
	}

	double MyArithmetic::Percent(double a, double b)
	{
		return a * (b/100);
	}

	double MyArithmetic::Root(double a)
	{
		return sqrt(a);
	}

	double MyArithmetic::Square(double a)
	{
		return a * a;
	}
}