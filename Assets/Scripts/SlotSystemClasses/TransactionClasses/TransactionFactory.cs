﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TransactionFactory: ITransactionFactory{

		public ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, ISlotSystemElement hovered){
			ISlotGroup origSG = pickedSB.sg;
			if(hovered != null){
				if(hovered is ISlotGroup){
					ISlotGroup hovSG = (ISlotGroup)hovered;
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
										ISlottable calcedSB = hovSG.SwappableSBs(pickedSB)[0];
										if(calcedSB.itemInst != pickedSB.itemInst)
											return new SwapTransaction(pickedSB, calcedSB);
									}
								}
							}
						}
					}
					return new RevertTransaction(pickedSB);
				}else if(hovered is ISlottable){
					ISlottable hovSB = (ISlottable)hovered;
					ISlotGroup hovSBSG = hovSB.sg;
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
	}
	public interface ITransactionFactory{
		ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, ISlotSystemElement hovered);
	}
}