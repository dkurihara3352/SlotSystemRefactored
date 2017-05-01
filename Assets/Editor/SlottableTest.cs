using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;

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
			sgm.SetupProcesses();
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
		SlotGroupBundle equipBundle = new SlotGroupBundle();
		equipBundle.Elements.Add(equipSetA);
		equipBundle.SetFocusedBundleElement(equipSetA);
		SlotGroupBundle poolBundle = new SlotGroupBundle();
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
			Assert.That(sgm.PrePickFilterCommand, Is.Not.Null);
			Assert.That(sgm.ProbingStateProcess, Is.Not.Null);

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

			Assert.That(sgm.CurState, Is.EqualTo(SlotGroupManager.DefocusedState));
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


		// Assert.That(sgm.CurState, Is.EqualTo(SlotGroupManager.FocusedState));
		// Assert.That(sgm.PrevState, Is.EqualTo(SlotGroupManager.DefocusedState));

		// /**/
			// defBowASB_p = sgpAll.GetSlottable(defBowA);	
			// defBowBSB_p = sgpAll.GetSlottable(defBowB);
			// crfBowASB_p = sgpAll.GetSlottable(crfBowA);
			// defWearASB_p = sgpAll.GetSlottable(defWearA);
			// defWearBSB_p = sgpAll.GetSlottable(defWearB);
			// crfWearASB_p = sgpAll.GetSlottable(crfWearA);
			// defPartsSB_p = sgpAll.GetSlottable(defPartsA);
			// crfPartsSB_p = sgpAll.GetSlottable(crfPartsA);
			// defBowASB_e = sgBow.GetSlottable(defBowA);
			// defWearASB_e = sgWear.GetSlottable(defWearA);
			// defPartsSB_p2 = sgpParts.GetSlottable(defPartsA);
			// crfPartsSB_p2 = sgpParts.GetSlottable(crfPartsA);
	}
	public void AssertSGMFocused(){
		AE(sgm.CurState, SlotGroupManager.FocusedState);

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
		AE(sgBow.CurState, SlotGroup.FocusedState);
			ASSB(defBowASB_e, Slottable.EquippedAndDeselectedState);
		AE(sgWear.CurState, SlotGroup.FocusedState);
			ASSB(defWearASB_e, Slottable.EquippedAndDeselectedState);
	}
	/*try this again after everything else*/
	public void TestSBSelectedState(){
		Assert.That(Slottable.SelectedState.GetType(), Is.EqualTo(typeof(SBSelectedState)));
		defBowASB_p.SetState(Slottable.SelectedState);
		AssertState(defBowASB_p, Slottable.SelectedState);
	}
	[Test]
	public void Test(){		
		// TestAllSbHoverOnAllSB();
	}
	public void TestAllSbHoverOnAllSB(){
		// TestHoverOnAllSB(defBowASB_p);
		// TestHoverOnAllSB(defBowBSB_p);
		// TestHoverOnAllSB(crfBowASB_p);
		// TestHoverOnAllSB(defWearASB_p);
		// TestHoverOnAllSB(defWearBSB_p);
		// TestHoverOnAllSB(crfWearASB_p);
		// TestHoverOnAllSB(defPartsSB_p);
		// TestHoverOnAllSB(crfPartsSB_p);
		// TestHoverOnAllSB(defBowASB_e);
		// TestHoverOnAllSB(defWearASB_e);

		TestPartsSBsHoverOnAllSB(defPartsSB_p2);
		// TestPartsSBsHoverOnAllSB(crfPartsSB_p2);

	}
	public void TestPartsSBsHoverOnAllSB(Slottable partsSb){
		sgpAll.SetState(SlotGroup.DefocusedState);
		sgpParts.SetState(SlotGroup.FocusedState);
		sgm.PrePickFilter();
		TestHover(partsSb, defBowASB_e);
		sgpAll.SetState(SlotGroup.DefocusedState);
		sgpParts.SetState(SlotGroup.FocusedState);
		sgm.PrePickFilter();
		TestHover(partsSb, defWearASB_e);
		sgpAll.SetState(SlotGroup.DefocusedState);
		sgpParts.SetState(SlotGroup.FocusedState);
		sgm.PrePickFilter();
		TestHover(partsSb, defPartsSB_p2);
		sgpAll.SetState(SlotGroup.DefocusedState);
		sgpParts.SetState(SlotGroup.FocusedState);
		sgm.PrePickFilter();
		TestHover(partsSb, crfPartsSB_p2);
	}
	public void TestHoverOnAllSB(Slottable pickedSB){
		TestHover(pickedSB, defBowASB_p);
		TestHover(pickedSB, defBowBSB_p);
		TestHover(pickedSB, crfBowASB_p);
		TestHover(pickedSB, defWearASB_p);
		TestHover(pickedSB, defWearBSB_p);
		TestHover(pickedSB, crfWearASB_p);
		TestHover(pickedSB, defPartsSB_p);
		TestHover(pickedSB, crfPartsSB_p);

		TestHover(pickedSB, defBowASB_e);
		TestHover(pickedSB, defWearASB_e);

		// TestHover(pickedSB, defBowBSB_p);
		// TestHover(pickedSB, defBowBSB_p);
	}
	public void TestPADeselAll(){
		TestPickedUpAndDeselectedState(defBowASB_p);
		TestPickedUpAndDeselectedState(defBowBSB_p);
		TestPickedUpAndDeselectedState(crfBowASB_p);
		TestPickedUpAndDeselectedState(defWearASB_p);
		TestPickedUpAndDeselectedState(defWearBSB_p);
		TestPickedUpAndDeselectedState(crfWearASB_p);
		TestPickedUpAndDeselectedState(defBowASB_e);
		TestPickedUpAndDeselectedState(defWearASB_e);
		TestPickedUpAndDeselectedState(defPartsSB_p);
		TestPickedUpAndDeselectedState(crfPartsSB_p);
		
		sgpAll.SetState(SlotGroup.DefocusedState);
		TestPickedUpAndDeselectedState(defPartsSB_p2);
		sgpAll.SetState(SlotGroup.DefocusedState);
		TestPickedUpAndDeselectedState(crfPartsSB_p2);
	}
	public void TestHover(Slottable pickedSB, Slottable hoveredSB){
		ValidatePickup(pickedSB);
		ValidatePostpickFilter(pickedSB);
		TestSimSBHover(pickedSB, hoveredSB);
		ValidateReset(pickedSB);
	}
	public void TestPickedUpAndDeselectedState(Slottable sb){
		ValidatePickup(sb);
		ValidatePostpickFilter(sb);
		ValidateReset(sb);
	}
	public void ValidateReset(Slottable sb){
		if(sb != null){
			if(sb.CurState == Slottable.PickedUpAndSelectedState || sb.CurState == Slottable.PickedUpAndDeselectedState){
				sb.OnPointerUpMock(eventData);
				if(sb.CurState == Slottable.WaitForNextTouchState)
					sb.WaitForNextTouchProcess.Expire();
				else if(sb.CurState == Slottable.WaitForNextTouchWhilePUState)
					sb.WaitForNextTouchWhilePUProcess.Expire();

				Assert.That(sgm.Transaction, Is.Null);
				Assert.That(sgm.CurState, Is.EqualTo(SlotGroupManager.FocusedState));
				Assert.That(sgm.SelectedSB, Is.Null);
				Assert.That(sgm.SelectedSG, Is.Null);
				Assert.That(sgm.PickedSB, Is.Null);
				AssertPrePickFiltered();
			}
		}
	}
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
	
	public void TestWaitForNextTouchState(Slottable sb){
		sb.OnPointerDownMock(eventData);
		
			Assert.That(sb.CurState, Is.EqualTo(Slottable.WaitForPickUpState));

		sb.OnPointerUpMock(eventData);

		if(sb.Item.IsStackable){
			Assert.That(sb.CurState, Is.EqualTo(Slottable.WaitForNextTouchState));
			Assert.That(sb.WaitForNextTouchProcess.IsRunning, Is.True);
			Assert.That(sb.WaitForNextTouchProcess.IsExpired, Is.False);
			/*	expire
			*/
			// sb.WaitForNextTouchProcess.Expire();
			// 	Assert.That(sb.WaitForNextTouchProcess.IsRunning, Is.False);
			// 	Assert.That(sb.WaitForNextTouchProcess.IsExpired, Is.True);
			// 	Assert.That(sb.Tapped, Is.True);
			// 	sb.Tapped = false;
			// 	Assert.That(sb.CurState, Is.EqualTo(Slottable.FocusedState));
			/*	pointerDown
			*/
			// sb.OnPointerDownMock(eventData);
			// sb.OnPointerUpMock(eventData);
			// sb.OnPointerDownMock(eventData);
			// 	Assert.That(sb.WaitForNextTouchProcess.IsRunning, Is.False);
			// 	Assert.That(sb.WaitForNextTouchProcess.IsExpired, Is.False);
			// 	Assert.That(sb.CurState, Is.EqualTo(Slottable.PickedUpAndSelectedState));
			// 	Assert.That(sb.PickedAmount, Is.EqualTo(1));
			/*	abort OnDeselected
			*/
			sb.OnDeselectedMock(eventData);
				Assert.That(sb.WaitForNextTouchProcess.IsRunning, Is.False);
				Assert.That(sb.WaitForNextTouchProcess.IsExpired, Is.False);
				Assert.That(sb.Tapped, Is.False);
				Assert.That(sb.CurState, Is.EqualTo(Slottable.FocusedState));
			
		}else{
			Assert.That(sb.Tapped, Is.True);
			sb.Tapped = false;
			Assert.That(sb.CurState, Is.EqualTo(Slottable.FocusedState));
		}
	}
	public void ValidatePickup(Slottable sb){
		if(sb != null){
			if(sb.CurState == Slottable.DeactivatedState){
				//undef
			}else if(sb.CurState == Slottable.DefocusedState){
				sb.OnPointerDownMock(eventData);
					Assert.That(sb.CurState, Is.EqualTo(Slottable.WaitForPointerUpState));
			}else if(sb.CurState == Slottable.FocusedState){
				sb.OnPointerDownMock(eventData);
					AssertState(sb, Slottable.WaitForPickUpState);
					Assert.That(sb.WaitForPickUpProcess.IsRunning, Is.True);
					Assert.That(sb.WaitForPickUpProcess.IsExpired, Is.False);
			
				sb.WaitForPickUpProcess.Expire();
			
					AssertAction(sb, "WaitAndPickUpProcess done");
					Assert.That(sb.WaitForPickUpProcess.IsRunning, Is.False);
					Assert.That(sb.WaitForPickUpProcess.IsExpired, Is.True);
				
					AssertState(sb, Slottable.PickedUpAndSelectedState);
					Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.True);
					Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.False);

				//validate SGM state and process
					Assert.That(sb.SGM.CurState, Is.EqualTo(SlotGroupManager.ProbingState));
					Assert.That(sb.SGM.ProbingStateProcess.IsRunning, Is.True);
					Assert.That(sb.SGM.ProbingStateProcess.IsExpired, Is.False);
				//SGM fields set properly
					Assert.That(sb.SGM.SelectedSB, Is.EqualTo(sb));
					Assert.That(sb.SGM.SelectedSG, Is.EqualTo(sb.SGM.GetSlotGroup(sb)));
					Assert.That(sb.SGM.PickedSB, Is.EqualTo(sb));
				//transaction
					Assert.That(sb.SGM.UpdateTransactionCommand.GetType(), Is.EqualTo(typeof(UpdateTransactionCommand)));
					Assert.That(sb.SGM.Transaction.GetType(), Is.EqualTo(typeof(RevertTransaction)));
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
	public void TestPickedUpAndSelectedState(Slottable sb){
		AssertPrePickFiltered();
		ValidatePickup(sb);
		ValidatePostpickFilter(sb);

		/*	expire
		*/
		sb.PickedUpAndSelectedProcess.Expire();
			AssertAction(sb, "PickedUpAndSelectedProcess done");
			Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.False);
			Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.True);
			AssertState(sb, Slottable.PickedUpAndSelectedState);
		
		/*	abort
		*/
		// sb.OnEndDragMock(eventDataMock);
		// 	Assert.That(sb.CurState, Is.EqualTo(Slottable.RevertingState));

		// 	Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.False);
		// 	Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.False);

		// 	Assert.That(sb.RevertingStateProcess.IsRunning, Is.True);
		// 	Assert.That(sb.RevertingStateProcess.IsExpired, Is.False);
		
		/*	OnPointerUp
		*/
			sb.OnPointerUpMock(eventData);
			Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.False);
			Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.True);
			if(sb.Item.IsStackable){

				Assert.That(sb.CurState, Is.EqualTo(Slottable.WaitForNextTouchWhilePUState));
				Assert.That(sb.WaitForNextTouchWhilePUProcess.IsRunning, Is.True);
				Assert.That(sb.WaitForNextTouchWhilePUProcess.IsExpired, Is.False);
				/*	try expiring 
				*/
				sb.WaitForNextTouchWhilePUProcess.Expire();
					
					Assert.That(sb.WaitForNextTouchWhilePUProcess.IsRunning, Is.False);
					Assert.That(sb.WaitForNextTouchWhilePUProcess.IsExpired, Is.True);
					Assert.That(sb.CurState, Is.EqualTo(Slottable.RevertingState));
					Assert.That(sb.RevertingStateProcess.IsRunning, Is.True);
					Assert.That(sb.RevertingStateProcess.IsExpired, Is.False);
			}else{

				Assert.That(sb.CurState, Is.EqualTo(Slottable.RevertingState));
				Assert.That(sb.RevertingStateProcess.IsRunning, Is.True);
				Assert.That(sb.RevertingStateProcess.IsExpired, Is.False);
			}


		/*	trying expring the revert state process(although it's a bit out of scope)
		*/

		// sb.RevertingStateProcess.Expire();
		// 	Assert.That(sb.RevertingStateProcess.IsRunning, Is.False);
		// 	Assert.That(sb.RevertingStateProcess.IsExpired, Is.True);
		// 	AssertPrePickFiltered();
		
		/*	OnDehovered
		*/
			// sgm.SimSBHover(null, eventDataMock);
			// 	Assert.That(sb.CurState, Is.EqualTo(Slottable.PickedUpAndDeselectedState));
			// 	Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.False);
			// 	Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.True);
			// 	Assert.That(sb.PickedUpAndDeselectedProcess.IsRunning, Is.True);
			// 	Assert.That(sb.PickedUpAndDeselectedProcess.IsExpired, Is.False);
		

	}
	public void AssertPrePickFiltered(){
		Assert.That(sgpAll.CurState, Is.EqualTo(SlotGroup.FocusedState));
		Assert.That(sgBow.CurState, Is.EqualTo(SlotGroup.FocusedState));
		Assert.That(sgWear.CurState, Is.EqualTo(SlotGroup.FocusedState));
		Assert.That(sgpParts.CurState, Is.EqualTo(SlotGroup.FocusedState));

		Assert.That(defBowASB_p.CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
		Assert.That(defBowBSB_p.CurState, Is.EqualTo(Slottable.FocusedState));
		Assert.That(crfBowASB_p.CurState, Is.EqualTo(Slottable.FocusedState));
		Assert.That(defWearASB_p.CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
		Assert.That(defWearBSB_p.CurState, Is.EqualTo(Slottable.FocusedState));
		Assert.That(crfWearASB_p.CurState, Is.EqualTo(Slottable.FocusedState));
		Assert.That(defPartsSB_p.CurState, Is.EqualTo(Slottable.DefocusedState));
		Assert.That(crfPartsSB_p.CurState, Is.EqualTo(Slottable.DefocusedState));

		Assert.That(defPartsSB_p2.CurState, Is.EqualTo(Slottable.FocusedState));
		Assert.That(crfPartsSB_p2.CurState, Is.EqualTo(Slottable.FocusedState));

		Assert.That(defBowASB_e.CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
		Assert.That(defWearASB_e.CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
	}
	public void TestWaitForPickUpState(Slottable sb){
		/*	entering
		*/
		sb.OnPointerDownMock(eventData);
		AssertState(sb, Slottable.WaitForPickUpState);
		Assert.That(sb.WaitForPickUpProcess.IsRunning, Is.True);
		Assert.That(sb.WaitForPickUpProcess.IsExpired, Is.False);
		/*	expire
		*/
		sb.WaitForPickUpProcess.Expire();
		AssertAction(sb,"WaitAndPickUpProcess done");
		AssertState(sb, Slottable.PickedUpAndSelectedState);
		Assert.That(sb.WaitForPickUpProcess.IsRunning, Is.False);
		Assert.That(sb.WaitForPickUpProcess.IsExpired, Is.True);
		/* abort, pointer up
		*/
		sb.SetState(Slottable.FocusedState);
		AssertState(sb, Slottable.FocusedState);
		sb.OnPointerDownMock(eventData);
		AssertState(sb, Slottable.WaitForPickUpState);
		
		sb.OnPointerUpMock(eventData);
		AssertState(sb, Slottable.WaitForNextTouchState);
		Assert.That(sb.WaitForPickUpProcess.IsRunning, Is.False);
		Assert.That(sb.WaitForPickUpProcess.IsExpired, Is.False);
		/*	OnEndDrag
		*/
		sb.SetState(Slottable.FocusedState);
		AssertState(sb, Slottable.FocusedState);
		sb.OnPointerDownMock(eventData);
		AssertState(sb, Slottable.WaitForPickUpState);

		sb.OnEndDragMock(eventData);
		AssertState(sb, Slottable.FocusedState);
		Assert.That(sb.WaitForPickUpProcess.IsRunning, Is.False);
		Assert.That(sb.WaitForPickUpProcess.IsExpired, Is.False);
		// /*	OnPointerExit
		// */
		// defBowASB_p.SetState(Slottable.FocusedState);
		// AssertState(defBowASB_p, Slottable.FocusedState);
		// defBowASB_p.OnPointerDownMock(eventDataMock);
		// AssertState(defBowASB_p, Slottable.WaitForPickUpState);

		// defBowASB_p.OnDehoveredMock(eventDataMock);
		// AssertState(defBowASB_p, Slottable.FocusedState);
		// Assert.That(defBowASB_p.WaitAndPickUpProcess.IsRunning, Is.False);
		// Assert.That(defBowASB_p.WaitAndPickUpProcess.IsExpired, Is.False);
		
	}
	public void TestWaitForPointerUpState(){
		defBowASB_p.SetState(Slottable.DefocusedState);
		
		defBowASB_p.OnPointerDownMock(eventData);
		AssertState(defBowASB_p, Slottable.WaitForPointerUpState);
		Assert.That(defBowASB_p.WaitAndSetBackToDefocusedStateProcess.IsRunning, Is.True);
		Assert.That(defBowASB_p.WaitAndSetBackToDefocusedStateProcess.IsExpired, Is.False);
		defBowASB_p.WaitAndSetBackToDefocusedStateProcess.Expire();
		AssertAction(defBowASB_p,"WaitAndSetBackToDefocusedStateProcess done");
		Assert.That(defBowASB_p.WaitAndSetBackToDefocusedStateProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.WaitAndSetBackToDefocusedStateProcess.IsExpired, Is.True);
		AssertState(defBowASB_p, Slottable.DefocusedState);
		
		/* tapping
		*/
		defBowASB_p.OnPointerDownMock(eventData);
		AssertState(defBowASB_p, Slottable.WaitForPointerUpState);
		defBowASB_p.OnPointerUpMock(eventData);
		AssertAction(defBowASB_p,"tapped");
		AssertState(defBowASB_p, Slottable.DefocusedState);
		Assert.That(defBowASB_p.WaitAndSetBackToDefocusedStateProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.WaitAndSetBackToDefocusedStateProcess.IsExpired, Is.False);
		/*OnEndDrag
		*/
		defBowASB_p.OnPointerDownMock(eventData);
		AssertState(defBowASB_p, Slottable.WaitForPointerUpState);
		defBowASB_p.OnEndDragMock(eventData);
		AssertState(defBowASB_p, Slottable.DefocusedState);
		Assert.That(defBowASB_p.WaitAndSetBackToDefocusedStateProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.WaitAndSetBackToDefocusedStateProcess.IsExpired, Is.False);
		
		
	}
	
	public void TestFocusedState(Slottable sb){
		
		// sb.SetState(Slottable.MovingState);
		// sb.SetState(Slottable.WaitForNextTouchState);
		// sb.SetState(Slottable.DeactivatedState);
		// sb.SetState(Slottable.FocusedState);
		
		/*	Graying in
		*/
		/*	enter
		*/
			AssertState(sb, Slottable.FocusedState);
			AssertAction(sb,"InstantGrayin called");
			sb.SetState(Slottable.DefocusedState);
			sb.SetState(Slottable.FocusedState);
			Assert.That(sb.GradualGrayinProcess.IsRunning, Is.True);
			Assert.That(sb.GradualGrayinProcess.IsExpired, Is.False);
			/*	expire
			*/
			sb.GradualGrayinProcess.Expire();
			AssertAction(sb,"GradualGrayinProcess done");
			Assert.That(sb.GradualGrayinProcess.IsRunning, Is.False);
			Assert.That(sb.GradualGrayinProcess.IsExpired, Is.True);

			sb.SetState(Slottable.DefocusedState);
			sb.SetState(Slottable.FocusedState);
			/*	abort
			*/
			sb.SetState(Slottable.SelectedState);
			Assert.That(sb.GradualGrayinProcess.IsRunning, Is.False);
			Assert.That(sb.GradualGrayinProcess.IsExpired, Is.False);
		
		/*	Dehighlighting
		*/
			/* enter
			*/
			sb.SetState(Slottable.FocusedState);
			Assert.That(sb.GradualDehighlightProcess.IsRunning, Is.True);
			Assert.That(sb.GradualDehighlightProcess.IsExpired, Is.False);
			/*	expire
			*/
			sb.GradualDehighlightProcess.Expire();
			AssertAction(sb,"GradualDehighlightProcess done");
			Assert.That(sb.GradualDehighlightProcess.IsRunning, Is.False);
			Assert.That(sb.GradualDehighlightProcess.IsExpired, Is.True);
			sb.SetState(Slottable.SelectedState);
			Assert.That(sb.GradualDehighlightProcess.IsRunning, Is.False);
			Assert.That(sb.GradualDehighlightProcess.IsExpired, Is.True);
			/*	abort
			*/
			sb.SetState(Slottable.FocusedState);
			sb.SetState(Slottable.SelectedState);
			Assert.That(sb.GradualDehighlightProcess.IsRunning, Is.False);
			Assert.That(sb.GradualDehighlightProcess.IsExpired, Is.False);
		
		/*Pointer enter
		*/
		sb.SetState(Slottable.FocusedState);
		sb.GradualDehighlightProcess.Expire();
		
		
		// anotherSb.SetState(Slottable.PickedUpAndSelectedState);
		// eventDataMock.pointerDrag = anotherGO;
		sb.OnHoverEnterMock(eventData);

		AssertState(sb, Slottable.SelectedState);
		/*	pointer down
		*/
		sb.SetState(Slottable.FocusedState);
		sb.GradualDehighlightProcess.Expire();
		AssertState(sb, Slottable.FocusedState);
		sb.OnPointerDownMock(eventData);
		AssertState(sb, Slottable.WaitForPickUpState);


	}
	public void TestDefocusedState(){
		defBowASB_p.SetState(Slottable.DefocusedState);
		AssertState(defBowASB_p, Slottable.DefocusedState);
		Assert.That(defBowASB_p.GradualGrayoutProcess.IsRunning, Is.True);
		Assert.That(defBowASB_p.GradualGrayoutProcess.IsExpired, Is.False);
		defBowASB_p.GradualGrayoutProcess.Expire();
		Assert.That(defBowASB_p.GradualGrayoutProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.GradualGrayoutProcess.IsExpired, Is.True);
		AssertAction(defBowASB_p,"GradualGrayoutProcess done");
		AssertState(defBowASB_p, Slottable.DefocusedState);
		defBowASB_p.SetState(Slottable.DefocusedState);
		Assert.That(defBowASB_p.GradualGrayoutProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.GradualGrayoutProcess.IsExpired, Is.True);
		defBowASB_p.SetState(Slottable.DeactivatedState);
		defBowASB_p.SetState(Slottable.DefocusedState);
		AssertState(defBowASB_p, Slottable.DefocusedState);
		Assert.That(defBowASB_p.UTLog, Is.EqualTo("InstantGrayout called"));
		Assert.That(defBowASB_p.GradualGrayoutProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.GradualGrayoutProcess.IsExpired, Is.True);
		defBowASB_p.SetState(Slottable.FocusedState);
		defBowASB_p.SetState(Slottable.DefocusedState);
		Assert.That(defBowASB_p.GradualGrayoutProcess.IsRunning, Is.True);
		Assert.That(defBowASB_p.GradualGrayoutProcess.IsExpired, Is.False);
		defBowASB_p.SetState(Slottable.DeactivatedState);
		Assert.That(defBowASB_p.GradualGrayoutProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.GradualGrayoutProcess.IsExpired, Is.False);
		AssertState(defBowASB_p, Slottable.DeactivatedState);
		



		// AssertAction("InstantGrayout called");
		
		// sb.OnPointerDownMock(eventDataMock);
		// AssertState(sb, Slottable.WaitForPointerUpState);
	}

	/* legacy
	*/
		// public void CheckEquippedStateIsAssigned(){
		// 	Assert.That(Slottable.EquippedState, Is.Not.Null);
		// 	Assert.That(Slottable.EquippedState.GetType(), Is.EqualTo(typeof(EquippedState)));
		// }
		// public void SlottableStateTransitionTest(){
		// 	sb.OnPointerDownMock();
		// 	AssertState<WaitForPickUpState>();
		// 	sb.ExpirePickupTimer();
		// 	AssertState<PickedUpState>();
		// 	sb.ReleasedInside = false;
		// 	sb.OnPointerUpMock();
		// 	AssertState<FocusedState>();
		// 	Assert.That(sb.UTLog, Is.EqualTo("Reverted"));
		// 	sb.UTLog = "";

		// 	sb.OnPointerDownMock();
		// 	AssertState<WaitForPickUpState>();
		// 	sb.OnPointerUpMock();
		// 	AssertState<WaitForNextTouchState>();

		// 	sb.OnDeselectedMock();
		// 	Assert.That(sb.UTLog, Is.EqualTo("Canceled"));
		// 	sb.UTLog = "";
		// 	AssertState<FocusedState>();

		// 	sb.OnPointerDownMock();
		// 	AssertState<WaitForPickUpState>();
		// 	sb.OnPointerUpMock();
		// 	AssertState<WaitForNextTouchState>();
		// 	sb.ExpireTapTimer();
		// 	AssertState<FocusedState>();
		// 	Assert.That(sb.UTLog, Is.EqualTo("tapped"));
		// 	sb.UTLog = "";

		// 	sb.OnPointerDownMock();
		// 	AssertState<WaitForPickUpState>();
		// 	sb.OnPointerUpMock();
		// 	AssertState<WaitForNextTouchState>();
		// 	sb.OnPointerDownMock();
		// 	AssertState<PickedUpState>();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(1));

		// 	sb.OnEndDragMock();
		// 	Assert.That(sb.UTLog, Is.EqualTo("Reverted"));
		// 	sb.UTLog = "";
		// 	AssertState<FocusedState>();

		// 	sb.OnPointerDownMock();
		// 	AssertState<WaitForPickUpState>();
		// 	sb.OnEndDragMock();
		// 	AssertCanceled();
		// 	AssertState<FocusedState>();

		// 	sb.OnPointerDownMock();
		// 	AssertState<WaitForPickUpState>();
		// 	sb.OnPointerUpMock();
		// 	AssertState<WaitForNextTouchState>();
		// 	sb.OnPointerDownMock();
		// 	AssertState<PickedUpState>();
		// 	AssertPickQuantity(1);
		// }


		// public void RevertTest(){

		// 	sb.OnPointerDownMock();
		// 	sb.ExpirePickupTimer();
		// 	sb.ReleasedInside = false;
		// 	sb.OnPointerUpMock();
		// 	AssertState<FocusedState>();
		// 	Assert.That(sb.UTLog, Is.EqualTo("Reverted"));
		// 	sb.UTLog = "";

		// 	sb.OnPointerDownMock();
		// 	sb.ExpirePickupTimer();
		// 	sb.ReleasedInside = true;
		// 	sb.ExpireRevertTimer();

		// 	AssertState<FocusedState>();
		// 	Assert.That(sb.UTLog, Is.EqualTo("Reverted"));
		// 	sb.UTLog = "";

		// 	sb.OnPointerDownMock();
		// 	sb.ExpirePickupTimer();
		// 	sb.ReleasedInside = true;//now W4NTWPU
		// 	sb.OnPointerUpMock();
		// 	sb.OnDeselectedMock();
		// 	AssertState<FocusedState>();
		// 	Assert.That(sb.UTLog, Is.EqualTo("Reverted"));
		// 	sb.UTLog = "";
		// }
		// public void SlottableInstantiation(){

		// 	Assert.That(sb, Is.Not.Null);

		// 	// sb.Initialize();
		// 	// Assert.That(sb.IsInitialized, Is.True);
		// }
		// public void SlottableInitialization(){
		
		// 	// sb.Initialize();
			
			
		// 	Assert.That(Slottable.DeactivatedState, Is.Not.Null);
		// 	Assert.That(Slottable.DefocusedState, Is.Not.Null);
		// 	Assert.That(Slottable.FocusedState, Is.Not.Null);
		// 	bool assertBool = sb.CurState.GetType() == typeof(DeactivatedState);
		// 	Assert.That(assertBool, Is.True);

		// 	sb.SetState(Slottable.DefocusedState);
		// 	Assert.That(sb.PrevState.GetType(), Is.EqualTo(typeof(DeactivatedState)));
			
		// 	sb.SetState(Slottable.FocusedState);
		// 	Assert.That(sb.PrevState.GetType(), Is.EqualTo(typeof(DefocusedState)));
			
		// 	sb.SetState(Slottable.DeactivatedState);
		// 	Assert.That(sb.PrevState.GetType(), Is.EqualTo(typeof(FocusedState)));
			
		// 	Assert.That(Slottable.WaitForPickUpState, Is.Not.Null);
		// 	Assert.That(Slottable.WaitForNextTouchState, Is.Not.Null);
		// 	Assert.That(Slottable.PickedUpState, Is.Not.Null);
		// 	Assert.That(Slottable.WaitForNextTouchWhilePUState, Is.Not.Null);
		// 	Assert.That(Slottable.MovingState, Is.Not.Null);
		// }

		
		// public void SlottableOnPointerDown(){
		// 	sb.Delayed = true;
		// 	// sb.SetState(Slottable.FocusedState);
		// 	sb.FilteredInMock();
		// 	sb.OnPointerDownMock();

		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(WaitForPickUpState)));
		// 	Assert.That(sb.IsPickupTimerOn, Is.True);

		// 	sb.OnPointerUpMock();
		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(WaitForNextTouchState)));
		// 	Assert.That(sb.IsPickupTimerOn, Is.False);
		// 	Assert.That(sb.IsTapTimerOn, Is.True);

		// 	// sb.ExpirePickupTimer();
		// 	// Assert.That(sb.CurrentState.GetType(), Is.EqualTo(typeof(PickedUpState)));
		// 	// Assert.That(sb.IsPickupTimerOn, Is.False);
			
		// 	sb.ExpireTapTimer();
		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(FocusedState)));
		// 	Assert.That(sb.IsPickupTimerOn, Is.False);
		// 	Assert.That(sb.IsTapTimerOn, Is.False);
		// 	Assert.That(sb.UTLog, Is.EqualTo("tapped"));
		// 	sb.ClearLog();
		// 	Assert.That(sb.UTLog, Is.EqualTo(""));

		// 	// sb.FingerMoveOverThreshMock();
		// 	// Assert.That(sb.CurrentState.GetType(), Is.EqualTo(typeof(DefocusedState)));
		// 	// Assert.That(sb.IsPickupTimerOn, Is.False);
			
		// 	// sb.Delayed = false;
		// 	// sb.SetState(Slottable.FocusedState);
		// 	// sb.OnPointerDownMock();
		// 	// Assert.That(sb.CurrentState.GetType(), Is.EqualTo(typeof(PickedUpState)));

		// 	// sb.ExpireTimer();
		// 	// Assert.That(sb.CurrentState.GetType(), Is.EqualTo(typeof(DefocusedState)));

		// 	// sb.OnDeselectedMock();
		// 	// Assert.That(sb.CurrentState.GetType(), Is.EqualTo(typeof(DefocusedState)));
		// 	// Assert.That(sb.IsPickupTimerOn, Is.False);
		// }

		
		// public void SlottableMultiTouchTest(){
		// 	sb.Delayed = true;
		// 	sb.FilteredInMock();
		// 	sb.OnPointerDownMock();
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(PickedUpState)));
		// 	Assert.That(sb.IsPickupTimerOn, Is.False);
		// 	Assert.That(sb.IsTapTimerOn, Is.False);
		// 	Assert.That(sb.UTLog, Is.EqualTo(""));
		// }
		
		// public void SbIncrementTest(){
			
		// 	sb.OnPointerDownMock();
		// 	sb.ExpirePickupTimer();
		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(PickedUpState)));
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(1));
		// 	sb.ReleasedInside = true;
		// 	sb.OnPointerUpMock();
		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(WaitForNextTouchWhilePUState)));
		// 	Assert.That(sb.IsRevertTimerOn, Is.True);

		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(PickedUpState)));
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(2));
		// 	Assert.That(sb.UTLog, Is.EqualTo("Incremented"));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(3));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(4));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(5));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(6));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(7));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(8));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(9));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(10));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(10));
			
		// 	// sb.OnPointerDownMock();
		// 	// Assert.That(sb.PickedAmount, Is.EqualTo(2));
		// }
		

		
		// public void TestSlottableEquality(){
		// 	InventoryItemMock itemA = new InventoryItemMock();
		
		// 	itemA.IsStackable = true;
		// 	itemA.ItemID = 1;

		// 	InventoryItemMock itemB = new InventoryItemMock();
		
		// 	itemB.IsStackable = true;
		// 	itemB.ItemID = 1;

		// 	InventoryItemMock itemC = new InventoryItemMock();
		
		// 	itemC.IsStackable = false;
		// 	itemC.ItemID = 2;

		// 	bool objectEquality = itemA.Equals((object)itemB);
		// 	bool objectInequality = !itemA.Equals((object)itemC);
		// 	bool iEquatableEquality = itemA.Equals(itemB);
		// 	bool iEquatableInequality = !itemB.Equals(itemC);
		// 	bool operatorEquality = (itemA == itemB) && (itemA == itemA) && (itemB == itemB);
		// 	bool operatorInequality = (itemA != itemC) && (itemB != itemC);
		// 	bool hashEquality = (itemA.GetHashCode() == itemB.GetHashCode()) && (itemA.GetHashCode() == itemA.GetHashCode());
		// 	bool hashIequality = itemA.GetHashCode() != itemC.GetHashCode();

		// 	Assert.That(objectEquality, Is.True);
		// 	Assert.That(objectInequality, Is.True);
		// 	Assert.That(iEquatableEquality, Is.True);
		// 	Assert.That(iEquatableInequality, Is.True);
		// 	Assert.That(operatorEquality, Is.True);
		// 	Assert.That(operatorInequality, Is.True);
		// 	Assert.That(hashEquality, Is.True);
		// 	Assert.That(hashIequality, Is.True);
			
		// }

	
	/*	Assertions
	*/
		public void AssertState<T>() where T: SlottableState{
			Assert.That(defBowASB_p.CurState.GetType(), Is.EqualTo(typeof(T)));
		}
		public void AssertState(Slottable sb, SlottableState sbState){
			Assert.That(sb.CurState, Is.EqualTo(sbState));
		}
		public void AssertAction(Slottable sb, string str){
			Assert.That(sb.UTLog, Is.EqualTo(str));
		}
		public void AssertCanceled(){
			Assert.That(defBowASB_p.UTLog, Is.EqualTo("Canceled"));
			defBowASB_p.UTLog = "";
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
