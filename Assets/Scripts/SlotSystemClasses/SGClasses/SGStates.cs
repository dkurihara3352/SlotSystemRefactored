using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class SGState: SSEState{
		protected ISlotGroup sg{
			get{
				return (ISlotGroup)sse;
			}
		}
	}
        public abstract class SGActState: SGState{}
            public class SGWaitForActionState: SGActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    sg.SetAndRunActProcess(null);
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGRevertState: SGActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    sg.UpdateToRevert();
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGReorderState: SGActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    sg.ReorderAndUpdateSBs();
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGSortState: SGActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    sg.SortAndUpdateSBs();
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGFillState: SGActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    sg.FillAndUpdateSBs();
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGSwapState: SGActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    sg.SwapAndUpdateSBs();
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGAddState: SGActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    sg.AddAndUpdateSBs();
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGRemoveState: SGActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    sg.RemoveAndUpdateSBs();
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
}
