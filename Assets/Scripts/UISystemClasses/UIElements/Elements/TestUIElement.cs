using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class TestUIElement: UIElement{
		public string message = "";
		public void SetHierarchyRecursively(){
			PerformInHierarchy(SetHierarchyInHi);
		}
			void SetHierarchyInHi(IUIElement ele){
				ele.SetHierarchy();
			}
		public void RecursiveTestMethod(){
			PerformInHierarchy(FocusIfAOD);
		}
			void FocusIfAOD(IUIElement ele){
				IUISelStateHandler eleSelStateHandler = ele.UISelStateHandler();
				if(ele.IsActivatedOnDefault())
					eleSelStateHandler.MakeSelectable();
			}
		public void InitializeStatesRecursively(){
			PerformInHierarchy(InitializeStateInHi);
		}
			void InitializeStateInHi(IUIElement ele){
				ele.InitializeStates();
			}
	}
}
