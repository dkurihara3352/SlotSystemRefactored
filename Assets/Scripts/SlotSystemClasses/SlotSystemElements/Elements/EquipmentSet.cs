using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SlotSystem{
	public class EquipmentSet : SlotSystemElement, IEquipmentSet{
		public ISlotGroup bowSG{
			get{
				if(m_bowSG == null)
					throw new System.InvalidOperationException("EquipmentSet.bowSG: not set, assign in the inspector first");
				else
					return m_bowSG;
			}
		}
			ISlotGroup m_bowSG;
		public ISlotGroup wearSG{
			get{
				if(m_wearSG == null)
					throw new System.InvalidOperationException("EquipmentSet.wearSG: not set, assign in the inspector first");
				else
					return m_wearSG;
			}
		}
			ISlotGroup m_wearSG; 
		public ISlotGroup cGearsSG{
			get{
				if(m_cGearsSG == null)
					throw new System.InvalidOperationException("EquipmentSet.m_cGearsSG: not set, assign in the inspector first");
				else
					return m_cGearsSG;
			}
		}
			ISlotGroup m_cGearsSG;
		public override void SetHierarchy(){
			if(transform.childCount == 3){
				for(int i = 0; i< transform.childCount; i++){
					ISlotGroup sg = transform.GetChild(i).GetComponent<ISlotGroup>();
					if(sg != null){
						if(sg.filter is SGBowFilter){
							m_bowSG = sg;
							bowSG.SetParent(this);
						}
						else if(sg.filter is SGWearFilter){
							m_wearSG = sg;
							wearSG.SetParent(this);
						}
						else if(sg.filter is SGCGearsFilter){
							m_cGearsSG = sg;
							cGearsSG.SetParent(this);
						}
					}else 
						throw new InvalidOperationException("some childrent does not have SG");
				}
				if(bowSG != null && wearSG != null && cGearsSG != null)
					return;
			}else
				throw new InvalidOperationException("transform children' count is not exactly 3");
		}
		public void InspectorSetUp(ISlotGroup bowSG, ISlotGroup wearSG, ISlotGroup cGearsSG){
			m_bowSG = bowSG; m_wearSG = wearSG; m_cGearsSG = cGearsSG;
		}
		protected override IEnumerable<ISlotSystemElement> elements{
			get{
				yield return m_bowSG;
				yield return m_wearSG;
				yield return m_cGearsSG;
			}
		}
	}
	public interface IEquipmentSet: ISlotSystemElement{
	}
}
