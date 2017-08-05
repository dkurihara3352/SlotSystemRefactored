using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class EmptyTransaction: AbsSlotSystemTransaction{
		public EmptyTransaction(ITransactionManager tam, ITAMActStateHandler handler): base(tam, handler){
		}
	}
}
