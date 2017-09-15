using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UISystem{
	public class UIProcess: IUIProcess{
		public UIProcess(IEnumeratorFake coroutine){
			_coroutineFake = coroutine;
		}
		public IEnumeratorFake Coroutine(){
			return _coroutineFake;
		}
		public void SetCoroutine(IEnumeratorFake coroutine){
			_coroutineFake = coroutine;
		}
			IEnumeratorFake _coroutineFake;
		public virtual bool IsRunning(){
			return _isRunning;
		}
			bool _isRunning;
		public virtual void Start(){
			_isRunning = true;
			Coroutine();
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
	}
	public interface IUIProcess: IEquatable<IUIProcess>{
		bool IsRunning();
		IEnumeratorFake Coroutine();
		void SetCoroutine(IEnumeratorFake coroutine);
		void Start();
		void Stop();
		void Expire();
	}
	/* Engine */
	public interface IUIProcessSwitch<T> where T: IUIProcess{
		T Process();
		void SetAndRunProcess(T process);
	}
	public class UIProcessSwitch<T>: IUIProcessSwitch<T> where T: IUIProcess{
		public virtual T Process(){return _process;}
		void SetProcess(T process){
			_process = process;
		}
		protected T _process;
		public virtual void SetAndRunProcess(T p){
			T process = Process();
			if(p == null || !p.Equals(process)){
				if(process != null)
					process.Stop();
				SetProcess(p);
				process = Process();
				if(process != null)
					process.Start();
			}
		}
	}
	/* repo */
	public class UISelCoroutineRepo: IUISelCoroutineRepo{
		public UISelCoroutineRepo(IUIElement element, IUISelStateEngine engine){
			this.element = element;
			this.engine = engine;
		}
		IUIElement element;
		IUISelStateEngine engine;
		public void InitializeFields(IUIElement element){
			this.element = element;
			this.engine = element.SelStateHandler();
		}
		public IEnumeratorFake DeactivateCoroutine(){
			return UIDeactivateCoroutine();
		}
			IEnumeratorFake UIDeactivateCoroutine(){
				if(engine.WasActivated()){
					/*	this coroutine may not be needed, since deactivation is supposed to happen at once
					*/
				}
				return null;
			}
		public IEnumeratorFake HideCoroutine(){
			return UIHideCoroutine();
		}
			IEnumeratorFake UIHideCoroutine(){
				if(engine.WasDeactivated()){
					/*	hide instantly and break
					*/
				}else if(engine.WasShown()){
					/*	Decrease scale or alpha to make it disappear gradually
					*/
				}
				return null;
			}
		public IEnumeratorFake MakeUnselectableCoroutine(){
			return UIUnselectableCoroutine();
		}
			IEnumeratorFake UIUnselectableCoroutine(){
				if(engine.WasDeactivated()){
					/*	show & turn unselectable instantly and break
					*/
				}else if(engine.WasHidden()){
					/*	show gradually
					*/
				}else{
					if(engine.WasSelectable()){
						/*	turn from selectable color to unselectable color
						*/
					}else if(engine.WasSelected()){
						/*	turn from selected color to unselectable color
						*/
					}
				}
				return null;
			}
		public IEnumeratorFake MakeSelectableCoroutine(){
			return UISelectableCoroutine();
		}
			IEnumeratorFake UISelectableCoroutine(){
				if(engine.WasDeactivated()){
					/*	show & turn selectable instantly and break
					*/
				}else if(engine.WasHidden()){
					/*	show gradually
					*/
				}else{
					if(engine.WasUnselectable()){
						/*	turn from unselectable color to selectable color
						*/
					}else if(engine.WasSelected()){
						/*	turn from selected color to selectable color
						*/
					}
				}
				return null;
			}
		public IEnumeratorFake SelectCoroutine(){
			return UISelectCoroutine();
		}
			IEnumeratorFake UISelectCoroutine(){
				if(engine.WasDeactivated()){
					/*	show & turn selected instantly and break
					*/
				}else if(engine.WasHidden()){
					/*	show gradually
					*/
				}else{
					if(engine.WasUnselectable()){
						/*	turn from unselectable color to selected color
						*/
					}else if(engine.WasSelectable()){
						/*	turn from selectable color to selected color
						*/
					}
				}
				return null;
			}
	}
	public interface IUISelCoroutineRepo{
		IEnumeratorFake DeactivateCoroutine();
		IEnumeratorFake HideCoroutine();
		IEnumeratorFake MakeUnselectableCoroutine();
		IEnumeratorFake MakeSelectableCoroutine();
		IEnumeratorFake SelectCoroutine();
	}
	/* SelProces */
	public interface IUISelProcess: IUIProcess{
	}
	public class UIDeactivateProcess: UIProcess, IUISelProcess{
		public UIDeactivateProcess(IEnumeratorFake coroutineMock): base(coroutineMock){
		}
	}
	public class UIHideProcess: UIProcess, IUISelProcess{
		public UIHideProcess(IEnumeratorFake coroutineMock): base(coroutineMock){
		}
	}
	public class UIMakeSelectableProcess: UIProcess, IUISelProcess{
		public UIMakeSelectableProcess(IEnumeratorFake coroutineMock): base(coroutineMock){
		}
	}
	public class UIMakeUnselectableProcess: UIProcess, IUISelProcess{
		public UIMakeUnselectableProcess(IEnumeratorFake coroutineMock): base(coroutineMock){
		}
	}
	public class UISelectProcess: UIProcess, IUISelProcess{
		public UISelectProcess(IEnumeratorFake coroutineMock): base(coroutineMock){
		}
	}
	/* TapProcess */
	public interface ITapStateProcess: IUIProcess{
	}
	public abstract class TapStateProcess: UIProcess, ITapStateProcess{
		protected ITapStateEngine engine;
		public TapStateProcess(IEnumeratorFake coroutine, ITapStateEngine engine): base(coroutine){
			this.engine = engine;
		}
	}
	public class UIWaitForTapPointerDownProcess: TapStateProcess{
		public UIWaitForTapPointerDownProcess(IEnumeratorFake coroutine, ITapStateEngine engine): base(coroutine, engine){}
	}
	public class UIWaitForTapTimerUpProcess: TapStateProcess{
		public UIWaitForTapTimerUpProcess(IEnumeratorFake coroutine, ITapStateEngine engine): base(coroutine, engine){}
		public override void Expire(){
			engine.WaitForTapPointerUp();
		}
	}
	public class UIWaitForTapPointerUpProcess: TapStateProcess{
		public UIWaitForTapPointerUpProcess(IEnumeratorFake coroutine, ITapStateEngine engine): base(coroutine, engine){}
	}
	public class UITapProcess: TapStateProcess{
		public UITapProcess(IEnumeratorFake coroutine, ITapStateEngine engine): base(coroutine, engine){
		}
		public override void Expire(){
			engine.WaitForTapPointerDown();
		}
	}
}