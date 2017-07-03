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
