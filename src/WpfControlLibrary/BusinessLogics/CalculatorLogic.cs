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
        { }

        private string mValStash = "";
        private bool mNeedDisplayByDouble = false;
        public async Task RecieveNumberCommand(string str)
        {
            await Task.Run(() =>
            {
                if (str == "+/-") { 
                    this.mValStash = this.mValStash[0] == '-' ? this.mValStash.Substring(1) : "-" + this.mValStash;
                }
                else
                    this.mValStash = this.mValStash + str;

                if (str == ".")
                    mNeedDisplayByDouble = true;

                this.mCurVal.OnNext(this.mValStash);
            });
        }

        public async Task RecieveOperatorCommand(string str)
        {
            await Task.Run(() =>
            {
                if (this.mValQueue.Count == 0)
                {
                    this.mValQueue.Enqueue(this.mValStash);
                    this.mValQueue.Enqueue(str);
                    this.mCurExpression.OnNext(this.mValStash + str);
                }
                else if (this.mValQueue.Count == 2 || str == "=")
                {
                }
                else
                {
                    Console.WriteLine($"queue count = {this.mValQueue.Count}:");
                    while (this.mValQueue.Count > 0)
                    {
                        Console.WriteLine($"\t{this.mValQueue.Count}-th element = {this.mValQueue.Dequeue()}");
                    }
                    Console.WriteLine($"mValStash = {this.mValStash}");
                    Console.WriteLine($"mCurExpression = {this.mCurExpression}");

                    mValStash = "";
                    mCurVal.OnNext(this.mValStash);
                    mCurExpression.OnNext(this.mValStash);

                    throw new Exception($"Occur error as CalculatorLogic.RecieveOperatorCommand\'s value queue over unexpected elements number!");
                }
            });
        }

        public async Task RecieveOtherCommand(string str)
        {
            await Task.Run(() =>
            { 
            });
        }
    }
}
