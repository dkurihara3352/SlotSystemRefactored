using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public abstract class AbsSlotSystemElement :MonoBehaviour, SlotSystemElement{
		/*	state	*/
			public SSEStateEngine selStateEngine{
				get{
					if(m_selStateEngine == null)
						m_selStateEngine = new SSEStateEngine(this);
					return m_selStateEngine;
				}
				}SSEStateEngine m_selStateEngine;
				public virtual SSEState prevSelState{
					get{return (SSESelState)selStateEngine.prevState;}
				}
				public virtual SSEState curSelState{
					get{return (SSEState)selStateEngine.curState;}
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
				}SSEStateEngine m_actStateEngine;
				public virtual SSEState prevActState{
					get{return (SSEActState)actStateEngine.prevState;}
				}
				public virtual SSEState curActState{
					get{return (SSEActState)actStateEngine.curState;}
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
					}SSEProcessEngine m_selProcEngine;
					public virtual SSEProcess selProcess{
						get{return (SSESelProcess)selProcEngine.process;}
					}
					public virtual void SetAndRunSelProcess(SSEProcess process){
						if(process == null||process is SSESelProcess)
							selProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("AbsSlotSystemElement.SetAndRunSelProcess: argument is not of type SSESelProcess");
					}
				public virtual IEnumeratorMock greyoutCoroutine(){
					return null;
				}
				public virtual IEnumeratorMock greyinCoroutine(){
					return null;
				}
				public virtual IEnumeratorMock highlightCoroutine(){
					return null;
				}
				public virtual IEnumeratorMock dehighlightCoroutine(){
					return null;
				}
			/*	Action Process	*/
				public SSEProcessEngine actProcEngine{
					get{
						if(m_actProcEngine == null)
							m_actProcEngine = new SSEProcessEngine();
						return m_actProcEngine;
					}
					}SSEProcessEngine m_actProcEngine;
					public virtual SSEProcess actProcess{
						get{return (SSEActProcess)actProcEngine.process;}
					}
					public virtual void SetAndRunActProcess(SSEProcess process){
						if(process == null || process is SSEActProcess)
							actProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("AbsSlotSystemElement.SetAndRunActProcess: argument is not of type SSEActProcess");
					}
		/*	public fields	*/
			public virtual SlotSystemElement this[int i]{
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
			protected abstract IEnumerable<SlotSystemElement> elements{get;}
			public virtual SlotSystemElement parent{
				get{return m_parent;}
				set{m_parent = value;}
				}SlotSystemElement m_parent;
			public virtual SlotSystemBundle immediateBundle{
				get{
					if(parent == null)
						return null;
					if(parent is SlotSystemBundle)
						return (SlotSystemBundle)parent;
					else
						return parent.immediateBundle;
				}
			}
			public SlotGroupManager sgm{
				get{return m_sgm;}
				set{m_sgm = value;}
				}protected SlotGroupManager m_sgm;

			public virtual int level{
				get{
					if(parent == null)
						return 0;
					return parent.level + 1;
				}
			}
			public virtual SlotSystemElement rootElement{
				get{return m_rootElement;}
				set{m_rootElement = value;}
				}
				SlotSystemElement m_rootElement;
		/*	methods	*/
			public void Initialize(){
				SetSelState(AbsSlotSystemElement.deactivatedState);
				SetActState(AbsSlotSystemElement.waitForActionState);
			}
			public virtual IEnumerator<SlotSystemElement> GetEnumerator(){
				foreach(SlotSystemElement ele in elements)
					yield return ele;
				}IEnumerator IEnumerable.GetEnumerator(){
					return GetEnumerator();
				}
			public virtual bool ContainsInHierarchy(SlotSystemElement ele){
				SlotSystemElement testEle = ele.parent;
				while(true){
					if(testEle == null)
						return false;
					if(testEle == this)
						return true;
					testEle = testEle.parent;
				}
			}
			public virtual void Activate(){
				foreach(SlotSystemElement ele in this){
					ele.Activate();
				}
			}
			public virtual void Deactivate(){
				SetSelState(AbsSlotSystemElement.deactivatedState);
				foreach(SlotSystemElement ele in this){
					ele.Deactivate();
				}
			}
			public virtual void Focus(){
				SetSelState(AbsSlotSystemElement.focusedState);
				foreach(SlotSystemElement ele in this){
					ele.Focus();
				}
			}
			public virtual void Defocus(){
				SetSelState(AbsSlotSystemElement.defocusedState);
				foreach(SlotSystemElement ele in this){
					ele.Defocus();
				}
			}
			public virtual void PerformInHierarchy(System.Action<SlotSystemElement> act){
				act(this);
				foreach(SlotSystemElement ele in this){
					ele.PerformInHierarchy(act);
				}
			}
			public virtual void PerformInHierarchy(System.Action<SlotSystemElement, object> act, object obj){
				act(this, obj);
				foreach(SlotSystemElement ele in this){
					ele.PerformInHierarchy(act, obj);
				}
			}
			public virtual void PerformInHierarchy<T>(System.Action<SlotSystemElement, IList<T>> act, IList<T> list){
				act(this, list);
				foreach(SlotSystemElement ele in this){
					ele.PerformInHierarchy<T>(act, list);
				}
			}
			public virtual bool Contains(SlotSystemElement element){
				foreach(SlotSystemElement ele in elements){
					if(ele != null && ele == element)
						return true;
				}
				return false;
			}
			public void InstantGreyout(){}
			public void InstantGreyin(){}
			public void InstantHighlight(){}
	}
}
