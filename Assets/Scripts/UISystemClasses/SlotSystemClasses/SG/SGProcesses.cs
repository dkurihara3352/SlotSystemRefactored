using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace UISystem{
	public abstract class SGProcess: UIProcess, ISGProcess{
		protected ISlotGroup sg;
		public SGProcess(ISlotGroup sg, Func<IEnumeratorFake> coroutine): base(coroutine){
		}
	}
	public interface ISGProcess: IUIProcess{
	}
	public interface ISGActProcess: ISGProcess{}
	public class SGResizeProcess: SGProcess, ISGActProcess{
		public SGResizeProcess(ISlotGroup sg, Func<IEnumeratorFake> coroutine): base(sg, coroutine){
		}
	}
	public class SGSelCoroutineRepo: IUISelCoroutineRepo{
		public Func<IEnumeratorFake> DeactivateCoroutine(){
			return SGDeactivateCoroutine;
		}
			IEnumeratorFake SGDeactivateCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> MakeSelectableCoroutine(){
			return SGMakeSelectableCoroutine;
		}
			IEnumeratorFake SGMakeSelectableCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> MakeUnselectableCoroutine(){
			return SGMakeUnselectableCoroutine;
		}
			IEnumeratorFake SGMakeUnselectableCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> SelectCoroutine(){
			return SGSelectCoroutine;
		}
			IEnumeratorFake SGSelectCoroutine(){
				return null;
			}
	}
}
