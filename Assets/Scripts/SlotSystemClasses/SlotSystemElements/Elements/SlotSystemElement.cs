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
			ISSESelState curSelState{
				get{return selStateEngine.curState;}
			}
			void SetSelState(ISSESelState state){
				selStateEngine.SetState(state);
			}
			/*	selStates	*/
				public bool isSelStateInit{get{return prevSelState ==null;}}
				public ISSESelState deactivatedState{
					get{
						if(m_deactivatedState == null)
							m_deactivatedState = new SSEDeactivatedState();
						return m_deactivatedState;
					}
					}ISSESelState m_deactivatedState;
					public virtual void Deactivate(){
						SetSelState(deactivatedState);
					}
					public virtual bool isDeactivated{get{return curSelState == deactivatedState;}}
					public virtual bool wasDeactivated{get{return prevSelState == deactivatedState;}}
				public ISSESelState defocusedState{
					get{
						if(m_defocusedState == null)
							m_defocusedState = new SSEDefocusedState();
						return m_defocusedState;
					}
					}ISSESelState m_defocusedState;
					public virtual void Defocus(){
						SetSelState(defocusedState);
					}
					public virtual bool isDefocused{get{return curSelState == defocusedState;}}
					public virtual bool wasDefocused{get{return prevSelState == defocusedState;}}
				public ISSESelState focusedState{
					get{
						if(m_focusedState == null)
							m_focusedState = new SSEFocusedState();
						return m_focusedState;
					}
					}ISSESelState m_focusedState;
					public virtual void Focus(){
						SetSelState(focusedState);
					}
					public virtual bool isFocused{get{return curSelState == focusedState;}}
					public virtual bool wasFocused{get{return prevSelState == focusedState;}}
				public ISSESelState selectedState{
					get{
						if(m_selectedState == null)
							m_selectedState = new SSESelectedState();
						return m_selectedState;
					}
					}ISSESelState m_selectedState;
					public virtual void Select(){
						SetSelState(selectedState);
					}
					public virtual bool isSelected{get{return curSelState == selectedState;}}
					public virtual bool wasSelected{get{return prevSelState == selectedState;}}
		/*	process	*/
			/*	Selection Processs	*/
				public ISSEProcessEngine<ISSESelProcess> selProcEngine{
					get{
						if(m_selProcEngine == null)
							m_selProcEngine = new SSEProcessEngine<ISSESelProcess>();
						return m_selProcEngine;
					}
					}ISSEProcessEngine<ISSESelProcess> m_selProcEngine;
				public void SetSelProcEngine(ISSEProcessEngine<ISSESelProcess> engine){
					m_selProcEngine = engine;
				}
				public virtual ISSESelProcess selProcess{
					get{return selProcEngine.process;}
				}
				public virtual void SetAndRunSelProcess(ISSESelProcess process){
					selProcEngine.SetAndRunProcess(process);
				}
				/* Coroutines */
					public virtual IEnumeratorFake deactivateCoroutine(){
						return null;
					}
					public virtual IEnumeratorFake focusCoroutine(){
						return null;
					}
					public virtual IEnumeratorFake defocusCoroutine(){
						return null;
					}
					public virtual IEnumeratorFake selectCoroutine(){
						return null;
					}
		/* Events */
			public virtual void OnHoverEnterMock(){
				if(curSelState != null)
					curSelState.OnHoverEnterMock(this, new PointerEventDataFake());
				else
					throw new System.InvalidOperationException("SlotSystemElement.OnHoverEnterMock: curSelState not set");
			}
			public virtual void OnHoverExitMock(){
				if(curSelState != null)
					curSelState.OnHoverExitMock(this, new PointerEventDataFake());
				else
					throw new System.InvalidOperationException("SlotSystemElement.OnHoverEnterMock: curSelState not set");
			}
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
			public virtual string eName{get{return m_eName;}}protected string m_eName;
			public virtual IEnumerable<ISlotSystemElement> elements{get{return m_elements;}} protected IEnumerable<ISlotSystemElement> m_elements;
			public virtual void SetElements(){
				List<ISlotSystemElement> elements = new List<ISlotSystemElement>();
				for(int i = 0; i < transform.childCount; i++){
					ISlotSystemElement ele = transform.GetChild(i).GetComponent<ISlotSystemElement>();
					if(ele != null){
						elements.Add(ele);
					}
				}
				m_elements = elements;
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
			public bool isPageElement{
				get{
					return (parent is ISlotSystemPage);
				}
			}
			public bool isToggledOn{
				get{
					if(isPageElement){
						ISlotSystemPage page = (ISlotSystemPage)parent;
						return (page.GetPageElement(this).isFocusToggleOn);
					}
					return false;
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
			public bool isToggledOnInPageByDefault{
				get{
					if(isPageElement){
						return m_isInitiallyFocusedInPage;
					}else return false;
				}
				set{m_isInitiallyFocusedInPage = value;}
			}public bool m_isInitiallyFocusedInPage = true;
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
			public virtual void Activate(){
				if(elements != null)
					foreach(ISlotSystemElement ele in this){
						ele.Activate();
					}
			}
			public virtual void Deselect(){}
			public virtual void PerformInHierarchy(System.Action<ISlotSystemElement> act){
				act(this);
				if(elements != null)
					foreach(ISlotSystemElement ele in this){
						ele.PerformInHierarchy(act);
					}
			}
				public virtual void FocusInHi(ISlotSystemElement sse){
					sse.Focus();
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
			public void ToggleOnPageElement(){
				if(isPageElement){
					ISlotSystemPage page = (ISlotSystemPage)parent;
					ISlotSystemPageElement pageEle = page.GetPageElement(this);
					if(!pageEle.isFocusToggleOn)
						pageEle.isFocusToggleOn = true;
				}
			}
			public virtual void InstantDefocus(){}
			public virtual void InstantFocus(){}
			public virtual void InstantSelect(){}
			public void SetElements(IEnumerable<ISlotSystemElement> elements){m_elements = elements;}
	}
	public interface ISlotSystemElement: IEnumerable<ISlotSystemElement>, IStateHandler{
		/* Sel states */
			ISSESelState deactivatedState{get;}
			bool isSelStateInit{get;}
			void Deactivate();
			bool isDeactivated{get;}
			bool wasDeactivated{get;}
			ISSESelState focusedState{get;}
			void Focus();
			bool isFocused{get;}
			bool wasFocused{get;}
			ISSESelState defocusedState{get;}
			void Defocus();
			bool isDefocused{get;}
			bool wasDefocused{get;}
			ISSESelState selectedState{get;}
			void Select();
			bool isSelected{get;}
			bool wasSelected{get;}
		ISSEProcessEngine<ISSESelProcess> selProcEngine{get;}
			void SetAndRunSelProcess(ISSESelProcess process);
			ISSESelProcess selProcess{get;}
			IEnumeratorFake deactivateCoroutine();
			IEnumeratorFake focusCoroutine();
			IEnumeratorFake defocusCoroutine();
			IEnumeratorFake selectCoroutine();
		void OnHoverEnterMock();
		void OnHoverExitMock();
		void InstantFocus();
		void InstantDefocus();
		void InstantSelect();
		string eName{get;}
		bool isBundleElement{get;}
		bool isPageElement{get;}
		bool isToggledOn{get;}
		bool isFocusedInHierarchy{get;}
		bool isToggledOnInPageByDefault{get;set;}
		void InitializeStates();
		void Activate();
		void Deselect();
		ISlotSystemBundle immediateBundle{get;}
		ISlotSystemElement parent{get;}
		void SetParent(ISlotSystemElement par);
		ISlotSystemManager ssm{get;}
		void SetSSM(ISlotSystemElement ssm);
		IEnumerable<ISlotSystemElement> elements{get;}
		void SetElements();
		bool ContainsInHierarchy(ISlotSystemElement ele);
		void PerformInHierarchy(System.Action<ISlotSystemElement> act);
			void FocusInHi(ISlotSystemElement sse);
		void PerformInHierarchy(System.Action<ISlotSystemElement, object> act, object obj);
		void PerformInHierarchy<T>(System.Action<ISlotSystemElement, IList<T>> act, IList<T> list);
		int level{get;}
		bool Contains(ISlotSystemElement element);
		ISlotSystemElement this[int i]{get;}
		void ToggleOnPageElement();
		void SetElements(IEnumerable<ISlotSystemElement> elements);
	}
}
