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
		public Func<IEnumeratorFake> DeactivateCoroutine(){
			return UIDeactivateCoroutine;
		}
			IEnumeratorFake UIDeactivateCoroutine(){
				if(handler.WasActivated()){
					/*	this coroutine may not be needed, since deactivation is supposed to happen at once
					*/
				}
				return null;
			}
		public Func<IEnumeratorFake> HideCoroutine(){
			return UIHideCoroutine;
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
		public Func<IEnumeratorFake> MakeUnselectableCoroutine(){
			return UIUnselectableCoroutine;
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
		public Func<IEnumeratorFake> MakeSelectableCoroutine(){
			return UISelectableCoroutine;
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
		public Func<IEnumeratorFake> SelectCoroutine(){
			return UISelectCoroutine;
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
		Func<IEnumeratorFake> DeactivateCoroutine();
		Func<IEnumeratorFake> HideCoroutine();
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
		public UIDeactivateProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
	public class UIHideProcess: UISelProcess{
		public UIHideProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
	public class UIMakeSelectableProcess: UISelProcess{
		public UIMakeSelectableProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
	public class UIMakeUnselectableProcess: UISelProcess{
		public UIMakeUnselectableProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
	public class UISelectProcess: UISelProcess{
		public UISelectProcess(System.Func<IEnumeratorFake> coroutineMock): base(coroutineMock){
		}
	}
}