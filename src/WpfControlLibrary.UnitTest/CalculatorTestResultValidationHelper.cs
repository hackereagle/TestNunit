using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfControlLibrary.UnitTest
{
    internal class CalculatorTestResultValidationHelper
    {
        private static CalculatorTestResultValidationHelper mInstance = null;
        private CalculatorTestResultValidationHelper() { } 

        public static CalculatorTestResultValidationHelper Instance 
        { 
            get 
            {
                if (mInstance == null)
                { 
                    mInstance= new CalculatorTestResultValidationHelper();
                }
                return mInstance; 
            } 
        }

        public int ConvertStringArrayToInt(string[] nums)
        {
            int ret = 0;
            int i = 0;
            int n = 0;
            while (i < nums.Length && int.TryParse(nums[i], out n))
            {
                ret = ret * 10 + n;
                i++;
            }
            return ret;
        }

        public int Calculate_TestInputNum_Operator_Num_Expected(string[] num1, string opt, string[] num2)
        {
            int ret = 0;
            int _num1 = ConvertStringArrayToInt(num1);
            int _num2 = ConvertStringArrayToInt(num2);

            if (opt == "+")
            {
                ret = _num1 + _num2;
            }
            else if (opt == "-")
            { 
                ret = _num1 - _num2;
            }
            else if (opt == "*")
            { 
                ret = _num1 * _num2;
            }
            else if (opt == "/")
            {
                if (_num2 == 0)
                    throw new Exception("Divisor cannot be zero");

                ret = _num1 / _num2;
            }

            return ret;
        }
    }
}
