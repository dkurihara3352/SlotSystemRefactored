using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class EquipmentSet : SlotSystemElement, IEquipmentSet{
		public ISlotGroup bowSG{get{return m_bowSG;}}
		ISlotGroup m_bowSG;
		public ISlotGroup wearSG{get{return m_wearSG;}}
		ISlotGroup m_wearSG;
		public ISlotGroup cGearsSG{get{return m_cGearsSG;}}
		ISlotGroup m_cGearsSG;
		public override void SetElements(){}
		public void InspectorSetUp(ISlotGroup bowSG, ISlotGroup wearSG, ISlotGroup cGearsSG){
			m_bowSG = bowSG; m_wearSG = wearSG; m_cGearsSG = cGearsSG;
		}
		public override IEnumerable<ISlotSystemElement> elements{
			get{
				yield return m_bowSG;
				yield return m_wearSG;
				yield return m_cGearsSG;
			}
		}
	}
	public interface IEquipmentSet: ISlotSystemElement{
		ISlotGroup bowSG{get;}
		ISlotGroup wearSG{get;}
		ISlotGroup cGearsSG{get;}
	}
}
