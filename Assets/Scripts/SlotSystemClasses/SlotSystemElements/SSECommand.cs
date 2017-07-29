using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class InstantFocusCommand: ISSECommand{
		public void Execute(){}
	}
	public class InstantDefocusCommand: ISSECommand{
		public void Execute(){}
	}
	public class InstantSelectCommand: ISSECommand{
		public void Execute(){}
	}
	public class OnHoverEnterCommand: ISSECommand{
		ISlotSystemElement sse;
		public OnHoverEnterCommand(ISlotSystemElement sse){this.sse = sse;}
		public void Execute(){
			if(!sse.isCurSelStateNull)
					sse.curSelState.OnHoverEnter(sse, new PointerEventDataFake());
				else
					throw new System.InvalidOperationException("SlotSystemElement.OnHoverEnter: curSelState not set");
		}
	}
	public class OnHoverExitCommand: ISSECommand{
		ISlotSystemElement sse;
		public OnHoverExitCommand(ISlotSystemElement sse){this.sse = sse;}
		public void Execute(){
			if(!sse.isCurSelStateNull)
					sse.curSelState.OnHoverExit(sse, new PointerEventDataFake());
				else
					throw new System.InvalidOperationException("SlotSystemElement.OnHoverExit: curSelState not set");
		}
	}
	public interface ISSECommand{
		void Execute();
	}
}

