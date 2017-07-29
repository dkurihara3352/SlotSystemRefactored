using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public abstract class SlotSystemElement :MonoBehaviour, ISlotSystemElement{
		/*	state	*/
			ISSEStateEngine<ISSESelState> selStateEngine{
				get{
					if(m_selStateEngine == null)
						m_selStateEngine = new SSEStateEngine<ISSESelState>(this);
					return m_selStateEngine;
				}
				}ISSEStateEngine<ISSESelState> m_selStateEngine;
			void SetSelStateEngine(ISSEStateEngine<ISSESelState> engine){m_selStateEngine = engine;}
			ISSESelState prevSelState{
				get{return selStateEngine.prevState;}
			}
			public ISSESelState curSelState{
				get{return selStateEngine.curState;}
			}
			void SetSelState(ISSESelState state){
				selStateEngine.SetState(state);
				if(state == null && selProcess != null)
					SetAndRunSelProcess(null);
			}
			public bool isSelStateInit{get{return prevSelState ==null;}}
			public bool isCurSelStateNull{get{return curSelState == null;}}
			public bool isPrevSelStateNull{get{return curSelState == null;}}
			public void ClearCurSelState(){SetSelState(null);}
			/*	selStates	*/
				ISSESelState deactivatedState{
					get{
						if(m_deactivatedState == null)
							m_deactivatedState = new SSEDeactivatedState();
						return m_deactivatedState;
					}
					}ISSESelState m_deactivatedState;
					public void SetDeactivatedState(ISSESelState state){m_deactivatedState = state;}
					public virtual void Deactivate(){
						SetSelState(deactivatedState);
					}
					public virtual bool isDeactivated{get{return curSelState == deactivatedState;}}
					public virtual bool wasDeactivated{get{return prevSelState == deactivatedState;}}
				ISSESelState defocusedState{
					get{
						if(m_defocusedState == null)
							m_defocusedState = new SSEDefocusedState();
						return m_defocusedState;
					}
					}ISSESelState m_defocusedState;
					public void SetDefocusedState(ISSESelState state){m_defocusedState = state;}
					public virtual void Defocus(){
						SetSelState(defocusedState);
					}
					public virtual bool isDefocused{get{return curSelState == defocusedState;}}
					public virtual bool wasDefocused{get{return prevSelState == defocusedState;}}
				ISSESelState focusedState{
					get{
						if(m_focusedState == null)
							m_focusedState = new SSEFocusedState();
						return m_focusedState;
					}
					}ISSESelState m_focusedState;
					public void SetFocusedState(ISSESelState state){m_focusedState = state;}
					public virtual void Focus(){
						SetSelState(focusedState);
					}
					public virtual bool isFocused{get{return curSelState == focusedState;}}
					public virtual bool wasFocused{get{return prevSelState == focusedState;}}
				ISSESelState selectedState{
					get{
						if(m_selectedState == null)
							m_selectedState = new SSESelectedState();
						return m_selectedState;
					}
					}ISSESelState m_selectedState;
					public void SetSelectedState(ISSESelState state){m_selectedState = state;}
					public virtual void Select(){
						SetSelState(selectedState);
					}
					public virtual bool isSelected{get{return curSelState == selectedState;}}
					public virtual bool wasSelected{get{return prevSelState == selectedState;}}
		/*	process	*/
			/*	Selection Processs	*/
				public virtual ISSEProcessEngine<ISSESelProcess> selProcEngine{
					get{
						if(m_selProcEngine == null)
							m_selProcEngine = new SSEProcessEngine<ISSESelProcess>();
						return m_selProcEngine;
					}
					}ISSEProcessEngine<ISSESelProcess> m_selProcEngine;
				public virtual void SetSelProcEngine(ISSEProcessEngine<ISSESelProcess> engine){
					m_selProcEngine = engine;
				}
				public virtual ISSESelProcess selProcess{
					get{return selProcEngine.process;}
				}
				public virtual void SetAndRunSelProcess(ISSESelProcess process){
					selProcEngine.SetAndRunProcess(process);
				}
				/* Coroutines */
					public System.Func<IEnumeratorFake> deactivateCoroutine{
						get{
							if(m_deactivateCoroutine == null)
								return defDeaCoroutine;
							return m_deactivateCoroutine;
						}
						}System.Func<IEnumeratorFake> m_deactivateCoroutine;
						public void SetDeaCoroutine(System.Func<IEnumeratorFake> func){m_deactivateCoroutine = func;}
						IEnumeratorFake defDeaCoroutine(){return null;}
					public System.Func<IEnumeratorFake> focusCoroutine{
						get{
							if(m_focusCoroutine == null)
								return defFocCoroutine;
							return m_focusCoroutine;
						}
						}System.Func<IEnumeratorFake> m_focusCoroutine;
						public void SetFocCoroutine(System.Func<IEnumeratorFake> func){m_focusCoroutine = func;}
						IEnumeratorFake defFocCoroutine(){return null;}
					public System.Func<IEnumeratorFake> defocusCoroutine{
						get{
							if(m_defocusCoroutine == null)
								return defDefocusCoroutine;
							return m_defocusCoroutine;
						}
						}System.Func<IEnumeratorFake> m_defocusCoroutine;
						public void SetDefCoroutine(System.Func<IEnumeratorFake> func){m_defocusCoroutine = func;}
						IEnumeratorFake defDefocusCoroutine(){return null;}
					public System.Func<IEnumeratorFake> selectCoroutine{
						get{
							if(m_selectCoroutine == null)
								return defSelCoroutine;
							return m_selectCoroutine;
						}
						}System.Func<IEnumeratorFake> m_selectCoroutine;
						public void SetSelCoroutine(System.Func<IEnumeratorFake> func){m_selectCoroutine = func;}
						IEnumeratorFake defSelCoroutine(){return null;}
					
		/* Events */
			public virtual void OnHoverEnter(){
				onHoverEnterCommand.Execute();
			}
			ISSECommand onHoverEnterCommand{
				get{
					if(m_onHoverEnterCommand == null)
						m_onHoverEnterCommand = new OnHoverEnterCommand(this);
					return m_onHoverEnterCommand;
				}
				}ISSECommand m_onHoverEnterCommand;
				public void SetOnHoverEnterCommand(ISSECommand comm){m_onHoverEnterCommand = comm;}
			public virtual void OnHoverExit(){
				onHoverExitCommand.Execute();
			}
			ISSECommand onHoverExitCommand{
				get{
					if(m_onHoverExitCommand == null)
						m_onHoverExitCommand = new OnHoverExitCommand(this);
					return m_onHoverExitCommand;
				}
				}ISSECommand m_onHoverExitCommand;
				public void SetOnHoverExitCommand(ISSECommand comm){m_onHoverExitCommand = comm;}
		/*	public fields	*/
			public virtual ISlotSystemElement this[int i]{
				get{
					int id = 0;
					foreach(var ele in elements){
						if(id++ == i)
							return ele;	
					}
					throw new System.ArgumentOutOfRangeException("AbsSlotSysElement.indexer: argument out of range");
				}
			}
			public virtual string eName{get{return m_eName;}} protected string m_eName;
			public virtual IEnumerable<ISlotSystemElement> elements{get{return m_elements;}} protected IEnumerable<ISlotSystemElement> m_elements;
			public virtual void SetHierarchy(){
				List<ISlotSystemElement> elements = new List<ISlotSystemElement>();
				for(int i = 0; i < transform.childCount; i++){
					ISlotSystemElement ele = transform.GetChild(i).GetComponent<ISlotSystemElement>();
					if(ele != null){
						elements.Add(ele);
					}
				}
				m_elements = elements;
				foreach(var e in this)
					e.SetParent(this);
			}
			public virtual ISlotSystemElement parent{get{return m_parent;}} ISlotSystemElement m_parent;
				public virtual void SetParent(ISlotSystemElement par){m_parent = par;}
			public virtual ISlotSystemBundle immediateBundle{
				get{
					if(parent == null)
						return null;
					else if(parent is ISlotSystemBundle)
						return (ISlotSystemBundle)parent;
					else
						return parent.immediateBundle;
				}
			}
			public virtual ISlotSystemManager ssm{get{return m_ssm;}} ISlotSystemManager m_ssm;
				public virtual void SetSSM(ISlotSystemElement ssm){m_ssm = (ISlotSystemManager)ssm;}
			public virtual int level{
				get{
					if(parent == null)
						return 0;
					return parent.level + 1;
				}
			}
			public bool isBundleElement{
				get{
					return parent is ISlotSystemBundle;
				}
			}
			public bool isFocusedInHierarchy{
				get{
					ISlotSystemElement inspected = this;
					while(true){
						if(inspected　== null)
							break;
						if(inspected.isFocused)
							inspected = inspected.parent;
						else
							return false;
					}
					return true;
				}
			}
		/*	methods	*/
			public virtual void InitializeStates(){
				Deactivate();
			}
			public virtual IEnumerator<ISlotSystemElement> GetEnumerator(){
				foreach(ISlotSystemElement ele in elements)
					yield return ele;
				}IEnumerator IEnumerable.GetEnumerator(){
					return GetEnumerator();
				}
			public virtual bool ContainsInHierarchy(ISlotSystemElement ele){
				if(ele != null){
					ISlotSystemElement testEle = ele.parent;
					while(true){
						if(testEle == null)
							return false;
						if(testEle == (ISlotSystemElement)this)
							return true;
						testEle = testEle.parent;
					}
				}
				throw new System.ArgumentNullException();
			}
			public virtual void PerformInHierarchy(System.Action<ISlotSystemElement> act){
				act(this);
				if(elements != null)
					foreach(ISlotSystemElement ele in this){
						ele.PerformInHierarchy(act);
					}
			}
			public virtual void PerformInHierarchy(System.Action<ISlotSystemElement, object> act, object obj){
				act(this, obj);
				if(elements != null)
					foreach(ISlotSystemElement ele in this){
						ele.PerformInHierarchy(act, obj);
					}
			}
			public virtual void PerformInHierarchy<T>(System.Action<ISlotSystemElement, IList<T>> act, IList<T> list){
				act(this, list);
				if(elements != null)
					foreach(ISlotSystemElement ele in this){
						ele.PerformInHierarchy<T>(act, list);
					}
			}
			public virtual bool Contains(ISlotSystemElement element){
				if(elements != null)
					foreach(ISlotSystemElement ele in elements){
						if(ele != null && ele == element)
							return true;
					}
				return false;
			}
			public virtual void InstantDefocus(){instantDefocusCommand.Execute();}
				public virtual ISSECommand instantDefocusCommand{
					get{if(m_instantDefocusCommand == null)
							m_instantDefocusCommand = new InstantDefocusCommand();
						return m_instantDefocusCommand;
					}
					}ISSECommand m_instantDefocusCommand;
					public virtual void SetInstantDefocusCommand(ISSECommand comm){m_instantDefocusCommand = comm;}
			public virtual void InstantFocus(){instantFocusCommand.Execute();}
				public virtual ISSECommand instantFocusCommand{
					get{if(m_instantFocusCommand == null)
							m_instantFocusCommand = new InstantFocusCommand();
						return m_instantFocusCommand;
					}
					}ISSECommand m_instantFocusCommand;
					public virtual void SetInstantFocusCommand(ISSECommand comm){m_instantFocusCommand = comm;}
					
			public virtual void InstantSelect(){instantSelectCommand.Execute();}
				public virtual ISSECommand instantSelectCommand{
					get{if(m_instantSelectCommand == null)
							m_instantSelectCommand = new InstantSelectCommand();
						return m_instantSelectCommand;
					}
					}ISSECommand m_instantSelectCommand;
					public virtual void SetInstantSelectCommand(ISSECommand comm){m_instantSelectCommand = comm;}
			public void SetElements(IEnumerable<ISlotSystemElement> elements){m_elements = elements;}
			public bool isActivatedOnDefault{
				get{
					ISlotSystemElement inspected = parent;
					while(true){
						if(inspected　== null)
							break;
						if(inspected.isActivatedOnDefault)
							inspected = inspected.parent;
						else
							return false;
					}
					return m_isActivatedOnDefault;
				}
				set{
					if(isBundleElement && immediateBundle.focusedElement == (ISlotSystemElement)this)
						m_isActivatedOnDefault = true;
					else
						m_isActivatedOnDefault = value;
					}
				}bool m_isActivatedOnDefault = true;
			public bool isFocusableInHierarchy{
				get{
					ISlotSystemElement inspected = this;
					while(true){
						if(inspected.immediateBundle == null)
							break;
						if(inspected.immediateBundle.focusedElement == inspected || inspected.immediateBundle.focusedElement.ContainsInHierarchy(inspected))
							inspected = inspected.immediateBundle;
						else
							return false;
					}
					return true;
				}
			}
			public void ActivateRecursively(){
				PerformInHierarchy(ActivateInHi);
			}
				void ActivateInHi(ISlotSystemElement ele){
					if(ele.isActivatedOnDefault){
						if(ele.isFocusableInHierarchy)
							ele.Activate();
						else
							ele.Defocus();
					}
				}
			public virtual void Activate(){
				Focus();
			}
			public virtual void Deselect(){
				Focus();
			}
	}
	public interface ISlotSystemElement: IEnumerable<ISlotSystemElement>, IStateHandler{
		/* Sel states */
			ISSESelState curSelState{get;}
			bool isSelStateInit{get;}
			bool isCurSelStateNull{get;}
			bool isPrevSelStateNull{get;}
			void ClearCurSelState();
			void Deactivate();
				bool isDeactivated{get;}
				bool wasDeactivated{get;}
				void SetDeactivatedState(ISSESelState state);
			void Focus();
				bool isFocused{get;}
				bool wasFocused{get;}
				void SetFocusedState(ISSESelState state);
			void Defocus();
				bool isDefocused{get;}
				bool wasDefocused{get;}
				void SetDefocusedState(ISSESelState state);
			void Select();
				bool isSelected{get;}
				bool wasSelected{get;}
				void SetSelectedState(ISSESelState state);
		ISSEProcessEngine<ISSESelProcess> selProcEngine{get;}
			void SetAndRunSelProcess(ISSESelProcess process);
			ISSESelProcess selProcess{get;}
			System.Func<IEnumeratorFake> deactivateCoroutine{get;}
			System.Func<IEnumeratorFake> focusCoroutine{get;}
			System.Func<IEnumeratorFake> defocusCoroutine{get;}
			System.Func<IEnumeratorFake> selectCoroutine{get;}
		void OnHoverEnter();
		void SetOnHoverEnterCommand(ISSECommand comm);
		void OnHoverExit();
		void SetOnHoverExitCommand(ISSECommand comm);
		void InstantFocus();
			ISSECommand instantFocusCommand{get;}
			void SetInstantFocusCommand(ISSECommand comm);
		void InstantDefocus();
			ISSECommand instantDefocusCommand{get;}
			void SetInstantDefocusCommand(ISSECommand comm);
		void InstantSelect();
			ISSECommand instantSelectCommand{get;}
			void SetInstantSelectCommand(ISSECommand comm);
		string eName{get;}
		bool isBundleElement{get;}
		bool isFocusedInHierarchy{get;}
		void InitializeStates();
		void Activate();
		void Deselect();
		ISlotSystemBundle immediateBundle{get;}
		ISlotSystemElement parent{get;}
		void SetParent(ISlotSystemElement par);
		ISlotSystemManager ssm{get;}
		void SetSSM(ISlotSystemElement ssm);
		IEnumerable<ISlotSystemElement> elements{get;}
		void SetHierarchy();
		bool ContainsInHierarchy(ISlotSystemElement ele);
		void PerformInHierarchy(System.Action<ISlotSystemElement> act);
		void PerformInHierarchy(System.Action<ISlotSystemElement, object> act, object obj);
		void PerformInHierarchy<T>(System.Action<ISlotSystemElement, IList<T>> act, IList<T> list);
		int level{get;}
		bool Contains(ISlotSystemElement element);
		ISlotSystemElement this[int i]{get;}
		void SetElements(IEnumerable<ISlotSystemElement> elements);
		bool isActivatedOnDefault{get;set;}
		void ActivateRecursively();
		bool isFocusableInHierarchy{get;}
	}
}
