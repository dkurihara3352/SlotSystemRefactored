using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class EquipmentSet : SlotSystemElement, IEquipmentSet{
		public ISlotGroup bowSG{
			get{
				if(m_bowSG == null)
					throw new System.InvalidOperationException("EquipmentSet.bowSG: not set, assign in the inspector first");
				else
					return m_bowSG;
			}
			} ISlotGroup m_bowSG;
		public ISlotGroup wearSG{
			get{
				if(m_wearSG == null)
					throw new System.InvalidOperationException("EquipmentSet.wearSG: not set, assign in the inspector first");
				else
					return m_wearSG;
			}
			} ISlotGroup m_wearSG; 
		public ISlotGroup cGearsSG{
			get{
				if(m_cGearsSG == null)
					throw new System.InvalidOperationException("EquipmentSet.m_cGearsSG: not set, assign in the inspector first");
				else
					return m_cGearsSG;
			}
			} ISlotGroup m_cGearsSG;
		public override void SetHierarchy(){}
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
