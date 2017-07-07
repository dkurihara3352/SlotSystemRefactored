using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class SGState: SSEState{
		protected SlotGroup sg{
			get{
				return (SlotGroup)sse;
			}
		}
	}
        public abstract class SGSelState: SGState{
            public virtual void OnHoverEnterMock(SlotGroup sg, PointerEventDataFake eventDataMock){
                sg.ssm.SetHovered(sg);
            }
            public virtual void OnHoverExitMock(SlotGroup sg, PointerEventDataFake eventDataMock){

            }
        }
            public class SGDeactivatedState : SGSelState{
                public override void EnterState(StateHandler sh){
                    base.EnterState(sh);
                    sg.SetAndRunSelProcess(null);
                }
                public override void ExitState(StateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGFocusedState: SGSelState{
                public override void EnterState(StateHandler sh){
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
                public override void ExitState(StateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGDefocusedState: SGSelState{
                public override void EnterState(StateHandler sh){
                    base.EnterState(sh);
                    SGSelProcess process = null;
                    if(sg.prevSelState == SlotGroup.sgDeactivatedState){
                        process = null;
                        sg.InstantGreyout();
                    }else if(sg.prevSelState == SlotGroup.sgFocusedState)
                        process = new SGGreyoutProcess(sg, sg.greyoutCoroutine);
                    else if(sg.prevSelState == SlotGroup.sgSelectedState)
                        process = new SGDehighlightProcess(sg, sg.greyoutCoroutine);
                    sg.SetAndRunSelProcess(process);
                }
                public override void ExitState(StateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGSelectedState: SGSelState{
                public override void EnterState(StateHandler sh){
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
                public override void ExitState(StateHandler sh){
                    base.ExitState(sh);
                }
            
            }
        public abstract class SGActState: SGState{}
            public class SGWaitForActionState: SGActState{
                public override void EnterState(StateHandler sh){
                    base.EnterState(sh);
                    sg.SetAndRunActProcess(null);
                }
                public override void ExitState(StateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGRevertState: SGActState{
                public override void EnterState(StateHandler sh){
                    base.EnterState(sh);
                    sg.UpdateSBs(new List<Slottable>(sg.toList));
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(StateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGReorderState: SGActState{
                public override void EnterState(StateHandler sh){
                    base.EnterState(sh);
                    Slottable sb1 = sg.ssm.pickedSB;
                    Slottable sb2 = sg.ssm.targetSB;
                    List<Slottable> newSBs = new List<Slottable>(sg.toList);
                    newSBs.Reorder(sb1, sb2);
                    sg.UpdateSBs(newSBs);
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(StateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGSortState: SGActState{
                public override void EnterState(StateHandler sh){
                    base.EnterState(sh);
                    List<Slottable> newSBs = new List<Slottable>(sg.toList);
                    int origCount = newSBs.Count;
                    sg.Sorter.TrimAndOrderSBs(ref newSBs);
                    if(!sg.isExpandable){
                        while(newSBs.Count <origCount){
                            newSBs.Add(null);
                        }
                    }
                    sg.UpdateSBs(newSBs);
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(StateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGFillState: SGActState{
                public override void EnterState(StateHandler sh){
                    base.EnterState(sh);
                    Slottable added;
                        if(sg.ssm.transaction.sg1 == sg)
                            added = null;
                        else
                            added = sg.ssm.pickedSB;
                    Slottable removed;
                        if(sg.ssm.transaction.sg1 == sg)
                            removed = sg.ssm.pickedSB;
                        else
                            removed = null;

                    List<Slottable> newSBs = new List<Slottable>(sg.toList);
                    int origCount = newSBs.Count;
                    if(!sg.isPool){
                        if(added != null){
                            GameObject newSBGO = new GameObject("newSBGO");
                            Slottable newSB = newSBGO.AddComponent<Slottable>();
                            newSB.Initialize(added.itemInst);
                            newSB.SetSSM(sg.ssm);
                            newSB.Defocus();
                            newSB.SetEqpState(Slottable.unequippedState);
                            SlotSystemUtil.AddInEmptyOrConcat(ref newSBs, newSB);
                        }
                        if(removed != null){
                            Slottable rem = null;
                            foreach(Slottable sb in newSBs){
                                if(sb != null){
                                    if(sb.itemInst == removed.itemInst)
                                        rem = sb;
                                }
                            }
                            newSBs[newSBs.IndexOf(rem)] = null;
                        }
                    }
                    if(sg.isAutoSort){
                        sg.Sorter.TrimAndOrderSBs(ref newSBs);
                    }
                    if(!sg.isExpandable){
                        while(newSBs.Count <origCount){
                            newSBs.Add(null);
                        }
                    }
                    sg.UpdateSBs(newSBs);
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(StateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGSwapState: SGActState{
                public override void EnterState(StateHandler sh){
                    base.EnterState(sh);
                    Slottable added;
                        if(sg.ssm.transaction.sg1 == sg)
                            added = sg.ssm.transaction.targetSB;
                        else
                            added = sg.ssm.pickedSB;
                    Slottable removed;
                        if(sg.ssm.transaction.sg1 == sg)
                            removed = sg.ssm.pickedSB;
                        else
                            removed = sg.ssm.transaction.targetSB;
                    List<Slottable> newSBs = new List<Slottable>(sg.toList);
                    int origCount = newSBs.Count;
                    if(!sg.isPool){
                        GameObject newSBGO = new GameObject("newSBGO");
                        Slottable newSB = newSBGO.AddComponent<Slottable>();
                        newSB.Initialize(added.itemInst);
                        newSB.SetSSM(sg.ssm);
                        newSB.SetEqpState(Slottable.unequippedState);
                        newSB.Defocus();
                        newSBs[newSBs.IndexOf(removed)] = newSB;
                    }
                    if(sg.isAutoSort){
                        sg.Sorter.TrimAndOrderSBs(ref newSBs);
                        if(!sg.isExpandable){
                            while(newSBs.Count <origCount){
                                newSBs.Add(null);
                            }
                        }
                    }
                    sg.UpdateSBs(newSBs);
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(StateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGAddState: SGActState{
                public override void EnterState(StateHandler sh){
                    base.EnterState(sh);
                    List<InventoryItemInstance> cache = sg.ssm.transaction.moved;
                    List<Slottable> newSBs = sg.toList;
                    int origCount = newSBs.Count;
                    foreach(InventoryItemInstance itemInst in cache){
                        bool found = false;
                        foreach(Slottable sb in newSBs){
                            if(sb!= null){
                                if(sb.itemInst == itemInst){
                                    if(itemInst.Item.IsStackable){
                                        sb.itemInst.Quantity += itemInst.Quantity;
                                        found = true;
                                    }
                                }
                            }
                        }
                        if(!found){
                            GameObject newSBSG = new GameObject("newSBSG");
                            Slottable newSB = newSBSG.AddComponent<Slottable>();
                            newSB.Initialize(itemInst);
                            newSB.SetSSM(sg.ssm);
                            newSB.Defocus();
                            SlotSystemUtil.AddInEmptyOrConcat(ref newSBs, newSB);
                        }
                    }
                    if(sg.isAutoSort)
                        sg.Sorter.TrimAndOrderSBs(ref newSBs);
                    if(!sg.isExpandable){
                        while(newSBs.Count <origCount){
                            newSBs.Add(null);
                        }
                    }
                    sg.SetNewSBs(newSBs);
                    sg.CreateNewSlots();
                    sg.SetSBsActStates();
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(StateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SGRemoveState: SGActState{
                public override void EnterState(StateHandler sh){
                    base.EnterState(sh);
                    List<InventoryItemInstance> cache = sg.ssm.transaction.moved;
                    List<Slottable> newSBs = sg.toList;
                    int origCount = newSBs.Count;
                    List<Slottable> removedList = new List<Slottable>();
                    List<Slottable> nonremoved = new List<Slottable>();
                    foreach(InventoryItemInstance itemInst in cache){
                        foreach(Slottable sb in newSBs){
                            if(sb!= null){
                                if(sb.itemInst == itemInst){
                                    if(itemInst.Item.IsStackable){
                                        sb.itemInst.Quantity -= itemInst.Quantity;
                                        if(sb.itemInst.Quantity <= 0)
                                            removedList.Add(sb);
                                    }else{
                                        removedList.Add(sb);
                                    }
                                }
                            }
                        }
                    }
                    foreach(Slottable sb in removedList){
                        newSBs[newSBs.IndexOf(sb)] = null;
                    }
                    if(sg.isAutoSort){
                        sg.Sorter.TrimAndOrderSBs(ref newSBs);
                        if(!sg.isExpandable){
                            while(newSBs.Count <origCount){
                                newSBs.Add(null);
                            }
                        }
                    }else{
                        if(sg.isExpandable)
                            SlotSystemUtil.Trim(ref newSBs);
                    }
                    sg.SetNewSBs(nonremoved);
                    sg.CreateNewSlots();
                    sg.SetSBsActStates();
                    if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
                        SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                        sg.SetAndRunActProcess(process);
                    }
                }
                public override void ExitState(StateHandler sh){
                    base.ExitState(sh);
                }
            }
}
