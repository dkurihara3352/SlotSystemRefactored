using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public class SSEProcess: ISSEProcess{
		public virtual bool isRunning{ get{return m_isRunning;} }bool m_isRunning;
		public System.Func<IEnumeratorFake> coroutineFake{get{return m_coroutineFake;} set{m_coroutineFake = value;}} System.Func<IEnumeratorFake> m_coroutineFake;
		public ISlotSystemElement sse{ get{return m_sse;} set{m_sse = value;}} ISlotSystemElement m_sse;
		public virtual void Start(){
			m_isRunning = true;
			m_coroutineFake();
		}
		public virtual void Stop(){ if(isRunning) m_isRunning = false; }
		public virtual void Expire(){if(isRunning) m_isRunning = false; }
		public bool Equals(ISSEProcess other){
			if(other != null)
				return this.GetType().Equals(other.GetType());
			else return false;
		}
		public SSEProcess(){}
		public SSEProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutine){
			m_sse = sse;
			m_coroutineFake = coroutine;
		}
	}
	public interface ISSEProcess: IEquatable<ISSEProcess>{
		bool isRunning{get;}
		System.Func<IEnumeratorFake> coroutineFake{get; set;}
		ISlotSystemElement sse{get;}
		void Start();
		void Stop();
		void Expire();
	}
	/* Engine */
	public class SSEProcessEngine<T>: ISSEProcessEngine<T> where T: ISSEProcess{
		public SSEProcessEngine(){}
		public SSEProcessEngine(T from){
			m_process = from;
		}
		public virtual T process{ get{return m_process;}}protected T m_process;
		public virtual void SetAndRunProcess(T p){
			if(p == null || !p.Equals(process)){
				if(process != null)
					process.Stop();
				m_process = p;
				if(process != null)
					process.Start();
			}
		}
	}
	public interface ISSEProcessEngine<T> where T: ISSEProcess{
		T process{get;}
		void SetAndRunProcess(T process);
	}
	/* Factory */
	public class SSECoroutineFactory: ISSECoroutineFactory{
		public Func<IEnumeratorFake> MakeDeactivateCoroutine(){
			return DefaultSSEDeactivateCoroutine;
		}
			IEnumeratorFake DefaultSSEDeactivateCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> MakeDefocusCoroutine(){
			return DefaultSSEDefocusCoroutine;
		}
			IEnumeratorFake DefaultSSEDefocusCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> MakeFocusCoroutine(){
			return DefaultSSEFocusCoroutine;
		}
			IEnumeratorFake DefaultSSEFocusCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> MakeSelectCoroutine(){
			return DefaultSSESelectCoroutine;
		}
			IEnumeratorFake DefaultSSESelectCoroutine(){
				return null;
			}
	}
	public interface ISSECoroutineFactory{
		Func<IEnumeratorFake> MakeDeactivateCoroutine();
		Func<IEnumeratorFake> MakeDefocusCoroutine();
		Func<IEnumeratorFake> MakeFocusCoroutine();
		Func<IEnumeratorFake> MakeSelectCoroutine();
	}
	/* SelProces */
	public abstract class SSESelProcess: SSEProcess, ISSESelProcess{
	}
	public interface ISSESelProcess: ISSEProcess{
	}
	public class SSEDeactivateProcess: SSESelProcess{
		/* 	Change color, alpha etc from whatever to deactivated value (probably make it disappear)
			if any indicator for selection is there, fade it out
		*/
		public SSEDeactivateProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
			this.sse = sse;
			this.coroutineFake = coroutineMock;
		}
	}
	public class SSEFocusProcess: SSESelProcess{
		/* 	Change color, alpha etc from whatever to focus value
			if its hidden, make it appear gradually
			if any indicator for selection is there, fade it out
		*/
		public SSEFocusProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
			this.sse = sse;
			this.coroutineFake = coroutineMock;
		}
	}
	public class SSEDefocusProcess: SSESelProcess{
		/* 	Change color, alpha etc from whatever to defocus value
			if its hidden, make it appear gradually
			if any indicator for selection is there, fade it out
		*/
		public SSEDefocusProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
			this.sse = sse;
			this.coroutineFake = coroutineMock;
		}
	}
	public class SSESelectProcess: SSESelProcess{
		/* 	Change color, alpha etc from whatever to select value
			if its hidden, make it appear gradually
			if any indicator for selection is not there, fade it in
		*/
		public SSESelectProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
			this.sse = sse;
			this.coroutineFake = coroutineMock;
		}
	}
}