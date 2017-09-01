using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace UISystem{
	public class EquipmentSet : UIElement, IEquipmentSet{
		public IResizableSG bowSG{
			get{
				if(m_bowSG == null)
					throw new System.InvalidOperationException("EquipmentSet.bowSG: not set, assign in the inspector first");
				else
					return m_bowSG;
			}
		}
			IResizableSG m_bowSG;
		public IResizableSG wearSG{
			get{
				if(m_wearSG == null)
					throw new System.InvalidOperationException("EquipmentSet.wearSG: not set, assign in the inspector first");
				else
					return m_wearSG;
			}
		}
			IResizableSG m_wearSG; 
		public IResizableSG cGearsSG{
			get{
				if(m_cGearsSG == null)
					throw new System.InvalidOperationException("EquipmentSet.m_cGearsSG: not set, assign in the inspector first");
				else
					return m_cGearsSG;
			}
		}
			IResizableSG m_cGearsSG;
		public override void SetHierarchy(){
			if(transform.childCount == 3){
				for(int i = 0; i< transform.childCount; i++){
					IResizableSG sg = transform.GetChild(i).GetComponent<IResizableSG>();
					if(sg != null){
						IFilterHandler filterHandler = sg.GetFilterHandler();
						if(filterHandler.GetFilter() is SGBowFilter){
							m_bowSG = sg;
							bowSG.SetParent(this);
						}
						else if(filterHandler.GetFilter() is SGWearFilter){
							m_wearSG = sg;
							wearSG.SetParent(this);
						}
						else if(filterHandler.GetFilter() is SGCGearsFilter){
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
		public void InspectorSetUp(IResizableSG bowSG, IResizableSG wearSG, IResizableSG cGearsSG){
			m_bowSG = bowSG; m_wearSG = wearSG; m_cGearsSG = cGearsSG;
		}
		protected override IEnumerable<IUIElement> elements{
			get{
				yield return m_bowSG;
				yield return m_wearSG;
				yield return m_cGearsSG;
			}
		}
	}
	public interface IEquipmentSet: IUIElement{
	}
}
