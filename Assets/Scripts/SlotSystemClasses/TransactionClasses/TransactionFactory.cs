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
					ISlotGroup hovSG = (ISlotGroup)hovered;
					if(hovSG.AcceptsFilter(pickedSB)){
						if(hovSG != origSG && origSG.IsShrinkable()){
							if(hovSG.HasItem(pickedSB.GetItem()) && pickedSB.IsStackable())
								return new StackTransaction(pickedSB, hovSG.GetSB(pickedSB.GetItem()), tam);
								
							if(hovSG.HasEmptySlot()){
								if(!hovSG.HasItem(pickedSB.GetItem()))
									return new FillTransaction(pickedSB, hovSG, tam);
							}else{
								if(hovSG.IsExpandable()){
									return new FillTransaction(pickedSB, hovSG, tam);
								}else{
									if(hovSG.SwappableSBs(pickedSB).Count == 1){
										ISlottable calcedSB = hovSG.SwappableSBs(pickedSB)[0];
										if(calcedSB.GetItem() != pickedSB.GetItem())
											return new SwapTransaction(pickedSB, calcedSB, tam);
									}
								}
							}
						}
					}
					return new RevertTransaction(pickedSB, tam);
				}else if(hovered is ISlottable){
					ISlottable hovSB = (ISlottable)hovered;
					ISlotGroup hovSBSG = hovSB.GetSG();
					if(hovSBSG == origSG){
						if(hovSB != pickedSB){
							if(!hovSBSG.IsAutoSort())
								return new ReorderTransaction(pickedSB, hovSB, tam);
						}
					}else{
						if(hovSBSG.AcceptsFilter(pickedSB)){
							//swap or stack, else insert
							if(pickedSB.GetItem() == hovSB.GetItem()){
								if(hovSBSG.IsPool() && origSG.IsShrinkable())
									return new FillTransaction(pickedSB, hovSBSG, tam);
								if(pickedSB.IsStackable())
									return new StackTransaction(pickedSB, hovSB, tam);
							}else{
								if(hovSBSG.HasItem(pickedSB.GetItem())){
									if(!origSG.HasItem(hovSB.GetItem())){
										if(hovSBSG.IsPool()){
											if(origSG.AcceptsFilter(hovSB))
												return new SwapTransaction(pickedSB, hovSB, tam);
											if(origSG.IsShrinkable())
												return new FillTransaction(pickedSB, hovSBSG, tam);
										}
									}
								}else{
									if(origSG.AcceptsFilter(hovSB))
										return new SwapTransaction(pickedSB, hovSB, tam);
									if(hovSBSG.HasEmptySlot() || hovSBSG.IsExpandable())
										if(origSG.IsShrinkable())
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
