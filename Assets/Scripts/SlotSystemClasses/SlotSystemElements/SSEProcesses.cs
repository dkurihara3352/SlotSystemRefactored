using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public class SSEProcess: ISSEProcess{
		public virtual bool IsRunning(){
			return _isRunning;
		}
			bool _isRunning;
		public Func<IEnumeratorFake> GetCoroutine(){
			return _coroutineFake;
		}
		public void SetCoroutine(Func<IEnumeratorFake> coroutine){
			_coroutineFake = coroutine;
		}
			Func<IEnumeratorFake> _coroutineFake;
		public virtual void Start(){
			_isRunning = true;
			_coroutineFake();
		}
		public virtual void Stop(){ 
			if(IsRunning())
				_isRunning = false; 
		}
		public virtual void Expire(){
			if(IsRunning())
				_isRunning = false; 
		}
		public bool Equals(ISSEProcess other){
			if(other != null)
				return this.GetType().Equals(other.GetType());
			else return false;
		}
		public SSEProcess(Func<IEnumeratorFake> coroutine){
			_coroutineFake = coroutine;
		}
	}
	public interface ISSEProcess: IEquatable<ISSEProcess>{
		bool IsRunning();
		Func<IEnumeratorFake> GetCoroutine();
		void SetCoroutine(Func<IEnumeratorFake> coroutine);
		void Start();
		void Stop();
		void Expire();
	}
	/* Engine */
	public class SSEProcessEngine<T>: ISSEProcessEngine<T> where T: ISSEProcess{
		public SSEProcessEngine(){}
		public SSEProcessEngine(T from){
			SetProcess(from);
		}
		public virtual T GetProcess(){return _process;}
		void SetProcess(T process){
			_process = process;
		}
		protected T _process;
		public virtual void SetAndRunProcess(T p){
			T process = GetProcess();
			if(p == null || !p.Equals(process)){
				if(process != null)
					process.Stop();
				SetProcess(p);
				process = GetProcess();
				if(process != null)
					process.Start();
			}
		}
	}
	public interface ISSEProcessEngine<T> where T: ISSEProcess{
		T GetProcess();
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
		public SSESelProcess(System.Func<IEnumeratorFake> coroutine): base(coroutine){
		}
	}
	public interface ISSESelProcess: ISSEProcess{
	}
	public class SSEDeactivateProcess: SSESelProcess{
		/* 	Change color, alpha etc from whatever to deactivated value (probably make it disappear)
			if any indicator for selection is there, fade it out
		*/
		public SSEDeactivateProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
	public class SSEFocusProcess: SSESelProcess{
		/* 	Change color, alpha etc from whatever to focus value
			if its hidden, make it appear gradually
			if any indicator for selection is there, fade it out
		*/
		public SSEFocusProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
	public class SSEDefocusProcess: SSESelProcess{
		/* 	Change color, alpha etc from whatever to defocus value
			if its hidden, make it appear gradually
			if any indicator for selection is there, fade it out
		*/
		public SSEDefocusProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
	public class SSESelectProcess: SSESelProcess{
		/* 	Change color, alpha etc from whatever to select value
			if its hidden, make it appear gradually
			if any indicator for selection is not there, fade it in
		*/
		public SSESelectProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
}