using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogAnalyzer{
	IExtensionManager extManager;
	public LogAnalyzer(IExtensionManager mgr){
		extManager = mgr;
	}
	public LogAnalyzer(){
	}
	public bool wasLastFileNameValid{get; set;}
	public bool IsValidLogFileName(string fileName){
		wasLastFileNameValid = false;
		// IExtensionManager extensionManager = new FileExtensionManager();
		bool result = false;
		if(extManager != null)
			result = extManager.isValid(fileName);
		else
			result = new FileExtensionManager().isValid(fileName);
		if(result == true)
			wasLastFileNameValid = true;
		return result;
	}
}
public class FileExtensionManager: IExtensionManager{
	public bool isValid(string filename){
		if(string.IsNullOrEmpty(filename)){
			throw new System.ArgumentException("LogAnalyzer.IsValidLogFileName: filename has to be provided");
		}
		if(!filename.EndsWith(".SLF", System.StringComparison.CurrentCultureIgnoreCase)){
			return false;
		}
		return true;
	}
}
public class AlwaysValidFakeExtensionManager: IExtensionManager{
	public bool isValid(string filename){
		return true;
	}
}
public interface IExtensionManager{
	bool isValid(string filename);
}
public interface ICalculator{
    int Add(int a, int b);
    string Mode { get; set; }
    event System.EventHandler PoweringUp;
}
public class MemCalculator{
	public int sum = 0;
	public void Add(int num){
		sum += num;
	}
	public int Sum(){
		int res = sum;
		return res;
	}
}
public class LogAnalyzer2{
	private ILogger _logger;
	private IWebService _webService;
	public LogAnalyzer2(ILogger logger, IWebService webService){
		_logger = logger;
		_webService = webService;
	}
	public int minNameLength{get; set;}
	public void Analyze(string filename){
		if(filename.Length < minNameLength){
			try{
				_logger.LogError(string.Format("filename too short: {0}", filename));
			}catch(System.Exception e){
				_webService.Write("error from logger: " + e);
			}
		}
	}
}
public interface ILogger{
	void LogError(string message);
}
public class FakeLogger2: ILogger{
	public System.Exception thrown = null;
	public string loggerMessage = null;
	public void LogError(string message){
		loggerMessage = message;
		if(thrown != null) throw thrown;
	}
}
public interface IWebService{
	void Write(string message);
}
public class FakeWebService: IWebService{
	public string messageToWebService;
	public void Write(string message){
		messageToWebService = message;
	}
}
public class Presenter{
	IView _view;
	ILogger _logger;
	public Presenter(IView view){
		_view = view;
		_view.Loaded += OnLoaded;
	}
	public Presenter(IView view, ILogger logger){
		_view = view;
		_logger = logger;
		_view.ErrorOccurred += _logger.LogError;
		
	}
	void OnLoaded(){
		_view.Render("Hello World");
	}
}
public interface IView{
	event System.Action Loaded;
	event System.Action<string> ErrorOccurred;
	void Render(string text);
}
public class SomeView: IView{
	public event System.Action Loaded;
	public event System.Action<string> ErrorOccurred;
	public void Render(string text){

	}
	public void LoadedTrigger(){
		Loaded();
	}
}
