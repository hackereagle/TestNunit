using System.ComponentModel;
//using System.Windows.Controls;

namespace WpfControlLibrary.UnitTest
{
    [TestFixture]
    public class DirectlyTestLogicClass
    {
        string CurrentValueTextBlock = "";
        string CurrentExpressionTextBlock = "";

        [SetUp]
        public void Setup()
        {
        }

        private void CreateTestObject(out BusinessLogics.CalculatorLogic logic)
        { 
            logic = new BusinessLogics.CalculatorLogic();
            logic.Value.Subscribe(x => { CurrentValueTextBlock = x; }) ;
            logic.CurrentExpression.Subscribe(x => { CurrentExpressionTextBlock = x; }) ;
        }

        private async Task TypeNumber(BusinessLogics.CalculatorLogic logic, string[] nums)
        { 
            foreach (var n in nums)
            {
                await logic.ReceiveNumberCommand(n);
            }
        }

        [Test]
        public async Task TypeNumberInInitialState()
        {
            // Arrange
            BusinessLogics.CalculatorLogic logic = null;
            CreateTestObject(out logic);
            string[] btns = { "1", "2", "3", "4"};

            // Act
            await TypeNumber(logic, btns);

            // Assert
            Assert.IsTrue(this.CurrentValueTextBlock == "1234", $"this value = {this.CurrentValueTextBlock}, not 1234");
        }

        [Test]
        public async Task TypeNumberAndAddMinus()
        {
            // Arrange
            BusinessLogics.CalculatorLogic logic = null;
            CreateTestObject(out logic);
            string[] btns = { "1", "2", "3", "4", "+/-"};

            // Act
            await TypeNumber(logic, btns);

            // Assert
            Assert.IsTrue(this.CurrentValueTextBlock == "-1234", $"this value = {this.CurrentValueTextBlock}, not 1234");

        }

        [Test]
        public async Task TestHaveMinusNumBecomePositive()
        {
            // Arrange
            BusinessLogics.CalculatorLogic logic = null;
            CreateTestObject(out logic);
            string[] btns = { "1", "2", "3", "4", "+/-", "+/-"};

            // Act
            await TypeNumber(logic, btns);

            // Assert
            Assert.IsTrue(this.CurrentValueTextBlock == "1234", $"this value = {this.CurrentValueTextBlock}, not 1234");
        }

        private int ConvertStringArrayToInt(string[] nums)
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

        private int Calculate_TestInputNum_Operator_Num_Expected(string[] num1, string opt, string[] num2)
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

        [TestCase("+")]
        [TestCase("-")]
        [TestCase("*")]
        [TestCase("/")]
        public async Task TestInputNum_Operator_Num(string opt)
        {
            // Arrange
            BusinessLogics.CalculatorLogic logic = null;
            CreateTestObject(out logic);
            string[] num1 = {"1", "2", "3", "4"};
            string[] num2 = {"5", "6", "7", "8", "9"};

            // Act
            await TypeNumber(logic, num1);
            await logic.ReceiveOperatorCommand(opt);
            await TypeNumber(logic, num2);
            await logic.ReceiveOperatorCommand("=");

            // Assert
            int expected = Calculate_TestInputNum_Operator_Num_Expected(num1, opt, num2);
            string expectedExpression = string.Join("", num1) + " " + opt + " " + string.Join("", num2) + " = "; 
            Assert.IsTrue(this.CurrentValueTextBlock == expected.ToString(), $"this value = {this.CurrentValueTextBlock}, not {expected}");
            Assert.IsTrue(this.CurrentExpressionTextBlock == expectedExpression, $"current expression = {this.CurrentExpressionTextBlock}, not {expectedExpression}");
        }

        [Test]
        public async Task TestContinuouslyTypeNumAndOperator()
        {
            // Arrange
            BusinessLogics.CalculatorLogic logic = null;
            CreateTestObject(out logic);
            string[] num1 = {"1", "2", "3", "4"};
            string[] num2 = {"5", "6", "7", "8", "9"};
            string[] num3 = {"9", "0", "8"};
            string[] num4 = {"1", "1", "1"};

            // Act
            await TypeNumber(logic, num1);
            await logic.ReceiveOperatorCommand("+");
            await TypeNumber(logic, num2);
            await logic.ReceiveOperatorCommand("*");
            await TypeNumber(logic, num3);
            await logic.ReceiveOperatorCommand("-");
            await TypeNumber(logic, num4);
            await logic.ReceiveOperatorCommand("=");

            // Assert
            int expected = 52684773;
            string expectedExpression = "52684884 - 111 = "; 
            Assert.IsTrue(this.CurrentValueTextBlock == expected.ToString(), $"this value = {this.CurrentValueTextBlock}, not {expected}");
            Assert.IsTrue(this.CurrentExpressionTextBlock == expectedExpression, $"current expression = {this.CurrentExpressionTextBlock}, not {expectedExpression}");
        }
    }

    [TestFixture]
    class TestMvvmFromViewModel
    {
        ViewModel.CalculatorViewModel mViewModel;
        string CurrentValueTextBlockText = "";
        string CurrentExpressionTextBlockText = "";

        private void CreateButtons()
        {

        }

        [SetUp]
        public void SetUp()
        { 
            this.mViewModel = new ViewModel.CalculatorViewModel();
            CreateButtons();
        }

        [Test]
        public void TestTypeNumberInInitialState()
        {
            // Arrange
            string[] btns = { "1", "2", "3", "4"};

            // Act
            foreach (var b in btns)
            {
                this.mViewModel.NumberBtn.Execute(b);
            }

            // Assert
            Assert.IsTrue(this.mViewModel.Value == "1234", $"this value = {this.mViewModel.Value}, not 1234");
        }
    }
}