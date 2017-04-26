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
		/*	not used
		*/
			// Use this for initialization
			void Start () {
				
			}
			
			// Update is called once per frame
			void Update () {
				
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
				set{
					if(m_selectedSB != value){
						m_selectedSB = value;
						// UpdateTransaction();
					}
				}
			}
			SlotGroup m_selectedSG;
			public SlotGroup SelectedSG{
				get{return m_selectedSG;}
				set{
					if(m_selectedSG != value){
						m_selectedSG = value;
						// UpdateTransaction();
					}
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
					if(SelectedSB != null && SelectedSB != sb)
						SelectedSB.OnDehoveredMock(eventData);
					m_selectedSB = sb;
					m_selectedSB.OnHoveredMock(eventData);
				}else{
					if(SelectedSB != null){
						SelectedSB.OnDehoveredMock(eventData);
						m_selectedSB = null;
					}
				}
			}
		}
		public void SimSGHover(SlotGroup sg, PointerEventDataMock eventData){
			if(CurState == SlotGroupManager.m_probingState){
				if(sg != null){
					if(SelectedSG != null && SelectedSG != sg)
						SelectedSG.OnDehoveredMock(eventData);
					m_selectedSG = sg;
					m_selectedSG.OnHoveredMock(eventData);
				}else{
					if(SelectedSG != null){
						SelectedSG.OnDehoveredMock(eventData);
						m_selectedSG = null;
					}
				}
			}
		}

	}

}
