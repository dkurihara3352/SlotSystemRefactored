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
		public override void SetElements(){
			if(transform.childCount == 3){
				List<ISlotSystemPageElement> pEles = new List<ISlotSystemPageElement>();
				for(int i = 0; i < transform.childCount; i++){
					ISlotSystemElement ele = transform.GetChild(i).GetComponent<ISlotSystemElement>();
					if(ele != null){
						if(ele is ISlotGroup){
							ISlotGroup sg = (ISlotGroup)ele;
								if(sg.filter is SGBowFilter){
									m_bowSG = sg;
									SlotSystemPageElement bowPE = new SlotSystemPageElement(sg, sg.isToggledOnInPageByDefault);
									pEles.Add(bowPE);
								}else if(sg.filter is SGWearFilter){
									m_wearSG = sg;
									SlotSystemPageElement wearPE = new SlotSystemPageElement(sg, sg.isToggledOnInPageByDefault);
									pEles.Add(wearPE);
								}else if(sg.filter is SGCGearsFilter){
									m_cGearsSG = sg;
									SlotSystemPageElement cGearsPE = new SlotSystemPageElement(sg, sg.isToggledOnInPageByDefault);
									pEles.Add(cGearsPE);
								}else throw new System.InvalidOperationException("EquipmentSet.InitializeMB: sg's filter is not set properly");

						}else throw new System.InvalidOperationException("EquipmentSet.InitializeMB: one of transform children's ISlotSystemElement is not of type ISlotGroup.");	
						
					}else throw new System.InvalidOperationException("EquipmentSet.InitializeMB: one of transform children does not have ISlotSystemElement component assigned.");	
				}
				if(!(bowSG ==null || wearSG == null || cGearsSG == null)){
					m_pageElements = pEles;
				}else throw new System.InvalidOperationException("EquipmentSet.InitializeMB: some SG is not set properly. Make sure to have one bowSG, one wearSG, and one cGesrsSG in the set's children");
			}
			else throw new System.InvalidOperationException("EquipmentSet.InitializeMB: transform.child's count is not 3. Make sure to have bowSG, wearSG, and CGearsSG in children");
			
		}
		public void Initialize(ISlotSystemPageElement bowSGPE, ISlotSystemPageElement wearSGPE, ISlotSystemPageElement cGearsSGPE){
			m_eName = SlotSystemUtil.Bold("eSet");
			m_bowSG = (ISlotGroup)bowSGPE.element;
			m_wearSG = (ISlotGroup)wearSGPE.element;
			m_cGearsSG = (ISlotGroup)cGearsSGPE.element;
			IEnumerable<ISlotSystemPageElement> pageEles = new ISlotSystemPageElement[]{
				bowSGPE, wearSGPE, cGearsSGPE
			};
			m_pageElements = pageEles;
			InitializeStates();
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
