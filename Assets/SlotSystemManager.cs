using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{

	public class SlotSystemManager : AbsSlotSystemElement, TransactionManager{
		public static SlotSystemManager curSSM;
			public void SetCurSSM(){
				if(curSSM != null){
					if(curSSM != this){
						curSSM.Defocus();
						curSSM = this;
					}else{
						// no change
					}
				}else{
					curSSM = this;
				}
			}
		/*	Managerial	*/
			/* fields	*/
				public List<SlotGroup> allSGs{
					get{
						List<SlotGroup> result = new List<SlotGroup>();
						result.AddRange(allSGPs);
						result.AddRange(allSGEs);
						result.AddRange(allSGGs);
						return result;
					}
				}
				public List<SlotGroup> allSGPs{
					get{
						List<SlotGroup> result = new List<SlotGroup>();
						poolBundle.PerformInHierarchy(AddInSGList, result);
						return result;
					}
				}
				public List<SlotGroup> allSGEs{
					get{
						List<SlotGroup> result = new List<SlotGroup>();
						equipBundle.PerformInHierarchy(AddInSGList, result);
						return result;
					}
				}
				public List<SlotGroup> allSGGs{
					get{
						List<SlotGroup> result = new List<SlotGroup>();
						foreach(SlotSystemBundle gBun in otherBundles){
							gBun.PerformInHierarchy(AddInSGList, result);
						}
						return result;
					}
				}
				public void AddInSGList(SlotSystemElement ele, IList<SlotGroup> sgs){
					if(ele is SlotGroup)
					sgs.Add((SlotGroup)ele);
				}
				public List<InventoryItemInstanceMock> allEquippedItems{
					get{
						List<InventoryItemInstanceMock> items = new List<InventoryItemInstanceMock>();
						items.Add(equippedBowInst);
						items.Add(equippedWearInst);
						foreach(CarriedGearInstanceMock cgItem in equippedCarriedGears){
							items.Add(cgItem);
						}
						return items;
					}
				}
				public SlotGroup focusedSGP{
					get{
						SlotSystemElement focusedEle = poolBundle.focusedElement;
						return (SlotGroup)focusedEle;
					}
				}
				public EquipmentSet focusedEqSet{
					get{
						return (EquipmentSet)equipBundle.focusedElement;
					}
				}
				public List<SlotGroup> focusedSGEs{
					get{
						List<SlotGroup> result = new List<SlotGroup>();
						EquipmentSet focusedEquipSet = focusedEqSet;
						foreach(SlotSystemElement ele in focusedEquipSet){
							result.Add((SlotGroup)ele);
						}
						return result;
					}
				}
				public List<SlotGroup> focusedSGGs{
					get{
						List<SlotGroup> res = new List<SlotGroup>();
						foreach(SlotSystemBundle gBundle in otherBundles){
							gBundle.PerformInHierarchy(AddFocusedSGTo, res);
						}
						return res;
					}
				}
				public void AddFocusedSGTo(SlotSystemElement ele, IList<SlotGroup> list){
					if(ele is SlotGroup){
						SlotGroup sg = (SlotGroup)ele;
						bool done = false;
						SlotSystemElement tested = sg;
						while(!done){
							if(tested.immediateBundle.focusedElement != tested && !tested.immediateBundle.focusedElement.ContainsInHierarchy(tested))
								return;
							tested = tested.immediateBundle;
							if(tested.immediateBundle == null)
								break;
						}
						list.Add(sg);
					}
				}
				public List<SlotGroup> focusedSGs{
					get{
						List<SlotGroup> result = new List<SlotGroup>();
						result.Add(focusedSGP);
						result.AddRange(focusedSGEs);
						result.AddRange(focusedSGGs);
						return result;
					}
				}
				public List<EquipmentSet> equipmentSets{
					get{
						List<EquipmentSet> result = new List<EquipmentSet>();
						foreach(SlotSystemElement ele in equipBundle){
							result.Add((EquipmentSet)ele);
						}
						return result;
					}
				}
				public PoolInventory poolInv{
					get{
						return (PoolInventory)focusedSGP.inventory;
					}
				}
				public EquipmentSetInventory equipInv{
					get{
						return (EquipmentSetInventory)focusedSGEs[0].inventory;
					}
				}
				public BowInstanceMock equippedBowInst{
					get{
						foreach(SlotGroup sge in focusedSGEs){
							if(sge.Filter is SGBowFilter)
								return (BowInstanceMock)sge.slots[0].sb.itemInst;
						}
						return null;
					}
				}
				public WearInstanceMock equippedWearInst{
					get{
						foreach(SlotGroup sge in focusedSGEs){
							if(sge.Filter is SGWearFilter)
								return (WearInstanceMock)sge.slots[0].sb.itemInst;
						}
						return null;
					}
				}
				public List<CarriedGearInstanceMock> equippedCarriedGears{
					get{
						List<CarriedGearInstanceMock> result = new List<CarriedGearInstanceMock>();
						foreach(SlotGroup sge in focusedSGEs){
							if(sge.Filter is SGCGearsFilter){
								foreach(Slottable sb in sge){
									if(sb != null)
										result.Add((CarriedGearInstanceMock)sb.itemInst);
								}
							}
						}
						return result;
					}
				}
				public List<PartsInstanceMock> equippedParts{
					get{
						List<PartsInstanceMock> items = new List<PartsInstanceMock>();
						return items;
					}
				}

			/*	methods	*/
				public void Reset(){
					SetActState(SlotSystemManager.ssmWaitForActionState);
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
					SetHoveredSB(null);
					SetHoveredSG(null);
					SetDIcon1(null);
					SetDIcon2(null);
					SetTransaction(null);
				}
				public void UpdateEquipStatesOnAll(){
					/*	update equip inventory	*/
						/*	remove	*/
							List<InventoryItemInstanceMock> removed = new List<InventoryItemInstanceMock>();
							foreach(InventoryItemInstanceMock itemInInv in equipInv){
								if(!allEquippedItems.Contains(itemInInv))
									removed.Add(itemInInv);
							}
							foreach(InventoryItemInstanceMock item in removed){
								equipInv.Remove(item);
							}
						/*	add	*/
							List<InventoryItemInstanceMock> added = new List<InventoryItemInstanceMock>();
							foreach(InventoryItemInstanceMock itemInAct in allEquippedItems){
								if(!equipInv.Contains(itemInAct))
									added.Add(itemInAct);
							}
							foreach(InventoryItemInstanceMock item in added){
								equipInv.Add(item);
							}
					/*	update all itemInst's isEquipped status	*/
					foreach(InventoryItemInstanceMock itemInst in poolInv){
						if(itemInst is BowInstanceMock)
							itemInst.isEquipped = itemInst == equippedBowInst;
						else if (itemInst is WearInstanceMock)
							itemInst.isEquipped = itemInst == equippedWearInst;
						else if(itemInst is CarriedGearInstanceMock)
							itemInst.isEquipped = equippedCarriedGears != null && equippedCarriedGears.Contains((CarriedGearInstanceMock)itemInst);
						else if(itemInst is PartsInstanceMock)
							itemInst.isEquipped = equippedParts != null && equippedParts.Contains((PartsInstanceMock)itemInst);
					}
					/*	set sbs equip states	*/
					foreach(SlotGroup sg in allSGs){
						foreach(Slottable sb in sg){
							if(sb!= null)
								sb.UpdateEquipState();
						}
					}
				}
				public void SortSG(SlotGroup sg, SGSorter sorter){
					SlotSystemTransaction sortTransaction = new SortTransaction(sg, sorter);
					SetTargetSB(sortTransaction.targetSB);
					SetSG1(sortTransaction.sg1);
					SetTransaction(sortTransaction);
					transaction.Execute();
				}
				public void ChangeEquippableCGearsCount(int i, SlotGroup targetSG){
					if(!targetSG.isExpandable){
						if(targetSG.curSelState == SlotGroup.sgFocusedState ||
							targetSG.curSelState == SlotGroup.sgDefocusedState){
								equipInv.SetEquippableCGearsCount(i);
								targetSG.InitializeItems();
								UpdateEquipStatesOnAll();
								ResetAndFocus();
							}
					}else{
						throw new System.InvalidOperationException("SlotGroupManager.ChangeEquippableCGearsCount: the targetSG is expandable");
					}
				}
				public void MarkEquippedInPool(InventoryItemInstanceMock item, bool equipped){
					foreach(InventoryItemInstanceMock itemInInv in poolInv){
						if(itemInInv == item)
							itemInInv.isEquipped = equipped;
					}
				}
				public void SetEquippedOnAllSBs(InventoryItemInstanceMock item, bool equipped){
					if(equipped)
						PerformInHierarchy(Equip, item);
					else
						PerformInHierarchy(Unequip, item);
				}
				public void Equip(SlotSystemElement ele, object obj){
					if(ele is Slottable){
						InventoryItemInstanceMock item = (InventoryItemInstanceMock)obj;
						Slottable sb = (Slottable)ele;
						/*	assume all sbs are properly set in slottables, not int newSBs	*/
						if(sb.itemInst == item){
							if(sb.sg.isFocusedInBundle){/*	focused sgp or sge	*/
								if(sb.newSlotID != -1)/*	not being removed	*/
									sb.SetEqpState(Slottable.equippedState);
							}else if(sb.sg.isPool){/*	defocused sgp	*/
								sb.SetEqpState(null);
								sb.SetEqpState(Slottable.equippedState);
							}
						}
					}
				}
				public void Unequip(SlotSystemElement ele, object obj){
					if(ele is Slottable){
						InventoryItemInstanceMock item = (InventoryItemInstanceMock)obj;
						Slottable sb = (Slottable)ele;
						/*	assume all sbs are properly set in slottables, not int newSBs	*/
						if(sb.itemInst == item){
							if(sb.sg.isFocusedInBundle){
								if(sb.slotID != -1)/*	not being added	*/
									sb.SetEqpState(Slottable.unequippedState);
							}else if(sb.sg.isPool){/*	defocused sgp	*/
								sb.SetEqpState(null);
								sb.SetEqpState(Slottable.unequippedState);
							}
						}
					}
				}
		/*	SlotSystemElement	*/
			/*	States	*/
				/*	Selection state	*/
					public override SSEState curSelState{
						get{return (SSMSelState)selStateEngine.curState;}
					}
					public override SSEState prevSelState{
						get{return (SSMSelState)selStateEngine.prevState;}
					}
					public override void SetSelState(SSEState state){
						if(state == null || state is SSMSelState)
							selStateEngine.SetState(state);
						else throw new System.InvalidOperationException("SlotSystemManager.SetSelState: argument is not of type SSMSelState");
					}
					public static SSMSelState ssmDeactivatedState{
						get{
							if(m_ssmDeactivatedState == null)
								m_ssmDeactivatedState = new SSMDeactivatedState();
							return m_ssmDeactivatedState;
						}
						}static SSMSelState m_ssmDeactivatedState;
					public static SSMSelState ssmDefocusedState{
						get{
							if(m_ssmDefocusedState == null)
								m_ssmDefocusedState = new SSMDefocusedState();
							return m_ssmDefocusedState;
						}
						}static SSMSelState m_ssmDefocusedState;
					public static SSMSelState ssmFocusedState{
						get{
							if(m_ssmFocusedState == null)
								m_ssmFocusedState = new SSMFocusedState();
							return m_ssmFocusedState;
						}
						}static SSMSelState m_ssmFocusedState;
					
				/*	Action state	*/
					public override SSEState curActState{
						get{return (SSMActState)actStateEngine.curState;}
					}
					public override SSEState prevActState{
						get{return (SSMActState)actStateEngine.prevState;}
					}
					public override void SetActState(SSEState state){
						if(state == null || state is SSMActState)
							actStateEngine.SetState(state);
						else throw new System.InvalidOperationException("SlotSystemManager.SetActState: argument is not of type SSMActState");
					}
					public static SSMActState ssmWaitForActionState{
						get{
							if(m_ssmWaitForActionState == null)
								m_ssmWaitForActionState = new SSMWaitForActionState();
							return m_ssmWaitForActionState;
						}
						}static SSMActState m_ssmWaitForActionState;
					public static SSMActState ssmProbingState{
						get{
							if(m_ssmProbingState == null)
								m_ssmProbingState = new SSMProbingState();
							return m_ssmProbingState;
						}
						}static SSMActState m_ssmProbingState;
					public static SSMActState ssmTransactionState{
						get{
							if(m_ssmTransactionState == null)
								m_ssmTransactionState = new SSMTransactionState();
							return m_ssmTransactionState;
						}
						}static SSMActState m_ssmTransactionState;
					

			/*	process	*/
				/*	Selection Process	*/
					public override SSEProcess selProcess{
						get{return (SSMSelProcess)selProcEngine.process;}
					}
					public override void SetAndRunSelProcess(SSEProcess process){
						if(process == null||process is SSMSelProcess)
							selProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("SlotSystemManager.SetAndRunSelProcess: argument is not of type SSMSelProcess");
					}
					public override IEnumeratorMock greyoutCoroutine(){
						return null;
					}
					public override IEnumeratorMock greyinCoroutine(){
						return null;
					}
				/*	Action Process	*/
					public override SSEProcess actProcess{
							get{return (SSMActProcess)actProcEngine.process;}
						}
						public override void SetAndRunActProcess(SSEProcess process){
							if(process == null||process is SSMActProcess)
								actProcEngine.SetAndRunProcess(process);
							else throw new System.InvalidOperationException("SlotSystemManager.SetAndRunActProcess: argument is not of type SSMActProcess");
						}
					public IEnumeratorMock probeCoroutine(){
						return null;
					}
					public IEnumeratorMock transactionCoroutine(){
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
				public override SlotSystemBundle immediateBundle{
					get{return null;}
				}
				public SlotSystemBundle poolBundle{
					get{return m_poolBundle;}
					}SlotSystemBundle m_poolBundle;
				public SlotSystemBundle equipBundle{
					get{return m_equipBundle;}
					}SlotSystemBundle m_equipBundle;
				public IEnumerable<SlotSystemBundle> otherBundles{
					get{
						if(m_otherBundles == null)
							m_otherBundles = new SlotSystemBundle[]{};
						return m_otherBundles;}
					}IEnumerable<SlotSystemBundle> m_otherBundles;


				protected override IEnumerable<SlotSystemElement> elements{
					get{
						yield return poolBundle;
						yield return equipBundle;
						foreach(var ele in otherBundles)
							yield return ele;
					}
				}
			/*	methods	*/
				public void Initialize(SlotSystemBundle poolBundle, SlotSystemBundle equipBundle, IEnumerable<SlotSystemBundle> gBundles){
					m_eName = Util.Bold("SSM");
					this.m_poolBundle = poolBundle;
					this.m_equipBundle = equipBundle;
					m_otherBundles = gBundles;
					PerformInHierarchy(SetSSM);
					PerformInHierarchy(SetParent);
					SetSelState(SlotSystemManager.ssmDeactivatedState);
					SetActState(SlotSystemManager.ssmWaitForActionState);
				}
				public SlotSystemElement FindParent(SlotSystemElement ele){
					foundParent = null;
					PerformInHierarchy(CheckAndReportParent, ele);
					return foundParent;
					}public SlotSystemElement foundParent;
				void CheckAndReportParent(SlotSystemElement ele, object obj){
					if(!(ele is Slottable)){
						SlotSystemElement tarEle = (SlotSystemElement)obj;
						foreach(SlotSystemElement e in ele){
							if(e == tarEle)
								this.foundParent = ele;
						}
					}
				}
				void SetSSM(SlotSystemElement ele){
					ele.ssm = this;
				}
				void SetParent(SlotSystemElement ele){
					if(!((ele is Slottable) || (ele is SlotGroup)))
					foreach(SlotSystemElement e in ele){
						if(e != null)
						e.parent = ele;
					}
				}
				public override void Activate(){
					SetCurSSM();
					UpdateEquipStatesOnAll();
					Focus();
				}
				public override void Focus(){
					SetSelState(SlotSystemManager.ssmFocusedState);
					foreach(SlotSystemElement ele in this){
						ele.Focus();
					}
				}
				public override void Defocus(){
					SetSelState(SlotSystemManager.ssmDefocusedState);
					foreach(SlotSystemElement ele in this){
						ele.Defocus();
					}
				}
				public override void Deactivate(){
					SetSelState(SlotSystemManager.ssmDeactivatedState);
					foreach(SlotSystemElement ele in this){
						ele.Deactivate();
					}
				}
				public void SetFocusedPoolSG(SlotGroup sg){
					poolBundle.SetFocusedBundleElement(sg);
					Focus();
				}
				public void SetFocusedEquipmentSet(EquipmentSet eSet){
					equipBundle.SetFocusedBundleElement(eSet);
					Focus();
				}
				public void FindAndFocusInBundle(SlotSystemElement ele){
					PerformInHierarchy(FocusInBundle, ele);
				}
					public void FocusInBundle(SlotSystemElement inspected, object target){
						SlotSystemElement targetEle = (SlotSystemElement)target;
						if(inspected == targetEle){
							SlotSystemElement tested = inspected;
							while(true){
								SlotSystemBundle immBundle = tested.immediateBundle;
								if(immBundle == null)
									break;
								SlotSystemElement containingEle = null;
								foreach(SlotSystemElement e in immBundle){
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
			public SlotSystemTransaction transaction{
				get{return m_transaction;}
				}SlotSystemTransaction m_transaction;
				public void SetTransaction(SlotSystemTransaction transaction){
					if(m_transaction != transaction){
						m_transaction = transaction;
						if(m_transaction != null){
							m_transaction.Indicate();
						}
					}
				}
			public void AcceptSGTAComp(SlotGroup sg){
				if(sg2 != null && sg == sg2) m_sg2Done = true;
				else if(sg1 != null && sg == sg1) m_sg1Done = true;
				if(curActState == SlotSystemManager.ssmTransactionState){
					transactionCoroutine();
				}
			}
			public void AcceptDITAComp(DraggedIcon di){
				if(dIcon2 != null && di == dIcon2) m_dIcon2Done = true;
				else if(dIcon1 != null && di == dIcon1) m_dIcon1Done = true;
				if(curActState == SlotSystemManager.ssmTransactionState){
					transactionCoroutine();
				}
			}
			public Slottable pickedSB{
				get{return m_pickedSB;}
				}Slottable m_pickedSB;
				public void SetPickedSB(Slottable sb){
					this.m_pickedSB = sb;
				}
			public Slottable targetSB{
				get{return m_targetSB;}
				}Slottable m_targetSB;
				public void SetTargetSB(Slottable sb){
					if(sb == null || sb != targetSB){
						if(targetSB != null)
							targetSB.SetSelState(Slottable.sbFocusedState);
					}
					this.m_targetSB = sb;
					if(targetSB != null)
						targetSB.SetSelState(Slottable.sbSelectedState);
				}
			public SlotGroup sg1{
				get{return m_sg1;}
				}SlotGroup m_sg1;
				public void SetSG1(SlotGroup sg){
					if(sg == null || sg != sg1){
						if(sg1 != null)
							sg1.SetSelState(SlotGroup.sgFocusedState);
					}
					this.m_sg1 = sg;
					if(sg1 != null)
						m_sg1Done = false;
					else
						m_sg1Done = true;
				}
				public bool sg1Done{
				get{return m_sg1Done;}
				}bool m_sg1Done = true;
			public SlotGroup sg2{
				get{return m_sg2;}
				}SlotGroup m_sg2;
				public void SetSG2(SlotGroup sg){
					if(sg == null || sg != sg2){
						if(sg2 != null)
							sg2.SetSelState(SlotGroup.sgFocusedState);
					}
					this.m_sg2 = sg;
					if(sg2 != null)
						sg2.SetSelState(SlotGroup.sgSelectedState);
					if(sg2 != null)
						m_sg2Done = false;
					else
						m_sg2Done = true;
				}
				public bool sg2Done{
					get{return m_sg2Done;}
					}bool m_sg2Done = true;
			public DraggedIcon dIcon1{
				get{return m_dIcon1;}
				}DraggedIcon m_dIcon1;
				public void SetDIcon1(DraggedIcon di){
					m_dIcon1 = di;
					if(m_dIcon1 == null)
						m_dIcon1Done = true;
					else
						m_dIcon1Done = false;
				}
				public bool dIcon1Done{
				get{return m_dIcon1Done;}
				}bool m_dIcon1Done = true;
			public DraggedIcon dIcon2{
				get{return m_dIcon2;}
				}DraggedIcon m_dIcon2;
				public void SetDIcon2(DraggedIcon di){
					m_dIcon2 = di;
					if(m_dIcon2 == null)
						m_dIcon2Done = true;
					else
						m_dIcon2Done = false;
				}
				public bool dIcon2Done{
				get{return m_dIcon2Done;}
				}bool m_dIcon2Done = true;
			public Slottable hoveredSB{
				get{return m_hoveredSB;}
				}Slottable m_hoveredSB;
				public void SetHoveredSB(Slottable sb){
					if(sb == null || sb != hoveredSB){
						if(hoveredSB != null)
							hoveredSB.OnHoverExitMock();
					}
					m_hoveredSB = sb;
				}
			public SlotGroup hoveredSG{
				get{return m_hoveredSG;}
				}SlotGroup m_hoveredSG;
				public void SetHoveredSG(SlotGroup sg){
					if(sg == null || sg != hoveredSG){
						if(hoveredSG != null)
							hoveredSG.OnHoverExitMock();
					}
					m_hoveredSG = sg;
				}

			public TransactionResults transactionResults{
				get{return m_transactionResults;}
				set{
					m_transactionResults = value;
				}
				}TransactionResults m_transactionResults;
			public void CreateTransactionResults(){
				/*	Create TransactionResult class instance with Transaction and SelectedSG, SelectedSB
					and store them in a list for lookup
					Filter all sbs and sgs according to the transaction result
					make sure to initialize with RevertTransaction with orig sg and pickedSB
					Perform Transaction update at SimHover looing up the list
				*/
				TransactionResults transactionResults = new TransactionResults();
				foreach(SlotGroup sg in focusedSGs){
					SlotSystemTransaction ta = AbsSlotSystemTransaction.GetTransaction(pickedSB, null, sg);
					TransactionResult tr = new TransactionResult(null, sg, ta);
					transactionResults.AddTransactionResult(tr);
					if(ta is RevertTransaction)
						sg.DefocusSelf();
					else
						sg.FocusSelf();
					foreach(Slottable sb in sg){
						if(sb != null){
							SlotSystemTransaction ta2 = AbsSlotSystemTransaction.GetTransaction(pickedSB, sb, null);
							TransactionResult tr2 = new TransactionResult(sb, null, ta2);
							transactionResults.AddTransactionResult(tr2);
							if(ta2 is RevertTransaction || ta2 is FillTransaction)
								sb.Defocus();
							else
								sb.Focus();
						}
					}
				}
				this.transactionResults = transactionResults;
			}
			public void UpdateTransaction(){
				SlotSystemTransaction ta = transactionResults.GetTransaction(hoveredSB, hoveredSG);
				SetTargetSB(ta.targetSB);
				SetSG1(ta.sg1);
				SetSG2(ta.sg2);
				SetTransaction(ta);
			}
			public SlotSystemTransaction GetTransaction(Slottable pickedSB, SlotGroup targetSG, Slottable targetSB){
				return AbsSlotSystemTransaction.GetTransaction(pickedSB, targetSB, targetSG);
			}
	}
}
