#pragma once

using namespace System;

namespace Wrapper {
	public ref class MyArithmetic
	{
	public:
		double Add(double a, double b);
		double Subtract(double a, double b);
		double Multiply(double a, double b);
		double Divide(double a, double b);
		double Exponent(double a);
		double Percent(double a, double b);
		double Root(double a);
		double Square(double a);

	};
}