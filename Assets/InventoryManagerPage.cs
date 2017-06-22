using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class InventoryManagerPage : AbsSlotSystemElement{
		/* public fields	*/
			public override SlotSystemBundle immediateBundle{
				get{return null;}
			}
			public SlotSystemBundle poolBundle{
				get{return m_poolBundle;}
				}SlotSystemBundle m_poolBundle;
			public SlotSystemBundle equipBundle{
				get{return m_equipBundle;}
				}SlotSystemBundle m_equipBundle;
			public IEnumerable<SlotSystemBundle> otherBundles{
				get{
					if(m_otherBundles == null)
						m_otherBundles = new SlotSystemBundle[]{};
					return m_otherBundles;}
				}IEnumerable<SlotSystemBundle> m_otherBundles;


			protected override IEnumerable<SlotSystemElement> elements{
				get{
					yield return poolBundle;
					yield return equipBundle;
					foreach(var ele in otherBundles)
						yield return ele;
				}
			}
			public override SlotSystemElement rootElement{
				get{return this;}
				set{}
			}
		/*	methods	*/
			public void Initialize(SlotSystemBundle poolBundle, SlotSystemBundle equipBundle, IEnumerable<SlotSystemBundle> gBundles){
				m_eName = Util.Bold("invManPage");
				this.m_poolBundle = poolBundle;
				this.m_equipBundle = equipBundle;
				m_otherBundles = gBundles;
				PerformInHierarchy(SetRoot);
				PerformInHierarchy(SetParent);
				base.Initialize();
			}
			public SlotSystemElement FindParent(SlotSystemElement ele){
				foundParent = null;
				PerformInHierarchy(CheckAndReportParent, ele);
				return foundParent;
				}public SlotSystemElement foundParent;
			void CheckAndReportParent(SlotSystemElement ele, object obj){
				if(!(ele is Slottable)){
					SlotSystemElement tarEle = (SlotSystemElement)obj;
					foreach(SlotSystemElement e in ele){
						if(e == tarEle)
							this.foundParent = ele;
					}
				}
			}
			void SetRoot(SlotSystemElement ele){
				ele.rootElement = this;
			}
			void SetParent(SlotSystemElement ele){
				if(!((ele is Slottable) || (ele is SlotGroup)))
				foreach(SlotSystemElement e in ele){
					if(e != null)
					e.parent = ele;
				}
				// if(ele != this)
				// ele.parent = this.FindParent(ele);
			}
			public void SetSGMRecursively(SlotGroupManager sgm){
				this.sgm = sgm;
				PerformInHierarchy(SetSGM);
			}
			public void SetSGM(SlotSystemElement ele){
				if(ele != this)
				ele.sgm = this.sgm;
			}
	}
}
