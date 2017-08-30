using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class WidgetUIRoot : UIElement, IWidgetUIRoot {
		public WidgetUIRoot(RectTransformFake rectTrans): base(rectTrans){}
		public void OnWidgetSelected(object uiManager, IWidgetUIRoot selectedRoot){
			if(selectedRoot == this)
				Activate();
			else
				Deactivate();
		}
	}
	public interface IWidgetUIRoot: IUIElement{
		void OnWidgetSelected(object uiManager, IWidgetUIRoot selectedRoot);
	}
}
