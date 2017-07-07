using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SBActState: SBState{
		public abstract void OnPointerDownMock(Slottable sb, PointerEventDataFake eventDataMock);
		public abstract void OnPointerUpMock(Slottable sb, PointerEventDataFake eventDataMock);
		public abstract void OnDeselectedMock(Slottable sb, PointerEventDataFake eventDataMock);
		public abstract void OnEndDragMock(Slottable sb, PointerEventDataFake eventDataMock);
	}
}
