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
		calc.Received().Add(1, 2);
		// calc.DidNotReceive().Add(1, 2);
		calc.Mode.Returns("SomeMode");
		Assert.That(calc.Mode, Is.EqualTo("SomeMode"));
		calc.Mode = "SomeOtherMode";
		Assert.That(calc.Mode, Is.EqualTo("SomeOtherMode"));
		calc.Add(Arg.Any<int>(), Arg.Any<int>()).Returns(10);
		Assert.That(calc.Add(1, 222), Is.EqualTo(10));
		calc.
			When(x => x.Add(Arg.Is<int>(y => y < 0), Arg.Any<int>())).
			Do(context => {throw new System.Exception("fake exception");});
		Assert.Throws<System.Exception>(() => calc.Add(-2, 2));
	}
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
[TestFixture]
public class LogAnalyzer2Tests{
	[Test]
	public void Analyze_LoggerThrows_WriteToWebService(){
		FakeLogger2 stubLogger = new FakeLogger2();
		stubLogger.thrown = new System.Exception("fake exception");
		FakeWebService mockWebService = new FakeWebService();
		LogAnalyzer2 logAn = new LogAnalyzer2(stubLogger, mockWebService);
		logAn.minNameLength = 8;

		string tooShortFileName = "abs.txt";
		logAn.Analyze(tooShortFileName);

		Assert.That(mockWebService.messageToWebService, Is.StringContaining("fake exception"));
	}
	[Test]
	public void Analyze_LoggerThrows_WriteToWebService_NSubVer(){
		ILogger stubLogger = Substitute.For<ILogger>();
		stubLogger.
			When(x => x.LogError(Arg.Any<string>())).
			Do(info => {throw new System.Exception("fake exception");});
		IWebService mockWebService = Substitute.For<IWebService>();
		LogAnalyzer2 logAn = new LogAnalyzer2(stubLogger, mockWebService);
		logAn.minNameLength = 8;

		string tooShortFileName = "abs.txt";
		logAn.Analyze(tooShortFileName);

		mockWebService.Received().Write(Arg.Is<string>(s => s.Contains("fake exception")));	
	}
	
}
[TestFixture]
public class EventsRelatedTests{
	[Test]
	public void ctor_WhenViewIsLoaded_RenderView(){
		IView mockView = Substitute.For<IView>();
		Presenter presenter = new Presenter(mockView);
		mockView.Loaded += Raise.Event<System.Action>();
		mockView.Received().Render(Arg.Is<string>(s => s.Contains("Hello World")));
	}
	[Test]
	public void ctor_WhenViewHasError_CallsLogger(){
		IView stubView = Substitute.For<IView>();
		ILogger mockLogger = Substitute.For<ILogger>();
		Presenter presenter = new Presenter(stubView, mockLogger);
		stubView.ErrorOccurred += Raise.Event<System.Action<string>>("error occurred");
		mockLogger.Received().LogError(Arg.Is<string>(s => s.Contains("error occurred")));
	}
	[Test]
	public void TestIfEventIsFiredManually(){
		SomeView someView = new SomeView();
		bool isCalled = false;
		someView.Loaded += () => { isCalled = true;};
		someView.LoadedTrigger();
		Assert.That(isCalled, Is.True);
	}
	//adde still another commnet
		//added comments in feature branch a lot
		//added comments in feature branch a lot
		//added comments in feature branch a lot
		//added comments in feature branch a lot
		//added comments in feature branch a lot
		//added comments in feature branch a lot
		//added comments in feature branch a lot
		//added comments in feature branch a lot
		//added comments in feature branch a lot
		//added comments in feature branch a lot
		//added comments in feature branch a lot
		//added comments in feature branch a lot

		//adding some more comment
		//adding some more comment
		//adding some more comment
		//adding some more comment
		//adding some more comment
		//adding some more comment
}