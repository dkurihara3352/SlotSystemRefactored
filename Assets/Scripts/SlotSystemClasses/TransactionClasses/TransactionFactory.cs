using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TransactionFactory: ITransactionFactory{
		ITransactionManager tam;
		public TransactionFactory(ITransactionManager tam){
			this.tam = tam;
		}
		public ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, IHoverable hovered){
			ISlotGroup origSG = pickedSB.GetSG();
			if(hovered != null){
				if(hovered is ISlotGroup){
					return MakeTAForHoveredSG((ISlotGroup)hovered, pickedSB, origSG);
				}else if(hovered is ISlottable){
					return MakeTAForHoveredSB((ISlottable)hovered, pickedSB, origSG);
					
				}else
					throw new System.InvalidOperationException("AbsSlotSystemTransaction.GetTransaction: hovered is neither SG nor SB");
			}
			return new RevertTransaction(pickedSB, tam);
		}
		ISlotSystemTransaction MakeTAForHoveredSG(ISlotGroup hoveredSG, ISlottable pickedSB, ISlotGroup origSG){
			if(hoveredSG.AcceptsFilter(pickedSB)){
				if(hoveredSG != origSG){
					if(origSG.AllowsOneWayTransaction()){
						IInventoryItemInstance pickedItem = pickedSB.GetItem();
						if(EligibleForStack(hoveredSG, pickedItem))
							return new StackTransaction(pickedSB, hoveredSG.GetSB(pickedItem), tam);
						if(EligibleForFillForHoveredSG(hoveredSG, pickedItem))
							return new FillTransaction(pickedSB, hoveredSG, tam);
					}
					ISlottable swappedSB;
					if(EligibleForSwap(hoveredSG, pickedSB, out swappedSB))
						return new SwapTransaction(pickedSB, swappedSB, tam);
				}
			}
			return new RevertTransaction(pickedSB, tam);
		}
		bool EligibleForStack(ISlotGroup hoveredSG, IInventoryItemInstance pickedItem){
			if(hoveredSG.HasItem(pickedItem) && pickedItem.IsStackable())
				return true;
			return false;
		}
		bool EligibleForFillForHoveredSG(ISlotGroup hoveredSG, IInventoryItemInstance pickedItem){
			if(!hoveredSG.HasItem(pickedItem))
				if(hoveredSG.HasEmptySlot() || hoveredSG.IsResizable())
					return true;
			return false;
		}
		bool EligibleForSwap(ISlotGroup hoveredSG, ISlottable pickedSB, out ISlottable swappedSB){
			IInventoryItemInstance pickedItem = pickedSB.GetItem();
			if(!hoveredSG.HasItem(pickedItem))
				if(!(hoveredSG.HasEmptySlot() || hoveredSG.IsResizable())){
					List<ISlottable> swappableSBs = hoveredSG.SwappableSBs(pickedSB);
					if(swappableSBs.Count == 1){
						swappedSB = swappableSBs[0];
						return true;
					}
				}
			swappedSB = null;
			return false;
		}
		ISlotSystemTransaction MakeTAForHoveredSB(ISlottable hoveredSB, ISlottable pickedSB, ISlotGroup origSG){
			ISlotGroup hovSBSG = hoveredSB.GetSG();
			if(hovSBSG == origSG){
				if(EligibleForReorder(hoveredSB, pickedSB, hovSBSG))
					return new ReorderTransaction(pickedSB, hoveredSB, tam);
			}else{
				if(hovSBSG.AcceptsFilter(pickedSB)){
					if(pickedSB.GetItem() == hoveredSB.GetItem()){
						if(hovSBSG.IsPool() && origSG.AllowsOneWayTransaction())
							return new FillTransaction(pickedSB, hovSBSG, tam);
						if(pickedSB.IsStackable())
							return new StackTransaction(pickedSB, hoveredSB, tam);
					}else{
						if(hovSBSG.HasItem(pickedSB.GetItem())){
							if(!origSG.HasItem(hoveredSB.GetItem())){
								if(hovSBSG.IsPool()){
									if(origSG.AcceptsFilter(hoveredSB))
										return new SwapTransaction(pickedSB, hoveredSB, tam);
									if(origSG.AllowsOneWayTransaction())
										return new FillTransaction(pickedSB, hovSBSG, tam);
								}
							}
						}else{
							if(origSG.AcceptsFilter(hoveredSB))
								return new SwapTransaction(pickedSB, hoveredSB, tam);
							if(hovSBSG.HasEmptySlot() || hovSBSG.IsResizable())
								if(origSG.AllowsOneWayTransaction())
								return new FillTransaction(pickedSB, hovSBSG, tam);
						}
					}
				}
			}
			return new RevertTransaction(pickedSB, tam);
		}
		bool EligibleForReorder(ISlottable hoveredSB, ISlottable pickedSB, ISlotGroup hovSBSG){
			if(hoveredSB != pickedSB)
				if(!hovSBSG.IsAutoSort())
					return true;
			return false;
		}
		// bool EligibleForFillWithHoveredSB(ISlottable hoveredSB, ISlottable pickedSB, ISlotGroup hovSBSG, ISlotGroup origSG){
		// 	IInventoryItemInstance pickedItem = pickedSB.GetItem();
		// 	IInventoryItemInstance hoveredItem = hoveredSB.GetItem();
		// 	if(hovSBSG.AcceptsFilter(pickedSB))
		// 	if(origSG.AllowsOneWayTransaction())
		// 	if(hovSBSG.IsPool()){
		// 		if(pickedItem == hoveredItem){
		// 				return true;
		// 		}else{
		// 			if(hovSBSG.HasItem(pickedItem))
		// 			if(!origSG.HasItem(hoveredItem))
		// 			if(!origSG.AcceptsFilter(hoveredSB))
		// 				return true;
		// 		}
		// 	}else{

		// 	}

		// }
	}
	public interface ITransactionFactory{
		ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, IHoverable hovered);
	}
}
