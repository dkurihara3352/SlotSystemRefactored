using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SlotSystemManager : SlotSystemElement, ISlotSystemManager{
		public static ISlotSystemManager curSSM;
			public void SetCurSSM(){
				if(curSSM != null){
					if(curSSM != (ISlotSystemManager)this){
						curSSM.Defocus();
						curSSM = this;
					}else{
						// no change
					}
				}else{
					curSSM = this;
				}
			}
		/* Managerial */
			/* fields	*/
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
				public void AddFocusedSGToObs(ISlotSystemElement ele, IList<ISlotGroup> list){
					if(ele is ISlotGroup){
						ISlotGroup sg = (ISlotGroup)ele;
						ISlotSystemElement inspected = sg;
						bool isF = true;
						while(true){
							if(inspected.parent == null)
								break;
							if((inspected.isBundleElement && inspected != ((ISlotSystemBundle)inspected.parent).focusedElement)){
								isF = false;
								break;
							}else{
								inspected = inspected.parent;
							}
						}
						if(isF)
							list.Add(sg);
					}
				}
				public List<ISlotGroup> focusedSGs{
					get{
						if(focusedSGsFactory == null)
							focusedSGsFactory = new FocusedSGsFactory(this);
						return focusedSGsFactory.focusedSGs;
					}
					}IFocusedSGsFactory focusedSGsFactory;
					public void SetFocusedSGsFactory(IFocusedSGsFactory fac){focusedSGsFactory = fac;}
				public List<IEquipmentSet> equipmentSets{
					get{
						List<IEquipmentSet> result = new List<IEquipmentSet>();
						foreach(ISlotSystemElement ele in equipBundle){
							result.Add((IEquipmentSet)ele);
						}
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
								BowInstance result = sb.itemInst as BowInstance;
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
								WearInstance result = ((ISlottable)focusedSGEWear[0]).itemInst as WearInstance;
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
								if(sb != null) result.Add((CarriedGearInstance)sb.itemInst);
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
			/*	methods	*/
				public void Reset(){
					SetActState(waitForActionState);
					ClearFields();
				}
				public void ResetAndFocus(){
					Reset();
					Focus();
				}
				public void ClearFields(){
					SetPickedSB(null);
					SetTargetSB(null);
					SetSG1(null);
					SetSG2(null);
					SetHovered(null);
					SetDIcon1(null);
					SetDIcon2(null);
					SetTransaction(null);
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
				public void SortSG(ISlotGroup sg, SGSorter sorter){
					ISlotSystemTransaction sortTransaction = sortFA.MakeSortTA(sg, sorter);
					SetTargetSB(sortTransaction.targetSB);
					SetSG1(sortTransaction.sg1);
					SetTransaction(sortTransaction);
					transaction.Execute();
				}ISortTransactionFactory sortFA{
					get{
						if(m_sortFA == null)
							m_sortFA = new SortTransactionFactory();
						return m_sortFA;
					}
				}ISortTransactionFactory m_sortFA;
				public void SetSortFA(ISortTransactionFactory fa){m_sortFA = fa;}
				public void ChangeEquippableCGearsCount(int i, ISlotGroup targetSG){
					if(!targetSG.isExpandable){
						if(targetSG.isFocused || targetSG.isDefocused){
							equipInv.SetEquippableCGearsCount(i);
							targetSG.InitializeItems();
							UpdateEquipStatesOnAll();
							ResetAndFocus();
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
						if(sb.itemInst == item){
							if(sb.sg.isFocusedInHierarchy){/*	focused sgp or sge	*/
								if(sb.newSlotID != -1)/*	not being removed	*/
									sb.Equip();
							}else if(sb.sg.isPool){/*	defocused sgp, setting equipped w/o transition	*/
								sb.ClearEqpState();
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
						if(sb.itemInst == item){
							if(sb.sg.isFocusedInHierarchy){
								if(sb.slotID != -1)/*	not being added	*/
									sb.Unequip();
							}else if(sb.sg.isPool){/*	defocused sgp	*/
								sb.ClearEqpState();
								sb.Unequip();
							}
						}
					}
				}
				public void PrePickFilter(ISlottable sb, out bool isFilteredIn){
					bool res = false;
					foreach(ISlotGroup targetSG in focusedSGs){
						ISlotSystemTransaction ta = MakeTransaction(sb, targetSG);
						if(ta == null)
							throw new System.InvalidOperationException("SlotSystemManager.PrePickFilter: given hoveredSSE does not yield any transaction. something's wrong baby");
						else{
							if(!(ta is IRevertTransaction)){
								res = true; break;
							}
						}
						foreach(ISlottable targetSB in targetSG){
							if(sb != null)
								if(!(MakeTransaction(sb, targetSB) is IRevertTransaction)){
									res = true; break;
								}
						}
					}
					isFilteredIn = res;
				}
		/*	SlotSystemElement	*/
			/*	States	*/
				/*	Action state	*/
					ISSEStateEngine<ISSMActState> actStateEngine{
						get{
							if(m_actStateEngine == null)
								m_actStateEngine = new SSEStateEngine<ISSMActState>(this);
							return m_actStateEngine;
						}
						}ISSEStateEngine<ISSMActState> m_actStateEngine;
					void SetActStateEngine(ISSEStateEngine<ISSMActState> engine){m_actStateEngine = engine;}
					ISSMActState curActState{
						get{return actStateEngine.curState;}
					}
					ISSMActState prevActState{
						get{return actStateEngine.prevState;}
					}
					void SetActState(ISSMActState state){
						actStateEngine.SetState(state);
					}
					public virtual bool isActStateInit{get{return prevActState == null;}}
					public virtual void ClearActState(){SetActState(null); SetActState(null);}
					/* Static states */
						public ISSMActState waitForActionState{
							get{
								if(m_waitForActionState == null)
									m_waitForActionState = new SSMWaitForActionState();
								return m_waitForActionState;
							}
							}ISSMActState m_waitForActionState;
							public virtual void WaitForAction(){SetActState(waitForActionState);}
							public virtual bool isWaitingForAction{get{return curActState == waitForActionState;}}
							public virtual bool wasWaitingForAction{get{return prevActState == waitForActionState;}}
						public ISSMActState probingState{
							get{
								if(m_probingState == null)
									m_probingState = new SSMProbingState();
								return m_probingState;
							}
							}ISSMActState m_probingState;
							public virtual void Probe(){SetActState(probingState);}
							public virtual bool isProbing{get{return curActState == probingState;}}
							public virtual bool wasProbing{get{return prevActState == probingState;}}
						public ISSMActState transactionState{
							get{
								if(m_transactionState == null)
									m_transactionState = new SSMTransactionState();
								return m_transactionState;
							}
							}ISSMActState m_transactionState;
							public virtual void Transact(){SetActState(transactionState);}
							public virtual bool isTransacting{get{return curActState == transactionState;}}
							public virtual bool wasTransacting{get{return prevActState == transactionState;}}
			/*	process	*/
				/*	Selection Process	*/
					/* Coroutine */
						public override IEnumeratorFake deactivateCoroutine(){return null;}
						public override IEnumeratorFake focusCoroutine(){return null;}
						public override IEnumeratorFake defocusCoroutine(){return null;}
						public override IEnumeratorFake selectCoroutine(){return null;}
				/*	Action Process	*/
					public virtual ISSEProcessEngine<ISSMActProcess> actProcEngine{
						get{
							if(m_actProcEngine == null)
								m_actProcEngine = new SSEProcessEngine<ISSMActProcess>();
							return m_actProcEngine;
						}
						}ISSEProcessEngine<ISSMActProcess> m_actProcEngine;
					public virtual void SetActProcEngine(ISSEProcessEngine<ISSMActProcess> engine){m_actProcEngine = engine;}
					public virtual ISSMActProcess actProcess{
						get{return actProcEngine.process;}
					}
					public virtual void SetAndRunActProcess(ISSMActProcess process){
						actProcEngine.SetAndRunProcess(process);
					}
					/* Coroutine */
						public IEnumeratorFake probeCoroutine(){
							return null;
						}
						public IEnumeratorFake transactionCoroutine(){
							bool done = true;
							done &= m_dIcon1Done;
							done &= m_dIcon2Done;
							done &= m_sg1Done;
							done &= m_sg2Done;
							if(done){
								this.actProcess.Expire();
							}
							return null;
						}
			/* public fields	*/
				public override ISlotSystemBundle immediateBundle{
					get{return null;}
				}
				public virtual ISlotSystemBundle poolBundle{
					get{return m_poolBundle;}
					}ISlotSystemBundle m_poolBundle;
				public virtual ISlotSystemBundle equipBundle{
					get{return m_equipBundle;}
					}ISlotSystemBundle m_equipBundle;
				public virtual IEnumerable<ISlotSystemBundle> otherBundles{
					get{
						if(m_otherBundles == null)
							m_otherBundles = new ISlotSystemBundle[]{};
						return m_otherBundles;}
					}IEnumerable<ISlotSystemBundle> m_otherBundles;
				public override IEnumerable<ISlotSystemElement> elements{
					get{
						yield return poolBundle;
						yield return equipBundle;
						foreach(var ele in otherBundles)
							yield return ele;
					}
				}
				public override ISlotSystemManager ssm{get{return this;}}
			/*	methods	*/
				public void InspectorSetUp(ISlotSystemBundle pBun, ISlotSystemBundle eBun, IEnumerable<ISlotSystemBundle> gBuns){
					m_poolBundle = pBun;
					m_equipBundle = eBun;
					m_otherBundles = gBuns;
				}
				public override void SetElements(){}
				public void Initialize(){
					PerformInHierarchy(SetSSMInH);
					PerformInHierarchy(SetParent);
					PerformInHierarchy(InitStatesInHi);
				}
				public override void InitializeStates(){
					base.Deactivate();
					WaitForAction();
				}
				public void InitStatesInHi(ISlotSystemElement element){
					element.InitializeStates();
				}
				public virtual ISlotSystemElement FindParent(ISlotSystemElement ele){
					foundParent = null;
					PerformInHierarchy(CheckAndReportParent, ele);
					return foundParent;
					}public ISlotSystemElement foundParent;
				public void CheckAndReportParent(ISlotSystemElement ele, object obj){
					if(!(ele is ISlottable)){
						ISlotSystemElement tarEle = (ISlotSystemElement)obj;
						foreach(ISlotSystemElement e in ele){
							if(e == tarEle)
								this.foundParent = ele;
						}
					}
				}
				public void SetSSMInH(ISlotSystemElement ele){
					ele.SetSSM(this);
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
		/*	Transaction Manager	*/
			public ITransactionFactory taFactory{
				get{
					if(m_taFactory == null)
						m_taFactory = new TransactionFactory();
					return m_taFactory;
				}
			}ITransactionFactory m_taFactory;
			public void SetTAFactory(ITransactionFactory taFac){m_taFactory = taFac;}
			public ISlotSystemTransaction transaction{get{return m_transaction;}} ISlotSystemTransaction m_transaction;
				public void SetTransaction(ISlotSystemTransaction transaction){
					if(m_transaction != transaction){
						m_transaction = transaction;
						if(m_transaction != null){
							m_transaction.Indicate();
						}
					}
				}
			public void AcceptSGTAComp(ISlotGroup sg){
				if(sg2 != null && sg == sg2) m_sg2Done = true;
				else if(sg1 != null && sg == sg1) m_sg1Done = true;
				if(isTransacting){
					transactionCoroutine();
				}
			}
			public void AcceptDITAComp(DraggedIcon di){
				if(dIcon2 != null && di == dIcon2) m_dIcon2Done = true;
				else if(dIcon1 != null && di == dIcon1) m_dIcon1Done = true;
				if(isTransacting){
					transactionCoroutine();
				}
			}
			public void ExecuteTransaction(){
				Transact();
				transaction.Execute();
			}
			public virtual ISlottable pickedSB{get{return m_pickedSB;}} ISlottable m_pickedSB;
				public virtual void SetPickedSB(ISlottable sb){this.m_pickedSB = sb;}
			public ISlottable targetSB{get{return m_targetSB;}} ISlottable m_targetSB;
				public void SetTargetSB(ISlottable sb){
					if(sb == null || sb != targetSB){
						if(targetSB != null)
							targetSB.Focus();
						this.m_targetSB = sb;
						if(targetSB != null)
							targetSB.Select();
					}
				}
			public ISlotGroup sg1{get{return m_sg1;}} ISlotGroup m_sg1;
				public void SetSG1(ISlotGroup sg){
					if(sg == null || sg != sg1){
						if(sg1 != null)
							ReferToTAAndUpdateSelState(sg1);
						this.m_sg1 = sg;
						if(sg1 != null)
							m_sg1Done = false;
						else
							m_sg1Done = true;
					}
				}
				public bool sg1Done{get{return m_sg1Done;}} bool m_sg1Done = true;
			public ISlotGroup sg2{get{return m_sg2;}} ISlotGroup m_sg2;
				public void SetSG2(ISlotGroup sg){
					if(sg == null || sg != sg2){
						if(sg2 != null)
							ReferToTAAndUpdateSelState(sg2);
						this.m_sg2 = sg;
						if(sg2 != null)
							sg.Select();
						if(sg2 != null)
							m_sg2Done = false;
						else
							m_sg2Done = true;
					}
				}
				public bool sg2Done{get{return m_sg2Done;}} bool m_sg2Done = true;
			public virtual DraggedIcon dIcon1{get{return m_dIcon1;}} DraggedIcon m_dIcon1;
				public virtual void SetDIcon1(DraggedIcon di){
					m_dIcon1 = di;
					if(m_dIcon1 == null)
						m_dIcon1Done = true;
					else
						m_dIcon1Done = false;
				}
				public bool dIcon1Done{get{return m_dIcon1Done;}} bool m_dIcon1Done = true;
			public virtual DraggedIcon dIcon2{get{return m_dIcon2;}} DraggedIcon m_dIcon2;
				public virtual void SetDIcon2(DraggedIcon di){
					m_dIcon2 = di;
					if(m_dIcon2 == null)
						m_dIcon2Done = true;
					else
						m_dIcon2Done = false;
				}
				public bool dIcon2Done{get{return m_dIcon2Done;}} bool m_dIcon2Done = true;
			public ISlotSystemElement hovered{get{return m_hovered;}} protected ISlotSystemElement m_hovered;
				public virtual void SetHovered(ISlotSystemElement ele){
					if(ele == null || ele != hovered){
						if(hovered != null){
							if(hovered is ISlottable)
								((ISlottable)hovered).OnHoverExitMock();
							else if(hovered is ISlotGroup)
								((ISlotGroup)hovered).OnHoverExitMock();
						}
						m_hovered = ele;
						if(hovered != null)
							UpdateTransaction();
					}
				}
			public virtual List<InventoryItemInstance> moved{get{return m_moved;}} List<InventoryItemInstance> m_moved;
			public virtual void SetMoved(List<InventoryItemInstance> moved){this.m_moved = moved;}
			public Dictionary<ISlotSystemElement, ISlotSystemTransaction> transactionResults;
			public virtual void CreateTransactionResults(){
				Dictionary<ISlotSystemElement, ISlotSystemTransaction> result = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
				foreach(ISlotGroup sg in focusedSGs){
					ISlotSystemTransaction ta = MakeTransaction(pickedSB, sg);
					result.Add(sg, ta);
					if(ta is IRevertTransaction)
						sg.DefocusSelf();
					else
						sg.FocusSelf();
					foreach(ISlottable sb in sg){
						if(sb != null){
							ISlotSystemTransaction ta2 = MakeTransaction(pickedSB, sb);
							result.Add(sb, ta2);
							if(ta2 is IRevertTransaction || ta2 is IFillTransaction)
								sb.Defocus();
							else
								sb.Focus();
						}
					}
				}
				this.transactionResults = result;
			}
			public virtual void UpdateTransaction(){
				if(transactionResults != null){
					ISlotSystemTransaction ta = null;
					if(transactionResults.TryGetValue(hovered, out ta)){
						SetTargetSB(ta.targetSB);
						SetSG1(ta.sg1);
						SetSG2(ta.sg2);
						SetMoved(ta.moved);
						SetTransaction(ta);
					}
				}
			}
			public void ReferToTAAndUpdateSelState(ISlotGroup sg){
				if(transactionResults != null){
					ISlotSystemTransaction ta = null;
					if(transactionResults.TryGetValue(sg, out ta)){
						if(ta is IRevertTransaction)
							sg.Defocus();
						else
							sg.Focus();
					}
				}else
					sg.Focus();
			}
			public ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, ISlotSystemElement hovered){
				return taFactory.MakeTransaction(pickedSB, hovered);
			}
	}
	public interface ISlotSystemManager: ISlotSystemElement, TransactionManager{
		void SetCurSSM();
		void Initialize();
		IEnumeratorFake probeCoroutine();
		/* States And Process */
			// ISSEStateEngine<ISSMActState> actStateEngine{get;}
			// 	void SetActStateEngine(ISSEStateEngine<ISSMActState> engine);
			// 	void SetActState(ISSMActState state);
			// 	ISSMActState curActState{get;}
			// 	ISSMActState prevActState{get;}
					bool isActStateInit{get;}
					void ClearActState();
					ISSMActState waitForActionState{get;}
					void WaitForAction();
					bool isWaitingForAction{get;}
					bool wasWaitingForAction{get;}
					ISSMActState probingState{get;}
					void Probe();
					bool isProbing{get;}
					bool wasProbing{get;}
					ISSMActState transactionState{get;}
					void Transact();
					bool isTransacting{get;}
					bool wasTransacting{get;}
			ISSEProcessEngine<ISSMActProcess> actProcEngine{get;}
				void SetActProcEngine(ISSEProcessEngine<ISSMActProcess> engine);
				void SetAndRunActProcess(ISSMActProcess process);
				ISSMActProcess actProcess{get;}
					IEnumeratorFake transactionCoroutine();
		/*	Managerial */
			List<ISlotGroup> allSGs{get;}
			List<ISlotGroup> allSGPs{get;}
			List<ISlotGroup> allSGEs{get;}
			List<ISlotGroup> allSGGs{get;}
			ISlotGroup focusedSGEBow{get;}
			ISlotGroup focusedSGEWear{get;}
			ISlotGroup focusedSGECGears{get;}
			void AddInSGList(ISlotSystemElement ele, IList<ISlotGroup> sgs);
			List<InventoryItemInstance> allEquippedItems{get;}
			ISlotGroup focusedSGP{get;}
			IEquipmentSet focusedEqSet{get;}
			List<ISlotGroup> focusedSGEs{get;}
			List<ISlotGroup> focusedSGGs{get;}
			void AddFocusedSGTo(ISlotSystemElement ele, IList<ISlotGroup> list);
			List<ISlotGroup> focusedSGs{get;}
			List<IEquipmentSet> equipmentSets{get;}
			IPoolInventory poolInv{get;}
			IEquipmentSetInventory equipInv{get;}
			BowInstance equippedBowInst{get;}
			WearInstance equippedWearInst{get;}
			List<CarriedGearInstance> equippedCarriedGears{get;}
			List<PartsInstance> equippedParts{get;}
			List<ISlottable> allSBs{get;}
			void AddSBToRes(ISlotSystemElement ele, IList<ISlottable> list);
			void Reset();
			void ResetAndFocus();
			void ClearFields();
			void UpdateEquipStatesOnAll();
			void SortSG(ISlotGroup sg, SGSorter sorter);
			void ChangeEquippableCGearsCount(int i, ISlotGroup targetSG);
			void MarkEquippedInPool(InventoryItemInstance item, bool equipped);
			void SetEquippedOnAllSBs(InventoryItemInstance item, bool equipped);
			void Equip(ISlotSystemElement ele, object obj);
			void Unequip(ISlotSystemElement ele, object obj);
			void PrePickFilter(ISlottable sb, out bool isFilteredIn);
			void ExecuteTransaction();
		/*	SlotSystemElement 	*/
			ISlotSystemBundle poolBundle{get;}
			ISlotSystemBundle equipBundle{get;}
			IEnumerable<ISlotSystemBundle> otherBundles{get;}
			ISlotSystemElement FindParent(ISlotSystemElement ele);
			void CheckAndReportParent(ISlotSystemElement ele, object obj);
			void FindAndFocusInBundle(ISlotSystemElement ele);
			void FocusInBundle(ISlotSystemElement inspected, object target);
			void SetSSMInH(ISlotSystemElement ele);	
	}
	public interface TransactionManager{
		ITransactionFactory taFactory{get;}
		ISlotSystemTransaction transaction{get;}
		void AcceptSGTAComp(ISlotGroup sg);
		void AcceptDITAComp(DraggedIcon di);
		ISlottable pickedSB{get;}
		ISlottable targetSB{get;}
		ISlotGroup sg1{get;}
		ISlotGroup sg2{get;}
		DraggedIcon dIcon1{get;}
		DraggedIcon dIcon2{get;}
		ISlotSystemElement hovered{get;}
		List<InventoryItemInstance> moved{get;}
		void UpdateTransaction();
		void CreateTransactionResults();
		void ReferToTAAndUpdateSelState(ISlotGroup sg);
		ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, ISlotSystemElement hovered);
		
		void SetTransaction(ISlotSystemTransaction transaction);	
		void SetPickedSB(ISlottable sb);
		void SetTargetSB(ISlottable sb);
		void SetSG1(ISlotGroup sg);
		bool sg1Done{get;}
		void SetSG2(ISlotGroup sg);
		bool sg2Done{get;}
		void SetDIcon1(DraggedIcon di);
		bool dIcon1Done{get;}
		void SetDIcon2(DraggedIcon di);
		bool dIcon2Done{get;}
		void SetHovered(ISlotSystemElement ele);
		void SetMoved(List<InventoryItemInstance> moved);
	}
	public class FocusedSGsFactory: IFocusedSGsFactory{
		ISlotSystemManager ssm;
		public FocusedSGsFactory(ISlotSystemManager ssm){this.ssm = ssm;}
		public List<ISlotGroup> focusedSGs{
			get{
				List<ISlotGroup> result = new List<ISlotGroup>();
				result.Add(ssm.focusedSGP);
				result.AddRange(ssm.focusedSGEs);
				result.AddRange(ssm.focusedSGGs);
				return result;
			}
		}
	}
	public interface IFocusedSGsFactory{
		List<ISlotGroup> focusedSGs{get;}
	}
	public class SortTransactionFactory: ISortTransactionFactory{
		public ISortTransaction MakeSortTA(ISlotGroup sg, SGSorter sorter){
			return new SortTransaction(sg, sorter);
		}
	}
	public interface ISortTransactionFactory{
		ISortTransaction MakeSortTA(ISlotGroup sg, SGSorter sorter);
	}
}
