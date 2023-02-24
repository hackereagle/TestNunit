using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfControlLibrary.Base;
using System.Windows.Input;
using System.Windows.Controls;

namespace WpfControlLibrary.ViewModel
{
    internal class CalculatorViewModel : Base.ViewModelBase
    {
        private AsyncCommand<object> mNumberBtnCommand;
        private AsyncCommand<object> mOperatorBtnCommand;
        private AsyncCommand<object> mOtherBtnCommand;
        BusinessLogics.CalculatorLogic mCalculatorLogic;
        public CalculatorViewModel()
        { 
            this.mCalculatorLogic = new BusinessLogics.CalculatorLogic();

            this.mNumberBtnCommand = new AsyncCommand<object>(this.NumberButtonCommand);
            this.mOperatorBtnCommand = new AsyncCommand<object>(this.OperatorButtonCommand);
            this.mOtherBtnCommand = new AsyncCommand<object>(this.OtherButtonCommand);
        }

        public ICommand NumberBtn => mNumberBtnCommand;
        public ICommand OperatorBtn => mNumberBtnCommand;
        public ICommand OtherBtn => mNumberBtnCommand;

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

        private async Task NumberButtonCommand(object obj)
        {
            Button btn = (Button)obj;
            await this.mCalculatorLogic.RecieveNumberCommand((string)btn.Content);
        }

        private async Task OperatorButtonCommand(object obj)
        { 
            Button btn = (Button)obj;
            await this.mCalculatorLogic.RecieveOperatorCommand((string)btn.Content);
        }

        private async Task OtherButtonCommand(object obj)
        { 
            Button btn = (Button)obj;
            await this.mCalculatorLogic.RecieveOtherCommand((string)btn.Content);
        }
    }
}
