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
        public abstract class SGSelState: SGState{
            public virtual void OnHoverEnterMock(ISlotGroup sg, PointerEventDataFake eventDataMock){
                sg.SetHovered();
            }
            public virtual void OnHoverExitMock(ISlotGroup sg, PointerEventDataFake eventDataMock){

            }
        }
            public class SGDeactivatedState : SGSelState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    sg.SetAndRunSelProcess(null);
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGFocusedState: SGSelState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    SGSelProcess process = null;
                    if(sg.prevSelState == SlotGroup.sgDeactivatedState){
                        process = null;
                        sg.InstantGreyin();
                    }
                    else if(sg.prevSelState == SlotGroup.sgDefocusedState)
                        process = new SGGreyinProcess(sg, sg.greyinCoroutine);
                    else if(sg.prevSelState == SlotGroup.sgSelectedState)
                        process = new SGDehighlightProcess(sg, sg.dehighlightCoroutine);
                    sg.SetAndRunSelProcess(process);
                }	
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGDefocusedState: SGSelState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    SGSelProcess process = null;
                    if(sg.prevSelState == SlotGroup.sgDeactivatedState){
                        process = null;
                        sg.InstantGreyout();
                    }else if(sg.prevSelState == SlotGroup.sgFocusedState)
                        process = new SGGreyoutProcess(sg, sg.greyoutCoroutine);
                    else if(sg.prevSelState == SlotGroup.sgSelectedState)
                        process = new SGGreyoutProcess(sg, sg.greyoutCoroutine);
                    sg.SetAndRunSelProcess(process);
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGSelectedState: SGSelState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    SGSelProcess process = null;
                    if(sg.prevSelState == SlotGroup.sgDeactivatedState){
                        sg.InstantHighlight();
                    }else if(sg.prevSelState == SlotGroup.sgDefocusedState)
                        process = new SGHighlightProcess(sg, sg.highlightCoroutine);
                    else if(sg.prevSelState == SlotGroup.sgFocusedState)
                        process = new SGHighlightProcess(sg, sg.highlightCoroutine);
                    sg.SetAndRunSelProcess(process);
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
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
