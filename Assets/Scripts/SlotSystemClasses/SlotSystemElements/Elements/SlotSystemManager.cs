using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SlotSystemManager : SlotSystemPage, TransactionManager{
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
				public SlotGroup focusedSGP{
					get{
						if(poolBundle.isToggledOn){
							SlotSystemElement focusedEle = poolBundle.focusedElement;
							return (SlotGroup)focusedEle;
						}
						return null;
					}
				}
				public EquipmentSet focusedEqSet{
					get{
						if(equipBundle.isToggledOn)
							return (EquipmentSet)equipBundle.focusedElement;
						return null;
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
						SlotSystemElement inspected = sg;
						bool isF = true;
						while(true){
							if(inspected.parent == null)
								break;
							if((inspected.isPageElement && !inspected.isToggledOn)||(inspected.isBundleElement && inspected != ((SlotSystemBundle)inspected.parent).focusedElement)){
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
				public BowInstance equippedBowInst{
					get{
						foreach(SlotGroup sge in focusedSGEs){
							if(sge.Filter is SGBowFilter)
								return (BowInstance)sge.slots[0].sb.itemInst;
						}
						return null;
					}
				}
				public WearInstance equippedWearInst{
					get{
						foreach(SlotGroup sge in focusedSGEs){
							if(sge.Filter is SGWearFilter)
								return (WearInstance)sge.slots[0].sb.itemInst;
						}
						return null;
					}
				}
				public List<CarriedGearInstance> equippedCarriedGears{
					get{
						List<CarriedGearInstance> result = new List<CarriedGearInstance>();
						foreach(SlotGroup sge in focusedSGEs){
							if(sge.Filter is SGCGearsFilter){
								foreach(Slottable sb in sge){
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
				public List<Slottable> allSBs{
					get{
						List<Slottable> res = new List<Slottable>();
						PerformInHierarchy(AddSBToRes, res);
						return res;
					}
				}
					public void AddSBToRes(SlotSystemElement ele, IList<Slottable> list){
						if(ele is Slottable)
							list.Add((Slottable)ele);
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
				public void Equip(SlotSystemElement ele, object obj){
					if(ele is Slottable){
						InventoryItemInstance item = (InventoryItemInstance)obj;
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
						InventoryItemInstance item = (InventoryItemInstance)obj;
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
				public void PointFocus(SlotSystemElement ele){
					/*	focus the given element and all those above it to the root
							if any element in the course is page element and is not toggled on, toggle it on and focus
							if any elemen in the course is bundle element and is not the focused element in the bundle, make it the focused element and focus it
					*/
					FindAndFocusInBundle(ele);
					if(ele.isPageElement){
						SlotSystemPage page = (SlotSystemPage)ele.parent;
						SlotSystemPageElement pageEle = page.GetPageElement(ele);
						if(!pageEle.isFocusToggleOn)
							page.TogglePageElementFocus(ele, true);
					}
					if(ele.isBundleElement){
						SlotSystemBundle bundle = (SlotSystemBundle)ele.parent;
						if(bundle.focusedElement != ele){
							bundle.SetFocusedBundleElement(ele);
							bundle.Focus();
						}
					}
				}
				public void PrePickFilter(Slottable sb, out bool isFilteredIn){
					bool res = false;
					foreach(SlotGroup targetSG in ssm.focusedSGs){
						if(ssm.GetTransaction(sb, targetSG).GetType() != typeof(RevertTransaction)){
							res = true; break;
						}
						foreach(Slottable targetSB in targetSG){
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
					public override SSEProcess selProcess{
						get{return (SSMSelProcess)selProcEngine.process;}
					}
					public override void SetAndRunSelProcess(SSEProcess process){
						if(process == null||process is SSMSelProcess)
							selProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("SlotSystemManager.SetAndRunSelProcess: argument is not of type SSMSelProcess");
					}
					public override IEnumeratorFake greyoutCoroutine(){
						return null;
					}
					public override IEnumeratorFake greyinCoroutine(){
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
				public void Initialize(SlotSystemPageElement pBundPageEle, SlotSystemPageElement eBundPageEle , IEnumerable<SlotSystemPageElement> gBundPageEles){
					m_eName = SlotSystemUtil.Bold("SSM");
					this.m_poolBundle = (SlotSystemBundle)pBundPageEle.element;
					this.m_equipBundle = (SlotSystemBundle)eBundPageEle.element;
					List<SlotSystemBundle> gBunds = new List<SlotSystemBundle>();
					foreach(SlotSystemPageElement pageEle in gBundPageEles){
						gBunds.Add((SlotSystemBundle)pageEle.element);
					}
					List<SlotSystemPageElement> pageEles = new List<SlotSystemPageElement>();
					pageEles.Add(pBundPageEle);
					pageEles.Add(eBundPageEle);
					foreach(SlotSystemPageElement pageEle in gBundPageEles){
						pageEles.Add(pageEle);
					}
					m_pageElements = pageEles;
					m_otherBundles = gBunds;
					PerformInHierarchy(SetSSM);
					PerformInHierarchy(SetParent);
					SetSelState(SlotSystemManager.ssmDeactivatedState);
					SetActState(SlotSystemManager.ssmWaitForActionState);
				}
				public virtual SlotSystemElement FindParent(SlotSystemElement ele){
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
					PageFocus();
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
					ToggleBack();
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
			public SlotGroup sg2{
				get{return m_sg2;}
				}SlotGroup m_sg2;
				public void SetSG2(SlotGroup sg){
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
			public SlotSystemElement hovered{
				get{return m_hovered;}
				}SlotSystemElement m_hovered;
				public void SetHovered(SlotSystemElement ele){
					if(ele == null || ele != hovered){
						if(hovered != null){
							if(hovered is Slottable)
								((Slottable)hovered).OnHoverExitMock();
							else if(hovered is SlotGroup)
								((SlotGroup)hovered).OnHoverExitMock();
						}
						m_hovered = ele;
					}
				}
			public Dictionary<SlotSystemElement, SlotSystemTransaction> transactionResultsV2;
			public void CreateTransactionResultsV2(){
				Dictionary<SlotSystemElement, SlotSystemTransaction> result = new Dictionary<SlotSystemElement, SlotSystemTransaction>();
				foreach(SlotGroup sg in focusedSGs){
					SlotSystemTransaction ta = AbsSlotSystemTransaction.GetTransaction(pickedSB, sg);
					result.Add(sg, ta);
					if(ta is RevertTransaction)
						sg.DefocusSelf();
					else
						sg.FocusSelf();
					foreach(Slottable sb in sg){
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
				this.transactionResultsV2 = result;
			}
			public void UpdateTransaction(){
				SlotSystemTransaction ta = null;
				if(transactionResultsV2.TryGetValue(hovered, out ta)){
					SetTargetSB(ta.targetSB);
					SetSG1(ta.sg1);
					SetSG2(ta.sg2);
					SetTransaction(ta);
				}
			}
			public void ReferToTAAndUpdateSelState(SlotGroup sg){
				if(transactionResultsV2 != null){
					SlotSystemTransaction ta = null;
					if(transactionResultsV2.TryGetValue(sg, out ta)){
						if(ta is RevertTransaction)
							sg.SetSelState(SlotGroup.sgDefocusedState);
						else
							sg.SetSelState(SlotGroup.sgFocusedState);
					}
				}else
					sg.SetSelState(SlotGroup.sgFocusedState);
			}
			public SlotSystemTransaction GetTransaction(Slottable pickedSB, SlotSystemElement hovered){
				return AbsSlotSystemTransaction.GetTransaction(pickedSB, hovered);
			}
	}
}
