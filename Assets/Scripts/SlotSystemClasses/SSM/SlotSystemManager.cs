using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SlotSystemManager : SlotSystemElement, ISlotSystemManager{
		public override void InitializeStates(){
			_selStateHandler.Deactivate();
			GetTAM().WaitForAction();
		}
		public IFocusedSGProvider GetFocusedSGProvider(){
			if(_focusedSGProvider != null)
				return _focusedSGProvider;
			else
				throw new InvalidOperationException("focusedSGProvider not set");
		}
		public void SetFocusedSGProvider(IFocusedSGProvider focusedSGProvider){
			_focusedSGProvider = focusedSGProvider;
		}
			IFocusedSGProvider _focusedSGProvider;
			public IPoolInventory GetPoolInv(){
				return GetFocusedSGProvider().GetPoolInv();
			}
			public IEquipmentSetInventory GetEquipInv(){
				return GetFocusedSGProvider().GetEquipInv();
			}
		public IEquippedProvider GetEquippedProvider(){
			if(_equippedProvider != null)
				return _equippedProvider;
			else
				throw new InvalidOperationException("equippedProvider not set");
		}
		public void SetEquippedProvider(IEquippedProvider equippedProvider){
			_equippedProvider = equippedProvider;
		}
			IEquippedProvider _equippedProvider;
		public IAllElementsProvider GetAllElementsProvider(){
			if(_allElementsProvider != null)
				return _allElementsProvider;
			else
				throw new InvalidOperationException("allElementsProvider not set");
		}
		public void SetAllElementsProvider(IAllElementsProvider allElementsProvider){
			_allElementsProvider = allElementsProvider;
		}
			IAllElementsProvider _allElementsProvider;
		ISSMCommand UpdateEquipInvAndAllSBsEquipStateCommand{
			get{
				if(_updateEquipInvAndAllSBsEquipStateCommand == null)
					_updateEquipInvAndAllSBsEquipStateCommand = new UpdateEquipInvAndAllSBsEquipStateCommand(this);
				return _updateEquipInvAndAllSBsEquipStateCommand;
			}
		}
			ISSMCommand _updateEquipInvAndAllSBsEquipStateCommand;
		public void SetUpdateEquipInvAndAllSBsEquipStateCommand(ISSMCommand comm){
			_updateEquipInvAndAllSBsEquipStateCommand = comm;
		}
		public void UpdateEquipInvAndAllSBsEquipState(){
			UpdateEquipInvAndAllSBsEquipStateCommand.Execute();
		}
		public void RemoveFromEquipInv(){
			List<IInventoryItemInstance> removed = new List<IInventoryItemInstance>();
			IEquipmentSetInventory equipInv = GetEquipInv();
			foreach(IInventoryItemInstance item in equipInv.GetItems()){
				if(!IsAllEquippedItemsContains(item))
					removed.Add(item);
			}
			foreach(IInventoryItemInstance item in removed){
				equipInv.Remove(item);
			}
		}
			bool IsAllEquippedItemsContains(IInventoryItemInstance item){
				return GetEquippedProvider().AllEquippedItemsContain(item);
			}
		public void AddToEquipInv(){
			List<IInventoryItemInstance> added = new List<IInventoryItemInstance>();
			IEquipmentSetInventory equipInv = GetEquipInv();
			foreach(IInventoryItemInstance itemInAct in GetEquippedProvider().GetAllEquippedItems()){
				if(!equipInv.GetItems().Contains(itemInAct))
					added.Add(itemInAct);
			}
			foreach(IInventoryItemInstance item in added){
				equipInv.Add(item);
			}
		}
		public void UpdateAllItemsEquipStatusInPoolInv(){
			IPoolInventory poolInv = GetPoolInv();
			IEquippedProvider equiProv = GetEquippedProvider();

			foreach(IInventoryItemInstance itemInst in poolInv.GetItems()){
				if(itemInst.IsContainedInEquippedItems(equiProv))
					itemInst.SetIsEquipped(true);
			}
		}
		public void UpdateAllSBsEquipState(){
			foreach(ISlottable sb in GetAllElementsProvider().GetAllSBs()){
				sb.UpdateEquipState();
			}
		}
		public void MarkEquippedInPool(IInventoryItemInstance item, bool equipped){
			IPoolInventory poolInv = GetPoolInv();
			foreach(IInventoryItemInstance itemInInv in poolInv.GetItems()){
				if(itemInInv == item)
					itemInInv.SetIsEquipped(equipped);
			}
		}
		public void SetEquippedOnAllSBs(IInventoryItemInstance item, bool equipped){
			if(equipped)
				PerformInHierarchy(Equip, item);
			else
				PerformInHierarchy(Unequip, item);
		}
			public void Equip(ISlotSystemElement ele, object obj){
				if(ele is ISlottable){
					IInventoryItemInstance item = (IInventoryItemInstance)obj;
					ISlottable sb = (ISlottable)ele;
					ISlotGroup sg = sb.GetSG();
					if(sb.GetItem() == item){
						if(sg.IsFocusedInHierarchy()){
							if(!sb.IsToBeRemoved())
								sb.Equip();
						}else if(sb.IsPool()){
							sb.ClearCurEqpState();
							sb.Equip();
						}
					}
				}
			}
			public void Unequip(ISlotSystemElement ele, object obj){
				if(ele is ISlottable){
					IInventoryItemInstance item = (IInventoryItemInstance)obj;
					ISlottable sb = (ISlottable)ele;
					ISlotGroup sg = sb.GetSG();
					if(sb.GetItem() == item){
						if(sg.IsFocusedInHierarchy()){
							if(!sb.IsToBeAdded())
								sb.Unequip();
						}else if(sb.IsPool()){
							sb.ClearCurEqpState();
							sb.Unequip();
						}
					}
				}
			}
		/*	SlotSystemElement	*/
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
			public ISlotSystemBundle GetPoolBundle(){
				return _poolBundle;
			}
				ISlotSystemBundle _poolBundle;
			public bool PoolBundleContains(ISlotGroup sg){
				return GetPoolBundle().ContainsInHierarchy(sg);
			}
			public ISlotSystemBundle GetEquipBundle(){
				return _equipBundle;
			}
				ISlotSystemBundle _equipBundle;
			public bool EquipBundleContains(ISlotGroup sg){
				return GetEquipBundle().ContainsInHierarchy(sg);
			}
			public List<IEquipmentSet> equipmentSets{
				get{
					List<IEquipmentSet> result = new List<IEquipmentSet>();
					foreach(ISlotSystemElement ele in GetEquipBundle()){
						result.Add((IEquipmentSet)ele);
					}
					return result;
				}
			}
			public IEnumerable<ISlotSystemBundle> GetOtherBundles(){
				if(_otherBundles == null)
					_otherBundles = new ISlotSystemBundle[]{};
				return _otherBundles;
			}
				IEnumerable<ISlotSystemBundle> _otherBundles;
			public bool OtherBundlesContain(ISlotGroup sg){
				foreach(var bundle in GetOtherBundles())
					if(bundle.ContainsInHierarchy(sg))
						return true;
				return false;
			}
			protected override IEnumerable<ISlotSystemElement> elements{
				get{
					yield return GetPoolBundle();
					yield return GetEquipBundle();
					foreach(var ele in GetOtherBundles())
						yield return ele;
				}
			}
			public override void SetElements(IEnumerable<ISlotSystemElement> elements){
				throw new InvalidOperationException("not valid for this object. Use InspectorSetUp instead.");
			}
			public ITransactionManager GetTAM(){
				if(_tam != null)
					return _tam;
				else
					throw new InvalidOperationException("tam no set");
			}
				ITransactionManager _tam;
			public void SetTAM(ITransactionManager tam){
				_tam = tam;
			}
			public ITransactionCache GetTAC(){
				if(_taCache != null)
					return _taCache;
				else
					throw new InvalidOperationException("tac not set");
			}
				ITransactionCache _taCache;
			public void SetTACache(ITransactionCache taCache){
				_taCache = taCache;
			}
			public void InspectorSetUp(ISlotSystemBundle pBun, ISlotSystemBundle eBun, IEnumerable<ISlotSystemBundle> gBuns, ITransactionManager tam){
				_poolBundle = pBun;
				_equipBundle = eBun;
				_otherBundles = gBuns;
				SetTAM(tam);
			}
			bool isElementsSetUp{
				get{
					return GetPoolBundle() != null && GetEquipBundle() != null;
				}
			}
			public override void SetHierarchy(){
				if(!isElementsSetUp){
					List<ISlotSystemBundle> genericBundles = new List<ISlotSystemBundle>();
					if(transform.childCount >= 2){
						for(int i = 0; i < transform.childCount; i ++){
							ISlotSystemBundle bun = transform.GetChild(i).GetComponent<ISlotSystemBundle>();
							if(bun != null){
								if(bun[0] is IEquipmentSet){
									_equipBundle = bun;
									GetEquipBundle().SetParent(this);
								}
								else if(bun[0] is ISlotGroup){
									ISlotGroup sg = (ISlotGroup)bun[0];
									if(sg.GetInventory() is PoolInventory){
										_poolBundle = bun;
										GetPoolBundle().SetParent(this);
									}
									else
										genericBundles.Add(bun);
								}
								else
									genericBundles.Add(bun);
							}else
								throw new InvalidOperationException("some child does not have ISlotSystemBundle component");
						}
						_otherBundles = genericBundles;
						foreach(var bun in GetOtherBundles())
							bun.SetParent(this);
						if(GetPoolBundle() == null)
							throw new InvalidOperationException("poolBundle is not set");
						if(GetEquipBundle() == null)
							throw new InvalidOperationException("equipBundle is not set");
					}else
						throw new InvalidOperationException("there has to be at least two transform children");
				}
			}
			public void Initialize(){
				_taCache = new TransactionCache(GetTAM(), GetFocusedSGProvider());
				SetSSMRecursively();
				SetTACacheRecursively();
				InitializeStatesRecursively();
			}
			public void SetSSMRecursively(){
				PerformInHierarchy(SetSSMInH);
			}
				public void SetSSMInH(ISlotSystemElement ele){
					ele.SetSSM(this);
				}
			public void SetTACacheRecursively(){
				PerformInHierarchy(SetTACacheInHi);
			}
				void SetTACacheInHi(ISlotSystemElement ele){
					if(ele is IHoverable)
						((IHoverable)ele).SetTACache(GetTAC());
				}

			public void InitializeStatesRecursively(){
				PerformInHierarchy(InitStatesInHi);
			}
				public void InitStatesInHi(ISlotSystemElement element){
					element.InitializeStates();
				}
			public ISlotSystemElement FindParent(ISlotSystemElement ele){
				foundParent = null;
				PerformInHierarchy(CheckAndReportParent, ele);
				return foundParent;
			}
				public ISlotSystemElement foundParent;
				public void CheckAndReportParent(ISlotSystemElement ele, object obj){
					if(!(ele is ISlottable)){
						ISlotSystemElement tarEle = (ISlotSystemElement)obj;
						foreach(ISlotSystemElement e in ele){
							if(e == tarEle)
								this.foundParent = ele;
						}
					}
				}

			public void FindAndFocusInBundle(ISlotSystemElement ele){
				PerformInHierarchy(FocusInBundle, ele);
			}
				void FocusInBundle(ISlotSystemElement inspected, object target){
					ISlotSystemElement targetEle = (ISlotSystemElement)target;
					if(inspected == targetEle){
						ISlotSystemElement tested = inspected;
						while(true){
							ISlotSystemBundle immBundle = tested.ImmediateBundle();
							if(immBundle == null)
								break;
							ISlotSystemElement containingEle = null;
							foreach(ISlotSystemElement e in immBundle){
								if(e.ContainsInHierarchy(tested) || e == tested)
									containingEle = e;
							}
							immBundle.SetFocusedElement(containingEle);
							tested = tested.ImmediateBundle();
						}
						_selStateHandler.Focus();
					}
				}
		
	}
	public interface ISlotSystemManager: ISlotSystemElement{
		void Initialize();
		void UpdateEquipInvAndAllSBsEquipState();
		void MarkEquippedInPool(IInventoryItemInstance item, bool equipped);
		void SetEquippedOnAllSBs(IInventoryItemInstance item, bool equipped);
		ISlotSystemBundle GetPoolBundle();
		bool PoolBundleContains(ISlotGroup sg);
		ISlotSystemBundle GetEquipBundle();
		bool EquipBundleContains(ISlotGroup sg);
		IEnumerable<ISlotSystemBundle> GetOtherBundles();
		bool OtherBundlesContain(ISlotGroup sg);
		ISlotSystemElement FindParent(ISlotSystemElement ele);
		void FindAndFocusInBundle(ISlotSystemElement ele);
		IEquippedProvider GetEquippedProvider();
		IFocusedSGProvider GetFocusedSGProvider();
			IPoolInventory GetPoolInv();
			IEquipmentSetInventory GetEquipInv();
		IAllElementsProvider GetAllElementsProvider();
		ITransactionManager GetTAM();
		ITransactionCache GetTAC();
	}
}
