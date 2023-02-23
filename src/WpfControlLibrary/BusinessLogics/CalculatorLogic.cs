using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace WpfControlLibrary.BusinessLogics
{
    internal class CalculatorLogic
    {
        private ISubject<string> mCurExpression;
        public IObservable<string> CurrentExpression => mCurExpression.AsObservable();
        private ISubject<string> mCurVal;
        public IObservable<string> Value => mCurVal.AsObservable();


    }
}
