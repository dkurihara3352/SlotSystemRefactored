using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{

	public class SlotSystemManager : AbsSlotSystemElement{
		/*	States	*/
			/*	Selection state	*/
				public override SSEState curSelState{
					get{return (SSMSelState)selStateEngine.curState;}
				}
				public override SSEState prevSelState{
					get{return (SSMSelState)selStateEngine.prevState;}
				}
				public override void SetSelState(SSEState state){
					if(state == null || state is SSMSelState)
						selStateEngine.SetState(state);
					else throw new System.InvalidOperationException("SlotSystemManager.SetSelState: argument is not of type SSMSelState");
				}
				public static SSMSelState ssmDeactivatedState{
					get{
						if(m_ssmDeactivatedState == null)
							m_ssmDeactivatedState = new SSMDeactivatedState();
						return m_ssmDeactivatedState;
					}
					}static SSMSelState m_ssmDeactivatedState;
				public static SSMSelState ssmDefocusedState{
					get{
						if(m_ssmDefocusedState == null)
							m_ssmDefocusedState = new SSMDefocusedState();
						return m_ssmDefocusedState;
					}
					}static SSMSelState m_ssmDefocusedState;
				public static SSMSelState ssmFocusedState{
					get{
						if(m_ssmFocusedState == null)
							m_ssmFocusedState = new SSMFocusedState();
						return m_ssmFocusedState;
					}
					}static SSMSelState m_ssmFocusedState;
				
			/*	Action state	*/
				public override SSEState curActState{
					get{return (SSMSelState)actStateEngine.curState;}
				}
				public override SSEState prevActState{
					get{return (SSMSelState)actStateEngine.prevState;}
				}
				public override void SetActState(SSEState state){
					if(state == null || state is SSMSelState)
						actStateEngine.SetState(state);
					else throw new System.InvalidOperationException("SlotSystemManager.SetActState: argument is not of type SSMSelState");
				}
				public static SSMActState ssmWaitForActionState{
					get{
						if(m_ssmWaitForActionState == null)
							m_ssmWaitForActionState = new SSMWaitForActionState();
						return m_ssmWaitForActionState;
					}
					}static SSMActState m_ssmWaitForActionState;
				public static SSMActState ssmProbingState{
					get{
						if(m_ssmProbingState == null)
							m_ssmProbingState = new SSMProbingState();
						return m_ssmProbingState;
					}
					}static SSMActState m_ssmProbingState;
				public static SSMActState ssmTransactionState{
					get{
						if(m_ssmTransactionState == null)
							m_ssmTransactionState = new SSMTransactionState();
						return m_ssmTransactionState;
					}
					}static SSMActState m_ssmTransactionState;
				

		/*	process	*/
			/*	Selection Process	*/
			/*	Action Process	*/
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
