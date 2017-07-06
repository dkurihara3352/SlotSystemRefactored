using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SGSelState: SGState{
		public virtual void OnHoverEnterMock(SlotGroup sg, PointerEventDataFake eventDataMock){
			sg.ssm.SetHovered(sg);
		}
		public virtual void OnHoverExitMock(SlotGroup sg, PointerEventDataFake eventDataMock){

		}
	}	
}
