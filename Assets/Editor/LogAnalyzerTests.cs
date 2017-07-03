using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;

[TestFixture]
public class LogAnalyzerTests{
	[TestCase("someInvalidFileName.foo", false)]
	[TestCase("someInvalidFileName.SLF", true)]
	[TestCase("someInvalidFileName.slf", true)]
	public void IsValidLogFileName_VariousExtension_RetursExpected(string fileName, bool expected){
		LogAnalyzer newLogAn = MakeLogAnalyzer();

		bool result = newLogAn.IsValidLogFileName(fileName);

		Assert.That(result, Is.EqualTo(expected));
	}

	[Test]
	[Category("SomeCategory")]
	public void IsValidLogFileName_EmptyArgument_ThrowsException(){
		LogAnalyzer la = MakeLogAnalyzer();
		var ex = Assert.Catch<System.Exception>(() => {la.IsValidLogFileName("");});
		StringAssert.Contains("LogAnalyzer.IsValidLogFileName: filename has to be provided", ex.Message);
	}
	[Test]
	[Ignore("this is just an ignore example")]
	public void SomeIgnoredTest(){

	}
	public LogAnalyzer MakeLogAnalyzer(){
		return new LogAnalyzer();
	}
	[TestCase("invalidExtension.foo", false)]
	[TestCase("validExtension.slf", true)]
	public void IsValidLogFileName_WhenCalled_ChangeWasLastFileNameValidTo(string filename, bool expected){
		LogAnalyzer la = MakeLogAnalyzer();
		la.IsValidLogFileName(filename);
		Assert.That(la.wasLastFileNameValid, Is.EqualTo(expected));
	}
	[Test]
	public void InValidExtension_NameSupportedExtension_ReturnsTrue(){
		FakeExtensionManager fakeExtensionManager = new FakeExtensionManager();
		fakeExtensionManager.willBeValid = true;
		LogAnalyzer logAn = new LogAnalyzer(fakeExtensionManager);
		bool result = logAn.IsValidLogFileName("whatever");
		Assert.That(result, Is.True);
	}
	internal class FakeExtensionManager: IExtensionManager{
		public bool willBeValid;
		public bool isValid(string filename){
			return willBeValid;
		}
	}
}
[TestFixture]
public class ICalculatorTests{
	[Test]
	public void Add_Always_ReturnsSumValue(){
		ICalculator calc = Substitute.For<ICalculator>();
		calc.Add(1, 2).Returns(3);
		Assert.That(calc.Add(1, 2), Is.EqualTo(3));
	}
}
public interface ICalculator
{
    int Add(int a, int b);
    string Mode { get; set; }
    event System.EventHandler PoweringUp;
}
[TestFixture]
public class MemCalculatorTests{
	[Test]
	public void Sum_ByDefault_ReturnsZero(){
		MemCalculator memCalc = MakeMemCalculator();
		int sum = memCalc.Sum();
		Assert.That(sum, Is.EqualTo(0));
	}
	[Test]
	public void Add_WhenCalled_ChangesSum(){
		MemCalculator memCalc = MakeMemCalculator();
		memCalc.Add(1);
		Assert.That(memCalc.Sum(), Is.EqualTo(1));
	}
	public MemCalculator MakeMemCalculator(){
		return new MemCalculator();
	}
}