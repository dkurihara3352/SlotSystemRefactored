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
	public class UIProcessEngine<T>: IUIProcessEngine<T> where T: IUIProcess{
		public UIProcessEngine(){}
		public UIProcessEngine(T from){
			SetProcess(from);
		}
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
	public interface IUIProcessEngine<T> where T: IUIProcess{
		T Process();
		void SetAndRunProcess(T process);
	}
	/* repo */
	public class UISelCoroutineRepo: IUISelCoroutineRepo{
		public UISelCoroutineRepo(IUIElement element, IUISelStateHandler handler){
			this.element = element;
			this.handler = handler;
		}
		IUIElement element;
		IUISelStateHandler handler;
		public void InitializeFields(IUIElement element){
			this.element = element;
			this.handler = element.SelStateHandler();
		}
		public IEnumeratorFake DeactivateCoroutine(){
			return UIDeactivateCoroutine();
		}
			IEnumeratorFake UIDeactivateCoroutine(){
				if(handler.WasActivated()){
					/*	this coroutine may not be needed, since deactivation is supposed to happen at once
					*/
				}
				return null;
			}
		public IEnumeratorFake HideCoroutine(){
			return UIHideCoroutine();
		}
			IEnumeratorFake UIHideCoroutine(){
				if(handler.WasDeactivated()){
					/*	hide instantly and break
					*/
				}else if(handler.WasShown()){
					/*	Decrease scale or alpha to make it disappear gradually
					*/
				}
				return null;
			}
		public IEnumeratorFake MakeUnselectableCoroutine(){
			return UIUnselectableCoroutine();
		}
			IEnumeratorFake UIUnselectableCoroutine(){
				if(handler.WasDeactivated()){
					/*	show & turn unselectable instantly and break
					*/
				}else if(handler.WasHidden()){
					/*	show gradually
					*/
				}else{
					if(handler.WasSelectable()){
						/*	turn from selectable color to unselectable color
						*/
					}else if(handler.WasSelected()){
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
				if(handler.WasDeactivated()){
					/*	show & turn selectable instantly and break
					*/
				}else if(handler.WasHidden()){
					/*	show gradually
					*/
				}else{
					if(handler.WasUnselectable()){
						/*	turn from unselectable color to selectable color
						*/
					}else if(handler.WasSelected()){
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
				if(handler.WasDeactivated()){
					/*	show & turn selected instantly and break
					*/
				}else if(handler.WasHidden()){
					/*	show gradually
					*/
				}else{
					if(handler.WasUnselectable()){
						/*	turn from unselectable color to selected color
						*/
					}else if(handler.WasSelectable()){
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
		protected ITapStateHandler handler;
		public TapStateProcess(IEnumeratorFake coroutine, ITapStateHandler handler): base(coroutine){
			this.handler = handler;
		}
	}
	public class UIWaitForTapPointerDownProcess: TapStateProcess{
		public UIWaitForTapPointerDownProcess(IEnumeratorFake coroutine, ITapStateHandler handler): base(coroutine, handler){}
	}
	public class UIWaitForTapTimerUpProcess: TapStateProcess{
		public UIWaitForTapTimerUpProcess(IEnumeratorFake coroutine, ITapStateHandler handler): base(coroutine, handler){}
		public override void Expire(){
			handler.WaitForTapPointerUp();
		}
	}
	public class UIWaitForTapPointerUpProcess: TapStateProcess{
		public UIWaitForTapPointerUpProcess(IEnumeratorFake coroutine, ITapStateHandler handler): base(coroutine, handler){}
	}
	public class UITapProcess: TapStateProcess{
		public UITapProcess(IEnumeratorFake coroutine, ITapStateHandler handler): base(coroutine, handler){
		}
		public override void Expire(){
			handler.WaitForTapPointerDown();
		}
	}
}