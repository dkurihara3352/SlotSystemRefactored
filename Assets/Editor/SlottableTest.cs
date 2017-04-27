using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;

public class SlottableTest {


	GameObject sbGO;

	PointerEventDataMock eventDataMock = new PointerEventDataMock();
	GameObject sgmGO;
	SlotGroupManager sgm;
	GameObject sgpAllGO;
	SlotGroup sgpAll;
	GameObject sgBowGO;
	SlotGroup sgBow;
	GameObject sgWearGO;
	SlotGroup sgWear;
	Slottable defBowBSB_p;
	Slottable defBowASB_p;
	Slottable defBowASB_e;
	Slottable defWearASB_e;
	Slottable defWearBSB_p;
	Slottable defWearASB_p;

	[SetUp]
	public void Setup(){
		// InstantiateAndInitialize();
		// CreateAndSetSlottableItem();
		// sb.Delayed = true;
		// sb.FilteredInMock();//SetState(Slottable.FocusedState)


		sgmGO = new GameObject("SlotGroupManager");
		sgm = sgmGO.AddComponent<SlotGroupManager>();
		SGMCommand updateTSCommand = new UpdateTransactionCommand();
		sgm.SetUpdateTransactionCommand(updateTSCommand);
		SGMCommand postPickFilterCommand = new PostPickFilterCommand();
		sgm.SetPostPickFilterCommand(postPickFilterCommand);
		/*	sgpAll
		*/
			sgpAllGO = new GameObject("PoolSlotGroup");
			sgpAll = sgpAllGO.AddComponent<SlotGroup>();
			sgm.SetSG(sgpAll);
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
			sgm.SetSG(sgBow);
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
			sgm.SetSG(sgWear);
			sgWear.Filter = new SGWearFilter();
			sgWear.Sorter = new SGItemIndexSorter();
			sgWear.UpdateEquipStatusCommand = new UpdateEquipStatusForEquipSGCommand();
			sgWear.SetInventory(equipInventory);
			sgWear.IsShrinkable = false;
			sgWear.IsExpandable = false;

			Slot wearSlot = new Slot();
			wearSlot.Position = Vector2.zero;
			sgWear.Slots.Add(wearSlot);

			
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
			PartsMock crfParts = new PartsMock();
			crfParts.ItemID = 602;
			
			PartsInstanceMock defPartsA = new PartsInstanceMock();
			defPartsA.Item = defParts;
			sgpAll.Inventory.Add(defPartsA);
			PartsInstanceMock defPartsB = new PartsInstanceMock();
			defPartsB.Item = defParts;
			sgpAll.Inventory.Add(defPartsB);
			PartsInstanceMock crfPartsA = new PartsInstanceMock();
			crfPartsA.Item = crfParts;
			sgpAll.Inventory.Add(crfPartsA);

		sgm.InitiallyFocusedSG = sgpAll;
		
				Assert.That(sgm.CurState, Is.EqualTo(SlotGroupManager.DeactivatedState));
				Assert.That(sgm.PrevState, Is.EqualTo(SlotGroupManager.DeactivatedState));
				Assert.That(sgm.SlotGroups.Count, Is.EqualTo(3));
		sgm.Initialize();
		defBowASB_p = sgpAll.GetSlottable(defBowA);		
		sbGO = defBowASB_p.gameObject;
			Assert.That(defBowASB_p.SGM.CurState, Is.EqualTo(SlotGroupManager.DefocusedState));
			Assert.That(defBowASB_p.SGM.PrevState, Is.EqualTo(SlotGroupManager.DeactivatedState));
		sgm.Focus();

		Assert.That(defBowASB_p.SGM, Is.EqualTo(sgm));
		Assert.That(sgpAll.SGM, Is.EqualTo(sgm));
		Assert.That(defBowASB_p.SGM.GetSlotGroup(defBowASB_p), Is.EqualTo(sgpAll));

		Assert.That(defBowASB_p.SGM.CurState, Is.EqualTo(SlotGroupManager.FocusedState));
		Assert.That(defBowASB_p.SGM.PrevState, Is.EqualTo(SlotGroupManager.DefocusedState));

		/*	Assertions
		*/
			Assert.That(sgm.SlotGroups.Count, Is.EqualTo(3));
			Assert.That(sgpAll.CurState, Is.EqualTo(SlotGroup.FocusedState));
			Assert.That(sgBow.CurState, Is.EqualTo(SlotGroup.FocusedState));
			Assert.That(sgWear.CurState, Is.EqualTo(SlotGroup.FocusedState));

			Assert.That(sgBow.Slots.Count, Is.EqualTo(1));
				Assert.That(sgBow.GetSlottable(defBowA).CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
			Assert.That(sgWear.Slots.Count, Is.EqualTo(1));
				Assert.That(sgWear.GetSlottable(defWearA).CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
			Assert.That(sgpAll.Slots.Count, Is.EqualTo(9));
				Assert.That(sgpAll.GetSlottable(defBowA).CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
				Assert.That(sgpAll.GetSlottable(defBowB).CurState, Is.EqualTo(Slottable.FocusedState));
				Assert.That(sgpAll.GetSlottable(crfBowA).CurState, Is.EqualTo(Slottable.FocusedState));
				Assert.That(sgpAll.GetSlottable(defWearA).CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
				Assert.That(sgpAll.GetSlottable(defWearB).CurState, Is.EqualTo(Slottable.FocusedState));
				Assert.That(sgpAll.GetSlottable(crfWearA).CurState, Is.EqualTo(Slottable.FocusedState));
				Assert.That(sgpAll.GetSlottable(defPartsA).CurState, Is.EqualTo(Slottable.DefocusedState));
				Assert.That(sgpAll.GetSlottable(defPartsB).CurState, Is.EqualTo(Slottable.DefocusedState));
				Assert.That(sgpAll.GetSlottable(crfPartsA).CurState, Is.EqualTo(Slottable.DefocusedState));
		/*
		*/
			defBowBSB_p = sgpAll.GetSlottable(defBowB);
			defBowASB_e = sgBow.GetSlottable(defBowA);
			defWearASB_e = sgWear.GetSlottable(defWearA);
			defWearBSB_p = sgpAll.GetSlottable(defWearB);
			defWearASB_p = sgpAll.GetSlottable(defWearA);
	}
	/*try this again after everything else*/
	public void TestSBSelectedState(){
		Assert.That(Slottable.SelectedState.GetType(), Is.EqualTo(typeof(SBSelectedState)));
		defBowASB_p.SetState(Slottable.SelectedState);
		AssertState(defBowASB_p, Slottable.SelectedState);
	}
	public void PickUpAndValidate(Slottable sb){

		sb.OnPointerDownMock(eventDataMock);
			
			AssertState(sb, Slottable.WaitForPickUpState);
			Assert.That(sb.WaitAndPickUpProcess.IsRunning, Is.True);
			Assert.That(sb.WaitAndPickUpProcess.IsExpired, Is.False);
		
		sb.WaitAndPickUpProcess.Expire();
		
			AssertAction(sb, "WaitAndPickUpProcess done");
			Assert.That(sb.WaitAndPickUpProcess.IsRunning, Is.False);
			Assert.That(sb.WaitAndPickUpProcess.IsExpired, Is.True);
		
			AssertState(sb, Slottable.PickedUpAndSelectedState);
			Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.True);
			Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.False);

		// sb.SGM.SimSBHover(sb, eventDataMock);
		// sb.SGM.SimSGHover(sb.SGM.GetSlotGroup(sb), eventDataMock);
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
	public void ValidateNonPickable(Slottable sb){
		sb.OnPointerDownMock(eventDataMock);
		Assert.That(sb.CurState, Is.Not.EqualTo(Slottable.WaitForPickUpState));
	}
	[Test]
	public void Test(){
		// PickUpAndValidate(defBowBSB_p);
		// ValidateStates(defBowBSB_p);

		// PickUpAndValidate(defBowASB_e);
		// ValidateStates(defBowASB_e);
		
		// PickUpAndValidate(defWearASB_e);
		// ValidateStates(defWearASB_e);
		
		PickUpAndValidate(defWearBSB_p);
		ValidateStates(defWearBSB_p);
		
		// ValidateNonPickable(defBowASB_p);
		// ValidateNonPickable(defWearASB_p);
	}
	public void ValidateStates(Slottable pickedSB){
		/*	Revise the post pick filter so that picking up the Equipped bow in the equipBowSG does not results anything other than bows to not get focused in the pool all sg
		*/
		if(sgm.GetSlotGroup(pickedSB).Filter is SGNullFilter){

		}else if(sgm.GetSlotGroup(pickedSB).Filter is SGBowFilter){
			Assert.That(pickedSB.Item, Is.TypeOf(typeof(BowInstanceMock)));
			Assert.That(sgBow.CurState, Is.EqualTo(SlotGroup.SelectedState));
			foreach(Slot slot in sgBow.Slots){
				if(slot.Sb != null){
					Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.PickedUpAndSelectedState));
				}
			}
			Assert.That(sgWear.CurState, Is.EqualTo(SlotGroup.DefocusedState));
			foreach(Slot slot in sgWear.Slots){
				if(slot.Sb != null){
					Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDefocusedState));
				}
			}
			Assert.That(sgpAll.CurState, Is.EqualTo(SlotGroup.FocusedState));
			foreach(Slot slot in sgpAll.Slots){
				if(slot.Sb != null){
					if(slot.Sb.Item is PartsInstanceMock)
						Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));
					else if(object.ReferenceEquals(slot.Sb.Item, sgBow.Slots[0].Sb.Item))
						Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDefocusedState));//needs to be defocused...
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
			
		}else if(sgm.GetSlotGroup(pickedSB).Filter is SGWearFilter){
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
			
		}else if(sgm.GetSlotGroup(pickedSB).Filter is SGPartsFilter){

		}
	}
	// [Test]
	public void TestPickedUpAndSelectedState(){
		/* pick up
		*/
			PickUpAndValidate(defBowBSB_p);
	
		/*	post pick filtering
			after this is done then test hovering on defocused entities to validate SGM does not update its Selected fields
		*/
			Assert.That(defBowBSB_p.SGM.PickedSB.Item, Is.TypeOf(typeof(BowInstanceMock)));
			Assert.That(sgWear.Filter, Is.TypeOf(typeof(SGWearFilter)));
			Assert.That(sgWear.CurState, Is.EqualTo(SlotGroup.DefocusedState));
			foreach(Slot slot in sgWear.Slots){
				if(slot.Sb != null){
					Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDefocusedState));
				}
			}
			Assert.That(sgBow.CurState, Is.EqualTo(SlotGroup.FocusedState));
			foreach(Slot slot in sgBow.Slots){
				if(slot.Sb != null){
					Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
				}
			}
			Assert.That(sgpAll.CurState, Is.EqualTo(SlotGroup.SelectedState));
			foreach(Slot slot in sgpAll.Slots){
				if(slot.Sb != null){
					if(slot.Sb.Item is PartsInstanceMock)
						Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));
					else if(object.ReferenceEquals(slot.Sb.Item, sgBow.Slots[0].Sb.Item))
						Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
					else if(object.ReferenceEquals(slot.Sb.Item, sgWear.Slots[0].Sb.Item))
						Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
					else if(slot.Sb == sgm.PickedSB)
						Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.PickedUpAndSelectedState));
					else
						Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.FocusedState));
				}
			}
		/*	checkTransaction
		*/
		/*	hover and dehover
		*/
		/*	expire
		*/
		// sb.PickedUpAndSelectedProcess.Expire();
		// AssertAction("PickedUpAndSelectedProcess done");
		// Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.False);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.True);
		// AssertState(sb, Slottable.PickedUpAndSelectedState);
		// /*	abort
		// */
		// sb.SetState(Slottable.FocusedState);
		// AssertState(sb, Slottable.FocusedState);
		// sb.OnPointerDownMock(eventDataMock);
		// AssertState(sb, Slottable.WaitForPickUpState);
		// sb.WaitAndPickUpProcess.Expire();
		// AssertAction("WaitAndPickUpProcess done");
		// Assert.That(sb.WaitAndPickUpProcess.IsRunning, Is.False);
		// Assert.That(sb.WaitAndPickUpProcess.IsExpired, Is.True);
		// AssertState(sb, Slottable.PickedUpAndSelectedState);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.True);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.False);

		// sb.SetState(Slottable.FocusedState);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.False);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.False);
		/*	OnDehovered
		*/
		//setting up
			defBowASB_p.SetState(Slottable.FocusedState);
			AssertState(defBowASB_p, Slottable.FocusedState);
			defBowASB_p.OnPointerDownMock(eventDataMock);
			AssertState(defBowASB_p, Slottable.WaitForPickUpState);
			defBowASB_p.WaitAndPickUpProcess.Expire();
			AssertAction(defBowASB_p,"WaitAndPickUpProcess done");
			Assert.That(defBowASB_p.WaitAndPickUpProcess.IsRunning, Is.False);
			Assert.That(defBowASB_p.WaitAndPickUpProcess.IsExpired, Is.True);
			AssertState(defBowASB_p, Slottable.PickedUpAndSelectedState);
			Assert.That(defBowASB_p.PickedUpAndSelectedProcess.IsRunning, Is.True);
			Assert.That(defBowASB_p.PickedUpAndSelectedProcess.IsExpired, Is.False);
		//
		// sb.OnDehoveredMock(eventDataMock);
		// AssertState(sb, Slottable.PickedUpAndDeselectedState);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.False);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.False);
		// Assert.That(sb.SGM.SelectedSB, Is.Null);
		

	}
	public void TestWaitForPickUpState(){
		/*	entering
		*/
		defBowASB_p.OnPointerDownMock(eventDataMock);
		AssertState(defBowASB_p, Slottable.WaitForPickUpState);
		Assert.That(defBowASB_p.WaitAndPickUpProcess.IsRunning, Is.True);
		Assert.That(defBowASB_p.WaitAndPickUpProcess.IsExpired, Is.False);
		/*	expire
		*/
		defBowASB_p.WaitAndPickUpProcess.Expire();
		AssertAction(defBowASB_p,"WaitAndPickUpProcess done");
		AssertState(defBowASB_p, Slottable.PickedUpAndSelectedState);
		Assert.That(defBowASB_p.WaitAndPickUpProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.WaitAndPickUpProcess.IsExpired, Is.True);
		/* abort, pointer up
		*/
		defBowASB_p.SetState(Slottable.FocusedState);
		AssertState(defBowASB_p, Slottable.FocusedState);
		defBowASB_p.OnPointerDownMock(eventDataMock);
		AssertState(defBowASB_p, Slottable.WaitForPickUpState);
		
		defBowASB_p.OnPointerUpMock(eventDataMock);
		AssertState(defBowASB_p, Slottable.WaitForNextTouchState);
		Assert.That(defBowASB_p.WaitAndPickUpProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.WaitAndPickUpProcess.IsExpired, Is.False);
		/*	OnEndDrag
		*/
		defBowASB_p.SetState(Slottable.FocusedState);
		AssertState(defBowASB_p, Slottable.FocusedState);
		defBowASB_p.OnPointerDownMock(eventDataMock);
		AssertState(defBowASB_p, Slottable.WaitForPickUpState);

		defBowASB_p.OnEndDragMock(eventDataMock);
		AssertState(defBowASB_p, Slottable.FocusedState);
		Assert.That(defBowASB_p.WaitAndPickUpProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.WaitAndPickUpProcess.IsExpired, Is.False);
		/*	OnPointerExit
		*/
		defBowASB_p.SetState(Slottable.FocusedState);
		AssertState(defBowASB_p, Slottable.FocusedState);
		defBowASB_p.OnPointerDownMock(eventDataMock);
		AssertState(defBowASB_p, Slottable.WaitForPickUpState);

		defBowASB_p.OnDehoveredMock(eventDataMock);
		AssertState(defBowASB_p, Slottable.FocusedState);
		Assert.That(defBowASB_p.WaitAndPickUpProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.WaitAndPickUpProcess.IsExpired, Is.False);
		


	}
	public void TestWaitForPointerUpState(){
		defBowASB_p.SetState(Slottable.DefocusedState);
		
		defBowASB_p.OnPointerDownMock(eventDataMock);
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
		defBowASB_p.OnPointerDownMock(eventDataMock);
		AssertState(defBowASB_p, Slottable.WaitForPointerUpState);
		defBowASB_p.OnPointerUpMock(eventDataMock);
		AssertAction(defBowASB_p,"tapped");
		AssertState(defBowASB_p, Slottable.DefocusedState);
		Assert.That(defBowASB_p.WaitAndSetBackToDefocusedStateProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.WaitAndSetBackToDefocusedStateProcess.IsExpired, Is.False);
		/*OnEndDrag
		*/
		defBowASB_p.OnPointerDownMock(eventDataMock);
		AssertState(defBowASB_p, Slottable.WaitForPointerUpState);
		defBowASB_p.OnEndDragMock(eventDataMock);
		AssertState(defBowASB_p, Slottable.DefocusedState);
		Assert.That(defBowASB_p.WaitAndSetBackToDefocusedStateProcess.IsRunning, Is.False);
		Assert.That(defBowASB_p.WaitAndSetBackToDefocusedStateProcess.IsExpired, Is.False);
		
		
	}
	
	public void TestFocusedState(){
		
		// sb.SetState(Slottable.MovingState);
		// sb.SetState(Slottable.WaitForNextTouchState);
		// sb.SetState(Slottable.DeactivatedState);
		// sb.SetState(Slottable.FocusedState);
		
		/*	Graying in
		*/
		/*	enter
		*/
			AssertState(defBowASB_p, Slottable.FocusedState);
			AssertAction(defBowASB_p,"InstantGrayin called");
			defBowASB_p.SetState(Slottable.DefocusedState);
			defBowASB_p.SetState(Slottable.FocusedState);
			Assert.That(defBowASB_p.GradualGrayinProcess.IsRunning, Is.True);
			Assert.That(defBowASB_p.GradualGrayinProcess.IsExpired, Is.False);
			/*	expire
			*/
			defBowASB_p.GradualGrayinProcess.Expire();
			AssertAction(defBowASB_p,"GradualGrayinProcess done");
			Assert.That(defBowASB_p.GradualGrayinProcess.IsRunning, Is.False);
			Assert.That(defBowASB_p.GradualGrayinProcess.IsExpired, Is.True);

			defBowASB_p.SetState(Slottable.DefocusedState);
			defBowASB_p.SetState(Slottable.FocusedState);
			/*	abort
			*/
			defBowASB_p.SetState(Slottable.SelectedState);
			Assert.That(defBowASB_p.GradualGrayinProcess.IsRunning, Is.False);
			Assert.That(defBowASB_p.GradualGrayinProcess.IsExpired, Is.False);
		
		/*	Dehighlighting
		*/
			/* enter
			*/
			defBowASB_p.SetState(Slottable.FocusedState);
			Assert.That(defBowASB_p.GradualDehighlightProcess.IsRunning, Is.True);
			Assert.That(defBowASB_p.GradualDehighlightProcess.IsExpired, Is.False);
			/*	expire
			*/
			defBowASB_p.GradualDehighlightProcess.Expire();
			AssertAction(defBowASB_p,"GradualDehighlightProcess done");
			Assert.That(defBowASB_p.GradualDehighlightProcess.IsRunning, Is.False);
			Assert.That(defBowASB_p.GradualDehighlightProcess.IsExpired, Is.True);
			defBowASB_p.SetState(Slottable.SelectedState);
			Assert.That(defBowASB_p.GradualDehighlightProcess.IsRunning, Is.False);
			Assert.That(defBowASB_p.GradualDehighlightProcess.IsExpired, Is.True);
			/*	abort
			*/
			defBowASB_p.SetState(Slottable.FocusedState);
			defBowASB_p.SetState(Slottable.SelectedState);
			Assert.That(defBowASB_p.GradualDehighlightProcess.IsRunning, Is.False);
			Assert.That(defBowASB_p.GradualDehighlightProcess.IsExpired, Is.False);
		
		/*Pointer enter
		*/
		defBowASB_p.SetState(Slottable.FocusedState);
		defBowASB_p.GradualDehighlightProcess.Expire();
		
		
		// anotherSb.SetState(Slottable.PickedUpAndSelectedState);
		// eventDataMock.pointerDrag = anotherGO;
		defBowASB_p.OnHoveredMock(eventDataMock);

		AssertState(defBowASB_p, Slottable.SelectedState);
		/*	pointer down
		*/
		defBowASB_p.SetState(Slottable.FocusedState);
		defBowASB_p.GradualDehighlightProcess.Expire();
		AssertState(defBowASB_p, Slottable.FocusedState);
		defBowASB_p.OnPointerDownMock(eventDataMock);
		AssertState(defBowASB_p, Slottable.WaitForPickUpState);


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
	
	public void TestDeactivatedState(){
		defBowASB_p.SetState(Slottable.DeactivatedState);
		AssertState(defBowASB_p, Slottable.DeactivatedState);
		defBowASB_p.OnPointerDownMock(eventDataMock);
		AssertState(defBowASB_p, Slottable.DeactivatedState);
		
	}
	
	public void InstantiateAndInitialize(){
		sbGO = new GameObject();
		defBowASB_p = sbGO.AddComponent<Slottable>();
		defBowASB_p.Initialize(sgpAll);

	}
	public void CreateAndSetSlottableItem(){
		
		BowMock defBow = new BowMock();
		defBow.ItemID = 0;
		BowInstanceMock defBowA = new BowInstanceMock();
		defBowA.Quantity = 1;
		defBowA.Item = defBow;

		defBowASB_p.SetSlottableItem(defBowA);
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
}
