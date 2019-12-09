using System;
using System.Collections.Generic;
using System.Text;

namespace LogItUpApi.Test
{

    public interface ICalculoPropina
    {
        public decimal CalcularPropina(int a, int b);
    }

    public class CalculoPropina : ICalculoPropina
    {
        private readonly IMath _iMathClass;
        public CalculoPropina(IMath iMarthClass)
        {
            _iMathClass = iMarthClass;
        }

        public decimal CalcularPropina(int a, int b)
        {
            return _iMathClass.DivideNumbers(a, b);
        }
    }


    public interface IMath
    {
        public int DivideNumbers(int a, int b);
    }

    public class Math : IMath
    {

        public int DivideNumbers(int a, int b)
        {
            if (b == 0)
                throw new DivideByZeroException("El divisor debe ser distinto de 0");

            return a / b;
        }
    }
}
