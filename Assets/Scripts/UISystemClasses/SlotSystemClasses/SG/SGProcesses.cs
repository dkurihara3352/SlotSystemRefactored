using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace UISystem{
	public abstract class SGProcess: UIProcess, ISGProcess{
		protected IResizableSG sg;
		public SGProcess(IResizableSG sg, IEnumeratorFake coroutine): base(coroutine){
		}
	}
	public interface ISGProcess: IUIProcess{
	}
	public interface ISGActProcess: ISGProcess{}
	public class SGResizeProcess: SGProcess, ISGActProcess{
		public SGResizeProcess(IResizableSG sg, IEnumeratorFake coroutine): base(sg, coroutine){
		}
	}
}
