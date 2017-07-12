using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class SBState: SSEState{
		protected ISlottable sb{
			get{
				return (ISlottable)sse;
			}
		}
	}
        public abstract class SBSelState: SBState{
            public virtual void OnHoverEnterMock(ISlottable sb, PointerEventDataFake eventDataMock){
                sb.SetHovered();
            }
            public virtual void OnHoverExitMock(ISlottable sb, PointerEventDataFake eventDataMock){
            }
        }
            public class SBDeactivatedState: SBSelState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    sb.SetAndRunSelProcess(null);
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SBFocusedState: SBSelState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    ISBSelProcess process = null;
                    if(sb.prevSelState == Slottable.sbDeactivatedState){
                        sb.InstantGreyin();
                    }else if(sb.prevSelState == Slottable.sbDefocusedState){
                        process = new SBGreyinProcess(sb, sb.greyinCoroutine);
                    }else if(sb.prevSelState == Slottable.sbSelectedState){
                        process = new SBDehighlightProcess(sb, sb.dehighlightCoroutine);
                    }
                    sb.SetAndRunSelProcess(process);
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SBDefocusedState: SBSelState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    ISBSelProcess process = null;
                    if(sb.prevSelState == Slottable.sbDeactivatedState){
                        sb.InstantGreyout();
                        process = null;
                    }else if(sb.prevSelState == Slottable.sbFocusedState){
                        process = new SBGreyoutProcess(sb, sb.greyoutCoroutine);
                    }else if(sb.prevSelState == Slottable.sbSelectedState){
                        process = new SBGreyoutProcess(sb, sb.greyoutCoroutine);
                    }
                    sb.SetAndRunSelProcess(process);
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SBSelectedState: SBSelState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    ISBSelProcess process = null;
                    if(sb.prevSelState == Slottable.sbDeactivatedState){
                        sb.InstantHighlight();
                    }else if(sb.prevSelState == Slottable.sbDefocusedState){
                        process = new SBHighlightProcess(sb, sb.highlightCoroutine);
                    }else if(sb.prevSelState == Slottable.sbFocusedState){
                        process = new SBHighlightProcess(sb, sb.highlightCoroutine);
                    }
                    sb.SetAndRunSelProcess(process);
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
        public abstract class SBActState: SBState{
            public abstract void OnPointerDownMock(ISlottable sb, PointerEventDataFake eventDataMock);
            public abstract void OnPointerUpMock(ISlottable sb, PointerEventDataFake eventDataMock);
            public abstract void OnDeselectedMock(ISlottable sb, PointerEventDataFake eventDataMock);
            public abstract void OnEndDragMock(ISlottable sb, PointerEventDataFake eventDataMock);
        }
            public class WaitForActionState: SBActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    ISBActProcess process = null;
                    sb.SetAndRunActProcess(process);
                }
                public override void OnPointerUpMock(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.Tap();
                    sb.Reset();
                    sb.Defocus();
                }
                public override void OnEndDragMock(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.Reset();
                    sb.Defocus();
                }
                public override void OnPointerDownMock(ISlottable sb, PointerEventDataFake eventDataMock){
                    if(sb.isFocused){
                        sb.SetSelState(Slottable.sbSelectedState);
                        sb.SetActState(Slottable.waitForPickUpState);
                    }
                    else
                        sb.SetActState(Slottable.waitForPointerUpState);
                }
                public override void OnDeselectedMock(ISlottable sb, PointerEventDataFake eventDataMock){}
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class WaitForPickUpState: SBActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    ISBActProcess wfpuProcess = new WaitForPickUpProcess(sb, sb.WaitForPickUpCoroutine);
                    sb.SetAndRunActProcess(wfpuProcess);
                }
                public override void OnPointerUpMock(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.SetActState(Slottable.waitForNextTouchState);
                }
                public override void OnEndDragMock(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.Reset();
                    sb.Focus();
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
                public override void OnDeselectedMock(ISlottable slottable, PointerEventDataFake eventDataMock){}
                public override void OnPointerDownMock(ISlottable slottable, PointerEventDataFake eventDataMock){}
            }
            public class WaitForPointerUpState: SBActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    ISBActProcess wfPtuProcess = new WaitForPointerUpProcess(sb, sb.WaitForPointerUpCoroutine);
                    sb.SetAndRunActProcess(wfPtuProcess);
                }
                public override void OnPointerUpMock(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.Tap();
                    sb.Reset();
                    sb.Defocus();
                }
                public override void OnEndDragMock(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.Reset();
                    sb.Defocus();
                }
                public override void OnDeselectedMock(ISlottable sb, PointerEventDataFake eventDataMock){}
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
                public override void OnPointerDownMock(ISlottable sb, PointerEventDataFake eventDataMock){}
            }
            public class WaitForNextTouchState: SBActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    ISBActProcess wfntProcess = new WaitForNextTouchProcess(sb, sb.WaitForNextTouchCoroutine);
                    sb.SetAndRunActProcess(wfntProcess);
                }
                public override void OnPointerDownMock(ISlottable sb, PointerEventDataFake eventDataMock){
                    if(!sb.isPickedUp)
                        sb.PickUp();
                    else{
                        sb.SetActState(Slottable.pickedUpState);
                        sb.Increment();
                    }
                }
                public override void OnDeselectedMock(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.Reset();
                    sb.Focus();
                }
                /*	undef	*/
                    public override void ExitState(IStateHandler sh){
                        base.ExitState(sh);
                    }
                    public override void OnPointerUpMock(ISlottable sb, PointerEventDataFake eventDataMock){}
                    public override void OnEndDragMock(ISlottable sb, PointerEventDataFake eventDataMock){}
            }
            public class PickedUpState: SBActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    sb.SetPickedSB();
                    sb.SetSSMActState(SlotSystemManager.ssmProbingState);
                    sb.SetDIcon1();
                    sb.CreateTAResult();
                    sb.OnHoverEnterMock();
                    sb.UpdateTA();
                    ISBActProcess pickedUpProcess = new SBPickedUpProcess(sb, sb.PickUpCoroutine);
                    sb.SetAndRunActProcess(pickedUpProcess);
                }
                public override void OnDeselectedMock(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.Reset();
                    sb.Focus();
                }
                public override void OnPointerUpMock(ISlottable sb, PointerEventDataFake eventDataMock){
                    if(sb.isHovered && sb.isStackable)
                        sb.SetActState(Slottable.waitForNextTouchState);
                    else
                        sb.ExecuteTransaction();
                }
                public override void OnEndDragMock(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.ExecuteTransaction();
                }
                /*	undef	*/
                    public override void ExitState(IStateHandler sh){
                        base.ExitState(sh);
                    }
                    public override void OnPointerDownMock(ISlottable sb, PointerEventDataFake eventDataMock){}
            }
            public class SBMoveWithinState: SBActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    SBMoveWithinProcess process = new SBMoveWithinProcess(sb, sb.MoveWithinCoroutine);
                    sb.SetAndRunActProcess(process);
                }
                /*	*/
                    public override void ExitState(IStateHandler sh){
                        base.ExitState(sh);
                    }
                    public override void OnPointerDownMock(ISlottable sb, PointerEventDataFake eventDataMock){}
                    public override void OnPointerUpMock(ISlottable sb, PointerEventDataFake eventDataMock){}
                    public override void OnDeselectedMock(ISlottable sb, PointerEventDataFake eventDataMock){}
                    public override void OnEndDragMock(ISlottable sb, PointerEventDataFake eventDataMock){}
            }
            public class SBAddedState: SBActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    SBAddProcess process = new SBAddProcess(sb, sb.AddCorouine);
                    sb.SetAndRunActProcess(process);
                }
                /*	*/
                    public override void ExitState(IStateHandler sh){
                        base.ExitState(sh);
                    }
                    public override void OnPointerDownMock(ISlottable sb, PointerEventDataFake eventDataMock){}
                    public override void OnPointerUpMock(ISlottable sb, PointerEventDataFake eventDataMock){}
                    public override void OnDeselectedMock(ISlottable sb, PointerEventDataFake eventDataMock){}
                    public override void OnEndDragMock(ISlottable sb, PointerEventDataFake eventDataMock){}
            }
            public class SBRemovedState: SBActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    SBRemoveProcess process = new SBRemoveProcess(sb, sb.RemoveCoroutine);
                    sb.SetAndRunActProcess(process);
                }
                /*	*/
                    public override void ExitState(IStateHandler sh){
                        base.ExitState(sh);
                    }
                    public override void OnPointerDownMock(ISlottable sb, PointerEventDataFake eventDataMock){}
                    public override void OnPointerUpMock(ISlottable sb, PointerEventDataFake eventDataMock){}
                    public override void OnDeselectedMock(ISlottable sb, PointerEventDataFake eventDataMock){}
                    public override void OnEndDragMock(ISlottable sb, PointerEventDataFake eventDataMock){}
            }
        public abstract class SBEqpState: SBState{}
            public class SBEquippedState: SBEqpState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(sb.isPool){
                        if(sb.prevEqpState != null && sb.prevEqpState == Slottable.unequippedState){
                            ISBEqpProcess process = new SBEquipProcess(sb, sb.EquipCoroutine);
                            sb.SetAndRunEqpProcess(process);
                        }
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SBUnequippedState: SBEqpState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(sb.prevEqpState == null || sb.prevEqpState == Slottable.unequippedState){
                        /*	when initialized	*/
                        return;
                    }
                    if(sb.isPool){
                        if(sb.prevEqpState != null && sb.prevEqpState == Slottable.equippedState){
                            ISBEqpProcess process = new SBUnequipProcess(sb, sb.UnequipCoroutine);
                            sb.SetAndRunEqpProcess(process);
                        }
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
        public abstract class SBMrkState: SBState{}
            public class SBMarkedState: SBMrkState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(sb.isPool){
                        if(sb.prevMrkState != null && sb.prevMrkState == Slottable.unmarkedState){
                            ISBMrkProcess process = new SBMarkProcess(sb, sb.markCoroutine);
                            sb.SetAndRunMrkProcess(process);
                        }
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SBUnmarkedState: SBMrkState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(sb.prevMrkState == null || sb.prevMrkState == Slottable.unmarkedState){
                        /*	when initialized	*/
                        return;
                    }
                    if(sb.isPool){
                        if(sb.prevMrkState != null && sb.prevMrkState == Slottable.markedState){
                            ISBMrkProcess process = new SBUnmarkProcess(sb, sb.unmarkCoroutine);
                            sb.SetAndRunMrkProcess(process);
                        }
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
}
