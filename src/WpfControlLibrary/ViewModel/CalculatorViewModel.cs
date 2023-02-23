using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfControlLibrary.Base;

namespace WpfControlLibrary.ViewModel
{
    internal class CalculatorViewModel : Base.ViewModelBase
    {
        BusinessLogics.CalculatorLogic mCalculatorLogic;
        public CalculatorViewModel()
        { 
            this.mCalculatorLogic = new BusinessLogics.CalculatorLogic();
        }

        private string mCurrentExpression;
        public string CurrentExpression
        {
            get { return mCurrentExpression; } 
            set
            {
                mCurrentExpression = value;
                OnPropertyChanged("CurrentExpression");
            }
        }

        private string mValue;
        public string Value
        {
            get { return mValue; }
            set
            { 
                mValue = value;
                OnPropertyChanged("Value");
            }
        }

        AsyncCommand<object> mNumsCommand;
    }
}
