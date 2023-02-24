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
        private ISubject<string> mCurExpression;
        public IObservable<string> CurrentExpression => mCurExpression.AsObservable();
        private ISubject<string> mCurVal;
        public IObservable<string> Value => mCurVal.AsObservable();

        private Queue<string> m;
        public CalculatorLogic()
        { }

        public async Task RecieveNumberCommand(string str)
        {
            await Task.Run(() =>
            { 
            });
        }

        public async Task RecieveOperatorCommand(string str)
        {
            await Task.Run(() =>
            { 
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
