using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlotSystem{
	public class SlotGroupManager : MonoBehaviour {
		string m_UTLog = "";
		public string UTLog{
			get{return m_UTLog;}
			set{m_UTLog = value;}
		}
		/*	transaction
		*/
			SlotSystemTransaction m_transaction;
			public SlotSystemTransaction Transaction{
				get{return m_transaction;}
			}
			public void SetTransaction(SlotSystemTransaction transaction){
				if(m_transaction != transaction){
					m_transaction = transaction;
					if(m_transaction != null){
						m_transaction.Indicate();
					}
				}
			}
		/*	command
		*/
			SGMCommand m_updateTransactionCommand;
				public SGMCommand UpdateTransactionCommand{
					get{return m_updateTransactionCommand;}
				}
				public void SetUpdateTransactionCommand(SGMCommand command){
					m_updateTransactionCommand = command;
				}
				public void UpdateTransaction(){
					m_updateTransactionCommand.Execute(this);
				}
			SGMCommand m_postPickFilterCommand;
				public SGMCommand PostPickFilterCommand{
					get{return m_postPickFilterCommand;}
				}
				public void SetPostPickFilterCommand(SGMCommand comm){
					m_postPickFilterCommand = comm;
				}
				public void PostPickFilter(){
					if(m_postPickFilterCommand != null)
					m_postPickFilterCommand.Execute(this);
				}
			SGMCommand m_prePickFilterCommand;
				public SGMCommand PrePickFilterCommand{
					get{return m_prePickFilterCommand;}
				}
				public void SetPrePickFilterCommand(SGMCommand comm){
					m_prePickFilterCommand = comm;
				}
				public void PrePickFilter(){
					if(m_prePickFilterCommand != null)
					m_prePickFilterCommand.Execute(this);
				}
		/*	process
		*/
			AbsSGMProcess m_probingStateProcess;
			public AbsSGMProcess ProbingStateProcess{
				get{return m_probingStateProcess;}
			}
			IEnumeratorMock ProbingStateCoroutine(){
				return null;
			}
		/*	states
		*/
			SGMState m_curState;
			public SGMState CurState{
				get{
					if(m_curState == null)
						m_curState = SlotGroupManager.DeactivatedState;
					return m_curState;}
			}
			SGMState m_prevState;
			public SGMState PrevState{
				get{
					if(m_prevState == null)
						m_prevState = SlotGroupManager.DeactivatedState;
					return m_prevState;
				}
			}
			static SGMState m_deactivatedState = new SGMDeactivatedState();
			public static SGMState DeactivatedState{
				get{return m_deactivatedState;}
			}
			static SGMState m_defocusedState = new SGMDefocusedState();
			public static SGMState DefocusedState{
				get{return m_defocusedState;}
			}
			static SGMState m_focusedState = new SGMFocusedState();
			public static SGMState FocusedState{
				get{return m_focusedState;}
			}
			
			static SGMState m_probingState = new SGMProbingState();
			public static SGMState ProbingState{
				get{return m_probingState;}
			}
		
		/*	public field
		*/
			List<SlotGroup> m_slotGroups;
			public List<SlotGroup> SlotGroups{
				get{return m_slotGroups;}
			}
			SlotGroup m_initiallyFocusedSG;
			public SlotGroup InitiallyFocusedSG{
				get{return m_initiallyFocusedSG;}
				set{m_initiallyFocusedSG = value;}
			}
			Slottable m_selectedSB;
			public Slottable SelectedSB{
				get{return m_selectedSB;}
			}
			public void SetSelectedSB(Slottable sb){
				if(m_selectedSB != sb){
					m_selectedSB = sb;
					UpdateTransaction();
				}
			}
			SlotGroup m_selectedSG;
			public SlotGroup SelectedSG{
				get{return m_selectedSG;}
			}
			public void SetSelectedSG(SlotGroup sg){
				if(m_selectedSG != sg){
					m_selectedSG = sg;
					UpdateTransaction();
				}
			}
			Slottable m_pickedSB;
			public Slottable PickedSB{
				get{return m_pickedSB;}
			}
		
		public void SetSG(SlotGroup sg){
			if(m_slotGroups == null)
				m_slotGroups = new List<SlotGroup>();
			m_slotGroups.Add(sg);
			sg.SGM = this;
		}
		public void Initialize(){
			this.SetState(SlotGroupManager.DefocusedState);
		}
		public void Focus(){
			this.SetState(SlotGroupManager.FocusedState);
		}
		public void InitializeItems(){
			foreach(SlotGroup sg in SlotGroups){
				sg.InitializeItems();
			}
		}
		
		public void ChangeFocus(SlotGroup sg){
			//first spot the scroller siblings
			List<SlotGroup> scrollerSiblings = new List<SlotGroup>();
			foreach(SlotGroup slotG in m_slotGroups){
				if(slotG.Scroller != null){
					if(slotG.Scroller == sg.Scroller)
						scrollerSiblings.Add(slotG);
				}
			}
			foreach(SlotGroup slotG in scrollerSiblings){
				if(slotG != sg)
					slotG.SetState(SlotGroup.DefocusedState);
				else
					slotG.SetState(SlotGroup.FocusedState);
				slotG.UpdateSbState();
			}
		}

		public SlotGroup GetSlotGroup(Slottable sb){
			foreach(SlotGroup sg in this.SlotGroups){
				foreach(Slot slot in sg.Slots){
					if(slot.Sb != null){
						if(slot.Sb == sb)
							return sg;
					}
				}
			}
			return null;
		}
		public void SetPickedSB(Slottable sb){
			this.m_pickedSB = sb;
		}
		public void SetState(SGMState sgmState){
			if(CurState != sgmState){
				m_prevState = CurState;
				m_prevState.ExitState(this);
				m_curState = sgmState;
				CurState.EnterState(this);
			}
		}
		public void InitializeProcesses(){
			m_probingStateProcess = new SGMProbingStateProcess(this, ProbingStateCoroutine);
		}
		public void SimSBHover(Slottable sb, PointerEventDataMock eventData){
			if(CurState == SlotGroupManager.m_probingState){
				if(sb != null){
					if(SelectedSB != null && SelectedSB != sb){
						SelectedSB.OnHoverExitMock(eventData);
						// SetSelectedSB(sb);
						sb.OnHoverEnterMock(eventData);
					}
				}else{
					if(SelectedSB != null){
						SelectedSB.OnHoverExitMock(eventData);
						// SetSelectedSB(null);
					}
				}
			}
		}
		public void SimSGHover(SlotGroup sg, PointerEventDataMock eventData){
			if(CurState == SlotGroupManager.m_probingState){
				if(sg != null){
					if(SelectedSG != null && SelectedSG != sg){
						SelectedSG.OnHoverExitMock(eventData);
						// SetSelectedSG(sg);
						sg.OnHoverEnterMock(eventData);
					}
				}else{
					if(SelectedSG != null){
						SelectedSG.OnHoverExitMock(eventData);
						// SetSelectedSG(null);
					}
				}
			}
		}
		public void ClearFields(){
			m_selectedSB = null;
			m_selectedSG = null;
			m_pickedSB = null;
		}
		public BowInstanceMock GetEquippedBow(){
			foreach(SlotGroup sg in SlotGroups){
				if(!sg.IsPool && sg.Filter.GetType() == typeof(SGBowFilter))
					return (BowInstanceMock)sg.Slots[0].Sb.Item;
			}
			return null;
		}
		public WearInstanceMock GetEquippedWear(){
			foreach(SlotGroup sg in SlotGroups){
				if(!sg.IsPool && sg.Filter.GetType() == typeof(SGWearFilter))
					return (WearInstanceMock)sg.Slots[0].Sb.Item;
			}
			return null;
		}
		public void CompleteTransactionMock(){
			this.SetTransaction(null);
			this.SetState(SlotGroupManager.FocusedState);
			this.ClearFields();
			PrePickFilter();
		}
	}

}
