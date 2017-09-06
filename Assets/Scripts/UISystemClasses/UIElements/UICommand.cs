using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface ITapCommand{
		void SetUIElement(IUIElement uiElement);
		void Execute();
	}
	public class TapCommand: ITapCommand{
		public void SetUIElement(IUIElement element){
			uiElement = element;
		}
		IUIElement uiElement;
		public void Execute(){}
	}
}

