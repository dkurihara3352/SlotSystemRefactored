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
	}
}
