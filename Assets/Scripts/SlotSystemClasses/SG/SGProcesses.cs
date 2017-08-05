using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SlotSystem{
	public abstract class SGProcess: SSEProcess, ISGProcess{
		public SGProcess(ISSESelStateHandler handler, Func<IEnumeratorFake> coroutine): base(handler, coroutine){
		}
		public ISlotGroup sg{
			get{return (ISlotGroup)handler;}
		}
	}
	public interface ISGProcess: ISSEProcess{
		ISlotGroup sg{get;}
	}
		public interface ISGSelProcess: ISGProcess{}
			public class SGGreyinProcess: SGProcess, ISGSelProcess{
				public SGGreyinProcess(ISlotGroup sg, Func<IEnumeratorFake> coroutine): base(sg, coroutine){
				}
			}
			public class SGGreyoutProcess: SGProcess, ISGSelProcess{
				public SGGreyoutProcess(ISlotGroup sg, Func<IEnumeratorFake> coroutine): base(sg, coroutine){
				}
			}
			public class SGHighlightProcess: SGProcess, ISGSelProcess{
				public SGHighlightProcess(ISlotGroup sg, Func<IEnumeratorFake> coroutine): base(sg, coroutine){
				}
			}
			public class SGDehighlightProcess: SGProcess, ISGSelProcess{
				public SGDehighlightProcess(ISlotGroup sg, Func<IEnumeratorFake> coroutine): base(sg, coroutine){
				}
			}
		public interface ISGActProcess: ISGProcess{}
			public class SGTransactionProcess: SGProcess, ISGActProcess{
				public SGTransactionProcess(ISlotGroup sg, Func<IEnumeratorFake> coroutine): base(sg, coroutine){
				}
				public override void Expire(){
					base.Expire();
					sg.ReportTAComp();
				}
			}
}
