using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SlotSystem{
	public class Slottable : MonoBehaviour, IComparable<Slottable>, IComparable{

		
		/*	States	*/	
			public void SetState(SBState state){
					this.m_prevState = this.m_curState;
					this.m_curState = state;
				if(this.m_curState != this.m_prevState){
					this.m_prevState.ExitState(this);
					this.m_curState.EnterState(this);
				}
			}
			SBState m_curState;
				public SBState CurState{
					get{return m_curState;}
				}
			SBState m_prevState;
				public SBState PrevState{
					get{return m_prevState;}
				}
			static SBState m_deactivatedState;
				public static SBState DeactivatedState{
					get{
						if(Slottable.m_deactivatedState != null)
							return Slottable.m_deactivatedState;
						else{
							Slottable.m_deactivatedState = new DeactivatedState();	
							return Slottable.m_deactivatedState;
						}
					}
				}
			
			static SBState m_defocusedState;
				public static SBState DefocusedState{
					get{
						if(Slottable.m_defocusedState != null)
							return Slottable.m_defocusedState;
						else{
							Slottable.m_defocusedState = new DefocusedState();
							return Slottable.m_defocusedState;
						}
					}
				}
			static SBState m_focusedState;
				public static SBState FocusedState{
					get{
						if(Slottable.m_focusedState != null)
							return Slottable.m_focusedState;
						else{
							Slottable.m_focusedState = new FocusedState();
							return Slottable.m_focusedState;
						}
					}
				}
			static SBState m_waitForPointerUpState;
				public static SBState WaitForPointerUpState{
					get{
						if(m_waitForPointerUpState != null)
							return m_waitForPointerUpState;
						else{
							m_waitForPointerUpState = new WaitForPointerUpState();
							return m_waitForPointerUpState;
						}
					}
				}

			static SBState m_waitForPickUpState;
				public static SBState WaitForPickUpState{
					get{
						if(Slottable.m_waitForPickUpState != null)
							return Slottable.m_waitForPickUpState;
						else{
							Slottable.m_waitForPickUpState = new WaitForPickUpState();
							return Slottable.m_waitForPickUpState;
						}
					}
				}
			static SBState m_waitForNextTouchState;
				public static SBState WaitForNextTouchState{
					get{
						if(Slottable.m_waitForNextTouchState != null)
							return Slottable.m_waitForNextTouchState;
						else{
							Slottable.m_waitForNextTouchState = new WaitForNextTouchState();
							return Slottable.m_waitForNextTouchState;
						}
					}
				}
			static SBState m_pickedUpAndSelectedState;
				public static SBState PickedAndSelectedState{
					get{
						if(Slottable.m_pickedUpAndSelectedState != null)
							return Slottable.m_pickedUpAndSelectedState;
						else{
							Slottable.m_pickedUpAndSelectedState = new PickedUpAndSelectedState();
							return Slottable.m_pickedUpAndSelectedState;
						}
					}
				}
			static SBState m_pickedUpAndDeselectedState;
				public static SBState PickedAndDeselectedState{
					get{
						if(Slottable.m_pickedUpAndDeselectedState != null)
							return Slottable.m_pickedUpAndDeselectedState;
						else{
							Slottable.m_pickedUpAndDeselectedState = new PickedUpAndDeselectedState();
							return Slottable.m_pickedUpAndDeselectedState;
						}
					}
				}
			static SBState m_waitForNextTouchWhilePUState;
				public static SBState WaitForNextTouchWhilePUState{
					get{
						if(Slottable.m_waitForNextTouchWhilePUState != null)
							return Slottable.m_waitForNextTouchWhilePUState;
						else{
							Slottable.m_waitForNextTouchWhilePUState = new WaitForNextTouchWhilePUState();
							return Slottable.m_waitForNextTouchWhilePUState;
						}
					}
				}
			static SBState m_equippedAndDeselectedState;
				public static SBState EquippedAndDeselectedState{
					get{
						if(Slottable.m_equippedAndDeselectedState != null)
							return Slottable.m_equippedAndDeselectedState;
						else
							Slottable.m_equippedAndDeselectedState = new EquippedAndDeselectedState();
							return Slottable.m_equippedAndDeselectedState;
					}
				}
			static SBState m_equippedAndSelectedState;
				public static SBState EquippedAndSelectedState{
					get{
						if(Slottable.m_equippedAndSelectedState != null)
							return Slottable.m_equippedAndSelectedState;
						else
							Slottable.m_equippedAndSelectedState = new EquippedAndSelectedState();
							return Slottable.m_equippedAndSelectedState;
					}
				}
			static SBState m_equippedAndDefocusedState;
				public static SBState EquippedAndDefocusedState{
					get{
						if(Slottable.m_equippedAndDefocusedState != null)
							return Slottable.m_equippedAndDefocusedState;
						else
							Slottable.m_equippedAndDefocusedState = new EquippedAndDefocusedState();
							return Slottable.m_equippedAndDefocusedState;
					}
				}

			static SBState m_selectedState;
				public static SBState SelectedState{
					get{
						if(Slottable.m_selectedState != null)
							return Slottable.m_selectedState;
						else
							Slottable.m_selectedState = new SBSelectedState();
							return Slottable.m_selectedState;
					}
				}
			static SBState m_removedState;
				public static SBState RemovedState{
					get{
						if(Slottable.m_removedState == null)
							Slottable.m_removedState = new SBRemovedState();
						return Slottable.m_removedState;			
					}
				}
			static SBState m_addedState;
				public static SBState AddedState{
					get{
						if(Slottable.m_addedState == null)
							Slottable.m_addedState = new SBAddedState();
						return Slottable.m_addedState;			
					}
				}
			static SBState m_movingInSGState;
				public static SBState MovingInSGState{
					get{
						if(Slottable.m_movingInSGState == null)
							Slottable.m_movingInSGState = new SBMovingInSGState();
						return Slottable.m_movingInSGState;			
					}
				}
			static SBState m_revertingState;
				public static SBState RevertingState{
					get{
						if(Slottable.m_revertingState == null)
							Slottable.m_revertingState = new SBRevertingState();
						return Slottable.m_revertingState;			
					}
				}
			static SBState m_movingOutState;
				public static SBState MovingOutState{
					get{
						if(Slottable.m_movingOutState == null)
							Slottable.m_movingOutState = new SBMovingOutState();
						return Slottable.m_movingOutState;			
					}
				}
			static SBState m_movingInState;
				public static SBState MovingInState{
					get{
						if(Slottable.m_movingInState == null)
							Slottable.m_movingInState = new SBMovingInState();
						return Slottable.m_movingInState;			
					}
				}
			/*	dump	*/
				// static SlottableState m_movingState;
				// 	public static SlottableState MovingState{
				// 		get{
				// 			if(Slottable.m_movingState == null)
				// 				Slottable.m_movingState = new MovingOutState();
				// 			return Slottable.m_movingState;	
				// 		}
				// 	}
				// static SlottableState m_removingState;
				// 	public static SlottableState RemovingState{
				// 		get{
				// 			if(m_removingState == null)
				// 				m_removingState = new SBRemovingState();
				// 			return m_removingState;
				// 		}
				// 	}
				// static SlottableState m_equippingState;
				// 	public static SlottableState EquippingState{
				// 		get{
				// 			if(m_equippingState == null)
				// 				m_equippingState = new SBEquippingState();
				// 			return m_equippingState;
				// 		}
				// 	}
				// static SlottableState m_unpickingState;
				// 	public static SlottableState UnpickingState{
				// 		get{
				// 			if(Slottable.m_unpickingState == null)
				// 				Slottable.m_unpickingState = new SBUnpickingState();
				// 			return Slottable.m_unpickingState;			
				// 		}
				// 	}
		/* commands	*/
			static SlottableCommand m_tapCommand = new SBTapCommand();
				static public SlottableCommand TapCommand{
					get{return m_tapCommand;}
				}
				public void Tap(){
					m_tapCommand.Execute(this);
				}
			/*	dump	*/
				// static SlottableCommand m_instantDeactivateCommand = new DefInstantDeactivateCommand();
				// 	public static SlottableCommand InstantDeactivateCommand{
				// 		get{return m_instantDeactivateCommand;}
				// 	}
				// 	public void InstantDeactivate(){
				// 		m_instantDeactivateCommand.Execute(this);
				// 	}
		/* public fields	*/	
			bool m_delayed = true;
				public bool Delayed{
					get{return m_delayed;}
					set{m_delayed = value;}
				}	
			int m_pickedAmount = 0;
				public int PickedAmount{
					get{return m_pickedAmount;}
					set{m_pickedAmount = value;}
				}
			SlottableItem m_item;
				public SlottableItem Item{
					get{return m_item;}
				}
				public InventoryItemInstanceMock ItemInst{
					get{
						return (InventoryItemInstanceMock)Item;
					}
				}
				public void SetItem(SlottableItem item){
					m_item = item;
				}
			SlotGroupManager m_sgm;
				public SlotGroupManager SGM{
					get{return m_sgm;}
					set{m_sgm = value;}
				}
			bool m_isEquipped;
				public bool IsEquipped{
					get{
						InventoryItemInstanceMock invInst = (InventoryItemInstanceMock)m_item;
						return invInst.IsEquipped;
					}
				}
				public void Equip(){
					InventoryItemInstanceMock invInst = (InventoryItemInstanceMock)m_item;
					invInst.IsEquipped = true;
					m_isEquipped = true;
				}
				public void Unequip(){
					InventoryItemInstanceMock invInst = (InventoryItemInstanceMock)m_item;
					invInst.IsEquipped = false;
					m_isEquipped = false;
				}
			SlotGroup m_destinationSG;
				public SlotGroup DestinationSG{
					get{
						return m_destinationSG;
					}
				}
			Slot m_destinationSlot;
				public Slot DestinationSlot{
					get{return m_destinationSlot;}
				}
		
			public int SlotID{
				get{
					// int result = -1;
					// List<Slot> slots = SG.Slots;
					// for(int i = 0; i < slots.Count; i++){
					// 	if(slots[i].Sb != null)
					// 		if(slots[i].Sb == this)
					// 			result = i;
					// }
					// return result;
					return SG.Slottables.IndexOf(this);
				}
			}
			SlotGroup m_sg;
				public SlotGroup SG{
					get{
						if(m_sg == null){
							m_sg = SGM.GetSlotGroup(this);
						}
						return m_sg;
					}
				}
				public void SetSG(SlotGroup sg){
					/*	use this only when creating a new Sb in transaction	*/
					m_sg = sg;
				}
			public bool IsPickable{
				get{
					return CurState == Slottable.FocusedState || CurState == Slottable.EquippedAndDeselectedState;
				}
			}
			public bool IsFocused{
				get{return CurState == Slottable.FocusedState || CurState == Slottable.EquippedAndDeselectedState;}
			}
		/*	processes	*/
			SBProcess m_curProcess;
			public SBProcess CurProcess{
				get{return m_curProcess;}
			}
			public void SetAndRun(SBProcess process){
				if(m_curProcess != null)
					m_curProcess.Stop();
				m_curProcess = process;
				if(m_curProcess != null)
					m_curProcess.Start();
			}
			/*	coroutines	*/		
				public IEnumeratorMock GreyoutCoroutine(){
					return null;
				}
				public IEnumeratorMock UnequipAndGreyoutCoroutine(){
					return null;
				}
				public IEnumeratorMock EquipAndGreyoutCoroutine(){
					return null;
				}
				public IEnumeratorMock GreyinCoroutine(){
					return null;
				}
				public IEnumeratorMock UnequipAndGreyinCoroutine(){
					return null;
				}
				public IEnumeratorMock EquipAndGreyinCoroutine(){
					return null;
				}
				public IEnumeratorMock HighlightCoroutine(){
					return null;
				}
				public IEnumeratorMock DehighlightCoroutine(){
					return null;
				}
				public IEnumeratorMock UnequipAndDehighlightCoroutine(){
					return null;
				}
				public IEnumeratorMock EquipAndDehighlightCoroutine(){
					return null;
				}
				public IEnumeratorMock WaitForPointerUpCoroutine(){
					return null;
				}
				public IEnumeratorMock WaitForPickUpCoroutine(){
					return null;
				}
				public IEnumeratorMock PickedUpAndSelectedCoroutine(){
					return null;
				}
				public IEnumeratorMock PickedUpAndDeselectedCoroutine(){
					return null;
				}
				public IEnumeratorMock WaitForNextTouchWhilePUCoroutine(){
					return null;
				}
				public IEnumeratorMock WaitForNextTouchCoroutine(){
					return null;
				}
				public IEnumeratorMock UnequipCoroutine(){
					return null;
				}
				public IEnumeratorMock EquipCoroutine(){
					return null;
				}
				public IEnumeratorMock UnpickCoroutine(){
					return null;
				}
				public IEnumeratorMock PickUpCoroutine(){
					return null;
				}
				public IEnumeratorMock RemovedCoroutine(){
					return null;
				}
				public IEnumeratorMock AddedCoroutine(){
					return null;
				}
				public IEnumeratorMock MoveInSGCoroutine(){
					return null;
				}
				public IEnumeratorMock RevertCoroutine(){
					return null;
				}
				public IEnumeratorMock MoveInCoroutine(){
					return null;
				}
				public IEnumeratorMock MoveOutCoroutine(){
					return null;
				}
				/*	dump	*/
					// public IEnumeratorMock MoveCoroutine(){
					// 	return null;
					// }
					// public IEnumeratorMock RemovingCoroutine(){
					// 	return null;
					// }
					// public IEnumeratorMock UnpickingCoroutine(){
					// 	return null;
					// }
					// public IEnumeratorMock ReorderingCoroutine(){
					// 	return null;
					// }
		/*	Event methods	*/
			public void OnPointerDownMock(PointerEventDataMock eventDataMock){
				m_curState.OnPointerDownMock(this, eventDataMock);
			}
			public void OnPointerUpMock(PointerEventDataMock eventDataMock){
				m_curState.OnPointerUpMock(this, eventDataMock);
			}
			public void OnHoverEnterMock(PointerEventDataMock eventDataMock){
				m_curState.OnHoverEnterMock(this, eventDataMock);
			}
			public void OnHoverExitMock(PointerEventDataMock eventDataMock){
				m_curState.OnHoverExitMock(this, eventDataMock);
			}
			public void OnDeselectedMock(PointerEventDataMock eventDataMock){
				CurState.OnDeselectedMock(this, eventDataMock);
			}
			public void OnEndDragMock(PointerEventDataMock eventDataMock){
				m_curState.OnEndDragMock(this, eventDataMock);
			}
			public void Focus(){
				m_curState.Focus(this);
			}
			public void Defocus(){
				m_curState.Defocus(this);
			}
		/*	Interface implementation	*/
			int IComparable.CompareTo(object other){
				if(!(other is Slottable))
					throw new InvalidOperationException("CompareTo: no a slottable");
				return CompareTo((Slottable)other);

			}
			public int CompareTo(Slottable other){
				return this.Item.CompareTo(other.Item);
			}
			public static bool operator > (Slottable a, Slottable b){
				return a.CompareTo(b) > 0;
			}
			public static bool operator < (Slottable a, Slottable b){
				return a.CompareTo(b) < 0;
			}
		/*	InstantActions	*/
			public void InstantGreyout(){}
			public void InstantEquipAndGreyout(){}
			public void InstantGreyin(){}
			public void InstantEquipAndGreyin(){}
			public void InstantEquip(){}
			public void InstantUnequip(){}
		public void Initialize(SlotGroupManager sgm, bool delayed, InventoryItemInstanceMock item){
			m_curState = Slottable.DeactivatedState;
			m_prevState = Slottable.DeactivatedState;
			this.m_sgm = sgm;
			Delayed = delayed;
			this.SetItem(item);
		}
		public void PickUp(){
			SetState(Slottable.PickedAndSelectedState);
			m_pickedAmount = 1;
		}
		public void Increment(){
			if(m_item.IsStackable && m_item.Quantity > m_pickedAmount){
				m_pickedAmount ++;
			}
		}
		public void Deactivate(){}
		public void ExecuteTransaction(){
			SGM.Transaction.Execute();
		}
		public void MoveDraggedIcon(SlotGroup sg, Slot slot){
			SetDraggedIconDestination(sg, slot);
		}
		public void SetDraggedIconDestination(SlotGroup sg, Slot slot){
			this.m_destinationSG = sg;
			this.m_destinationSlot = slot;
		}
		public void ClearDraggedIconDestination(){
			this.m_destinationSG = null;
			this.m_destinationSlot = null;
		}
		public void GetSlotIndex(out int curID, out int newID){
			SG.GetSlotMovement(this).GetIndex(out curID, out newID);
		}
		public void ExpireProcess(){
			if(CurProcess.IsRunning)
				CurProcess.Expire();
			// SG.CheckProcessCompletion();
			// SGM.CompleteTransactionOnSB(this);
		}
	}

}
