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
			ISlotGroup origSG = pickedSB.sg;
			if(hovered != null){
				if(hovered is ISlotGroup){
					ISlotGroup hovSG = (ISlotGroup)hovered;
					if(hovSG.AcceptsFilter(pickedSB)){
						if(hovSG != origSG && origSG.isShrinkable){
							if(hovSG.HasItem(pickedSB.item) && pickedSB.item.Item.IsStackable)
								return new StackTransaction(pickedSB, hovSG.GetSB(pickedSB.item), tam);
								
							if(hovSG.hasEmptySlot){
								if(!hovSG.HasItem(pickedSB.item))
									return new FillTransaction(pickedSB, hovSG, tam);
							}else{
								if(hovSG.isExpandable){
									return new FillTransaction(pickedSB, hovSG, tam);
								}else{
									if(hovSG.SwappableSBs(pickedSB).Count == 1){
										ISlottable calcedSB = hovSG.SwappableSBs(pickedSB)[0];
										if(calcedSB.item != pickedSB.item)
											return new SwapTransaction(pickedSB, calcedSB, tam);
									}
								}
							}
						}
					}
					return new RevertTransaction(pickedSB, tam);
				}else if(hovered is ISlottable){
					ISlottable hovSB = (ISlottable)hovered;
					ISlotGroup hovSBSG = hovSB.sg;
					if(hovSBSG == origSG){
						if(hovSB != pickedSB){
							if(!hovSBSG.isAutoSort)
								return new ReorderTransaction(pickedSB, hovSB, tam);
						}
					}else{
						if(hovSBSG.AcceptsFilter(pickedSB)){
							//swap or stack, else insert
							if(pickedSB.item == hovSB.item){
								if(hovSBSG.isPool && origSG.isShrinkable)
									return new FillTransaction(pickedSB, hovSBSG, tam);
								if(pickedSB.item.Item.IsStackable)
									return new StackTransaction(pickedSB, hovSB, tam);
							}else{
								if(hovSBSG.HasItem(pickedSB.item)){
									if(!origSG.HasItem(hovSB.item)){
										if(hovSBSG.isPool){
											if(origSG.AcceptsFilter(hovSB))
												return new SwapTransaction(pickedSB, hovSB, tam);
											if(origSG.isShrinkable)
												return new FillTransaction(pickedSB, hovSBSG, tam);
										}
									}
								}else{
									if(origSG.AcceptsFilter(hovSB))
										return new SwapTransaction(pickedSB, hovSB, tam);
									if(hovSBSG.hasEmptySlot || hovSBSG.isExpandable)
										if(origSG.isShrinkable)
										return new FillTransaction(pickedSB, hovSBSG, tam);
								}
							}
						}
					}
					return new RevertTransaction(pickedSB, tam);
				}else
					throw new System.InvalidOperationException("AbsSlotSystemTransaction.GetTransaction: hovered is neither SG nor SB");
			}
			return new RevertTransaction(pickedSB, tam);
		}
	}
	public interface ITransactionFactory{
		ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, IHoverable hovered);
	}
}
