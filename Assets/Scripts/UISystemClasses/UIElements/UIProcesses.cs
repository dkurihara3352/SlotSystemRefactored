using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UISystem{
	public class UIProcess: IUIProcess{
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
		public bool Equals(IUIProcess other){
			if(other != null)
				return this.GetType().Equals(other.GetType());
			else return false;
		}
		public UIProcess(Func<IEnumeratorFake> coroutine){
			_coroutineFake = coroutine;
		}
	}
	public interface IUIProcess: IEquatable<IUIProcess>{
		bool IsRunning();
		Func<IEnumeratorFake> GetCoroutine();
		void SetCoroutine(Func<IEnumeratorFake> coroutine);
		void Start();
		void Stop();
		void Expire();
	}
	/* Engine */
	public class UIProcessEngine<T>: IUIProcessEngine<T> where T: IUIProcess{
		public UIProcessEngine(){}
		public UIProcessEngine(T from){
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
	public interface IUIProcessEngine<T> where T: IUIProcess{
		T GetProcess();
		void SetAndRunProcess(T process);
	}
	/* Factory */
	public class UISelCoroutineRepo: IUISelCoroutineRepo{
		public Func<IEnumeratorFake> DeactivateCoroutine(){
			return UIDeactivateCoroutine;
		}
			IEnumeratorFake UIDeactivateCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> MakeUnselectableCoroutine(){
			return UIUnselectableCoroutine;
		}
			IEnumeratorFake UIUnselectableCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> MakeSelectableCoroutine(){
			return UISelectableCoroutine;
		}
			IEnumeratorFake UISelectableCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> SelectCoroutine(){
			return UISelectCoroutine;
		}
			IEnumeratorFake UISelectCoroutine(){
				return null;
			}
	}
	public interface IUISelCoroutineRepo{
		Func<IEnumeratorFake> DeactivateCoroutine();
		Func<IEnumeratorFake> MakeUnselectableCoroutine();
		Func<IEnumeratorFake> MakeSelectableCoroutine();
		Func<IEnumeratorFake> SelectCoroutine();
	}
	/* SelProces */
	public abstract class UISelProcess: UIProcess, IUISelProcess{
		public UISelProcess(System.Func<IEnumeratorFake> coroutine): base(coroutine){
		}
	}
	public interface IUISelProcess: IUIProcess{
	}
	public class UIDeactivateProcess: UISelProcess{
		/* 	Change color, alpha etc from whatever to deactivated value (probably make it disappear)
			if any indicator for selection is there, fade it out
		*/
		public UIDeactivateProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
	public class UIFocusProcess: UISelProcess{
		/* 	Change color, alpha etc from whatever to focus value
			if its hidden, make it appear gradually
			if any indicator for selection is there, fade it out
		*/
		public UIFocusProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
	public class UIDefocusProcess: UISelProcess{
		/* 	Change color, alpha etc from whatever to defocus value
			if its hidden, make it appear gradually
			if any indicator for selection is there, fade it out
		*/
		public UIDefocusProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
	public class UISelectProcess: UISelProcess{
		/* 	Change color, alpha etc from whatever to select value
			if its hidden, make it appear gradually
			if any indicator for selection is not there, fade it in
		*/
		public UISelectProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
}