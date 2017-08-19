using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class Slottable : SlotSystemElement, ISlottable{
		public override void InitializeStates(){
			GetSelStateHandler().Deactivate();
			GetActStateHandler().WaitForAction();
			ClearCurEqpState();
			Unmark();
		}
		/*	States	*/
			public void InitializeSB(IInventoryItemInstance item){
				IHoverable hoverable = new Hoverable(GetSSM().GetTAC());
				SetHoverable(hoverable);
				SetTapCommand(new SBTapCommand());
				SetItemHandler(new ItemHandler(item));
				SetSlotHandler(new SlotHandler());
				InitializeStateHandlers();
				hoverable.SetSSESelStateHandler(_selStateHandler);
			}
			public void InitializeStateHandlers(){
				_selStateHandler = new SBSelStateHandler(this);
				_actStateHandler = new SBActStateHandler(this, GetSSM().GetTAM());
				_eqpStateHandler = new SBEqpStateHandler(this);
				_mrkStateHandler = new SBMrkStateHandler(this);
			}
			/*	Selection state */
				public override ISSESelStateHandler GetSelStateHandler(){
					if(_selStateHandler != null)
						return _selStateHandler;
					else
						throw new InvalidOperationException("selStateHandler not set");
				}
					ISSESelStateHandler _selStateHandler;
				public override void SetSelStateHandler(ISSESelStateHandler handler){
					_selStateHandler = handler;
				}
			/*	Action State */
				public ISBActStateHandler GetActStateHandler(){
					if(_actStateHandler != null)
						return _actStateHandler;
					else
						throw new InvalidOperationException("actStateHandler not set");
				}
				public void SetActStateHandler(ISBActStateHandler actStateHandler){
					_actStateHandler = actStateHandler;
				}
					ISBActStateHandler _actStateHandler;
				public bool IsActProcessRunning(){
					return GetActStateHandler().IsActProcessRunning();
				}
				public void MoveWithin(){
					GetActStateHandler().MoveWithin();
				}
				public void Add(){
					GetActStateHandler().Add();
				}
				public void Remove(){
					GetActStateHandler().Remove();
				}
				public void PickUp(){
					GetActStateHandler().PickUp();
					GetItemHandler().SetPickedAmount(1);
				}
			/*	Equip State	*/
				ISBEqpStateHandler eqpStateHandler{
					get{
						if(_eqpStateHandler != null)
							return _eqpStateHandler;
						else
							throw new InvalidOperationException("eqpStateHandler not set");
					}
				}
					ISBEqpStateHandler _eqpStateHandler;
				public void SetEqpStateHandler(ISBEqpStateHandler handler){
					_eqpStateHandler = handler;
				}
				public void ClearCurEqpState(){
					eqpStateHandler.ClearCurEqpState();
				}
				public bool IsEqpStateNull(){
					return eqpStateHandler.IsEqpStateNull();
				}
				public bool WasEqpStateNull(){
					return eqpStateHandler.WasEqpStateNull();
				}
				public void Equip(){
					eqpStateHandler.Equip();
				}
				public bool IsEquipped(){
					return eqpStateHandler.IsEquipped();
				}
				public bool WasEquipped(){
					return eqpStateHandler.WasEquipped();
				}
				public void Unequip(){
					eqpStateHandler.Unequip();
				}
				public bool IsUnequipped(){
					return eqpStateHandler.IsUnequipped();
				}
				public bool WasUnequipped(){
					return eqpStateHandler.WasUnequipped();
				}
				public ISBEqpProcess GetEqpProcess(){
					return eqpStateHandler.GetEqpProcess();
				}
				public void SetEqpProcessEngine(ISSEProcessEngine<ISBEqpProcess> engine){
					((SBEqpStateHandler)eqpStateHandler).SetEqpProcessEngine(engine);
				}
				public void SetAndRunEqpProcess(ISBEqpProcess process){
					eqpStateHandler.SetAndRunEqpProcess(process);
				}
				public System.Func<IEnumeratorFake> GetUnequipCoroutine(){
					return eqpStateHandler.GetUnequipCoroutine();
				}
				public System.Func<IEnumeratorFake> GetEquipCoroutine(){
					return eqpStateHandler.GetEquipCoroutine();
				}
			/*	Mark state	*/
				ISBMrkStateHandler mrkStateHandler{
					get{
						if(_mrkStateHandler != null)
							return _mrkStateHandler;
						else
							throw new InvalidOperationException("mrkStateHandler not set");
					}
				}
					ISBMrkStateHandler _mrkStateHandler;
				public void SetMrkStateHandler(ISBMrkStateHandler mrkStateHandler){
					_mrkStateHandler = mrkStateHandler;
				}
				public void ClearCurMrkState(){
					mrkStateHandler.ClearCurMrkState();
				}
				public bool IsMrkStateNull(){
					return mrkStateHandler.IsMrkStateNull();
				}
				public bool WasMrkStateNull(){
					return mrkStateHandler.WasMrkStateNull();
				}
				public void Mark(){
					mrkStateHandler.Mark();
				}
				public bool IsMarked(){
					return mrkStateHandler.IsMarked();
				}
				public bool WasMarked(){
					return mrkStateHandler.WasMarked();
				}
				public void Unmark(){
					mrkStateHandler.Unmark();
				}
				public bool IsUnmarked(){
					return mrkStateHandler.IsUnmarked();
				}
				public bool WasUnmarked(){
					return mrkStateHandler.WasUnmarked();
				}
				public ISBMrkProcess GetMrkProcess(){
					return mrkStateHandler.GetMrkProcess();
				}
				public void SetMrkProcessEngine(ISSEProcessEngine<ISBMrkProcess> engine){
					((SBMrkStateHandler)mrkStateHandler).SetMrkProcessEngine(engine);
				}
				public void SetAndRunMrkProcess(ISBMrkProcess process){
					mrkStateHandler.SetAndRunMrkProcess(process);
				}
				public System.Func<IEnumeratorFake> GetUnmarkCoroutine(){
					return mrkStateHandler.GetUnmarkCoroutine();
				}
				public System.Func<IEnumeratorFake> GetMarkCoroutine(){
					return mrkStateHandler.GetMarkCoroutine();
				}
		/*	commands	*/
			public void Tap(){
				tapCommand.Execute(this);
			}
			public ISBCommand tapCommand{
				get{
					if(_tapCommand != null)
						return _tapCommand;
					else
						throw new InvalidOperationException("tapCommand not set");
				}
			}
				ISBCommand _tapCommand;
			public void SetTapCommand(ISBCommand comm){
				_tapCommand = comm;
			}
		/*	public fields	*/
			public void Increment(){
				GetActStateHandler().SetPickedUpState();
				GetItemHandler().IncreasePickedAmount();
			}
		/* Item Handling */
			public IItemHandler GetItemHandler(){
				if(_itemHandler != null)
					return _itemHandler;
				else throw new InvalidOperationException("itemHandler not set");
			}
			public void SetItemHandler(IItemHandler itemHandler){
				_itemHandler = itemHandler;
			}
				IItemHandler _itemHandler;
			public IInventoryItemInstance GetItem(){
				return GetItemHandler().GetItem();
			}
			public void UpdateEquipState(){
				if(GetItemHandler().GetItem().GetIsEquipped()) Equip();
				else Unequip();
			}
		/* SG And Slots */
			public ISlotGroup GetSG(){
				ISlotSystemElement parent = GetParent();
				if(parent != null)
					return parent as ISlotGroup;
				return null;
			}
			public bool IsPool(){
				ISlotGroup sg = GetSG();
				if(sg != null){
					return sg.IsPool();
				}else
					throw new System.InvalidOperationException("Slottable.isPool: sg is not set");
			}
			public bool IsHierarchySetUp(){
				return GetSG() != null;
			}
		/* slotHandling */
			public ISlotHandler slotHandler{
				get{
					if(_slotHandler != null)
						return _slotHandler;
					else
						throw new InvalidOperationException("slotHandler not set");
				}
			}
				ISlotHandler _slotHandler;
			public void SetSlotHandler(ISlotHandler slotHandler){
				_slotHandler = slotHandler;
			}
			public int GetSlotID(){
				return slotHandler.GetSlotID();
			}
			public void SetSlotID(int i){
				slotHandler.SetSlotID(i);
			}
			public int GetNewSlotID(){
				return slotHandler.GetNewSlotID();
			}
			public void SetNewSlotID(int id){
				slotHandler.SetNewSlotID(id);
			}
			public bool IsToBeAdded(){
				return slotHandler.IsToBeAdded();
			}
			public bool IsToBeRemoved(){
				return slotHandler.IsToBeRemoved();
			}
		/* Others */
			public void Refresh(){
				GetActStateHandler().WaitForAction();
				GetItemHandler().SetPickedAmount(0);
				slotHandler.SetNewSlotID(-2);
			}
			public bool ShareSGAndItem(ISlottable other){
				bool flag = true;
				flag &= this.GetSG() == other.GetSG();
				flag &= this.GetItem().Equals(other.GetItem());
				return flag;
			}
			public void Destroy(){
				// GameObject go = gameObject;
				// DestroyImmediate(go);
			}
		/*	SlotSystemElement imple and overrides	*/
			protected override IEnumerable<ISlotSystemElement> elements{
				get{return new ISlotSystemElement[]{};}
			}
			public override void SetElements(IEnumerable<ISlotSystemElement> elements){
			}
			public override ISlotSystemElement GetParent(){
				return GetSSM().FindParent(this);
			}
			public override bool Contains(ISlotSystemElement element){
				return false;
			}
			public override bool ContainsInHierarchy(ISlotSystemElement element){
				return false;
			}
		/*	Transaction	*/
			public bool IsPickedUp(){
				return GetHoverable().GetTAC().GetPickedSB() == (ISlottable)this;
			}
			public bool PassesPrePickFilter(){
				return !GetHoverable().GetTAC().IsTransactionGoingToBeRevert(this);
			}
		/* hoverable */
			public IHoverable GetHoverable(){
				if(_hoverable != null)
					return _hoverable;
				else
					throw new InvalidOperationException("hoverable not set");
			}
			public void SetHoverable(IHoverable hoverable){
				_hoverable = hoverable;
			}
				IHoverable _hoverable;
	}
	public interface ISlottable: ISlotSystemElement, ISBEqpStateHandler, ISBMrkStateHandler, ISlotHandler{
			IHoverable GetHoverable();
			IItemHandler GetItemHandler();
				IInventoryItemInstance GetItem();
			ISBActStateHandler GetActStateHandler();
				bool IsActProcessRunning();
				void PickUp();
				void MoveWithin();
				void Remove();
				void Add();
		/*	Commands	*/
			void Tap();
		/* Item Handling */
			void UpdateEquipState();
		/* SG And Slot */
			ISlotGroup GetSG();
			bool IsPool();
			bool IsHierarchySetUp();
		/* Transaction */
			bool IsPickedUp();
			bool PassesPrePickFilter();
		/* Other */
			void Increment();
			void Refresh();
			bool ShareSGAndItem(ISlottable other);
			void Destroy();
	}
}
