using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections.Generic;
public class SlottableTest {


	// GameObject sbGO;

	PointerEventDataMock eventData = new PointerEventDataMock();
	GameObject sgmGO;
	SlotGroupManager sgm;
	GameObject sgpAllGO;
	SlotGroup sgpAll;
	GameObject sgBowGO;
	SlotGroup sgBow;
	GameObject sgWearGO;
	SlotGroup sgWear;
	GameObject sgpPartsGO;
	SlotGroup sgpParts;
	Slottable defBowBSB_p;
	Slottable defBowASB_p;
	Slottable crfBowASB_p;
	Slottable defWearBSB_p;
	Slottable defWearASB_p;
	Slottable crfWearASB_p;
	Slottable defPartsSB_p;
	Slottable crfPartsSB_p;
	Slottable defBowASB_e;
	Slottable defWearASB_e;
	Slottable defPartsSB_p2;
	Slottable crfPartsSB_p2;


	[SetUp]
	public void Setup(){	
		/*	SGM
		*/
			sgmGO = new GameObject("SlotGroupManager");
			sgm = sgmGO.AddComponent<SlotGroupManager>();
			sgm.SetupCommands();
			// sgm.SetupProcesses();
		/*	SGs
		*/
			/*	sgpAll
			*/
				sgpAllGO = new GameObject("PoolSlotGroup");
				sgpAll = sgpAllGO.AddComponent<SlotGroup>();
				sgpAll.Filter = new SGNullFilter();
				sgpAll.Sorter = new SGItemIndexSorter();
				sgpAll.UpdateEquipStatusCommand = new UpdateEquipStatusForPoolCommmand();
				PoolInventory inventory = new PoolInventory();
				sgpAll.SetInventory(inventory);
				sgpAll.IsShrinkable = true;
				sgpAll.IsExpandable = true;
			/*	sgBow
			*/
				sgBowGO = new GameObject("BowSlotGroup");
				sgBow = sgBowGO.AddComponent<SlotGroup>();
				sgBow.Filter = new SGBowFilter();
				sgBow.Sorter = new SGItemIndexSorter();
				sgBow.UpdateEquipStatusCommand = new UpdateEquipStatusForEquipSGCommand();
				EquipmentSetInventory equipInventory = new EquipmentSetInventory();
				sgBow.SetInventory(equipInventory);
				sgBow.IsShrinkable = false;
				sgBow.IsExpandable = false;

				Slot bowSlot = new Slot();
				bowSlot.Position = Vector2.zero;
				sgBow.Slots.Add(bowSlot);

			/*	sgWear
			*/
				sgWearGO = new GameObject("WearSlotGroup");
				sgWear = sgWearGO.AddComponent<SlotGroup>();
				sgWear.Filter = new SGWearFilter();
				sgWear.Sorter = new SGItemIndexSorter();
				sgWear.UpdateEquipStatusCommand = new UpdateEquipStatusForEquipSGCommand();
				sgWear.SetInventory(equipInventory);
				sgWear.IsShrinkable = false;
				sgWear.IsExpandable = false;

				Slot wearSlot = new Slot();
				wearSlot.Position = Vector2.zero;
				sgWear.Slots.Add(wearSlot);

			/*	sgpParts
			*/	
				sgpPartsGO = new GameObject("PartsSlotGroupPool");
				sgpParts = sgpPartsGO.AddComponent<SlotGroup>();
				sgpParts.Filter = new SGPartsFilter();
				sgpParts.Sorter = new SGItemIndexSorter();
				sgpParts.UpdateEquipStatusCommand = new UpdateEquipStatusForPoolCommmand();
				sgpParts.SetInventory(inventory);
				sgpParts.IsShrinkable = true;
				sgpParts.IsExpandable = true;

				
				
		/*	Items
		*/
			/*	bows
			*/
				BowMock defBow = new BowMock();
				defBow.ItemID = 1;
				BowMock crfBow = new BowMock();
				crfBow.ItemID = 2;
				
				BowInstanceMock defBowA = new BowInstanceMock();//	equipped
				defBowA.Item = defBow;
				sgpAll.Inventory.Add(defBowA);
				sgBow.Inventory.Add(defBowA);
				BowInstanceMock defBowB = new BowInstanceMock();
				defBowB.Item = defBow;
				sgpAll.Inventory.Add(defBowB);
				BowInstanceMock crfBowA = new BowInstanceMock();
				crfBowA.Item = crfBow;
				sgpAll.Inventory.Add(crfBowA);
			/*	wears
			*/
					WearMock defWear = new WearMock();
					defWear.ItemID = 101;
					WearMock crfWear = new WearMock();
					crfWear.ItemID = 102;
					
					WearInstanceMock defWearA = new WearInstanceMock();//	equipped
					defWearA.Item = defWear;
					sgpAll.Inventory.Add(defWearA);
					sgWear.Inventory.Add(defWearA);
					WearInstanceMock defWearB = new WearInstanceMock();
					defWearB.Item = defWear;
					sgpAll.Inventory.Add(defWearB);
					WearInstanceMock crfWearA = new WearInstanceMock();
					crfWearA.Item = crfWear;
					sgpAll.Inventory.Add(crfWearA);
			/*	parts
			*/ 
				PartsMock defParts = new PartsMock();
				defParts.ItemID = 601;
				defParts.IsStackable = true;
				PartsMock crfParts = new PartsMock();
				crfParts.ItemID = 602;
				crfParts.IsStackable = true;
				
				PartsInstanceMock defPartsA = new PartsInstanceMock();
				defPartsA.Item = defParts;
				defPartsA.Quantity = 10;
				sgpAll.Inventory.Add(defPartsA);
				PartsInstanceMock defPartsB = new PartsInstanceMock();
				defPartsB.Item = defParts;
				defPartsB.Quantity = 5;
				sgpAll.Inventory.Add(defPartsB);
				PartsInstanceMock crfPartsA = new PartsInstanceMock();
				crfPartsA.Item = crfParts;
				crfPartsA.Quantity = 3;
				sgpAll.Inventory.Add(crfPartsA);

				Assert.That(defPartsA, Is.EqualTo(defPartsB));
				Assert.That(object.ReferenceEquals(defPartsA, defPartsB), Is.False);
		/**/
		EquipmentSet equipSetA = new EquipmentSet(sgBow, sgWear);
		SlotSystemBundle equipBundle = new SlotSystemBundle();
		equipBundle.Elements.Add(equipSetA);
		equipBundle.SetFocusedBundleElement(equipSetA);
		SlotSystemBundle poolBundle = new SlotSystemBundle();
		poolBundle.Elements.Add(sgpAll);
		poolBundle.Elements.Add(sgpParts);
		poolBundle.SetFocusedBundleElement(sgpAll);
		InventoryManagerPage invManPage = new InventoryManagerPage(poolBundle, equipBundle);
	
		sgm.SetRootPage(invManPage);	
			/*	Assert Setup
				Preinitialized validation
				validate all the required fields are filled within the inspector window or at the time of declaration
			*/
			Assert.That(sgm.UpdateTransactionCommand, Is.Not.Null);
			Assert.That(sgm.PostPickFilterCommand, Is.Not.Null);
			// Assert.That(sgm.PrePickFilterCommand, Is.Not.Null);
			// Assert.That(sgm.ProbingStateProcess, Is.Not.Null);

			Assert.That(sgm.Transaction, Is.Null);

			Assert.That(sgm.PrevState, Is.EqualTo(SlotGroupManager.DeactivatedState));
			Assert.That(sgm.CurState, Is.EqualTo(SlotGroupManager.DeactivatedState));

			Assert.That(sgm.SelectedSB, Is.Null);
			Assert.That(sgm.SelectedSG, Is.Null);
			Assert.That(sgm.PickedSB, Is.Null);

			Assert.That(sgm.RootPage, Is.TypeOf(typeof(InventoryManagerPage)));
			AE(sgm.RootPage.SGM, sgm);
			Assert.That(sgm.RootPage.Elements.Count, Is.EqualTo(2));
			Assert.That(sgm.RootPage.ContainsElement(poolBundle));
			AE(poolBundle.SGM, sgm);
			Assert.That(sgm.RootPage.ContainsElement(equipBundle));
			AE(equipBundle.SGM, sgm);

			Assert.That(poolBundle.Elements.Count, Is.EqualTo(2));
			AB(poolBundle.ContainsElement(sgpAll), true);
			AE(sgpAll.SGM, sgm);
			AB(poolBundle.ContainsElement(sgpParts), true);
			AE(sgpParts.SGM, sgm);
			Assert.That(poolBundle.GetFocusedBundleElement(), Is.EqualTo(sgpAll));

			Assert.That(equipBundle.Elements.Count, Is.EqualTo(1));
			Assert.That(equipBundle.GetFocusedBundleElement(), Is.EqualTo(equipSetA));
			AE(equipSetA.SGM, sgm);

			Assert.That(equipSetA.Elements.Count, Is.EqualTo(2));
			Assert.That(equipSetA.ContainsElement(sgBow), Is.True);
			AE(sgBow.SGM, sgm);
			Assert.That(equipSetA.ContainsElement(sgWear), Is.True);
			AE(sgWear.SGM, sgm);
		

		sgm.Initialize();
			/*	Assert Initialiation
				when the scene is loaded, but not yet focused
			*/
			defBowASB_p = sgpAll.GetSlottable(defBowA);	
			defBowBSB_p = sgpAll.GetSlottable(defBowB);
			crfBowASB_p = sgpAll.GetSlottable(crfBowA);
			defWearASB_p = sgpAll.GetSlottable(defWearA);
			defWearBSB_p = sgpAll.GetSlottable(defWearB);
			crfWearASB_p = sgpAll.GetSlottable(crfWearA);
			defPartsSB_p = sgpAll.GetSlottable(defPartsA);
			crfPartsSB_p = sgpAll.GetSlottable(crfPartsA);
			defBowASB_e = sgBow.GetSlottable(defBowA);
			defWearASB_e = sgWear.GetSlottable(defWearA);
			defPartsSB_p2 = sgpParts.GetSlottable(defPartsA);
			crfPartsSB_p2 = sgpParts.GetSlottable(crfPartsA);

			Assert.That(sgm.CurState, Is.EqualTo(SlotGroupManager.DeactivatedState));
			/*	sgpAll
			*/
			Assert.That(sgpAll.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgpAll.FilteredItems.Count, Is.EqualTo(8));
			Assert.That(sgpAll.FilteredItems, Is.Ordered);
			Assert.That(sgpAll.Slots.Count, Is.EqualTo(8));
			foreach(Slot slot in sgpAll.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
				
				AE(sgm.GetSlotGroup(defBowASB_p), sgpAll);
				AE(sgm.GetSlotGroup(defBowBSB_p), sgpAll);
				AE(sgm.GetSlotGroup(crfBowASB_p), sgpAll);
				AE(sgm.GetSlotGroup(defWearASB_p), sgpAll);
				AE(sgm.GetSlotGroup(defWearBSB_p), sgpAll);
				AE(sgm.GetSlotGroup(crfWearASB_p), sgpAll);
				AE(sgm.GetSlotGroup(defPartsSB_p), sgpAll);
				AE(sgm.GetSlotGroup(crfPartsSB_p), sgpAll);
				
			/*	sgpParts
			*/
			Assert.That(sgpParts.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgpParts.FilteredItems.Count, Is.EqualTo(2));
			Assert.That(sgpParts.FilteredItems, Is.Ordered);
			Assert.That(sgpParts.Slots.Count, Is.EqualTo(2));
			foreach(Slot slot in sgpParts.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
				AE(sgm.GetSlotGroup(defPartsSB_p2), sgpParts);
				AE(sgm.GetSlotGroup(crfPartsSB_p2), sgpParts);
				
			/*	sgBow
			*/
			Assert.That(sgBow.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgBow.FilteredItems.Count, Is.EqualTo(1));
			Assert.That(sgBow.Slots.Count, Is.EqualTo(1));
			foreach(Slot slot in sgBow.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
			AE(sgm.GetSlotGroup(defBowASB_e), sgBow);
			InventoryItemInstanceMock defBowA_e_Inst = (InventoryItemInstanceMock)defBowASB_e.Item;
			AB(defBowA_e_Inst.IsEquipped, true);
			/*	sgWear
			*/
			Assert.That(sgWear.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgWear.FilteredItems.Count, Is.EqualTo(1));
			Assert.That(sgWear.Slots.Count, Is.EqualTo(1));
			foreach(Slot slot in sgWear.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
			AE(sgm.GetSlotGroup(defWearASB_e), sgWear);
			InventoryItemInstanceMock defWearA_e_Inst = (InventoryItemInstanceMock)defWearASB_e.Item;
			AB(defWearA_e_Inst.IsEquipped, true);

		sgm.Focus();
			/*	when the widget gets focus
			*/
			AssertSGMFocused();
			// AssertInstGICalled();
		sgm.Defocus();
			AssertSGMDefocus();
		sgm.Focus();
			AssertSGMFocused();
	}
	[Test]
	public void Test(){		
	
		TestOnPointerDownOnAllSB();

	}
	public void TestOnPointerDownOnAllSB(){
		foreach(Slottable sb in SlottableList()){
			TestOnPointerDownSequence(sb);
		}
	}
		public void TestOnPointerDownSequence(Slottable sb){
			sgm.RootPage.PoolBundle.SetFocusedBundleElement(sgpParts);
			TestDeactivatedState(sb);
			TestWaitForPointerUpState(sb);
			TestFocusedState(sb);
			TestEqDeselectedState(sb);
			TestWaitForPickUpState(sb);
			TestPickedUpAndSelectedState(sb);
			TestWaitForNextTouchWhilePUState(sb);

			sgm.RootPage.PoolBundle.SetFocusedBundleElement(sgpAll);
			TestDeactivatedState(sb);
			TestWaitForPointerUpState(sb);
			TestFocusedState(sb);
			TestEqDeselectedState(sb);
			TestWaitForPickUpState(sb);
			TestPickedUpAndSelectedState(sb);
			TestWaitForNextTouchWhilePUState(sb);
		}
	/*	supportives
	*/
		List<Slottable> SlottableList(){
			List<Slottable> sbList = new List<Slottable>();
			sbList.Add(defBowASB_p);
			sbList.Add(defBowBSB_p);
			sbList.Add(crfBowASB_p);
			sbList.Add(defWearASB_p);
			sbList.Add(defWearBSB_p);
			sbList.Add(crfWearASB_p);
			sbList.Add(defPartsSB_p);
			sbList.Add(crfPartsSB_p);
			sbList.Add(defBowASB_e);
			sbList.Add(defWearASB_e);
			sbList.Add(defPartsSB_p2);
			sbList.Add(crfPartsSB_p2);

			return sbList;
		}
		public void AssertTransactionFieldsAreCleared(Slottable sb){
			AE(sgm.CurState, SlotGroupManager.FocusedState);
			AE(sgm.CurProcess, null);
			AE(sgm.SBA, null);
			AE(sgm.SBB, null);
			AE(sgm.SGA, null);
			AE(sgm.SGB, null);
			AB(sgm.SBADoneTransaction, true);
			AB(sgm.SBBDoneTransaction, true);
			AB(sgm.SGADoneTransaction, true);
			AB(sgm.SGBDoneTransaction, true);

			AE(sb.DestinationSG, null);
			AE(sb.DestinationSlot, null);
			
		}
		public void AssertPostPickFilter(Slottable sb){
			ASSB(sb, Slottable.PickedUpAndSelectedState);
			SlotGroup origSG = sgm.GetSlotGroup(sb);
				ASSG(origSG, SlotGroup.SelectedState);
				
			AE(sgm.CurState, SlotGroupManager.ProbingState);
				AE(sgm.CurProcess.GetType(), typeof(SGMProbingStateProcess));
				AB(sgm.CurProcess.IsRunning, true);
				AB(sgm.CurProcess.IsExpired, false);
				AE(sgm.PickedSB, sb);
				AE(sgm.SelectedSB, sb);
				AE(sgm.SelectedSG, origSG);
			AE(sgm.Transaction.GetType(), typeof(RevertTransaction));

			SlotSystemBundle poolBundle = sgm.RootPage.PoolBundle;
			SlotSystemBundle equipBundle = sgm.RootPage.EquipBundle;
			if(poolBundle.ContainsElement(origSG)){// in the same bundle
				foreach(SlotSystemElement ele in poolBundle.Elements){
					if(ele is SlotGroup){
						SlotGroup sg = (SlotGroup)ele;
						if(sg != origSG){
							ASSG(sg, SlotGroup.DefocusedState);
							/*	sbs
							*/
								foreach(Slot slot in sg.Slots){
									if(slot.Sb.IsEquipped)
										ASSB(slot.Sb, Slottable.EquippedAndDefocusedState);
									else
										ASSB(slot.Sb, Slottable.DefocusedState);
								}
						}else{//	origSG is poolSGs memeber
							foreach(Slot slot in sg.Slots){
								if(slot.Sb == sb)
									ASSB(slot.Sb, Slottable.PickedUpAndSelectedState);
								else if(sg.AutoSort){
									if(slot.Sb.IsEquipped)
										ASSB(slot.Sb, Slottable.EquippedAndDefocusedState);
									else
										ASSB(slot.Sb, Slottable.DefocusedState);
								}else{//	non auto sort
									if(slot.Sb.IsEquipped)
										ASSB(slot.Sb, Slottable.EquippedAndDeselectedState);
									else
										ASSB(slot.Sb, Slottable.FocusedState);
								}
							}
						}
					}
				}
			}else{//	origSg not found in poolBundle
				for(int i = 0; i < equipBundle.Elements.Count; i++){
					EquipmentSet equipSet = (EquipmentSet)equipBundle.Elements[i];
					if(equipSet.ContainsElement(origSG)){// this page contains the origSG
						foreach(SlotSystemElement ele in equipSet.Elements){
							SlotGroup sg = (SlotGroup)ele;
							if(sg != origSG){// on the same page
								if(sg.AcceptsFilter(sb)){
									ASSG(sg, SlotGroup.FocusedState);
									ASSB(sg.Slots[0].Sb, Slottable.EquippedAndDeselectedState);
								}else{
									ASSG(sg, SlotGroup.DefocusedState);
									ASSB(sg.Slots[0].Sb, Slottable.EquippedAndDefocusedState);
								}
							}
						}
					}else{//	this page does not contain the origSG
						foreach(SlotSystemElement ele in equipSet.Elements){
							SlotGroup sg = (SlotGroup)ele;
							ASSG(sg, SlotGroup.DefocusedState);
							foreach(Slot slot in sg.Slots){
								if(slot.Sb != null){
									ASSB(slot.Sb, Slottable.EquippedAndDefocusedState);
								}
							}
						}
					}
				}
			}
		}
		public void AssertSGMFocused(){
			AssertTransactionFieldsAreClearedOnAllSB();
			AE(sgm.CurState, SlotGroupManager.FocusedState);
			if(sgm.RootPage.PoolBundle.GetFocusedBundleElement() == (SlotSystemElement)sgpAll){
				AE(sgpAll.CurState, SlotGroup.FocusedState);
					ASSB(defBowASB_p, Slottable.EquippedAndDeselectedState);
					ASSB(defBowBSB_p, Slottable.FocusedState);
					ASSB(crfBowASB_p, Slottable.FocusedState);
					ASSB(defWearASB_p, Slottable.EquippedAndDeselectedState);
					ASSB(defWearBSB_p, Slottable.FocusedState);
					ASSB(crfWearASB_p, Slottable.FocusedState);
					ASSB(defPartsSB_p, Slottable.DefocusedState);
					ASSB(crfPartsSB_p, Slottable.DefocusedState);
				AE(sgpParts.CurState, SlotGroup.DefocusedState);
					ASSB(defPartsSB_p2, Slottable.DefocusedState);
					ASSB(crfPartsSB_p2, Slottable.DefocusedState);
			}else{
				AE(sgpAll.CurState, SlotGroup.DefocusedState);
					ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowBSB_p, Slottable.DefocusedState);
					ASSB(crfBowASB_p, Slottable.DefocusedState);
					ASSB(defWearASB_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearBSB_p, Slottable.DefocusedState);
					ASSB(crfWearASB_p, Slottable.DefocusedState);
					ASSB(defPartsSB_p, Slottable.DefocusedState);
					ASSB(crfPartsSB_p, Slottable.DefocusedState);
				AE(sgpParts.CurState, SlotGroup.FocusedState);
					ASSB(defPartsSB_p2, Slottable.FocusedState);
					ASSB(crfPartsSB_p2, Slottable.FocusedState);
			}
			AE(sgBow.CurState, SlotGroup.FocusedState);
				ASSB(defBowASB_e, Slottable.EquippedAndDeselectedState);
			AE(sgWear.CurState, SlotGroup.FocusedState);
				ASSB(defWearASB_e, Slottable.EquippedAndDeselectedState);
		}
		public void AssertSGMDefocus(){
			AE(sgm.CurState, SlotGroupManager.DefocusedState);

			AE(sgpAll.CurState, SlotGroup.DefocusedState);
				ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
				ASSB(defBowBSB_p, Slottable.DefocusedState);
				ASSB(crfBowASB_p, Slottable.DefocusedState);
				ASSB(defWearASB_p, Slottable.EquippedAndDefocusedState);
				ASSB(defWearBSB_p, Slottable.DefocusedState);
				ASSB(crfWearASB_p, Slottable.DefocusedState);

				AB(((InventoryItemInstanceMock)defPartsSB_p.Item).IsEquipped, false);
				AE(sgm.GetSlotGroup(defPartsSB_p), sgpAll);

				ASSB(defPartsSB_p, Slottable.DefocusedState);
				ASSB(crfPartsSB_p, Slottable.DefocusedState);
			AE(sgpParts.CurState, SlotGroup.DefocusedState);
				ASSB(defPartsSB_p2, Slottable.DefocusedState);
				ASSB(crfPartsSB_p2, Slottable.DefocusedState);
			AE(sgBow.CurState, SlotGroup.DefocusedState);
				ASSB(defBowASB_e, Slottable.EquippedAndDefocusedState);
			AE(sgWear.CurState, SlotGroup.DefocusedState);
				ASSB(defWearASB_e, Slottable.EquippedAndDefocusedState);

		}
		public void AssertTransactionFieldsAreClearedOnAllSB(){
			foreach(Slottable sb in SlottableList()){
				AssertTransactionFieldsAreCleared(sb);
			}
		}
	/*	states test
	*/
		public void TestDeactivatedState(Slottable sb){
			
			sgm.Deactivate();

			ASSB(sb, Slottable.DeactivatedState);
			ASSG(sgm.GetSlotGroup(sb), SlotGroup.DeactivatedState);

			ASSB(sb, Slottable.DeactivatedState);

			sb.OnPointerDownMock(eventData);

			ASSB(sb, Slottable.DeactivatedState);
			AE(sb.CurProcess, null);
			
			sb.OnPointerUpMock(eventData);
			ASSB(sb, Slottable.DeactivatedState);
			AE(sb.CurProcess, null);

			sgm.Focus();
			AssertSGMFocused();
		}
		public void TestWaitForPointerUpState(Slottable sb){
			sgm.Defocus();
			AssertSGMDefocus();
			sb.OnPointerDownMock(eventData);


			ASSB(sb, Slottable.WaitForPointerUpState);
			AE(sb.CurProcess.GetType(), typeof(WaitForPointerUpProcess));
			AB(sb.CurProcess.IsRunning, true);
			AB(sb.CurProcess.IsExpired, false);
			/*	OnEndDrag
			*/
				sb.OnEndDragMock(eventData);
				if(sb.IsEquipped){
					ASSB(sb, Slottable.EquippedAndDefocusedState);
					AE(sb.CurProcess, null);
				}else{
					ASSB(sb, Slottable.DefocusedState);
					AE(sb.CurProcess, null);
				}
			
			sb.OnPointerDownMock(eventData);

			/*	OnPointerUp
			*/
				AB(sb.Tapped, false);
				sb.OnPointerUpMock(eventData);
				AB(sb.Tapped, true);
				sb.Tapped = false;
				if(sb.IsEquipped){
					ASSB(sb, Slottable.EquippedAndDefocusedState);
					AE(sb.CurProcess, null);
				}else{
					ASSB(sb, Slottable.DefocusedState);
					AE(sb.CurProcess, null);
				}
			sgm.Focus();

		}
		public void TestFocusedState(Slottable sb){
			sgm.Deactivate();
			sgm.Focus();
			if(sb.CurState == Slottable.FocusedState){
				AE(sb.CurProcess, null);
			}
			sgm.Defocus();
			sgm.Focus();
			if(sb.CurState == Slottable.FocusedState){
				AE(sb.CurProcess.GetType(), typeof(GradualGrayinProcess));
				AB(sb.CurProcess.IsRunning, true);
				AB(sb.CurProcess.IsExpired, false);

				sb.OnPointerDownMock(eventData);
					ASSB(sb,Slottable.WaitForPickUpState);
					AE(sb.CurProcess.GetType(), typeof(WaitForPickUpProcess));
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				
				// sb.CurProcess.Expire();
				// 	ASSB(sb, Slottable.PickedUpAndSelectedState);
				sb.OnPointerUpMock(eventData);
				if(sb.Item.IsStackable){
					ASSB(sb, Slottable.WaitForNextTouchState);
					AE(sb.CurProcess.GetType(), typeof(WaitForNextTouchProcess));
					AB(sb.Tapped, false);
					sb.CurProcess.Expire();
					AB(sb.Tapped, true);
					sb.Tapped = false;
					ASSB(sb, Slottable.FocusedState);
				}else{
					ASSB(sb, Slottable.FocusedState);
					AB(sb.Tapped, true);
					sb.Tapped = false;
				}
			}
		}
		public void TestEqDeselectedState(Slottable sb){
			sgm.Deactivate();
			sgm.Focus();
			if(sb.CurState == Slottable.EquippedAndDeselectedState){
				AE(sb.CurProcess, null);
			}
			sgm.Defocus();
			sgm.Focus();
			if(sb.CurState == Slottable.EquippedAndDeselectedState){
				AE(sb.CurProcess.GetType(), typeof(EquipGradualGrayinProcess));
				AB(sb.CurProcess.IsRunning, true);
				AB(sb.CurProcess.IsExpired, false);

				sb.OnPointerDownMock(eventData);
					ASSB(sb,Slottable.WaitForPickUpState);
					AE(sb.CurProcess.GetType(), typeof(WaitForPickUpProcess));
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				
				// sb.CurProcess.Expire();
				// 	ASSB(sb, Slottable.PickedUpAndSelectedState);
				sb.OnPointerUpMock(eventData);
				if(sb.Item.IsStackable){
					
					ASSB(sb, Slottable.WaitForNextTouchState);
					AE(sb.CurProcess.GetType(), typeof(WaitForNextTouchProcess));
					
					sb.CurProcess.Expire();
					AB(sb.Tapped, true);
					sb.Tapped = false;
					ASSB(sb, Slottable.EquippedAndDeselectedState);
				}else{
					ASSB(sb, Slottable.EquippedAndDeselectedState);
					AB(sb.Tapped, true);
					sb.Tapped = false;
				}
			}
		}
		public void TestWaitForPickUpState(Slottable sb){
		AssertSGMFocused();

			if(sb.CurState == Slottable.FocusedState || sb.CurState == Slottable.EquippedAndDeselectedState){
				sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPickUpState);
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				/*	1.0	Expire
				*/
				sb.CurProcess.Expire();
					ASSB(sb, Slottable.PickedUpAndSelectedState);
					AssertPostPickFilter(sb);
					AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
				/**/
				sb.OnPointerUpMock(eventData);
				
				if(sb.Item.IsStackable){
					ASSB(sb, Slottable.WaitForNextTouchWhilePUState);
					AE(sb.CurProcess.GetType(), typeof(WaitForNextTouchWhilePUProcess));
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
					/**/
					sb.CurProcess.Expire();

					AssertReverting(sb);
					
					ASSB(sb, Slottable.MovingState);
					AE(sb.CurProcess.GetType(), typeof(MoveProcess));
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				}else{
					// AB(sgm.CurProcess != null, true);
					AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
					AssertReverting(sb);

					ASSB(sb, Slottable.MovingState);
					AE(sb.CurProcess.GetType(), typeof(MoveProcess));
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);

				}

				sb.CurProcess.Expire();
				AssertTransactionFieldsAreCleared(sb);
				if(sb.IsEquipped)
					ASSB(sb, Slottable.EquippedAndDeselectedState);
				else
					ASSB(sb, Slottable.FocusedState);
				AssertSGMFocused();

				/*	1.1 OnEndDrag
				*/	
				sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPickUpState);
				sb.OnEndDragMock(eventData);
					if(sb.IsEquipped)
						ASSB(sb, Slottable.EquippedAndDeselectedState);
					else
						ASSB(sb, Slottable.FocusedState);
				AssertSGMFocused();
				/*	1.2	OnPointerUp
				*/
				sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPickUpState);
				sb.OnPointerUpMock(eventData);
				if(sb.Item.IsStackable){
					ASSB(sb, Slottable.WaitForNextTouchState);
					AB(sb.Tapped, false);
					sb.CurProcess.Expire();
					AB(sb.Tapped, true);
					sb.Tapped = false;

				}else{
					AB(sb.Tapped, true);
					sb.Tapped = false;
					if(sb.IsEquipped)
						ASSB(sb, Slottable.EquippedAndDeselectedState);
					else
						ASSB(sb, Slottable.FocusedState);
				}
				AssertSGMFocused();
				/*	
				*/

			}
		}
			public void AssertReverting(Slottable sb){
				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
				AE(sgm.CurProcess.GetType(), typeof(SGMRevertTransactionProcess));
				AB(sgm.CurProcess.IsRunning, true);
				AB(sgm.CurProcess.IsExpired, false);
				AE(sgm.SBA, sb);
				AE(sgm.SBB, null);
				AE(sgm.SGA, null);
				AE(sgm.SGB, null);
				AB(sgm.SBADoneTransaction, false);
				AB(sgm.SBBDoneTransaction, true);
				AB(sgm.SGADoneTransaction, true);
				AB(sgm.SGBDoneTransaction, true);
				
				AE(sb.DestinationSG, sgm.GetSlotGroup(sb));
				AB(sb.DestinationSlot != null, true);
			}
		public void TestPickedUpAndSelectedState(Slottable sb){
			if(sb.CurState == Slottable.FocusedState || sb.CurState == Slottable.EquippedAndDeselectedState){
				sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPickUpState);
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				
				sb.CurProcess.Expire();
					ASSB(sb, Slottable.PickedUpAndSelectedState);
					AssertPostPickFilter(sb);
					AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
				/*	1.0 OnPointerUp --> TestWaitForPickUpState
				*/
				/*	1.1 OnEndDrag
				*/
				sb.OnEndDragMock(eventData);
					ASSB(sb, Slottable.MovingState);
					AssertReverting(sb);
					sb.CurProcess.Expire();
					AssertSGMFocused();
			}
		}
		public void TestWaitForNextTouchWhilePUState(Slottable sb){

			if(sb.CurState == Slottable.FocusedState || sb.CurState == Slottable.EquippedAndDeselectedState){
				sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPickUpState);
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				
				sb.CurProcess.Expire();
					ASSB(sb, Slottable.PickedUpAndSelectedState);
					AssertPostPickFilter(sb);
					AE(sgm.Transaction.GetType(), typeof(RevertTransaction));

				sb.OnPointerUpMock(eventData);
				
				if(sb.Item.IsStackable){
					ASSB(sb, Slottable.WaitForNextTouchWhilePUState);
					AE(sb.CurProcess.GetType(), typeof(WaitForNextTouchWhilePUProcess));
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
					/*	1.0 Expire --> TestWaitForPickUpState
					*/
					AE(sb.PickedAmount, 1);
					/*	1.1	OnPointerDwon
					*/
					sb.OnPointerDownMock(eventData);
					ASSB(sb, Slottable.PickedUpAndSelectedState);
					AE(sb.PickedAmount, 2);

					sb.OnPointerUpMock(eventData);
					ASSB(sb, Slottable.WaitForNextTouchWhilePUState);
					/*	1.2 OnDeselect
					*/
					sb.OnDeselectedMock(eventData);
					ASSB(sb, Slottable.MovingState);
					AssertReverting(sb);
					sb.CurProcess.Expire();
					AssertSGMFocused();
					/*	1.3
					*/
					
				}else{
					AssertReverting(sb);
					sb.CurProcess.Expire();
					AssertSGMFocused();
				}
			}
		}
		public void TestDefocusedState(Slottable sb){

		}
		public void TestEquippedAndDefocusedState(Slottable sb){
			
		}
	/*
	*/
	public void TestSimSBHover(Slottable pickedSb, Slottable hoveredSb){
		if(pickedSb != null && pickedSb.CurState == Slottable.PickedUpAndSelectedState){

			SlotGroup targetSG = sgm.GetSlotGroup(hoveredSb);
			SlotGroup origSG = sgm.GetSlotGroup(pickedSb);
			SlotGroup selectedSG = pickedSb.SGM.SelectedSG;
			sgm.SimSBHover(hoveredSb, eventData);
			if(hoveredSb == null){
					Assert.That(pickedSb.CurState, Is.EqualTo(Slottable.PickedUpAndDeselectedState));
				if(selectedSG == null || selectedSG == origSG){
					Assert.That(sgm.Transaction, Is.TypeOf(typeof(RevertTransaction)));
				}else{
					if(selectedSG.HasItem((InventoryItemInstanceMock)pickedSb.Item)){
						Assert.That(sgm.Transaction, Is.TypeOf(typeof(StackTransaction)));
					}else{
						Assert.That(sgm.Transaction, Is.TypeOf(typeof(FillTransaction)));
					}
				}
			}else{
				if(targetSG.AutoSort && targetSG == origSG){
					if(hoveredSb == pickedSb){
						Assert.That(hoveredSb.CurState, Is.EqualTo(Slottable.PickedUpAndSelectedState));
						Assert.That(sgm.SelectedSB, Is.EqualTo(hoveredSb));
						Assert.That(sgm.SelectedSB, Is.EqualTo(sgm.PickedSB));
						Assert.That(pickedSb.CurState, Is.EqualTo(Slottable.PickedUpAndSelectedState));
					}else{
						Assert.That(pickedSb.CurState, Is.EqualTo(Slottable.PickedUpAndDeselectedState));
						if(object.ReferenceEquals(hoveredSb.Item, sgm.GetEquippedBow()) || object.ReferenceEquals(hoveredSb.Item, sgm.GetEquippedWear())){
						
							Assert.That(hoveredSb.CurState, Is.EqualTo(Slottable.EquippedAndDefocusedState));
						} 
						else
							Assert.That(hoveredSb.CurState, Is.EqualTo(Slottable.DefocusedState));

						Assert.That(sgm.SelectedSB, Is.EqualTo(null));
					}

					Assert.That(sgm.Transaction, Is.TypeOf(typeof(RevertTransaction)));
				}else{//hoveredSb != null && (!AutoSort OR differentSG)
					if(pickedSb == hoveredSb){
						Assert.That(sgm.SelectedSB, Is.EqualTo(hoveredSb));
						Assert.That(hoveredSb.CurState, Is.EqualTo(Slottable.PickedUpAndSelectedState));
					}else{
						if((pickedSb.Item is BowInstanceMock && hoveredSb.Item is BowInstanceMock)|| (pickedSb.Item is WearInstanceMock && hoveredSb.Item is WearInstanceMock) || (pickedSb.Item is PartsInstanceMock && hoveredSb.Item is PartsInstanceMock)){
							if(object.ReferenceEquals(sgm.GetEquippedBow(), hoveredSb.Item)||(object.ReferenceEquals(sgm.GetEquippedWear(), hoveredSb.Item))){
								Assert.That(sgm.SelectedSB, Is.EqualTo(hoveredSb));
								Assert.That(hoveredSb.CurState, Is.EqualTo(Slottable.EquippedAndSelectedState));
								Assert.That(pickedSb.CurState, Is.EqualTo(Slottable.PickedUpAndDeselectedState));
							}else{//hoveredSb not equipped
								Assert.That(sgm.SelectedSB, Is.EqualTo(hoveredSb));
								Assert.That(hoveredSb.CurState, Is.EqualTo(Slottable.SelectedState));
								Assert.That(pickedSb.CurState, Is.EqualTo(Slottable.PickedUpAndDeselectedState));
							}
						}else{//different item family
							Assert.That(sgm.SelectedSB, Is.EqualTo(null));
							Assert.That(pickedSb.CurState, Is.EqualTo(Slottable.PickedUpAndDeselectedState));

							if(object.ReferenceEquals(hoveredSb.Item, sgm.GetEquippedBow())|| object.ReferenceEquals(hoveredSb.Item, sgm.GetEquippedWear())){
							
								Assert.That(hoveredSb.CurState, Is.EqualTo(Slottable.EquippedAndDefocusedState));
							
							}else{//hoveredSb not equipped
							
								Assert.That(hoveredSb.CurState, Is.EqualTo(Slottable.DefocusedState));
							}
						}
					}
					
					// Assert.That(sgm.SelectedSB, Is.EqualTo(hoveredSb));
					// if(object.ReferenceEquals(hoveredSb.Item, sgm.GetEquippedBow()) || object.ReferenceEquals(hoveredSb.Item, sgm.GetEquippedWear()))
					// 	Assert.That(hoveredSb.CurState, Is.EqualTo(Slottable.EquippedAndSelectedState));
					// else
					// 	Assert.That(hoveredSb.CurState, Is.EqualTo(Slottable.SelectedState));

					// Assert.That(sgm.PickedSB, Is.EqualTo(pickedSb));
					// Assert.That(pickedSb.CurState, Is.EqualTo(Slottable.PickedUpAndDeselectedState));
					// Assert.That(pickedSb.PickedUpAndSelectedProcess.IsRunning, Is.False);
					// Assert.That(pickedSb.PickedUpAndSelectedProcess.IsExpired, Is.False);
					if(targetSG == origSG){
						Assert.That(sgm.Transaction, Is.TypeOf(typeof(ReorderTransaction)));
					}else{
						if(hoveredSb.Item == pickedSb.Item)
							Assert.That(sgm.Transaction, Is.TypeOf(typeof(StackTransaction)));
						else{
							if(sgm.SelectedSB != null)
								Assert.That(sgm.Transaction, Is.TypeOf(typeof(SwapTransaction)));
							else
								Assert.That(sgm.Transaction, Is.TypeOf(typeof(RevertTransaction)));
						}
					}

				}
			}
		}
	}
	public void ValidatePostpickFilter(Slottable pickedSB){
		if(pickedSB != null && pickedSB.CurState == Slottable.PickedUpAndSelectedState){

			SlotGroup origSG = sgm.GetSlotGroup(pickedSB);
			
			if(origSG.Filter is SGNullFilter){
				foreach(SlotGroup sg in sgm.SlotGroups){
					if(sg == origSG){
						Assert.That(sg.CurState, Is.EqualTo(SlotGroup.SelectedState));
						foreach(Slot slot in sg.Slots){
							if(slot.Sb != null){
								if(pickedSB == slot.Sb)
									Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.PickedUpAndSelectedState));
								else{
									BowInstanceMock equippedBow = (BowInstanceMock)sgBow.Slots[0].Sb.Item;
									WearInstanceMock equippedWear = (WearInstanceMock)sgWear.Slots[0].Sb.Item;
									if(sg.AutoSort){
										if(object.ReferenceEquals(slot.Sb.Item, equippedBow) || (object.ReferenceEquals(slot.Sb.Item, equippedWear)))

											Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDefocusedState));
										else
											Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));
									}else{
										if(object.ReferenceEquals(slot.Sb.Item, equippedBow) || (object.ReferenceEquals(slot.Sb.Item, equippedWear)))
											
											Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
										else
											Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.FocusedState));

									}		
								}
							}
						}
					}else{// if SG under examination is not the origSG
						if(sg.IsPool){
							Assert.That(sg.CurState, Is.EqualTo(SlotGroup.DefocusedState));
							
						}else{
							if(sgm.PickedSB.Item is BowInstanceMock){
								if(sg.Filter is SGBowFilter)
									Assert.That(sg.CurState, Is.EqualTo(SlotGroup.FocusedState));
								else
									Assert.That(sg.CurState, Is.EqualTo(SlotGroup.DefocusedState));
							}else if(sgm.PickedSB.Item is WearInstanceMock){
								if(sg.Filter is SGWearFilter)
									Assert.That(sg.CurState, Is.EqualTo(SlotGroup.FocusedState));
								else
									Assert.That(sg.CurState, Is.EqualTo(SlotGroup.DefocusedState));
							}else if(sgm.PickedSB.Item is PartsInstanceMock){
								if(sg.Filter is SGPartsFilter)
									Assert.That(sg.CurState, Is.EqualTo(SlotGroup.FocusedState));
								else
									Assert.That(sg.CurState, Is.EqualTo(SlotGroup.DefocusedState));
							}
						}
					}
				}
			}else if(origSG.Filter is SGBowFilter && !origSG.IsPool){
				
				Assert.That(origSG, Is.EqualTo(sgBow));

				Assert.That(pickedSB.Item, Is.TypeOf(typeof(BowInstanceMock)));
				Assert.That(sgBow.CurState, Is.EqualTo(SlotGroup.SelectedState));
				/*	Validate sgBow
				*/
					Assert.That(sgBow.Slots.Count, Is.EqualTo(1));
					Assert.That(sgBow.Slots[0].Sb.CurState, Is.EqualTo(Slottable.PickedUpAndSelectedState));
				/*	Validate sgWear
				*/
					Assert.That(sgWear.Slots.Count, Is.EqualTo(1));
					Assert.That(sgWear.Slots[0].Sb.CurState, Is.EqualTo(Slottable.EquippedAndDefocusedState));
				/*	Validate sgpAll
				*/
					Assert.That(sgpAll.CurState, Is.EqualTo(SlotGroup.FocusedState));
					foreach(Slot slot in sgpAll.Slots){
						if(slot.Sb != null){
							if(slot.Sb.Item is PartsInstanceMock)
								Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));
							else if(object.ReferenceEquals(slot.Sb.Item, sgBow.Slots[0].Sb.Item))
								Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDefocusedState));
							else if(object.ReferenceEquals(slot.Sb.Item, sgWear.Slots[0].Sb.Item))
								Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDefocusedState));
							else if(slot.Sb == sgm.PickedSB)
								Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.PickedUpAndSelectedState));
							else if(slot.Sb.Item is BowInstanceMock)
								Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.FocusedState));
							else 
								Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));
						}
					}
				/*	Validate sgpParts
				*/
					Assert.That(sgpParts.CurState, Is.EqualTo(SlotGroup.DefocusedState));
					foreach(Slot slot in sgpParts.Slots){
						if(slot.Sb != null){
							Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));
						}
					}
				
			}else if(origSG.Filter is SGWearFilter){
				Assert.That(pickedSB.Item, Is.TypeOf(typeof(WearInstanceMock)));
				Assert.That(sgBow.CurState, Is.EqualTo(SlotGroup.DefocusedState));
				foreach(Slot slot in sgBow.Slots){
					if(slot.Sb != null){
						Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDefocusedState));
					}
				}
				Assert.That(sgWear.CurState, Is.EqualTo(SlotGroup.SelectedState));
				foreach(Slot slot in sgWear.Slots){
					if(slot.Sb != null){
						Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.PickedUpAndSelectedState));
					}
				}
				Assert.That(sgpAll.CurState, Is.EqualTo(SlotGroup.FocusedState));
				foreach(Slot slot in sgpAll.Slots){
					if(slot.Sb != null){
						if(slot.Sb.Item is PartsInstanceMock)
							Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));
						else if(object.ReferenceEquals(slot.Sb.Item, sgBow.Slots[0].Sb.Item))
							Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDefocusedState));
						else if(object.ReferenceEquals(slot.Sb.Item, sgWear.Slots[0].Sb.Item))
							Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDefocusedState));//needs to be defocused?
						else if(slot.Sb == sgm.PickedSB)//	never happens
							Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.PickedUpAndSelectedState));
						else if(slot.Sb.Item is WearInstanceMock)
							Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.FocusedState));
						else 
							Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));

					}
				}
				Assert.That(sgpParts.CurState, Is.EqualTo(SlotGroup.DefocusedState));
				foreach(Slot slot in sgpParts.Slots){
					if(slot.Sb != null){
						Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));
					}
				}
				
			}else if(origSG.Filter is SGPartsFilter){
				foreach(SlotGroup sg in sgm.SlotGroups){
					if(sg == origSG){
						foreach(Slot slot in sg.Slots){
							if(slot.Sb != null){
								if(slot.Sb == pickedSB){
									Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.PickedUpAndSelectedState));
								}else{
									if(sg.AutoSort)
										Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));
									else
										Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.FocusedState));
								}
							}
						}
					}else{// the sg under inspection is not the orig sg
						if(origSG.IsPool){
							if(sg.IsPool){//multiple pool sgs cannot be focused at the same time
								Assert.That(sg.CurState, Is.EqualTo(SlotGroup.DefocusedState));
							}else{// sg not pool
								if(sg.Filter is SGPartsFilter){
									Assert.That(sg.CurState, Is.EqualTo(SlotGroup.FocusedState));
								}else
									Assert.That(sg.CurState, Is.EqualTo(SlotGroup.DefocusedState));
							}
						}else{// orig not pool
							if(sg.Filter is SGNullFilter || sg.Filter is SGPartsFilter){
								Assert.That(sg.CurState, Is.EqualTo(SlotGroup.FocusedState));
							}else{
								Assert.That(sg.CurState, Is.EqualTo(SlotGroup.DefocusedState));
							
							}
						}
					}
				}
			}
		}
	}
	/*	Assertions
	*/
		public void AssertState<T>() where T: SlottableState{
			Assert.That(defBowASB_p.CurState.GetType(), Is.EqualTo(typeof(T)));
		}
		public void AssertState(Slottable sb, SlottableState sbState){
			Assert.That(sb.CurState, Is.EqualTo(sbState));
		}
		public void AssertPickQuantity(int quant){
			Assert.That(defBowASB_p.PickedAmount, Is.EqualTo(quant));
		}
		public void AE(object inspected, object expected){
			Assert.That(inspected, Is.EqualTo(expected));
		}
		public void AB(bool inspectedBool, bool value){
			if(value)
				Assert.That(inspectedBool, Is.True);
			else
				Assert.That(inspectedBool, Is.False);
		}
		public void AB(object inspected, bool isNull){
			if(isNull)
				Assert.That(inspected, Is.Null);
			else
				Assert.That(inspected, Is.Not.Null);
		}
		public void ASSB(Slottable sb, SlottableState state){
			AE(sb.CurState, state);
		}
		public void ASSG(SlotGroup sg, SlotGroupState state){
			AE(sg.CurState, state);
		}
}
