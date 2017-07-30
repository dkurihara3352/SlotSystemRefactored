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
	public interface ISSECommand{
		void Execute();
	}
}

