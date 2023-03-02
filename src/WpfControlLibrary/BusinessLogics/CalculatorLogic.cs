using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfControlLibrary.Base;

namespace WpfControlLibrary.BusinessLogics
{
    internal class CalculatorLogic
    {
        private ISubject<string> mCurExpression; // display on upper textblock
        public IObservable<string> CurrentExpression => mCurExpression.AsObservable();
        private ISubject<string> mCurVal; // display on down textblock
        public IObservable<string> Value => mCurVal.AsObservable();

        private Queue<string> mValQueue;
        public CalculatorLogic()
        { 
            this.mValQueue = new Queue<string>();
            this.mCurVal = new Subject<string>();
            this.mCurExpression = new Subject<string>();
        }

        private string mValStash = "";
        public async Task ReceiveNumberCommand(string str)
        {
            await Task.Run(() =>
            {
                if (str == "+/-") { 
                    this.mValStash = this.mValStash[0] == '-' ? this.mValStash.Substring(1) : "-" + this.mValStash;
                }
                else
                    this.mValStash = this.mValStash + str;

                this.mCurVal.OnNext(this.mValStash);
            });
        }

        private T CalculateInstance<T>(T n1, T n2, string opt)
        {
            Type type = typeof(T);
            //if (type != typeof(double) || type != typeof(int) || type != typeof(float) ||
            //    type != typeof(System.Int32) || type != typeof(System.Double) || type != typeof(System.Single))
            //    throw new ArgumentException("Type is not double, int or float.");

            dynamic _n1 = n1;
            dynamic _n2 = n2;
            dynamic result = -9999999999999;
            if (opt == "+")
            {
                result = _n1 + _n2;
                return result;
            }
            else if (opt == "-")
            {
                result = _n1 - _n2;
                return result;
            }
            else if (opt == "*")
            {
                result = _n1 * _n2;
                return result;
            }
            else if (opt == "/")
            {
                result = _n1 / _n2;
                return result;
            }
            else
            {
                throw new ArgumentException("Undefined operator!");
            }
        }

        private string Calculate(string num1, string opt, string num2)
        { 
            bool byDouble = num1.IndexOf(".") != -1 || num2.IndexOf(".") != -1;

            if (byDouble)
            { 
                double n1 = double.Parse(num1);
                double n2 = double.Parse(num2);
                return $"{CalculateInstance<double>(n1, n2, opt)}";
            }
            else
            { 
                int n1 = int.Parse(num1);
                int n2 = int.Parse(num2);
                return $"{CalculateInstance<int>(n1, n2, opt)}";
            }
        }

        public async Task ReceiveOperatorCommand(string str)
        {
            await Task.Run(() =>
            {
                if (this.mValStash != "")
                {
                    if (this.mValQueue.Count > 0)
                    {
                        this.mValQueue.Enqueue(str);
                        this.mValQueue.Enqueue(this.mValStash);
                    }
                    else
                    {
                        this.mValQueue.Enqueue(this.mValStash);
                        this.mValQueue.Enqueue(str);
                        this.mCurExpression.OnNext(this.mValStash + str);
                    }
                    this.mValStash = "";
                }
                
                if (this.mValQueue.Count > 2 || str == "=")
                {
                    string num1 = this.mValQueue.Dequeue();
                    string opt = this.mValQueue.Dequeue();
                    string num2 = this.mValQueue.Dequeue();
                    string result = Calculate(num1, opt, num2);

                    this.mValStash = result;
                    this.mCurExpression.OnNext($"{num1} {opt} {num2}");
                }
                this.mCurVal.OnNext(this.mValStash);
            });
        }

        public async Task ReceiveOtherCommand(string str)
        {
            await Task.Run(() =>
            { 
            });
        }
    }
}
