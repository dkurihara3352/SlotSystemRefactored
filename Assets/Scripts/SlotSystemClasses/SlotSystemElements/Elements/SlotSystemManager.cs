using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SlotSystemManager : SlotSystemPage, ISlotSystemManager{
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
		/*	Managerial	*/
			/* fields	*/
				public List<ISlotGroup> allSGs{
					get{
						List<ISlotGroup> result = new List<ISlotGroup>();
						result.AddRange(allSGPs);
						result.AddRange(allSGEs);
						result.AddRange(allSGGs);
						return result;
					}
				}
				public List<ISlotGroup> allSGPs{
					get{
						List<ISlotGroup> result = new List<ISlotGroup>();
						poolBundle.PerformInHierarchy(AddInSGList, result);
						return result;
					}
				}
				public List<ISlotGroup> allSGEs{
					get{
						List<ISlotGroup> result = new List<ISlotGroup>();
						equipBundle.PerformInHierarchy(AddInSGList, result);
						return result;
					}
				}
				public List<ISlotGroup> allSGGs{
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
						if(poolBundle.isToggledOn){
							ISlotSystemElement focusedEle = poolBundle.focusedElement;
							return (ISlotGroup)focusedEle;
						}
						return null;
					}
				}
				public IEquipmentSet focusedEqSet{
					get{
						if(equipBundle.isToggledOn)
							return (IEquipmentSet)equipBundle.focusedElement;
						return null;
					}
				}
				public List<ISlotGroup> focusedSGEs{
					get{
						List<ISlotGroup> result = new List<ISlotGroup>();
						IEquipmentSet focusedEquipSet = focusedEqSet;
						foreach(ISlotSystemElement ele in focusedEquipSet){
							result.Add((ISlotGroup)ele);
						}
						return result;
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
							if((inspected.isPageElement && !inspected.isToggledOn)||(inspected.isBundleElement && inspected != ((ISlotSystemBundle)inspected.parent).focusedElement)){
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
						List<ISlotGroup> result = new List<ISlotGroup>();
						result.Add(focusedSGP);
						result.AddRange(focusedSGEs);
						result.AddRange(focusedSGGs);
						return result;
					}
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
				public BowInstance equippedBowInst{
					get{
						foreach(ISlotGroup sge in focusedSGEs){
							if(sge.Filter is SGBowFilter)
								return (BowInstance)sge.slots[0].sb.itemInst;
						}
						return null;
					}
				}
				public WearInstance equippedWearInst{
					get{
						foreach(ISlotGroup sge in focusedSGEs){
							if(sge.Filter is SGWearFilter)
								return (WearInstance)sge.slots[0].sb.itemInst;
						}
						return null;
					}
				}
				public List<CarriedGearInstance> equippedCarriedGears{
					get{
						List<CarriedGearInstance> result = new List<CarriedGearInstance>();
						foreach(ISlotGroup sge in focusedSGEs){
							if(sge.Filter is SGCGearsFilter){
								foreach(ISlottable sb in sge){
									if(sb != null)
										result.Add((CarriedGearInstance)sb.itemInst);
								}
							}
						}
						return result;
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
					SlotSystemTransaction sortTransaction = new SortTransaction(sg, sorter);
					SetTargetSB(sortTransaction.targetSB);
					SetSG1(sortTransaction.sg1);
					SetTransaction(sortTransaction);
					transaction.Execute();
				}
				public void ChangeEquippableCGearsCount(int i, ISlotGroup targetSG){
					if(!targetSG.isExpandable){
						if(targetSG.curSelState == SlotGroup.sgFocusedState ||
							targetSG.curSelState == SlotGroup.sgDefocusedState){
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
				public void Unequip(ISlotSystemElement ele, object obj){
					if(ele is ISlottable){
						InventoryItemInstance item = (InventoryItemInstance)obj;
						ISlottable sb = (ISlottable)ele;
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
				public void PointFocus(ISlotSystemElement ele){
					/*	focus the given element and all those above it to the root
							if any element in the course is page element and is not toggled on, toggle it on and focus
							if any elemen in the course is bundle element and is not the focused element in the bundle, make it the focused element and focus it
					*/
					FindAndFocusInBundle(ele);
					if(ele.isPageElement){
						ISlotSystemPage page = (ISlotSystemPage)ele.parent;
						ISlotSystemPageElement pageEle = page.GetPageElement(ele);
						if(!pageEle.isFocusToggleOn)
							page.TogglePageElementFocus(ele, true);
					}
					if(ele.isBundleElement){
						ISlotSystemBundle bundle = (ISlotSystemBundle)ele.parent;
						if(bundle.focusedElement != ele){
							bundle.SetFocusedBundleElement(ele);
							bundle.Focus();
						}
					}
				}
				public void PrePickFilter(ISlottable sb, out bool isFilteredIn){
					bool res = false;
					foreach(ISlotGroup targetSG in ssm.focusedSGs){
						if(ssm.GetTransaction(sb, targetSG).GetType() != typeof(RevertTransaction)){
							res = true; break;
						}
						foreach(ISlottable targetSB in targetSG){
							if(ssm.GetTransaction(sb, targetSB).GetType() != typeof(RevertTransaction)){
								res = true; break;
							}
						}
					}
					isFilteredIn = res;
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
					public override ISSEProcess selProcess{
						get{return (ISSMSelProcess)selProcEngine.process;}
					}
					public override void SetAndRunSelProcess(ISSEProcess process){
						if(process == null||process is ISSMSelProcess)
							selProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("SlotSystemManager.SetAndRunSelProcess: argument is not of type ISSMSelProcess");
					}
					public override IEnumeratorFake greyoutCoroutine(){
						return null;
					}
					public override IEnumeratorFake greyinCoroutine(){
						return null;
					}
				/*	Action Process	*/
					public override ISSEProcess actProcess{
							get{return (ISSMActProcess)actProcEngine.process;}
						}
						public override void SetAndRunActProcess(ISSEProcess process){
							if(process == null||process is ISSMActProcess)
								actProcEngine.SetAndRunProcess(process);
							else throw new System.InvalidOperationException("SlotSystemManager.SetAndRunActProcess: argument is not of type ISSMActProcess");
						}
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
				public ISlotSystemBundle poolBundle{
					get{return m_poolBundle;}
					}ISlotSystemBundle m_poolBundle;
				public ISlotSystemBundle equipBundle{
					get{return m_equipBundle;}
					}ISlotSystemBundle m_equipBundle;
				public IEnumerable<ISlotSystemBundle> otherBundles{
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
				public override bool isFocused{
					get{return curSelState == SlotSystemManager.ssmFocusedState;}
				}
				public override bool isDefocused{
					get{return curSelState == SlotSystemManager.ssmDefocusedState;}
				}
				public override bool isDeactivated{
					get{return curSelState == SlotSystemManager.ssmDeactivatedState;}
				}
			/*	methods	*/
				public void Initialize(ISlotSystemPageElement pBundPageEle, ISlotSystemPageElement eBundPageEle , IEnumerable<ISlotSystemPageElement> gBundPageEles){
					m_eName = SlotSystemUtil.Bold("SSM");
					this.m_poolBundle = (ISlotSystemBundle)pBundPageEle.element;
					this.m_equipBundle = (ISlotSystemBundle)eBundPageEle.element;
					List<ISlotSystemBundle> gBunds = new List<ISlotSystemBundle>();
					foreach(ISlotSystemPageElement pageEle in gBundPageEles){
						gBunds.Add((ISlotSystemBundle)pageEle.element);
					}
					List<ISlotSystemPageElement> pageEles = new List<ISlotSystemPageElement>();
					pageEles.Add(pBundPageEle);
					pageEles.Add(eBundPageEle);
					foreach(ISlotSystemPageElement pageEle in gBundPageEles){
						pageEles.Add(pageEle);
					}
					m_pageElements = pageEles;
					m_otherBundles = gBunds;
					PerformInHierarchy(SetSSM);
					PerformInHierarchy(SetParent);
					SetSelState(SlotSystemManager.ssmDeactivatedState);
					SetActState(SlotSystemManager.ssmWaitForActionState);
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
				public void SetSSM(ISlotSystemElement ele){
					ele.ssm = this;
				}
				public void SetParent(ISlotSystemElement ele){
					if(!((ele is ISlottable) || (ele is ISlotGroup)))
					foreach(ISlotSystemElement e in ele){
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
					PageFocus();
				}
				public override void Defocus(){
					SetSelState(SlotSystemManager.ssmDefocusedState);
					foreach(ISlotSystemElement ele in this){
						ele.Defocus();
					}
				}
				public override void Deactivate(){
					SetSelState(SlotSystemManager.ssmDeactivatedState);
					foreach(ISlotSystemElement ele in this){
						ele.Deactivate();
					}
					ToggleBack();
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
								if(immBundle.isPageElement && !immBundle.isToggledOn)
									immBundle.ToggleOnPageElement();
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
			public void AcceptSGTAComp(ISlotGroup sg){
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
			public void ExecuteTransaction(){
				SetActState(SlotSystemManager.ssmTransactionState);
				transaction.Execute();
			}
			public virtual ISlottable pickedSB{
				get{return m_pickedSB;}
				}ISlottable m_pickedSB;
				public virtual void SetPickedSB(ISlottable sb){
					this.m_pickedSB = sb;
				}
			public ISlottable targetSB{
				get{return m_targetSB;}
				}ISlottable m_targetSB;
				public void SetTargetSB(ISlottable sb){
					if(sb == null || sb != targetSB){
						if(targetSB != null)
							targetSB.SetSelState(Slottable.sbFocusedState);
					}
					this.m_targetSB = sb;
					if(targetSB != null)
						targetSB.SetSelState(Slottable.sbSelectedState);
				}
			public ISlotGroup sg1{
				get{return m_sg1;}
				}ISlotGroup m_sg1;
				public void SetSG1(ISlotGroup sg){
					if(sg == null || sg != sg1){
						if(sg1 != null)
							ReferToTAAndUpdateSelState(sg1);
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
			public ISlotGroup sg2{
				get{return m_sg2;}
				}ISlotGroup m_sg2;
				public void SetSG2(ISlotGroup sg){
					if(sg == null || sg != sg2){
						if(sg2 != null)
							ReferToTAAndUpdateSelState(sg2);
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
			public virtual DraggedIcon dIcon1{
				get{return m_dIcon1;}
				}DraggedIcon m_dIcon1;
				public virtual void SetDIcon1(DraggedIcon di){
					m_dIcon1 = di;
					if(m_dIcon1 == null)
						m_dIcon1Done = true;
					else
						m_dIcon1Done = false;
				}
				public bool dIcon1Done{
				get{return m_dIcon1Done;}
				}bool m_dIcon1Done = true;
			public virtual DraggedIcon dIcon2{
				get{return m_dIcon2;}
				}DraggedIcon m_dIcon2;
				public virtual void SetDIcon2(DraggedIcon di){
					m_dIcon2 = di;
					if(m_dIcon2 == null)
						m_dIcon2Done = true;
					else
						m_dIcon2Done = false;
				}
				public bool dIcon2Done{
				get{return m_dIcon2Done;}
				}bool m_dIcon2Done = true;
			public ISlotSystemElement hovered{
				get{return m_hovered;}
				}protected ISlotSystemElement m_hovered;
				public virtual void SetHovered(ISlotSystemElement ele){
					if(ele == null || ele != hovered){
						if(hovered != null){
							if(hovered is ISlottable)
								((ISlottable)hovered).OnHoverExitMock();
							else if(hovered is ISlotGroup)
								((ISlotGroup)hovered).OnHoverExitMock();
						}
						m_hovered = ele;
					}
				}
			public Dictionary<ISlotSystemElement, SlotSystemTransaction> transactionResults;
			public virtual void CreateTransactionResults(){
				Dictionary<ISlotSystemElement, SlotSystemTransaction> result = new Dictionary<ISlotSystemElement, SlotSystemTransaction>();
				foreach(ISlotGroup sg in focusedSGs){
					SlotSystemTransaction ta = AbsSlotSystemTransaction.GetTransaction(pickedSB, sg);
					result.Add(sg, ta);
					if(ta is RevertTransaction)
						sg.DefocusSelf();
					else
						sg.FocusSelf();
					foreach(ISlottable sb in sg){
						if(sb != null){
							SlotSystemTransaction ta2 = AbsSlotSystemTransaction.GetTransaction(pickedSB, sb);
							result.Add(sb, ta2);
							if(ta2 is RevertTransaction || ta2 is FillTransaction)
								sb.Defocus();
							else
								sb.Focus();
						}
					}
				}
				this.transactionResults = result;
			}
			public virtual void UpdateTransaction(){
				SlotSystemTransaction ta = null;
				if(transactionResults.TryGetValue(hovered, out ta)){
					SetTargetSB(ta.targetSB);
					SetSG1(ta.sg1);
					SetSG2(ta.sg2);
					SetTransaction(ta);
				}
			}
			public void ReferToTAAndUpdateSelState(ISlotGroup sg){
				if(transactionResults != null){
					SlotSystemTransaction ta = null;
					if(transactionResults.TryGetValue(sg, out ta)){
						if(ta is RevertTransaction)
							sg.SetSelState(SlotGroup.sgDefocusedState);
						else
							sg.SetSelState(SlotGroup.sgFocusedState);
					}
				}else
					sg.SetSelState(SlotGroup.sgFocusedState);
			}
			public SlotSystemTransaction GetTransaction(ISlottable pickedSB, ISlotSystemElement hovered){
				return AbsSlotSystemTransaction.GetTransaction(pickedSB, hovered);
			}
	}
	public interface ISlotSystemManager: IAbsSlotSystemElement, TransactionManager{
		void SetCurSSM();
		IEnumeratorFake probeCoroutine();
		IEnumeratorFake transactionCoroutine();
		/*	Managerial */
			List<ISlotGroup> allSGs{get;}
			List<ISlotGroup> allSGPs{get;}
			List<ISlotGroup> allSGEs{get;}
			List<ISlotGroup> allSGGs{get;}
			void AddInSGList(ISlotSystemElement ele, IList<ISlotGroup> sgs);
			List<InventoryItemInstance> allEquippedItems{get;}
			ISlotGroup focusedSGP{get;}
			IEquipmentSet focusedEqSet{get;}
			List<ISlotGroup> focusedSGEs{get;}
			List<ISlotGroup> focusedSGGs{get;}
			void AddFocusedSGTo(ISlotSystemElement ele, IList<ISlotGroup> list);
			List<ISlotGroup> focusedSGs{get;}
			List<IEquipmentSet> equipmentSets{get;}
			PoolInventory poolInv{get;}
			EquipmentSetInventory equipInv{get;}
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
			void PointFocus(ISlotSystemElement ele);
			void PrePickFilter(ISlottable sb, out bool isFilteredIn);
			void ExecuteTransaction();
		/*	SlotSystemElement 	*/
			ISlotSystemBundle poolBundle{get;}
			ISlotSystemBundle equipBundle{get;}
			IEnumerable<ISlotSystemBundle> otherBundles{get;}
			ISlotSystemElement FindParent(ISlotSystemElement ele);
			void CheckAndReportParent(ISlotSystemElement ele, object obj);
			void SetSSM(ISlotSystemElement ele);
			void SetParent(ISlotSystemElement ele);
			void FindAndFocusInBundle(ISlotSystemElement ele);
			void FocusInBundle(ISlotSystemElement inspected, object target);
		
	}
}
