using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public abstract class SlotSystemElement :MonoBehaviour, ISlotSystemElement{
		/*	state	*/
			public void ClearCurSelState(){
				SetSelState(null);
			}
				public bool isSelStateNull{
					get{return curSelState == null;}
				}
				public bool wasSelStateNull{
					get{return prevSelState == null;}
				}
			public virtual void Deactivate(){
				SetSelState(deactivatedState);
			}
				ISSESelState deactivatedState{
					get{return selStateFactory.MakeDeactivatedState();}
				}
				public virtual bool isDeactivated{
					get{return curSelState == deactivatedState;}
				}
				public virtual bool wasDeactivated{
					get{return prevSelState == deactivatedState;}
				}
			public virtual void Defocus(){
				SetSelState(defocusedState);
			}
				ISSESelState defocusedState{
					get{return selStateFactory.MakeDefocusedState();}
				}
				public virtual bool isDefocused{
					get{return curSelState == defocusedState;}
				}
				public virtual bool wasDefocused{
					get{return prevSelState == defocusedState;}
				}
			public virtual void Focus(){
				SetSelState(focusedState);
			}
				ISSESelState focusedState{
					get{return selStateFactory.MakeFocusedState();}
				}
				public virtual bool isFocused{
					get{return curSelState == focusedState;}
				}
				public virtual bool wasFocused{
					get{return prevSelState == focusedState;}
				}
			public virtual void Select(){
				SetSelState(selectedState);
			}
				ISSESelState selectedState{
					get{return selStateFactory.MakeSelectedState();}
				}
				public virtual bool isSelected{
					get{return curSelState == selectedState;}
				}
				public virtual bool wasSelected{
					get{return prevSelState == selectedState;}
				}
			public virtual void Activate(){
				Focus();
			}
			public virtual void Deselect(){
				Focus();
			}
			public virtual void InitializeStates(){
				Deactivate();
			}
			public virtual ISSESelStateFactory selStateFactory{
				get{
					if(m_selStateFactory == null)
						m_selStateFactory = new SSESelStateFacotory(this);
					return m_selStateFactory;
				}
			}
				protected ISSESelStateFactory m_selStateFactory;
			public void SetSelStateFactory(ISSESelStateFactory factory){
				m_selStateFactory = factory;
			}
			ISSEStateEngine<ISSESelState> selStateEngine{
				get{
					if(m_selStateEngine == null)
						m_selStateEngine = new SSEStateEngine<ISSESelState>();
					return m_selStateEngine;
				}
			}
				ISSEStateEngine<ISSESelState> m_selStateEngine;
			void SetSelStateEngine(ISSEStateEngine<ISSESelState> engine){
				m_selStateEngine = engine;
			}
			ISSESelState prevSelState{
				get{return selStateEngine.prevState;}
			}
			ISSESelState curSelState{
				get{return selStateEngine.curState;}
			}
			void SetSelState(ISSESelState state){
				selStateEngine.SetState(state);
				if(state == null && selProcess != null)
					SetAndRunSelProcess(null);
			}
		/*	process	*/
			public virtual void SetAndRunSelProcess(ISSESelProcess process){
				selProcEngine.SetAndRunProcess(process);
			}
			public virtual ISSESelProcess selProcess{
				get{return selProcEngine.process;}
			}
			public virtual ISSEProcessEngine<ISSESelProcess> selProcEngine{
				get{
					if(m_selProcEngine == null)
						m_selProcEngine = new SSEProcessEngine<ISSESelProcess>();
					return m_selProcEngine;
				}
			}
				ISSEProcessEngine<ISSESelProcess> m_selProcEngine;
			public virtual void SetSelProcEngine(ISSEProcessEngine<ISSESelProcess> engine){
				m_selProcEngine = engine;
			}
			/* Coroutines */
				public System.Func<IEnumeratorFake> deactivateCoroutine{
					get{return coroutineFactory.MakeDeactivateCoroutine();}
				}
				public System.Func<IEnumeratorFake> focusCoroutine{
					get{return coroutineFactory.MakeFocusCoroutine();}
				}
				public System.Func<IEnumeratorFake> defocusCoroutine{
					get{return coroutineFactory.MakeDefocusCoroutine();}
				}
				public System.Func<IEnumeratorFake> selectCoroutine{
					get{return coroutineFactory.MakeSelectCoroutine();}
				}
				ISSECoroutineFactory coroutineFactory{
					get{
						if(m_coroutineFactory == null)
							m_coroutineFactory = new SSECoroutineFactory();
						return m_coroutineFactory;
					}
				}
					ISSECoroutineFactory m_coroutineFactory;
					public void SetCoroutineFactory(ISSECoroutineFactory factory){m_coroutineFactory = factory;}

		/* Instant Methods */
			IInstantCommands instantCommands{
				get{
					if(m_instantCommands == null)
						m_instantCommands = new InstantCommands();
					return m_instantCommands;
				}
			}
				IInstantCommands m_instantCommands;
				public void SetInstantCommands(IInstantCommands comms){
					m_instantCommands = comms;
				}
			public virtual void InstantDefocus(){
				instantCommands.ExecuteInstantDefocus();
			}
			public virtual void InstantFocus(){
				instantCommands.ExecuteInstantFocus();
			}
			public virtual void InstantSelect(){
				instantCommands.ExecuteInstantSelect();
			}
		/*	public fields	*/
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
			public virtual bool isActivatedOnDefault{
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
			}
				bool m_isActivatedOnDefault = true;
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
			public virtual bool isFocusedInHierarchy{
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
			public virtual bool isFocusableInHierarchy{
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
			public virtual bool isBundleElement{
				get{
					return parent is ISlotSystemBundle;
				}
			}
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
			public virtual ISlotSystemElement parent{
				get{return m_parent;}
			}
				ISlotSystemElement m_parent;
			public virtual void SetParent(ISlotSystemElement par){
				m_parent = par;
			}
			public virtual ISlotSystemManager ssm{
				get{return m_ssm;}
			}
				ISlotSystemManager m_ssm;
			public virtual void SetSSM(ISlotSystemElement ssm){
				m_ssm = (ISlotSystemManager)ssm;
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
			public void SetElements(IEnumerable<ISlotSystemElement> elements){m_elements = elements;}
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
			public virtual string eName{
				get{return m_eName;}
			}
				protected string m_eName;
			protected virtual IEnumerable<ISlotSystemElement> elements{
				get{return m_elements;}
			}
				protected IEnumerable<ISlotSystemElement> m_elements;
			public virtual int level{
				get{
					if(parent == null)
						return 0;
					return parent.level + 1;
				}
			}
			public virtual IEnumerator<ISlotSystemElement> GetEnumerator(){
				foreach(ISlotSystemElement ele in elements)
					yield return ele;
			}
				IEnumerator IEnumerable.GetEnumerator(){
					return GetEnumerator();
				}	
	}
	public interface ISlotSystemElement: IEnumerable<ISlotSystemElement>, IStateHandler{
		/* Sel states */
			bool isSelStateNull{get;}
			bool wasSelStateNull{get;}
			void Deactivate();
				bool isDeactivated{get;}
				bool wasDeactivated{get;}
			void Focus();
				bool isFocused{get;}
				bool wasFocused{get;}
			void Defocus();
				bool isDefocused{get;}
				bool wasDefocused{get;}
			void Select();
				bool isSelected{get;}
				bool wasSelected{get;}
			void Activate();
			void Deselect();
			void InitializeStates();
			void SetAndRunSelProcess(ISSESelProcess process);
			ISSESelProcess selProcess{get;}
			System.Func<IEnumeratorFake> deactivateCoroutine{get;}
			System.Func<IEnumeratorFake> focusCoroutine{get;}
			System.Func<IEnumeratorFake> defocusCoroutine{get;}
			System.Func<IEnumeratorFake> selectCoroutine{get;}
		void InstantFocus();
		void InstantDefocus();
		void InstantSelect();

		void SetHierarchy();
		bool isActivatedOnDefault{get;set;}
		void ActivateRecursively();
		bool isFocusedInHierarchy{get;}
		bool isFocusableInHierarchy{get;}
		bool isBundleElement{get;}
		ISlotSystemBundle immediateBundle{get;}
		ISlotSystemElement parent{get;}
		void SetParent(ISlotSystemElement par);
		ISlotSystemManager ssm{get;}
		void SetSSM(ISlotSystemElement ssm);
		bool ContainsInHierarchy(ISlotSystemElement ele);
		void PerformInHierarchy(System.Action<ISlotSystemElement> act);
		void PerformInHierarchy(System.Action<ISlotSystemElement, object> act, object obj);
		void PerformInHierarchy<T>(System.Action<ISlotSystemElement, IList<T>> act, IList<T> list);
		bool Contains(ISlotSystemElement element);
		ISlotSystemElement this[int i]{get;}
		string eName{get;}
		int level{get;}
	}
}
