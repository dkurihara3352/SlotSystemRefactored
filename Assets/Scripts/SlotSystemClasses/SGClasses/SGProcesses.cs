using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SGProcess: AbsSSEProcess{
		public ISlotGroup sg{
			get{return (ISlotGroup)sse;}
		}
	}
		public abstract class SGSelProcess: SGProcess{}
			public class SGGreyinProcess: SGSelProcess{
				public SGGreyinProcess(ISlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SGGreyoutProcess: SGSelProcess{
				public SGGreyoutProcess(ISlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SGHighlightProcess: SGSelProcess{
				public SGHighlightProcess(ISlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SGDehighlightProcess: SGSelProcess{
				public SGDehighlightProcess(ISlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
			}
		public abstract class SGActProcess: SGProcess{}
			public class SGTransactionProcess: SGActProcess{
				public SGTransactionProcess(ISlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					sg.ssm.AcceptSGTAComp(sg);
				}
			}
}
