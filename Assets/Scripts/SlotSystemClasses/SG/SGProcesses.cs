using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SGProcess: SSEProcess, ISGProcess{
		public ISlotGroup sg{
			get{return (ISlotGroup)sse;}
			set{}
		}
	}
	public interface ISGProcess: ISSEProcess{
		ISlotGroup sg{get; set;}
	}
		public interface ISGSelProcess: ISGProcess{}
			public class SGGreyinProcess: SGProcess, ISGSelProcess{
				public SGGreyinProcess(ISlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SGGreyoutProcess: SGProcess, ISGSelProcess{
				public SGGreyoutProcess(ISlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SGHighlightProcess: SGProcess, ISGSelProcess{
				public SGHighlightProcess(ISlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SGDehighlightProcess: SGProcess, ISGSelProcess{
				public SGDehighlightProcess(ISlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
			}
		public interface ISGActProcess: ISGProcess{}
			public class SGTransactionProcess: SGProcess, ISGActProcess{
				public SGTransactionProcess(ISlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					sg.ReportTAComp();
				}
			}
}
