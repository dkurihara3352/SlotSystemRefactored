using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SBClone: Slottable{
		public override SlotSystemElement parent{
			get{return m_parent;}
		}SlotSystemElement m_parent;
		public void Initialize(Slottable orig){
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
