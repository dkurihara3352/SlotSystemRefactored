using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SlotSystemManager : SlotSystemElement, ISlotSystemManager{
		public IFocusedSGProvider focusedSGProvider{
			get{
				if(_focusedSGProvider != null)
					return _focusedSGProvider;
				else
					throw new InvalidOperationException("focusedSGProvider not set");
			}
		}
			IFocusedSGProvider _focusedSGProvider;
		public void SetFocusedSGProvider(IFocusedSGProvider focusedSGProvider){
			_focusedSGProvider = focusedSGProvider;
		}
		public IEquippedProvider equippedProvider{
			get{
				if(_equippedProvider != null)
					return _equippedProvider;
				else
					throw new InvalidOperationException("equippedProvider not set");
			}
		}
			IEquippedProvider _equippedProvider;
		public void SetEquippedProvider(IEquippedProvider equippedProvider){
			_equippedProvider = equippedProvider;
		}
		public IAllElementsProvider allElementsProvider{
			get{
				if(_allElementsProvider != null)
					return _allElementsProvider;
				else
					throw new InvalidOperationException("allElementsProvider not set");
			}
		}
			IAllElementsProvider _allElementsProvider;
		public void SetAllElementsProvider(IAllElementsProvider allElementsProvider){
			_allElementsProvider = allElementsProvider;
		}
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
			List<InventoryItemInstance> removed = new List<InventoryItemInstance>();
			foreach(InventoryItemInstance itemInInv in focusedSGProvider.equipInv){
				if(!equippedProvider.allEquippedItems.Contains(itemInInv))
					removed.Add(itemInInv);
			}
			foreach(InventoryItemInstance item in removed){
				focusedSGProvider.equipInv.Remove(item);
			}
		}
		public void AddToEquipInv(){
			List<InventoryItemInstance> added = new List<InventoryItemInstance>();
			foreach(InventoryItemInstance itemInAct in equippedProvider.allEquippedItems){
				if(!focusedSGProvider.equipInv.Contains(itemInAct))
					added.Add(itemInAct);
			}
			foreach(InventoryItemInstance item in added){
				focusedSGProvider.equipInv.Add(item);
			}
		}
		public void UpdateAllItemsEquipStatusInPoolInv(){
			foreach(InventoryItemInstance itemInst in focusedSGProvider.poolInv){
				if(itemInst is BowInstance)
					itemInst.isEquipped = itemInst == equippedProvider.equippedBowInst;
				else if (itemInst is WearInstance)
					itemInst.isEquipped = itemInst == equippedProvider.equippedWearInst;
				else if(itemInst is CarriedGearInstance)
					itemInst.isEquipped = equippedProvider.equippedCarriedGears != null && equippedProvider.equippedCarriedGears.Contains((CarriedGearInstance)itemInst);
				else if(itemInst is PartsInstance)
					itemInst.isEquipped = equippedProvider.equippedParts != null && equippedProvider.equippedParts.Contains((PartsInstance)itemInst);
			}
		}
		public void UpdateAllSBsEquipState(){
			foreach(ISlottable sb in allElementsProvider.allSBs){
				sb.UpdateEquipState();
			}
		}
		public void MarkEquippedInPool(InventoryItemInstance item, bool equipped){
			foreach(InventoryItemInstance itemInInv in focusedSGProvider.poolInv){
				if(itemInInv == item)
					itemInInv.isEquipped = equipped;
			}
		}
		public void SetEquippedOnAllSBs(InventoryItemInstance item, bool equipped){
			if(equipped)
				PerformInHierarchy(Equip, item);
			else
				PerformInHierarchy(Unequip, item);
		}
			public void Equip(ISlotSystemElement ele, object obj){
				if(ele is ISlottable){
					InventoryItemInstance item = (InventoryItemInstance)obj;
					ISlottable sb = (ISlottable)ele;
					/*	assume all sbs are properly set in slottables, not in newSBs	*/
					if(sb.item == item){
						if(sb.sg.isFocusedInHierarchy){/*	focused sgp or sge	*/
							if(!sb.isToBeRemoved)/*	not being removed	*/
								sb.Equip();
						}else if(sb.isPool){/*	defocused sgp, setting equipped w/o transition	*/
							sb.ClearCurEqpState();
							sb.Equip();
						}
					}
				}
			}
			public void Unequip(ISlotSystemElement ele, object obj){
				if(ele is ISlottable){
					InventoryItemInstance item = (InventoryItemInstance)obj;
					ISlottable sb = (ISlottable)ele;
					/*	assume all sbs are properly set in slottables, not int newSBs	*/
					if(sb.item == item){
						if(sb.sg.isFocusedInHierarchy){
							if(!sb.isToBeAdded)
								sb.Unequip();
						}else if(sb.isPool){/*	defocused sgp	*/
							sb.ClearCurEqpState();
							sb.Unequip();
						}
					}
				}
			}
		/*	SlotSystemElement	*/
			public override ISSESelStateHandler selStateHandler{
				get{
					if(_selStateHandler == null)
						_selStateHandler = new SSMSelStateHandler(this);
					return _selStateHandler;
				}
			}
				ISSESelStateHandler _selStateHandler;
			public ISlotSystemBundle poolBundle{
				get{return m_poolBundle;}
			}
				ISlotSystemBundle m_poolBundle;
			public ISlotSystemBundle equipBundle{
				get{return m_equipBundle;}
			}
				ISlotSystemBundle m_equipBundle;
			public List<IEquipmentSet> equipmentSets{
				get{
					List<IEquipmentSet> result = new List<IEquipmentSet>();
					foreach(ISlotSystemElement ele in equipBundle){
						result.Add((IEquipmentSet)ele);
					}
					return result;
				}
			}
			public IEnumerable<ISlotSystemBundle> otherBundles{
				get{
					if(m_otherBundles == null)
						m_otherBundles = new ISlotSystemBundle[]{};
					return m_otherBundles;}
			}
				IEnumerable<ISlotSystemBundle> m_otherBundles;
			protected override IEnumerable<ISlotSystemElement> elements{
				get{
					yield return poolBundle;
					yield return equipBundle;
					foreach(var ele in otherBundles)
						yield return ele;
				}
			}
			public override void SetElements(IEnumerable<ISlotSystemElement> elements){
				throw new InvalidOperationException("not valid for this object. Use InspectorSetUp instead.");
			}
			public ITransactionManager tam{
				get{
					if(_tam != null)
						return _tam;
					else
						throw new InvalidOperationException("tam no set");
				}
			}
				ITransactionManager _tam;
			public void SetTAM(ITransactionManager tam){
				_tam = tam;
			}
			public ITransactionCache taCache{
				get{
					if(_taCache != null)
						return _taCache;
					else
						throw new InvalidOperationException("tac not set");
				}
			}
				ITransactionCache _taCache;
			public void SetTACache(ITransactionCache taCache){
				_taCache = taCache;
			}
			public void InspectorSetUp(ISlotSystemBundle pBun, ISlotSystemBundle eBun, IEnumerable<ISlotSystemBundle> gBuns, ITransactionManager tam){
				m_poolBundle = pBun;
				m_equipBundle = eBun;
				m_otherBundles = gBuns;
				_tam = tam;
			}
			bool isElementsSetUp{
				get{
					return poolBundle != null && equipBundle != null;
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
									m_equipBundle = bun;
									equipBundle.SetParent(this);
								}
								else if(bun[0] is ISlotGroup){
									ISlotGroup sg = (ISlotGroup)bun[0];
									if(sg.inventory is PoolInventory){
										m_poolBundle = bun;
										poolBundle.SetParent(this);
									}
									else
										genericBundles.Add(bun);
								}
								else
									genericBundles.Add(bun);
							}else
								throw new InvalidOperationException("some child does not have ISlotSystemBundle component");
						}
						m_otherBundles = genericBundles;
						foreach(var bun in otherBundles)
							bun.SetParent(this);
						if(poolBundle == null)
							throw new InvalidOperationException("poolBundle is not set");
						if(equipBundle == null)
							throw new InvalidOperationException("equipBundle is not set");
					}else
						throw new InvalidOperationException("there has to be at least two transform children");
				}
			}
			public void Initialize(){
				_taCache = new TransactionCache(tam, focusedSGProvider);
				SetSSMRecursively();
				SetTAMRecursively();
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
						((IHoverable)ele).SetTACache(taCache);
				}
			public void SetTAMRecursively(){
				PerformInHierarchy(SetTAMInHi);
			}
				void SetTAMInHi(ISlotSystemElement ele){
					if(ele is IHoverable)
						((IHoverable)ele).SetTAM(tam);
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
							ISlotSystemBundle immBundle = tested.immediateBundle;
							if(immBundle == null)
								break;
							ISlotSystemElement containingEle = null;
							foreach(ISlotSystemElement e in immBundle){
								if(e.ContainsInHierarchy(tested) || e == tested)
									containingEle = e;
							}
							immBundle.SetFocusedBundleElement(containingEle);
							tested = tested.immediateBundle;
						}
						this.Focus();
					}
				}
		
	}
	public interface ISlotSystemManager: ISlotSystemElement{
		void Initialize();
		void UpdateEquipInvAndAllSBsEquipState();
		void MarkEquippedInPool(InventoryItemInstance item, bool equipped);
		void SetEquippedOnAllSBs(InventoryItemInstance item, bool equipped);
		ISlotSystemBundle poolBundle{get;}
		ISlotSystemBundle equipBundle{get;}
		IEnumerable<ISlotSystemBundle> otherBundles{get;}
		ISlotSystemElement FindParent(ISlotSystemElement ele);
		void FindAndFocusInBundle(ISlotSystemElement ele);
		IEquippedProvider equippedProvider{get;}
		IFocusedSGProvider focusedSGProvider{get;}
		IAllElementsProvider allElementsProvider{get;}
		ITransactionManager tam{get;}
		ITransactionCache taCache{get;}
	}
}
