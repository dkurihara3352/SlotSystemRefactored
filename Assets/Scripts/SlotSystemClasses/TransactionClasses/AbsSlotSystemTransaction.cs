using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class AbsSlotSystemTransaction: SlotSystemTransaction{
		public static SlotSystemTransaction GetTransaction(Slottable pickedSB, SlotSystemElement hovered){
			SlotGroup origSG = pickedSB.sg;
			if(hovered != null){
				if(hovered is SlotGroup){
					SlotGroup hovSG = (SlotGroup)hovered;
					if(hovSG.AcceptsFilter(pickedSB)){
						if(hovSG != origSG && origSG.isShrinkable){
							if(hovSG.HasItem(pickedSB.itemInst) && pickedSB.itemInst.Item.IsStackable)
								return new StackTransaction(pickedSB, hovSG.GetSB(pickedSB.itemInst));
								
							if(hovSG.hasEmptySlot){
								if(!hovSG.HasItem(pickedSB.itemInst))
									return new FillTransaction(pickedSB, hovSG);
							}else{
								if(hovSG.isExpandable){
									return new FillTransaction(pickedSB, hovSG);
								}else{
									if(hovSG.SwappableSBs(pickedSB).Count == 1){
										Slottable calcedSB = hovSG.SwappableSBs(pickedSB)[0];
										if(calcedSB.itemInst != pickedSB.itemInst)
											return new SwapTransaction(pickedSB, calcedSB);
									}
								}
							}
						}
					}
					return new RevertTransaction(pickedSB);
				}else if(hovered is Slottable){
					Slottable hovSB = (Slottable)hovered;
					SlotGroup hovSBSG = hovSB.sg;
					if(hovSBSG == origSG){
						if(hovSB != pickedSB){
							if(!hovSBSG.isAutoSort)
								return new ReorderTransaction(pickedSB, hovSB);
						}
					}else{
						if(hovSBSG.AcceptsFilter(pickedSB)){
							//swap or stack, else insert
							if(pickedSB.itemInst == hovSB.itemInst){
								if(hovSBSG.isPool && origSG.isShrinkable)
									return new FillTransaction(pickedSB, hovSBSG);
								if(pickedSB.itemInst.Item.IsStackable)
									return new StackTransaction(pickedSB, hovSB);
							}else{
								if(hovSBSG.HasItem(pickedSB.itemInst)){
									if(!origSG.HasItem(hovSB.itemInst)){
										if(hovSBSG.isPool){
											if(origSG.AcceptsFilter(hovSB))
												return new SwapTransaction(pickedSB, hovSB);
											if(origSG.isShrinkable)
												return new FillTransaction(pickedSB, hovSBSG);
										}
									}
								}else{
									if(origSG.AcceptsFilter(hovSB))
										return new SwapTransaction(pickedSB, hovSB);
									if(hovSBSG.hasEmptySlot || hovSBSG.isExpandable)
										if(origSG.isShrinkable)
										return new FillTransaction(pickedSB, hovSBSG);
								}
							}
						}
					}
					return new RevertTransaction(pickedSB);
				}else
					throw new System.InvalidOperationException("AbsSlotSystemTransaction.GetTransaction: hovered is neither SG nor SB");
			}
			return new RevertTransaction(pickedSB);
		}
		protected SlotSystemManager ssm = SlotSystemManager.curSSM;
		protected List<InventoryItemInstance> removed = new List<InventoryItemInstance>();
		protected List<InventoryItemInstance> added = new List<InventoryItemInstance>();
		public virtual Slottable targetSB{get{return null;}}
		public virtual SlotGroup sg1{get{return null;}}
		public virtual SlotGroup sg2{get{return null;}}
		public virtual List<InventoryItemInstance> moved{get{return null;}}
		public virtual void Indicate(){}
		public virtual void Execute(){
			ssm.SetActState(SlotSystemManager.ssmTransactionState);
		}
		public virtual void OnComplete(){
			ssm.ResetAndFocus();
		}
	}
}
