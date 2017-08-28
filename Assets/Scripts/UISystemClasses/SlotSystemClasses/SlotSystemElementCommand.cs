using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface SBEventArgsCommand: ISlotSystemElementCommand{
		void Execute(ISlottable pickedSB);
	}
	public interface SGEventArgsCommand: ISlotSystemElementCommand{
		void Execute(ISlotGroup hoveredSG);
	}
	public interface SlotEventArgsCommand: ISlotSystemElementCommand{
		void Execute(ISlot slot);
	}
	public interface ISlotSystemElementCommand{
	}
}
