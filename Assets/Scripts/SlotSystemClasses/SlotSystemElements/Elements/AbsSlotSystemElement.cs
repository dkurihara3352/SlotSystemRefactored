using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public abstract class AbsSlotSystemElement :MonoBehaviour, IAbsSlotSystemElement{
		/*	state	*/
			public ISSEStateEngine selStateEngine{
				get{
					if(m_selStateEngine == null)
						m_selStateEngine = new SSEStateEngine(this);
					return m_selStateEngine;
				}
				}ISSEStateEngine m_selStateEngine;
				public void SetSelStateEngine(ISSEStateEngine engine){m_selStateEngine = engine;}
				public virtual SSEState prevSelState{
					get{return (SSESelState)selStateEngine.prevState;}
				}
				public virtual SSEState curSelState{
					get{return (SSEState)selStateEngine.curState;}
				}
				public virtual void SetSelState(SSEState state){
					if(state == null || state is SSESelState)
						selStateEngine.SetState(state);
					else throw new System.ArgumentException("AbsSlotSystemElement.SetSelState: argument is not of type SSESelState");
				}
				/*	static sel states	*/
					public static SSESelState deactivatedState{
						get{
							if(m_deactivatedState == null)
								m_deactivatedState = new SSEDeactivatedState();
							return m_deactivatedState;
						}
						}static SSESelState m_deactivatedState;
					public static SSESelState defocusedState{
						get{
							if(m_defocusedState == null)
								m_defocusedState = new SSEDefocusedState();
							return m_defocusedState;
						}
						}static SSESelState m_defocusedState;
					public static SSESelState focusedState{
						get{
							if(m_focusedState == null)
								m_focusedState = new SSEFocusedState();
							return m_focusedState;
						}
						}static SSESelState m_focusedState;
					public static SSESelState selectedState{
						get{
							if(m_selectedState == null)
								m_selectedState = new SSESelectedState();
							return m_selectedState;
						}
						}static SSESelState m_selectedState;
			public ISSEStateEngine actStateEngine{
				get{
					if(m_actStateEngine == null)
						m_actStateEngine = new SSEStateEngine(this);
					return m_actStateEngine;
				}
				}ISSEStateEngine m_actStateEngine;
				public void SetActStateEngine(ISSEStateEngine engine){m_actStateEngine = engine;}
				public virtual SSEState prevActState{
					get{return (SSEActState)actStateEngine.prevState;}
				}
				public virtual SSEState curActState{
					get{return (SSEActState)actStateEngine.curState;}
				}
				public virtual void SetActState(SSEState state){
					if(state == null || state is SSEActState)
						actStateEngine.SetState(state);
					else throw new System.ArgumentException("AbsSlotSystemElement.SetActState: argument is not of type SSEActState");
				}
				/*	static act state	*/
					public static SSEActState waitForActionState{
						get{
							if(m_waitForActionState == null)
								m_waitForActionState = new SSEWaitForActionState();
							return m_waitForActionState;
						}
						}static SSEActState m_waitForActionState;
		/*	process	*/
			/*	Selection Processs	*/
				public ISSEProcessEngine selProcEngine{
					get{
						if(m_selProcEngine == null)
							m_selProcEngine = new SSEProcessEngine();
						return m_selProcEngine;
					}
					}ISSEProcessEngine m_selProcEngine;
					public void SetSelProcEngine(ISSEProcessEngine engine){
						m_selProcEngine = engine;
					}
					public virtual ISSEProcess selProcess{
						get{return (SSESelProcess)selProcEngine.process;}
					}
					public virtual void SetAndRunSelProcess(ISSEProcess process){
						if(process == null||process is SSESelProcess)
							selProcEngine.SetAndRunProcess(process);
						else throw new System.ArgumentException("AbsSlotSystemElement.SetAndRunSelProcess: argument is not of type SSESelProcess");
					}
				public virtual IEnumeratorFake greyoutCoroutine(){
					return null;
				}
				public virtual IEnumeratorFake greyinCoroutine(){
					return null;
				}
				public virtual IEnumeratorFake highlightCoroutine(){
					return null;
				}
				public virtual IEnumeratorFake dehighlightCoroutine(){
					return null;
				}
			/*	Action Process	*/
				public ISSEProcessEngine actProcEngine{
					get{
						if(m_actProcEngine == null)
							m_actProcEngine = new SSEProcessEngine();
						return m_actProcEngine;
					}
					}ISSEProcessEngine m_actProcEngine;
					public void SetActProcEngine(ISSEProcessEngine engine){m_actProcEngine = engine;}
					public virtual ISSEProcess actProcess{
						get{return (SSEActProcess)actProcEngine.process;}
					}
					public virtual void SetAndRunActProcess(ISSEProcess process){
						if(process == null || process is SSEActProcess)
							actProcEngine.SetAndRunProcess(process);
						else throw new System.ArgumentException("AbsSlotSystemElement.SetAndRunActProcess: argument is not of type SSEActProcess");
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
			public virtual bool isFocused{
				get{return curSelState == AbsSlotSystemElement.focusedState;}
			}
			public virtual bool isDefocused{
				get{return curSelState == AbsSlotSystemElement.defocusedState;}
			}
			public virtual bool isDeactivated{
				get{return curSelState == AbsSlotSystemElement.deactivatedState;}
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
			public bool isInitiallyFocusedInPage{
				get{
					if(isPageElement){
						return m_isInitiallyFocusedInPage;
					}else return false;
				}
				set{m_isInitiallyFocusedInPage = value;}
			}public bool m_isInitiallyFocusedInPage;
		/*	methods	*/
			public virtual void InitializeStates(){
				SetSelState(AbsSlotSystemElement.deactivatedState);
				SetActState(AbsSlotSystemElement.waitForActionState);
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
				foreach(ISlotSystemElement ele in this){
					ele.Activate();
				}
			}
			public virtual void Deactivate(){
				SetSelState(AbsSlotSystemElement.deactivatedState);
				if(elements != null){
					foreach(ISlotSystemElement ele in this){
						ele.Deactivate();
					}
				}
			}
			public virtual void Focus(){
				SetSelState(AbsSlotSystemElement.focusedState);
				if(elements != null)
					foreach(ISlotSystemElement ele in this){
						ele.Focus();
					}
			}
			public virtual void Defocus(){
				SetSelState(AbsSlotSystemElement.defocusedState);
				if(elements != null)
					foreach(ISlotSystemElement ele in this){
						ele.Defocus();
					}
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
			public void ToggleOnPageElement(){
				if(isPageElement){
					ISlotSystemPage page = (ISlotSystemPage)parent;
					ISlotSystemPageElement pageEle = page.GetPageElement(this);
					if(!pageEle.isFocusToggleOn)
						pageEle.isFocusToggleOn = true;
				}
			}
			public virtual void InstantGreyout(){}
			public virtual void InstantGreyin(){}
			public virtual void InstantHighlight(){}
	}
	public interface IAbsSlotSystemElement: ISlotSystemElement{
		ISSEStateEngine selStateEngine{get;}
		void SetSelState(SSEState state);
		ISSEStateEngine actStateEngine{get;}
		void SetActState(SSEState state);
		ISSEProcessEngine selProcEngine{get;}
		ISSEProcessEngine actProcEngine{get;}
	}
	public interface ISlotSystemElement: IEnumerable<ISlotSystemElement>, IStateHandler{
		SSEState curSelState{get;}
		SSEState prevSelState{get;}
		SSEState curActState{get;}
		SSEState prevActState{get;}
		void SetAndRunSelProcess(ISSEProcess process);
		void SetAndRunActProcess(ISSEProcess process);
		ISSEProcess selProcess{get;}
		ISSEProcess actProcess{get;}
		IEnumeratorFake greyoutCoroutine();
		IEnumeratorFake greyinCoroutine();
		IEnumeratorFake highlightCoroutine();
		IEnumeratorFake dehighlightCoroutine();
		void InstantGreyin();
		void InstantGreyout();
		void InstantHighlight();
		string eName{get;}
		bool isBundleElement{get;}
		bool isPageElement{get;}
		bool isToggledOn{get;}
		bool isFocused{get;}
		bool isDefocused{get;}
		bool isDeactivated{get;}
		bool isFocusedInHierarchy{get;}
		bool isInitiallyFocusedInPage{get;set;}
		void InitializeStates();
		void Activate();
		void Deactivate();
		void Focus();
		void Defocus();
		ISlotSystemBundle immediateBundle{get;}
		ISlotSystemElement parent{get;}
		void SetParent(ISlotSystemElement par);
		ISlotSystemManager ssm{get;}
		void SetSSM(ISlotSystemElement ssm);
		IEnumerable<ISlotSystemElement> elements{get;}
		void SetElements();
		bool ContainsInHierarchy(ISlotSystemElement ele);
		void PerformInHierarchy(System.Action<ISlotSystemElement> act);
		void PerformInHierarchy(System.Action<ISlotSystemElement, object> act, object obj);
		void PerformInHierarchy<T>(System.Action<ISlotSystemElement, IList<T>> act, IList<T> list);
		int level{get;}
		bool Contains(ISlotSystemElement element);
		ISlotSystemElement this[int i]{get;}
		void ToggleOnPageElement();
	}
}
