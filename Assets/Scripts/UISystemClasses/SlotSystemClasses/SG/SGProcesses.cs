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
			public class SGTransactionProcess: SGProcess, ISGActProcess{
				ISGTransactionHandler sgTAHandler;
				public SGTransactionProcess(ISlotGroup sg, Func<IEnumeratorFake> coroutine): base(sg, coroutine){
					sgTAHandler = sg.GetSGTAHandler();
				}
				public override void Expire(){
					base.Expire();
					sgTAHandler.ReportTAComp();
				}
			}
}
