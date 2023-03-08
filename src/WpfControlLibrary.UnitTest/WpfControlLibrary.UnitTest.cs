using NUnit.Framework;
using WpfControlLibrary;
using WpfControlLibrary.Base;
using WpfControlLibrary.Views;
using System;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;

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
            int expected = CalculatorTestResultValidationHelper.Instance.Calculate_TestInputNum_Operator_Num_Expected(num1, opt, num2);
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

        [SetUp]
        public void SetUp()
        { 
            this.mViewModel = new ViewModel.CalculatorViewModel();
        }

        private void ClickButton(ViewModel.CalculatorViewModel logic, string[] nums)
        { 
            foreach (var n in nums)
            {
                this.mViewModel.NumberBtn.Execute(n);
                System.Threading.Thread.Sleep(10);
            }
        }

        [Test]
        public void TestTypeNumberInInitialState()
        {
            // Arrange
            string[] btns = { "1", "2", "3", "4"};

            // Act
            ClickButton(this.mViewModel, btns);

            // Assert
            Assert.IsTrue(this.mViewModel.Value == "1234", $"this value = {this.mViewModel.Value}, not 1234");
        }
    }
}