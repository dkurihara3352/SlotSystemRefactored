using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class EquipmentSet : SlotSystemPage{
		SlotGroup m_bowSG;
		SlotGroup m_wearSG;
		SlotGroup m_cGearsSG;
		public void Initialize(SlotSystemPageElement bowSGPE, SlotSystemPageElement wearSGPE, SlotSystemPageElement cGearsSGPE){
			m_eName = Util.Bold("eSet");
			m_bowSG = (SlotGroup)bowSGPE.element;
			m_wearSG = (SlotGroup)wearSGPE.element;
			m_cGearsSG = (SlotGroup)cGearsSGPE.element;
			IEnumerable<SlotSystemPageElement> pageEles = new SlotSystemPageElement[]{
				bowSGPE, wearSGPE, cGearsSGPE
			};
			m_pageElements = pageEles;
			base.Initialize();
		}
		protected override IEnumerable<SlotSystemElement> elements{
			get{
				yield return m_bowSG;
				yield return m_wearSG;
				yield return m_cGearsSG;
			}
		}
		public override void Focus(){
			SetSelState(AbsSlotSystemElement.focusedState);
			PageFocus();
		}
		public override void Deactivate(){
			base.Deactivate();
			ToggleBack();
		}
	}
}
