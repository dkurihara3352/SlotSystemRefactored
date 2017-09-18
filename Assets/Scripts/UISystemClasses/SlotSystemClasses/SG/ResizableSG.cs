using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class ResizableSG : SlotGroup, IResizableSG{
		public ResizableSG(RectTransformFake rectTrans, ISGConstructorArg constArg): base(rectTrans, constArg){
			SetActStateHandler(new ResizableSGActStateEngine(this));
		}
		public IResizableSGActStateEngine ActStateHandler(){
			Debug.Assert(_actStateHandler != null);
			return _actStateHandler;
		}
		void SetActStateHandler(IResizableSGActStateEngine handler){
			_actStateHandler = handler;
		}
		IResizableSGActStateEngine _actStateHandler;
		public void WaitForResize(){
			ActStateHandler().WaitForAction();
		}
		public void Resize(){
			ActStateHandler().Resize();
		}
		public override bool IsFillable(){
			return true;
		}
		public override bool IsPotentialDropTargetFor(ISlottableItem pickedItem){
			if(SSM().SourceSG() == this)
				return true;
			else{
				if(AcceptsItem( pickedItem))
					return true;
				else
					return false;
			}
		}
	}
	public interface IResizableSG: ISlotGroup{
		IResizableSGActStateEngine ActStateHandler();
			void WaitForResize();
			void Resize();
	}
}
