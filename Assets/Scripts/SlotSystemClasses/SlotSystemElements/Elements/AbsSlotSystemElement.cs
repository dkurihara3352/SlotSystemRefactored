using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public abstract class AbsSlotSystemElement :MonoBehaviour, IAbsSlotSystemElement{
		/*	state	*/
			public SSEStateEngine selStateEngine{
				get{
					if(m_selStateEngine == null)
						m_selStateEngine = new SSEStateEngine(this);
					return m_selStateEngine;
				}
				set{m_selStateEngine = value;}
				}SSEStateEngine m_selStateEngine;
				public virtual SSEState prevSelState{
					get{return (SSESelState)selStateEngine.prevState;}
					set{}
				}
				public virtual SSEState curSelState{
					get{return (SSEState)selStateEngine.curState;}
					set{}
				}
				public virtual void SetSelState(SSEState state){
					if(state == null || state is SSESelState)
						selStateEngine.SetState(state);
					else throw new System.InvalidOperationException("AbsSlotSystemElement.SetSelState: argument is not of type SSESelState");
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
			public SSEStateEngine actStateEngine{
				get{
					if(m_actStateEngine == null)
						m_actStateEngine = new SSEStateEngine(this);
					return m_actStateEngine;
				}
				set{m_actStateEngine = value;}
				}SSEStateEngine m_actStateEngine;
				public virtual SSEState prevActState{
					get{return (SSEActState)actStateEngine.prevState;}
					set{}
				}
				public virtual SSEState curActState{
					get{return (SSEActState)actStateEngine.curState;}
					set{}
				}
				public virtual void SetActState(SSEState state){
					if(state == null || state is SSEActState)
						actStateEngine.SetState(state);
					else throw new System.InvalidOperationException("AbsSlotSystemElement.SetActState: argument is not of type SSEActState");
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
				public SSEProcessEngine selProcEngine{
					get{
						if(m_selProcEngine == null)
							m_selProcEngine = new SSEProcessEngine();
						return m_selProcEngine;
					}
					set{m_selProcEngine = value;}
					}SSEProcessEngine m_selProcEngine;
					public virtual SSEProcess selProcess{
						get{return (SSESelProcess)selProcEngine.process;}
						set{}
					}
					public virtual void SetAndRunSelProcess(SSEProcess process){
						if(process == null||process is SSESelProcess)
							selProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("AbsSlotSystemElement.SetAndRunSelProcess: argument is not of type SSESelProcess");
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
				public SSEProcessEngine actProcEngine{
					get{
						if(m_actProcEngine == null)
							m_actProcEngine = new SSEProcessEngine();
						return m_actProcEngine;
					}
					set{m_actProcEngine = value;}
					}SSEProcessEngine m_actProcEngine;
					public virtual SSEProcess actProcess{
						get{return (SSEActProcess)actProcEngine.process;}
						set{}
					}
					public virtual void SetAndRunActProcess(SSEProcess process){
						if(process == null || process is SSEActProcess)
							actProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("AbsSlotSystemElement.SetAndRunActProcess: argument is not of type SSEActProcess");
					}
		/*	public fields	*/
			public virtual ISlotSystemElement this[int i]{
				get{
					int id = 0;
					foreach(var ele in elements){
						if(id++ == i)
							return ele;	
					}
					throw new System.InvalidOperationException("AbsSlotSysElement.indexer: argument out of range");
				}
			}
			public virtual string eName{get{return m_eName;}}protected string m_eName;
			public abstract IEnumerable<ISlotSystemElement> elements{get;}
			public virtual ISlotSystemElement parent{
				get{return m_parent;}
				set{m_parent = value;}
				}ISlotSystemElement m_parent;
			public virtual ISlotSystemBundle immediateBundle{
				get{
					if(parent == null)
						return null;
					if(parent is ISlotSystemBundle)
						return (ISlotSystemBundle)parent;
					else
						return parent.immediateBundle;
				}
				set{}
			}
			public ISlotSystemManager ssm{
				get{return m_ssm;}
				set{m_ssm = value;}
				}ISlotSystemManager m_ssm;
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
				set{}
			}
			public virtual bool isDefocused{
				get{return curSelState == AbsSlotSystemElement.defocusedState;}
				set{}
			}
			public virtual bool isDeactivated{
				get{return curSelState == AbsSlotSystemElement.deactivatedState;}
				set{}
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
			public void Initialize(){
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
		IEnumerable<ISlotSystemElement> elements{get;}
		SSEStateEngine selStateEngine{get;set;}
		void SetSelState(SSEState state);
		SSEStateEngine actStateEngine{get;set;}
		void SetActState(SSEState state);
		SSEProcessEngine selProcEngine{get;set;}
		SSEProcessEngine actProcEngine{get;set;}
		void Initialize();
	}
}
