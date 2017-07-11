using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class EquipmentSet : SlotSystemPage, IEquipmentSet{
		public ISlotGroup bowSG{get{return m_bowSG;}}
		ISlotGroup m_bowSG;
		public ISlotGroup wearSG{get{return m_wearSG;}}
		ISlotGroup m_wearSG;
		public ISlotGroup cGearsSG{get{return m_cGearsSG;}}
		ISlotGroup m_cGearsSG;
		public void Initialize(ISlotSystemPageElement bowSGPE, ISlotSystemPageElement wearSGPE, ISlotSystemPageElement cGearsSGPE){
			m_eName = SlotSystemUtil.Bold("eSet");
			m_bowSG = (ISlotGroup)bowSGPE.element;
			m_wearSG = (ISlotGroup)wearSGPE.element;
			m_cGearsSG = (ISlotGroup)cGearsSGPE.element;
			IEnumerable<ISlotSystemPageElement> pageEles = new ISlotSystemPageElement[]{
				bowSGPE, wearSGPE, cGearsSGPE
			};
			m_pageElements = pageEles;
			base.Initialize();
		}
		public override IEnumerable<ISlotSystemElement> elements{
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
	public interface IEquipmentSet: ISlotSystemPage{
		ISlotGroup bowSG{get;}
		ISlotGroup wearSG{get;}
		ISlotGroup cGearsSG{get;}
		void Initialize(ISlotSystemPageElement bowSGPE, ISlotSystemPageElement wearSGPE, ISlotSystemPageElement cGearsSGPE);
	}
}
