using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SBSelState: SBState{
		public virtual void OnHoverEnterMock(Slottable sb, PointerEventDataFake eventDataMock){
			sb.ssm.SetHovered(sb);
		}
		public virtual void OnHoverExitMock(Slottable sb, PointerEventDataFake eventDataMock){
		}
}
	}
