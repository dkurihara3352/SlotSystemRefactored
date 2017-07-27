using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TestSlotSystemElement: SlotSystemElement{
		public string message = "";
		public void SetElementsRecursively(){
			PerformInHierarchy(SetElementsInHi);
		}
			void SetElementsInHi(ISlotSystemElement ele){
				ele.SetElements();
			}
		public void SetParentRecursively(){
			PerformInHierarchy(SetParentInHi);
		}
			void SetParentInHi(ISlotSystemElement ele){
				foreach(var e in ele)
					e.SetParent(ele);
			}
		public void RecursiveTestMethod(){
			PerformInHierarchy(FocusIfAOD);
		}
			void FocusIfAOD(ISlotSystemElement ele){
				if(ele.isActivatedOnDefault)
					ele.Focus();
			}
		public void FocusRecursively(){
			PerformInHierarchy(FocusInHi);
			}
			void FocusInHi(ISlotSystemElement sse){
				if(sse.isFocusableInHierarchy)
					sse.Focus();
				else
					sse.Defocus();
			}
		public void InitializeStatesRecursively(){
			PerformInHierarchy(InitializeStateInHi);
		}
			void InitializeStateInHi(ISlotSystemElement ele){
				ele.InitializeStates();
			}
		public bool isCurSelStateNull{
			get{
				return !(isDeactivated || isFocused || isDefocused || isSelected);
			}
		}
	}
}
