using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class ResizableSG : SlotGroup, IResizableSG{
		public ResizableSG(RectTransformFake rectTrans, ISGConstructorArg constArg): base(rectTrans, constArg){
			SetActStateHandler(new ResizableSGActStateHandler(this));
		}
		public IResizableSGActStateHandler ActStateHandler(){
			Debug.Assert(_actStateHandler != null);
			return _actStateHandler;
		}
		void SetActStateHandler(IResizableSGActStateHandler handler){
			_actStateHandler = handler;
		}
		IResizableSGActStateHandler _actStateHandler;
		public void WaitForResize(){
			ActStateHandler().WaitForAction();
		}
		public void Resize(){
			ActStateHandler().Resize();
		}
		public override bool IsReceivable(){
			return true;
		}
	}
	public interface IResizableSG: ISlotGroup{
		IResizableSGActStateHandler ActStateHandler();
			void WaitForResize();
			void Resize();
	}
}
