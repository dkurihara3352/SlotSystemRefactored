using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SlotSystemManager : SlotSystemRootElement, ISlotSystemManager{
		public static ISlotSystemManager CurSSM;
		public void SetCurSSM(){
			if(CurSSM != null){
				if(CurSSM != (ISlotSystemManager)this){
					CurSSM.Defocus();
					CurSSM = this;
				}else{
					// no change
				}
			}else{
				CurSSM = this;
			}
		}
		public List<ISlotGroup> focusedSGs{
			get{
				List<ISlotGroup> result = new List<ISlotGroup>();
				result.Add(focusedSGP);
				result.AddRange(focusedSGEs);
				result.AddRange(focusedSGGs);
				return result;
			}
		}
		public IPoolInventory poolInv{
			get{
				if(focusedSGP != null){
					if(focusedSGP.inventory != null)
						return (IPoolInventory)focusedSGP.inventory;
					throw new System.InvalidOperationException("SlotSystemManager.poolInv: focusedSGP.inventory is not set");
				}
				throw new System.InvalidOperationException("SlotSystemManager.poolInv: focusedSGP is not set");
			}
		}
			public virtual List<ISlotGroup> allSGs{
				get{
					List<ISlotGroup> result = new List<ISlotGroup>();
					result.AddRange(allSGPs);
					result.AddRange(allSGEs);
					result.AddRange(allSGGs);
					return result;
				}
			}
			public virtual List<ISlotGroup> allSGPs
			{
				get{
					List<ISlotGroup> result = new List<ISlotGroup>();
					poolBundle.PerformInHierarchy(AddInSGList, result);
					return result;
				}
			}
			public virtual List<ISlotGroup> allSGEs{
				get{
					List<ISlotGroup> result = new List<ISlotGroup>();
					equipBundle.PerformInHierarchy(AddInSGList, result);
					return result;
				}
			}
			public virtual List<ISlotGroup> allSGGs{
				get{
					List<ISlotGroup> result = new List<ISlotGroup>();
					foreach(ISlotSystemBundle gBun in otherBundles){
						gBun.PerformInHierarchy(AddInSGList, result);
					}
					return result;
				}
			}
			public void AddInSGList(ISlotSystemElement ele, IList<ISlotGroup> sgs){
				if(ele is ISlotGroup)
				sgs.Add((ISlotGroup)ele);
			}
			public List<InventoryItemInstance> allEquippedItems{
				get{
					List<InventoryItemInstance> items = new List<InventoryItemInstance>();
					items.Add(equippedBowInst);
					items.Add(equippedWearInst);
					foreach(CarriedGearInstance cgItem in equippedCarriedGears){
						items.Add(cgItem);
					}
					return items;
				}
			}
			public IEquipmentSet focusedEqSet{
				get{
					if(equipBundle != null){
						ISlotSystemElement focEle = equipBundle.focusedElement;
						if(focEle != null){
							IEquipmentSet result = focEle as IEquipmentSet;
							if(result != null) return result;
							throw new System.InvalidOperationException("SlotSystemManger.focusedEqSet: equipBundle.focusedElement is not of valid type or substitute with null");
						}
						else
							throw new System.InvalidOperationException("SlotSystemManager.focusedEqpSet: equipBundle.focusedElement is not set");
					}else
						throw new System.InvalidOperationException("SlotSystemManager.focusedEqSet: equipBundle is not set");
					
				}
			}
			public ISlotGroup focusedSGP{
				get{
					if(poolBundle != null){
						ISlotSystemElement focusedEle = poolBundle.focusedElement;
						if(focusedEle != null){
							ISlotGroup result = focusedEle as ISlotGroup;
							if(result != null)
								return result;
							else
								throw new System.InvalidOperationException("SlotSystemManger.focusedSGP: poolBundle.focusedElement is not of valid type or substitute with null");
						}else
							throw new System.InvalidOperationException("SlotSystemManager.focusedSGP: poolBundle.focusedElement is not set");
					}else
						throw new System.InvalidOperationException("SlotSystemManager.focusedSGP: poolBundle is not set");
				}
			}
			public List<ISlotGroup> focusedSGEs{
				get{
					if(focusedEqSet != null){
						List<ISlotGroup> result = new List<ISlotGroup>();
							foreach(ISlotSystemElement ele in focusedEqSet){
								if(ele != null)
									result.Add((ISlotGroup)ele);
							}
						if(result.Count != 0)
							return result;
						else
							throw new System.InvalidOperationException("SlotSystemManager.focusedSGEs: focusedEqSet empty");
					}else
						throw new System.InvalidOperationException("SlotSystemManager.focusedSGEs: focusedEqSet not set");
				}
			}
			public List<ISlotGroup> focusedSGGs{
				get{
					List<ISlotGroup> res = new List<ISlotGroup>();
					foreach(ISlotSystemBundle gBundle in otherBundles){
						gBundle.PerformInHierarchy(AddFocusedSGTo, res);
					}
					return res;
				}
			}
			public void AddFocusedSGTo(ISlotSystemElement ele, IList<ISlotGroup> list){
				if(ele is ISlotGroup && ele.isFocusedInHierarchy)
					list.Add((ISlotGroup)ele);
			}
			public List<IEquipmentSet> equipmentSets{
				get{
					List<IEquipmentSet> result = new List<IEquipmentSet>();
					foreach(ISlotSystemElement ele in equipBundle){
						result.Add((IEquipmentSet)ele);
					}
					return result;
				}
			}
			public IEquipmentSetInventory equipInv{
				get{
					foreach(ISlotGroup sge in focusedSGEs){
						if(sge != null){
							IEquipmentSetInventory result = (IEquipmentSetInventory)sge.inventory;
							if(result == null)
								throw new System.InvalidOperationException("SlotSystemManager.equipInv: someSGEs not set with an inv is found");
							else return result;
						}
					}
					return null;
				}
			}
			public ISlotGroup focusedSGEBow{
				get{
					if(focusedSGEs != null){
						foreach(ISlotGroup sg in focusedSGEs){
							if(sg.filter is SGBowFilter)
								return sg;
						}
						throw new System.InvalidOperationException("SlotSystemManager.focusedSGEBow: there's no sg set with SGBowFilter in focusedSGEs");
					}
					throw new System.InvalidOperationException("SlotSystemManager.focusedSGEBow: focusedSGEs not set");
				}
			}
			public ISlotGroup focusedSGEWear{
				get{
					if(focusedSGEs != null){
						foreach(ISlotGroup sg in focusedSGEs){
							if(sg.filter is SGWearFilter)
								return sg;
						}
						throw new System.InvalidOperationException("SlotSystemManager.focusedSGEWear: there's no sg set with SGWearFilter in focusedSGEs");
					}
					throw new System.InvalidOperationException("SlotSystemManager.focusedSGEWear: focusedSGEs not set");
				}
			}
			public ISlotGroup focusedSGECGears{
				get{
					if(focusedSGEs != null){
						foreach(ISlotGroup sg in focusedSGEs){
							if(sg.filter is SGCGearsFilter)
								return sg;
						}
						throw new System.InvalidOperationException("SlotSystemManager.focusedSGECGears: there's no sg set with SGCGearsFilter in focusedSGEs");
					}
					throw new System.InvalidOperationException("SlotSystemManager.focusedSGECGears: focusedSGEs not set");
				}
			}
			public BowInstance equippedBowInst{
				get{
					if(focusedSGEBow != null){
						ISlottable sb = focusedSGEBow[0] as ISlottable;
						if(sb != null){
							BowInstance result = sb.item as BowInstance;
							if(result != null) return result;
							throw new System.InvalidOperationException("SlotSystemManager.equippedBowInst: focusedSGEBow's sb item is not set right");
						}
						throw new System.InvalidOperationException("SlotSystemManager.equippedBowInst: focusedSGEBow's indexer not set right");
					}
					throw new System.InvalidOperationException("SlotSystemManager.equippedBowInst: focusedSGEBow is not set");
				}
			}
			public WearInstance equippedWearInst{
				get{
					if(focusedSGEWear != null){
						ISlottable sb = focusedSGEWear[0] as ISlottable;
						if(sb!=null){
							WearInstance result = ((ISlottable)focusedSGEWear[0]).item as WearInstance;
							if(result != null) return result;
							throw new System.InvalidOperationException("SlotSystemManager.equippedWearInst: focusedSGEWear's sb item is not set right");
						}
						throw new System.InvalidOperationException("SlotSystemManager.equippedWearInst: focusedSGEWear's indexer not set right");
					}
					throw new System.InvalidOperationException("SlotSystemManager.equippedWearInst: focusedSGEWear is not set");
				}
			}
			public List<CarriedGearInstance> equippedCarriedGears{
				get{
					if(focusedSGECGears != null){
						List<CarriedGearInstance> result = new List<CarriedGearInstance>();
						foreach(ISlottable sb in focusedSGECGears)
							if(sb != null) result.Add((CarriedGearInstance)sb.item);
						return result;
					}
					throw new System.InvalidOperationException("SlotSystemManager.equippedCGearsInst: focusedSGECGears is not set");
				}
			}
			public List<PartsInstance> equippedParts{
				get{
					List<PartsInstance> items = new List<PartsInstance>();
					return items;
				}
			}
			public List<ISlottable> allSBs{
				get{
					List<ISlottable> res = new List<ISlottable>();
					PerformInHierarchy(AddSBToRes, res);
					return res;
				}
			}
			public void AddSBToRes(ISlotSystemElement ele, IList<ISlottable> list){
				if(ele is ISlottable)
					list.Add((ISlottable)ele);
			}
		public void UpdateEquipStatesOnAll(){
			/*	update equip inventory	*/
				/*	remove	*/
					List<InventoryItemInstance> removed = new List<InventoryItemInstance>();
					foreach(InventoryItemInstance itemInInv in equipInv){
						if(!allEquippedItems.Contains(itemInInv))
							removed.Add(itemInInv);
					}
					foreach(InventoryItemInstance item in removed){
						equipInv.Remove(item);
					}
				/*	add	*/
					List<InventoryItemInstance> added = new List<InventoryItemInstance>();
					foreach(InventoryItemInstance itemInAct in allEquippedItems){
						if(!equipInv.Contains(itemInAct))
							added.Add(itemInAct);
					}
					foreach(InventoryItemInstance item in added){
						equipInv.Add(item);
					}
			/*	update all itemInst's isEquipped status	*/
			foreach(InventoryItemInstance itemInst in poolInv){
				if(itemInst is BowInstance)
					itemInst.isEquipped = itemInst == equippedBowInst;
				else if (itemInst is WearInstance)
					itemInst.isEquipped = itemInst == equippedWearInst;
				else if(itemInst is CarriedGearInstance)
					itemInst.isEquipped = equippedCarriedGears != null && equippedCarriedGears.Contains((CarriedGearInstance)itemInst);
				else if(itemInst is PartsInstance)
					itemInst.isEquipped = equippedParts != null && equippedParts.Contains((PartsInstance)itemInst);
			}
			/*	set sbs equip states	*/
			foreach(ISlotGroup sg in allSGs){
				foreach(ISlottable sb in sg){
					if(sb!= null)
						sb.UpdateEquipState();
				}
			}
		}
		public void ChangeEquippableCGearsCount(int i, ISlotGroup targetSG){
			if(!targetSG.isExpandable){
				if(targetSG.isFocused || targetSG.isDefocused){
					equipInv.SetEquippableCGearsCount(i);
					targetSG.InitializeItems();
					UpdateEquipStatesOnAll();
				}
			}else{
				throw new System.InvalidOperationException("ISlotGroupManager.ChangeEquippableCGearsCount: the targetSG is expandable");
			}
		}
		public void MarkEquippedInPool(InventoryItemInstance item, bool equipped){
			foreach(InventoryItemInstance itemInInv in poolInv){
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
			public virtual ISlotSystemBundle poolBundle{
				get{return m_poolBundle;}
			}
				ISlotSystemBundle m_poolBundle;
			public virtual ISlotSystemBundle equipBundle{
				get{return m_equipBundle;}
			}
				ISlotSystemBundle m_equipBundle;
			public virtual IEnumerable<ISlotSystemBundle> otherBundles{
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
			public override ISlotSystemManager ssm{
				get{return this;}
			}
			public void InspectorSetUp(ISlotSystemBundle pBun, ISlotSystemBundle eBun, IEnumerable<ISlotSystemBundle> gBuns){
				m_poolBundle = pBun;
				m_equipBundle = eBun;
				m_otherBundles = gBuns;
			}
			public override void SetHierarchy(){
			}
			public void Initialize(){
				PerformInHierarchy(SetSSMInH);
				PerformInHierarchy(SetParent);
				PerformInHierarchy(InitStatesInHi);
			}
			public void SetSSMInH(ISlotSystemElement ele){
				ele.SetSSM(this);
			}
			public void InitStatesInHi(ISlotSystemElement element){
				element.InitializeStates();
			}
			public override void InitializeStates(){
				base.Deactivate();
			}
			public virtual ISlotSystemElement FindParent(ISlotSystemElement ele){
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

			public override void SetSSM(ISlotSystemElement ssm){
			}
			public override void SetParent(ISlotSystemElement ele){
				if(!((ele is ISlottable) || (ele is ISlotGroup)))
				foreach(ISlotSystemElement e in ele){
					if(e != null)
					e.SetParent(ele);
				}
			}
			public override void Activate(){
				SetCurSSM();
				UpdateEquipStatesOnAll();
				Focus();
			}
			public void FindAndFocusInBundle(ISlotSystemElement ele){
					PerformInHierarchy(FocusInBundle, ele);
					}
					public void FocusInBundle(ISlotSystemElement inspected, object target){
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
		List<ISlotGroup> focusedSGs{get;}
		IPoolInventory poolInv{get;}
		void UpdateEquipStatesOnAll();
		void ChangeEquippableCGearsCount(int i, ISlotGroup targetSG);
		void MarkEquippedInPool(InventoryItemInstance item, bool equipped);
		void SetEquippedOnAllSBs(InventoryItemInstance item, bool equipped);
		
		ISlotSystemBundle poolBundle{get;}
		ISlotSystemBundle equipBundle{get;}
		IEnumerable<ISlotSystemBundle> otherBundles{get;}
		ISlotSystemElement FindParent(ISlotSystemElement ele);
		void FindAndFocusInBundle(ISlotSystemElement ele);
		void FocusInBundle(ISlotSystemElement inspected, object target);
	}
}
