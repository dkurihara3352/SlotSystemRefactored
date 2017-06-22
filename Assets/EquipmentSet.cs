using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class EquipmentSet : AbsSlotSystemElement{
		SlotGroup m_bowSG;
		SlotGroup m_wearSG;
		SlotGroup m_cGearsSG;
		public void Initialize(SlotGroup bowSG, SlotGroup wearSG, SlotGroup cGearsSG){
			m_eName = Util.Bold("eSet");
			m_bowSG = bowSG;
			m_wearSG = wearSG;
			m_cGearsSG = cGearsSG;
			base.Initialize();
		}
		protected override IEnumerable<SlotSystemElement> elements{
			get{
				yield return m_bowSG;
				yield return m_wearSG;
				yield return m_cGearsSG;
			}
		}
	}
}
