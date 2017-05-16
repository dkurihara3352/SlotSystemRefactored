using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections.Generic;
public class SlottableTest {


	
	/*	fields
	*/
		PointerEventDataMock eventData = new PointerEventDataMock();
		GameObject sgmGO;
		SlotGroupManager sgm;
		/*	sgs
		*/
			GameObject sgpAllGO;
			SlotGroup sgpAll;
			GameObject sgBowGO;
			SlotGroup sgBow;
			GameObject sgWearGO;
			SlotGroup sgWear;
			GameObject sgpPartsGO;
			SlotGroup sgpParts;
			GameObject sgCGearsGO;
			SlotGroup sgCGears;
		/*	items
		*/
			Slottable defBowB_p;
			Slottable defBowA_p;
			Slottable crfBowA_p;
			Slottable defWearB_p;
			Slottable defWearA_p;
			Slottable crfWearA_p;
			Slottable defParts_p;
			Slottable crfParts_p;
			Slottable defBowA_e;
			Slottable defWearA_e;
			Slottable defParts_p2;
			Slottable crfParts_p2;
			Slottable defShieldA_e;
			Slottable defShieldA_p;
			Slottable defMWeaponA_e;
			Slottable defMWeaponA_p;
			Slottable crfShieldA_e;
			Slottable crfShieldA_p;
			Slottable crfMWeaponA_e;
			Slottable crfMWeaponA_p;
			Slottable defQuiverA_p;
			Slottable defPackA_p;

	/*
	*/
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
				// sgpAll.UpdateEquipStatusCommand = new UpdateEquipStatusForPoolCommmand();
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
				// sgBow.UpdateEquipStatusCommand = new UpdateEquipStatusForEquipSGCommand();
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
				// sgWear.UpdateEquipStatusCommand = new UpdateEquipStatusForEquipSGCommand();
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
				// sgpParts.UpdateEquipStatusCommand = new UpdateEquipStatusForPoolCommmand();
				sgpParts.SetInventory(inventory);
				sgpParts.IsShrinkable = true;
				sgpParts.IsExpandable = true;
			/*	sgCGears
			*/
				sgCGearsGO = new GameObject("CarriedGearsSG");
				sgCGears = sgCGearsGO.AddComponent<SlotGroup>();
				sgCGears.Filter = new SGCarriedGearFilter();
				sgCGears.Sorter = new SGItemIndexSorter();
				// sgCGears.UpdateEquipStatusCommand = new UpdateEquipStatusForEquipSGCommand();
				sgCGears.SetInventory(equipInventory);
				sgCGears.IsShrinkable = true;
				sgCGears.IsExpandable = false;

				((EquipmentSetInventory)sgCGears.Inventory).SetEquippableCGearsCount(4);

				sgCGears.Slots = new List<Slot>();
				for(int i = 0; i < ((EquipmentSetInventory)sgCGears.Inventory).EquippableCGearsCount; i++){
					Slot slot = new Slot();
					sgCGears.Slots.Add(slot);
				}
		/*	Items
		*/
			/*	bows
			*/
				BowMock defBow = new BowMock();
				defBow.ItemID = 0;
				BowMock crfBow = new BowMock();
				crfBow.ItemID = 1;
				
				BowInstanceMock defBowA = new BowInstanceMock();//	equipped
				defBowA.Item = defBow;
				sgpAll.Inventory.AddItem(defBowA);
				sgBow.Inventory.AddItem(defBowA);
				BowInstanceMock defBowB = new BowInstanceMock();
				defBowB.Item = defBow;
				sgpAll.Inventory.AddItem(defBowB);
				BowInstanceMock crfBowA = new BowInstanceMock();
				crfBowA.Item = crfBow;
				sgpAll.Inventory.AddItem(crfBowA);
			/*	wears
			*/
					WearMock defWear = new WearMock();
					defWear.ItemID = 100;
					WearMock crfWear = new WearMock();
					crfWear.ItemID = 101;
					
					WearInstanceMock defWearA = new WearInstanceMock();//	equipped
					defWearA.Item = defWear;
					sgpAll.Inventory.AddItem(defWearA);
					sgWear.Inventory.AddItem(defWearA);
					WearInstanceMock defWearB = new WearInstanceMock();
					defWearB.Item = defWear;
					sgpAll.Inventory.AddItem(defWearB);
					WearInstanceMock crfWearA = new WearInstanceMock();
					crfWearA.Item = crfWear;
					sgpAll.Inventory.AddItem(crfWearA);
			/*	parts
			*/ 
				PartsMock defParts = new PartsMock();
				defParts.ItemID = 600;
				defParts.IsStackable = true;
				PartsMock crfParts = new PartsMock();
				crfParts.ItemID = 601;
				crfParts.IsStackable = true;
				
				PartsInstanceMock defPartsA = new PartsInstanceMock();
				defPartsA.Item = defParts;
				defPartsA.Quantity = 10;
				sgpAll.Inventory.AddItem(defPartsA);
				PartsInstanceMock defPartsB = new PartsInstanceMock();
				defPartsB.Item = defParts;
				defPartsB.Quantity = 5;
				sgpAll.Inventory.AddItem(defPartsB);
				PartsInstanceMock crfPartsA = new PartsInstanceMock();
				crfPartsA.Item = crfParts;
				crfPartsA.Quantity = 3;
				sgpAll.Inventory.AddItem(crfPartsA);

				Assert.That(defPartsA, Is.EqualTo(defPartsB));
				Assert.That(object.ReferenceEquals(defPartsA, defPartsB), Is.False);
		
			/*	carried gears
			*/
				ShieldMock defShield = new ShieldMock();
				defShield.ItemID = 200;
				AB(defShield.IsStackable, false);
				
				ShieldInstanceMock defShieldA = new ShieldInstanceMock();
				defShieldA.Item = defShield;
				AE(defShieldA.Quantity, 1);
				
				ShieldMock crfShield = new ShieldMock();
				crfShield.ItemID = 201;
				AB(crfShield.IsStackable, false);

				ShieldInstanceMock crfShieldA = new ShieldInstanceMock();
				crfShieldA.Item = crfShield;
				AE(crfShieldA.Quantity, 1);

				MeleeWeaponMock defMWeapon = new MeleeWeaponMock();
				defMWeapon.ItemID = 300;
				MeleeWeaponMock crfMWeapon = new MeleeWeaponMock();
				crfMWeapon.ItemID = 301;
				MeleeWeaponInstanceMock defMWeaponA = new MeleeWeaponInstanceMock();
				defMWeaponA.Item = defMWeapon;
				MeleeWeaponInstanceMock crfMWeaponA = new MeleeWeaponInstanceMock();
				crfMWeaponA.Item = crfMWeapon;

				QuiverMock defQuiver = new QuiverMock();
				defQuiver.ItemID = 400;
				QuiverInstanceMock defQuiverA = new QuiverInstanceMock();
				defQuiverA.Item = defQuiver;
				PackMock defPack = new PackMock();
				defPack.ItemID = 500;
				PackInstanceMock defPackA = new PackInstanceMock();
				defPackA.Item = defPack;

				inventory.AddItem(defShieldA);
				inventory.AddItem(crfShieldA);
				inventory.AddItem(defMWeaponA);
				inventory.AddItem(crfMWeaponA);
				inventory.AddItem(defQuiverA);
				inventory.AddItem(defPackA);
				equipInventory.AddItem(defShieldA);
				equipInventory.AddItem(defMWeaponA);

		/**/
		EquipmentSet equipSetA = new EquipmentSet(sgBow, sgWear, sgCGears);
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

			Assert.That(equipSetA.Elements.Count, Is.EqualTo(3));
			Assert.That(equipSetA.ContainsElement(sgBow), Is.True);
			AE(sgBow.SGM, sgm);
			Assert.That(equipSetA.ContainsElement(sgWear), Is.True);
			AE(sgWear.SGM, sgm);
			
		

		sgm.Initialize();
			/*	Assert Initialiation
				when the scene is loaded, but not yet focused
			*/
			defBowA_p = sgpAll.GetSlottable(defBowA);	
			defBowB_p = sgpAll.GetSlottable(defBowB);
			crfBowA_p = sgpAll.GetSlottable(crfBowA);
			defWearA_p = sgpAll.GetSlottable(defWearA);
			defWearB_p = sgpAll.GetSlottable(defWearB);
			crfWearA_p = sgpAll.GetSlottable(crfWearA);
			defParts_p = sgpAll.GetSlottable(defPartsA);
			crfParts_p = sgpAll.GetSlottable(crfPartsA);
			defBowA_e = sgBow.GetSlottable(defBowA);
			defWearA_e = sgWear.GetSlottable(defWearA);
			defParts_p2 = sgpParts.GetSlottable(defPartsA);
			crfParts_p2 = sgpParts.GetSlottable(crfPartsA);
			defShieldA_p = sgpAll.GetSlottable(defShieldA);
			crfShieldA_p = sgpAll.GetSlottable(crfShieldA);
			defMWeaponA_p =	sgpAll.GetSlottable(defMWeaponA); 
			crfMWeaponA_p =	sgpAll.GetSlottable(crfMWeaponA);
			defShieldA_e = sgCGears.GetSlottable(defShieldA);
			defMWeaponA_e = sgCGears.GetSlottable(defMWeaponA);
			defQuiverA_p = sgpAll.GetSlottable(defQuiverA);
			defPackA_p = sgpAll.GetSlottable(defPackA);

			Assert.That(sgm.CurState, Is.EqualTo(SlotGroupManager.DeactivatedState));
			/*	sgpAll
			*/
			Assert.That(sgpAll.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgpAll.FilteredItems.Count, Is.EqualTo(14));
			Assert.That(sgpAll.FilteredItems, Is.Ordered);
			Assert.That(sgpAll.Slots.Count, Is.EqualTo(14));
			foreach(Slot slot in sgpAll.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
				
				AE(sgm.GetSlotGroup(defBowA_p), sgpAll);
				AE(sgm.GetSlotGroup(defBowB_p), sgpAll);
				AE(sgm.GetSlotGroup(crfBowA_p), sgpAll);
				AE(sgm.GetSlotGroup(defWearA_p), sgpAll);
				AE(sgm.GetSlotGroup(defWearB_p), sgpAll);
				AE(sgm.GetSlotGroup(crfWearA_p), sgpAll);
				AE(sgm.GetSlotGroup(defParts_p), sgpAll);
				AE(sgm.GetSlotGroup(crfParts_p), sgpAll);
				AE(sgm.GetSlotGroup(defShieldA_p), sgpAll);
				AE(sgm.GetSlotGroup(crfShieldA_p), sgpAll);
				AE(sgm.GetSlotGroup(defMWeaponA_p), sgpAll);
				AE(sgm.GetSlotGroup(crfMWeaponA_p), sgpAll);
				AE(sgm.GetSlotGroup(defQuiverA_p), sgpAll);
				AE(sgm.GetSlotGroup(defPackA_p), sgpAll);
				
			/*	sgpParts
			*/
			Assert.That(sgpParts.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgpParts.FilteredItems.Count, Is.EqualTo(2));
			Assert.That(sgpParts.FilteredItems, Is.Ordered);
			Assert.That(sgpParts.Slots.Count, Is.EqualTo(2));
			foreach(Slot slot in sgpParts.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
				AE(sgm.GetSlotGroup(defParts_p2), sgpParts);
				AE(sgm.GetSlotGroup(crfParts_p2), sgpParts);
				
			/*	sgBow
			*/
			Assert.That(sgBow.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgBow.FilteredItems.Count, Is.EqualTo(1));
			Assert.That(sgBow.Slots.Count, Is.EqualTo(1));
			foreach(Slot slot in sgBow.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
			AE(sgm.GetSlotGroup(defBowA_e), sgBow);
			InventoryItemInstanceMock defBowA_e_Inst = (InventoryItemInstanceMock)defBowA_e.Item;
			AB(defBowA_e_Inst.IsEquipped, true);
			/*	sgWear
			*/
			Assert.That(sgWear.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgWear.FilteredItems.Count, Is.EqualTo(1));
			Assert.That(sgWear.Slots.Count, Is.EqualTo(1));
			foreach(Slot slot in sgWear.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
			AE(sgm.GetSlotGroup(defWearA_e), sgWear);
			InventoryItemInstanceMock defWearA_e_Inst = (InventoryItemInstanceMock)defWearA_e.Item;
			AB(defWearA_e_Inst.IsEquipped, true);

		sgm.Focus();
			AssertSGMFocused();
		sgm.Defocus();
			AssertSGMDefocus();
		sgm.Focus();
			AssertSGMFocused();
	}
	public void TestSGProcesses(){
		sgm.Defocus();
		sgm.Focus();

		AssertSGMFocused();
			ASSG(sgpAll, SlotGroup.FocusedState);
			AE(sgpAll.PrevState, SlotGroup.DefocusedState);
			AE(sgpAll.CurProcess.GetType(), typeof(SGGreyinProcess));
			ASSG(sgBow, SlotGroup.FocusedState);
			AE(sgBow.PrevState, SlotGroup.DefocusedState);
			AE(sgBow.CurProcess.GetType(), typeof(SGGreyinProcess));
			ASSG(sgWear, SlotGroup.FocusedState);
			AE(sgWear.PrevState, SlotGroup.DefocusedState);
			AE(sgWear.CurProcess.GetType(), typeof(SGGreyinProcess));
			ASSG(sgCGears, SlotGroup.FocusedState);
			AE(sgCGears.PrevState, SlotGroup.DefocusedState);
			AE(sgCGears.CurProcess.GetType(), typeof(SGGreyinProcess));
			ASSG(sgpParts, SlotGroup.DefocusedState);
			AE(sgpParts.PrevState, SlotGroup.DefocusedState);
			AE(sgpParts.CurProcess.GetType(), typeof(SGInstantGreyoutProcess));
		sgm.Focus();
			ASSG(sgpAll, SlotGroup.FocusedState);
			AE(sgpAll.PrevState, SlotGroup.FocusedState);
			AE(sgpAll.CurProcess.GetType(), typeof(SGGreyinProcess));
			ASSG(sgBow, SlotGroup.FocusedState);
			AE(sgBow.PrevState, SlotGroup.FocusedState);
			AE(sgBow.CurProcess.GetType(), typeof(SGGreyinProcess));
			ASSG(sgWear, SlotGroup.FocusedState);
			AE(sgWear.PrevState, SlotGroup.FocusedState);
			AE(sgWear.CurProcess.GetType(), typeof(SGGreyinProcess));
			ASSG(sgCGears, SlotGroup.FocusedState);
			AE(sgCGears.PrevState, SlotGroup.FocusedState);
			AE(sgCGears.CurProcess.GetType(), typeof(SGGreyinProcess));
			ASSG(sgpParts, SlotGroup.DefocusedState);
			AE(sgpParts.PrevState, SlotGroup.DefocusedState);
			AE(sgpParts.CurProcess.GetType(), typeof(SGInstantGreyoutProcess));
		sgm.Defocus();
			ASSG(sgpAll, SlotGroup.DefocusedState);
			AE(sgpAll.PrevState, SlotGroup.FocusedState);
			AE(sgpAll.CurProcess.GetType(), typeof(SGGreyoutProcess));
			ASSG(sgBow, SlotGroup.DefocusedState);
			AE(sgBow.PrevState, SlotGroup.FocusedState);
			AE(sgBow.CurProcess.GetType(), typeof(SGGreyoutProcess));
			ASSG(sgWear, SlotGroup.DefocusedState);
			AE(sgWear.PrevState, SlotGroup.FocusedState);
			AE(sgWear.CurProcess.GetType(), typeof(SGGreyoutProcess));
			ASSG(sgCGears, SlotGroup.DefocusedState);
			AE(sgCGears.PrevState, SlotGroup.FocusedState);
			AE(sgCGears.CurProcess.GetType(), typeof(SGGreyoutProcess));
			ASSG(sgpParts, SlotGroup.DefocusedState);
			AE(sgpParts.PrevState, SlotGroup.DefocusedState);
			AE(sgpParts.CurProcess.GetType(), typeof(SGInstantGreyoutProcess));
		sgm.Deactivate();
			ASSG(sgpAll, SlotGroup.DeactivatedState);
			AE(sgpAll.PrevState, SlotGroup.DefocusedState);
			// AE(sgpAll.CurProcess.GetType(), typeof(SGGreyoutProcess));
			AB(sgpAll.CurProcess == null, true);

			ASSG(sgBow, SlotGroup.DeactivatedState);
			AE(sgBow.PrevState, SlotGroup.DefocusedState);
			// AE(sgBow.CurProcess.GetType(), typeof(SGGreyoutProcess));
			AB(sgBow.CurProcess == null, true);

			ASSG(sgWear, SlotGroup.DeactivatedState);
			AE(sgWear.PrevState, SlotGroup.DefocusedState);
			// AE(sgWear.CurProcess.GetType(), typeof(SGGreyoutProcess));
			AB(sgWear.CurProcess == null, true);
			
			ASSG(sgCGears, SlotGroup.DeactivatedState);
			AE(sgCGears.PrevState, SlotGroup.DefocusedState);
			// AE(sgCGears.CurProcess.GetType(), typeof(SGGreyoutProcess));
			AB(sgCGears.CurProcess == null, true);
			
			ASSG(sgpParts, SlotGroup.DeactivatedState);
			AE(sgpParts.PrevState, SlotGroup.DefocusedState);
			// AE(sgpParts.CurProcess.GetType(), typeof(SGInstantGreyoutProcess));
			AB(sgpParts.CurProcess == null, true);
		sgm.Focus();
			ASSG(sgpAll, SlotGroup.FocusedState);
			AE(sgpAll.PrevState, SlotGroup.DeactivatedState);
			AE(sgpAll.CurProcess.GetType(), typeof(SGInstantGreyinProcess));
			ASSG(sgBow, SlotGroup.FocusedState);
			AE(sgBow.PrevState, SlotGroup.DeactivatedState);
			AE(sgBow.CurProcess.GetType(), typeof(SGInstantGreyinProcess));
			ASSG(sgWear, SlotGroup.FocusedState);
			AE(sgWear.PrevState, SlotGroup.DeactivatedState);
			AE(sgWear.CurProcess.GetType(), typeof(SGInstantGreyinProcess));
			ASSG(sgCGears, SlotGroup.FocusedState);
			AE(sgCGears.PrevState, SlotGroup.DeactivatedState);
			AE(sgCGears.CurProcess.GetType(), typeof(SGInstantGreyinProcess));
			ASSG(sgpParts, SlotGroup.DefocusedState);
			AE(sgpParts.PrevState, SlotGroup.DeactivatedState);
			AE(sgpParts.CurProcess.GetType(), typeof(SGInstantGreyoutProcess));
		
		PickUp(defBowB_p, out picked);
			ASSG(sgpAll, SlotGroup.SelectedState);
			AE(sgpAll.PrevState, SlotGroup.FocusedState);
			AE(sgpAll.CurProcess.GetType(), typeof(SGHighlightProcess));
		sgm.SimSGHover(sgBow, eventData);
			ASSG(sgpAll, SlotGroup.FocusedState);
			AE(sgpAll.PrevState, SlotGroup.SelectedState);
			AE(sgpAll.CurProcess.GetType(), typeof(SGDehighlightProcess));
			ASSG(sgBow, SlotGroup.SelectedState);
			AE(sgBow.PrevState, SlotGroup.FocusedState);
			AE(sgBow.CurProcess.GetType(), typeof(SGHighlightProcess));
		sgm.SimSGHover(sgpAll, eventData);
		bool reverted = false;
		Revert(defBowB_p, out reverted);
		AssertSGMFocused();
		AB(picked, true);
		AB(reverted, true);

	}
	public void TestSBProcesses(){
		/*	FocusedState
		*/
			sgm.Defocus();
			sgm.Focus();
			ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
			AE(defBowA_p.PrevState, Slottable.EquippedAndDefocusedState);
			AB(defBowA_p.CurProcess == null, true);
			ASSB(defBowB_p, Slottable.FocusedState);
			AE(defBowB_p.PrevState, Slottable.DefocusedState);
			AE(defBowB_p.CurProcess.GetType(), typeof(SBGreyinProcess));
			ASSB(defParts_p, Slottable.DefocusedState);
			AE(defParts_p.PrevState, Slottable.DefocusedState);
			AB(defParts_p.CurProcess == null, true);
			ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
			AE(defBowA_e.PrevState, Slottable.EquippedAndDefocusedState);
			AE(defBowA_e.CurProcess.GetType(), typeof(SBGreyinProcess));

			sgpAll.AutoSort = false;
			PickUp(defBowB_p, out picked);
			sgm.SimSBHover(defWearB_p, eventData);
			ASSB(defWearB_p, Slottable.SelectedState);
			sgm.SimSBHover(defBowB_p, eventData);
			ASSB(defWearB_p, Slottable.FocusedState);
			AE(defWearB_p.PrevState, Slottable.SelectedState);
			AE(defWearB_p.CurProcess.GetType(), typeof(SBDehighlightProcess));
			bool reverted;
			Revert(defBowB_p, out reverted);

			sgpAll.AutoSort = true;
			sgm.Focus();
			AssertSGMFocused();

			ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
			defBowA_p.SetState(Slottable.FocusedState);
			AE(defBowA_p.PrevState, Slottable.EquippedAndDefocusedState);
			ASSB(defBowA_p, Slottable.FocusedState);
			AE(defBowA_p.CurProcess.GetType(), typeof(SBUnequipAndGreyinProcess));
			defBowA_p.SetState(Slottable.EquippedAndDefocusedState);
			AssertSGMFocused();
			
			defBowB_p.SetState(Slottable.EquippedAndSelectedState);
			defBowB_p.SetState(Slottable.FocusedState);
			AE(defBowB_p.PrevState, Slottable.EquippedAndSelectedState);
			ASSB(defBowB_p, Slottable.FocusedState);
			AE(defBowB_p.CurProcess.GetType(), typeof(SBUnequipAndDehighlightProcess));
			AssertSGMFocused();

			sgm.Deactivate();
			sgm.Focus();
			AE(defBowB_p.PrevState, Slottable.DeactivatedState);
			ASSB(defBowB_p, Slottable.FocusedState);
			AE(defBowB_p.CurProcess, null);
			AssertSGMFocused();

			PickUp(defBowB_p, out picked);
			defBowB_p.OnPointerUpMock(eventData);
			ASSB(defBowB_p, Slottable.MovingState);
			AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
			AE(defBowB_p.CurProcess.GetType(), typeof(MoveProcess));
			defBowB_p.CurProcess.Expire();
			AssertSGMFocused();
			AE(defBowB_p.PrevState, Slottable.MovingState);
			AE(defBowB_p.CurProcess.GetType(), typeof(SBUnpickProcess));
		/*	EquippedAndDeselectedState
		*/
			AssertSGMFocused();
			sgm.Defocus();
			sgm.Focus();
			ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
			AE(defBowA_e.PrevState, Slottable.EquippedAndDefocusedState);
			AE(defBowA_e.CurProcess.GetType(), typeof(SBGreyinProcess));
			sgm.Deactivate();
			sgm.Focus();
			ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
			AE(defBowA_e.PrevState, Slottable.DeactivatedState);
			AB(defBowA_e.CurProcess == null, true);

			defBowA_e.SetState(Slottable.DefocusedState);
			defBowA_e.SetState(Slottable.EquippedAndDeselectedState);
			ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
			AE(defBowA_e.PrevState, Slottable.DefocusedState);
			AE(defBowA_e.CurProcess.GetType(), typeof(SBEquipAndGreyinProcess));
			
			defBowA_e.SetState(Slottable.SelectedState);
			defBowA_e.SetState(Slottable.EquippedAndDeselectedState);
			ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
			AE(defBowA_e.PrevState, Slottable.SelectedState);
			AE(defBowA_e.CurProcess.GetType(), typeof(SBEquipAndDehighlightProcess));
		/*	EquippedAndSelectedState
		*/
			AssertSGMFocused();
			sgpAll.AutoSort = false;
			PickUp(defBowB_p, out picked);
			sgm.SimSBHover(defBowA_p, eventData);
			ASSB(defBowA_p, Slottable.EquippedAndSelectedState);
			AE(defBowA_p.PrevState, Slottable.EquippedAndDeselectedState);
			AE(defBowA_p.CurProcess.GetType(), typeof(SBHighlightProcess));
			sgm.SimSBHover(defBowB_p, eventData);
			Revert(defBowB_p, out reverted);
			sgpAll.AutoSort = true;
			AssertSGMFocused();


		/*	EquippedAndDefocusedState
		*/
			AssertSGMFocused();
			sgm.Defocus();
			ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
			AE(defShieldA_e.PrevState, Slottable.EquippedAndDeselectedState);
			AE(defShieldA_e.CurProcess.GetType(), typeof(SBGreyoutProcess));
			defShieldA_e.SetState(Slottable.FocusedState);
			ASSB(defShieldA_e, Slottable.FocusedState);
			defShieldA_e.SetState(Slottable.EquippedAndDefocusedState);
			AE(defShieldA_e.PrevState, Slottable.FocusedState);
			ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
			AE(defShieldA_e.CurProcess.GetType(), typeof(SBEquipAndGreyoutProcess));
			
			sgm.Focus();
	}
	[Test]
	public void Test(){
		
		// TestHover();
		/*
		*/
		/*	fill equipping defQuiverA_p
		*/
			AE(sgCGears.Slots.Count, 4);
			AE(sgm.GetEquippedCarriedGears().Count, 2);
			
			
			AB(sgm.GetEquippedCarriedGears().Contains((CarriedGearInstanceMock)defShieldA_e.Item), true);
			AB(sgm.GetEquippedCarriedGears().Contains((CarriedGearInstanceMock)defMWeaponA_e.Item), true);
			
			AssertSGMFocused();

			PickUp(defQuiverA_p, out picked);
				AssertPostPickFilter(defQuiverA_p);
				ASSB(defQuiverA_p, Slottable.PickedAndSelectedState);
				AE(sgm.PickedSB, defQuiverA_p);
				AE(sgm.SelectedSB, defQuiverA_p);
				AE(sgm.SelectedSG, sgpAll);
				AE(sgm.Transaction.GetType(), typeof(RevertTransaction));

			sgm.SimSBHover(null, eventData);
			sgm.SimSGHover(sgCGears, eventData);

				ASSB(defQuiverA_p, Slottable.PickedAndDeselectedState);
				AE(sgm.PickedSB, defQuiverA_p);
				AE(sgm.SelectedSB, null);
				AE(sgm.SelectedSG, sgCGears);
				AE(sgm.Transaction.GetType(), typeof(FillEquipTransaction));

				AE(sgm.PickedSBDoneTransaction, true);
				AE(sgm.SelectedSBDoneTransaction, true);
				AE(sgm.OrigSGDoneTransaction, true);
				AE(sgm.SelectedSGDoneTransaction, true);

				AE(sgm.CurState, SlotGroupManager.ProbingState);
				AE(sgm.CurProcess.GetType(), typeof(SGMProbingStateProcess));

				ASSG(sgpAll, SlotGroup.FocusedState);
				ASSG(sgCGears, SlotGroup.SelectedState);
			
			/*	pre transaction
			*/
				AB(defQuiverA_p.IsEquipped, false);
				AB(sgCGears.Inventory.Items.Contains(defQuiverA_p.Item), false);
				AE(sgCGears.GetSlottable((InventoryItemInstanceMock)defQuiverA_p.Item), null);

			defQuiverA_p.OnPointerUpMock(eventData);
			/*	post transaction
			*/
				AE(sgm.PickedSBDoneTransaction, false);
				AE(sgm.SelectedSBDoneTransaction, true);
				AE(sgm.OrigSGDoneTransaction, false);
				AE(sgm.SelectedSGDoneTransaction, false);

				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
				AE(sgm.CurProcess.GetType(), typeof(SGMFillEquipTransactionProcess));
				AE(sgm.GetSlotGroup(defQuiverA_p), sgpAll);
				
				AB(sgpAll.Inventory.Items.Contains(defQuiverA_p.Item), true);
				AE(sgpAll.Slots.Count, 14);
				AE(sgpAll.GetSlottable((InventoryItemInstanceMock)defQuiverA_p.Item), defQuiverA_p);
				AB(sgCGears.Inventory.Items.Contains(defQuiverA_p.Item), true);
				AE(sgCGears.Slots.Count, 4);
				AB(sgCGears.GetSlottable((InventoryItemInstanceMock)defQuiverA_p.Item) != null, false);

				
				ASSB(defQuiverA_p, Slottable.EquippingState);
				AE(defQuiverA_p.PrevState, Slottable.PickedAndDeselectedState);
				AB(defQuiverA_p.IsEquipped, false);
				AE(defQuiverA_p.CurProcess.GetType(), typeof(SBEquippingProcess));
				AE(defQuiverA_p.DestinationSG, sgCGears);
				AE(defQuiverA_p.DestinationSlot, /*sgCGears.GetSlot(defQuiverA_e)*/sgCGears.GetNextEmptySlot());
			

			/*	pre completion SBA
			*/
			defQuiverA_p.CurProcess.Expire();
			/*	post completion SBA
			*/
				AB(sgm.PickedSBDoneTransaction, true);
				AB(sgm.SelectedSBDoneTransaction, true);
				AB(sgm.OrigSGDoneTransaction, false);
				AB(sgm.SelectedSGDoneTransaction, false);
				AE(sgm.CurProcess.GetType(), typeof(SGMFillEquipTransactionProcess));
				AB(sgm.CurProcess.IsRunning, true);
				AB(sgm.CurProcess.IsExpired, false);
				
				ASSG(sgpAll, SlotGroup.PerformingTransactionState);
				AE(sgpAll.CurProcess.GetType(), typeof(SGUpdateTransactionProcess));
				AB(sgpAll.CurProcess.IsRunning, true);
				AB(sgpAll.CurProcess.IsExpired, false);
				ASSG(sgCGears, SlotGroup.PerformingTransactionState);
				AE(sgCGears.CurProcess.GetType(), typeof(SGUpdateTransactionProcess));
				AB(sgCGears.CurProcess.IsRunning, true);
				AB(sgCGears.CurProcess.IsExpired, false);

				ASSB(defQuiverA_p, Slottable.EquippingState);
				AE(defQuiverA_p.PrevState, Slottable.PickedAndDeselectedState);//
				AE(defQuiverA_p.CurProcess.GetType(), typeof(SBEquippingProcess));
				AB(defQuiverA_p.CurProcess.IsRunning, false);
				AB(defQuiverA_p.CurProcess.IsExpired, true);

			sgpAll.CurProcess.Expire();
				AB(sgm.PickedSBDoneTransaction, true);
				AB(sgm.SelectedSBDoneTransaction, true);
				AB(sgm.OrigSGDoneTransaction, true);
				AB(sgm.SelectedSGDoneTransaction, false);
				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
				AE(sgm.CurProcess.GetType(), typeof(SGMFillEquipTransactionProcess));

				ASSG(sgpAll, SlotGroup.PerformingTransactionState);
				AE(sgpAll.CurProcess.GetType(), typeof(SGUpdateTransactionProcess));
				AE(sgpAll.CurProcess.IsRunning, false);
				AE(sgpAll.CurProcess.IsExpired, true);
				
				ASSG(sgCGears, SlotGroup.PerformingTransactionState);
				AE(sgCGears.CurProcess.GetType(), typeof(SGUpdateTransactionProcess));
				AE(sgCGears.CurProcess.IsRunning, true);
				AE(sgCGears.CurProcess.IsExpired, false);

			sgCGears.CurProcess.Expire();
			Slottable defQuiverA_e = sgCGears.GetSlottable((InventoryItemInstanceMock)defQuiverA_p.Item);
				AB(sgm.PickedSBDoneTransaction, true);
				AB(sgm.SelectedSBDoneTransaction, true);
				AB(sgm.OrigSGDoneTransaction, true);
				AB(sgm.SelectedSGDoneTransaction, true);
				AE(sgm.CurState, SlotGroupManager.FocusedState);
				AE(sgm.CurProcess, null);

				ASSG(sgpAll, SlotGroup.FocusedState);
				AE(sgpAll.PrevState, SlotGroup.PerformingTransactionState);
				AE(sgpAll.CurProcess, null);
				
				ASSG(sgCGears, SlotGroup.FocusedState);
				AE(sgCGears.PrevState, SlotGroup.PerformingTransactionState);
				AE(sgCGears.CurProcess, null);
				
				AB(defQuiverA_p.IsEquipped, true);
				ASSB(defQuiverA_p, Slottable.EquippedAndDefocusedState);
				AE(defQuiverA_p.PrevState, Slottable.EquippingState);
				AE(defQuiverA_p.CurProcess, null);
				AB(defQuiverA_e.IsEquipped, true);
				ASSB(defQuiverA_e, Slottable.EquippedAndDeselectedState);

				AE(sgm.GetEquippedCarriedGears().Count , 3);
		/*	fill equipping defPackA_p
		*/
				AssertSGMFocused();
			PickUp(defPackA_p, out picked);
			sgm.SimSBHover(null, eventData);
			sgm.SimSGHover(sgCGears, eventData);
				AE(sgm.PickedSB, defPackA_p);
				AE(sgm.SelectedSB, null);
				AE(sgm.SelectedSG, sgCGears);

				AE(sgm.CurState, SlotGroupManager.ProbingState);
				AE(sgm.CurProcess.GetType(), typeof(SGMProbingStateProcess));
				AE(sgm.Transaction.GetType(), typeof(FillEquipTransaction));

				AE(sgpAll.PrevState, SlotGroup.SelectedState);
				ASSG(sgpAll, SlotGroup.FocusedState);
				AE(sgpAll.CurProcess.GetType(), typeof(SGDehighlightProcess));

				AE(sgCGears.PrevState, SlotGroup.FocusedState);
				ASSG(sgCGears, SlotGroup.SelectedState);
				AE(sgCGears.CurProcess.GetType(), typeof(SGHighlightProcess));

				AE(defPackA_p.PrevState, Slottable.PickedAndSelectedState);
				ASSB(defPackA_p, Slottable.PickedAndDeselectedState);
				AE(defPackA_p.CurProcess.GetType(), typeof(SBDehighlightProcess));
				AE(defPackA_p.DestinationSG, null);
				AE(defPackA_p.DestinationSlot, null);

				AB(sgCGears.GetNextEmptySlot() != null, true);
			
			defPackA_p.OnPointerUpMock(eventData);
			sgpAll.CurProcess.Expire();
			defPackA_p.CurProcess.Expire();
			sgCGears.CurProcess.Expire();

				AE(sgm.PickedSB, null);
				AE(sgm.SelectedSB, null);
				AE(sgm.SelectedSG, null);

				AE(sgm.CurState, SlotGroupManager.FocusedState);
				AE(sgm.CurProcess, null);
				AE(sgm.Transaction, null);

				AE(sgpAll.PrevState, SlotGroup.PerformingTransactionState);
				ASSG(sgpAll, SlotGroup.FocusedState);
				AE(sgpAll.CurProcess, null);

				AE(sgCGears.PrevState, SlotGroup.PerformingTransactionState);
				ASSG(sgCGears, SlotGroup.FocusedState);
				AE(sgCGears.CurProcess, null);

				AE(defPackA_p.PrevState, Slottable.EquippingState);
				ASSB(defPackA_p, Slottable.EquippedAndDefocusedState);
				AE(defPackA_p.CurProcess, null);
				AB(defPackA_p.IsEquipped, true);
				AE(defPackA_p.DestinationSG, null);
				AE(defPackA_p.DestinationSlot, null);

				AB(sgCGears.GetNextEmptySlot() != null, false);
				AE(sgm.GetEquippedCarriedGears().Count, 4);
				AB(sgCGears.GetSlottable((InventoryItemInstanceMock)defPackA_p.Item) == null, false);

				Slottable defPackA_e = sgCGears.GetSlottable((InventoryItemInstanceMock)defPackA_p.Item);
				ASSB(defPackA_e, Slottable.EquippedAndDeselectedState);

				AssertSGMFocused();
		/*	make sure no more Fill transaction is allowed
		*/
			AB(crfShieldA_e == null, true);
			AB(sgm.GetSlotGroup(crfShieldA_p), sgpAll);
				AE(sgCGears.GetNextEmptySlot(), null);
				AB(sgCGears.AcceptsFilter(crfShieldA_p), true);
				AB(sgCGears.Filter is SGCarriedGearFilter, true);
			PickUp(crfShieldA_p, out picked);
				ASSG(sgCGears, SlotGroup.DefocusedState);
			sgm.SimSBHover(null, eventData);
			sgm.SimSGHover(sgCGears, eventData);

				AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
				AE(sgm.PickedSB, crfShieldA_p);
				AE(sgm.SelectedSB, null);
				AE(sgm.SelectedSG, null);
				AE(sgCGears.GetNextEmptySlot(), null);
			crfShieldA_p.OnPointerUpMock(eventData);
				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
				AE(sgm.CurProcess.GetType(), typeof(SGMRevertTransactionProcess));
				AE(crfShieldA_p.CurProcess.GetType(), typeof(SBUnpickingProcess));
			crfShieldA_p.CurProcess.Expire();
		/*	remove defShieldA_e
		*/
			AssertSGMFocused();
			PickUp(defShieldA_e, out picked);
				AE(sgm.CurState, SlotGroupManager.ProbingState);
				AE(sgm.CurProcess.GetType(), typeof(SGMProbingStateProcess));
				AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
				AE(sgm.PickedSB, defShieldA_e);
				AE(sgm.SelectedSB, defShieldA_e);
				AE(sgm.SelectedSG, sgCGears);

				ASSG(sgCGears, SlotGroup.SelectedState);
				AE(sgCGears.PrevState, SlotGroup.FocusedState);
				AE(sgCGears.CurProcess.GetType(),typeof(SGHighlightProcess));

				ASSB(defShieldA_e, Slottable.PickedAndSelectedState);
				AE(defShieldA_e.PrevState, Slottable.WaitForPickUpState);
				AE(defShieldA_e.CurProcess.GetType(), typeof(SBPickUpProcess));

				ASSG(sgpAll, SlotGroup.FocusedState);
			sgm.SimSBHover(null, eventData);
			sgm.SimSGHover(sgpAll, eventData);
				AE(sgm.CurState, SlotGroupManager.ProbingState);
				AE(sgm.CurProcess.GetType(), typeof(SGMProbingStateProcess));
				AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
				AE(sgm.PickedSB, defShieldA_e);
				AE(sgm.SelectedSB, null);
				AE(sgm.SelectedSG, sgpAll);

				ASSG(sgCGears, SlotGroup.FocusedState);
				AE(sgCGears.PrevState, SlotGroup.SelectedState);
				AE(sgCGears.CurProcess.GetType(),typeof(SGDehighlightProcess));
				AB(sgCGears.Inventory.Items.Contains(defShieldA_p.Item), true);
				AB(sgCGears.GetSlottable(defShieldA_e.Item), defShieldA_e);

				ASSB(defShieldA_e, Slottable.PickedAndDeselectedState);
				AE(defShieldA_e.PrevState, Slottable.PickedAndSelectedState);
				AE(defShieldA_e.CurProcess.GetType(), typeof(SBDehighlightProcess));

				ASSG(sgpAll, SlotGroup.SelectedState);
				AE(sgpAll.PrevState, SlotGroup.FocusedState);
				AE(sgpAll.CurProcess.GetType(), typeof(SGHighlightProcess));
			defShieldA_e.OnPointerUpMock(eventData);
				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
				AE(sgm.CurProcess.GetType(), typeof(SGMUnequipTransactionProcess));
				AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
				AE(sgm.PickedSB, defShieldA_e);
				AE(sgm.SelectedSB, null);
				AE(sgm.SelectedSG, sgpAll);
				AE(sgm.PickedSBDoneTransaction, false);
				AE(sgm.SelectedSBDoneTransaction, true);
				AE(sgm.OrigSGDoneTransaction, false);
				AE(sgm.SelectedSGDoneTransaction, false);

				EquipmentSet focusedEquipSet = (EquipmentSet)sgm.RootPage.EquipBundle.GetFocusedBundleElement();
				AB(focusedEquipSet.ContainsElement(sgCGears), true);

				ASSG(sgCGears, SlotGroup.PerformingTransactionState);
				AE(sgCGears.PrevState, SlotGroup.FocusedState);
				AE(sgCGears.CurProcess.GetType(),typeof(SGUpdateTransactionProcess));
				AE(sgCGears.Inventory.Items.Contains(defShieldA_p.Item), false);

				ASSB(defShieldA_e, Slottable.RemovingState);
				AE(defShieldA_e.PrevState, Slottable.PickedAndDeselectedState);
				AE(defShieldA_e.CurProcess.GetType(), typeof(SBRemovingProcess));
				AE(defShieldA_e.DestinationSG, sgpAll);
				AE(defShieldA_e.DestinationSlot, sgpAll.GetSlot(defShieldA_p));//

				AE(sgm.GetSlotGroup(defShieldA_e), sgCGears);
				AE(defShieldA_e.IsEquipped, true);
				AE(defShieldA_p.IsEquipped, true);

				ASSG(sgpAll, SlotGroup.PerformingTransactionState);
				AE(sgpAll.PrevState, SlotGroup.SelectedState);
				AE(sgpAll.CurProcess.GetType(), typeof(SGUpdateTransactionProcess));
			
				AE(sgm.PickedSBDoneTransaction, false);
				AE(sgm.SelectedSBDoneTransaction, true);
				AE(sgm.OrigSGDoneTransaction, false);
				AE(sgm.SelectedSGDoneTransaction, false);
			defShieldA_e.CurProcess.Expire();
				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
				AE(sgm.CurProcess.GetType(), typeof(SGMUnequipTransactionProcess));
				AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
				AE(sgm.PickedSB, defShieldA_e);
				AE(sgm.SelectedSB, null);
				AE(sgm.SelectedSG, sgpAll);
				AE(sgm.PickedSBDoneTransaction, true);
				AE(sgm.SelectedSBDoneTransaction, true);
				AE(sgm.OrigSGDoneTransaction, false);
				AE(sgm.SelectedSGDoneTransaction, false);
			sgpAll.CurProcess.Expire();
				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
				AE(sgm.CurProcess.GetType(), typeof(SGMUnequipTransactionProcess));
				AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
				AE(sgm.PickedSB, defShieldA_e);
				AE(sgm.SelectedSB, null);
				AE(sgm.SelectedSG, sgpAll);
				AE(sgm.PickedSBDoneTransaction, true);
				AE(sgm.SelectedSBDoneTransaction, true);
				AE(sgm.OrigSGDoneTransaction, false);
				AE(sgm.SelectedSGDoneTransaction, true);
			sgCGears.CurProcess.Expire();
				AE(sgm.PickedSBDoneTransaction, true);
				AE(sgm.SelectedSBDoneTransaction, true);
				AE(sgm.OrigSGDoneTransaction, true);
				AE(sgm.SelectedSGDoneTransaction, true);
				AE(sgm.CurProcess, null);
				AE(sgm.Transaction, null);
				AE(sgm.PickedSB, null);
				AE(sgm.SelectedSB, null);
				AE(sgm.SelectedSG, null);
				AE(sgm.CurState, SlotGroupManager.FocusedState);

				ASSG(sgCGears, SlotGroup.FocusedState);
				AE(sgCGears.PrevState, SlotGroup.PerformingTransactionState);
				AE(sgCGears.CurProcess, null);
				AB(sgCGears.Inventory.Items.Contains(defShieldA_p.Item), false);
				AE(sgCGears.Slots.Count, 4);
				AE(sgm.GetEquippedCarriedGears().Count, 3);
				AE(sgCGears.GetSlottable((InventoryItemInstanceMock)defShieldA_p.Item), null);
				AB(sgCGears.GetNextEmptySlot() != null, true);

				ASSG(sgpAll, SlotGroup.FocusedState);
				AE(sgpAll.PrevState, SlotGroup.PerformingTransactionState);
				AE(sgpAll.CurProcess, null);
				AB(sgpAll.Inventory.Items.Contains(defShieldA_p.Item), true);
		
				AB(defShieldA_e == null, true);
				AE(defShieldA_p.IsEquipped, false);
				ASSB(defShieldA_p, Slottable.FocusedState);
		/*	remove defMWeaponA_e
		*/
			// AssertSGMFocused();
			AB(sgCGears.Inventory.Items.Contains(defMWeaponA_p.Item), true);
			AE(sgm.GetEquippedCarriedGears().Count, 3);
			PickUp(defMWeaponA_e, out picked);
			sgm.SimSBHover(null, eventData);
			sgm.SimSGHover(sgpAll, eventData);
			defMWeaponA_e.OnPointerUpMock(eventData);
			defMWeaponA_e.CurProcess.Expire();
			sgCGears.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			AE(sgm.GetEquippedCarriedGears().Count, 2);
			AE(sgm.GetCGEmptySlots().Count, 2);
			AB(defMWeaponA_e != null, false);
			AB(defMWeaponA_p.IsEquipped, false);
			AB(sgpAll.Inventory.Items.Contains(defMWeaponA_p.Item), true);
			AB(sgCGears.Inventory.Items.Contains(defMWeaponA_p.Item), false);
			AM(defMWeaponA_p, sgCGears, false);
			AM(defShieldA_p, sgCGears, false);
			AM(defQuiverA_p, sgCGears, true);
			AM(defPackA_p, sgCGears, true);
			AM(defBowA_p, sgBow, true);
			AM(defWearA_p, sgWear, true);
		/*	remove defQuiverA_e
		*/
			PickUp(defQuiverA_e, out picked);
			sgm.SimSBHover(null, eventData);
			sgm.SimSGHover(sgpAll, eventData);
			defQuiverA_e.OnPointerUpMock(eventData);
			defQuiverA_e.CurProcess.Expire();
			sgCGears.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
				AB(defQuiverA_p.IsEquipped, false);
				AE(sgm.GetCGEmptySlots().Count, 3);
				AE(sgm.GetEquippedCarriedGears().Count, 1);
				AB(defQuiverA_e == null, true);
				AM(defQuiverA_p, sgCGears, false);
				AM(defPackA_p, sgCGears, true);
		/*	remove defPackA_e
		*/
			PickUp(defPackA_e, out picked);
			sgm.SimSBHover(null, eventData);
			sgm.SimSGHover(sgpAll, eventData);
			defPackA_e.OnPointerUpMock(eventData);
			defPackA_e.CurProcess.Expire();
			sgCGears.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
				AB(defPackA_p.IsEquipped, false);
				AE(sgm.GetCGEmptySlots().Count, 4);
				AE(sgm.GetEquippedCarriedGears().Count, 0);
				AM(defPackA_p, sgCGears, false);
		/*	fill equip defShieldA_p
		*/	
			AB(crfShieldA_e == null, true);
			AB(crfShieldA_p.IsEquipped, false);
			PickUp(crfShieldA_p, out picked);
			sgm.SimSBHover(null, eventData);
			sgm.SimSGHover(sgCGears, eventData);
			crfShieldA_p.OnPointerUpMock(eventData);
			crfShieldA_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgCGears.CurProcess.Expire();
				crfShieldA_e = sgCGears.GetSlottable(crfShieldA_p.Item);
				AB(crfShieldA_p.IsEquipped, true);
				AM(crfShieldA_p, sgCGears, true);
				AE(sgm.GetCGEmptySlots().Count, 3);
				AE(sgm.GetEquippedCarriedGears().Count, 1);
		/*	fill equip crfMWeaponA_p
		*/
			AB(crfMWeaponA_e == null, true);
			AB(crfMWeaponA_p.IsEquipped, false);
			PickUp(crfMWeaponA_p, out picked);
			sgm.SimSBHover(null, eventData);
			sgm.SimSGHover(sgCGears, eventData);
			crfMWeaponA_p.OnPointerUpMock(eventData);
			crfMWeaponA_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgCGears.CurProcess.Expire();
				crfMWeaponA_e = sgCGears.GetSlottable(crfMWeaponA_p.Item);
				AB(crfMWeaponA_p.IsEquipped, true);
				AM(crfMWeaponA_p, sgCGears, true);
				AE(sgm.GetCGEmptySlots().Count, 2);
				AE(sgm.GetEquippedCarriedGears().Count, 2);
		/*	swap equip crfBowA_p
		*/
			PickUp(crfBowA_p, out picked);
				ASSG(sgBow, SlotGroup.FocusedState);
				AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
			// sgm.SimSBHover(null, eventData);
			// sgm.SimSGHover(sgBow, eventData);
			sgm.SimHover(null, sgBow, eventData);
				AE(sgm.PickedSB, crfBowA_p);
				AE(sgm.SelectedSB, defBowA_e);//
				AE(sgm.SelectedSG, sgBow);
				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
			crfBowA_p.OnPointerUpMock(eventData);
				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
				AE(sgm.CurProcess.GetType(), typeof(SGMSwapTransactionProcess));
					AE(sgm.PickedSBDoneTransaction, false);
					AE(sgm.SelectedSBDoneTransaction, false);
					AE(sgm.OrigSGDoneTransaction, false);
					AE(sgm.SelectedSGDoneTransaction, false);

				ASSG(sgpAll, SlotGroup.PerformingTransactionState);
				AE(sgpAll.CurProcess.GetType(), typeof(SGUpdateTransactionProcess));

				ASSG(sgBow, SlotGroup.PerformingTransactionState);
				AE(sgBow.CurProcess.GetType(), typeof(SGUpdateTransactionProcess));

				ASSB(defBowA_e, Slottable.RemovingState);
				AE(defBowA_e.CurProcess.GetType(), typeof(SBRemovingProcess));
				ASSB(crfBowA_p, Slottable.EquippingState);
				AE(crfBowA_p.CurProcess.GetType(), typeof(SBEquippingProcess));

				AE(crfBowA_p.DestinationSG, sgBow);
				AE(crfBowA_p.DestinationSlot, sgBow.Slots[0]);
				AE(defBowA_e.DestinationSG, sgpAll);
				AE(defBowA_e.DestinationSlot, sgpAll.GetSlot(crfBowA_p));
			crfBowA_p.CurProcess.Expire();
				AE(sgm.CurProcess.GetType(), typeof(SGMSwapTransactionProcess));
					AE(sgm.PickedSBDoneTransaction, true);
					AE(sgm.SelectedSBDoneTransaction, false);
					AE(sgm.OrigSGDoneTransaction, false);
					AE(sgm.SelectedSGDoneTransaction, false);
				AE(crfBowA_p.DestinationSG, null);
				AE(crfBowA_p.DestinationSlot, null);

			defBowA_e.CurProcess.Expire();
				AE(sgm.CurProcess.GetType(), typeof(SGMSwapTransactionProcess));
					AE(sgm.PickedSBDoneTransaction, true);
					AE(sgm.SelectedSBDoneTransaction, true);
					AE(sgm.OrigSGDoneTransaction, false);
					AE(sgm.SelectedSGDoneTransaction, false);
				AE(defBowA_e.DestinationSG, null);
				AE(defBowA_e.DestinationSlot, null);

			sgpAll.CurProcess.Expire();
				AE(sgm.CurProcess.GetType(), typeof(SGMSwapTransactionProcess));
					AE(sgm.PickedSBDoneTransaction, true);
					AE(sgm.SelectedSBDoneTransaction, true);
					AE(sgm.OrigSGDoneTransaction, true);
					AE(sgm.SelectedSGDoneTransaction, false);

			sgBow.CurProcess.Expire();
				AE(sgm.CurState, SlotGroupManager.FocusedState);
				AE(sgm.CurProcess, null);
					AE(sgm.PickedSBDoneTransaction, true);
					AE(sgm.SelectedSBDoneTransaction, true);
					AE(sgm.OrigSGDoneTransaction, true);
					AE(sgm.SelectedSGDoneTransaction, true);
				AB(sgm.Transaction == null, true);

				ASSG(sgpAll, SlotGroup.FocusedState);
					AE(sgpAll.Slots.Count, 14);
				ASSG(sgBow, SlotGroup.FocusedState);
					AE(sgBow.Slots.Count, 1);
				
				AE(sgBow.Inventory.Items.Contains(defBowA_p.Item), false);
				AE(defBowA_p.IsEquipped, false);
				ASSB(defBowA_p, Slottable.FocusedState);
					AB(defBowA_p.IsEquipped, false);
					AB(defBowA_e == null, true);
				Slottable crfBowA_e = sgBow.GetSlottable(crfBowA_p.Item);
				ASSB(crfBowA_e, Slottable.EquippedAndDeselectedState);
					AB(crfBowA_e.IsEquipped, true);
					AB(crfBowA_p.IsEquipped, true);
				
				AE(sgm.GetSlotGroup(defBowA_p), sgpAll);
				AE(sgm.GetSlotGroup(crfBowA_e), sgBow);
				AE(sgm.GetEquippedBow(), crfBowA_e.Item);
					
		/*	swapping some more
		*/
			PickUp(defBowB_p, out picked);
			sgm.SimHover(null, sgBow, eventData);
			defBowB_p.OnPointerUpMock(eventData);
			defBowB_p.CurProcess.Expire();
			crfBowA_e.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgBow.CurProcess.Expire();
				AE(sgm.CurState, SlotGroupManager.FocusedState);
				AE(sgm.CurProcess, null);
				ASSG(sgpAll, SlotGroup.FocusedState);
				ASSG(sgBow, SlotGroup.FocusedState);
				ASSB(defBowB_p, Slottable.EquippedAndDefocusedState);
				Slottable defBowB_e = sgBow.GetSlottable(defBowB_p.Item);
				ASSB(defBowB_e, Slottable.EquippedAndDeselectedState);
				AB(crfBowA_e == null, true);
				ASSB(crfBowA_p, Slottable.FocusedState);

				AE(sgm.GetEquippedBow(), defBowB_p.Item);
				AE(sgBow.Slots[0].Sb, defBowB_e);
			PickUp(defBowA_p, out picked);
			sgm.SimHover(null, sgBow, eventData);
			defBowA_p.OnPointerUpMock(eventData);
			defBowA_p.CurProcess.Expire();
			defBowB_e.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgBow.CurProcess.Expire();
				AE(sgm.CurState, SlotGroupManager.FocusedState);
				AE(sgm.CurProcess, null);
				ASSG(sgpAll, SlotGroup.FocusedState);
				ASSG(sgBow, SlotGroup.FocusedState);
				ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
				defBowA_e = sgBow.GetSlottable(defBowA_p.Item);
				ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
				AB(defBowB_e == null, true);
				ASSB(defBowB_p, Slottable.FocusedState);

				AE(sgm.GetEquippedBow(), defBowA_p.Item);
				AE(sgBow.Slots[0].Sb, defBowA_e);
		/*	swap equip defWearB_p and then to crfWearA_p
		*/
			PickUp(defWearB_p, out picked);
			sgm.SimHover(null, sgWear, eventData);
			defWearB_p.OnPointerUpMock(eventData);
			defWearB_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgWear.CurProcess.Expire();
			defWearA_e.CurProcess.Expire();
				AE(sgm.CurState, SlotGroupManager.FocusedState);
				AE(sgm.CurProcess, null);
				ASSG(sgpAll, SlotGroup.FocusedState);
				ASSG(sgWear, SlotGroup.FocusedState);
				ASSB(defWearB_p, Slottable.EquippedAndDefocusedState);
				Slottable defWearB_e = sgWear.GetSlottable(defWearB_p.Item);
				ASSB(defWearB_e, Slottable.EquippedAndDeselectedState);
				AB(defWearA_e == null, true);
				ASSB(defWearA_p, Slottable.FocusedState);
				AE(sgm.GetEquippedWear(), defWearB_p.Item);
				AE(sgWear.Slots[0].Sb, defWearB_e);
			PickUp(crfWearA_p, out picked);
			sgm.SimHover(null, sgWear, eventData);
			crfWearA_p.OnPointerUpMock(eventData);
			crfWearA_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgWear.CurProcess.Expire();
			defWearB_e.CurProcess.Expire();
				AE(sgm.CurState, SlotGroupManager.FocusedState);
				AE(sgm.CurProcess, null);
				ASSG(sgpAll, SlotGroup.FocusedState);
				ASSG(sgWear, SlotGroup.FocusedState);
				ASSB(crfWearA_p, Slottable.EquippedAndDefocusedState);
				Slottable crfWearA_e = sgWear.GetSlottable(crfWearA_p.Item);
				ASSB(crfWearA_e, Slottable.EquippedAndDeselectedState);
				AB(defWearB_e == null, true);
				ASSB(defWearB_p, Slottable.FocusedState);
				AE(sgm.GetEquippedWear(), crfWearA_p.Item);
				AE(sgWear.Slots[0].Sb, crfWearA_e);
		/*	swap bow explicitly on equipped sb back to defBowA_p
		*/
				AE(sgm.GetEquippedBow(), defBowA_e.Item);
				AE(sgBow.Slots[0].Sb, defBowA_e);
			PickUp(defBowB_p, out picked);
			sgm.SimHover(defBowA_e, sgBow, eventData);
				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
			sgm.SimHover(defBowB_p, sgpAll, eventData);
			bool reverted = false;
			Revert(defBowB_p, out reverted);
				AB(reverted, true);

				AssertEquippedBow(defBowA_p, defBowB_p);
			PickUp(defBowB_p, out picked);
			sgm.SimHover(defBowA_e, sgBow, eventData);
				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
			defBowB_p.OnPointerUpMock(eventData);
			defBowB_p.CurProcess.Expire();
			defBowA_e.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgBow.CurProcess.Expire();
				AssertEquippedBow(defBowB_p, defBowA_p);
			
			defBowB_e = sgBow.Slots[0].Sb;
			PickUp(defBowA_p, out picked);
			sgm.SimHover(defBowB_e, sgBow, eventData);
			defBowA_p.OnPointerUpMock(eventData);
			defBowA_p.CurProcess.Expire();
			defBowB_e.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgBow.CurProcess.Expire();
				AssertEquippedBow(defBowA_p, defBowB_p);
		/*	swap wears explicitly to defWearB_p
		*/
			PickUp(defWearB_p, out picked);
			sgm.SimHover(crfWearA_e, sgWear, eventData);
			defWearB_p.OnPointerUpMock(eventData);
			defWearB_p.CurProcess.Expire();
			crfWearA_e.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgWear.CurProcess.Expire();
				AssertEquippedWear(defWearB_p, crfWearA_p);
		/*	explicity swap on pool bow defBowB_p
		*/
			AE(sgm.GetEquippedBow(), defBowA_p.Item);
			defBowA_e = sgBow.GetSlottable(defBowA_p.Item);
				ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
			PickUp(defBowA_e, out picked);
				AB(picked, true);
			sgm.SimHover(defBowB_p, sgpAll, eventData);
				AB(sgm.Transaction == null, false);
				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
					AE(sgm.PickedSB, defBowA_e);
					AE(sgm.SelectedSB, defBowB_p);
					AE(sgm.SelectedSG, sgpAll);
			defBowA_e.OnPointerUpMock(eventData);
			defBowA_e.CurProcess.Expire();
			defBowB_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgBow.CurProcess.Expire();
				AssertEquippedBow(defBowB_p, defBowA_p);
			/*	make sure it reverts back when dropped on sg only
			*/
					defBowB_e = sgBow.GetSlottable(defBowB_p.Item);
				PickUp(defBowB_e, out picked);
				sgm.SimHover(null, sgpAll, eventData);
					AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
				Revert(defBowB_e, out reverted);
					AB(reverted, true);
		/*	then back to defWearA_p
		*/
			PickUp(defBowB_e, out picked);
			sgm.SimHover(defBowA_p, sgpAll, eventData);
			defBowB_e.OnPointerUpMock(eventData);
			defBowB_e.CurProcess.Expire();
			defBowA_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgBow.CurProcess.Expire();
				AssertEquippedBow(defBowA_p, defBowB_p);
		/*	explicitly swap wear from defWearB_e to crfWearA_p
		*/
				AE(sgm.GetEquippedWear(), defWearB_p.Item);
				defWearB_e = sgWear.Slots[0].Sb;
			PickUp(defWearB_e, out picked);
			sgm.SimHover(crfWearA_p, sgpAll, eventData);
			defWearB_e.OnPointerUpMock(eventData);
			defWearB_e.CurProcess.Expire();
			crfWearA_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgWear.CurProcess.Expire();
				AssertEquippedWear(crfWearA_p, defWearB_p);
				AB(picked, true);
		/*	explicitly swap wear from crfWearA_e to defWearA_p
		*/
			crfWearA_e = sgWear.Slots[0].Sb;
			PickUp(crfWearA_e, out picked);
			sgm.SimHover(defWearA_p, sgpAll, eventData);
			crfWearA_e.OnPointerUpMock(eventData);
			crfWearA_e.CurProcess.Expire();
			defWearA_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgWear.CurProcess.Expire();
				AssertEquippedWear(defWearA_p, crfWearA_p);
		/*	explicitly swap from defQuiverA_p to defShieldA_e, then from defQuiverA_p to 		crfMWeaponA_e
		*/
				AssertFocusedGeneric();
				AE(sgm.GetEquippedCarriedGears().Count, 2);
				AssertEquippedCGears(crfShieldA_p, crfMWeaponA_p, null, null);
			PickUp(defShieldA_p, out picked);
			crfShieldA_e = sgCGears.GetSlottable(crfShieldA_p.Item);
			sgm.SimHover(crfShieldA_e, sgCGears, eventData);
				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
			defShieldA_p.OnPointerUpMock(eventData);
			defShieldA_p.CurProcess.Expire();
			crfShieldA_e.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgCGears.CurProcess.Expire();
				AssertFocusedGeneric();
				AssertEquippedCGears(defShieldA_p, crfMWeaponA_p, null, null);
			
			PickUp(defQuiverA_p, out picked);
			crfMWeaponA_e = sgCGears.GetSlottable(crfMWeaponA_p.Item);
			sgm.SimHover(crfMWeaponA_e, sgCGears, eventData);
				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
			defQuiverA_p.OnPointerUpMock(eventData);
			crfMWeaponA_e.CurProcess.Expire();
			defQuiverA_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgCGears.CurProcess.Expire();
				AssertFocusedGeneric();
				AssertEquippedCGears(defQuiverA_p, defShieldA_p, null, null);
		/*	explicit swap from defShieldA_e to defPackA_p, then from defPackA_e to 			crfShieldA_p
		*/
			defShieldA_e = sgCGears.GetSlottable(defShieldA_p.Item);
			PickUp(defShieldA_e, out picked);
			sgm.SimHover(defPackA_p, sgpAll, eventData);
				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
			defShieldA_e.OnPointerUpMock(eventData);
			defShieldA_e.CurProcess.Expire();
			defPackA_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgCGears.CurProcess.Expire();
				AssertFocusedGeneric();
				AssertEquippedCGears(defQuiverA_p, defPackA_p, null, null);
			
			defPackA_e = sgCGears.GetSlottable(defPackA_p.Item);
			PickUp(defPackA_e, out picked);
			sgm.SimHover(crfShieldA_p, sgpAll, eventData);
			defPackA_e.OnPointerUpMock(eventData);
			defPackA_e.CurProcess.Expire();
			crfShieldA_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgCGears.CurProcess.Expire();
				AssertFocusedGeneric();
				AssertEquippedCGears(defQuiverA_p, crfShieldA_p, null, null);
		/*	fill equip defMWeaponA_p and defPackA_p
		*/
			PickUp(defMWeaponA_p, out picked);
			sgm.SimHover(null, sgCGears, eventData);
			defMWeaponA_p.OnPointerUpMock(eventData);
			defMWeaponA_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgCGears.CurProcess.Expire();
				AssertFocusedGeneric();
				AssertEquippedCGears(defQuiverA_p, crfShieldA_p, defMWeaponA_p, null);
			PickUp(defPackA_p, out picked);
			sgm.SimHover(null, sgCGears, eventData);
			defPackA_p.OnPointerUpMock(eventData);
			defPackA_p.CurProcess.Expire();
			sgpAll.CurProcess.Expire();
			sgCGears.CurProcess.Expire();
				AssertFocusedGeneric();
				AssertEquippedCGears(defQuiverA_p, crfShieldA_p, defMWeaponA_p, defPackA_p);
		/*	assert equipment
		*/
			
			AssertEquipped(defBowA_p);
			AssertEquipped(defWearA_p);
			AssertEquippedCGears(defQuiverA_p, crfShieldA_p, defMWeaponA_p, defPackA_p);

	}
	
	/*	CarriedGearsTesting
	*/
		public void TestCarriedGearsSetup(){
			/*	setting up
			*/
				AB(sgCGears != null, true);
				AE(sgCGears.Filter.GetType(), typeof(SGCarriedGearFilter));
				AE(sgCGears.Sorter.GetType(), typeof(SGItemIndexSorter));
				// AE(sgCGears.UpdateEquipStatusCommand.GetType(), typeof(UpdateEquipStatusForEquipSGCommand));
				AE(sgCGears.Inventory, sgBow.Inventory);
				AB(sgCGears.IsShrinkable, true);
				AB(sgCGears.IsExpandable, false);

				AE(((EquipmentSet)sgm.RootPage.EquipBundle.GetFocusedBundleElement()).Elements.Count, 3);
				AB(sgm.RootPage.EquipBundle.GetFocusedBundleElement().ContainsElement(sgCGears), true);
				AE(sgCGears.Slots.Count, 4);
				AE(sgm.GetEquippedCarriedGears().Count, 2);

				AE(sgpAll.Slots.Count, 14);
				AB(defShieldA_p == null, false);
				AE(sgm.GetSlotGroup(defShieldA_p), sgpAll);
				AB(crfShieldA_p == null, false);
				AE(sgm.GetSlotGroup(crfShieldA_p), sgpAll);
				AB(defMWeaponA_p == null, false);
				AE(sgm.GetSlotGroup(defMWeaponA_p), sgpAll);
				AB(crfMWeaponA_p == null, false);
				AE(sgm.GetSlotGroup(crfMWeaponA_p), sgpAll);
				AB(defShieldA_e == null, false);
				AE(sgm.GetSlotGroup(defShieldA_e), sgCGears);
				AB(defMWeaponA_e == null, false);
				AE(sgm.GetSlotGroup(defMWeaponA_e), sgCGears);
				
				ASSG(sgpAll, SlotGroup.FocusedState);
				ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
				ASSB(crfShieldA_p, Slottable.FocusedState);
				ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
				ASSB(crfMWeaponA_p, Slottable.FocusedState);

				ASSG(sgCGears, SlotGroup.FocusedState);
				ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
				ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);

				sgm.Defocus();

				ASSG(sgpAll, SlotGroup.DefocusedState);
				ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
				ASSB(crfShieldA_p, Slottable.DefocusedState);
				ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
				ASSB(crfMWeaponA_p, Slottable.DefocusedState);

				ASSG(sgCGears, SlotGroup.DefocusedState);
				ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
				ASSB(defMWeaponA_e, Slottable.EquippedAndDefocusedState);

				sgm.Focus();
				sgm.SetFocusedPoolSG(sgpParts);
				ASSG(sgpAll, SlotGroup.DefocusedState);
				ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
				ASSB(crfShieldA_p, Slottable.DefocusedState);
				ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
				ASSB(crfMWeaponA_p, Slottable.DefocusedState);

				ASSG(sgCGears, SlotGroup.FocusedState);
				ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
				ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
				
				sgm.SetFocusedPoolSG(sgpAll);
		}		
	/*	Test hover
	*/
		bool picked;
		Slottable selectedSB;
		SlotGroup selectedSG;
		SlotSystemTransaction transaction;
		public void TestHover(){
			
			SlotGroup origSG = sgm.GetSlotGroup(defBowB_p);
			Slottable targetSB;

			// /*	test reorder
				// */
				// AB(origSG.AutoSort, true);
				// origSG.AutoSort = false;

				// TestSimHover(defBowBSB_p, defWearBSB_p, origSG, out picked, out selectedSBChanged, out selectedSGChanged, out transaction);
				// 	AB(picked, true);
				// 	AB(selectedSBChanged, true);
				// 	AB(selectedSGChanged, false);
				// 	AE(transaction.GetType(), typeof(ReorderTransaction));
				
				// TestSimHover(defBowBSB_p, crfBowASB_p, origSG, out picked, out selectedSBChanged, out selectedSGChanged, out transaction);
				// 	AB(picked, true);
				// 	AB(selectedSBChanged, true);
				// 	AB(selectedSGChanged, false);
				// 	AE(transaction.GetType(), typeof(ReorderTransaction));
			// /*	test swap
				// */
				// TestSimHover(defBowBSB_p, defBowASB_e, sgBow, out picked, out selectedSBChanged, out selectedSGChanged, out transaction);
				// 	AB(picked, true);
				// 	AB(selectedSBChanged, true);
				// 	AB(selectedSGChanged, true);
				// 	AE(transaction.GetType(), typeof(SwapTransaction));
				
				// TestSimHover(defWearBSB_p, defWearASB_e, sgWear, out picked, out selectedSBChanged, out selectedSGChanged, out transaction);
				// 	AB(picked, true);
				// 	AB(selectedSBChanged, true);
				// 	AB(selectedSGChanged, true);
				// 	AE(transaction.GetType(), typeof(SwapTransaction));
			/**/

			// TestPostPickFilter();

			TestSimHoverOnAllSB();
				// TestSimHoverOnAllSB(defBowASB_p);
				// TestSimHoverOnAllSB(defBowBSB_p);
				// TestSimHoverOnAllSB(crfBowASB_p);
				// TestSimHoverOnAllSB(defWearASB_p);
				// TestSimHoverOnAllSB(defWearBSB_p);
				// TestSimHoverOnAllSB(crfWearASB_p);
				// TestSimHoverOnAllSB(defBowASB_e);
				// TestSimHoverOnAllSB(defWearASB_e);
				// TestSimHoverOnAllSB(defBowASB_p);
			
				
			/*
			*/
			// TestHoverDefBowASBE();
			// TestHoverDefBowASBP();
			// TestHoverDefBowBSBP();
			// TestHoverDefWearASBE();
			//TestHoverPartsInSGPParts();
			// TestHoverDefShieldA();
				
		}
		public void TestPostPickFilter(){
			AB(sgpAll.AutoSort, true);
			bool picked;
			bool reverted;
			PickUp(defBowB_p, out picked);
				ASSG(sgpAll, SlotGroup.SelectedState);
					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowB_p, Slottable.PickedAndSelectedState);
					ASSB(crfBowA_p, Slottable.DefocusedState);
					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearB_p, Slottable.DefocusedState);
					ASSB(crfWearA_p, Slottable.DefocusedState);
					ASSB(defParts_p, Slottable.DefocusedState);
					ASSB(crfParts_p, Slottable.DefocusedState);

					ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfShieldA_p, Slottable.DefocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfMWeaponA_p, Slottable.DefocusedState);

				ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
				AB(sgm.RootPage.EquipBundle.GetFocusedBundleElement().ContainsElement(sgWear), true);
				AB(sgWear.AcceptsFilter(defBowB_p), false);
				ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
				ASSB(defParts_p2, Slottable.DefocusedState);
				ASSB(crfParts_p2, Slottable.DefocusedState);

				ASSG(sgCGears, SlotGroup.DefocusedState);
					ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
					ASSB(defMWeaponA_e, Slottable.EquippedAndDefocusedState);

			Revert(defBowB_p, out reverted);

			AssertSGMFocused();
			sgpAll.AutoSort = false;
			PickUp(defBowB_p, out picked);
				ASSG(sgpAll, SlotGroup.SelectedState);
					ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
					ASSB(defBowB_p, Slottable.PickedAndSelectedState);
					ASSB(crfBowA_p, Slottable.FocusedState);
					ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);
					ASSB(defWearB_p, Slottable.FocusedState);
					ASSB(crfWearA_p, Slottable.FocusedState);
					ASSB(defParts_p, Slottable.FocusedState);
					ASSB(crfParts_p, Slottable.FocusedState);

					ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);
					ASSB(crfShieldA_p, Slottable.FocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
					ASSB(crfMWeaponA_p, Slottable.FocusedState);

				ASSG(sgBow, SlotGroup.FocusedState);
					ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
				ASSG(sgWear, SlotGroup.DefocusedState);
					ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgCGears, SlotGroup.DefocusedState);
					ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
					ASSB(defMWeaponA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgpParts, SlotGroup.DefocusedState);
					ASSB(defParts_p2, Slottable.DefocusedState);
					ASSB(crfParts_p2, Slottable.DefocusedState);
			Revert(defBowB_p, out reverted);
			AssertSGMFocused();
			//

				ASSG(sgpAll, SlotGroup.FocusedState);
					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowB_p, Slottable.FocusedState);
					ASSB(crfBowA_p, Slottable.FocusedState);
					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearB_p, Slottable.FocusedState);
					ASSB(crfWearA_p, Slottable.FocusedState);
					ASSB(defParts_p, Slottable.DefocusedState);
					ASSB(crfParts_p, Slottable.DefocusedState);
					ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfShieldA_p, Slottable.FocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfMWeaponA_p, Slottable.FocusedState);
				ASSG(sgBow, SlotGroup.FocusedState);
					ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
				ASSG(sgWear, SlotGroup.FocusedState);
					ASSB(defWearA_e, Slottable.EquippedAndDeselectedState);
				ASSG(sgCGears, SlotGroup.FocusedState);
					ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
					ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
				ASSG(sgpParts, SlotGroup.DefocusedState);
					ASSB(defParts_p2, Slottable.DefocusedState);
					ASSB(crfParts_p2, Slottable.DefocusedState);
			
			AB(sgpAll.AutoSort, false);
			
			PickUp(defBowA_e, out picked);
			
				ASSG(sgpAll, SlotGroup.FocusedState);
					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowB_p, Slottable.FocusedState);
					ASSB(crfBowA_p, Slottable.FocusedState);
					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearB_p, Slottable.DefocusedState);
					ASSB(crfWearA_p, Slottable.DefocusedState);
					ASSB(defParts_p, Slottable.DefocusedState);
					ASSB(crfParts_p, Slottable.DefocusedState);
					ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfShieldA_p, Slottable.DefocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfMWeaponA_p, Slottable.DefocusedState);
				ASSG(sgBow, SlotGroup.SelectedState);
					ASSB(defBowA_e, Slottable.PickedAndSelectedState);
				ASSG(sgWear, SlotGroup.DefocusedState);
					ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgCGears, SlotGroup.DefocusedState);
					ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
					ASSB(defMWeaponA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgpParts, SlotGroup.DefocusedState);
					ASSB(defParts_p2, Slottable.DefocusedState);
					ASSB(crfParts_p2, Slottable.DefocusedState);
				
			Revert(defBowA_e, out reverted);
			AssertSGMFocused();

			PickUp(defShieldA_p, out picked);
				ASSG(sgpAll, SlotGroup.FocusedState);
				ASSG(sgBow, SlotGroup.FocusedState);
				ASSG(sgWear, SlotGroup.FocusedState);
				ASSG(sgCGears, SlotGroup.FocusedState);
				ASSG(sgpParts, SlotGroup.DefocusedState);
			Revert(defShieldA_p, out reverted);
			AB(picked, false);
			AB(reverted, false);

			PickUp(crfShieldA_p, out picked);
				ASSG(sgpAll, SlotGroup.SelectedState);
					ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
					ASSB(defBowB_p, Slottable.FocusedState);
					ASSB(crfBowA_p, Slottable.FocusedState);
					ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);
					ASSB(defWearB_p, Slottable.FocusedState);
					ASSB(crfWearA_p, Slottable.FocusedState);
					ASSB(defParts_p, Slottable.FocusedState);
					ASSB(crfParts_p, Slottable.FocusedState);
					ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);
					ASSB(crfShieldA_p, Slottable.PickedAndSelectedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
					ASSB(crfMWeaponA_p, Slottable.FocusedState);
				ASSG(sgBow, SlotGroup.DefocusedState);
					ASSB(defBowA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgWear, SlotGroup.DefocusedState);
					ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgCGears, SlotGroup.FocusedState);
					ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
					ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
				ASSG(sgpParts, SlotGroup.DefocusedState);
					ASSB(defParts_p2, Slottable.DefocusedState);
					ASSB(crfParts_p2, Slottable.DefocusedState);
			Revert(crfShieldA_p, out reverted);
			AB(picked, true);
			AB(reverted, true);
			AssertSGMFocused();

			sgpAll.AutoSort = true;
			sgCGears.AutoSort = false;
			AB(sgCGears.AutoSort, false);
			PickUp(defShieldA_e, out picked);
			AB(picked, true);
				ASSG(sgpAll, SlotGroup.FocusedState);
					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowB_p, Slottable.DefocusedState);
					ASSB(crfBowA_p, Slottable.DefocusedState);
					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearB_p, Slottable.DefocusedState);
					ASSB(crfWearA_p, Slottable.DefocusedState);
					ASSB(defParts_p, Slottable.DefocusedState);
					ASSB(crfParts_p, Slottable.DefocusedState);
						AB(object.ReferenceEquals(defShieldA_e.Item, defShieldA_p.Item), true);
						AB(sgm.GetSlotGroup(defShieldA_e).IsShrinkable, true);
						AB(SlotSystem.Utility.HaveCommonItemFamily(defShieldA_e, defShieldA_p), true);
					ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);//
					ASSB(crfShieldA_p, Slottable.FocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfMWeaponA_p, Slottable.FocusedState);
				ASSG(sgBow, SlotGroup.DefocusedState);
					ASSB(defBowA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgWear, SlotGroup.DefocusedState);
					ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgCGears, SlotGroup.SelectedState);
					ASSB(defShieldA_e, Slottable.PickedAndSelectedState);
					ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
				ASSG(sgpParts, SlotGroup.DefocusedState);
					ASSB(defParts_p2, Slottable.DefocusedState);
					ASSB(crfParts_p2, Slottable.DefocusedState);
			AB(reverted, true);
			Revert(defShieldA_e, out reverted);
			AssertSGMFocused();
		}
		/*	spot tests hover
		*/
			public void TestHoverDefShieldA(){
				bool picked;
				bool reverted;
				AssertSGMFocused();
				sgm.SetFocusedPoolSG(sgpParts);
				AssertSGMFocused();
				sgm.SetFocusedPoolSG(sgpAll);
				AssertSGMFocused();

				PickUp(defShieldA_p, out picked);
				Revert(defShieldA_p, out reverted);
				AssertSGMFocused();
				AB(picked, false);
				AB(reverted, false);


				/*	picking crfShieldA_p
				*/
					Slottable pickedSB = crfShieldA_p;
					AB(sgpAll.AutoSort, true);
					Slottable target;
					/*	sgpAll
					*/
						target = defBowA_p;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
						target = pickedSB;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, pickedSB, sgpAll, false);
						
						sgpAll.AutoSort = false;
						
						target = defBowA_p;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<ReorderTransaction>(true, target, sgpAll, false);
						target = defShieldA_p;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<ReorderTransaction>(true, target, sgpAll, false);
						target = pickedSB;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, pickedSB, sgpAll, false);
					/*	equip sgs
					*/
						target = defBowA_e;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, null, null, false);
						target = defShieldA_e;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<SwapTransaction>(true, target, sgCGears, false);
						target = defMWeaponA_e;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<SwapTransaction>(true, target, sgCGears, false);


				
				
				/*	picking defMWeaponA_p
				*/
					pickedSB = defMWeaponA_p;
					target = defBowB_p;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(false, null, null, true);
				/*	picking defMWeaponA_e
				*/
					AB(sgpAll.AutoSort, false);
					AB(sgCGears.AutoSort, true);
					pickedSB = defMWeaponA_e;
					target = defBowB_p;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
					target = defMWeaponA_p;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<UnequipTransaction>(true, defMWeaponA_p, sgpAll, false);
					target = crfShieldA_p;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<SwapTransaction>(true, target, sgpAll, false);
					target = crfMWeaponA_p;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<SwapTransaction>(true, target, sgpAll, false);
					target = defShieldA_p;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
					target = defBowA_e;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(true, null, null, false);
					target = defWearA_e;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(true, null, null, false);
					
					target = defShieldA_e;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(true, null, sgCGears, false);
					
			}
			public void TestHoverDefBowASBE(){
				SlotGroup origSG = sgBow;
				/*	defBowASB_e
				*/
					/*	AutoSort false
					*/
						Slottable targetSB = defBowA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defBowB_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, defBowB_p);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(SwapTransaction));

						targetSB = crfBowA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, crfBowA_p);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(SwapTransaction));

						targetSB = defWearA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defWearB_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = crfWearA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));

						targetSB = defParts_p2;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = crfParts_p2;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defBowA_e;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, defBowA_e);
							AE(selectedSG, sgBow);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defWearA_e;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defShieldA_p;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
						targetSB = crfShieldA_p;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
						targetSB = defShieldA_e;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, null, null, false);
						
						
						sgm.SetFocusedPoolSG(sgpParts);
						
							targetSB = defParts_p2;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
								AB(picked, true);
								AE(selectedSB, null);
								AE(selectedSG, sgpParts);
								AE(transaction.GetType(), typeof(RevertTransaction));
							
							targetSB = crfParts_p2;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
								AB(picked, true);
								AE(selectedSB, null);
								AE(selectedSG, sgpParts);
								AE(transaction.GetType(), typeof(RevertTransaction));
					/*	AutoSort true
					*/
						sgm.SetFocusedPoolSG(sgpAll);
						origSG.AutoSort = true;
						targetSB = defBowA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defBowB_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, defBowB_p);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(SwapTransaction));

						targetSB = crfBowA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, crfBowA_p);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(SwapTransaction));

						targetSB = defWearA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defWearB_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = crfWearA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));

						targetSB = defParts_p2;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = crfParts_p2;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defBowA_e;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, defBowA_e);
							AE(selectedSG, sgBow);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defWearA_e;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						sgm.SetFocusedPoolSG(sgpParts);
						
							targetSB = defParts_p2;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
								AB(picked, true);
								AE(selectedSB, null);
								AE(selectedSG, sgpParts);
								AE(transaction.GetType(), typeof(RevertTransaction));
							
							targetSB = crfParts_p2;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
								AB(picked, true);
								AE(selectedSB, null);
								AE(selectedSG, sgpParts);
								AE(transaction.GetType(), typeof(RevertTransaction));
			}
			public void TestHoverDefBowASBP(){
				Slottable targetSB;
				targetSB = defBowA_p;
						TestSimHover(defBowA_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(false, null, null, true);
					targetSB = defBowB_p;
						TestSimHover(defBowA_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(false, null, null, true);
					targetSB = defBowA_e;
						TestSimHover(defBowA_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(false, null, null, true);
			}
			public void TestHoverDefBowBSBP(){
				sgpAll.AutoSort = true;
				sgm.SetFocusedPoolSG(sgpAll);
				AssertSGMFocused();

				Slottable targetSB;
				targetSB = defBowA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				targetSB = defBowB_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, defBowB_p, sgpAll, false);
				targetSB = crfBowA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				targetSB = defBowA_e;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<SwapTransaction>(true, defBowA_e, sgBow, false);
				targetSB = defWearA_e;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				targetSB = defShieldA_e;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				targetSB = defShieldA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				

				sgpAll.AutoSort = false;

				targetSB = defBowA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, defBowA_p, sgpAll, false);
				targetSB = crfBowA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, crfBowA_p, sgpAll, false);
				targetSB = defBowB_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, defBowB_p, sgpAll, false);
				targetSB = defWearA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, defWearA_p, sgpAll, false);
				targetSB = defWearB_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, defWearB_p, sgpAll, false);
				targetSB = crfWearA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, crfWearA_p, sgpAll, false);
				targetSB = defParts_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, defParts_p, sgpAll, false);
				targetSB = crfParts_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, crfParts_p, sgpAll, false);
				targetSB = defBowA_e;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<SwapTransaction>(true, defBowA_e, sgBow, false);
				targetSB = defWearA_e;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				targetSB = defParts_p2;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				targetSB = crfParts_p2;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);

			}
			public void TestHoverDefWearASBE(){
				sgm.SetFocusedPoolSG(sgpAll);
				AssertSGMFocused();
				Slottable target;

				target = defBowA_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = defBowB_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = crfBowA_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = defWearA_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = defWearB_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<SwapTransaction>(true, defWearB_p, sgpAll, false);
				target = crfWearA_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<SwapTransaction>(true, crfWearA_p, sgpAll, false);
				target = defParts_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = crfParts_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				
				target = defBowA_e;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				target = defWearA_e;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, defWearA_e, sgWear, false);
				target = defParts_p2;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				
				sgm.SetFocusedPoolSG(sgpParts);
				target = defParts_p2;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpParts, false);
				sgm.SetFocusedPoolSG(sgpAll);


			}
			public void TestHoverPartsInSGPParts(){
				sgm.SetFocusedPoolSG(sgpParts);
				AssertSGMFocused();
				Slottable target;

				target = defParts_p2;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, target, sgpParts, false);
				target = crfParts_p2;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpParts, false);
				
				sgpParts.AutoSort = false;
				target = defParts_p2;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, target, sgpParts, false);
				target = crfParts_p2;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, target, sgpParts, false);
				sgpParts.AutoSort = true;
				
				target = defParts_p;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				target = defBowA_e;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				target = defWearA_e;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
			}
		public void AssertSimHover<T>(bool picked, Slottable sb, SlotGroup sg, bool TSNull){
			if(picked) AB(picked, true); else AB(picked, false);
			if(sb != null) AE(selectedSB, sb); else AB(selectedSB == null, true);
			if(sg != null) AE(selectedSG, sg); else AB(selectedSG == null, true);
			if(!TSNull) AE(transaction.GetType(), typeof(T)); else AB(transaction == null, true);
		}
		public void TestSimHoverOnAllSB(){
			foreach(Slottable sb in SlottableList()){
				foreach(Slottable hoveredSB in SlottableList()){
					SlotGroup destSG = sgm.GetSlotGroup(hoveredSB);
					bool picked;
					Slottable selectedSB;
					SlotGroup selectedSG;
					SlotSystemTransaction transaction;
					TestSimHover(sb, hoveredSB, destSG, out picked, out selectedSB, out selectedSG, out transaction);
				}
			}
		}
		public void TestSimHover(Slottable pickedSB, Slottable hoveredSB, SlotGroup hoveredSG, out bool picked, out Slottable selectedSB, out SlotGroup selectedSG, out SlotSystemTransaction transaction){
			SlotGroup origSG = sgm.GetSlotGroup(pickedSB);// == SelectedSG
			
			if(pickedSB.CurState == Slottable.FocusedState || pickedSB.CurState == Slottable.EquippedAndDeselectedState){

				PickUp(pickedSB, out picked);//picked = true;
				ASSB(pickedSB, Slottable.PickedAndSelectedState);
				AssertPostPickFilter(pickedSB);
				
				if(hoveredSB != null){
					SlotGroup destSG = sgm.GetSlotGroup(hoveredSB);
					
					if(hoveredSB == pickedSB){
						
						selectedSB = pickedSB;
						selectedSG = origSG;
						sgm.SimSBHover(hoveredSB, eventData);

						AE(hoveredSB.PrevState, Slottable.PickedAndSelectedState);
						ASSB(hoveredSB, Slottable.PickedAndSelectedState);
						AB((hoveredSB.CurProcess.GetType() == typeof(SBPickUpProcess) || hoveredSB.CurProcess.GetType() == typeof(SBHighlightProcess)), true);
						// AE(hoveredSB.CurProcess.GetType(), typeof(PickedUpAndSelectedProcess));
						AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
						transaction = sgm.Transaction;

					}else{//	hover is not null nor same as pickedSb
						if(origSG == destSG){
							selectedSG = origSG;
							// ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
							if(hoveredSB.CurState == Slottable.FocusedState){
								selectedSB = hoveredSB;
								sgm.SimSBHover(hoveredSB, eventData);
								sgm.SimSGHover(hoveredSG, eventData);
								ASSB(hoveredSB, Slottable.SelectedState);
								AE(hoveredSB.CurProcess.GetType(), typeof(SBHighlightProcess));
								ASSB(pickedSB, Slottable.PickedAndDeselectedState);
								AE(pickedSB.CurProcess.GetType(), typeof(SBDehighlightProcess));

								AE(sgm.Transaction.GetType(), typeof(ReorderTransaction));
								transaction = sgm.Transaction;

								sgm.SimSBHover(pickedSB, eventData);

							}else if(hoveredSB.CurState == Slottable.EquippedAndDeselectedState){
								selectedSB = hoveredSB;
								sgm.SimSBHover(hoveredSB, eventData);
								sgm.SimSGHover(hoveredSG, eventData);
								ASSB(hoveredSB, Slottable.EquippedAndSelectedState);
								AE(hoveredSB.CurProcess.GetType(), typeof(SBHighlightProcess));
								ASSB(pickedSB, Slottable.PickedAndDeselectedState);
								AE(pickedSB.CurProcess.GetType(), typeof(SBDehighlightProcess));

								AE(sgm.Transaction.GetType(), typeof(ReorderTransaction));
								transaction = sgm.Transaction;

								sgm.SimSBHover(pickedSB, eventData);

							}else{
								selectedSB = null;
								SlottableState preState = hoveredSB.CurState;
								sgm.SimSBHover(hoveredSB, eventData);
								sgm.SimSGHover(hoveredSG, eventData);
								ASSB(hoveredSB, preState);
								AE(pickedSB.PrevState, Slottable.PickedAndSelectedState);
								ASSB(pickedSB, Slottable.PickedAndDeselectedState);
								AE(pickedSB.CurProcess.GetType(), typeof(SBDehighlightProcess));

								AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
								transaction = sgm.Transaction;

								sgm.SimSBHover(pickedSB, eventData);
							}		
						}else{
							/*	destSG != null, != origSG
								selectedSG == null || == destSG || != origSG
								hoveredSB != null, != pickdSB
								fill, swap, stack
								not revert
							*/
							AE(sgm.SelectedSB, pickedSB);
							AE(sgm.SelectedSG, origSG);
							// ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
							if(hoveredSB.CurState == Slottable.FocusedState || hoveredSB.CurState == Slottable.EquippedAndDeselectedState || hoveredSB.CurState == Slottable.PickedAndDeselectedState){
								sgm.SimSBHover(hoveredSB, eventData);
									AE(sgm.SelectedSB, hoveredSB);
									selectedSB = hoveredSB;
									if(hoveredSB.CurState != Slottable.PickedAndSelectedState){
										if(hoveredSB.IsEquipped)
											ASSB(hoveredSB, Slottable.EquippedAndSelectedState);
										else
											ASSB(hoveredSB, Slottable.SelectedState);
									}else
										ASSB(hoveredSB, Slottable.PickedAndSelectedState);
								
								if(hoveredSB.Item.IsStackable)
									AE(sgm.Transaction.GetType(), typeof(StackTransaction));
								else{
									if(object.ReferenceEquals(hoveredSB.Item, pickedSB.Item) && sgm.GetSlotGroup(hoveredSB).IsShrinkable)
										AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
									else
										AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
								}
								transaction = sgm.Transaction;
							}else{// hoveredSB not in a state to be hovered
								SlottableState preState = hoveredSB.CurState;
								selectedSB = null;
								sgm.SimSBHover(hoveredSB, eventData);
									AE(sgm.SelectedSB, null);
									ASSB(hoveredSB, preState);
								
								AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
								transaction = sgm.Transaction;
							}


							ASSB(pickedSB, Slottable.PickedAndDeselectedState);
							/*	selected, eqSelected, pickSele, 
							*/
							if(hoveredSG.CurState == SlotGroup.FocusedState){
								selectedSG = hoveredSG;
								sgm.SimSGHover(hoveredSG, eventData);
									AE(sgm.SelectedSG, hoveredSG);
									ASSG(hoveredSG, SlotGroup.SelectedState);
							}else{
								selectedSG = null;
								SlotGroupState preState = hoveredSG.CurState;
								sgm.SimSGHover(hoveredSG, eventData);
									AE(sgm.SelectedSG, null);
									ASSG(hoveredSG, preState);
							}

							sgm.SimSBHover(pickedSB, eventData);
							sgm.SimSGHover(origSG, eventData);
						}
					}
				}
				else{//	hoveredSB == null
					selectedSB = null;
					selectedSG = hoveredSG;
					sgm.SimSBHover(hoveredSB, eventData);
					sgm.SimSGHover(hoveredSG, eventData);
					transaction = sgm.Transaction;
					// transaction = new RevertTransaction(pickedSB);
					// if(hoveredSG == null){
						
					// 	/*	revert
					// 	*/
					// }else if(hoveredSG == origSG){
						
					// }else{
						
					// 	if(hoveredSG == origSG){// same sg, no selectable sb under cursor
							

					// 		/*	revert
					// 		*/
					// 	}else{//	hoveredSG not null nor the same as orig

							
					// 	}
					// }
					/*	for reverting
					*/
					sgm.SimSBHover(pickedSB, eventData);
					sgm.SimSGHover(origSG, eventData);
				}
				bool reverted;
				Revert(pickedSB, out reverted);
				AB(reverted, true);
				AssertSGMFocused();
				
			}else{//	pickedSB is not in a state to be picked up
				picked = false;
				selectedSB = null;
				selectedSG = null;
				transaction = null;

				if(pickedSB.CurState == Slottable.DefocusedState || pickedSB.CurState == Slottable.EquippedAndDefocusedState){
					pickedSB.OnPointerDownMock(eventData);
					ASSB(pickedSB, Slottable.WaitForPointerUpState);
					pickedSB.OnPointerUpMock(eventData);
					if(pickedSB.IsEquipped)
						ASSB(pickedSB, Slottable.EquippedAndDefocusedState);
					else
						ASSB(pickedSB, Slottable.DefocusedState);
				}else{
					/*	not testable
					*/
					SlottableState preState = pickedSB.CurState;
					pickedSB.OnPointerDownMock(eventData);
					ASSB(pickedSB, preState);
				}
				AssertSGMFocused();
			}
		}
		/*	hovering
		*/
			public void TestPickupAndRevertAll(){
				int hoveredCount = 0;
				TestHoverAll(ref hoveredCount);
				AE(hoveredCount, 6);

				hoveredCount = 0;
				sgm.SetFocusedPoolSG(sgpParts);
				TestHoverAll(ref hoveredCount);
				AE(hoveredCount, 4);

				AB(HoverTestPassed(defBowA_p), false);
				AB(HoverTestPassed(defBowB_p), false);
				AB(HoverTestPassed(crfBowA_p), false);
				AB(HoverTestPassed(defWearA_p), false);
				AB(HoverTestPassed(defWearB_p), false);
				AB(HoverTestPassed(crfWearA_p), false);
				AB(HoverTestPassed(defParts_p), false);
				AB(HoverTestPassed(crfParts_p), false);
				AB(HoverTestPassed(defBowA_e), true);
				AB(HoverTestPassed(defWearA_e), true);
				AB(HoverTestPassed(defParts_p2), true);
				AB(HoverTestPassed(crfParts_p2), true);
				
				hoveredCount = 0;
				sgm.SetFocusedPoolSG(sgpAll);
				TestHoverAll(ref hoveredCount);
				AE(hoveredCount, 6);
				
				AB(HoverTestPassed(defBowA_p), false);
				AB(HoverTestPassed(defBowB_p), true);
				AB(HoverTestPassed(crfBowA_p), true);
				AB(HoverTestPassed(defWearA_p), false);
				AB(HoverTestPassed(defWearB_p), true);
				AB(HoverTestPassed(crfWearA_p), true);
				AB(HoverTestPassed(defParts_p), false);
				AB(HoverTestPassed(crfParts_p), false);
				AB(HoverTestPassed(defBowA_e), true);
				AB(HoverTestPassed(defWearA_e), true);
				AB(HoverTestPassed(defParts_p2), false);
				AB(HoverTestPassed(crfParts_p2), false);
			}
			public bool HoverTestPassed(Slottable sb){
				bool result = false;
				TestHoverSequence(sb, out result);
				return result;
			}
			public void TestHoverAll(ref int count){
				
				foreach(Slottable sb in SlottableList()){
					bool hovered = false;
					TestHoverSequence(sb, out hovered);
					if(hovered)
						count ++;
				}

			}
			public void TestHoverSequence(Slottable sb, out bool hovered){
				bool pickedUp = false;
				bool reverted = false;
				PickUp(sb, out pickedUp);
				// TestHover(sb, ref pickedUp);
				Revert(sb, out reverted);
				hovered = pickedUp && reverted;
			}
			// public void TestHover(Slottable sb, ref bool picked){
				// 	if(picked){
				// 		ASSB(sb, Slottable.PickedUpAndSelectedState);
						
				// 		// sb.OnEndDragMock(eventData);
						
				// 		ASSB(sb, Slottable.PickedUpAndSelectedState);
				// 	}
				// }
			public void PickUp(Slottable sb, out bool pickedUp){	
				// AssertSGMFocused();
				if(sb.CurState == Slottable.FocusedState || sb.CurState == Slottable.EquippedAndDeselectedState){
					
					sb.OnPointerDownMock(eventData);
					ASSB(sb, Slottable.WaitForPickUpState);
					sb.CurProcess.Expire();
					pickedUp = true;
					ASSB(sb, Slottable.PickedAndSelectedState);
					// AssertPostPickFilter(sb);
				}else{
					pickedUp = false;
				}
			}
			public void Revert(Slottable sb, out bool reverted){
				if(sb.CurState == Slottable.PickedAndSelectedState || sb.CurState == Slottable.PickedAndDeselectedState){
					sb.OnPointerUpMock(eventData);
					if(sb.Item.IsStackable){
						ASSB(sb, Slottable.WaitForNextTouchWhilePUState);
						sb.CurProcess.Expire();
					}
						AssertReverting(sb);
					sb.CurProcess.Expire();
					reverted = true;
				}else{
					reverted = false;
				}
				// AssertSGMFocused();
			}
	
		
	/*	supportives
	*/
		List<Slottable> SlottableList(){
			List<Slottable> sbList = new List<Slottable>();
			sbList.Add(defBowA_p);
			sbList.Add(defBowB_p);
			sbList.Add(crfBowA_p);
			sbList.Add(defWearA_p);
			sbList.Add(defWearB_p);
			sbList.Add(crfWearA_p);
			sbList.Add(defParts_p);
			sbList.Add(crfParts_p);
			sbList.Add(defBowA_e);
			sbList.Add(defWearA_e);
			sbList.Add(defParts_p2);
			sbList.Add(crfParts_p2);
			sbList.Add(defShieldA_p);
			sbList.Add(crfShieldA_p);
			sbList.Add(defMWeaponA_p);
			sbList.Add(crfMWeaponA_p);
			sbList.Add(defShieldA_e);
			sbList.Add(defMWeaponA_e);
			sbList.Add(defQuiverA_p);
			sbList.Add(defPackA_p);
			return sbList;
		}
		public void AssertTransactionFieldsAreCleared(Slottable sb){
			AE(sgm.CurState, SlotGroupManager.FocusedState);
			AE(sgm.CurProcess, null);
			AE(sgm.PickedSB, null);
			AE(sgm.SelectedSB, null);
			AE(sgm.SelectedSG, null);

			AB(sgm.PickedSBDoneTransaction, true);
			AB(sgm.SelectedSBDoneTransaction, true);
			AB(sgm.OrigSGDoneTransaction, true);
			AB(sgm.SelectedSGDoneTransaction, true);

			AE(sb.DestinationSG, null);
			AE(sb.DestinationSlot, null);
			
		}
		public void AssertPostPickFilter(Slottable sb){
			ASSB(sb, Slottable.PickedAndSelectedState);
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
									ASSB(slot.Sb, Slottable.PickedAndSelectedState);
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
				ASSG(sgpAll, SlotGroup.FocusedState);
					AB(defBowA_p.IsEquipped, true);
					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowB_p, Slottable.FocusedState);
					ASSB(crfBowA_p, Slottable.FocusedState);
					AB(defWearA_p.IsEquipped, true);
					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearB_p, Slottable.FocusedState);
					ASSB(crfWearA_p, Slottable.FocusedState);
					ASSB(defParts_p, Slottable.DefocusedState);
					ASSB(crfParts_p, Slottable.DefocusedState);
					ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfShieldA_p, Slottable.FocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfMWeaponA_p, Slottable.FocusedState);
					
				ASSG(sgpParts, SlotGroup.DefocusedState);
					ASSB(defParts_p2, Slottable.DefocusedState);
					ASSB(crfParts_p2, Slottable.DefocusedState);
			}else{
				AE(sgm.RootPage.PoolBundle.GetFocusedBundleElement(), sgpParts);
				ASSG(sgpAll, SlotGroup.DefocusedState);
					AB(defBowA_p.IsEquipped, true);
					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowB_p, Slottable.DefocusedState);
					ASSB(crfBowA_p, Slottable.DefocusedState);
					AB(defWearA_p.IsEquipped, true);
					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearB_p, Slottable.DefocusedState);
					ASSB(crfWearA_p, Slottable.DefocusedState);
					ASSB(defParts_p, Slottable.DefocusedState);
					ASSB(crfParts_p, Slottable.DefocusedState);
					ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfShieldA_p, Slottable.DefocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfMWeaponA_p, Slottable.DefocusedState);
				ASSG(sgpParts, SlotGroup.FocusedState);
					ASSB(defParts_p2, Slottable.FocusedState);
					ASSB(crfParts_p2, Slottable.FocusedState);
			}
			ASSG(sgBow, SlotGroup.FocusedState);
				AB(defBowA_e.IsEquipped, true);
				ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
			ASSG(sgWear, SlotGroup.FocusedState);
				AB(defWearA_e.IsEquipped, true);
				ASSB(defWearA_e, Slottable.EquippedAndDeselectedState);
			ASSG(sgCGears, SlotGroup.FocusedState);
				AB(defShieldA_e.IsEquipped, true);
				ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
				AB(defMWeaponA_e.IsEquipped, true);
				ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
		}
		public void AssertFocusedGeneric(){
			AE(sgm.CurState, SlotGroupManager.FocusedState);
				AE(sgm.CurProcess == null, true);
				AE(sgm.Transaction == null, true);
			foreach(SlotSystemElement ele in sgm.RootPage.PoolBundle.Elements){
				SlotGroup sg = (SlotGroup)ele;
				if(sg == sgm.GetFocusedPoolSG()){
					ASSG(sg, SlotGroup.FocusedState);
					foreach(Slot slot in sg.Slots){
						if(slot.Sb != null){
							if(slot.Sb.IsEquipped)
								ASSB(slot.Sb, Slottable.EquippedAndDefocusedState);
							else{
								if(slot.Sb.Item is PartsInstanceMock)
									ASSB(slot.Sb, Slottable.DefocusedState);
								else
									ASSB(slot.Sb, Slottable.FocusedState);
							}
						}
					}
				}else{
					ASSG(sg, SlotGroup.DefocusedState);
					foreach(Slot slot in sg.Slots){
						if(slot.Sb != null){
							if(slot.Sb.IsEquipped)
								ASSB(slot.Sb, Slottable.EquippedAndDefocusedState);
							else
								ASSB(slot.Sb, Slottable.DefocusedState);
						}
					}
				}
			}
			foreach(SlotSystemElement ele in sgm.RootPage.EquipBundle.Elements){
				EquipmentSet equipSet = (EquipmentSet)ele;
				if(equipSet == sgm.GetFocusedEquipSet()){
					foreach(SlotSystemElement sgEle in equipSet.Elements){
						SlotGroup sg = (SlotGroup)sgEle;
						ASSG(sg, SlotGroup.FocusedState);
						foreach(Slot slot in sg.Slots){
							if(slot.Sb != null){
								if(slot.Sb.IsEquipped)
									ASSB(slot.Sb, Slottable.EquippedAndDeselectedState);
								else
									ASSB(slot.Sb, Slottable.DefocusedState);
							}
						}
					}
				}else{
					foreach(SlotSystemElement sgEle in equipSet.Elements){
						SlotGroup sg = (SlotGroup)sgEle;
						ASSG(sg, SlotGroup.DefocusedState);
						foreach(Slot slot in sg.Slots){
							if(slot.Sb != null){
								if(slot.Sb.IsEquipped)
									ASSB(slot.Sb, Slottable.EquippedAndDefocusedState);
								else
									ASSB(slot.Sb, Slottable.DefocusedState);
							}
						}
					}
				}
			}
		}
		public void AssertSGMDefocus(){
			AE(sgm.CurState, SlotGroupManager.DefocusedState);

			AE(sgpAll.CurState, SlotGroup.DefocusedState);
				ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
				ASSB(defBowB_p, Slottable.DefocusedState);
				ASSB(crfBowA_p, Slottable.DefocusedState);
				ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
				ASSB(defWearB_p, Slottable.DefocusedState);
				ASSB(crfWearA_p, Slottable.DefocusedState);

				AB(((InventoryItemInstanceMock)defParts_p.Item).IsEquipped, false);
				AE(sgm.GetSlotGroup(defParts_p), sgpAll);

				ASSB(defParts_p, Slottable.DefocusedState);
				ASSB(crfParts_p, Slottable.DefocusedState);
			AE(sgpParts.CurState, SlotGroup.DefocusedState);
				ASSB(defParts_p2, Slottable.DefocusedState);
				ASSB(crfParts_p2, Slottable.DefocusedState);
			AE(sgBow.CurState, SlotGroup.DefocusedState);
				ASSB(defBowA_e, Slottable.EquippedAndDefocusedState);
			AE(sgWear.CurState, SlotGroup.DefocusedState);
				ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);

		}
		public void AssertTransactionFieldsAreClearedOnAllSB(){
			foreach(Slottable sb in SlottableList()){
				AssertTransactionFieldsAreCleared(sb);
			}
		}
	/*	states test
	*/
		// [Test]
		public void TestOnPointerDownOnAllSB(){
			foreach(Slottable sb in SlottableList()){
				TestOnPointerDownSequence(sb);
			}
		}
		public void TestOnPointerDownSequence(Slottable sb){
			sgm.RootPage.PoolBundle.SetFocusedBundleElement(sgpParts);
			sgm.Focus();
			AssertSGMFocused();
			TestDeactivatedState(sb);
			TestWaitForPointerUpState(sb);
			TestFocusedState(sb);
			TestEqDeselectedState(sb);
			TestWaitForPickUpState(sb);
			TestPickedUpAndSelectedState(sb);
			TestWaitForNextTouchWhilePUState(sb);
			TestDefocusedStates(sb);
			TestWaitForNextTouchState(sb);

			sgm.RootPage.PoolBundle.SetFocusedBundleElement(sgpAll);		
			sgm.Focus();
			AssertSGMFocused();
			TestDeactivatedState(sb);
			TestWaitForPointerUpState(sb);
			TestFocusedState(sb);
			TestEqDeselectedState(sb);
			TestWaitForPickUpState(sb);
			TestPickedUpAndSelectedState(sb);
			TestWaitForNextTouchWhilePUState(sb);
			TestDefocusedStates(sb);
			TestWaitForNextTouchState(sb);
		}
		public void TestDeactivatedState(Slottable sb){
			
			sgm.Deactivate();

			AE(sgm.CurState, SlotGroupManager.DeactivatedState);
			AB(sgm.CurProcess == null, true);
			ASSG(sgm.GetSlotGroup(sb), SlotGroup.DeactivatedState);
			AB(sgm.GetSlotGroup(sb).CurProcess == null, true);
			ASSB(sb, Slottable.DeactivatedState);
			AB(sb.CurProcess == null, true);


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
				
				sb.OnPointerUpMock(eventData);
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
				AE(sb.CurProcess.GetType(), typeof(SBGreyinProcess));
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
					// AB(sb.Tapped, false);
					sb.CurProcess.Expire();
					// AB(sb.Tapped, true);
					// sb.Tapped = false;
					ASSB(sb, Slottable.FocusedState);
				}else{
					ASSB(sb, Slottable.FocusedState);
					// AB(sb.Tapped, true);
					// sb.Tapped = false;
				}
			}
		}
		public void TestEqDeselectedState(Slottable sb){
			sgm.Deactivate();
			sgm.Focus();
			if(sb.CurState == Slottable.EquippedAndDeselectedState){
				AB(sb.CurProcess == null, true);
			}
			sgm.Defocus();
			sgm.Focus();
			if(sb.CurState == Slottable.EquippedAndDeselectedState){
				AE(sb.CurProcess.GetType(), typeof(SBGreyinProcess));
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
					ASSB(sb, Slottable.EquippedAndDeselectedState);
				}else{
					ASSB(sb, Slottable.EquippedAndDeselectedState);
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
					ASSB(sb, Slottable.PickedAndSelectedState);
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
					sb.CurProcess.Expire();

				}else{
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
				AE(sgm.PickedSB, sb);
				
				AB(sgm.PickedSBDoneTransaction, false);
				// AB(sgm.SelectedSBDoneTransaction, true);
				// AB(sgm.OrigSGDoneTransaction, true);
				// AB(sgm.SelectedSGDoneTransaction, true);
				
				AE(sb.DestinationSG, sgm.GetSlotGroup(sb));
				// AB(sb.DestinationSlot != null, true);
				AE(sb.DestinationSlot, sb.DestinationSG.GetSlot(sb));
			}
		public void TestPickedUpAndSelectedState(Slottable sb){
			if(sb.CurState == Slottable.FocusedState || sb.CurState == Slottable.EquippedAndDeselectedState){
				sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPickUpState);
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				
				sb.CurProcess.Expire();
					ASSB(sb, Slottable.PickedAndSelectedState);
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
					ASSB(sb, Slottable.PickedAndSelectedState);
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
					ASSB(sb, Slottable.PickedAndSelectedState);
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
		public void TestDefocusedStates(Slottable sb){
			sgm.Defocus();
			if(sb.IsEquipped)
				ASSB(sb, Slottable.EquippedAndDefocusedState);
			else
				ASSB(sb, Slottable.DefocusedState);
			
			sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPointerUpState);
			sb.OnPointerUpMock(eventData);
				AssertSGMDefocus();
				
			sgm.Focus();
		}
		public void TestWaitForNextTouchState(Slottable sb){
			if(sb.CurState == Slottable.FocusedState || sb.CurState == Slottable.EquippedAndDeselectedState){

				sb.OnPointerDownMock(eventData);
				sb.OnPointerUpMock(eventData);

				if(sb.Item.IsStackable){
					ASSB(sb, Slottable.WaitForNextTouchState);
					/*	1.0 Expire
					*/
					sb.CurProcess.Expire();
						
							ASSB(sb, Slottable.FocusedState);
					/*	1.1 OnPointerDown
					*/
					sb.OnPointerDownMock(eventData);
					sb.OnPointerUpMock(eventData);
					
					sb.OnPointerDownMock(eventData);
						AE(sb.PrevState, Slottable.WaitForNextTouchState);
						ASSB(sb, Slottable.PickedAndSelectedState);
						AE(sb.PickedAmount, 1);
						AE(sb.CurProcess.GetType(), typeof(SBPickUpProcess));
						AB(sb.CurProcess.IsRunning, true);
						AB(sb.CurProcess.IsExpired, false);
						AssertPostPickFilter(sb);
					
					sb.OnPointerUpMock(eventData);
						ASSB(sb, Slottable.WaitForNextTouchWhilePUState);
					sb.CurProcess.Expire();
						ASSB(sb, Slottable.MovingState);
					sb.CurProcess.Expire();
					/*	1.2 OnDeselected
					*/
					sb.OnPointerDownMock(eventData);
					sb.OnPointerUpMock(eventData);
					
					sb.OnDeselectedMock(eventData);
						AssertTransactionFieldsAreCleared(sb);
						if(sb == defBowA_p || sb == defBowA_e || sb == defWearA_p || sb == defWearA_e)
							AB(sb.IsEquipped, true);
						// if(sb.IsEquipped)
						// 	ASSB(sb, Slottable.MovingState);
						// else
							ASSB(sb, Slottable.FocusedState);
				}else{//	non stackable
					
				}
			}
			AssertSGMFocused();
		}
		
	/*
	*/
	/*	Assertions
	*/
		public void AssertState<T>() where T: SlottableState{
			Assert.That(defBowA_p.CurState.GetType(), Is.EqualTo(typeof(T)));
		}
		public void AssertState(Slottable sb, SlottableState sbState){
			Assert.That(sb.CurState, Is.EqualTo(sbState));
		}
		public void AssertPickQuantity(int quant){
			Assert.That(defBowA_p.PickedAmount, Is.EqualTo(quant));
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
		public void AM(Slottable sb, SlotGroup sg, bool isMember){
			if(sg.Filter is SGBowFilter){
				if((InventoryItemInstanceMock)sg.Slots[0].Sb.Item == (InventoryItemInstanceMock)sb.Item){
					AB(isMember, true);
					return;
				}
			}
			else if(sg.Filter is SGWearFilter){
				if((InventoryItemInstanceMock)sg.Slots[0].Sb.Item == (InventoryItemInstanceMock)sb.Item){
					AB(isMember, true);
					return;
				}
			}
			else if(sg.Filter is SGCarriedGearFilter){
				foreach(Slot slot in sg.Slots){
					if(slot.Sb != null){
						if((InventoryItemInstanceMock)slot.Sb.Item == (InventoryItemInstanceMock)sb.Item){
							AB(isMember, true);
							return;
						}
					}
				}
			}
			AB(isMember, false);
		}
		public void AssertEquippedBow(Slottable equippedInPool, Slottable prevInPool){
			AE(sgm.CurState, SlotGroupManager.FocusedState);
			AB(sgm.CurProcess == null, true);
			AB(sgm.Transaction == null, true);
			AE(sgm.PickedSB, null);
			AE(sgm.SelectedSG, null);
			AE(sgm.SelectedSB, null);

			ASSG(sgpAll, SlotGroup.FocusedState);
			ASSG(sgBow, SlotGroup.FocusedState);

			ASSB(equippedInPool, Slottable.EquippedAndDefocusedState);
			AB(equippedInPool.IsEquipped, true);

			ASSB(prevInPool, Slottable.FocusedState);
			AB(prevInPool.IsEquipped, false);

			Slottable equippedInEq = sgBow.Slots[0].Sb;
			AE(equippedInEq.Item, equippedInPool.Item);
			ASSB(equippedInEq, Slottable.EquippedAndDeselectedState);
			AE(sgpAll.Slots.Count, 14);
			AE(sgBow.Slots.Count, 1);
		}
		public void AssertEquippedWear(Slottable equippedInPool, Slottable prevInPool){
			AE(sgm.CurState, SlotGroupManager.FocusedState);
			AB(sgm.CurProcess == null, true);
			AB(sgm.Transaction == null, true);
			AE(sgm.PickedSB, null);
			AE(sgm.SelectedSG, null);
			AE(sgm.SelectedSB, null);

			ASSG(sgpAll, SlotGroup.FocusedState);
			ASSG(sgWear, SlotGroup.FocusedState);

			ASSB(equippedInPool, Slottable.EquippedAndDefocusedState);
			AB(equippedInPool.IsEquipped, true);

			ASSB(prevInPool, Slottable.FocusedState);
			AB(prevInPool.IsEquipped, false);

			Slottable equippedInEq = sgWear.Slots[0].Sb;
			AE(equippedInEq.Item, equippedInPool.Item);
			ASSB(equippedInEq, Slottable.EquippedAndDeselectedState);
			AE(sgpAll.Slots.Count, 14);
			AE(sgWear.Slots.Count, 1);
		}
		public void PrintEquipped(){
			string bowName = null;
			switch(sgm.GetEquippedBow().Item.ItemID){
				case 0:
					bowName = "defBow";
					break;
				case 1:
					bowName = "crfBow";
					break;
			}
			string wearName = null;
			switch(sgm.GetEquippedWear().Item.ItemID){
				case 100:
					wearName = "defWear";
					break;
				case 101:
					wearName = "crfWear";
					break;
			}
			List<string> cgNamesList = new List<string>();
			foreach(CarriedGearInstanceMock cgInst in sgm.GetEquippedCarriedGears()){
				switch(cgInst.Item.ItemID){
					case 200:
						cgNamesList.Add("defShield");
						break;
					case 201:
						cgNamesList.Add("crfShield");
						break;
					case 300:
						cgNamesList.Add("defMWeapon");
						break;
					case 301:
						cgNamesList.Add("crfMWeapon");
						break;
					case 400:
						cgNamesList.Add("defQuiver");
						break;
					case 401:
						cgNamesList.Add("crfQuiver");
						break;
					case 500:
						cgNamesList.Add("defPack");
						break;
					case 501:
						cgNamesList.Add("crfPack");
						break;
				}
			}
			string cGearsNames = "";
			foreach(string name in cgNamesList){
				cGearsNames += name + ", ";
			}
			Debug.Log("<b>Bow</b>: " + bowName + "<b> Wear: </b>" + wearName + "<b> CGears: </b>" + cGearsNames);
		}
		public void AssertEquipped(Slottable sbInPool){
			if(sbInPool.Item is BowInstanceMock){
				foreach(Slot slot in sgpAll.Slots){
					if(slot.Sb != null){
						if(slot.Sb.Item is BowInstanceMock){
							if(slot.Sb == sbInPool){
								AB(slot.Sb.IsEquipped, true);
								AE(sgm.GetEquippedBow(), slot.Sb.Item);
								AB(sgBow.GetSlottable(slot.Sb.Item) != null, true);
							}else{
								AB(slot.Sb.IsEquipped, false);
								AB(sgm.GetEquippedBow() != (BowInstanceMock)slot.Sb.Item, true);
								AB(sgBow.GetSlottable(slot.Sb.Item) == null, true);
							}
						}
					}
				}
			}else if(sbInPool.Item is WearInstanceMock){
				foreach(Slot slot in sgpAll.Slots){
					if(slot.Sb != null){
						if(slot.Sb.Item is WearInstanceMock){
							if(slot.Sb == sbInPool){
								AB(slot.Sb.IsEquipped, true);
								AE(sgm.GetEquippedWear(), slot.Sb.Item);
								AB(sgWear.GetSlottable(slot.Sb.Item) != null, true);
							}else{
								AB(slot.Sb.IsEquipped, false);
								AB(sgm.GetEquippedWear() != (WearInstanceMock)slot.Sb.Item, true);
								AB(sgWear.GetSlottable(slot.Sb.Item) == null, true);
							}
						}
					}
				}
			}
		} 
		public void AssertEquippedCGears(Slottable cg1, Slottable cg2, Slottable cg3, Slottable cg4){
			List<CarriedGearInstanceMock> checkedList = new List<CarriedGearInstanceMock>();
			if(cg1 != null)
			checkedList.Add((CarriedGearInstanceMock)cg1.Item);
			if(cg2 != null)
			checkedList.Add((CarriedGearInstanceMock)cg2.Item);
			if(cg3 != null)
			checkedList.Add((CarriedGearInstanceMock)cg3.Item);
			if(cg4 != null)
			checkedList.Add((CarriedGearInstanceMock)cg4.Item);
			foreach(Slot slot in sgpAll.Slots){
				if(slot.Sb != null){
					if(slot.Sb.Item is CarriedGearInstanceMock){
						bool found = false;
						foreach(CarriedGearInstanceMock checkedCG in checkedList){
							if((CarriedGearInstanceMock)slot.Sb.Item == checkedCG)
								found = true;
						}
						if(found){
							AB(slot.Sb.IsEquipped, true);
							AB(sgCGears.GetSlottable(slot.Sb.Item) != null, true);
						}else{
							AB(slot.Sb.IsEquipped, false);
							AB(sgCGears.GetSlottable(slot.Sb.Item) == null, true);
						}
					}
				}
			} 
		}
}
