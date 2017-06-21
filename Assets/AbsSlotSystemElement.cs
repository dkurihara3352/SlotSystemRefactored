using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public abstract class AbsSlotSystemElement :MonoBehaviour, SlotSystemElement{
		/*	state	*/
		public SSESelStateEngine selStateEngine{
			get{
				if(m_selStateEngine == null)
					m_selStateEngine = new SSESelStateEngine(this);
				return m_selStateEngine;
			}
			}SSESelStateEngine m_selStateEngine;
			public SSESelState prevSelState{
				get{return selStateEngine.prevState;}
			}
			public SSESelState curSelState{
				get{return selStateEngine.curState;}
			}
			public virtual void SetSelState(SSESelState state){
				selStateEngine.SetState(state);
			}
		public SSEActStateEngine actStateEngine{
			get{
				if(m_actStateEngine == null)
					m_actStateEngine = new SSEActStateEngine(this);
				return m_actStateEngine;
			}
			}SSEActStateEngine m_actStateEngine;
			public SSEActState prevActState{
				get{return actStateEngine.prevState;}
			}
			public SSEActState curActState{
				get{return actStateEngine.curState;}
			}
			public virtual void SetActState(SSEActState state){
				actStateEngine.SetState(state);
			}
		

		/*	process	*/
		public string eName{get{return m_eName;}}protected string m_eName;
		protected abstract IEnumerable<SlotSystemElement> elements{get;}
		public IEnumerator<SlotSystemElement> GetEnumerator(){
			foreach(SlotSystemElement ele in elements)
				yield return ele;
			}IEnumerator IEnumerable.GetEnumerator(){
				return GetEnumerator();
			}
		public bool Contains(SlotSystemElement element){
			foreach(SlotSystemElement ele in elements){
				if(ele != null && ele == element)
					return true;
			}
			return false;
		}
		public SlotSystemElement this[int i]{
			get{
				int id = 0;
				foreach(var ele in elements){
					if(id++ == i)
						return ele;	
				}
				throw new System.InvalidOperationException("AbsSlotSysElement.indexer: argument out of range");
			}
		}
		public SlotSystemElement parent{
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
			foreach(SlotSystemElement ele in this){
				ele.Deactivate();
			}
		}
		public virtual void Focus(){
			foreach(SlotSystemElement ele in this){
				ele.Focus();
			}
		}
		public virtual void Defocus(){
			foreach(SlotSystemElement ele in this){
				ele.Defocus();
			}
		}
		public SlotGroupManager sgm{
			get{return m_sgm;}
			set{m_sgm = value;}
			}SlotGroupManager m_sgm;
		public void PerformInHierarchy(System.Action<SlotSystemElement> act){
			act(this);
			foreach(SlotSystemElement ele in this){
				ele.PerformInHierarchy(act);
			}
		}
		public void PerformInHierarchy(System.Action<SlotSystemElement, object> act, object obj){
			act(this, obj);
			foreach(SlotSystemElement ele in this){
				ele.PerformInHierarchy(act, obj);
			}
		}
		public void PerformInHierarchy<T>(System.Action<SlotSystemElement, IList<T>> act, IList<T> list){
			act(this, list);
			foreach(SlotSystemElement ele in this){
				ele.PerformInHierarchy<T>(act, list);
			}
		}
		public int level{
			get{
				if(parent == null)
					return 0;
				else
					return parent.level + 1;
			}
		}
		public virtual SlotSystemElement rootElement{
			get{return m_rootElement;}
			set{m_rootElement = value;}
			}
			SlotSystemElement m_rootElement;
	}
}
