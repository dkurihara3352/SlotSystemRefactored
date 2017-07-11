using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SBClone: Slottable{
		public override ISlotSystemElement parent{
			get{return m_parent;}
		}ISlotSystemElement m_parent;
		public void Initialize(ISlottable orig){
			this.delayed = orig.delayed;
			this.SetItem(orig.itemInst);
			m_parent = orig.sg;
			SetSSM(orig.ssm);
			SetSelState(orig.prevSelState);
			SetSelState(orig.curSelState);
			SetActState(orig.prevActState);
			SetActState(orig.curActState);
			SetEqpState(orig.prevEqpState);
			SetEqpState(orig.curEqpState);
			SetMrkState(orig.prevMrkState);
			SetMrkState(orig.curMrkState);
			SetSlotID(orig.slotID);
			SetNewSlotID(orig.newSlotID);
		}
	}
}
