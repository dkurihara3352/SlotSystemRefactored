using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{

	public class SlotSystemManager : AbsSlotSystemElement, TransactionManager{
		public static SlotSystemManager curSSM;
			void SetCurSSM(){
				if(curSSM != null){
					if(curSSM != this){
						curSSM.Defocus();
						curSSM = this;
					}else{
						// no change
					}
				}else{
					curSSM = this;
				}
			}
		/*	SlotSystemElement	*/
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
					public override SSEProcess selProcess{
						get{return (SSMSelProcess)selProcEngine.process;}
					}
					public override void SetAndRunSelProcess(SSEProcess process){
						if(process == null||process is SSMSelProcess)
							selProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("SlotSystemManager.SetAndRunSelProcess: argument is not of type SSMSelProcess");
					}
					public override IEnumeratorMock greyoutCoroutine(){
						return null;
					}
					public override IEnumeratorMock greyinCoroutine(){
						return null;
					}
				/*	Action Process	*/
					public override SSEProcess actProcess{
							get{return (SSMActProcess)actProcEngine.process;}
						}
						public override void SetAndRunActProcess(SSEProcess process){
							if(process == null||process is SSMActProcess)
								actProcEngine.SetAndRunProcess(process);
							else throw new System.InvalidOperationException("SlotSystemManager.SetAndRunActProcess: argument is not of type SSMActProcess");
						}
					public IEnumeratorMock probeCoroutine(){
						return null;
					}
					public IEnumeratorMock transactionCoroutine(){
						bool done = true;
						done &= m_dIcon1Done;
						done &= m_dIcon2Done;
						done &= m_sg1Done;
						done &= m_sg2Done;
						if(done){
							this.actProcess.Expire();
						}
						return null;
					}
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
		/*	Transaction Manager	*/
			public SlotSystemTransaction Transaction{
				get{return m_transaction;}
				}SlotSystemTransaction m_transaction;
				public void SetTransaction(SlotSystemTransaction transaction){
					if(m_transaction != transaction){
						m_transaction = transaction;
						if(m_transaction != null){
							m_transaction.Indicate();
						}
					}
				}
			public void AcceptSGTAComp(SlotGroup sg){
				if(sg2 != null && sg == sg2) m_sg2Done = true;
				else if(sg1 != null && sg == sg1) m_sg1Done = true;
				if(curActState == SlotSystemManager.ssmTransactionState){
					// IEnumeratorMock tryInvoke = ((SSMActProcess)actProcess).coroutineMock();
					transactionCoroutine();
				}
			}
			public void AcceptDITAComp(DraggedIcon di){
				if(dIcon2 != null && di == dIcon2) m_dIcon2Done = true;
				else if(dIcon1 != null && di == dIcon1) m_dIcon1Done = true;
				if(curActState == SlotSystemManager.ssmTransactionState){
					// IEnumeratorMock tryInvoke = ((SSMActProcess)actProcess).coroutineMock();
					transactionCoroutine();
				}
			}
			public Slottable pickedSB{
				get{return m_pickedSB;}
				}Slottable m_pickedSB;
				public void SetPickedSB(Slottable sb){
					this.m_pickedSB = sb;
				}
			public Slottable targetSB{
				get{return m_targetSB;}
				}Slottable m_targetSB;
				public void SetTargetSB(Slottable sb){
					if(sb == null || sb != targetSB){
						if(targetSB != null)
							targetSB.SetSelState(Slottable.sbFocusedState);
					}
					this.m_targetSB = sb;
					if(targetSB != null)
						targetSB.SetSelState(Slottable.sbSelectedState);
				}
			public SlotGroup sg1{
				get{return m_sg1;}
				}SlotGroup m_sg1;
				public void SetSG1(SlotGroup sg){
					if(sg == null || sg != sg1){
						if(sg1 != null)
							sg1.SetSelState(SlotGroup.sgFocusedState);
					}
					this.m_sg1 = sg;
					if(sg1 != null)
						m_sg1Done = false;
					else
						m_sg1Done = true;
				}
				public bool sg1Done{
				get{return m_sg1Done;}
				}bool m_sg1Done = true;
			public SlotGroup sg2{
				get{return m_sg2;}
				}SlotGroup m_sg2;
				public void SetSG2(SlotGroup sg){
					if(sg == null || sg != sg2){
						if(sg2 != null)
							sg2.SetSelState(SlotGroup.sgFocusedState);
					}
					this.m_sg2 = sg;
					if(sg2 != null)
						sg2.SetSelState(SlotGroup.sgSelectedState);
					if(sg2 != null)
						m_sg2Done = false;
					else
						m_sg2Done = true;
				}
				public bool sg2Done{
					get{return m_sg2Done;}
					}bool m_sg2Done = true;
			public DraggedIcon dIcon1{
				get{return m_dIcon1;}
				}DraggedIcon m_dIcon1;
				public void SetDIcon1(DraggedIcon di){
					m_dIcon1 = di;
					if(m_dIcon1 == null)
						m_dIcon1Done = true;
					else
						m_dIcon1Done = false;
				}
				public bool dIcon1Done{
				get{return m_dIcon1Done;}
				}bool m_dIcon1Done = true;
			public DraggedIcon dIcon2{
				get{return m_dIcon2;}
				}DraggedIcon m_dIcon2;
				public void SetDIcon2(DraggedIcon di){
					m_dIcon2 = di;
					if(m_dIcon2 == null)
						m_dIcon2Done = true;
					else
						m_dIcon2Done = false;
				}
				public bool dIcon2Done{
				get{return m_dIcon2Done;}
				}bool m_dIcon2Done = true;
			public Slottable hoveredSB{
				get{return m_hoveredSB;}
				}Slottable m_hoveredSB;
				public void SetHoveredSB(Slottable sb){
					if(sb == null || sb != hoveredSB){
						if(hoveredSB != null)
							hoveredSB.OnHoverExitMock();
					}
					m_hoveredSB = sb;
				}
			public SlotGroup hoveredSG{
				get{return m_hoveredSG;}
				}SlotGroup m_hoveredSG;
				public void SetHoveredSG(SlotGroup sg){
					if(sg == null || sg != hoveredSG){
						if(hoveredSG != null)
							hoveredSG.OnHoverExitMock();
					}
					m_hoveredSG = sg;
				}
	}
}
