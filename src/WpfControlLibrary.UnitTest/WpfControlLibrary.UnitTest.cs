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
        string CurrentValueTextBlockText = "";
        string CurrentExpressionTextBlockText = "";

        [SetUp]
        public void SetUp()
        { 
        }

        void CreateTestObject(out ViewModel.CalculatorViewModel vm)
        { 
            vm = new ViewModel.CalculatorViewModel();
        }

        private void ClickNumberButton(ViewModel.CalculatorViewModel vm, string[] nums)
        { 
            foreach (var n in nums)
            {
                vm.NumberBtn.Execute(n);
                System.Threading.Thread.Sleep(10);
            }
        }

        [Test]
        public void TestTypeNumberInInitialState()
        {
            // Arrange
            ViewModel.CalculatorViewModel vm = null;
            CreateTestObject(out vm);
            string[] btns = { "1", "2", "3", "4"};

            // Act
            ClickNumberButton(vm, btns);

            // Assert
            Assert.IsTrue(vm.Value == "1234", $"this value = {vm.Value}, not 1234");
        }

        [Test]
        public void TypeNumberAndAddMinus()
        {
            // Arrange
            ViewModel.CalculatorViewModel vm = null;
            CreateTestObject(out vm);
            string[] btns = { "1", "2", "3", "4", "+/-"};

            // Act
            ClickNumberButton(vm, btns);

            // Assert
            Assert.IsTrue(vm.Value == "-1234", $"this value = {vm.Value}, not 1234");

        }

        [Test]
        public void TestHaveMinusNumBecomePositive()
        {
            // Arrange
            ViewModel.CalculatorViewModel vm = null;
            CreateTestObject(out vm);
            string[] btns = { "1", "2", "3", "4", "+/-", "+/-"};

            // Act
            ClickNumberButton(vm, btns);

            // Assert
            Assert.IsTrue(vm.Value == "1234", $"this value = {vm.Value}, not 1234");
        }

        private readonly int WAIT_OPT_BTN_TIME = 100;
        [TestCase("+")]
        [TestCase("-")]
        [TestCase("*")]
        [TestCase("/")]
        public void TestInputNum_Operator_Num(string opt)
        {
            // Arrange
            ViewModel.CalculatorViewModel vm = null;
            CreateTestObject(out vm);
            string[] num1 = {"1", "2", "3", "4"};
            string[] num2 = {"5", "6", "7", "8", "9"};

            // Act
            ClickNumberButton(vm, num1);
            vm.OperatorBtn.Execute(opt);
            System.Threading.Thread.Sleep(this.WAIT_OPT_BTN_TIME);
            ClickNumberButton(vm, num2);
            vm.OperatorBtn.Execute("=");
            System.Threading.Thread.Sleep(this.WAIT_OPT_BTN_TIME);

            // Assert
            int expected = CalculatorTestResultValidationHelper.Instance.Calculate_TestInputNum_Operator_Num_Expected(num1, opt, num2);
            string expectedExpression = string.Join("", num1) + " " + opt + " " + string.Join("", num2) + " = "; 
            Assert.IsTrue(vm.Value == expected.ToString(), $"this value = {vm.Value}, not {expected}");
            Assert.IsTrue(vm.CurrentExpression == expectedExpression, $"current expression = {vm.CurrentExpression}, not {expectedExpression}");
        }

        [Test]
        public void TestContinuouslyTypeNumAndOperator()
        {
            // Arrange
            ViewModel.CalculatorViewModel vm = null;
            CreateTestObject(out vm);
            string[] num1 = {"1", "2", "3", "4"};
            string[] num2 = {"5", "6", "7", "8", "9"};
            string[] num3 = {"9", "0", "8"};
            string[] num4 = {"1", "1", "1"};

            // Act
            ClickNumberButton(vm, num1);
            vm.OperatorBtn.Execute("+");
            System.Threading.Thread.Sleep(this.WAIT_OPT_BTN_TIME);
            ClickNumberButton(vm, num2);
            vm.OperatorBtn.Execute("*");
            System.Threading.Thread.Sleep(this.WAIT_OPT_BTN_TIME);
            ClickNumberButton(vm, num3);
            vm.OperatorBtn.Execute("-");
            System.Threading.Thread.Sleep(this.WAIT_OPT_BTN_TIME);
            ClickNumberButton(vm, num4);
            vm.OperatorBtn.Execute("=");
            System.Threading.Thread.Sleep(this.WAIT_OPT_BTN_TIME);

            // Assert
            int expected = 52684773;
            string expectedExpression = "52684884 - 111 = "; 
            Assert.IsTrue(vm.Value == expected.ToString(), $"this value = {vm.Value}, not {expected}");
            Assert.IsTrue(vm.CurrentExpression == expectedExpression, $"current expression = {vm.CurrentExpression}, not {expectedExpression}");
        }
    }
}