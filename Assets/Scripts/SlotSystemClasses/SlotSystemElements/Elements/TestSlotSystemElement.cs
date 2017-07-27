using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TestSlotSystemElement: SlotSystemElement{
		public string message = "";
		public void SetElementsInHi(ISlotSystemElement ele){
			ele.SetElements();
		}
		public void SetParentInHi(ISlotSystemElement ele){
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
	}
}
