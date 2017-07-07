using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SGProcess: AbsSSEProcess{
		public SlotGroup sg{
			get{return (SlotGroup)sse;}
		}
	}
		public abstract class SGSelProcess: SGProcess{}
			public class SGGreyinProcess: SGSelProcess{
				public SGGreyinProcess(SlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SGGreyoutProcess: SGSelProcess{
				public SGGreyoutProcess(SlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SGHighlightProcess: SGSelProcess{
				public SGHighlightProcess(SlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SGDehighlightProcess: SGSelProcess{
				public SGDehighlightProcess(SlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
			}
		public abstract class SGActProcess: SGProcess{}
			public class SGTransactionProcess: SGActProcess{
				public SGTransactionProcess(SlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
					sse = sg;
					this.coroutineFake = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					sg.ssm.AcceptSGTAComp(sg);
				}
			}
}
