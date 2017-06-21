using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class InventoryManagerPageMB : AbsSlotSystemElement{
		/*	state	*/
			SSEStateEngine selStateEngine{
				get{
					if(m_selStateEngine == null)
						m_selStateEngine = new SSEStateEngine(this);
					return m_selStateEngine;
				}
				}SSEStateEngine m_selStateEngine;
				public override SSEState curSelState{
					get{return (SSEState)selStateEngine.curState;}
				}
				public override SSEState prevSelState{
					get{return (SSEState)selStateEngine.prevState;}
				}
				public override void SetSelState(SSEState state){
					selStateEngine.SetState(state);
				}
			SSEStateEngine actStateEngine{
				get{
					if(m_actStateEngine == null)
						m_actStateEngine = new SSEStateEngine(this);
					return m_actStateEngine;
				}
				}SSEStateEngine m_actStateEngine;
				public override SSEState curActState{
					get{return (SSEState)actStateEngine.curState;}
				}
				public override SSEState prevActState{
					get{return (SSEState)actStateEngine.prevState;}
				}
				public override void SetActState(SSEState state){
					actStateEngine.SetState(state);
				}
			
			/*	static states */
				public static SSEState deactivatedState{
					get{
						if(m_deactivatedState == null)
							m_deactivatedState = new SSEDeactivatedState();
						return m_deactivatedState;
					}
				}static SSEState m_deactivatedState;
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

		public void Initialize(SlotSystemBundle poolBundle, SlotSystemBundle equipBundle, IEnumerable<SlotSystemBundle> gBundles){
			m_eName = Util.Bold("invManPage");
			this.m_poolBundle = poolBundle;
			this.m_equipBundle = equipBundle;
			m_otherBundles = gBundles;
			PerformInHierarchy(SetRoot);
			PerformInHierarchy(SetParent);
		}
		protected override IEnumerable<SlotSystemElement> elements{
			get{
				yield return poolBundle;
				yield return equipBundle;
				foreach(var ele in otherBundles)
					yield return ele;
			}
		}
		public SlotSystemElement foundParent;
		public SlotSystemElement FindParent(SlotSystemElement ele){
			foundParent = null;
			PerformInHierarchy(CheckAndReportParent, ele);
			return foundParent;
		}
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
		public override SlotSystemElement rootElement{
			get{return this;}
			set{}
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
