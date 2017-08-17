using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TestSlotSystemElement: SlotSystemElement{
		public string message = "";
		public void SetHierarchyRecursively(){
			PerformInHierarchy(SetHierarchyInHi);
		}
			void SetHierarchyInHi(ISlotSystemElement ele){
				ele.SetHierarchy();
			}
		public void RecursiveTestMethod(){
			PerformInHierarchy(FocusIfAOD);
		}
			void FocusIfAOD(ISlotSystemElement ele){
				ISSESelStateHandler eleSelStateHandler = ele.GetSelStateHandler();
				if(ele.IsActivatedOnDefault())
					eleSelStateHandler.Focus();
			}
		public void InitializeStatesRecursively(){
			PerformInHierarchy(InitializeStateInHi);
		}
			void InitializeStateInHi(ISlotSystemElement ele){
				ele.InitializeStates();
			}
	}
}
