using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections.Generic;
public class SlottableTest {


	class SlotSystemTestResult{
		public string pAS;
		public string tAS;
		public SlotGroup hoveredSG;
		public Slottable hoveredSB;
		public string hoveredName;
		public Slottable pickedSB;
		public string pickedSBName;
		public string Val;
		public SlotSystemTestResult(bool isPAS, bool isTAS, Slottable pickedSB, SlotGroup hoveredSG,  int i){
			this.hoveredSG = hoveredSG; this.pickedSB = pickedSB;
			this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			this.tAS = "tAS: " + (isTAS?Util.Blue("on "):Util.Red("off "));
			this.hoveredName = " on " + Util.SGName(hoveredSG);
				if(hoveredSG.IsPool)
					hoveredName = Util.Red(hoveredName);
				else
					hoveredName = Util.Blue(hoveredName);

			this.pickedSBName = Util.SBName(pickedSB) + " of " + Util.SGName(pickedSB.SG);
				if(pickedSB.SG.IsPool)
					pickedSBName = Util.Red(pickedSBName);
				else
					pickedSBName = Util.Blue(pickedSBName);
			this.Val = i.ToString();
		}
		public SlotSystemTestResult(bool isPAS, bool isTAS, Slottable pickedSB, SlotGroup hoveredSG, SlotSystemTransaction ta){
			this.hoveredSG = hoveredSG; this.pickedSB = pickedSB;
			this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			this.tAS = "tAS: " + (isTAS?Util.Blue("on "):Util.Red("off "));
			this.hoveredName = " on " + Util.SGName(hoveredSG);
				if(hoveredSG.IsPool)
					hoveredName = Util.Red(hoveredName);
				else
					hoveredName = Util.Blue(hoveredName);

			this.pickedSBName = Util.SBName(pickedSB) + " of " + Util.SGName(pickedSB.SG);
				if(pickedSB.SG.IsPool)
					pickedSBName = Util.Red(pickedSBName);
				else
					pickedSBName = Util.Blue(pickedSBName);
				if(pickedSB.IsEquipped && pickedSB.SG.IsPool)
					pickedSBName = Util.Bold(pickedSBName);
			this.Val = ta.GetType().ToString();
			if(ta.GetType() == typeof(FillEquipTransaction))
				Val = Util.Terra(Val);
			if(ta.GetType() == typeof(SwapTransaction))
				Val = Util.Aqua(Val);
			if(ta.GetType() == typeof(StackTransaction))
				Val = Util.Forest(Val);
			if(ta.GetType() == typeof(ReorderTransaction))
				Val = Util.Berry(Val);
			if(ta.GetType() == typeof(InsertTransaction))
				Val = Util.Violet(Val);
		}
		public SlotSystemTestResult(bool isPAS, bool isTAS, Slottable pickedSB, SlotGroup sg, Slottable hoveredSB, SlotSystemTransaction ta){
			this.hoveredSG = sg; this.pickedSB = pickedSB; this.hoveredSB = hoveredSB;
			this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			this.tAS = "tAS: " + (isTAS?Util.Blue("on "):Util.Red("off "));

			this.hoveredName = Util.SGName(sg);
			string hoveredSBStr = Util.SBName(hoveredSB);
				if(hoveredSB.SG.IsPool && hoveredSB.IsEquipped)
					hoveredSBStr = Util.Bold(hoveredSBStr);
			hoveredName = " on " + hoveredSBStr + " of " + hoveredName;
				if(sg.IsPool)
					hoveredName = Util.Red(hoveredName);
				else
					hoveredName = Util.Blue(hoveredName);

			this.pickedSBName = Util.SBName(pickedSB) + " of " + Util.SGName(pickedSB.SG);
				if(pickedSB.SG.IsPool)
					pickedSBName = Util.Red(pickedSBName);
				else
					pickedSBName = Util.Blue(pickedSBName);
				if(pickedSB.IsEquipped && pickedSB.SG.IsPool)
					pickedSBName = Util.Bold(pickedSBName);
			this.Val = ta.GetType().ToString();
			if(ta.GetType() == typeof(FillEquipTransaction))
				Val = Util.Terra(Val);
			if(ta.GetType() == typeof(SwapTransaction))
				Val = Util.Aqua(Val);
			if(ta.GetType() == typeof(StackTransaction))
				Val = Util.Forest(Val);
			if(ta.GetType() == typeof(ReorderTransaction))
				Val = Util.Berry(Val);
			if(ta.GetType() == typeof(InsertTransaction))
				Val = Util.Violet(Val);
			if(ta.GetType() == typeof(ReorderInOtherSGTransaction))
				Val = Util.Khaki(Val);
		}
		public SlotSystemTestResult(bool isPAS, Slottable pickedSB, bool test){
			this.pickedSB = pickedSB;
			this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			this.pickedSBName = Util.SBName(pickedSB) + " of " + Util.SGName(pickedSB.SG);
				if(pickedSB.SG.IsPool)
					pickedSBName = Util.Red(pickedSBName);
				else
					pickedSBName = Util.Blue(pickedSBName);
				if(pickedSB.IsEquipped && pickedSB.SG.IsPool)
					pickedSBName = Util.Bold(pickedSBName);
			this.Val = " isPickable: " + test.ToString();
				if(test) Val = Util.Blue(Val); else Val = Util.Red(Val);
		}
		public SlotSystemTestResult(bool isPAS, Slottable pickedSB, SBState state){
			this.pickedSB = pickedSB;
			this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			this.pickedSBName = Util.SBName(pickedSB) + " of " + Util.SGName(pickedSB.SG);
				if(pickedSB.SG.IsPool)
					pickedSBName = Util.Red(pickedSBName);
				else
					pickedSBName = Util.Blue(pickedSBName);
				if(pickedSB.IsEquipped && pickedSB.SG.IsPool)
					pickedSBName = Util.Bold(pickedSBName);
			this.Val = state.GetType().ToString();
				if(state is SBFocusedState)
					Val = Util.Terra(Val);
				if(state is SBDefocusedState)
					Val = Util.Ciel(Val);
				if(state is WaitForPointerUpState)
					Val = Util.Aqua(Val);
		}
		public SlotSystemTestResult(bool isPAS, Slottable pickedSB, SBSelectionState curSelSt, SBSelectionState prevSelSt, SBProcess selProcess ,SBActionState curActSt, SBActionState prevActSt, SBProcess actProcess){
			this.pickedSB = pickedSB;
			this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			this.pickedSBName = Util.SBofSG(pickedSB);

			string curSelStStr = Util.SBStateName(curSelSt);
			string prevSelStStr = Util.SBStateName(prevSelSt);
			string selProcStr;
			if(selProcess == null) selProcStr = "null";
			else selProcStr = Util.SBProcessName(selProcess);
			string curActStStr = Util.SBStateName(curActSt);
			string prevActStStr = Util.SBStateName(prevActSt);
			string actProcStr;
			if(actProcess == null) actProcStr = "null";
			else actProcStr = Util.SBProcessName(actProcess);
			
			this.Val = Util.Bold("Sel: ") + prevSelStStr + " to " + curSelStStr + ", Process: " + selProcStr + Util.Bold(" Act: ") + prevActStStr + " to " + curActStStr + ", Process: " + actProcStr;
		}
		public SlotSystemTestResult(bool isPAS, Slottable pickedSB, SGMState curState, SGMState prevState, SlotSystemTransaction ta, SGMProcess proc, bool pSBDone, bool sSBDone, bool oSGDone, bool sSGDone){
			this.pickedSB = pickedSB;
			this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			this.pickedSBName = Util.SBofSG(pickedSB);

			string curSGMSt = Util.SGMStateName(curState);
			string prevSGMSt = Util.SGMStateName(prevState);
			string taStr = Util.TransactionName(ta);
			string procStr = Util.SGMProcessName(proc);

			this.Val = "PickedSB: " + pickedSBName + " SGM: from " + prevSGMSt + " to " + curSGMSt + ", TA: " + taStr + ", Proc: " + procStr + ", taComp: " + pSBDone + ", " + sSBDone+ ", " + oSGDone + ", " + sSGDone; 
			
		}
		public SlotSystemTestResult(bool isPAS, SlotGroup sg, SGSelectionState prevSel, SGSelectionState curSel, SGProcess selProc, SGActionState prevAct, SGActionState curAct, SGProcess actProc){
			this.hoveredSG = sg;
			this.hoveredName = Util.SGName(sg);
			this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			
			this.Val = Util.SGName(sg)+ " Sel from " + Util.SGStateName(prevSel) + " to "+ Util.SGStateName(curSel) + ", proc " + Util.SGProcessName(selProc)+", Act from " + Util.SGStateName(prevAct) + " to " + Util.SGStateName(curAct) + ", proc " + Util.SGProcessName(actProc);

		}
		public bool SameSGSB(SlotSystemTestResult other){
			if(pickedSB == null){
				return this.hoveredSG == other.hoveredSG;
			}else{
				if(hoveredSG == null)
					return this.pickedSB == other.pickedSB;
				else{
					if(hoveredSB == null)
						return this.hoveredSG == other.hoveredSG && this.pickedSB == other.pickedSB;
					else
						return this.hoveredSG == other.hoveredSG && this.pickedSB == other.pickedSB && this.hoveredSB == other.hoveredSB;
				}
			}
		}
		
	}
		List<SlotSystemTestResult> testResults = new List<SlotSystemTestResult>();
		List<List<SlotSystemTestResult>> crossResultsBundles{
			get{
				List<List<SlotSystemTestResult>> result = new List<List<SlotSystemTestResult>>();
				foreach(SlotSystemTestResult res in testResults){
					bool found = false;
					foreach(List<SlotSystemTestResult> bundle in result){
						if(res.SameSGSB(bundle[0]))
							found = true;
					}
					if(!found){
						List<SlotSystemTestResult> bundle = new List<SlotSystemTestResult>();
						bundle.Add(res);
						foreach(SlotSystemTestResult res2 in testResults){
							if(res != res2)
								if(res.SameSGSB(res2)){
									bundle.Add(res2);
								}
						}
						result.Add(bundle);
					}
				}
				return result;
			}
		}
		public void PrintTestResult(string suppressedVal){
			foreach(List<SlotSystemTestResult> bundle in crossResultsBundles){
				if(HasAllSameValue(bundle)){
					if(!(suppressedVal != null && bundle[0].Val == suppressedVal))
						Debug.Log(bundle[0].pickedSBName + bundle[0].hoveredName + " " + bundle[0].Val);
				}else{
					foreach(SlotSystemTestResult res in bundle){
						if(!(suppressedVal != null && bundle[0].Val == suppressedVal))
							Debug.Log(res.pAS + " " + res.tAS +" "+ res.pickedSBName + res.hoveredName + " " + res.Val);
					}
				}
			}
			ClearCResults();
		}
			bool HasAllSameValue(List<SlotSystemTestResult> bundle){
				bool flag = true;
				string prevVal = "";
				foreach(SlotSystemTestResult res in bundle){
					flag &= prevVal == ""? true: res.Val == prevVal;
					prevVal = res.Val;
				}
				return flag;
			}
			public void ClearCResults(){
				testResults.Clear();
			}
	/*	fields 	*/
		PointerEventDataMock eventData = new PointerEventDataMock();
		GameObject sgmGO;
		SlotGroupManager sgm;
		/*	sgs	*/
			/*	pool	*/
				GameObject sgpAllGO;
				SlotGroup sgpAll;
				GameObject sgpPartsGO;
				SlotGroup sgpParts;
				GameObject sgpBowGO;
				SlotGroup sgpBow;
				GameObject sgpWearGO;
				SlotGroup sgpWear;
				GameObject sgpCGearsGO;
				SlotGroup sgpCGears;
			/*	Equip */
				GameObject sgBowGO;
				SlotGroup sgBow;
				GameObject sgWearGO;
				SlotGroup sgWear;
				GameObject sgCGearsGO;
				SlotGroup sgCGears;
		/*	items	*/
			Slottable defBowB_p;
			Slottable defBowB_e;
			Slottable defBowA_p;
			Slottable defBowA_e;
			Slottable crfBowA_p;
			Slottable crfBowA_e;
			Slottable defWearB_p;
			Slottable defWearB_e;
			Slottable defWearA_p;
			Slottable defWearA_e;
			Slottable crfWearA_p;
			Slottable crfWearA_e;
			Slottable defParts_p;
			Slottable crfParts_p;
			Slottable defParts_p2;
			Slottable crfParts_p2;
			Slottable defShieldA_p;
			Slottable defShieldA_e;
			Slottable defMWeaponA_p;
			Slottable defMWeaponA_e;
			Slottable crfShieldA_p;
			Slottable crfShieldA_e;
			Slottable crfMWeaponA_p;
			Slottable crfMWeaponA_e;
			Slottable defQuiverA_p;
			Slottable defQuiverA_e;
			Slottable defPackA_p;
			Slottable defPackA_e;

	
		/*	inventories	*/
		PoolInventory poolInv = new PoolInventory();
		EquipmentSetInventory equipInv = new EquipmentSetInventory();
		bool picked;
	[SetUp]
	public void Setup(){
		/*	SGM	*/
			sgmGO = new GameObject("SlotGroupManager");
			sgm = sgmGO.AddComponent<SlotGroupManager>();
			// sgm.SetupCommands();
		/*	SGs	*/
			/*	sgpAll	*/
				sgpAllGO = new GameObject("PoolSlotGroup");
				sgpAll = sgpAllGO.AddComponent<SlotGroup>();
				sgpAll.Initialize(SlotGroup.NullFilter, poolInv, true, 0);
			/*	sgBow	*/
				sgBowGO = new GameObject("BowSlotGroup");
				sgBow = sgBowGO.AddComponent<SlotGroup>();
				sgBow.Initialize(SlotGroup.BowFilter, equipInv, false, 1);
			/*	sgWear	*/
				sgWearGO = new GameObject("WearSlotGroup");
				sgWear = sgWearGO.AddComponent<SlotGroup>();
				sgWear.Initialize(SlotGroup.WearFilter, equipInv, false, 1);
			/*	sgCGears	*/
				sgCGearsGO = new GameObject("CarriedGearsSG");
				sgCGears = sgCGearsGO.AddComponent<SlotGroup>();
				equipInv.SetEquippableCGearsCount(4);
				int slotCount = equipInv.EquippableCGearsCount;
				sgCGears.Initialize(SlotGroup.CGearsFilter, equipInv, true, slotCount);
			/*	sgpParts	*/
				sgpPartsGO = new GameObject("PartsSlotGroupPool");
				sgpParts = sgpPartsGO.AddComponent<SlotGroup>();
				sgpParts.Initialize(SlotGroup.PartsFilter, poolInv, true, 0);
			/*	sgpBow	*/
				sgpBowGO = new GameObject("sgpBowGO");
				sgpBow = sgpBowGO.AddComponent<SlotGroup>();
				sgpBow.Initialize(SlotGroup.BowFilter, poolInv, true, 0);
			/*	sgpWear	*/
				sgpWearGO = new GameObject("sgpWearGO");
				sgpWear = sgpWearGO.AddComponent<SlotGroup>();
				sgpWear.Initialize(SlotGroup.WearFilter, poolInv, true, 0);
			/*	sgpCGears	*/
				sgpCGearsGO = new GameObject("sgpCGearsGO");
				sgpCGears = sgpCGearsGO.AddComponent<SlotGroup>();
				sgpCGears.Initialize(SlotGroup.CGearsFilter, poolInv, true, 0);
		/*	Items	*/
			/*	bows	*/
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
			/*	wears	*/
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
			/*	parts	*/ 
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
		
			/*	carried gears	*/
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

				poolInv.AddItem(defShieldA);
				poolInv.AddItem(crfShieldA);
				poolInv.AddItem(defMWeaponA);
				poolInv.AddItem(crfMWeaponA);
				poolInv.AddItem(defQuiverA);
				poolInv.AddItem(defPackA);
				equipInv.AddItem(defShieldA);
				equipInv.AddItem(defMWeaponA);

		/*	SGM fields	*/
			EquipmentSet equipSetA = new EquipmentSet(sgBow, sgWear, sgCGears);
			SlotSystemBundle equipBundle = new SlotSystemBundle();
			equipBundle.Elements.Add(equipSetA);
			equipBundle.SetFocusedBundleElement(equipSetA);
			SlotSystemBundle poolBundle = new SlotSystemBundle();
			poolBundle.Elements.Add(sgpAll);
			poolBundle.Elements.Add(sgpParts);
			poolBundle.Elements.Add(sgpBow);
			poolBundle.Elements.Add(sgpWear);
			poolBundle.Elements.Add(sgpCGears);
			poolBundle.SetFocusedBundleElement(sgpAll);
			InventoryManagerPage invManPage = new InventoryManagerPage(poolBundle, equipBundle);
			sgm.SetRootPage(invManPage);	
			/*	Assert Setup
				Preinitialized validation
				validate all the required fields are filled within the inspector window or at the time of declaration
			*/
			/*	SGM setup */
				// Assert.That(sgm.UpdateTransactionCommand, Is.Not.Null);
				// AB(sgm.PostPickFilterCommand != null, true);
				ANull(sgm.Transaction);
				
				AE(sgm.PrevState, SGMDeactivated);
				AE(sgm.CurState, SGMDeactivated);

				ANull(sgm.SelectedSB);
				ANull(sgm.SelectedSG);
				ANull(sgm.PickedSB);

				Assert.That(sgm.RootPage, Is.TypeOf(typeof(InventoryManagerPage)));
				AE(sgm.RootPage.SGM, sgm);

				AE(sgm.RootPage.Elements.Count, 2);
				AB(sgm.RootPage.ContainsElement(poolBundle), true);
				AE(poolBundle.SGM, sgm);
				AB(sgm.RootPage.ContainsElement(equipBundle), true);
				AE(equipBundle.SGM, sgm);

				AE(poolBundle.Elements.Count, 5);
				AB(poolBundle.ContainsElement(sgpAll), true);
				AE(sgpAll.SGM, sgm);
				AB(poolBundle.ContainsElement(sgpParts), true);
				AE(sgpParts.SGM, sgm);
				AB(poolBundle.ContainsElement(sgpBow), true);
				AE(sgpBow.SGM, sgm);
				AB(poolBundle.ContainsElement(sgpWear), true);
				AE(sgpWear.SGM, sgm);
				AB(poolBundle.ContainsElement(sgpCGears), true);
				AE(sgpCGears.SGM, sgm);
				AE(poolBundle.GetFocusedBundleElement(), sgpAll);

				AE(equipBundle.Elements.Count, 1);
				AE(equipBundle.GetFocusedBundleElement(), equipSetA);
				AE(equipSetA.SGM, sgm);

				AE(equipSetA.Elements.Count, 3);
				AB(equipSetA.ContainsElement(sgBow), true);
				AE(sgBow.SGM, sgm);
				AE(equipSetA.ContainsElement(sgWear), true);
				AE(sgWear.SGM, sgm);

		sgm.Initialize();
			/*	Assert Initialization
				when the scene is loaded, but not yet focused
			*/
			/*	define sbs	*/
				defBowA_p = sgpAll.GetSB(defBowA);
				defBowB_p = sgpAll.GetSB(defBowB);
				crfBowA_p = sgpAll.GetSB(crfBowA);
				defWearA_p = sgpAll.GetSB(defWearA);
				defWearB_p = sgpAll.GetSB(defWearB);
				crfWearA_p = sgpAll.GetSB(crfWearA);
				defParts_p = sgpAll.GetSB(defPartsA);
				crfParts_p = sgpAll.GetSB(crfPartsA);
				defBowA_e = sgBow.GetSB(defBowA);
				defWearA_e = sgWear.GetSB(defWearA);
				defParts_p2 = sgpParts.GetSB(defPartsA);
				crfParts_p2 = sgpParts.GetSB(crfPartsA);
				defShieldA_p = sgpAll.GetSB(defShieldA);
				crfShieldA_p = sgpAll.GetSB(crfShieldA);
				defMWeaponA_p =	sgpAll.GetSB(defMWeaponA); 
				crfMWeaponA_p =	sgpAll.GetSB(crfMWeaponA);
				defShieldA_e = sgCGears.GetSB(defShieldA);
				defMWeaponA_e = sgCGears.GetSB(defMWeaponA);
				defQuiverA_p = sgpAll.GetSB(defQuiverA);
				defPackA_p = sgpAll.GetSB(defPackA);

			AE(sgm.CurState, SGMDeactivated);
				/*	SGPs	*/
					/*	sgpAll	*/
						ASSG(sgpAll,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null);
						AB(sgpAll.IsShrinkable, true);
						AB(sgpAll.IsExpandable, true);
						AssertSGCounts(sgpAll,
							14,//slots
							14,//items
							14);//sbs
						AE(sgpAll.EquippedSBs.Count, 4);
						foreach(Slottable sb in sgpAll.Slottables){
							if(sb != null){
								ASSB_s(sb, SBDeactivated, SBWFA);
								AE(sb.SG, sgpAll);
							}
						}
					/*	sgpBow	*/
						ASSG(sgpBow,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null);
						AB(sgpBow.IsShrinkable, true);
						AB(sgpBow.IsExpandable, true);
						AssertSGCounts(sgpBow,
							3,//slots
							3,//items
							3);//sbs
						AE(sgpBow.EquippedSBs.Count, 1);
						foreach(Slottable sb in sgpBow.Slottables){
							if(sb != null){
								ASSB_s(sb, SBDeactivated, SBWFA);
								AE(sb.SG, sgpBow);
							}
						}
					/*	sgpWear	*/
						ASSG(sgpWear,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null);
						AB(sgpWear.IsShrinkable, true);
						AB(sgpWear.IsExpandable, true);
						AssertSGCounts(sgpWear,
							3,//slots
							3,//items
							3);//sbs
						AE(sgpWear.EquippedSBs.Count, 1);
						foreach(Slottable sb in sgpWear.Slottables){
							if(sb != null){
								ASSB_s(sb, SBDeactivated, SBWFA);
								AE(sb.SG, sgpWear);
							}
						}
					/*	sgpCGears	*/
						ASSG(sgpCGears,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null);
						AB(sgpCGears.IsShrinkable, true);
						AB(sgpCGears.IsExpandable, true);
						AssertSGCounts(sgpCGears,
							6,//slots
							6,//items
							6);//sbs
						AE(sgpCGears.EquippedSBs.Count, 2);
						foreach(Slottable sb in sgpCGears.Slottables){
							if(sb != null){
								ASSB_s(sb, SBDeactivated, SBWFA);
								AE(sb.SG, sgpCGears);
							}
						}
					/*	sgpParts	*/
						ASSG(sgpParts,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null);
						AB(sgpParts.IsShrinkable, true);
						AB(sgpParts.IsExpandable, true);
						AssertSGCounts(sgpParts,
							2,//slots
							2,//items
							2);//sbs
						AE(sgpParts.EquippedSBs.Count, 0);
						foreach(Slottable sb in sgpParts.Slottables){
							if(sb != null){
								ASSB_s(sb, SBDeactivated, SBWFA);
								AE(sb.SG, sgpParts);
							}
						}
				/*	SGEs	*/
					/*	sgBow	*/
						ASSG(sgBow,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null);
						AB(sgBow.IsShrinkable, false);
						AB(sgBow.IsExpandable, false);
						AssertSGCounts(sgBow,
							1,//slots
							1,//items
							1);//sbs
						AE(sgBow.EquippedSBs.Count, 1);
						foreach(Slottable sb in sgBow.Slottables){
							if(sb != null){
								ASSB_s(sb, SBDeactivated, SBWFA);
								AE(sb.SG, sgBow);
								AE(sb.IsEquipped, true);
							}
						}
					/*	sgWear	*/
						ASSG(sgWear,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null);
						AB(sgWear.IsShrinkable, false);
						AB(sgWear.IsExpandable, false);
						AssertSGCounts(sgWear,
							1,//slots
							1,//items
							1);//sbs
						AE(sgWear.EquippedSBs.Count, 1);
						foreach(Slottable sb in sgWear.Slottables){
							if(sb != null){
								ASSB_s(sb, SBDeactivated, SBWFA);
								AE(sb.SG, sgWear);
								AE(sb.IsEquipped, true);
							}
						}
					/*	sgCGears	*/
						ASSG(sgCGears,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null);
						AB(sgCGears.IsShrinkable, true);
						AB(sgCGears.IsExpandable, false);
						AssertSGCounts(sgCGears,
							4,//slots
							2,//items
							2);//sbs
						AE(sgCGears.EquippedSBs.Count, 2);
						foreach(Slottable sb in sgCGears.Slottables){
							if(sb != null){
								ASSB_s(sb, SBDeactivated, SBWFA);
								AE(sb.SG, sgCGears);
								AE(sb.IsEquipped, true);
							}
						}
		sgm.Focus();
		AssertFocused();
	}
	[Test]
	public void TestAll(){
		//	done
			// TestFillEquippableOnAll();
			// TestSwappable();
			// TestVolSortOnAll();
			// CheckShrinkableAndExpandableOnAllSGs();
			// CheckTransactionOnAllSG();
			// CheckTransacitonWithSBSpecifiedOnAll();
			// CheckPickcableOnAllSB();
			// CheckEventOnAllSB();
			// TestPickUpTransitionOnAll(); //after transaction update done
			// TestSBStateTransitionOnAll();
			// TestSGStateTransitionOnAll();
		// TestRevertOnAllSBs();
		// TestReorderOnAll();
		// TestFillEquipOnAll();
		// test transactions after each slotsys elements's state stransition is done
		TestSGMStateTransition();
	}
	public void TestSGMStateTransition(){

	}
	public void TestSGStateTransitionOnAll(){
		PerformOnAllSGAfterFocusing(TestSGStateTransition);
		PrintTestResult(null);
	}
		public void TestSGStateTransition(SlotGroup sg, bool isPAS){
			// CaptureSGState(sg, isPAS);
			/*	Selection State	*/
				// if(sg.PrevSelState == SGDeactivated)
				// 		ASSG(sg,
				// 			SGDeactivated, SGFocused, null,
				// 			SGWFA, SGWFA, null);
				// else 
				// 		ASSG(sg,
				// 			SGDefocused, SGFocused, typeof(SGGreyinProcess),
				// 			SGWFA, SGWFA, null);

				// 	sg.SetSelState(SGDefocused);
				// 		ASSG(sg,
				// 			SGFocused, SGDefocused, typeof(SGGreyoutProcess),
				// 			SGWFA, SGWFA, null);
				// 	sg.SetSelState(SGFocused);
				// 		ASSG(sg,
				// 			SGDefocused, SGFocused, typeof(SGGreyinProcess),
				// 			SGWFA, SGWFA, null);
				// 	sg.SetSelState(SGSelected);
				// 		ASSG(sg,
				// 			SGFocused, SGSelected, typeof(SGHighlightProcess),
				// 			SGWFA, SGWFA, null);
				// 	sg.SetSelState(SGFocused);
				// 		ASSG(sg,
				// 			SGSelected, SGFocused, typeof(SGDehighlightProcess),
				// 			SGWFA, SGWFA, null);
				// 	sg.SetSelState(SGDeactivated);
				// 		ASSG(sg,
				// 			SGFocused, SGDeactivated, null,
				// 			SGWFA, SGWFA, null);
				// 	sg.SetSelState(SGFocused);
				// 		ASSG(sg,
				// 			SGDeactivated, SGFocused, null,
				// 			SGWFA, SGWFA, null);
			/*	Action state	*/
			if(isPAS)
					ASGActState(sg, SGWFA, SGWFA, null);
			else
					ASGActState(sg, SGTransaction, SGWFA, null);
				sg.SetActState(SlotGroup.PerformingTransactionState);
					ASGActState(sg, SGWFA, SGTransaction, typeof(SGTransactionProcess));
				sg.SetActState(SlotGroup.WaitForActionState);
					ASGActState(sg, SGTransaction, SGWFA, null);
				
		}
	public void CaptureSGState(SlotGroup sg, bool isPAS){
		SlotSystemTestResult res = new SlotSystemTestResult(isPAS, sg,	
		sg.PrevSelState, sg.CurSelState, sg.SelectionProcess,
		sg.PrevActState, sg.CurActState, sg.ActionProcess);
		testResults.Add(res);
	}
	public void PrintSGState(SlotGroup sg, bool isPAS){
		Debug.Log(Util.SGName(sg) + " Sel from " + Util.SGStateName(sg.PrevSelState) + " to " + Util.SGStateName(sg.CurSelState) + ", proc " + Util.SGProcessName(sg.SelectionProcess) + " , Act from " + Util.SGStateName(sg.PrevActState) + " to " + Util.SGStateName(sg.CurActState) + ", proc " + Util.SGProcessName(sg.ActionProcess));
	}
	public void TestSBStateTransitionOnAll(){
		PerformOnAllSBs(TestSBStateTransition);
		PrintTestResult(null);
	}
		public void TestSBStateTransition(Slottable sb, bool isPAS){
			/*	SelState */
				// if(isPAS){
				// 	if(sb.SG.IsPool){
				// 		if(sb.IsEquipped || (sb.ItemInst is PartsInstanceMock && !(sb.SG.Filter is SGPartsFilter))){
				// 			if(sb.PrevSelState == SBDeactivated){
				// 				ASBSelState(sb, SBDeactivated, SBDefocused, null);

				// 			}else
				// 				ASBSelState(sb, SBFocused, SBDefocused, typeof(SBGreyoutProcess));

				// 			sb.SetSelState(SBSelected);
				// 				ASBSelState(sb, SBDefocused, SBSelected, typeof(SBHighlightProcess));
				// 			sb.SetSelState(SBDefocused);
				// 				ASBSelState(sb, SBSelected, SBDefocused, typeof(SBGreyoutProcess));
				// 			sb.SetSelState(SBFocused);
				// 				ASBSelState(sb, SBDefocused, SBFocused, typeof(SBGreyinProcess));
				// 			return;
				// 		}
				// 	}
				// }
				// if(sb.PrevSelState == SBDeactivated)
				// 	ASBSelState(sb, SBDeactivated, SBFocused, null);
				// else
				// 	ASBSelState(sb, SBDefocused, SBFocused, typeof(SBGreyinProcess));
				// sb.SetSelState(SBSelected);
				// 	ASBSelState(sb, SBFocused, SBSelected, typeof(SBHighlightProcess));
				// sb.SetSelState(SBFocused);
				// 	ASBSelState(sb, SBSelected, SBFocused, typeof(SBDehighlightProcess));
				// sb.SetSelState(SBDeactivated);
				// 	ASBSelState(sb, SBFocused, SBDeactivated, null);
				// sb.SetSelState(SBFocused);
				// 	ASBSelState(sb, SBDeactivated, SBFocused, null);
				// sb.SetSelState(SBDeactivated);
				// 	ASBSelState(sb, SBFocused, SBDeactivated, null);
				// sb.SetSelState(SBDefocused);
				// 	ASBSelState(sb, SBDeactivated, SBDefocused, null);
				// sb.SetSelState(SBDeactivated);
				// 	ASBSelState(sb, SBDefocused, SBDeactivated, null);
				// sb.SetSelState(SBSelected);
				// 	ASBSelState(sb, SBDeactivated, SBSelected, null);
				// sb.SetSelState(SBDeactivated);
				// 	ASBSelState(sb, SBSelected, SBDeactivated, null);
			/*	Equip State	*/
						PrintSBState(sb, isPAS);
			if(sb.SG.IsPool){
				if(sb.IsEquipped){
						ASBEqpState(sb, SBUnequipped, SBEquipped, typeof(SBEquipProcess));
					sb.SetEqpState(SBUnequipped);
						ASBEqpState(sb, SBEquipped, SBUnequipped, typeof(SBUnequipProcess));
					sb.SetEqpState(SBEquipped);
						ASBEqpState(sb, SBUnequipped, SBEquipped, typeof(SBEquipProcess));
				}
				else{
					if(sb.PrevEqpState == SBUnequipped)
						ASBEqpState(sb, SBUnequipped, SBUnequipped, null);
					else
						ASBEqpState(sb, SBEquipped, SBUnequipped, typeof(SBUnequipProcess));

					sb.SetEqpState(SBEquipped);
						ASBEqpState(sb, SBUnequipped, SBEquipped, typeof(SBEquipProcess));
					sb.SetEqpState(SBUnequipped);
						ASBEqpState(sb, SBEquipped, SBUnequipped, typeof(SBUnequipProcess));
				}
			}else{
					ASBEqpState(sb, SBUnequipped, SBEquipped, null);
				sb.SetEqpState(SBUnequipped);
					ASBEqpState(sb, SBEquipped, SBUnequipped, null);
				sb.SetEqpState(SBEquipped);
					ASBEqpState(sb, SBUnequipped, SBEquipped, null);
			}
		}
	public void TestPickUpTransitionOnAll(){
		PerformOnAllSBs(TestPickUpTransition);
		PrintTestResult(null);
	}
		public void TestPickUpTransition(Slottable sb, bool isPAS){
				AssertFocused();
			if(sb.IsPickable){
				/*	tap	*/
					sb.OnPointerDownMock(eventData);
					sb.OnPointerUpMock(eventData);
					sb.ActionProcess.Expire();
					AssertFocused();
				/*	multi tap -> pickup	*/
					sb.OnPointerDownMock(eventData);
					sb.OnPointerUpMock(eventData);
					sb.OnPointerDownMock(eventData);
					LetGo();
					sb.ActionProcess.Expire();
					AssertFocused();
				/*	pickup -> release -> expire to revert	*/
					sb.OnPointerDownMock(eventData);
					sb.ActionProcess.Expire();
					LetGo();
					sb.ActionProcess.Expire();
					AssertFocused();
				/* pickup -> release -> touch	*/
					PickUp(sb, out picked);
					sb.OnPointerUpMock(eventData);
					sb.OnPointerDownMock(eventData);
					LetGo();
					sb.ActionProcess.Expire();
					AssertFocused();
			}else{
			}
		}
	public void CaptureSBState(Slottable sb, bool isPAS){
		SBSelectionState curSelSt = sb.CurSelState;
		SBSelectionState prevSelSt = sb.PrevSelState;
		SBProcess selProcess = sb.SelectionProcess;

		SBActionState curActState = sb.CurActState;
		SBActionState prevActState = sb.PrevActState;
		SBProcess actProcess = sb.ActionProcess;
		SlotSystemTestResult res = new SlotSystemTestResult(isPAS, sb, curSelSt, prevSelSt, selProcess, curActState, prevActState, actProcess);
		testResults.Add(res);
	}
	public void PrintSBState(Slottable sb, bool isPAS){
		SBSelectionState curSelSt = sb.CurSelState;
		SBSelectionState prevSelSt = sb.PrevSelState;
		SBProcess selProcess = sb.SelectionProcess;

		SBActionState curActState = sb.CurActState;
		SBActionState prevActState = sb.PrevActState;
		SBProcess actProcess = sb.ActionProcess;

		SBEquipState curEqpState = sb.CurEqpState;
		SBEquipState prevEqpState = sb.PrevEqpState;
		SBProcess eqpProces = sb.EquipProcess;
		
		Debug.Log("AS: " + (isPAS?"on":"off") + ", " + Util.SBofSG(sb) + ", Equipped: " + (sb.IsEquipped?"true":"false")+ " Sel: from " + Util.SBStateName(prevSelSt) + " to " + Util.SBStateName(curSelSt) + ", proc: " + Util.SBProcessName(selProcess) + " , Act: from " + Util.SBStateName(prevActState) + " to " + Util.SBStateName(curActState) + ", proc: " + Util.SBProcessName(actProcess) + ", Equip: from " + Util.SBStateName(prevEqpState) + " to " + Util.SBStateName(curEqpState) + ", proc: " + Util.SBProcessName(eqpProces));
	}
	public void CaptureSGMState(Slottable sb, bool isPAS){
		Slottable pickedSB = sb;
		SGMState curState = sgm.CurState;
		SGMState prevState = sgm.PrevState;
		SlotSystemTransaction ta = sgm.Transaction;
		SGMProcess proc = sgm.StateProcess;
		bool pSBDone = sgm.PickedSBDoneTransaction;
		bool sSBDOne = sgm.SelectedSBDoneTransaction;
		bool oSGDone = sgm.OrigSGDoneTransaction;
		bool sSGDone = sgm.SelectedSGDoneTransaction;

		SlotSystemTestResult res = new SlotSystemTestResult(isPAS, sb, curState, prevState, ta, proc, pSBDone, sSBDOne, oSGDone, sSGDone);
		testResults.Add(res);
	}
	public void PrintSGMState(Slottable sb, bool isPAS){
		Slottable pickedSB = sb;
		SGMState curState = sgm.CurState;
		SGMState prevState = sgm.PrevState;
		SlotSystemTransaction ta = sgm.Transaction;
		SGMProcess proc = sgm.StateProcess;
		bool pSBDone = sgm.PickedSBDoneTransaction;
		bool sSBDone = sgm.SelectedSBDoneTransaction;
		bool oSGDone = sgm.OrigSGDoneTransaction;
		bool sSGDone = sgm.SelectedSGDoneTransaction;
		SlotGroup focSGP = sgm.FocusedSGP;

		Debug.Log("PickedSB: " + Util.SBofSG(sb) + ", SGM: from " + Util.SGMStateName(prevState) + " to " + Util.SGMStateName(curState) + ", TA: " + Util.TransactionName(ta) + ", Proc: " + Util.SGMProcessName(proc) + ", TAComp: " + pSBDone.ToString() + ", " + sSBDone.ToString() + ", " + oSGDone.ToString() + ", " + sSGDone.ToString() + ", FocSGP: " + Util.SGName(focSGP));
	}
	public void CheckEventOnAllSB(){
		PerformOnAllSBs(CheckEvent);
		PrintTestResult(null);
		}public void CheckEvent(Slottable sb, bool isPAS){
			// sb.Focus();
			sb.Defocus();
			sb.OnPointerDownMock(eventData);
			// sb.CurProcess.Expire();
			sb.OnPointerUpMock(eventData);
			SBActionState sbState = sb.CurActState;
			SlotSystemTestResult res = new SlotSystemTestResult(isPAS, sb, sbState);
			testResults.Add(res);
		}
	public void CheckPickcableOnAllSB(){
		PerformOnAllSBs(CheckPickable);
		PrintTestResult(null);
		}public void CheckPickable(Slottable sb, bool isPas){
			bool pickable = sb.IsPickable;
			SlotSystemTestResult res = new SlotSystemTestResult(isPas, sb, pickable);
			testResults.Add(res);
		}
	
	/*	Test on all	*/
		public void CheckTransacitonWithSBSpecifiedOnAll(){
			PerformOnAllSBs(CheckTransactionWithSB);
			PrintTestResult(null);
			}public void CheckTransactionWithSB(Slottable sb, bool isPickedAS){
				CrossTestingSGs(CrossCheckTransactionWithSB, sb, isPickedAS);
			}
			public void CrossCheckTransactionWithSB(SlotGroup sg, Slottable pickedSB, bool isPickedAS, bool isTargetAS){
				if(pickedSB.IsPickable){
					sgm.SetPickedSB(pickedSB);
					foreach(Slottable sb in sg.Slottables){
						if(sb != null){
							SlotSystemTransaction ta = AbsSlotSystemTransaction.GetTransaction(pickedSB, sb, null);
							SlotSystemTestResult res = new SlotSystemTestResult(isPickedAS, isTargetAS, pickedSB, sg, sb, ta);
							testResults.Add(res);
						}
					}
				}
			}
		public void CheckShrinkableAndExpandableOnAllSGs(){
			foreach(SlotGroup sg in sgm.AllSGs){
				string shrinkStr = " Shrinkable: " + sg.IsShrinkable;
				if(sg.IsShrinkable)	shrinkStr = Blue(shrinkStr);
				else shrinkStr = Red(shrinkStr);
				string expandStr = " Expandable: " + sg.IsExpandable;
				if(sg.IsExpandable)	expandStr = Blue(expandStr);
				else expandStr = Red(expandStr);
				Debug.Log(Name(sg) + shrinkStr + ", " + expandStr);
			}
		}
		public void CheckSwappableOnAll(){
			PerformOnAllSBs(CheckSwappable);
			PrintTestResult(0.ToString());
			}public void CheckSwappable(Slottable sb, bool isPickedAS){
				CrossTestingSGs(CrossCheckSwappable, sb, isPickedAS);
			}
			public void CrossCheckSwappable(SlotGroup sg, Slottable sb, bool isPickedAS ,bool isTargetAS){
				int count = sg.SwappableSBs(sb).Count;
				SlotSystemTestResult newRes = new SlotSystemTestResult(isPickedAS, isTargetAS, sb, sg, count);
				testResults.Add(newRes);
			}
		public void CheckTransactionOnAllSG(){
			PerformOnAllSBs(CheckTransaction);
			PrintTestResult(null);
			}public void CheckTransaction(Slottable sb, bool isPickedAS){
				CrossTestingSGs(CrossCheckTransaction, sb, isPickedAS);
			}
			public void CrossCheckTransaction(SlotGroup sg, Slottable sb, bool isPickedAS ,bool isTargetAS){
				if(sb.IsPickable){
					sgm.SetPickedSB(sb);
					SlotSystemTransaction ta = AbsSlotSystemTransaction.GetTransaction(sb, null, sg);
					SlotSystemTestResult newRes = new SlotSystemTestResult(isPickedAS, isTargetAS, sb, sg, ta);
					testResults.Add(newRes);
				}
			}
		public void TestFillEquippableOnAll(){
			PerformOnAllSBs(TestFillEquippable);
			}public void TestFillEquippable(Slottable sb, bool isPickedAS){
				CrossTestingSGs(CrossTestFillEquippable, sb, isPickedAS);
			}
			public void CrossTestFillEquippable(SlotGroup sg, Slottable sb, bool isPickedAS, bool isTargetAS){
				SlotGroup origSG = sb.SG;
				bool isFillEquippable = false;
				if(origSG.IsShrinkable){
					if(sg != origSG){
						if(sg.IsFocused){
							if(sg.AcceptsFilter(sb)){
								if(sg.IsExpandable){
									isFillEquippable = true;
									Debug.Log(Bold(Name(origSG) + "'s " + Name(sb) + ": " + Name(sg) + " Valid"));
								}else{
									bool emptyFound = false;
									foreach(Slot slot in sg.Slots){
										if(slot.Sb == null)
											emptyFound = true;
									}
									if(emptyFound){
										isFillEquippable = true;
										Debug.Log(Bold(Name(origSG) + "'s " + Name(sb) + ": " + Name(sg) + " Valid"));
									}else{
										Debug.Log(Red(Name(origSG) + "'s " + Name(sb) + ": " + Name(sg) + " Invalid : Not Expandable and No space"));
									}
								}
							}else{
								Debug.Log(Red(Name(origSG) + "'s " + Name(sb) + ": " + Name(sg) + " Invalid: Filtered out"));
							}
						}else{
							Debug.Log(Red(Name(origSG) + "'s " + Name(sb) + ": " + Name(sg) + " Invalid : Not Focused"));
						}
					}else{
						Debug.Log(Red(Name(origSG) + "'s " + Name(sb) + ": " + Name(sg) + " Invalid : OrigSG"));
					}
				}else{
					Debug.Log(Red(Name(origSG) + "'s " + Name(sb) + ": " + Name(sg) + " Invalid : OrigSG Not Shrinkable"));
				}
				if(isFillEquippable)
					AB(sg.IsFillEquippable(sb), true);
				else
					AB(sg.IsFillEquippable(sb), false);
			}
		public void TestFillEquipOnAll(){
			PerformOnAllSBs(TestFillEquip);
			}public void TestFillEquip(Slottable sb, bool isPickedAS){
				CrossTestingSGs(CrossTestFillEquip, sb, isPickedAS);
			}
			public void CrossTestFillEquip(SlotGroup sg, Slottable sb, bool isPickedAS, bool isTargetAS){
				InventoryItemInstanceMock pickedItem = sb.ItemInst;
				SlotGroup origSG = sb.SG;
				if(sg.IsFillEquippable(sb)){
					if(sb.IsPickable){
						Debug.Log(Name(origSG)+"'s " + Name(sb)+" on " + Name(sg));
						/*	on null sb	*/
						PickUp(sb, out picked);
						SimHover(null, sg, eventData);
							ASSGM(sgm,
								SGMProbing,
								SGMFocused,
								typeof(FillEquipTransaction),
								typeof(SGMProbeProcess),
								sb,	/*null, sg,*/
								true, true, true, true);
							ASSG(origSG,
								SGSelected, SGFocused, typeof(SGDehighlightProcess),
								SGWFA, SGWFA, null);
							ASSG(sg,
								SGFocused, SGSelected, typeof(SGHighlightProcess),
								SGWFA, SGWFA, null);
						LetGo();
							ASSGM(sgm,
								SGMTransaction,
								SGMProbing,
								typeof(FillEquipTransaction),
								typeof(SGMTransactionProcess),
								sb, /*null, sg,*/
								false, true, origSG.IsPool?true: false, sg.IsPool?true: false);
							ASSG(origSG,
								SGSelected, SGFocused, typeof(SGDehighlightProcess),
								SGWFA, SGTransaction, typeof(SGTransactionProcess));
							ASSG(sg,
								SGFocused, SGSelected, typeof(SGHighlightProcess),
								SGWFA, SGTransaction, typeof(SGTransactionProcess));
						sb.ExpireActionProcess();
							AE(sgm.PickedSBDoneTransaction, true);
						if(!sg.IsPool)
							CompleteAllSBActProcesses(sg);
						if(!origSG.IsPool)
							CompleteAllSBActProcesses(origSG);
							AssertFocused();
							ASSGM(sgm,
								SGMFocused,
								SGMTransaction,
								null,
								null,
								null, /*null, null,*/
								true, true, true, true);
							ASSG(origSG,
								SGSelected, SGFocused, typeof(SGDehighlightProcess),
								SGTransaction, SGWFA, null);
							ASSG(sg,
								SGSelected, SGFocused, typeof(SGDehighlightProcess),
								SGTransaction, SGWFA, null);
						/*	reverse	*/
							Slottable revPickedSB = sg.GetSB(pickedItem);
							PickUp(revPickedSB, out picked);
							SimHover(null ,origSG, eventData);
							PointerUp();
							if(!origSG.IsPool)
								CompleteAllSBActProcesses(origSG);
							if(!sg.IsPool)
								CompleteAllSBActProcesses(sg);
							revPickedSB.ExpireActionProcess();
								AssertFocused();
						/*	on inval sb	*/
					}
				}
			}
		public void TestVolSortOnAll(){
			PerformOnAllSGAfterFocusing(TestVolSort);
		}
		public void TestVolSort(SlotGroup sg, bool isPAS){
			if(!sg.IsAutoSort){
				sgm.SortSG(sg, SlotGroup.InverseItemIDSorter);
				if(sg.ActionProcess.IsRunning)
					CompleteAllSBActProcesses(sg);
					AssertFocused();
					AssertSBsSorted(sg, SlotGroup.InverseItemIDSorter);
				sgm.SortSG(sg, SlotGroup.AcquisitionOrderSorter);
				if(sg.ActionProcess.IsRunning)
					CompleteAllSBActProcesses(sg);
					AssertFocused();
					AssertSBsSorted(sg, SlotGroup.AcquisitionOrderSorter);
				sgm.SortSG(sg, SlotGroup.ItemIDSorter);
				if(sg.ActionProcess.IsRunning)
					CompleteAllSBActProcesses(sg);
					AssertFocused();
					AssertSBsSorted(sg, SlotGroup.ItemIDSorter);
			}
		}
	/*	thorough testing utility	*/
		public void PerformOnAllSBs(System.Action<Slottable, bool> act){
			foreach(SlotGroup sgp in sgm.AllSGPs){
				sgm.SetFocusedPoolSG(sgp);
				sgp.ToggleAutoSort(true);
				foreach(Slottable sb in sgp.Slottables){
					if(sb != null){
						act(sb, true);
					}
				}
				sgp.ToggleAutoSort(false);
				foreach(Slottable sb in sgp.Slottables){
					if(sb != null){
						act(sb, false);
					}
				}
			}
			foreach(EquipmentSet eSet in sgm.EquipmentSets){
				sgm.SetFocusedEquipmentSet(eSet);
				foreach(SlotGroup sge in sgm.FocusedSGEs){
					sge.ToggleAutoSort(true);
					foreach(Slottable sb in sge.Slottables){
						if(sb != null){
							act(sb, true);
						}
					}
					sge.ToggleAutoSort(false);
					foreach(Slottable sb in sge.Slottables){
						if(sb != null){
							act(sb, false);
						}
					}
				}
			}
		}
		public void PerformOnAllSGAfterFocusing(System.Action<SlotGroup, bool> act){
			foreach(SlotGroup sgp in sgm.AllSGPs){
				sgm.SetFocusedPoolSG(sgp);
				sgp.ToggleAutoSort(true);
				act(sgp, true);
				sgp.ToggleAutoSort(false);
				act(sgp, false);
			}
			foreach(EquipmentSet eSet in sgm.EquipmentSets){
				sgm.SetFocusedEquipmentSet(eSet);
				foreach(SlotGroup sge in sgm.FocusedSGEs){
					sge.ToggleAutoSort(true);
					act(sge, true);
					sge.ToggleAutoSort(false);
					act(sge, false);
				}
			}
		}
		public void CrossTestingSGs(System.Action<SlotGroup, Slottable, bool, bool> act, Slottable sb, bool isPickedAS){
			if(sb.SG.IsPool){
				act(sb.SG, sb, isPickedAS, isPickedAS);
				foreach(EquipmentSet eSet in sgm.EquipmentSets){
					sgm.SetFocusedEquipmentSet(eSet);
					foreach(SlotGroup sge in sgm.FocusedSGEs){
						sge.ToggleAutoSort(true);
						act(sge, sb, isPickedAS, true);
						sge.ToggleAutoSort(false);
						act(sge, sb, isPickedAS, false);
					}
				}
			}else if(sb.SG.IsSGE){
				foreach(SlotGroup sgp in sgm.AllSGPs){
					sgm.SetFocusedPoolSG(sgp);
					sgp.ToggleAutoSort(true);
					act(sgp, sb, isPickedAS, true);
					sgp.ToggleAutoSort(false);
					act(sgp, sb, isPickedAS, false);
				}
				foreach(SlotGroup sge in sgm.FocusedSGEs){
					sge.ToggleAutoSort(true);
					act(sge, sb, isPickedAS, true);
					sge.ToggleAutoSort(false);
					act(sge, sb, isPickedAS, false);
				}
			}
		}
		public void PerformOnAllSGs(System.Action<SlotGroup> act){
			foreach(SlotGroup sgp in sgm.AllSGs){
				act(sgp);
			}
		}
	/*	*/
	/*	actions	*/
		public void PickUp(Slottable sb, out bool pickedUp){
			AssertFocused();
			if(sb.IsFocused){
				sb.OnPointerDownMock(eventData);
				ASSB(sb, SBSelected, SBFocused, typeof(SBHighlightProcess),
					SBWFPickUp, SBWFA, typeof(WaitForPickUpProcess), false);
				sb.ActionProcess.Expire();
				ASSB(sb, SBDefocused, SBSelected, typeof(SBGreyoutProcess),
					SBPickedUp, SBWFPickUp, typeof(PickedUpProcess), false);
				pickedUp = true;
			}else{
				pickedUp = false;
			}
		}
		public void PointerUp(){
			sgm.PickedSB.OnPointerUpMock(eventData);
		}
		public void LetGo(){
			sgm.PickedSB.OnPointerUpMock(eventData);
			if(sgm.PickedSB.CurActState == Slottable.WaitForNextTouchState)
			sgm.PickedSB.ActionProcess.Expire();
		}
		public void CompleteAllSBActProcesses(SlotGroup sg){	
			foreach(SlotMovement sm in sg.SlotMovements){
				Slottable sb = sm.SB;
				if(sb.ActionProcess.GetType() == typeof(SBRemovedProcess) ||
				sb.ActionProcess.GetType() == typeof(SBAddedProcess) ||
				sb.ActionProcess.GetType() == typeof(SBMoveInSGProcess) ||
				sb.ActionProcess.GetType() == typeof(SBMoveOutProcess) ||
				sb.ActionProcess.GetType() == typeof(SBMoveInProcess))
					if(!sb.ActionProcess.IsExpired)
						sb.ActionProcess.Expire();
			}
			sg.CheckProcessCompletion();
		}
		public void FillEquip(Slottable sb, SlotGroup destSG){
			SlotGroup origSG = sb.SG;
			PickUp(sb, out picked);
			SimHover(null, destSG, eventData);
			LetGo();
			sb.ExpireActionProcess();
			if(!origSG.IsPool)
				CompleteAllSBActProcesses(origSG);
			if(!destSG.IsPool)
				CompleteAllSBActProcesses(destSG);
			AssertFocused();
		}
		public void SwapBowOrWear(Slottable pickedSB, SlotGroup targetSG, Slottable hoveredSB){
				AssertFocused();
			SlotGroup origSG = pickedSB.SG;
			PickUp(pickedSB, out picked);
			SimHover(hoveredSB, targetSG, eventData);			
			LetGo();
			pickedSB.ExpireActionProcess();
			if(!targetSG.IsPool)
				CompleteAllSBActProcesses(targetSG);
			else
				hoveredSB.ExpireActionProcess();
			
			if(!origSG.IsPool)
				CompleteAllSBActProcesses(origSG);
				AssertFocused();
			Slottable equipped_p = null;
			if(origSG.IsPool)
				equipped_p = sgpAll.GetSB(pickedSB.ItemInst);
			else
				equipped_p = sgpAll.GetSB(hoveredSB.ItemInst);
				AssertEquipped(equipped_p);
		}
		public void SwapCGears(Slottable pickedSB, Slottable hoveredSB){
			SlotGroup origSG = pickedSB.SG;
			SlotGroup selectedSG = hoveredSB.SG;
			PickUp(pickedSB, out picked);
			SimHover(hoveredSB, selectedSG, eventData);
			LetGo();
			pickedSB.ExpireActionProcess();
			if(!origSG.IsPool){
				CompleteAllSBActProcesses(origSG);
			}
			if(!selectedSG.IsPool){
				CompleteAllSBActProcesses(selectedSG);
			}else{
				hoveredSB.ExpireActionProcess();
			}
			AssertFocused();
		}
	/*	utility	*/
		public Slottable GetSB(SlotGroup sg, Slottable anySB){
			return sg.GetSB(anySB.ItemInst);
		} 
		public void SimHover(Slottable sb, SlotGroup sg, PointerEventDataMock eventData){
			/*	in actual implementation, this method is called whenever either sb or sg's 		boarder is crossed
			*/
			if(sgm.CurState == SlotGroupManager.ProbingState){
				if(sb != null){
					if(sgm.SelectedSB != sb){
						if(sgm.SelectedSB != null)
							sgm.SelectedSB.OnHoverExitMock(eventData);
						sb.OnHoverEnterMock(eventData);
					}
				}else{
					if(sgm.SelectedSB != null){
						sgm.SelectedSB.OnHoverExitMock(eventData);
					}
				}
				if(sg != null){
					if(sgm.SelectedSG != sg){
						if(sgm.SelectedSG != null)
							sgm.SelectedSG.OnHoverExitMock(eventData);
						sg.OnHoverEnterMock(eventData);
					}
				}else{
					if(sgm.SelectedSG != null){
						sgm.SelectedSG.OnHoverExitMock(eventData);
					}
				}
			}
			sgm.CreateTransactionResults();
		}
		List<Slottable> InvalidSBs(Slottable picked, SlotGroup targetSG){
			/*	always includes picked itself */
			SlotGroup origSG = picked.SG;
			List<Slottable> result = new List<Slottable>();
			foreach(Slottable sb in targetSG.Slottables){
				if(sb != null){
					if(sb == picked)
						result.Add(sb);
					else{
						if(origSG.IsPool){
							if(targetSG.IsPool){
								if(targetSG.IsAutoSort){
									result.Add(sb);
								}else{

								}
							}else{
								if(!targetSG.AcceptsFilter(sb))
									result.Add(sb);
							}
						}else{
							if(!targetSG.AcceptsFilter(sb))
								result.Add(sb);
							else{
								if(!Util.HaveCommonItemFamily(picked, sb)){
									result.Add(sb);
								}else{// either 1) sgpAll, 2) sgpSame, 3) sgeSame
									if(origSG != targetSG){// 1) or 2)
										if(origSG.ItemInstances.Contains(sb.ItemInst)){
											if(!(picked.ItemInst == sb.ItemInst && origSG.IsShrinkable))
											result.Add(sb);
										}
									}else{
										if(targetSG.IsAutoSort)
											result.Add(sb);
									}
								} 
							}
						}
					}
				}
			}
			return result;
		}
		public void TestInvalidSGs(){
			foreach(SlotGroup sgp in sgm.AllSGPs){
				sgm.SetFocusedPoolSG(sgp);
				sgp.ToggleAutoSort(true);
				foreach(Slottable sb in sgp.Slottables){
					if(sb != null){
						if(sb.ItemInst is PartsInstanceMock)
							AE(InvalidSGs(sb).Count, 7);
						else
							AE(InvalidSGs(sb).Count, 6);
					}
				}
				sgp.ToggleAutoSort(false);
				foreach(Slottable sb in sgp.Slottables){
					if(sb != null){
						if(sb.ItemInst is PartsInstanceMock)
							AE(InvalidSGs(sb).Count, 7);
						else
							AE(InvalidSGs(sb).Count, 6);
					}
				}
			}
			foreach(SlotGroup sgp in sgm.AllSGPs){
				sgm.SetFocusedPoolSG(sgp);
				foreach(SlotGroup sge in sgm.FocusedSGEs){
					foreach(Slottable sb_e in sge.Slottables){
						if(sb_e != null){
							if(sgp.AcceptsFilter(sb_e))
								AE(InvalidSGs(sb_e).Count, 6);
							else
								AE(InvalidSGs(sb_e).Count, 7);
						}
					}
				}
			}
		}
		List<SlotGroup> InvalidSGs(Slottable pickedSB){
			List<SlotGroup> result = new List<SlotGroup>();
			foreach(SlotGroup sgp in sgm.AllSGPs){
				if(sgm.FocusedSGP != sgp)
					result.Add(sgp);
				else{
					if(!sgp.AcceptsFilter(pickedSB))
						result.Add(sgp);
				}
			}
			foreach(SlotGroup sge in sgm.FocusedSGEs){
				if(!sge.AcceptsFilter(pickedSB))
					result.Add(sge);
			}
			return result;
		}
	/*	Assertions	*/
		/*	SGM	*/
			public void ASSGM_s(SlotGroupManager sgm, SGMState state){
				AE(sgm.CurState, state);
			}
			public void ASSGM(SlotGroupManager sgm, SGMState curState, SGMState prevState, System.Type taType, System.Type sgmProcessType, Slottable pickedSB, /*Slottable selectedSB, SlotGroup selectedSG, */bool pSBDone, bool sSBDone, bool oSGDone, bool sSGDone){
				ASSGM_s(sgm, curState);
				if(prevState != null)
					AE(sgm.PrevState, prevState);
				if(taType == null)
					ANull(sgm.Transaction);
				else
					AE(sgm.Transaction.GetType(), taType);
				if(sgmProcessType == null)
					ANull(sgm.StateProcess);
				else
					AE(sgm.StateProcess.GetType(), sgmProcessType);
				AE(sgm.PickedSB, pickedSB);
				// AE(sgm.SelectedSB, selectedSB);
				// AE(sgm.SelectedSG, selectedSG);
				AE(sgm.PickedSBDoneTransaction, pSBDone);
				AE(sgm.SelectedSBDoneTransaction, sSBDone);
				AE(sgm.OrigSGDoneTransaction, oSGDone);
				AE(sgm.SelectedSGDoneTransaction, sSGDone);
			}
		/*	SG	*/
			public void ASGSelState(SlotGroup sg, SGSelectionState prev, SGSelectionState cur, System.Type procT){
				AE(sg.CurSelState, cur);
				if(prev != null){
					AE(sg.PrevSelState, prev);
					if(procT != null)
						AE(sg.SelectionProcess.GetType(), procT);
					else
						ANull(sg.SelectionProcess);
				}
			}
			public void ASGActState(SlotGroup sg, SGActionState prev, SGActionState cur, System.Type procT){
				AE(sg.CurActState, cur);
				if(prev != null){
					AE(sg.PrevActState, prev);
					if(procT != null)
						AE(sg.ActionProcess.GetType(), procT);
					else
						ANull(sg.ActionProcess);
				}
			}
			public void AssertSGCounts(SlotGroup sg, int slotsC, int itemC, int sbsC){
				AE(sg.Slots.Count, slotsC);
				AE(sg.ActualItemInsts.Count, itemC);
				AE(sg.ActualSBsCount, sbsC);
			}
			public void AssertSBsSorted(SlotGroup sg, SGSorter sorter){
				List<Slottable> sbs = sg.Slottables;
				sorter.OrderSBs(ref sbs);
				AE(sbs.Count, sg.Slottables.Count);
				for(int i = 0; i < sg.Slottables.Count; i++){
					AE(sg.Slottables[i], sbs[i]);
				}
			}
			public void ASSG(SlotGroup sg, SGSelectionState prevSel, SGSelectionState curSel, System.Type selProcT, SGActionState prevAct, SGActionState curAct, System.Type actProcT){
				ASGSelState(sg, prevSel, curSel, selProcT);
				ASGActState(sg, prevAct, curAct, actProcT);
			}
		/*	SB	*/
			int ASBCount = 0;
			SlotGroup ASBSG;
			public void ASBReset(){
				ASBCount = 0;
				ASBSG = null;
			}
			public void ASBNull(){
				AE(ASBSG.Slottables[ASBCount], null);
				ASBCount++;
			}
			public void ASSB(Slottable sb, SBSelectionState curSelState, SBSelectionState prevSelState , System.Type selProcT, SBActionState curActState, SBActionState prevActState, System.Type actProcT, bool assertOrder){
				AE(sb.CurSelState, curSelState);
				if(prevSelState != null){
					AE(sb.PrevSelState, prevSelState);
					AE(sb.SelectionProcess.GetType(), selProcT);
					AB(sb.SelectionProcess.IsRunning, true);
					AB(sb.SelectionProcess.IsExpired, false);
				}else
					ANull(sb.SelectionProcess);
				if(prevActState!= null){
					AE(sb.PrevActState, prevActState);
					AE(sb.ActionProcess.GetType(), actProcT);
					AB(sb.ActionProcess.IsRunning, true);
					AB(sb.ActionProcess.IsExpired, false);
				}else
					ANull(sb.ActionProcess);
				AE(sb.CurActState, curActState);
				if(assertOrder){
					if(ASBCount == 0)
						ASBSG = sb.SG;
					AE(sb.SG.Slottables.IndexOf(sb), ASBCount);
					ASBCount ++;
				}
			}
			int ASBICount;
			public void ASBIReset(){
				ASBICount = 0;
			}
			public void AssertSBIndex(Slottable sb, SBActionState state, System.Type actProcT,int newID){
				/*	AssertTransactionSlottableProcess	*/
				if(actProcT!= null){
					AE(sb.ActionProcess.GetType(), actProcT);
					AB(sb.ActionProcess.IsRunning, true);
					AB(sb.ActionProcess.IsExpired, false);
				}else
					ANull(sb.ActionProcess);

				int curId;
				int newId;
				sb.GetSlotIndex(out curId, out newId);
				if(curId != -1 && curId != -2){
					AE(curId, ASBICount);
					ASBICount ++;
				}
				AE(newId, newID);
				AE(sb.CurActState, state);
			}
			public void ASSB_s(Slottable sb, SBSelectionState selState, SBActionState actState){
				AE(sb.CurSelState, selState);
				AE(sb.CurActState, actState);
			}
			public void ASBSelState(Slottable sb, SBSelectionState prevSelState, SBSelectionState curSelState, System.Type selProcT){
				AE(sb.PrevSelState, prevSelState);
				AE(sb.CurSelState, curSelState);
				if(selProcT != null)
					AE(sb.SelectionProcess.GetType(), selProcT);
				else
					ANull(sb.SelectionProcess);
			}
			public void ASBActState(Slottable sb, SBActionState prevActState, SBActionState curActState, System.Type actProcT){
				AE(sb.PrevActState, prevActState);
				AE(sb.CurActState, curActState);
				if(actProcT != null)
					AE(sb.ActionProcess.GetType(), actProcT);
				else
					ANull(sb.ActionProcess);
			}
			public void ASBEqpState(Slottable sb, SBEquipState prevEqpState, SBEquipState curEqpState, System.Type EqpProcT){
				AE(sb.PrevEqpState, prevEqpState);
				AE(sb.CurEqpState, curEqpState);
				if(EqpProcT != null)
					AE(sb.EquipProcess.GetType(), EqpProcT);
				else
					ANull(sb.EquipProcess);
			}
		/*	other	*/
			public void ANull(object obj){
				Assert.That(obj, Is.Null);
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
			public void AssertEquipped(Slottable sbInPool){
				if(sbInPool.ItemInst is BowInstanceMock || sbInPool.ItemInst is WearInstanceMock){
					bool isBow = false;
					if(sbInPool.ItemInst is BowInstanceMock)
						isBow = true;
					foreach(Slottable sbp in sgpAll.Slottables){
						if(sbp != null){
							InventoryItemInstanceMock itemInst = sbp.ItemInst;
							if((isBow && itemInst is BowInstanceMock) || (!isBow && itemInst is WearInstanceMock)){
								if(sbp == sbInPool){
									AB(sbp.IsEquipped, true);
									if(isBow)
										AE(sgm.EquippedBowInst, itemInst);
									else
										AE(sgm.EquippedWearInst, itemInst);
									Slottable sbe = isBow?sgBow.GetSB(itemInst): sgWear.GetSB(itemInst);
									AB(sbe!= null, true);
									ASSB(sbe, SBFocused, null, null, SBWFA, null, null, false);
									AB(sbe.IsEquipped, true);
									if(sgpAll == sgm.FocusedSGP){
										if(sgpAll.IsAutoSort){
											ASSB(sbp, SBDefocused, null, null, SBWFA, null, null, false);
										}else{
											ASSB(sbp, SBFocused, null, null, SBWFA, null, null, false);
										}
									}else{
										ASSB(sbp, SBDefocused, null, null, SBWFA, null, null, false);
									}
								}else{// deemed not the equipped bow/wear
									AB(sbp.IsEquipped, false);
									if(isBow){
										AB(sgm.EquippedBowInst != (BowInstanceMock)itemInst, true);
										AB(sgBow.GetSB(itemInst) == null, true);
									}else{
										AB(sgm.EquippedWearInst != (WearInstanceMock)itemInst, true);
										AB(sgWear.GetSB(itemInst) == null, true);
									}
									if(sgpAll == sgm.FocusedSGP){
										ASSB(sbp, SBFocused, null, null, SBWFA, null, null, false);
									}else{
										ASSB(sbp, SBDefocused, null, null, SBWFA, null, null, false);
									}
								}
							}
						}
					}
					SlotGroup sgp = isBow?sgpBow: sgpWear;
					foreach(Slottable sbp in sgp.Slottables){
						if(sbp != null){
							if(sbp.ItemInst == sbInPool.ItemInst){
								AB(sbp.IsEquipped, true);
								if(sgp == sgm.FocusedSGP){
									if(sgp.IsAutoSort)
										ASSB(sbp, SBDefocused, null, null, SBWFA, null, null, false);
									else
										ASSB(sbp, SBFocused, null, null, SBWFA, null, null, false);
								}else{
									ASSB(sbp, SBDefocused, null, null, SBWFA, null, null, false);
								}
							}else{/* deemed not equipped	*/
								AB(sbp.IsEquipped, false);
								if(sgp == sgm.FocusedSGP)
									ASSB(sbp, SBFocused, null, null, SBWFA, null, null, false);
								else
									ASSB(sbp, SBDefocused, null, null, SBWFA, null, null, false);
							}
						}
					}
				}
			} 
			public void AECGears(Slottable cg1, Slottable cg2, Slottable cg3, Slottable cg4){
				List<CarriedGearInstanceMock> checkedList = new List<CarriedGearInstanceMock>();
				if(cg1 != null)
					checkedList.Add((CarriedGearInstanceMock)cg1.Item);
				else
					checkedList.Add(null);
				if(cg2 != null)
					checkedList.Add((CarriedGearInstanceMock)cg2.Item);
				else
					checkedList.Add(null);
				if(cg3 != null)
					checkedList.Add((CarriedGearInstanceMock)cg3.Item);
				else
					checkedList.Add(null);
				if(cg4 != null)
					checkedList.Add((CarriedGearInstanceMock)cg4.Item);
				else
					checkedList.Add(null);
				
				int allowedCount = ((EquipmentSetInventory)sgCGears.Inventory).EquippableCGearsCount;
				
				for(int i = 0; i < 4; i++){
					if(i +1 > allowedCount)
						if(checkedList[i] != null)
							throw new System.InvalidOperationException("Slottable at index " + i + " is not checked since it exceeds the max slot count");
				}
				for(int i = 0; i < allowedCount; i++){
					Slottable sb = sgCGears.Slots[i].Sb;
					if(sb != null)
						AE(sb.Item, checkedList[i]);
					else
						Assert.That(checkedList[i], Is.Null);
				}
				foreach(SlotGroup sgp in sgm.AllSGPs){
					if(sgp.Filter is SGNullFilter || sgp.Filter is SGCGearsFilter){
						foreach(Slottable sbp in sgp.Slottables){
							if(sbp != null){
								if(sbp.ItemInst is CarriedGearInstanceMock){
									if(checkedList.Contains((CarriedGearInstanceMock)sbp.ItemInst)){
										if(sgp == sgm.FocusedSGP){
											if(sgp.IsAutoSort){
												ASSB(sbp, SBDefocused, null, null, SBWFA, null, null, false);
												AB(sbp.IsEquipped, true);
											}else{
												ASSB(sbp, SBFocused, null, null, SBWFA, null, null, false);
												AB(sbp.IsEquipped, true);
											}
										}else{
											ASSB(sbp, SBDefocused, null, null, SBWFA, null, null, false);
											AB(sbp.IsEquipped, true);
										}
									}else{	/* deemed not equipped */
										if(sgp == sgm.FocusedSGP){
											ASSB(sbp, SBFocused, null, null, SBWFA, null, null, false);
											AB(sbp.IsEquipped, false);
										}else{
											ASSB(sbp, SBDefocused, null, null, SBWFA, null, null, false);
											AB(sbp.IsEquipped, false);
										}
									}
								}
							}
						}
					}
				}
			}
			public void AssertFocused(){
				ASSGM(sgm,
					SGMFocused,
					null,//prev state
					null,//ta
					null,//process
					null, /*null, null,*/
					true, true, true, true);
				foreach(SlotGroup sgp in sgm.AllSGPs){
					if(sgp == sgm.FocusedSGP){
						ASSG(sgp,
							null, SGFocused, null,
							null, SGWFA, null);
						foreach(Slottable sb in sgp.Slottables){
							if(sb != null){
								if(!sgp.IsAutoSort){
										ASSB_s(sb, SBFocused, SBWFA);
								}else{
									if(sb.Item is PartsInstanceMock && !(sgp.Filter is SGPartsFilter))
										ASSB_s(sb, SBDefocused, SBWFA);
									else
										if(sb.IsEquipped)
											ASSB_s(sb, SBDefocused, SBWFA);
										else{
											ASSB_s(sb, SBFocused, SBWFA);
										}
								}
							}
						}
					}else{/*  sgp not focused	*/
						ASSG(sgp,
							null, SGDefocused, null,
							null, SGWFA, null);
						foreach(Slottable sb in sgp.Slottables){
							if(sb != null){
								ASSB_s(sb, SBDefocused, SBWFA);
							}
						}
					}
				}
				foreach(SlotGroup sge in sgm.AllSGEs){
					if(sgm.FocusedSGEs.Contains(sge)){
						ASSG(sge,
							null, SGFocused, null,
							null, SGWFA, null);
						foreach(Slottable sbe in sge.Slottables){
							if(sbe != null){
								ASSB_s(sbe, SBFocused, SBWFA);
							}
						}
					}else{
						ASSG(sge,
							null, SGFocused, null,
							null, SGWFA, null);
						foreach(Slottable sbe in sge.Slottables){
							if(sbe != null){
								ASSB_s(sbe, SBDefocused, SBWFA);
							}
						}
					}
				}
			}
	/*	shortcut	*/
		/*	Debug	*/
			public string Name(SlotGroup sg){
				return Util.SGName(sg);
			}
			public string Name(Slottable sb){
				return Util.SBName(sb);
			}
			public string Bold(string str){
				return Util.Bold(str);
			}
			public string Red(string str){
				return Util.Red(str);
			}
			public string Blue(string str){
				return Util.Blue(str);
			}
			public string Green(string str){
				return Util.Green(str);
			}
			public string Ciel(string str){
				return Util.Ciel(str);
			}
			public string Aqua(string str){
				return Util.Aqua(str);
			}
			public string Forest(string str){
				return Util.Forest(str);
			}
			public string Brown(string str){
				return Util.Brown(str);
			}
			public string Terra(string str){
				return Util.Terra(str);
			}
			public string Berry(string str){
				return Util.Berry(str);
			}
			public string Violet(string str){
				return Util.Violet(str);
			}
		/*	SGM	*/
			SGMState SGMTransaction{
				get{return SlotGroupManager.PerformingTransactionState;}
			}
			SGMState SGMFocused{
				get{return SlotGroupManager.FocusedState;}
			}
			SGMState SGMDeactivated{
				get{return SlotGroupManager.DeactivatedState;}
			}
			SGMState SGMProbing{
				get{return SlotGroupManager.ProbingState;}
			}
		/*	SG	*/
			SGSelectionState SGFocused{
				get{return SlotGroup.FocusedState;}
			}
			SGSelectionState SGDefocused{
				get{return SlotGroup.DefocusedState;}
			}
			SGSelectionState SGDeactivated{
				get{return SlotGroup.DeactivatedState;}
			}
			SGSelectionState SGSelected{
				get{return SlotGroup.SelectedState;}
			}
			SGActionState SGWFA{
				get{return SlotGroup.WaitForActionState;}
			}
			SGActionState SGTransaction{
				get{return SlotGroup.PerformingTransactionState;}
			}
		/*	SB states shortcut	*/
			SBSelectionState SBFocused{
				get{return Slottable.FocusedState;}
			}
			SBSelectionState SBDefocused{
				get{return Slottable.DefocusedState;}
			}
			SBSelectionState SBDeactivated{
				get{return Slottable.DeactivatedState;}
			}
			SBSelectionState SBSelected{
				get{return Slottable.SelectedState;}
			}
			SBActionState SBWFA{
				get{return Slottable.WaitForActionState;}
			}
			SBActionState SBWFPickUp{
				get{return Slottable.WaitForPickUpState;}
			}
			SBActionState SBPickedUp{
				get{return Slottable.PickedUpState;}
			}
			SBActionState SBReverting{
				get{return Slottable.RevertingState;}
			}
			SBActionState SBMovingInSG{
				get{return Slottable.MovingInSGState;}
			}
			SBEquipState SBEquipped{
				get{return Slottable.EquippedState;}
			}
			SBEquipState SBUnequipped{
				get{return Slottable.UnequippedState;}
			}

	/*	dump	*/
		// public void TestReorderOnAll(){
			// 	PerformOnAllSBs(TestReorder);
			// }
			// public void TestReorder(Slottable sb, bool isAutoSort){
			// 	SlotGroup origSG = sb.SG;
			// 	if(!origSG.IsAutoSort){
			// 		foreach(Slottable other in origSG.Slottables){
			// 			string stacked;
			// 			if(other != null){
			// 				stacked = Util.SBName(other);
			// 				if(!InvalidSBs(sb, origSG).Contains(other)){
			// 					if(sb.IsPickable){
			// 						PickUp(sb, out picked);
			// 						SimHover(other, origSG, eventData);
			// 							AT<ReorderTransaction>(false);
			// 						PointerUp();
			// 							ASSGM_s(sgm, SGMTransaction);
			// 							AP<SGMActionProcess>(sgm, false);
			// 							AT<ReorderTransaction>(false);
			// 							AE(sgm.PickedSB, sb);
			// 							AE(sgm.SelectedSB, other);
			// 							AE(sgm.SelectedSG, origSG);
			// 							AE(sgm.PickedSBDoneTransaction, false);
			// 							AE(sgm.SelectedSBDoneTransaction, true);
			// 							AE(sgm.OrigSGDoneTransaction, true);
			// 							AE(sgm.SelectedSGDoneTransaction, false);
			// 							AssertSG(origSG, SGSelected, SGFocused, 2, typeof(SGHighlightProcess), true);
			// 						sb.ExpireProcess();
			// 						CompleteAllSBActProcesses(origSG);
			// 							AssertFocused();
			// 							AssertSG(origSG, SGFocused, SGSelected, 1, typeof(SGDehighlightProcess), false);
			// 					}
			// 				}else{
			// 					stacked = Util.Red(stacked);
			// 				}
			// 			}else{
			// 				stacked = Util.Red("null");
			// 			}
			// 			Util.Stack(stacked);
			// 		}
			// 	}
			// 	string origSGstr = Util.SGName(origSG);
			// 	if(origSG.IsAutoSort)
			// 		origSGstr = Util.Red(origSGstr);
			// 	string pickedStr = Util.SBName(sb);
			// 	string stackStr = Util.Stacked;
			// 	if(!sb.IsPickable){
			// 		pickedStr = Util.Red(pickedStr);
			// 		stackStr = "";
			// 	}
			// 	// Debug.Log(Utility.Bold("SG: ") + origSGstr + Utility.Bold(", Picked: ") + pickedStr + Utility.Bold(", hovered: ") + stackStr);
			// }

		// public void TestRevertOnAllSBs(){
			// 	PerformOnAllSBs(TestRevert);
			// }
			// public void TestRevert(Slottable sb, bool isPickedAS){
			// 	if(sb.CurState == SBFocused || sb.CurState == SBEADeselected){
			// 		SlotGroup origSG = sb.SG;
			// 			AssertFocused();
			// 		/*	release on spot	*/
			// 			PickUp(sb, out picked);
			// 				AB(picked, true);
			// 				AT<RevertTransaction>(false);
			// 			LetGo();
			// 				ASSGM_s(sgm, SGMTransaction);
			// 				AP<SGMActionProcess>(sgm, false);
			// 				AT<RevertTransaction>(false);
			// 					AE(sgm.PickedSB, sb);
			// 					AE(sgm.SelectedSB, sb);
			// 					AE(sgm.SelectedSG, origSG);
			// 					AE(sgm.PickedSBDoneTransaction, false);
			// 					AE(sgm.SelectedSBDoneTransaction, true);
			// 					AE(sgm.OrigSGDoneTransaction, true);
			// 					AE(sgm.SelectedSGDoneTransaction, true);
			// 					AssertSG(origSG, SGSelected, SGFocused, 1, typeof(SGHighlightProcess), false);
			// 			sb.ExpireProcess();
			// 				AssertFocused();
			// 				AssertSG(origSG, SGFocused, SGSelected, 1, typeof(SGDehighlightProcess), false);
			// 		/*	nullSB, origSG	*/
			// 			PickUp(sb, out picked);
			// 				AB(picked, true);
			// 				AT<RevertTransaction>(false);
			// 			SimHover(null, origSG, eventData);
			// 			LetGo();
			// 				ASSGM_s(sgm, SGMTransaction);
			// 				AP<SGMActionProcess>(sgm, false);
			// 				AT<RevertTransaction>(false);
			// 					AE(sgm.PickedSB, sb);
			// 					AE(sgm.SelectedSB, null);
			// 					AE(sgm.SelectedSG, origSG);
			// 					AE(sgm.PickedSBDoneTransaction, false);
			// 					AE(sgm.SelectedSBDoneTransaction, true);
			// 					AE(sgm.OrigSGDoneTransaction, true);
			// 					AE(sgm.SelectedSGDoneTransaction, true);
			// 					AssertSG(origSG, SGSelected, SGFocused, 1, typeof(SGHighlightProcess), false);
			// 			sb.ExpireProcess();
			// 				AssertFocused();
			// 				AssertSG(origSG, SGFocused, SGSelected, 1, typeof(SGDehighlightProcess), false);
			// 		/*	nullSB, nullSG	*/
			// 			PickUp(sb, out picked);
			// 				AB(picked, true);
			// 				AT<RevertTransaction>(false);
			// 			SimHover(null, null, eventData);
			// 			LetGo();
			// 				ASSGM_s(sgm, SGMTransaction);
			// 				AP<SGMActionProcess>(sgm, false);
			// 				AT<RevertTransaction>(false);
			// 					AE(sgm.PickedSB, sb);
			// 					AE(sgm.SelectedSB, null);
			// 					AE(sgm.SelectedSG, null);
			// 					AE(sgm.PickedSBDoneTransaction, false);
			// 					AE(sgm.SelectedSBDoneTransaction, true);
			// 					AE(sgm.OrigSGDoneTransaction, true);
			// 					AE(sgm.SelectedSGDoneTransaction, true);
			// 					AssertSG(origSG, SGFocused, SGSelected, 1, typeof(SGDehighlightProcess), false);
			// 			sb.ExpireProcess();
			// 				AssertFocused();
			// 				AssertSG(origSG, SGFocused, SGSelected, 1, typeof(SGDehighlightProcess), false);
			// 		/*	invalidSB, origSG	*/
			// 			foreach(Slottable invalidSB in InvalidSBs(sb, origSG)){
			// 				PickUp(sb, out picked);
			// 					AB(picked, true);
			// 					AT<RevertTransaction>(false);
			// 				SimHover(invalidSB, origSG, eventData);
			// 				LetGo();
			// 					ASSGM_s(sgm, SGMTransaction);
			// 					AP<SGMActionProcess>(sgm, false);
			// 					AT<RevertTransaction>(false);
			// 						AE(sgm.PickedSB, sb);
			// 						if(sb == invalidSB)
			// 							AE(sgm.SelectedSB, sb);
			// 						else
			// 							AE(sgm.SelectedSB, null);
			// 						AE(sgm.SelectedSG, origSG);
			// 						AE(sgm.PickedSBDoneTransaction, false);
			// 						AE(sgm.SelectedSBDoneTransaction, true);
			// 						AE(sgm.OrigSGDoneTransaction, true);
			// 						AE(sgm.SelectedSGDoneTransaction, true);
			// 						AssertSG(origSG, SGSelected, SGFocused, 1, typeof(SGHighlightProcess), false);
			// 				sb.ExpireProcess();
			// 					AssertFocused();
			// 					AssertSG(origSG, SGFocused, SGSelected, 1, typeof(SGDehighlightProcess), false);
			// 			}
			// 		/*	invalidSB, invalidSG	*/
			// 			foreach(SlotGroup invSG in InvalidSGs(sb)){
			// 				foreach(Slottable sbInInvSG in invSG.Slottables){
			// 					if(sbInInvSG != null){
			// 						if(InvalidSBs(sb, invSG).Contains(sbInInvSG)){
			// 							PickUp(sb, out picked);
			// 								AB(picked, true);
			// 								AT<RevertTransaction>(false);
			// 							SimHover(sbInInvSG, invSG, eventData);
			// 							LetGo();
			// 								ASSGM_s(sgm, SGMTransaction);
			// 								AP<SGMActionProcess>(sgm, false);
			// 								AT<RevertTransaction>(false);
			// 									AE(sgm.PickedSB, sb);
			// 									if(sb == sbInInvSG)
			// 										AE(sgm.SelectedSB, sb);
			// 									else
			// 										AE(sgm.SelectedSB, null);
			// 									AE(sgm.SelectedSG, null);
			// 									AE(sgm.PickedSBDoneTransaction, false);
			// 									AE(sgm.SelectedSBDoneTransaction, true);
			// 									AE(sgm.OrigSGDoneTransaction, true);
			// 									AE(sgm.SelectedSGDoneTransaction, true);
			// 									AssertSG(origSG, SGFocused, SGSelected, 1, typeof(SGDehighlightProcess), false);
			// 							sb.ExpireProcess();
			// 								AssertFocused();
			// 								AssertSG(origSG, SGFocused, SGSelected, 1, typeof(SGDehighlightProcess), false);
			// 						}
			// 					}
			// 				}
			// 			}
			// 	}
			// }
		// int aoCount = 0;
		// public void AssertOrder(Slottable sb){
		// 	if(sb != null)
		// 		AE(sb.SlotID, aoCount);
		// 	aoCount ++;
		// }
		// public void AssertOrderReset(){
		// 	aoCount = 0;
		// }
		// public void SwapEquipObsolete(Slottable pickedSB, Slottable hoveredSB){
		// 		AssertFocused();
		// 	SlotGroup origSG = sgm.GetSlotGroup(pickedSB);
		// 		ASSG(origSG, SlotGroup.FocusedState);
		// 	SlotGroup destSG = sgm.GetSlotGroup(hoveredSB);
		// 		ASSG(destSG, SlotGroup.FocusedState);
		// 		if(pickedSB.IsEquipped)
		// 			ASSB(pickedSB, Slottable.EquippedAndDeselectedState);
		// 		else
		// 			ASSB(pickedSB, Slottable.FocusedState);
		// 		if(hoveredSB.IsEquipped)
		// 			ASSB(hoveredSB, Slottable.EquippedAndDeselectedState);
		// 		else
		// 			ASSB(hoveredSB, Slottable.FocusedState);
				
		// 		AB(pickedSB.ItemInst == hoveredSB.ItemInst && pickedSB.ItemInst.IsStackable, false);
		// 		AB(origSG != destSG, true);

		// 	PickUp(pickedSB, out picked);
		// 	SimHover(hoveredSB,sgm.GetSlotGroup(hoveredSB), eventData);
		// 		AT<SwapTransaction>(false);
		// 		ASSGM(sgm, SlotGroupManager.ProbingState);
		// 		AP<SGMProbingStateProcess>(sgm, false);
		// 		ASSG(origSG, SlotGroup.FocusedState);
		// 		AP<SGDehighlightProcess>(origSG, false);
		// 		if(destSG.Filter is SGCarriedGearFilter && destSG.GetNextEmptySlot() == null){
		// 			ASSG(destSG, SlotGroup.DefocusedState);
		// 			AP<SGGreyoutProcess>(destSG, false);
		// 		}else{
		// 			ASSG(destSG, SlotGroup.SelectedState);
		// 			AP<SGHighlightProcess>(destSG, false);
		// 		}
		// 		ASSB(pickedSB, Slottable.PickedAndDeselectedState);
		// 		AP<SBDehighlightProcess>(pickedSB, false);
		// 		if(hoveredSB.IsEquipped)
		// 			ASSB(hoveredSB, Slottable.EquippedAndSelectedState);
		// 		else
		// 			ASSB(hoveredSB, Slottable.SelectedState);
		// 		AP<SBHighlightProcess>(hoveredSB, false);
		// 	pickedSB.OnPointerUpMock(eventData);
		// 		AT<SwapTransaction>(false);
		// 		ASSGM(sgm, SlotGroupManager.PerformingTransactionState);
		// 		AP<SGMActionProcess>(sgm, false);
		// 		if(origSG.IsAutoSort){
		// 			ASSG(origSG, SlotGroup.SortingState);
		// 			AP<SGSortingProcess>(origSG, false);
		// 		}else{	
		// 			ASSG(origSG, SlotGroup.FocusedState);
		// 			AP<SGDehighlightProcess>(origSG, false);
		// 		}
		// 		if(destSG.IsAutoSort){
		// 			ASSG(destSG, SlotGroup.SortingState);
		// 			AP<SGSortingProcess>(destSG, false);
		// 		}else{
		// 			if(destSG.Filter is SGCarriedGearFilter && destSG.GetNextEmptySlot() == null){
		// 				ASSG(destSG, SlotGroup.DefocusedState);
		// 				AP<SGGreyoutProcess>(destSG, false);
		// 			}else{
		// 				ASSG(destSG, SlotGroup.SelectedState);
		// 				AP<SGHighlightProcess>(destSG, false);
		// 			}
		// 		}
		// 		ASSB(pickedSB, Slottable.MovingState);
		// 		if(pickedSB.IsEquipped)
		// 			AP<SBRemovingProcess>(pickedSB, false);
		// 		else
		// 			AP<SBEquippingProcess>(pickedSB, false);
		// 		ASSB(hoveredSB, Slottable.MovingState);
		// 		if(hoveredSB.IsEquipped)
		// 			AP<SBRemovingProcess>(hoveredSB, false);
		// 		else
		// 			AP<SBEquippingProcess>(hoveredSB, false);
		// 	pickedSB.CurProcess.Expire();
		// 	hoveredSB.CurProcess.Expire();
		// 	if(origSG.IsAutoSort)
		// 		CompleteAllSBProcesses(origSG);
		// 	if(destSG.IsAutoSort)
		// 		CompleteAllSBProcesses(destSG);

		// 		AssertFocused();
		// }
		// public void SwapEquip(Slottable pickedSB){
		// 	if(pickedSB.ItemInst is BowInstanceMock || pickedSB.ItemInst is WearInstanceMock){
		// 		SlotGroup pickedSG = sgm.GetSlotGroup(pickedSB);
		// 		if(pickedSG.IsPool){
		// 			PickUp(pickedSB, out picked);
		// 			Slottable destSB = pickedSB.ItemInst is BowInstanceMock? sgBow.Slots[0].Sb: sgWear.Slots[0].Sb;
		// 			SlotGroup destSG = sgm.GetSlotGroup(destSB);
		// 			SimHover(destSB, destSG, eventData);
		// 			pickedSB.OnPointerUpMock(eventData);
		// 			pickedSB.CurProcess.Expire();
		// 			destSB.CurProcess.Expire();
		// 			if(pickedSG.IsAutoSort)
		// 				CompleteAllSBProcesses(pickedSG);
		// 			if(destSG.IsAutoSort)
		// 				CompleteAllSBProcesses(destSG);
		// 			AssertFocused();
		// 		}else
		// 			throw new System.InvalidOperationException("SwapEquip: need to specify destination SB in case picking up equipped");
		// 	}else
		// 		throw new System.InvalidOperationException("SwapEquip: need to specify destination SB in case switching CGears");
		// }
		// public void FillEquipObsolete(Slottable sb, SlotGroup destSG){
		// 	PickUp(sb, out picked);
		// 	SimHover(null, destSG, eventData);
		// 		if(destSG.IsPool)
		// 			AT<UnequipTransaction>(false);
		// 		else
		// 			AT<FillEquipTransaction>(false);
		// 		SlotGroup origSG = sgm.GetSlotGroup(sb);
		// 		SlotGroup oSG = null;
		// 	sb.OnPointerUpMock(eventData);
		// 		AE(sgm.PickedSBDoneTransaction, false);
		// 		AE(sgm.SelectedSBDoneTransaction, true);
		// 		if(!origSG.IsAutoSort){
		// 			ASSG(origSG, SlotGroup.FocusedState);
		// 			AP<SGDehighlightProcess>(origSG, false);
		// 		}else{
		// 			ASSG(origSG, SlotGroup.SortingState);
		// 			AP<SGSortingProcess>(origSG, false);
		// 			if(origSG.IsPool){
		// 				AB(origSG.CurProcess.IsExpired, true);
		// 				AE(sgm.OrigSGDoneTransaction, true);
		// 			}else{
		// 				if(!origSG.CurProcess.IsExpired)
		// 					oSG = origSG;
		// 			}
		// 		}
		// 		SlotGroup sSG = null;
		// 		if(!destSG.IsAutoSort){
		// 			ASSG(destSG, SlotGroup.SelectedState);
		// 			AP<SGHighlightProcess>(destSG, false);
		// 		}else{
		// 			ASSG(destSG, SlotGroup.SortingState);
		// 			AP<SGSortingProcess>(destSG, false);
		// 			if(destSG.IsPool){
		// 				AB(destSG.CurProcess.IsExpired, true);
		// 				AE(sgm.SelectedSGDoneTransaction, true);
		// 				sSG = null;
		// 			}else{
		// 				if(!destSG.CurProcess.IsExpired)
		// 					sSG = destSG;
		// 			}
		// 		}
		// 	ExpireProcesses(sb, null, oSG, sSG);
		// }
		// public void ExpireProcesses(Slottable pickedSB, Slottable selectedSB, SlotGroup origSG, SlotGroup selectedSG){
		// 	if(pickedSB != null)
		// 		if(pickedSB.CurProcess != null)
		// 			pickedSB.CurProcess.Expire();
		// 	if(selectedSB != null)
		// 		if(selectedSB.CurProcess != null)
		// 			selectedSB.CurProcess.Expire();
		// 	if(origSG != null)
		// 		if(origSG.CurProcess != null)
		// 			origSG.CurProcess.Expire();
		// 	if(selectedSG != null)
		// 		if(selectedSG.CurProcess != null)
		// 			selectedSG.CurProcess.Expire();
		// }
		// public void Revert(Slottable sb, out bool reverted){
		// 	if(sb.CurState == Slottable.PickedAndSelectedState || sb.CurState == Slottable.PickedAndDeselectedState){
		// 		sb.OnPointerUpMock(eventData);
		// 		if(sb.Item.IsStackable){
		// 			ASSB(sb, Slottable.WaitForNextTouchWhilePUState);
		// 			sb.CurProcess.Expire();
		// 		}
		// 			// AssertReverting(sb);
		// 		sb.CurProcess.Expire();
		// 		reverted = true;
		// 	}else{
		// 		reverted = false;
		// 	}
		// }
		// /*	Complex Transaction */
		// 	public void TestComplexTransaction(){
		// 			AssertFocused();
		// 		/*	sgpAll	*/
		// 			sgpAll.ToggleAutoSort(false);
		// 				AssertFocused();
		// 				AssertSGCounts(sgpAll, 14);
		// 				ASSBOReset();
		// 				ASSBO(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSBO(defBowB_p, Slottable.FocusedState);
		// 				ASSBO(crfBowA_p, Slottable.FocusedState);
		// 				ASSBO(defWearA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSBO(defWearB_p, Slottable.FocusedState);
		// 				ASSBO(crfWearA_p, Slottable.FocusedState);
		// 				ASSBO(defShieldA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSBO(crfShieldA_p, Slottable.FocusedState);
		// 				ASSBO(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSBO(crfMWeaponA_p, Slottable.FocusedState);
		// 				ASSBO(defQuiverA_p, Slottable.FocusedState);
		// 				ASSBO(defPackA_p, Slottable.FocusedState);
		// 				ASSBO(defParts_p, Slottable.FocusedState);
		// 				ASSBO(crfParts_p, Slottable.FocusedState);
		// 			/*	complex removal, AS off	*/
		// 				poolInv.RemoveItem(defBowB_p.ItemInst);
		// 				poolInv.RemoveItem(crfBowA_p.ItemInst);
		// 				SyncSBsOnAll();
		// 					AssertSGCounts(sgpAll, 14);
		// 					ATSBReset();
		// 					ATSB(defBowA_p, Slottable.MovingInSGState, 0);
		// 					ATSB(defBowB_p, Slottable.RemovedState, -1);
		// 					ATSB(crfBowA_p, Slottable.RemovedState, -1);
		// 					ATSB(defWearA_p, Slottable.MovingInSGState, 1);
		// 					ATSB(defWearB_p, Slottable.MovingInSGState, 2);
		// 					ATSB(crfWearA_p, Slottable.MovingInSGState, 3);
		// 					ATSB(defShieldA_p, Slottable.MovingInSGState, 4);
		// 					ATSB(crfShieldA_p, Slottable.MovingInSGState, 5);
		// 					ATSB(defMWeaponA_p, Slottable.MovingInSGState, 6);
		// 					ATSB(crfMWeaponA_p, Slottable.MovingInSGState, 7);
		// 					ATSB(defQuiverA_p, Slottable.MovingInSGState, 8);
		// 					ATSB(defPackA_p, Slottable.MovingInSGState, 9);
		// 					ATSB(defParts_p, Slottable.MovingInSGState, 10);
		// 					ATSB(crfParts_p, Slottable.MovingInSGState, 11);

		// 					AE(sgpAll.SlotMovements.Count, 14);
								
		// 					AE(sgm.PickedSBDoneTransaction, true);
		// 					AE(sgm.SelectedSBDoneTransaction, true);
		// 					AE(sgm.OrigSGDoneTransaction, true);
		// 					AE(sgm.SelectedSGDoneTransaction, false);
		// 					AE(sgm.Transaction.GetType(), typeof(ComplexTransaction));
		// 					AP<SGMActionProcess>(sgm, false);
		// 					ASSGM(sgm, SlotGroupManager.PerformingTransactionState);

		// 				CompleteAllSBProcesses(sgpAll);
		// 					AssertFocused();
		// 					AssertSGCounts(sgpAll, 12);
		// 						ASSBOReset();
		// 						ASSBO(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 						ASSBO(defWearA_p, Slottable.EquippedAndDeselectedState);
		// 						ASSBO(defWearB_p, Slottable.FocusedState);
		// 						ASSBO(crfWearA_p, Slottable.FocusedState);
		// 						ASSBO(defShieldA_p, Slottable.EquippedAndDeselectedState);
		// 						ASSBO(crfShieldA_p, Slottable.FocusedState);
		// 						ASSBO(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
		// 						ASSBO(crfMWeaponA_p, Slottable.FocusedState);
		// 						ASSBO(defQuiverA_p, Slottable.FocusedState);
		// 						ASSBO(defPackA_p, Slottable.FocusedState);
		// 						ASSBO(defParts_p, Slottable.FocusedState);
		// 						ASSBO(crfParts_p, Slottable.FocusedState);
		// 					AssertSGCounts(sgpBow, 1);
		// 					AssertSGCounts(sgpWear, 3);
		// 					AssertSGCounts(sgpCGears, 6);
		// 					AssertSGCounts(sgpParts, 2);
						
		// 			/*	complex addition, AS off*/
		// 				AssertFocused();
		// 				BowInstanceMock defBowB = new BowInstanceMock();
		// 				defBowB.Item = defBowA_p.ItemInst.Item;
		// 				BowInstanceMock defBowC = new BowInstanceMock();
		// 				defBowC.Item = defBowA_p.ItemInst.Item;
		// 				poolInv.AddItem(defBowB);
		// 				poolInv.AddItem(defBowC);
		// 				SyncSBsOnAll();
		// 					defBowB_p = sgpAll.GetSlottable(defBowB);
		// 					Slottable defBowC_p = sgpAll.GetSlottable(defBowC);
		// 					AB(sgpAll.GetSlotMovement(defBowB_p) != null, true);
		// 					AssertSGCounts(sgpAll, 12);
		// 						AE(sgpAll.SlotMovements.Count, 14);
		// 						ATSBReset();
		// 						ATSB(defBowA_p, Slottable.MovingInSGState, 0);
		// 						ATSB(defWearA_p, Slottable.MovingInSGState, 1);
		// 						ATSB(defWearB_p, Slottable.MovingInSGState, 2);
		// 						ATSB(crfWearA_p, Slottable.MovingInSGState, 3);
		// 						ATSB(defShieldA_p, Slottable.MovingInSGState, 4);
		// 						ATSB(crfShieldA_p, Slottable.MovingInSGState, 5);
		// 						ATSB(defMWeaponA_p, Slottable.MovingInSGState, 6);
		// 						ATSB(crfMWeaponA_p, Slottable.MovingInSGState, 7);
		// 						ATSB(defQuiverA_p, Slottable.MovingInSGState, 8);
		// 						ATSB(defPackA_p, Slottable.MovingInSGState, 9);
		// 						ATSB(defParts_p, Slottable.MovingInSGState, 10);
		// 						ATSB(crfParts_p, Slottable.MovingInSGState, 11);
		// 						ATSB(defBowB_p, Slottable.AddedState, 12);
		// 						ATSB(defBowC_p, Slottable.AddedState, 13);
		// 					AssertSGCounts(sgpBow, 3);
		// 					AssertSGCounts(sgpWear, 3);
		// 					AssertSGCounts(sgpCGears, 6);
		// 					AssertSGCounts(sgpParts, 2);
		// 				CompleteAllSBProcesses(sgpAll);
		// 					AssertSGCounts(sgpAll, 14);
		// 						ASSBOReset();
		// 						ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 						ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);
		// 						ASSB(defWearB_p, Slottable.FocusedState);
		// 						ASSB(crfWearA_p, Slottable.FocusedState);
		// 						ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);
		// 						ASSB(crfShieldA_p, Slottable.FocusedState);
		// 						ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
		// 						ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 						ASSB(defQuiverA_p, Slottable.FocusedState);
		// 						ASSB(defPackA_p, Slottable.FocusedState);
		// 						ASSB(defParts_p, Slottable.FocusedState);
		// 						ASSB(crfParts_p, Slottable.FocusedState);
		// 						ASSB(defBowB_p, Slottable.FocusedState);
		// 						ASSB(defBowC_p, Slottable.FocusedState);
		// 					AssertSGCounts(sgpBow, 3);
		// 					AssertSGCounts(sgpWear, 3);
		// 					AssertSGCounts(sgpCGears, 6);
		// 					AssertSGCounts(sgpParts, 2);
		// 			/*	complex addition and removal, AS off	*/
		// 				poolInv.RemoveItem(defWearB_p.ItemInst);
		// 				poolInv.RemoveItem(crfWearA_p.ItemInst);
		// 					QuiverMock crfQuiver = new QuiverMock();
		// 					crfQuiver.ItemID = 401;
		// 					QuiverInstanceMock crfQuiverA = new QuiverInstanceMock();
		// 					crfQuiverA.Item = crfQuiver;
		// 					PackMock crfPack = new PackMock();
		// 					crfPack.ItemID = 501;
		// 					PackInstanceMock crfPackA = new PackInstanceMock();
		// 					crfPackA.Item = crfPack;
		// 					WearInstanceMock defWearC = new WearInstanceMock();
		// 					defWearC.Item = defWearA_p.ItemInst.Item;
		// 					WearInstanceMock defWearD = new WearInstanceMock();
		// 					defWearD.Item = defWearA_p.ItemInst.Item;
		// 				poolInv.AddItem(crfQuiverA);
		// 				poolInv.AddItem(crfPackA);
		// 				poolInv.AddItem(defWearC);
		// 				poolInv.AddItem(defWearD);
		// 				SyncSBsOnAll();
		// 					AssertSGCounts(sgpAll, 14);
		// 					AE(sgpAll.SlotMovements.Count, 18);
		// 						Slottable crfQuiverA_p = sgpAll.GetSlottable(crfQuiverA);
		// 						Slottable crfPackA_p = sgpAll.GetSlottable(crfPackA);
		// 						Slottable defWearC_p = sgpAll.GetSlottable(defWearC);
		// 						Slottable defWearD_p = sgpAll.GetSlottable(defWearD);
		// 						ATSBReset();
		// 							ATSB(defBowA_p, Slottable.MovingInSGState, 0);
		// 							ATSB(defWearA_p, Slottable.MovingInSGState, 1);
		// 							ATSB(defWearB_p, Slottable.RemovedState, -1);//r
		// 							ATSB(crfWearA_p, Slottable.RemovedState, -1);//r
		// 							ATSB(defShieldA_p, Slottable.MovingInSGState, 2);
		// 							ATSB(crfShieldA_p, Slottable.MovingInSGState, 3);
		// 							ATSB(defMWeaponA_p, Slottable.MovingInSGState, 4);
		// 							ATSB(crfMWeaponA_p, Slottable.MovingInSGState, 5);
		// 							ATSB(defQuiverA_p, Slottable.MovingInSGState, 6);
		// 							ATSB(defPackA_p, Slottable.MovingInSGState, 7);
		// 							ATSB(defParts_p, Slottable.MovingInSGState, 8);
		// 							ATSB(crfParts_p, Slottable.MovingInSGState, 9);
		// 							ATSB(defBowB_p, Slottable.MovingInSGState, 10);
		// 							ATSB(defBowC_p, Slottable.MovingInSGState, 11);
		// 							ATSB(crfQuiverA_p, Slottable.AddedState, 12);
		// 							ATSB(crfPackA_p, Slottable.AddedState, 13);
		// 							ATSB(defWearC_p, Slottable.AddedState, 14);
		// 							ATSB(defWearD_p, Slottable.AddedState, 15);			
		// 					AssertSGCounts(sgpBow, 3);
		// 					AssertSGCounts(sgpWear, 3);
		// 					AssertSGCounts(sgpCGears, 8);
		// 					AssertSGCounts(sgpParts, 2);
		// 				CompleteAllSBProcesses(sgpAll);
		// 					AssertFocused();
		// 					AssertSGCounts(sgpAll, 16);
		// 						ASSBOReset();
		// 							ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 							ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);
		// 							ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);
		// 							ASSB(crfShieldA_p, Slottable.FocusedState);
		// 							ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
		// 							ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 							ASSB(defQuiverA_p, Slottable.FocusedState);
		// 							ASSB(defPackA_p, Slottable.FocusedState);
		// 							ASSB(defParts_p, Slottable.FocusedState);
		// 							ASSB(crfParts_p, Slottable.FocusedState);
		// 							ASSB(defBowB_p, Slottable.FocusedState);
		// 							ASSB(defBowC_p, Slottable.FocusedState);
		// 							ASSB(crfQuiverA_p, Slottable.FocusedState);
		// 							ASSB(crfPackA_p, Slottable.FocusedState);
		// 							ASSB(defWearC_p, Slottable.FocusedState);
		// 							ASSB(defWearD_p, Slottable.FocusedState);
		// 					AssertSGCounts(sgpBow, 3);
		// 						Slottable defBowA_p2 = sgpBow.GetSlottable(defBowA_p.ItemInst);
		// 						Slottable defBowB_p2 = sgpBow.GetSlottable(defBowB_p.ItemInst);
		// 						Slottable defBowC_p2 = sgpBow.GetSlottable(defBowC_p.ItemInst);
		// 						ASSBOReset();
		// 						ASSBO(defBowA_p2, Slottable.EquippedAndDefocusedState);
		// 						ASSBO(defBowB_p2, Slottable.DefocusedState);
		// 						ASSBO(defBowC_p2, Slottable.DefocusedState);
		// 					AssertSGCounts(sgpWear, 3);
		// 						Slottable defWearA_p2 = sgpWear.GetSlottable(defWearA_p.ItemInst);
		// 						Slottable defWearC_p2 = sgpWear.GetSlottable(defWearC_p.ItemInst);
		// 						Slottable defWearD_p2 = sgpWear.GetSlottable(defWearD_p.ItemInst);
		// 						ASSBOReset();
		// 						ASSBO(defWearA_p2, Slottable.EquippedAndDefocusedState);
		// 						ASSBO(defWearC_p2, Slottable.DefocusedState);
		// 						ASSBO(defWearD_p2, Slottable.DefocusedState);
		// 					AssertSGCounts(sgpCGears, 8);
		// 						Slottable defShieldA_p2 = sgpCGears.GetSlottable(defShieldA_p.ItemInst);
		// 						Slottable crfShieldA_p2 = sgpCGears.GetSlottable(crfShieldA_p.ItemInst);
		// 						Slottable defMWeaponA_p2 = sgpCGears.GetSlottable(defMWeaponA_p.ItemInst);
		// 						Slottable crfMWeaponA_p2 = sgpCGears.GetSlottable(crfMWeaponA_p.ItemInst);
		// 						Slottable defQuiverA_p2 = sgpCGears.GetSlottable(defQuiverA_p.ItemInst);
		// 						Slottable defPackA_p2 = sgpCGears.GetSlottable(defPackA_p.ItemInst);
		// 						Slottable crfQuiverA_p2 = sgpCGears.GetSlottable(crfQuiverA_p.ItemInst);
		// 						Slottable crfPackA_p2 = sgpCGears.GetSlottable(crfPackA_p.ItemInst);
		// 						ASSBOReset();
		// 						ASSB(defShieldA_p2, Slottable.EquippedAndDefocusedState);
		// 						ASSB(crfShieldA_p2, Slottable.DefocusedState);
		// 						ASSB(defMWeaponA_p2, Slottable.EquippedAndDefocusedState);
		// 						ASSB(crfMWeaponA_p2, Slottable.DefocusedState);
		// 						ASSB(defQuiverA_p2, Slottable.DefocusedState);
		// 						ASSB(crfQuiverA_p2, Slottable.DefocusedState);
		// 						ASSB(defPackA_p2, Slottable.DefocusedState);
		// 						ASSB(crfPackA_p2, Slottable.DefocusedState);
		// 					AssertSGCounts(sgpParts, 2);
		// 			/*	complex removal and addition, AS on	*/
		// 			sgpAll.ToggleAutoSort(true);
		// 				AssertFocused();
		// 					AssertSGCounts(sgpAll, 16);
		// 					ASSBOReset();
		// 						ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 						ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
		// 						ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 						ASSB(crfShieldA_p, Slottable.FocusedState);
		// 						ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 						ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 						ASSB(defQuiverA_p, Slottable.FocusedState);
		// 						ASSB(defPackA_p, Slottable.FocusedState);
		// 						ASSB(defParts_p, Slottable.DefocusedState);
		// 						ASSB(crfParts_p, Slottable.DefocusedState);
		// 						ASSB(defBowB_p, Slottable.FocusedState);
		// 						ASSB(defBowC_p, Slottable.FocusedState);
		// 						ASSB(crfQuiverA_p, Slottable.FocusedState);
		// 						ASSB(crfPackA_p, Slottable.FocusedState);
		// 						ASSB(defWearC_p, Slottable.FocusedState);
		// 						ASSB(defWearD_p, Slottable.FocusedState);
		// 					AssertSGCounts(sgpBow, 3);
		// 					AssertSGCounts(sgpWear, 3);
		// 					AssertSGCounts(sgpCGears, 8);
		// 					AssertSGCounts(sgpParts, 2);
		// 			poolInv.RemoveItem(defBowB_p.ItemInst);
		// 			poolInv.RemoveItem(defWearD_p.ItemInst);
		// 			poolInv.RemoveItem(defQuiverA_p.ItemInst);
		// 			poolInv.RemoveItem(defPackA_p.ItemInst);
		// 				BowMock crfBow = new BowMock();
		// 				crfBow.ItemID = 1;
		// 				BowInstanceMock crfBowA = new BowInstanceMock();
		// 				crfBowA.Item = crfBow;
		// 				WearMock crfWear = new WearMock();
		// 				crfWear.ItemID = 101;
		// 				WearInstanceMock crfWearA = new WearInstanceMock();
		// 				crfWearA.Item = crfWear;
		// 				QuiverInstanceMock crfQuiverB = new QuiverInstanceMock();
		// 				crfQuiverB.Item = crfQuiverA_p.ItemInst.Item;
		// 				PackInstanceMock crfPackB = new PackInstanceMock();
		// 				crfPackB.Item = crfPackA_p.ItemInst.Item;
		// 			poolInv.AddItem(crfBowA);
		// 			poolInv.AddItem(crfWearA);
		// 			poolInv.AddItem(crfQuiverB);
		// 			poolInv.AddItem(crfPackB);
		// 			SyncSBsOnAll();
		// 				AssertSGCounts(sgpAll, 16);
		// 					AE(sgpAll.SlotMovements.Count, 20);
		// 					crfBowA_p = sgpAll.GetSlottable(crfBowA);
		// 					crfWearA_p = sgpAll.GetSlottable(crfWearA);
		// 					Slottable crfQuiverB_p = sgpAll.GetSlottable(crfQuiverB);
		// 					Slottable crfPackB_p = sgpAll.GetSlottable(crfPackB);
		// 					ATSBReset();
		// 						ATSB(defBowA_p, Slottable.MovingInSGState, 0);
		// 						ATSB(defWearA_p, Slottable.MovingInSGState, 3);
		// 						ATSB(defShieldA_p, Slottable.MovingInSGState, 6);
		// 						ATSB(crfShieldA_p, Slottable.MovingInSGState, 7);
		// 						ATSB(defMWeaponA_p, Slottable.MovingInSGState, 8);
		// 						ATSB(crfMWeaponA_p, Slottable.MovingInSGState, 9);
		// 						ATSB(defQuiverA_p, Slottable.RemovedState, -1);//
		// 						ATSB(defPackA_p, Slottable.RemovedState, -1);//
		// 						ATSB(defParts_p, Slottable.MovingInSGState, 14);
		// 						ATSB(crfParts_p, Slottable.MovingInSGState, 15);
		// 						ATSB(defBowB_p, Slottable.RemovedState, -1);//
		// 						ATSB(defBowC_p, Slottable.MovingInSGState, 1);
		// 						ATSB(crfQuiverA_p, Slottable.MovingInSGState, 10);
		// 						ATSB(crfPackA_p, Slottable.MovingInSGState, 12);
		// 						ATSB(defWearC_p, Slottable.MovingInSGState, 4);
		// 						ATSB(defWearD_p, Slottable.RemovedState, -1);//
		// 						ATSB(crfBowA_p, Slottable.AddedState, 2);
		// 						ATSB(crfWearA_p, Slottable.AddedState, 5);
		// 						ATSB(crfQuiverB_p, Slottable.AddedState, 11);
		// 						ATSB(crfPackB_p, Slottable.AddedState, 13);
		// 				AssertSGCounts(sgpBow, 3);
		// 					Slottable crfBowA_p2 = sgpBow.GetSlottable(crfBowA);
		// 					ASSBOReset();
		// 					ASSBO(defBowA_p2, Slottable.EquippedAndDefocusedState);
		// 					ASSBO(defBowC_p2, Slottable.DefocusedState);
		// 					ASSBO(crfBowA_p2, Slottable.DefocusedState);
		// 				AssertSGCounts(sgpWear, 3);
		// 					Slottable crfWearA_p2 = sgpWear.GetSlottable(crfWearA);
		// 					ASSBOReset();
		// 					ASSBO(defWearA_p2, Slottable.EquippedAndDefocusedState);
		// 					ASSBO(defWearC_p2, Slottable.DefocusedState);
		// 					ASSBO(crfWearA_p2, Slottable.DefocusedState);
		// 				AssertSGCounts(sgpCGears, 8);
		// 					Slottable crfQuiverB_p2 = sgpCGears.GetSlottable(crfQuiverB_p.ItemInst);
		// 					Slottable crfPackB_p2 = sgpCGears.GetSlottable(crfPackB_p.ItemInst);
		// 					ASSBOReset();
		// 					ASSBO(defShieldA_p2, Slottable.EquippedAndDefocusedState);
		// 					ASSBO(crfShieldA_p2, Slottable.DefocusedState);
		// 					ASSBO(defMWeaponA_p2, Slottable.EquippedAndDefocusedState);
		// 					ASSBO(crfMWeaponA_p2, Slottable.DefocusedState);
		// 					ASSBO(crfQuiverA_p2, Slottable.DefocusedState);
		// 					ASSBO(crfQuiverB_p2, Slottable.DefocusedState);
		// 					ASSBO(crfPackA_p2, Slottable.DefocusedState);
		// 					ASSBO(crfPackB_p2, Slottable.DefocusedState);
		// 				AssertSGCounts(sgpParts, 2);
		// 			CompleteAllSBProcesses(sgpAll);
		// 				AssertFocused();
		// 				AssertSGCounts(sgpAll, 16);
		// 					ASSBOReset();
		// 					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);//0
		// 					ASSB(defBowC_p, Slottable.FocusedState);//1
		// 					ASSB(crfBowA_p, Slottable.FocusedState);//2
		// 					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);//3
		// 					ASSB(defWearC_p, Slottable.FocusedState);//4
		// 					ASSB(crfWearA_p, Slottable.FocusedState);//5
		// 					ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);//6
		// 					ASSB(crfShieldA_p, Slottable.FocusedState);//7
		// 					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);//8
		// 					ASSB(crfMWeaponA_p, Slottable.FocusedState);//9
		// 					ASSB(crfQuiverA_p, Slottable.FocusedState);//10
		// 					ASSB(crfQuiverB_p, Slottable.FocusedState);//11
		// 					ASSB(crfPackA_p, Slottable.FocusedState);//12
		// 					ASSB(crfPackB_p, Slottable.FocusedState);//13
		// 					ASSB(defParts_p, Slottable.DefocusedState);//14
		// 					ASSB(crfParts_p, Slottable.DefocusedState);//15
		// 				AssertSGCounts(sgpBow, 3);
		// 				AssertSGCounts(sgpWear, 3);
		// 				AssertSGCounts(sgpCGears, 8);
		// 				AssertSGCounts(sgpParts, 2);
		// 		/*	sgpBow	*/
		// 			/*	AS off	*/
		// 				sgm.SetFocusedPoolSG(sgpBow);
		// 				sgpBow.ToggleAutoSort(false);
		// 				AssertFocused();
		// 					AssertSGCounts(sgpBow, 3);
		// 						defBowA_p2 = sgpBow.GetSlottable(defBowA_p.ItemInst);
		// 						defBowC_p2 = sgpBow.GetSlottable(defBowC_p.ItemInst);
		// 						crfBowA_p2 = sgpBow.GetSlottable(crfBowA_p.ItemInst);
		// 						ASSBOReset();
		// 						ASSBO(defBowA_p2, Slottable.EquippedAndDeselectedState);
		// 						ASSBO(defBowC_p2, Slottable.FocusedState);
		// 						ASSBO(crfBowA_p2, Slottable.FocusedState);
		// 					AssertSGCounts(sgpAll, 16);
		// 					AssertSGCounts(sgpWear, 3);
		// 					AssertSGCounts(sgpCGears, 8);
		// 					AssertSGCounts(sgpParts, 2);
		// 				/*	removal and add	*/
		// 					poolInv.RemoveItem(defBowC_p2.ItemInst);
		// 					poolInv.RemoveItem(crfBowA_p2.ItemInst);
		// 						BowInstanceMock defBowD = new BowInstanceMock();
		// 						defBowD.Item = defBowA_p.ItemInst.Item;
		// 						BowInstanceMock defBowE = new BowInstanceMock();
		// 						defBowE.Item = defBowA_p.ItemInst.Item;
		// 						BowInstanceMock crfBowB = new BowInstanceMock();
		// 						crfBowB.Item = crfBowA_p.ItemInst.Item;
		// 					poolInv.AddItem(defBowD);
		// 					poolInv.AddItem(defBowE);
		// 					poolInv.AddItem(crfBowB);
		// 					SyncSBsOnAll();
		// 						AssertSGCounts(sgpBow, 3);
		// 							AE(sgpBow.SlotMovements.Count, 6);
		// 							Slottable defBowD_p2 = sgpBow.GetSlottable(defBowD);
		// 							Slottable defBowE_p2 = sgpBow.GetSlottable(defBowE);
		// 							Slottable crfBowB_p2 = sgpBow.GetSlottable(crfBowB);
		// 							ATSBReset();
		// 							ATSB(defBowA_p2, Slottable.MovingInSGState, 0);
		// 							ATSB(defBowC_p2, Slottable.RemovedState, -1);
		// 							ATSB(crfBowA_p2, Slottable.RemovedState, -1);
		// 							ATSB(defBowD_p2, Slottable.AddedState, 1);
		// 							ATSB(defBowE_p2, Slottable.AddedState, 2);
		// 							ATSB(crfBowB_p2, Slottable.AddedState, 3);
		// 						AssertSGCounts(sgpAll, 17);
		// 							Slottable defBowD_p = sgpAll.GetSlottable(defBowD);
		// 							Slottable defBowE_p = sgpAll.GetSlottable(defBowE);
		// 							Slottable crfBowB_p = sgpAll.GetSlottable(crfBowB);
		// 							ASSBOReset();
		// 							ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);//0
		// 							ASSB(defBowD_p, Slottable.DefocusedState);//0
		// 							ASSB(defBowE_p, Slottable.DefocusedState);//0
		// 							ASSB(crfBowB_p, Slottable.DefocusedState);//0
		// 							ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);//3
		// 							ASSB(defWearC_p, Slottable.DefocusedState);//4
		// 							ASSB(crfWearA_p, Slottable.DefocusedState);//5
		// 							ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);//6
		// 							ASSB(crfShieldA_p, Slottable.DefocusedState);//7
		// 							ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);//8
		// 							ASSB(crfMWeaponA_p, Slottable.DefocusedState);//9
		// 							ASSB(crfQuiverA_p, Slottable.DefocusedState);//10
		// 							ASSB(crfQuiverB_p, Slottable.DefocusedState);//11
		// 							ASSB(crfPackA_p, Slottable.DefocusedState);//12
		// 							ASSB(crfPackB_p, Slottable.DefocusedState);//13
		// 							ASSB(defParts_p, Slottable.DefocusedState);//14
		// 							ASSB(crfParts_p, Slottable.DefocusedState);//1		
		// 						AssertSGCounts(sgpWear, 3);
		// 						AssertSGCounts(sgpCGears, 8);
		// 						AssertSGCounts(sgpParts, 2);
		// 					CompleteAllSBProcesses(sgpBow);
		// 						AssertFocused();
		// 							AssertSGCounts(sgpBow, 4);
		// 								ASSBOReset();
		// 								ASSBO(defBowA_p2, Slottable.EquippedAndDeselectedState);
		// 								ASSBO(defBowD_p2, Slottable.FocusedState);
		// 								ASSBO(defBowE_p2, Slottable.FocusedState);
		// 								ASSBO(crfBowB_p2, Slottable.FocusedState);
		// 							AssertSGCounts(sgpAll, 17);
		// 							AssertSGCounts(sgpWear, 3);
		// 							AssertSGCounts(sgpCGears, 8);
		// 							AssertSGCounts(sgpParts, 2);
		// 			/*	AS on	*/
		// 				sgpBow.ToggleAutoSort(true);
		// 					AssertFocused();
		// 						ASSBOReset();
		// 						ASSBO(defBowA_p2, Slottable.EquippedAndDefocusedState);
		// 						ASSBO(defBowD_p2, Slottable.FocusedState);
		// 						ASSBO(defBowE_p2, Slottable.FocusedState);
		// 						ASSBO(crfBowB_p2, Slottable.FocusedState);
		// 				/*	removal and addition	*/
		// 					poolInv.RemoveItem(defBowD_p2.ItemInst);
		// 					poolInv.RemoveItem(defBowE_p2.ItemInst);
		// 						BowInstanceMock crfBowC = new BowInstanceMock();
		// 						crfBowC.Item = crfBowB_p2.ItemInst.Item;
		// 					poolInv.AddItem(crfBowC);
		// 						BowInstanceMock defBowF = new BowInstanceMock();
		// 						defBowF.Item = defBowA_p2.ItemInst.Item;
		// 					poolInv.AddItem(defBowF);
		// 						BowInstanceMock crfBowD = new BowInstanceMock();
		// 						crfBowD.Item = crfBowB_p2.ItemInst.Item;
		// 					poolInv.AddItem(crfBowD);
		// 					SyncSBsOnAll();
		// 						AssertSGCounts(sgpBow, 4);
		// 							AE(sgpBow.SlotMovements.Count, 7);
		// 							Slottable crfBowC_p2 = sgpBow.GetSlottable(crfBowC);
		// 							Slottable defBowF_p2 = sgpBow.GetSlottable(defBowF);
		// 							Slottable crfBowD_p2 = sgpBow.GetSlottable(crfBowD);
		// 							ATSBReset();
		// 							ATSB(defBowA_p2, Slottable.MovingInSGState, 0);
		// 							ATSB(defBowD_p2, Slottable.RemovedState, -1);
		// 							ATSB(defBowE_p2, Slottable.RemovedState, -1);
		// 							ATSB(crfBowB_p2, Slottable.MovingInSGState, 2);
		// 							ATSB(crfBowC_p2, Slottable.AddedState, 3);
		// 							ATSB(defBowF_p2, Slottable.AddedState, 1);
		// 							ATSB(crfBowD_p2, Slottable.AddedState, 4);
		// 						AssertSGCounts(sgpAll, 18);
		// 						AssertSGCounts(sgpWear, 3);
		// 						AssertSGCounts(sgpCGears, 8);
		// 						AssertSGCounts(sgpParts, 2);
		// 					CompleteAllSBProcesses(sgpBow);
		// 						AssertFocused();
		// 						AssertSGCounts(sgpBow, 5);
		// 							ASSBOReset();
		// 							ASSBO(defBowA_p2, Slottable.EquippedAndDefocusedState);
		// 							ASSBO(defBowF_p2, Slottable.FocusedState);
		// 							ASSBO(crfBowB_p2, Slottable.FocusedState);
		// 							ASSBO(crfBowC_p2, Slottable.FocusedState);
		// 							ASSBO(crfBowD_p2, Slottable.FocusedState);
		// 						AssertSGCounts(sgpAll, 18);
		// 						AssertSGCounts(sgpWear, 3);
		// 						AssertSGCounts(sgpCGears, 8);
		// 						AssertSGCounts(sgpParts, 2);
		// 			/**/
		// 		}
			
		// 	public void InstantSyncSBsOnAllSGs(){
		// 		foreach(SlotSystemElement ele in sgm.RootPage.PoolBundle.Elements){
		// 			SlotGroup sg = (SlotGroup)ele;
		// 			InstantSyncSBs(sg);
		// 		}
		// 	}
		// 	public void InstantSyncSBs(SlotGroup sg){
		// 		List<InventoryItemInstanceMock> addedItemInsts = addedItemInvList(sg);
		// 		List<InventoryItemInstanceMock> removedItemInsts = removedItemInvList(sg);
				
		// 		List<Slottable> result = sg.Slottables;
		// 		SpotRemoveSBs(removedItemInsts, ref result);
		// 		ScoochSBs(ref result);
		// 		AddSBs(addedItemInsts, ref result, sg);
		// 		if(sg.IsAutoSort)
		// 			OrderSBs(SlotGroup.ItemIDSorter, ref result);
		// 		UpdateSBs(sg, result);
		// 		sgm.Focus();
		// 	}
		// 	public void SyncSBsOnAll(){
		// 		SlotGroup focusedSG = sgm.FocusedSGP;
		// 		foreach(SlotSystemElement ele in sgm.RootPage.PoolBundle.Elements){
		// 			SlotGroup sG = (SlotGroup)ele;
		// 			// if(sG == sg)
		// 			// 	SyncSBs(sG);
		// 			// else
		// 			// 	InstantSyncSBs(sG);
		// 			if(sG != focusedSG)
		// 				InstantSyncSBs(sG);
		// 		}
		// 		SyncSBs(focusedSG);
		// 	}
		// 	public void SyncSBs(SlotGroup sg){
		// 		List<InventoryItemInstanceMock> addedItemInsts = addedItemInvList(sg);
		// 		List<InventoryItemInstanceMock> removedItemInsts = removedItemInvList(sg);
				
		// 		ComplexTransaction complexTs = new ComplexTransaction(removedItemInsts, addedItemInsts, sg);
		// 		sg.SGM.SetTransaction(complexTs);
		// 		sg.SGM.Transaction.Execute();
		// 		/*	
		// 			those to be removed
		// 				=> transaction from cur to -1
		// 			those to be added
		// 				=> transaction from -1 to new
		// 			else
		// 				=> cur to new
		// 		*/
		// 		// UpdateSBs(sg, result);
		// 		// sgm.Focus();
		// 	}
		// 	public void SpotRemoveSBs(List<InventoryItemInstanceMock> removed, ref List<Slottable> sbs){
		// 		List<Slottable> result = new List<Slottable>();
		// 		foreach(Slottable sb in sbs){
		// 			if(sb != null){
		// 				if(removed.Contains(sb.ItemInst))
		// 					result.Add(null);
		// 				else
		// 					result.Add(sb);
		// 			}else
		// 				result.Add(null);
		// 		}
		// 		sbs = result;
		// 	}
		// 	public void ScoochSBs(ref List<Slottable> sbs){
		// 		List<Slottable> temp = new List<Slottable>();
		// 		foreach(Slottable sb in sbs){
		// 			if(sb != null)
		// 				temp.Add(sb);
		// 		}
		// 		for(int i = 0; i < sbs.Count; i ++){
		// 			if(i < temp.Count)
		// 				sbs[i] = temp[i];
		// 			else
		// 				sbs[i] = null;
		// 		}
		// 	}
		// 	public void AddSBs(List<InventoryItemInstanceMock> added, ref List<Slottable> sbs, SlotGroup sg){
		// 		/*	find an empty index and put it there in order
		// 		*/
		// 		foreach(InventoryItemInstanceMock itemInst in added){
		// 			GameObject newSBGO = new GameObject("newSBGO");
		// 			Slottable newSB = newSBGO.AddComponent<Slottable>();
		// 			newSB.Initialize(sg.SGM, true, itemInst);
		// 			// if(sg.CurState == SlotGroup.FocusedState)
		// 			// 	newSB.Focus();
		// 			// else if(sg.CurState == SlotGroup.DefocusedState)
		// 			// 	newSB.Defocus();

		// 			int emptyIndex = -1;
		// 			foreach(Slottable sb in sbs){
		// 				if(sb == null){
		// 					emptyIndex = sbs.IndexOf(sb);
		// 					break;
		// 				}
		// 			}
		// 			if(emptyIndex != -1)
		// 				sbs[emptyIndex] = newSB;
		// 			else{
		// 				sbs.Add(newSB);
		// 			}
		// 		}
		// 	}
		// 	public void OrderSBs(SGSorter sorter, ref List<Slottable> sbs){
		// 		sorter.OrderSBs(ref sbs);
		// 	}
		// 	public void UpdateSBs(SlotGroup sg, List<Slottable> sbs){
		// 		foreach(Slot slot in sg.Slots){
		// 			slot.Sb = null;
		// 		}
		// 		for(int i = 0; i < sbs.Count; i ++){
		// 			if(i < sg.Slots.Count)
		// 				sg.Slots[i].Sb = sbs[i];
		// 			else{
		// 				Slot newSlot = new Slot();
		// 				newSlot.Position = Vector2.zero;
		// 				newSlot.Sb = sbs[i];
		// 				sg.Slots.Add(newSlot);
		// 			}
		// 		}
		// 		if(sg.IsExpandable){
		// 			List<Slot> temp = new List<Slot>();
		// 			foreach(Slot slot in sg.Slots){
		// 				if(slot.Sb != null)
		// 					temp.Add(slot);
		// 			}
		// 			sg.Slots = temp;
		// 		}
		// 	}
		// 	public void RemoveSlottables(ref SlotGroup sg, List<InventoryItemInstanceMock> removedList){
		// 		if(removedList.Count > 0){
		// 			foreach(Slot slot in sg.Slots){
		// 				if(slot.Sb != null){
		// 					if(removedList.Contains(slot.Sb.ItemInst)){
		// 						Slottable sb = slot.Sb;
		// 						GameObject go = sb.gameObject;
		// 						Object.DestroyImmediate(sb);
		// 						Object.DestroyImmediate(go);
		// 						slot.Sb = null;
		// 					}
		// 				}
		// 			}
		// 			/*	tidy up the slots
		// 					if IsExpandable (not IsShrinkable, as it pertains to the sg's vacuum aversion, like when deciding whether to dislodge or swap)
		// 						=> remove slots
		// 					else
		// 						leave it be
		// 				if want to sort do so independently
		// 			*/
		// 				if(sg.IsExpandable){
		// 					List<Slot> scooched = new List<Slot>();
		// 					foreach(Slot slot in sg.Slots){
		// 						if(slot.Sb != null)
		// 							scooched.Add(slot);
		// 					}
		// 					sg.Slots = scooched;
		// 				}else{
		// 					List<Slottable> sbs = sg.Slottables;
		// 					foreach(Slot slot in sg.Slots){
		// 						slot.Sb = null;
		// 					}
		// 					for(int i = 0; i < sbs.Count; i++){
		// 						sg.Slots[i].Sb = sbs[i];
		// 					}
		// 				}
		// 			/**/
		// 		}
		// 	}
		// 	public void AddSlottables(ref SlotGroup sg, List<InventoryItemInstanceMock> addedList){
		// 		foreach(InventoryItemInstanceMock itemInst in addedList){
		// 			/*	Create Slottable	*/
		// 			GameObject newSBGO = new GameObject("newSBGO");
		// 			Slottable newSB = newSBGO.AddComponent<Slottable>();
		// 			newSB.Initialize(sg.SGM, true, itemInst);
		// 			/*	Assign to a Slot	*/
		// 			Slot newSlot = sg.GetNextEmptySlot();
		// 			if(newSlot != null)
		// 				newSlot.Sb = newSB;
					
		// 			if(sg.CurState == SlotGroup.FocusedState)
		// 				newSB.Focus();
		// 			else if(sg.CurState == SlotGroup.DefocusedState)
		// 				newSB.Defocus();
		// 		}
		// 	}
		// 	List<InventoryItemInstanceMock> removedItemInvList(SlotGroup sg){
		// 		List<InventoryItemInstanceMock> removed = new List<InventoryItemInstanceMock>();
		// 		foreach(Slottable sb in sg.Slottables){
		// 			bool found = poolInv.Items.Contains(sb.ItemInst);
		// 			if(!found)
		// 				removed.Add(sb.ItemInst);
		// 		}
		// 		return removed;
		// 	}
		// 	List<InventoryItemInstanceMock> addedItemInvList(SlotGroup sg){
		// 		List<InventoryItemInstanceMock> added = new List<InventoryItemInstanceMock>();
		// 		foreach(InventoryItemInstanceMock itemInst in poolInv.Items){
		// 			bool found = sg.ItemInstances.Contains(itemInst);
		// 			if(!found)
		// 				added.Add(itemInst);
		// 		}
		// 		return sg.Filter.filteredItemInstances(added);
		// 	}
		// 	public void TestAcquisitionOrder(){
		// 		AE(sgpAll.Inventory.Items.Count, 14);
		// 		AssertAcquisitionOrderReset();
		// 		AssertAcquisitionOrder(defBowA_p);
		// 		AssertAcquisitionOrder(defBowB_p);
		// 		AssertAcquisitionOrder(crfBowA_p);
		// 		AssertAcquisitionOrder(defWearA_p);
		// 		AssertAcquisitionOrder(defWearB_p);
		// 		AssertAcquisitionOrder(crfWearA_p);
		// 		AssertAcquisitionOrder(defParts_p);
		// 		AssertAcquisitionOrder(crfParts_p);
		// 		AssertAcquisitionOrder(defShieldA_p);
		// 		AssertAcquisitionOrder(crfShieldA_p);
		// 		AssertAcquisitionOrder(defMWeaponA_p);
		// 		AssertAcquisitionOrder(crfMWeaponA_p);
		// 		AssertAcquisitionOrder(defQuiverA_p);
		// 		AssertAcquisitionOrder(defPackA_p);
		// 	}
		// 	int acqOrder = 0;
		// 	public void AssertAcquisitionOrderReset(){
		// 		acqOrder = 0;
		// 	}
		// 	public void AssertAcquisitionOrder(Slottable sb){
		// 		AE(sb.ItemInst.AcquisitionOrder, acqOrder);
		// 		acqOrder ++;
		// 	}
		// 	public void TestPoolSGs(){
		// 		Slottable defBowA_p2 = sgpBow.GetSlottable(defBowA_p.Item);
		// 		Slottable defBowB_p2 = sgpBow.GetSlottable(defBowB_p.Item);
		// 		Slottable crfBowA_p2 = sgpBow.GetSlottable(crfBowA_p.Item);
		// 		Slottable defWearA_p2 = sgpWear.GetSlottable(defWearA_p.Item);
		// 		Slottable defWearB_p2 = sgpWear.GetSlottable(defWearB_p.Item);
		// 		Slottable crfWearA_p2 = sgpWear.GetSlottable(crfWearA_p.Item);
				
		// 		AE(sgm.FocusedSGP, sgpAll);
		// 		AB(sgpAll.IsAutoSort, true);
		// 		AB(sgpBow.IsAutoSort, true);
		// 		AB(sgpWear.IsAutoSort, true);
		// 		AB(sgpParts.IsAutoSort, true);
		// 		/**/
		// 		ASSG(sgpAll, SlotGroup.FocusedState);
		// 			ASSBOReset();
		// 			ASSBO(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSBO(defBowB_p, Slottable.FocusedState);
		// 			ASSBO(crfBowA_p, Slottable.FocusedState);
		// 			ASSBO(defWearA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSBO(defWearB_p, Slottable.FocusedState);
		// 			ASSBO(crfWearA_p, Slottable.FocusedState);
		// 			ASSBO(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSBO(crfShieldA_p, Slottable.FocusedState);
		// 			ASSBO(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSBO(crfMWeaponA_p, Slottable.FocusedState);
		// 			ASSBO(defQuiverA_p, Slottable.FocusedState);
		// 			ASSBO(defPackA_p, Slottable.FocusedState);
		// 			ASSBO(defParts_p, Slottable.DefocusedState);
		// 			ASSBO(crfParts_p, Slottable.DefocusedState);
					
		// 		ASSG(sgpBow, SlotGroup.DefocusedState);
		// 			ASSBOReset();
		// 			ASSBO(defBowA_p2, Slottable.EquippedAndDefocusedState);
		// 			ASSBO(defBowB_p2, Slottable.DefocusedState);
		// 			ASSBO(crfBowA_p2, Slottable.DefocusedState);
					
		// 		ASSG(sgpWear, SlotGroup.DefocusedState);
		// 			ASSBOReset();
		// 			ASSBO(defWearA_p2, Slottable.EquippedAndDefocusedState);
		// 			ASSBO(defWearB_p2, Slottable.DefocusedState);
		// 			ASSBO(crfWearA_p2, Slottable.DefocusedState);
					
		// 		ASSG(sgpParts, SlotGroup.DefocusedState);
		// 			ASSBOReset();
		// 			ASSBO(defParts_p2, Slottable.DefocusedState);
		// 			ASSBO(crfParts_p2, Slottable.DefocusedState);
		// 		/**/
		// 		sgpAll.ToggleAutoSort(false);
		// 		ASSG(sgpAll, SlotGroup.FocusedState);
		// 			ASSBOReset();
		// 			ASSBO(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 			ASSBO(defBowB_p, Slottable.FocusedState);
		// 			ASSBO(crfBowA_p, Slottable.FocusedState);
		// 			ASSBO(defWearA_p, Slottable.EquippedAndDeselectedState);
		// 			ASSBO(defWearB_p, Slottable.FocusedState);
		// 			ASSBO(crfWearA_p, Slottable.FocusedState);
		// 			ASSBO(defShieldA_p, Slottable.EquippedAndDeselectedState);
		// 			ASSBO(crfShieldA_p, Slottable.FocusedState);
		// 			ASSBO(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
		// 			ASSBO(crfMWeaponA_p, Slottable.FocusedState);
		// 			ASSBO(defQuiverA_p, Slottable.FocusedState);
		// 			ASSBO(defPackA_p, Slottable.FocusedState);
		// 			ASSBO(defParts_p, Slottable.FocusedState);
		// 			ASSBO(crfParts_p, Slottable.FocusedState);
		// 		ASSG(sgpBow, SlotGroup.DefocusedState);
		// 			ASSBOReset();
		// 			ASSBO(defBowA_p2, Slottable.EquippedAndDefocusedState);
		// 			ASSBO(defBowB_p2, Slottable.DefocusedState);
		// 			ASSBO(crfBowA_p2, Slottable.DefocusedState);
		// 		ASSG(sgpWear, SlotGroup.DefocusedState);
		// 			ASSBOReset();
		// 			ASSBO(defWearA_p2, Slottable.EquippedAndDefocusedState);
		// 			ASSBO(defWearB_p2, Slottable.DefocusedState);
		// 			ASSBO(crfWearA_p2, Slottable.DefocusedState);
		// 		ASSG(sgpParts, SlotGroup.DefocusedState);
		// 			ASSBOReset();
		// 			ASSBO(defParts_p2, Slottable.DefocusedState);
		// 			ASSBO(crfParts_p2, Slottable.DefocusedState);
		// 		sgpAll.ToggleAutoSort(true);
		// 		/**/
		// 		sgm.SetFocusedPoolSG(sgpBow);
		// 			ASSG(sgpAll, SlotGroup.DefocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defBowB_p, Slottable.DefocusedState);
		// 				ASSBO(crfBowA_p, Slottable.DefocusedState);
		// 				ASSBO(defWearA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defWearB_p, Slottable.DefocusedState);
		// 				ASSBO(crfWearA_p, Slottable.DefocusedState);
		// 				ASSBO(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(crfShieldA_p, Slottable.DefocusedState);
		// 				ASSBO(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(crfMWeaponA_p, Slottable.DefocusedState);
		// 				ASSBO(defQuiverA_p, Slottable.DefocusedState);
		// 				ASSBO(defPackA_p, Slottable.DefocusedState);
		// 				ASSBO(defParts_p, Slottable.DefocusedState);
		// 				ASSBO(crfParts_p, Slottable.DefocusedState);
		// 			ASSG(sgpBow, SlotGroup.FocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defBowA_p2, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defBowB_p2, Slottable.FocusedState);
		// 				ASSBO(crfBowA_p2, Slottable.FocusedState);
		// 			ASSG(sgpWear, SlotGroup.DefocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defWearA_p2, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defWearB_p2, Slottable.DefocusedState);
		// 				ASSBO(crfWearA_p2, Slottable.DefocusedState);
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defParts_p2, Slottable.DefocusedState);
		// 				ASSBO(crfParts_p2, Slottable.DefocusedState);

		// 		/**/
		// 		sgpBow.ToggleAutoSort(false);
		// 		ASSG(sgpBow, SlotGroup.FocusedState);
		// 			ASSBOReset();
		// 			ASSBO(defBowA_p2, Slottable.EquippedAndDeselectedState);
		// 			ASSBO(defBowB_p2, Slottable.FocusedState);
		// 			ASSBO(crfBowA_p2, Slottable.FocusedState);
		// 		sgpBow.ToggleAutoSort(true);
		// 		/**/
		// 		sgm.SetFocusedPoolSG(sgpWear);
		// 			ASSG(sgpAll, SlotGroup.DefocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defBowB_p, Slottable.DefocusedState);
		// 				ASSBO(crfBowA_p, Slottable.DefocusedState);
		// 				ASSBO(defWearA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defWearB_p, Slottable.DefocusedState);
		// 				ASSBO(crfWearA_p, Slottable.DefocusedState);
		// 				ASSBO(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(crfShieldA_p, Slottable.DefocusedState);
		// 				ASSBO(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(crfMWeaponA_p, Slottable.DefocusedState);
		// 				ASSBO(defQuiverA_p, Slottable.DefocusedState);
		// 				ASSBO(defPackA_p, Slottable.DefocusedState);
		// 				ASSBO(defParts_p, Slottable.DefocusedState);
		// 				ASSBO(crfParts_p, Slottable.DefocusedState);
		// 			ASSG(sgpBow, SlotGroup.DefocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defBowA_p2, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defBowB_p2, Slottable.DefocusedState);
		// 				ASSBO(crfBowA_p2, Slottable.DefocusedState);
		// 			ASSG(sgpWear, SlotGroup.FocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defWearA_p2, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defWearB_p2, Slottable.FocusedState);
		// 				ASSBO(crfWearA_p2, Slottable.FocusedState);
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defParts_p2, Slottable.DefocusedState);
		// 				ASSBO(crfParts_p2, Slottable.DefocusedState);

		// 			/**/
		// 			sgpWear.ToggleAutoSort(false);
		// 			ASSG(sgpWear, SlotGroup.FocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defWearA_p2, Slottable.EquippedAndDeselectedState);
		// 				ASSBO(defWearB_p2, Slottable.FocusedState);
		// 				ASSBO(crfWearA_p2, Slottable.FocusedState);
		// 			sgpWear.ToggleAutoSort(true);
		// 			/**/
		// 		sgm.SetFocusedPoolSG(sgpParts);
		// 			/**/
		// 			ASSG(sgpAll, SlotGroup.DefocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defBowB_p, Slottable.DefocusedState);
		// 				ASSBO(crfBowA_p, Slottable.DefocusedState);
		// 				ASSBO(defWearA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defWearB_p, Slottable.DefocusedState);
		// 				ASSBO(crfWearA_p, Slottable.DefocusedState);
		// 				ASSBO(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(crfShieldA_p, Slottable.DefocusedState);
		// 				ASSBO(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(crfMWeaponA_p, Slottable.DefocusedState);
		// 				ASSBO(defQuiverA_p, Slottable.DefocusedState);
		// 				ASSBO(defPackA_p, Slottable.DefocusedState);
		// 				ASSBO(defParts_p, Slottable.DefocusedState);
		// 				ASSBO(crfParts_p, Slottable.DefocusedState);
		// 			ASSG(sgpBow, SlotGroup.DefocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defBowA_p2, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defBowB_p2, Slottable.DefocusedState);
		// 				ASSBO(crfBowA_p2, Slottable.DefocusedState);
		// 			ASSG(sgpWear, SlotGroup.DefocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defWearA_p2, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defWearB_p2, Slottable.DefocusedState);
		// 				ASSBO(crfWearA_p2, Slottable.DefocusedState);
		// 			ASSG(sgpParts, SlotGroup.FocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defParts_p2, Slottable.FocusedState);
		// 				ASSBO(crfParts_p2, Slottable.FocusedState);
		// 			/**/
		// 			sgpParts.ToggleAutoSort(false);
		// 			/**/
		// 			ASSG(sgpParts, SlotGroup.FocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defParts_p2, Slottable.FocusedState);
		// 				ASSBO(crfParts_p2, Slottable.FocusedState);
		// 			sgpParts.ToggleAutoSort(true);
		// 			/**/
		// 		sgm.SetFocusedPoolSG(sgpAll);
		// 			ASSG(sgpAll, SlotGroup.FocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defBowB_p, Slottable.FocusedState);
		// 				ASSBO(crfBowA_p, Slottable.FocusedState);
		// 				ASSBO(defWearA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defWearB_p, Slottable.FocusedState);
		// 				ASSBO(crfWearA_p, Slottable.FocusedState);
		// 				ASSBO(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(crfShieldA_p, Slottable.FocusedState);
		// 				ASSBO(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(crfMWeaponA_p, Slottable.FocusedState);
		// 				ASSBO(defQuiverA_p, Slottable.FocusedState);
		// 				ASSBO(defPackA_p, Slottable.FocusedState);
		// 				ASSBO(defParts_p, Slottable.DefocusedState);
		// 				ASSBO(crfParts_p, Slottable.DefocusedState);
						
		// 			ASSG(sgpBow, SlotGroup.DefocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defBowA_p2, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defBowB_p2, Slottable.DefocusedState);
		// 				ASSBO(crfBowA_p2, Slottable.DefocusedState);
						
		// 			ASSG(sgpWear, SlotGroup.DefocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defWearA_p2, Slottable.EquippedAndDefocusedState);
		// 				ASSBO(defWearB_p2, Slottable.DefocusedState);
		// 				ASSBO(crfWearA_p2, Slottable.DefocusedState);
						
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 				ASSBOReset();
		// 				ASSBO(defParts_p2, Slottable.DefocusedState);
		// 				ASSBO(crfParts_p2, Slottable.DefocusedState);

		// 	}
		// 	public void AssertAllSBsFocused(SlotGroup sg, bool focused){
		// 		foreach(Slottable sb in sg.Slottables){
		// 			if(focused){
		// 				if(!sg.IsAutoSort){
		// 					ASSB(sb, Slottable.FocusedState);
		// 				}else{
		// 					if(sb.IsEquipped){
		// 						if(sg.IsPool)
		// 							ASSB(sb, Slottable.EquippedAndDefocusedState);
		// 						else
		// 							ASSB(sb, Slottable.EquippedAndDeselectedState);
		// 					}else{
		// 						if(sb.ItemInst is PartsInstanceMock && sg.Filter != SlotGroup.PartsFilter)
		// 							ASSB(sb, Slottable.DefocusedState);
		// 						else
		// 							ASSB(sb, Slottable.FocusedState);
		// 					}
		// 				}
		// 			}else{
		// 				if(sb.IsEquipped)
		// 					ASSB(sb, Slottable.EquippedAndDefocusedState);
		// 				else
		// 					ASSB(sb, Slottable.DefocusedState);
		// 			}
		// 		}
		// 	}
		// 	public void TestInstantTransaction(){
		// 		/*	AS off	*/
		// 		sgpAll.ToggleAutoSort(false);
		// 			AssertFocused();
		// 			AssertSGCounts(sgpAll, 14);
		// 				ASSBOReset();
		// 				ASSBO(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSBO(defBowB_p, Slottable.FocusedState);
		// 				ASSBO(crfBowA_p, Slottable.FocusedState);
		// 				ASSBO(defWearA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSBO(defWearB_p, Slottable.FocusedState);
		// 				ASSBO(crfWearA_p, Slottable.FocusedState);
		// 				ASSBO(defShieldA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSBO(crfShieldA_p, Slottable.FocusedState);
		// 				ASSBO(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSBO(crfMWeaponA_p, Slottable.FocusedState);
		// 				ASSBO(defQuiverA_p, Slottable.FocusedState);
		// 				ASSBO(defPackA_p, Slottable.FocusedState);
		// 				ASSBO(defParts_p, Slottable.FocusedState);
		// 				ASSBO(crfParts_p, Slottable.FocusedState);
		// 			AssertSGCounts(sgpBow, 3);
		// 			AssertSGCounts(sgpWear, 3);
		// 			AssertSGCounts(sgpCGears, 6);
		// 			AssertSGCounts(sgpParts, 2);
		// 		/*	removal	*/
		// 			poolInv.RemoveItem(crfBowA_p.ItemInst);
		// 			poolInv.RemoveItem(crfWearA_p.ItemInst);
		// 			poolInv.RemoveItem(crfShieldA_p.ItemInst);
		// 			poolInv.RemoveItem(crfMWeaponA_p.ItemInst);
		// 			InstantSyncSBsOnAllSGs();
		// 	}
		// /*	Test swapping with action	*/
		// 	public void TestSwapActionCGears(){
		// 		/*	defShieldA
		// 			crfShieldA
		// 			defMWeaponA
		// 			crfMWeaponA
		// 			defQuiverA
		// 			defPackA
		// 		*/
		// 			AssertFocused();
		// 		/*	defShieldA -> */
		// 			AECGears(defShieldA_p, defMWeaponA_p, null, null);
		// 		Slottable defShieldA_e = ECGear(defShieldA_p);
		// 		FillEquip(defShieldA_e, sgpAll);
		// 			AECGears(defMWeaponA_p, null, null, null);
		// 		FillEquip(crfShieldA_p, sgCGears);
		// 			AECGears(crfShieldA_p, defMWeaponA_p, null, null);
		// 			AssertFocused();
		// 		FillEquip(crfMWeaponA_p, sgCGears);
		// 		FillEquip(defQuiverA_p, sgCGears);
		// 			AECGears(crfShieldA_p, defMWeaponA_p, crfMWeaponA_p, defQuiverA_p);
				
		// 		Slottable crfShieldA_e = ECGear(crfShieldA_p);
		// 		Slottable defMWeaponA_e = ECGear(defMWeaponA_p);
		// 		Slottable crfMWeaponA_e = ECGear(crfMWeaponA_p);
		// 		Slottable defQuiverA_e = ECGear(defQuiverA_p);
		// 		SwapEquipObsolete(defShieldA_p, crfShieldA_e);
		// 			AECGears(defShieldA_p, defMWeaponA_p, crfMWeaponA_p, defQuiverA_p);
		// 		defShieldA_e = ECGear(defShieldA_p);
		// 		SwapEquipObsolete(defShieldA_e, crfShieldA_p);
		// 			AECGears(crfShieldA_p, defMWeaponA_p, crfMWeaponA_p, defQuiverA_p);

		// 		SwapEquipObsolete(defShieldA_p, defMWeaponA_e);
		// 			AECGears(defShieldA_p, crfShieldA_p, crfMWeaponA_p, defQuiverA_p);
		// 		defShieldA_e = ECGear(defShieldA_p);
		// 		SwapEquipObsolete(defShieldA_e, defMWeaponA_p);
		// 			AECGears(crfShieldA_p, defMWeaponA_p, crfMWeaponA_p, defQuiverA_p);
				
		// 		SwapEquipObsolete(defShieldA_p, crfMWeaponA_e);
		// 			AECGears(defShieldA_p, crfShieldA_p, defMWeaponA_p, defQuiverA_p);
		// 		defShieldA_e = ECGear(defShieldA_p);
		// 		SwapEquipObsolete(defShieldA_e, crfMWeaponA_p);
		// 			AECGears(crfShieldA_p, defMWeaponA_p, crfMWeaponA_p, defQuiverA_p);
				
		// 		SwapEquipObsolete(defShieldA_p, defQuiverA_e);
		// 			AECGears(defShieldA_p, crfShieldA_p, defMWeaponA_p, crfMWeaponA_p);
		// 		defShieldA_e = ECGear(defShieldA_p);
		// 		SwapEquipObsolete(defShieldA_e, defQuiverA_p);
		// 			AECGears(crfShieldA_p, defMWeaponA_p, crfMWeaponA_p, defQuiverA_p);
		// 		/*	crfShieldA_p ->	*/
		// 		crfShieldA_e = ECGear(crfShieldA_p);
		// 		SwapEquipObsolete(defShieldA_p, crfShieldA_e);
		// 			AECGears(defShieldA_p, defMWeaponA_p, crfMWeaponA_p, defQuiverA_p);
				
		// 		defQuiverA_e = ECGear(defQuiverA_p);
		// 		SwapEquipObsolete(crfShieldA_p, defQuiverA_e);
		// 			AECGears(defShieldA_p, crfShieldA_p, defMWeaponA_p, crfMWeaponA_p);
		// 		crfShieldA_e = ECGear(crfShieldA_p);
		// 		SwapEquipObsolete(crfShieldA_e, defQuiverA_p);
		// 			AECGears(defShieldA_p, defMWeaponA_p, crfMWeaponA_p, defQuiverA_p);
				
		// 		crfMWeaponA_e = ECGear(crfMWeaponA_p);
		// 		SwapEquipObsolete(crfShieldA_p, crfMWeaponA_e);
		// 			AECGears(defShieldA_p, crfShieldA_p, defMWeaponA_p, defQuiverA_p);
		// 		crfShieldA_e = ECGear(crfShieldA_p);
		// 		SwapEquipObsolete(crfShieldA_e, crfMWeaponA_p);
		// 			AECGears(defShieldA_p, defMWeaponA_p, crfMWeaponA_p, defQuiverA_p);
				
		// 		defMWeaponA_e = ECGear(defMWeaponA_p);
		// 		SwapEquipObsolete(crfShieldA_p, defMWeaponA_e);
		// 			AECGears(defShieldA_p, crfShieldA_p, crfMWeaponA_p, defQuiverA_p);
		// 		crfShieldA_e = ECGear(crfShieldA_p);
		// 		SwapEquipObsolete(crfShieldA_e, defMWeaponA_p);
		// 			AECGears(defShieldA_p, defMWeaponA_p, crfMWeaponA_p, defQuiverA_p);
					
		// 		defShieldA_e = ECGear(defShieldA_p);
		// 		SwapEquipObsolete(crfShieldA_p, defShieldA_e);
		// 			AECGears(crfShieldA_p, defMWeaponA_p, crfMWeaponA_p, defQuiverA_p);
		// 		crfShieldA_e = ECGear(crfShieldA_p);
		// 		SwapEquipObsolete(crfShieldA_e, defShieldA_p);
		// 			AECGears(defShieldA_p, defMWeaponA_p, crfMWeaponA_p, defQuiverA_p);
		// 	}
		// 	public void TestSwapActionWear(){
		// 		/*	<- defWearA_e	*/
		// 			AssertEquipped(defWearA_p);
		// 		SwapEquipObsolete(EWear_e, defWearB_p);
		// 		SwapEquip(defWearA_p);
		// 		SwapEquipObsolete(EWear_e, crfWearA_p);
		// 		/*	<- defWearB_e	*/
		// 		SwapEquip(defWearB_p);
		// 			AssertEquipped(defWearB_p);
		// 		SwapEquipObsolete(EWear_e, defWearA_p);
		// 		SwapEquip(defWearB_p);
		// 		SwapEquipObsolete(EWear_e, crfWearA_p);
		// 		/*	<- crfWearA_e	*/
		// 			AssertEquipped(crfWearA_p);
		// 		SwapEquipObsolete(EWear_e, defWearA_p);
		// 		SwapEquip(crfWearA_p);
		// 		SwapEquipObsolete(EWear_e, defWearB_p);

		// 		/*	defWearA_p -> */
		// 			AssertEquipped(defWearB_p);
		// 		SwapEquip(defWearA_p);
		// 		SwapEquip(crfWearA_p);
		// 		SwapEquip(defWearA_p);
		// 		/*	defWearB_p -> */
		// 		SwapEquip(defWearB_p);
		// 			AssertEquipped(defWearB_p);
		// 		SwapEquip(defWearA_p);
		// 		SwapEquip(defWearB_p);
		// 		SwapEquip(crfWearA_p);
		// 		/*	defWearB_p -> */
		// 			AssertEquipped(crfWearA_p);
		// 		SwapEquip(defWearA_p);
		// 		SwapEquip(crfWearA_p);
		// 		SwapEquip(defWearB_p);
		// 	}
		// 	public void TestSwapActionBow(){
		// 		/*	<- defBowA_e	*/
		// 			AssertEquipped(defBowA_p);
		// 		SwapEquipObsolete(EBow_e, defBowB_p);
		// 		SwapEquip(defBowA_p);
		// 		SwapEquipObsolete(EBow_e, crfBowA_p);
		// 		/*	<- defBowB_e	*/
		// 		SwapEquip(defBowB_p);
		// 			AssertEquipped(defBowB_p);
		// 		SwapEquipObsolete(EBow_e, defBowA_p);
		// 		SwapEquip(defBowB_p);
		// 		SwapEquipObsolete(EBow_e, crfBowA_p);
		// 		/*	<- crfBowA_e	*/
		// 			AssertEquipped(crfBowA_p);
		// 		SwapEquipObsolete(EBow_e, defBowA_p);
		// 		SwapEquip(crfBowA_p);
		// 		SwapEquipObsolete(EBow_e, defBowB_p);

		// 		/*	defBowA_p -> */
		// 			AssertEquipped(defBowB_p);
		// 		SwapEquip(defBowA_p);
		// 		SwapEquip(crfBowA_p);
		// 		SwapEquip(defBowA_p);
		// 		/*	defBowB_p -> */
		// 		SwapEquip(defBowB_p);
		// 			AssertEquipped(defBowB_p);
		// 		SwapEquip(defBowA_p);
		// 		SwapEquip(defBowB_p);
		// 		SwapEquip(crfBowA_p);
		// 		/*	defBowB_p -> */
		// 			AssertEquipped(crfBowA_p);
		// 		SwapEquip(defBowA_p);
		// 		SwapEquip(crfBowA_p);
		// 		SwapEquip(defBowB_p);
		// 	}
		// 	public void TestSwapActionGeneric(){
		// 		foreach(Slot slot in sgpAll.Slots){
		// 			if(slot.Sb != null){
		// 				Slottable sb = slot.Sb;
		// 				if(sb.ItemInst is BowInstanceMock){
		// 					if(sb == EBow_p){
		// 						foreach(Slot slot_n in sgpAll.Slots){
		// 							if(slot_n.Sb != null){
		// 								Slottable sb_n = slot_n.Sb;
		// 								if(sb != EBow_p){
		// 									SwapAndSwapBack(sb, EBow_p);
		// 									SwapAndSwapBack(EBow_e, sb);
		// 								}
		// 							}
		// 						}
		// 					}else{// if not equipped switch equipment
		// 						SwapEquip(sb);
		// 							AssertEquipped(sb);
		// 						foreach(Slot slot_n in sgpAll.Slots){
		// 							if(slot_n.Sb != null){
		// 								Slottable sb_n = slot_n.Sb;
		// 								if(sb != EBow_p){
		// 									SwapAndSwapBack(sb, EBow_p);
		// 									SwapAndSwapBack(EBow_e, sb);
		// 								}
		// 							}
		// 						}
		// 					}
		// 				}else if(sb.ItemInst is WearInstanceMock){
		// 					if(sb == EWear_p){
		// 						foreach(Slot slot_n in sgpAll.Slots){
		// 							if(slot_n.Sb != null){
		// 								Slottable sb_n = slot_n.Sb;
		// 								if(sb != EWear_p){
		// 									SwapAndSwapBack(sb, EWear_p);
		// 									SwapAndSwapBack(EWear_e, sb);
		// 								}
		// 							}
		// 						}
		// 					}else{// if not equipped switch equipment
		// 						SwapEquip(sb);
		// 							AssertEquipped(sb);
		// 						foreach(Slot slot_n in sgpAll.Slots){
		// 							if(slot_n.Sb != null){
		// 								Slottable sb_n = slot_n.Sb;
		// 								if(sb != EWear_p){
		// 									SwapAndSwapBack(sb, EWear_p);
		// 									SwapAndSwapBack(EWear_e, sb);
		// 								}
		// 							}
		// 						}
		// 					}
		// 				}
		// 			}
		// 		}
		// 	}
		// 	public void SwapAndSwapBack(Slottable pickedSB, Slottable hoveredSB_p){
		// 		if(pickedSB.ItemInst is BowInstanceMock){
		// 			SlotGroup pickedSG = sgm.GetSlotGroup(pickedSB);
		// 			Slottable hoveredSB;
		// 			Slottable equipped;
		// 			Slottable nonequipped;
		// 			if(pickedSG.IsPool){
		// 				hoveredSB = EBow_e;	
		// 				equipped = hoveredSB_p;
		// 				nonequipped = pickedSB;
		// 			}else{
		// 				hoveredSB = hoveredSB_p;
		// 				equipped = sgpAll.GetSlottable(pickedSB.Item);
		// 				nonequipped = hoveredSB;
		// 			}
		// 				AssertEquipped(equipped);
		// 			SwapEquipObsolete(pickedSB, hoveredSB);
		// 				AssertEquipped(nonequipped);
		// 			if(pickedSG.IsPool){
		// 				SwapEquipObsolete(EBow_e, hoveredSB_p);
		// 			}else{
		// 				SwapEquipObsolete(sgpAll.GetSlottable(pickedSB.Item), EBow_e);
		// 			}
		// 				AssertEquipped(equipped);
		// 		}else if(pickedSB.ItemInst is WearInstanceMock){
		// 			SlotGroup pickedSG = sgm.GetSlotGroup(pickedSB);
		// 			Slottable hoveredSB;
		// 			Slottable equipped;
		// 			Slottable nonequipped;
		// 			if(pickedSG.IsPool){
		// 				hoveredSB = EWear_e;	
		// 				equipped = hoveredSB_p;
		// 				nonequipped = pickedSB;
		// 			}else{
		// 				hoveredSB = hoveredSB_p;
		// 				equipped = sgpAll.GetSlottable(pickedSB.Item);
		// 				nonequipped = hoveredSB;
		// 			}
		// 				AssertEquipped(equipped);
		// 			SwapEquipObsolete(pickedSB, hoveredSB);
		// 				AssertEquipped(nonequipped);
		// 			if(pickedSG.IsPool){
		// 				SwapEquipObsolete(EWear_e, hoveredSB_p);
		// 			}else{
		// 				SwapEquipObsolete(sgpAll.GetSlottable(pickedSB.Item), EWear_e);
		// 			}
		// 				AssertEquipped(equipped);
		// 		}
		// 		AssertFocused();
		// 	}
		
		// /*	Reordering and Sorting */
		// 	public void TestTogglingAutoSort(){
		// 		AB(sgpAll.IsAutoSort, true);
		// 			ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(defBowB_p, Slottable.FocusedState);
		// 			ASSB(crfBowA_p, Slottable.FocusedState);
		// 			ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(defWearB_p, Slottable.FocusedState);
		// 			ASSB(crfWearA_p, Slottable.FocusedState);
		// 			ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(crfShieldA_p, Slottable.FocusedState);
		// 			ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 			ASSB(defQuiverA_p, Slottable.FocusedState);
		// 			ASSB(defPackA_p, Slottable.FocusedState);
		// 			ASSB(defParts_p, Slottable.DefocusedState);
		// 			ASSB(crfParts_p, Slottable.DefocusedState);
		// 		sgpAll.ToggleAutoSort(false);
		// 			ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 			ASSB(defBowB_p, Slottable.FocusedState);
		// 			ASSB(crfBowA_p, Slottable.FocusedState);
		// 			ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);
		// 			ASSB(defWearB_p, Slottable.FocusedState);
		// 			ASSB(crfWearA_p, Slottable.FocusedState);
		// 			ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);
		// 			ASSB(crfShieldA_p, Slottable.FocusedState);
		// 			ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
		// 			ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 			ASSB(defQuiverA_p, Slottable.FocusedState);
		// 			ASSB(defPackA_p, Slottable.FocusedState);
		// 			ASSB(defParts_p, Slottable.FocusedState);
		// 			ASSB(crfParts_p, Slottable.FocusedState);
		// 		sgpAll.ToggleAutoSort(true);
		// 			ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(defBowB_p, Slottable.FocusedState);
		// 			ASSB(crfBowA_p, Slottable.FocusedState);
		// 			ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(defWearB_p, Slottable.FocusedState);
		// 			ASSB(crfWearA_p, Slottable.FocusedState);
		// 			ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(crfShieldA_p, Slottable.FocusedState);
		// 			ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 			ASSB(defQuiverA_p, Slottable.FocusedState);
		// 			ASSB(defPackA_p, Slottable.FocusedState);
		// 			ASSB(defParts_p, Slottable.DefocusedState);
		// 			ASSB(crfParts_p, Slottable.DefocusedState);

		// 	}
		// 	public void TestReorderSGCGears(){
		// 		sgCGears.ToggleAutoSort(false);
		// 			AssertFocused();
		// 			AECGears(defShieldA_p, defMWeaponA_p, null, null);
		// 		FillEquip(defQuiverA_p, sgCGears);
		// 			AssertFocused();
		// 			AECGears(defShieldA_p, defMWeaponA_p, defQuiverA_p, null);
		// 		FillEquip(defPackA_p, sgCGears);
		// 			AssertFocused();
		// 			AECGears(defShieldA_p, defMWeaponA_p, defQuiverA_p, defPackA_p);
		// 		sgCGears.ToggleAutoSort(false);
		// 		/* states */
		// 			defShieldA_e = sgCGears.GetSlottable(defShieldA_p.Item);
		// 			defMWeaponA_e = sgCGears.GetSlottable(defMWeaponA_p.Item);
		// 			defQuiverA_e = sgCGears.GetSlottable(defQuiverA_p.Item);
		// 			defPackA_e = sgCGears.GetSlottable(defPackA_p.Item);
		// 			ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSB(defQuiverA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSB(defPackA_e, Slottable.EquippedAndDeselectedState);
					
		// 		PickUp(defShieldA_e, out picked);
		// 			ASSB(defShieldA_e, Slottable.PickedAndSelectedState);
		// 			ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSB(defQuiverA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSB(defPackA_e, Slottable.EquippedAndDeselectedState);

		// 		SimHover(defQuiverA_e, sgCGears, eventData);
		// 			AE(sgm.PickedSB, defShieldA_e);
		// 			AB(sgm.SelectedSB == null, false);
		// 			AE(sgm.SelectedSB, defQuiverA_e);
		// 			AE(sgm.SelectedSG, sgCGears);

		// 			AT<ReorderTransaction>(false);
					
		// 			ASSGM(sgm, SlotGroupManager.ProbingState);
		// 			AP<SGMProbingStateProcess>(sgm, false);

		// 			ASSG(sgCGears, SlotGroup.SelectedState);
		// 			AP<SGHighlightProcess>(sgCGears, false);

		// 			ASSB(defShieldA_e, Slottable.PickedAndDeselectedState);
		// 			AP<SBDehighlightProcess>(defShieldA_e, false);
		// 			ASSB(defQuiverA_e, Slottable.EquippedAndSelectedState);
		// 			AP<SBHighlightProcess>(defQuiverA_e, false);

		// 		defShieldA_e.OnPointerUpMock(eventData);
		// 		defShieldA_e.CurProcess.Expire();
		// 		CompleteAllSBProcesses(sgCGears);
		// 			AssertFocused();
		// 			AECGears(defMWeaponA_p, defQuiverA_p, defShieldA_p, defPackA_p);
		// 		PickUp(defPackA_e, out picked);
		// 		SimHover(defMWeaponA_e, sgCGears, eventData);
		// 		defPackA_e.OnPointerUpMock(eventData);
		// 		defPackA_e.CurProcess.Expire();
		// 		CompleteAllSBProcesses(sgCGears);
		// 			AssertFocused();
		// 			AECGears(defPackA_p, defMWeaponA_p, defQuiverA_p, defShieldA_p);
		// 		/*	some interSG transactions */
		// 			sgCGears.ToggleAutoSort(false);
		// 			sgpAll.ToggleAutoSort(false);
		// 				AssertFocused();
		// 				AECGears(defPackA_p, defMWeaponA_p, defQuiverA_p, defShieldA_p);
		// 				AssertOrderReset();
		// 				AssertOrder(defPackA_e);
		// 				AssertOrder(defMWeaponA_e);
		// 				AssertOrder(defQuiverA_e);
		// 				AssertOrder(defShieldA_e);
		// 			PickUp(defQuiverA_e, out picked);
		// 			SimHover(null, sgpAll, eventData);
		// 			defQuiverA_e.OnPointerUpMock(eventData);
		// 			defQuiverA_e.CurProcess.Expire();
		// 				AssertFocused();
		// 				AECGears(defPackA_p, defMWeaponA_p, null, defShieldA_p);
		// 				AssertOrderReset();
		// 				AssertOrder(defPackA_e);
		// 				AssertOrder(defMWeaponA_e);
		// 				AssertOrder(null);
		// 				AssertOrder(defShieldA_e);
		// 			/*	make sure dropping on an equipped occupant triggers swapping, not reordering	*/
		// 				PickUp(crfShieldA_p, out picked);
		// 				SimHover(defMWeaponA_e, sgCGears, eventData);
		// 				crfShieldA_p.OnPointerUpMock(eventData);
		// 				crfShieldA_p.CurProcess.Expire();
		// 				defMWeaponA_e.CurProcess.Expire();
		// 					AssertFocused();
		// 					AECGears(defPackA_p, crfShieldA_p, null, defShieldA_p);
		// 		/* vol sort*/
		// 			sgm.SortSG(sgCGears, SlotGroup.ItemIDSorter);
		// 			CompleteAllSBProcesses(sgCGears);
		// 				AssertFocused();
		// 				AECGears(defShieldA_p, crfShieldA_p, defPackA_p, null);
		// 		/*	toggle on and swap equip */
		// 			sgCGears.ToggleAutoSort(true);
		// 			PickUp(crfMWeaponA_p, out picked);
		// 			SimHover(defShieldA_e, sgCGears, eventData);
		// 			crfMWeaponA_p.OnPointerUpMock(eventData);
		// 			crfMWeaponA_p.CurProcess.Expire();
		// 			defShieldA_e.CurProcess.Expire();
		// 			CompleteAllSBProcesses(sgCGears);
		// 				AssertFocused();
		// 				AECGears(crfShieldA_p, crfMWeaponA_p, defPackA_p, null);
		// 		/*	fill equip	*/
		// 			PickUp(defMWeaponA_p, out picked);
		// 			SimHover(null, sgCGears, eventData);
		// 			defMWeaponA_p.OnPointerUpMock(eventData);
		// 			defMWeaponA_p.CurProcess.Expire();
		// 			CompleteAllSBProcesses(sgCGears);
		// 				AssertFocused();
		// 				AECGears(crfShieldA_p, defMWeaponA_p, crfMWeaponA_p, defPackA_p);

		// 	}
		// 	public void TestReorderTransactionOnSGPAll(){
		// 		/*	on focused	*/
		// 			sgpAll.ToggleAutoSort(false);
		// 			sgCGears.ToggleAutoSort(false);
		// 				AssertFocused();
		// 				AssertOrderReset();
		// 				AssertOrder(defBowA_p);
		// 				AssertOrder(defBowB_p);
		// 				AssertOrder(crfBowA_p);
		// 				AssertOrder(defWearA_p);
		// 				AssertOrder(defWearB_p);
		// 				AssertOrder(crfWearA_p);
		// 				AssertOrder(defShieldA_p);
		// 				AssertOrder(crfShieldA_p);
		// 				AssertOrder(defMWeaponA_p);
		// 				AssertOrder(crfMWeaponA_p);
		// 				AssertOrder(defQuiverA_p);
		// 				AssertOrder(defPackA_p);
		// 				AssertOrder(defParts_p);
		// 				AssertOrder(crfParts_p);
		// 			PickUp(defBowB_p, out picked);
		// 				ASSG(sgpAll, SlotGroup.SelectedState);
		// 				ASSB(defBowB_p, Slottable.PickedAndSelectedState);
		// 				ASSB(defBowB_p, Slottable.PickedAndSelectedState);
		// 				ASSB(crfBowA_p, Slottable.FocusedState);
		// 			SimHover(crfBowA_p, sgpAll, eventData);
		// 				AT<ReorderTransaction>(false);
		// 			defBowB_p.OnPointerUpMock(eventData);
		// 				AE(sgm.SelectedSGDoneTransaction, false);
		// 				ASSGM(sgm, SlotGroupManager.PerformingTransactionState);
		// 				AP<SGMActionProcess>(sgm, false);
		// 				ASSG(sgpAll, SlotGroup.SortingState);
		// 				AP<SGSortingProcess>(sgpAll, false);
		// 				ASSB(defBowB_p, Slottable.MovingState);
		// 				AP<SBReorderingProcess>(defBowB_p, false);
		// 				AE(sgpAll.SlotMovements.Count, 14);
		// 				AssertMoveSlotIndex(sgpAll, defBowA_p, 0, 0, true);
		// 				AssertMoveSlotIndex(sgpAll, defBowB_p, 1, 2, false);
		// 				AssertMoveSlotIndex(sgpAll, crfBowA_p, 2, 1, false);
		// 				AssertMoveSlotIndex(sgpAll, defWearA_p, 3, 3, true);
		// 				AssertMoveSlotIndex(sgpAll, defWearB_p, 4, 4, true);
		// 				AssertMoveSlotIndex(sgpAll, crfWearA_p, 5, 5, true);
		// 				AssertMoveSlotIndex(sgpAll, defShieldA_p, 6, 6, true);
		// 				AssertMoveSlotIndex(sgpAll, crfShieldA_p, 7, 7, true);
		// 				AssertMoveSlotIndex(sgpAll, defMWeaponA_p, 8, 8, true);
		// 				AssertMoveSlotIndex(sgpAll, crfMWeaponA_p, 9, 9, true);
		// 				AssertMoveSlotIndex(sgpAll, defQuiverA_p, 10, 10, true);
		// 				AssertMoveSlotIndex(sgpAll, defPackA_p, 11, 11, true);
		// 				AssertMoveSlotIndex(sgpAll, defParts_p, 12, 12, true);
		// 				AssertMoveSlotIndex(sgpAll, crfParts_p, 13, 13, true);
		// 			// ExpireProcesses(defBowB_p, null, sgpAll, null);
		// 				AE(sgm.SelectedSGDoneTransaction, false);
		// 			CompleteAllSBProcesses(sgpAll);
		// 				AE(sgm.SelectedSGDoneTransaction, true);
		// 				AE(sgm.PickedSBDoneTransaction, false);
		// 			defBowB_p.CurProcess.Expire();
		// 				AE(sgm.PickedSBDoneTransaction, true);

		// 				AE(sgm.SelectedSBDoneTransaction, true);
		// 				AE(sgm.OrigSGDoneTransaction, true);
		// 				AssertFocused();
		// 				AssertOrderReset();
		// 				AssertOrder(defBowA_p);
		// 				AssertOrder(crfBowA_p);
		// 				AssertOrder(defBowB_p);
		// 				AssertOrder(defWearA_p);
		// 				AssertOrder(defWearB_p);
		// 				AssertOrder(crfWearA_p);
		// 				AssertOrder(defShieldA_p);
		// 				AssertOrder(crfShieldA_p);
		// 				AssertOrder(defMWeaponA_p);
		// 				AssertOrder(crfMWeaponA_p);
		// 				AssertOrder(defQuiverA_p);
		// 				AssertOrder(defPackA_p);
		// 				AssertOrder(defParts_p);
		// 				AssertOrder(crfParts_p);
		// 		/*	on Defocused (nonesense)	*/
		// 			sgpAll.ToggleAutoSort(true);
		// 				AssertFocused();
		// 				AssertOrderReset();
		// 				AssertOrder(defBowA_p);
		// 				AssertOrder(crfBowA_p);
		// 				AssertOrder(defBowB_p);
		// 				AssertOrder(defWearA_p);
		// 				AssertOrder(defWearB_p);
		// 				AssertOrder(crfWearA_p);
		// 				AssertOrder(defShieldA_p);
		// 				AssertOrder(crfShieldA_p);
		// 				AssertOrder(defMWeaponA_p);
		// 				AssertOrder(crfMWeaponA_p);
		// 				AssertOrder(defQuiverA_p);
		// 				AssertOrder(defPackA_p);
		// 				AssertOrder(defParts_p);
		// 				AssertOrder(crfParts_p);
		// 				ASSB(defWearB_p, Slottable.FocusedState);
		// 				ASSB(defParts_p, Slottable.DefocusedState);
		// 				ASSB(crfParts_p, Slottable.DefocusedState);
		// 			sgpAll.ToggleAutoSort(false);
		// 				ASSB(defParts_p, Slottable.FocusedState);
		// 				ASSB(crfParts_p, Slottable.FocusedState);

		// 			PickUp(defWearB_p, out picked);
		// 			SimHover(defParts_p, sgpAll, eventData);
		// 				AT<ReorderTransaction>(false);
		// 			defWearB_p.OnPointerUpMock(eventData);
		// 				AssertMoveSlotIndex(sgpAll, defBowA_p, 0, 0, true);
		// 				AssertMoveSlotIndex(sgpAll, crfBowA_p, 1, 1, true);
		// 				AssertMoveSlotIndex(sgpAll, defBowB_p, 2, 2, true);
		// 				AssertMoveSlotIndex(sgpAll, defWearA_p, 3, 3, true);
		// 				AssertMoveSlotIndex(sgpAll, defWearB_p, 4, 12, false);
		// 				AssertMoveSlotIndex(sgpAll, crfWearA_p, 5, 4, false);
		// 				AssertMoveSlotIndex(sgpAll, defShieldA_p, 6, 5, false);
		// 				AssertMoveSlotIndex(sgpAll, crfShieldA_p, 7, 6, false);
		// 				AssertMoveSlotIndex(sgpAll, defMWeaponA_p, 8, 7, false);
		// 				AssertMoveSlotIndex(sgpAll, crfMWeaponA_p, 9, 8, false);
		// 				AssertMoveSlotIndex(sgpAll, defQuiverA_p, 10, 9, false);
		// 				AssertMoveSlotIndex(sgpAll, defPackA_p, 11, 10, false);
		// 				AssertMoveSlotIndex(sgpAll, defParts_p, 12, 11, false);
		// 				AssertMoveSlotIndex(sgpAll, crfParts_p, 13, 13, true);

		// 				ASSGM(sgm, SlotGroupManager.PerformingTransactionState);
		// 				AT<ReorderTransaction>(false);
		// 				AP<SGMActionProcess>(sgm, false);
		// 				ASSG(sgpAll, SlotGroup.SortingState);
		// 				AP<SGSortingProcess>(sgpAll, false);
		// 				ASSB(defWearB_p, Slottable.MovingState);
		// 				AP<SBReorderingProcess>(defWearB_p, false);
		// 				ASSB(defParts_p, Slottable.SelectedState);

		// 				AE(sgm.PickedSB, defWearB_p);
		// 				AE(sgm.SelectedSB, defParts_p);
		// 				AE(sgm.SelectedSG, sgpAll);

		// 				AE(sgpAll.SlotMovements.Count, 14);
		// 				AE(sgm.PickedSBDoneTransaction, false);
		// 				AE(sgm.OrigSGDoneTransaction, true);
		// 				AE(sgm.SelectedSBDoneTransaction, true);
		// 				AE(sgm.SelectedSGDoneTransaction, false);

		// 			defWearB_p.CurProcess.Expire();
		// 			CompleteAllSBProcesses(sgpAll);

		// 				AE(sgm.PickedSBDoneTransaction, true);
		// 				AE(sgm.OrigSGDoneTransaction, true);
		// 				AE(sgm.SelectedSBDoneTransaction, true);
		// 				AE(sgm.SelectedSGDoneTransaction, true);
		// 				AE(sgpAll.SlotMovements.Count, 0);

		// 				AP<SGMActionProcess>(sgm, true);
		// 				// AE(sgm.CurProcess.IsExpired, true);
		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 				AP<SGSortingProcess>(sgpAll, false);
		// 				AE(sgpAll.CurProcess.IsExpired, true);
		// 				ASSB(defWearB_p, Slottable.FocusedState);
		// 				AP<SBReorderingProcess>(defWearB_p, true);
		// 				// ASSB(defParts_p, Slottable.FocusedState);

		// 				AE(sgm.PickedSB, null);
		// 				AE(sgm.SelectedSB, null);
		// 				AE(sgm.SelectedSG, null);

		// 				ASSGM(sgm, SlotGroupManager.FocusedState);
		// 				AT<ReorderTransaction>(true);
						
		// 				AssertFocused();
		// 				AssertOrderReset();
		// 				AssertOrder(defBowA_p);
		// 				AssertOrder(crfBowA_p);
		// 				AssertOrder(defBowB_p);
		// 				AssertOrder(defWearA_p);
		// 				AssertOrder(crfWearA_p);
		// 				AssertOrder(defShieldA_p);
		// 				AssertOrder(crfShieldA_p);
		// 				AssertOrder(defMWeaponA_p);
		// 				AssertOrder(crfMWeaponA_p);
		// 				AssertOrder(defQuiverA_p);
		// 				AssertOrder(defPackA_p);
		// 				AssertOrder(defParts_p);
		// 				AssertOrder(defWearB_p);
		// 				AssertOrder(crfParts_p);
		// 		/*	Focused on Focused */
		// 			sgpAll.ToggleAutoSort(false);
		// 				AssertFocused();
		// 				AssertOrderReset();
		// 					AssertOrder(defBowA_p);
		// 					AssertOrder(crfBowA_p);
		// 					AssertOrder(defBowB_p);
		// 					AssertOrder(defWearA_p);
		// 					AssertOrder(crfWearA_p);
		// 					AssertOrder(defShieldA_p);
		// 					AssertOrder(crfShieldA_p);
		// 					AssertOrder(defMWeaponA_p);
		// 					AssertOrder(crfMWeaponA_p);
		// 					AssertOrder(defQuiverA_p);
		// 					AssertOrder(defPackA_p);
		// 					AssertOrder(defParts_p);
		// 					AssertOrder(defWearB_p);
		// 					AssertOrder(crfParts_p);
		// 				/*	states  */
		// 					ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 					ASSB(crfBowA_p, Slottable.FocusedState);
		// 					ASSB(defBowB_p, Slottable.FocusedState);
		// 					ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);
		// 					ASSB(crfWearA_p, Slottable.FocusedState);// picked
		// 					ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);
		// 					ASSB(crfShieldA_p, Slottable.FocusedState);
		// 					ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
		// 					ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 					ASSB(defQuiverA_p, Slottable.FocusedState);
		// 					ASSB(defPackA_p, Slottable.FocusedState);// hovered
		// 					ASSB(defParts_p, Slottable.FocusedState);
		// 					ASSB(defWearB_p, Slottable.FocusedState);
		// 					ASSB(crfParts_p, Slottable.FocusedState);
		// 			PickUp(crfWearA_p, out picked);
		// 			SimHover(defPackA_p, sgpAll, eventData);
		// 				AT<ReorderTransaction>(false);
		// 			crfWearA_p.OnPointerUpMock(eventData);
		// 			crfWearA_p.CurProcess.Expire();
		// 				/*	Move ids	*/
		// 					AssertMoveSlotIndex(sgpAll, defBowA_p, 0, 0, true);
		// 					AssertMoveSlotIndex(sgpAll, crfBowA_p, 1, 1, true);
		// 					AssertMoveSlotIndex(sgpAll, defBowB_p, 2, 2, true);
		// 					AssertMoveSlotIndex(sgpAll, defWearA_p, 3, 3, true);
		// 					AssertMoveSlotIndex(sgpAll, crfWearA_p, 4, 10, false);
		// 					AssertMoveSlotIndex(sgpAll, defShieldA_p, 5, 4, false);
		// 					AssertMoveSlotIndex(sgpAll, crfShieldA_p, 6, 5, false);
		// 					AssertMoveSlotIndex(sgpAll, defMWeaponA_p, 7, 6, false);
		// 					AssertMoveSlotIndex(sgpAll, crfMWeaponA_p, 8, 7, false);
		// 					AssertMoveSlotIndex(sgpAll, defQuiverA_p, 9, 8, false);
		// 					AssertMoveSlotIndex(sgpAll, defPackA_p, 10, 9, false);
		// 					AssertMoveSlotIndex(sgpAll, defParts_p, 11, 11, true);
		// 					AssertMoveSlotIndex(sgpAll, defWearB_p, 12, 12, true);
		// 					AssertMoveSlotIndex(sgpAll, crfParts_p, 13, 13, true);
		// 			CompleteAllSBProcesses(sgpAll);
		// 				AssertFocused();
		// 					AssertOrderReset();
		// 					AssertOrder(defBowA_p);
		// 					AssertOrder(crfBowA_p);
		// 					AssertOrder(defBowB_p);
		// 					AssertOrder(defWearA_p);
		// 					AssertOrder(defShieldA_p);
		// 					AssertOrder(crfShieldA_p);
		// 					AssertOrder(defMWeaponA_p);
		// 					AssertOrder(crfMWeaponA_p);
		// 					AssertOrder(defQuiverA_p);
		// 					AssertOrder(defPackA_p);
		// 					AssertOrder(crfWearA_p);
		// 					AssertOrder(defParts_p);
		// 					AssertOrder(defWearB_p);
		// 					AssertOrder(crfParts_p);
		// 		/*	Focused on Equipped	*/
		// 			/*	states */
		// 				ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(crfBowA_p, Slottable.FocusedState);// picked
		// 				ASSB(defBowB_p, Slottable.FocusedState);
		// 				ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(crfShieldA_p, Slottable.FocusedState);
		// 				ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);//hovered
		// 				ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 				ASSB(defQuiverA_p, Slottable.FocusedState);
		// 				ASSB(defPackA_p, Slottable.FocusedState);
		// 				ASSB(crfWearA_p, Slottable.FocusedState);
		// 				ASSB(defParts_p, Slottable.FocusedState);
		// 				ASSB(defWearB_p, Slottable.FocusedState);
		// 				ASSB(crfParts_p, Slottable.FocusedState);
		// 			PickUp(crfBowA_p, out picked);
		// 			SimHover(defMWeaponA_p, sgpAll, eventData);
		// 				ASSB(defMWeaponA_p, Slottable.EquippedAndSelectedState);
		// 			crfBowA_p.OnPointerUpMock(eventData);
		// 			crfBowA_p.CurProcess.Expire();
		// 			CompleteAllSBProcesses(sgpAll);
		// 				AssertFocused();
		// 					AssertOrderReset();
		// 					AssertOrder(defBowA_p);
		// 					AssertOrder(defBowB_p);
		// 					AssertOrder(defWearA_p);
		// 					AssertOrder(defShieldA_p);
		// 					AssertOrder(crfShieldA_p);
		// 					AssertOrder(defMWeaponA_p);
		// 					AssertOrder(crfBowA_p);
		// 					AssertOrder(crfMWeaponA_p);
		// 					AssertOrder(defQuiverA_p);
		// 					AssertOrder(defPackA_p);
		// 					AssertOrder(crfWearA_p);
		// 					AssertOrder(defParts_p);
		// 					AssertOrder(defWearB_p);
		// 					AssertOrder(crfParts_p);
		// 		/*	Equipped on Focused	*/
		// 			/*	states */
		// 				ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(defBowB_p, Slottable.FocusedState);
		// 				ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);// picked
		// 				ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(crfShieldA_p, Slottable.FocusedState);
		// 				ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(crfBowA_p, Slottable.FocusedState);
		// 				ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 				ASSB(defQuiverA_p, Slottable.FocusedState);
		// 				ASSB(defPackA_p, Slottable.FocusedState);
		// 				ASSB(crfWearA_p, Slottable.FocusedState);// hovered
		// 				ASSB(defParts_p, Slottable.FocusedState);
		// 				ASSB(defWearB_p, Slottable.FocusedState);
		// 				ASSB(crfParts_p, Slottable.FocusedState);
		// 			PickUp(defWearA_p, out picked);
		// 			SimHover(crfWearA_p, sgpAll, eventData);
		// 			defWearA_p.OnPointerUpMock(eventData);
		// 			defWearA_p.CurProcess.Expire();
		// 			CompleteAllSBProcesses(sgpAll);
		// 			/*	order */
		// 				AssertFocused();
		// 					AssertOrderReset();
		// 					AssertOrder(defBowA_p);
		// 					AssertOrder(defBowB_p);
		// 					AssertOrder(defShieldA_p);
		// 					AssertOrder(crfShieldA_p);
		// 					AssertOrder(defMWeaponA_p);
		// 					AssertOrder(crfBowA_p);
		// 					AssertOrder(crfMWeaponA_p);
		// 					AssertOrder(defQuiverA_p);
		// 					AssertOrder(defPackA_p);
		// 					AssertOrder(crfWearA_p);
		// 					AssertOrder(defWearA_p);
		// 					AssertOrder(defParts_p);
		// 					AssertOrder(defWearB_p);
		// 					AssertOrder(crfParts_p);
		// 		/*	Equipped on Equipped (reverse) */
		// 			/*	reverse */
		// 				/*	states */
		// 					ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 					ASSB(defBowB_p, Slottable.FocusedState);
		// 					ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);// hovered
		// 					ASSB(crfShieldA_p, Slottable.FocusedState);
		// 					ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
		// 					ASSB(crfBowA_p, Slottable.FocusedState);
		// 					ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 					ASSB(defQuiverA_p, Slottable.FocusedState);
		// 					ASSB(defPackA_p, Slottable.FocusedState);
		// 					ASSB(crfWearA_p, Slottable.FocusedState);
		// 					ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);// picked
		// 					ASSB(defParts_p, Slottable.FocusedState);
		// 					ASSB(defWearB_p, Slottable.FocusedState);
		// 					ASSB(crfParts_p, Slottable.FocusedState);
		// 				PickUp(defWearA_p, out picked);
		// 				SimHover(defShieldA_p, sgpAll, eventData);
		// 				defWearA_p.OnPointerUpMock(eventData);
		// 				defWearA_p.CurProcess.Expire();
		// 				CompleteAllSBProcesses(sgpAll);
		// 				/*	order */
		// 					AssertFocused();
		// 						AssertOrderReset();
		// 						AssertOrder(defBowA_p);
		// 						AssertOrder(defBowB_p);
		// 						AssertOrder(defWearA_p);
		// 						AssertOrder(defShieldA_p);
		// 						AssertOrder(crfShieldA_p);
		// 						AssertOrder(defMWeaponA_p);
		// 						AssertOrder(crfBowA_p);
		// 						AssertOrder(crfMWeaponA_p);
		// 						AssertOrder(defQuiverA_p);
		// 						AssertOrder(defPackA_p);
		// 						AssertOrder(crfWearA_p);
		// 						AssertOrder(defParts_p);
		// 						AssertOrder(defWearB_p);
		// 						AssertOrder(crfParts_p);
		// 		/*	Focused on Focused (reverse) */
		// 			AssertFocused();
		// 				AssertOrderReset();
		// 				AssertOrder(defBowA_p);
		// 				AssertOrder(defBowB_p);
		// 				AssertOrder(defWearA_p);
		// 				AssertOrder(defShieldA_p);
		// 				AssertOrder(crfShieldA_p);
		// 				AssertOrder(defMWeaponA_p);
		// 				AssertOrder(crfBowA_p);
		// 				AssertOrder(crfMWeaponA_p);
		// 				AssertOrder(defQuiverA_p);
		// 				AssertOrder(defPackA_p);
		// 				AssertOrder(crfWearA_p);
		// 				AssertOrder(defParts_p);
		// 				AssertOrder(defWearB_p);
		// 				AssertOrder(crfParts_p);
		// 			PickUp(crfParts_p, out picked);
		// 			SimHover(defBowB_p, sgpAll, eventData);
		// 			crfParts_p.OnPointerUpMock(eventData);
		// 			crfParts_p.CurProcess.Expire();
		// 			CompleteAllSBProcesses(sgpAll);
		// 			AssertFocused();
		// 				AssertOrderReset();
		// 				AssertOrder(defBowA_p);
		// 				AssertOrder(crfParts_p);
		// 				AssertOrder(defBowB_p);
		// 				AssertOrder(defWearA_p);
		// 				AssertOrder(defShieldA_p);
		// 				AssertOrder(crfShieldA_p);
		// 				AssertOrder(defMWeaponA_p);
		// 				AssertOrder(crfBowA_p);
		// 				AssertOrder(crfMWeaponA_p);
		// 				AssertOrder(defQuiverA_p);
		// 				AssertOrder(defPackA_p);
		// 				AssertOrder(crfWearA_p);
		// 				AssertOrder(defParts_p);
		// 				AssertOrder(defWearB_p);
		// 		}
		// 	public void TestVolSortWhileAutoSort(){
		// 		/*	voluntary sort while auto sort on	*/
		// 			sgpAll.ToggleAutoSort(true);
		// 			sgCGears.ToggleAutoSort(true);
		// 				AssertFocused();
		// 				AssertOrderReset();
		// 					AssertOrder(defBowA_p);
		// 					AssertOrder(defBowB_p);
		// 					AssertOrder(crfBowA_p);
		// 					AssertOrder(defWearA_p);
		// 					AssertOrder(defWearB_p);
		// 					AssertOrder(crfWearA_p);
		// 					AssertOrder(defShieldA_p);
		// 					AssertOrder(crfShieldA_p);
		// 					AssertOrder(defMWeaponA_p);
		// 					AssertOrder(crfMWeaponA_p);
		// 					AssertOrder(defQuiverA_p);
		// 					AssertOrder(defPackA_p);
		// 					AssertOrder(defParts_p);
		// 					AssertOrder(crfParts_p);
		// 			sgm.SortSG(sgpAll, SlotGroup.InverseItemIDSorter);
		// 				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
		// 				AE(sgm.SelectedSGDoneTransaction, false);
		// 				AE(sgm.Transaction.GetType(), typeof(SortTransaction));
		// 				ASSG(sgpAll, SlotGroup.SortingState);
		// 				AssertMoveSlotIndex(sgpAll, defBowA_p, 0, 13, false);
		// 				AssertMoveSlotIndex(sgpAll, defBowB_p, 1, 12, false);
		// 				AssertMoveSlotIndex(sgpAll, crfBowA_p, 2, 11, false);
		// 				AssertMoveSlotIndex(sgpAll, defWearA_p, 3, 10, false);
		// 				AssertMoveSlotIndex(sgpAll, defWearB_p, 4, 9, false);
		// 				AssertMoveSlotIndex(sgpAll, crfWearA_p, 5, 8, false);
		// 				AssertMoveSlotIndex(sgpAll, defShieldA_p, 6, 7, false);
		// 				AssertMoveSlotIndex(sgpAll, crfShieldA_p, 7, 6, false);
		// 				AssertMoveSlotIndex(sgpAll, defMWeaponA_p, 8, 5, false);
		// 				AssertMoveSlotIndex(sgpAll, crfMWeaponA_p, 9, 4, false);
		// 				AssertMoveSlotIndex(sgpAll, defQuiverA_p, 10, 3, false);
		// 				AssertMoveSlotIndex(sgpAll, defPackA_p, 11, 2, false);
		// 				AssertMoveSlotIndex(sgpAll, defParts_p, 12, 1, false);
		// 				AssertMoveSlotIndex(sgpAll, crfParts_p, 13, 0, false);
		// 			CompleteAllSBProcesses(sgpAll);
		// 				AE(sgm.SelectedSGDoneTransaction, true);
		// 				AssertFocused();/*	transaction no done? */
		// 				AssertOrderReset();
		// 					AssertOrder(crfParts_p);
		// 					AssertOrder(defParts_p);
		// 					AssertOrder(defPackA_p);
		// 					AssertOrder(defQuiverA_p);
		// 					AssertOrder(crfMWeaponA_p);
		// 					AssertOrder(defMWeaponA_p);
		// 					AssertOrder(crfShieldA_p);
		// 					AssertOrder(defShieldA_p);
		// 					AssertOrder(crfWearA_p);
		// 					AssertOrder(defWearB_p);
		// 					AssertOrder(defWearA_p);
		// 					AssertOrder(crfBowA_p);
		// 					AssertOrder(defBowB_p);
		// 					AssertOrder(defBowA_p);
		// 	}
		// 	public void TestUnequipTransaction(){
		// 		/*	Unequip Transaction	*/
		// 			/*	AutoSort off	*/
		// 			AE(sgpAll.SlotMovements.Count , 0);
		// 			AE(sgCGears.SlotMovements.Count , 0);
		// 				sgpAll.ToggleAutoSort(false);
		// 				sgCGears.ToggleAutoSort(false);
		// 				AssertFocused();
		// 				AECGears(defShieldA_p, crfShieldA_p, defMWeaponA_p, defQuiverA_p);
		// 				defShieldA_e = sgCGears.GetSlottable(defShieldA_p.Item);
		// 			PickUp(defShieldA_e, out picked);
		// 			SimHover(null, sgpAll, eventData);
		// 				AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
		// 				AE(sgm.PickedSB, defShieldA_e);
		// 				AE(sgm.SelectedSB, null);
		// 				AE(sgm.SelectedSG, sgpAll);
		// 			defShieldA_e.OnPointerUpMock(eventData);
		// 				AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
		// 				ASSGM(sgm, SlotGroupManager.PerformingTransactionState);
		// 				ASSB(defShieldA_e, Slottable.MovingState);
		// 				ASSG(sgpAll, SlotGroup.SelectedState);
		// 				ASSG(sgCGears, SlotGroup.FocusedState);
		// 				AE(sgm.PickedSBDoneTransaction, false);
		// 				AE(sgm.SelectedSBDoneTransaction, true);
		// 				AE(sgm.OrigSGDoneTransaction, true);
		// 				AE(sgm.SelectedSGDoneTransaction, true);
		// 				AP<SGMActionProcess>(sgm, false);
		// 				AP<SBRemovingProcess>(defShieldA_e, false);
		// 				AP<SGHighlightProcess>(sgpAll, false);
		// 				AP<SGDehighlightProcess>(sgCGears, false);
		// 				crfShieldA_e = sgCGears.GetSlottable(crfShieldA_p.Item);
		// 				defMWeaponA_e = sgCGears.GetSlottable(defMWeaponA_p.Item);
		// 				defQuiverA_e = sgCGears.GetSlottable(defQuiverA_p.Item);
		// 				AB(sgCGears.GetSlotMovement(crfShieldA_e) == null, true);
		// 				AB(sgCGears.GetSlotMovement(defMWeaponA_e) == null, true);
		// 				AB(sgCGears.GetSlotMovement(defQuiverA_e) == null, true);
		// 			ExpireProcesses(defShieldA_e, null, sgCGears, sgpAll);
		// 				AssertFocused();
		// 				AECGears(null, crfShieldA_p, defMWeaponA_p, defQuiverA_p);
		// 	}
		// 	public void TestUnequipTransactionFuck(){
		// 		sgpAll.ToggleAutoSort(false);
		// 		sgCGears.ToggleAutoSort(false);
		// 		defShieldA_e = sgCGears.GetSlottable(defShieldA_p.Item);
		// 		PickUp(defShieldA_e, out picked);
		// 		SimHover(null, sgpAll, eventData);
		// 			AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
		// 			AE(sgm.PickedSB, defShieldA_e);
		// 			AE(sgm.SelectedSB, null);
		// 			AE(sgm.SelectedSG, sgpAll);
		// 		defShieldA_e.OnPointerUpMock(eventData);
		// 			AT<UnequipTransaction>(false);
		// 			ASSGM(sgm, SlotGroupManager.PerformingTransactionState);
		// 			ASSB(defShieldA_e, Slottable.MovingState);
		// 			ASSG(sgpAll, SlotGroup.SelectedState);
		// 			ASSG(sgCGears, SlotGroup.FocusedState);
		// 			AE(sgm.PickedSBDoneTransaction, false);
		// 			AE(sgm.SelectedSBDoneTransaction, true);
		// 			AE(sgm.OrigSGDoneTransaction, true);
		// 			AE(sgm.SelectedSGDoneTransaction, true);
		// 			AP<SGMActionProcess>(sgm, false);
		// 			AP<SBRemovingProcess>(defShieldA_e, false);
		// 			AP<SGHighlightProcess>(sgpAll, false);
		// 			AP<SGDehighlightProcess>(sgCGears, false);
		// 			defMWeaponA_e = sgCGears.GetSlottable(defMWeaponA_p.Item);
		// 			Assert.That(sgCGears.GetSlotMovement(defMWeaponA_e), Is.Null);

		// 		ExpireProcesses(defShieldA_e, null, null, null);
				
		// 			AssertFocused();
		// 				AECGears(null, defMWeaponA_p, null, null);

		// 	}
		// 	public void TestUnequipWhileAutoSort(){
		// 			sgpAll.ToggleAutoSort(true);
		// 			sgCGears.ToggleAutoSort(true);
		// 			AssertFocused();
		// 			AECGears(null, crfShieldA_p, defMWeaponA_p, defQuiverA_p);
		// 		defMWeaponA_e = sgCGears.GetSlottable(defMWeaponA_p.Item);
		// 		PickUp(defMWeaponA_e, out picked);
		// 		SimHover(null, sgpAll, eventData);
		// 			AT<UnequipTransaction>(false);
		// 			ASSGM(sgm, SlotGroupManager.ProbingState);
		// 			ASSG(sgpAll, SlotGroup.SelectedState);
		// 			ASSG(sgCGears, SlotGroup.FocusedState);
		// 		defMWeaponA_e.OnPointerUpMock(eventData);
		// 			AECGears(null, crfShieldA_p, null, defQuiverA_p);
		// 			ASSGM(sgm, SlotGroupManager.PerformingTransactionState);
		// 			ASSG(sgpAll, SlotGroup.SortingState);
		// 			ASSG(sgCGears, SlotGroup.SortingState);
		// 			AP<SGMActionProcess>(sgm, false);
		// 			AP<SGSortingProcess>(sgpAll, false);
		// 			AP<SGSortingProcess>(sgCGears, false);
		// 			AB(sgm.CurProcess.IsExpired, false);
		// 			AB(sgpAll.CurProcess.IsExpired, true);
		// 			AB(sgCGears.CurProcess.IsExpired, false);
		// 			AssertMoveSlotIndex(sgCGears, crfShieldA_p, 1, 0, false);
		// 			AssertMoveSlotIndex(sgCGears, defQuiverA_p, 3, 1, false);
		// 		ExpireProcesses(defMWeaponA_e, null, sgCGears, null);
		// 			AssertFocused();
		// 			AECGears(crfShieldA_p, defQuiverA_p, null, null);

		// 	}
		// 	public void TestSwapTransactionWhileAutoSort(){
		// 		/*	AutoSort on and Swap Equip while not maxed	*/
		// 			sgpAll.ToggleAutoSort(true);
		// 			sgCGears.ToggleAutoSort(true);
		// 				AssertFocused();
		// 				AECGears(crfShieldA_p, defQuiverA_p, null, null);
		// 			FillEquip(defShieldA_p, sgCGears);
		// 				AECGears(defShieldA_p, crfShieldA_p, defQuiverA_p, null);
					
		// 				AssertFocused();
		// 			PickUp(defPackA_p, out picked);
		// 			defShieldA_e = sgCGears.GetSlottable(defShieldA_p.Item);
		// 			SimHover(defShieldA_e, sgCGears, eventData);
		// 				AT<SwapTransaction>(false);
		// 			defPackA_p.OnPointerUpMock(eventData);

		// 				AECGears(defPackA_p, crfShieldA_p, defQuiverA_p, null);
		// 					AssertMoveSlotIndex(sgCGears, crfShieldA_p, 1, 0, false);
		// 					AssertMoveSlotIndex(sgCGears, defQuiverA_p, 2, 1, false);
		// 					AssertMoveSlotIndex(sgCGears, defPackA_p, 0, 2, false);
		// 			ExpireProcesses(defPackA_p, defShieldA_e, sgpAll, sgCGears);
		// 				AssertFocused();
		// 				AECGears(crfShieldA_p, defQuiverA_p, defPackA_p, null);
		// 		/*	while maxed	*/
		// 				AssertFocused();
		// 				AECGears(crfShieldA_p, defQuiverA_p, defPackA_p, null);
		// 			FillEquip(crfMWeaponA_p, sgCGears);
		// 				AECGears(crfShieldA_p, crfMWeaponA_p ,defQuiverA_p, defPackA_p);
					
		// 			PickUp(defMWeaponA_p, out picked);
		// 			defPackA_e = sgCGears.GetSlottable(defPackA_p.Item);
		// 			SimHover(defPackA_e, sgCGears, eventData);
		// 				AT<SwapTransaction>(false);
		// 			defMWeaponA_p.OnPointerUpMock(eventData);
		// 				AECGears(crfShieldA_p, crfMWeaponA_p ,defQuiverA_p, defMWeaponA_p);
		// 					AssertMoveSlotIndex(sgCGears, crfShieldA_p, 0, 0, true);
		// 					AssertMoveSlotIndex(sgCGears, crfMWeaponA_p, 1, 2, false);
		// 					AssertMoveSlotIndex(sgCGears, defQuiverA_p, 2, 3, false);
		// 					AssertMoveSlotIndex(sgCGears, defMWeaponA_p, 3, 1, false);
		// 			ExpireProcesses(defMWeaponA_p, defPackA_e, sgpAll, sgCGears);
		// 				AssertFocused();
		// 				AECGears(crfShieldA_p, defMWeaponA_p, crfMWeaponA_p ,defQuiverA_p);
		// 	}
		// 	public void TestFillEquipWhileAutoSort(){
		// 		/*	AutoSort on and Fill Equip	*/
		// 				AECGears(defShieldA_p, defMWeaponA_p, defQuiverA_p, null);
		// 			sgpAll.ToggleAutoSort(true);
		// 			sgCGears.ToggleAutoSort(true);

		// 			PickUp(crfShieldA_p, out picked);
		// 			SimHover(null, sgCGears, eventData);
		// 			crfShieldA_p.OnPointerUpMock(eventData);
		// 				crfShieldA_e = sgCGears.GetSlottable(crfShieldA_p.Item);
		// 				AECGears(defShieldA_p, defMWeaponA_p, defQuiverA_p, crfShieldA_p);
		// 				AE(sgCGears.SlotMovements.Count, 4);
		// 					AssertMoveSlotIndex(sgCGears, defShieldA_p, 0, 0, true);
		// 					AssertMoveSlotIndex(sgCGears, crfShieldA_p, 3, 1, false);
		// 					AssertMoveSlotIndex(sgCGears, defMWeaponA_p, 1, 2, false);
		// 					AssertMoveSlotIndex(sgCGears, defQuiverA_p, 2, 3, false);
		// 			ExpireProcesses(crfShieldA_p, null, sgpAll, sgCGears);
		// 				AssertFocused();
		// 				AECGears(defShieldA_p, crfShieldA_p, defMWeaponA_p, defQuiverA_p);
		// 	}
		// 	public void TestRevisedTransactionUpdate(){
		// 		/*	AutoSort off
		// 		*/
		// 		/*	SwapTransaction	*/
		// 			sgCGears.ToggleAutoSort(false);
		// 			sgpAll.ToggleAutoSort(false);
		// 				AECGears(defShieldA_p, defMWeaponA_p, null, null);
		// 			defShieldA_e = sgCGears.GetSlottable(defShieldA_p.Item);
		// 			PickUp(defQuiverA_p, out picked);
		// 			SimHover(defShieldA_e, sgCGears, eventData);
		// 				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
		// 				Slottable pickedSB;
		// 				SlotGroup origSG;
		// 				Slottable selectedSB;
		// 				SlotGroup selectedSG;
		// 				((SwapTransaction)sgm.Transaction).CheckFields(out pickedSB, out origSG, out selectedSB, out selectedSG);
		// 				AE(pickedSB, defQuiverA_p);
		// 				AE(origSG, sgpAll);
		// 				AE(selectedSB, defShieldA_e);
		// 				AE(selectedSG, sgCGears);

		// 				AE(sgm.PickedSB, defQuiverA_p);
		// 				AE(sgm.SelectedSB, defShieldA_e);
		// 				AE(sgm.SelectedSG, sgCGears);

		// 			defQuiverA_p.OnPointerUpMock(eventData);
		// 				AE(sgm.PickedSB, defQuiverA_p);
		// 				AE(sgm.SelectedSB, defShieldA_e);
		// 				AE(sgm.SelectedSG, sgCGears);

		// 				ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);

		// 				AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));
		// 				AE(sgpAll.CurProcess.GetType(), typeof(SGDehighlightProcess));
		// 				AE(sgCGears.CurProcess.GetType(), typeof(SGHighlightProcess));
		// 				AE(defQuiverA_p.CurProcess.GetType(), typeof(SBEquippingProcess));
		// 				AE(defShieldA_e.CurProcess.GetType(), typeof(SBRemovingProcess));
		// 			ExpireProcesses(defQuiverA_p, defShieldA_e, sgpAll, sgCGears);
		// 				AE(sgm.PickedSBDoneTransaction, true);
		// 				AE(sgm.OrigSGDoneTransaction, true);
		// 				AE(sgm.SelectedSBDoneTransaction, true);
		// 				AE(sgm.SelectedSGDoneTransaction, true);
		// 				AECGears(defQuiverA_p, defMWeaponA_p, null, null);
		// 				AB(defShieldA_p.IsEquipped, false);

						
		// 				ASSB(defShieldA_p, Slottable.FocusedState);
		// 				AssertFocused();
		// 		/*	FillEquip	*/
		// 			PickUp(defShieldA_p, out picked);
		// 				AB(picked, true);
		// 			SimHover(null, sgCGears, eventData);
		// 				AB(sgm.Transaction == null, false);
		// 				AE(sgm.Transaction.GetType(), typeof(FillEquipTransaction));
		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 				AECGears(defQuiverA_p, defMWeaponA_p, null, null);
		// 			defShieldA_p.OnPointerUpMock(eventData);
		// 				AECGears(defQuiverA_p, defMWeaponA_p, defShieldA_p, null);
		// 					ASSG(sgpAll, SlotGroup.FocusedState);
		// 					ASSG(sgCGears, SlotGroup.SelectedState);
		// 			ExpireProcesses(defShieldA_p, null, sgpAll, sgCGears);
		// 				AssertFocused();
		// 				AECGears(defQuiverA_p, defMWeaponA_p, defShieldA_p, null);
		// 		/*	Voluntary Sort	*/
		// 			sgm.SortSG(sgCGears, SlotGroup.ItemIDSorter);
		// 				AE(sgCGears.SlotMovements.Count, 3);
		// 				AssertMoveSlotIndex(sgCGears, defQuiverA_p, 0, 2, false);
		// 				AssertMoveSlotIndex(sgCGears, defMWeaponA_p, 1, 1, true);
		// 				AssertMoveSlotIndex(sgCGears, defShieldA_p, 2, 0, false);
		// 			CompleteAllSBProcesses(sgCGears);
		// 				AECGears(defShieldA_p, defMWeaponA_p, defQuiverA_p, null);
		// 		/*	*/
		// 			TestFillEquipWhileAutoSort();
		// 			TestVolSortWhileAutoSort();
		// 			TestUnequipTransaction();
		// 			TestUnequipWhileAutoSort();
		// 			TestSwapTransactionWhileAutoSort();
		// 	}
		// 	public void TestVoluntarySortOnSGCGears(){
		// 			AECGears(defShieldA_p, defMWeaponA_p, null, null);
		// 		PickUp(defQuiverA_p, out picked);
		// 		SimHover(null, sgCGears, eventData);
		// 		defQuiverA_p.OnPointerUpMock(eventData);
		// 		// ExpireProcesses();
		// 		ExpireProcesses(defQuiverA_p, null, sgpAll, sgCGears);
		// 			AECGears(defShieldA_p, defMWeaponA_p, defQuiverA_p, null);
				
		// 		PickUp(crfShieldA_p, out picked);
		// 		defShieldA_e = sgCGears.GetSlottable(defShieldA_p.Item);
		// 		SimHover(defShieldA_e, sgCGears, eventData);
		// 		crfShieldA_p.OnPointerUpMock(eventData);
		// 		// ExpireProcesses();
		// 		ExpireProcesses(crfShieldA_p, defShieldA_e, sgpAll, sgCGears);
		// 			AECGears(crfShieldA_p, defMWeaponA_p, defQuiverA_p, null);
		// 			crfShieldA_e = sgCGears.GetSlottable(crfShieldA_p.Item);
		// 			defMWeaponA_e = sgCGears.GetSlottable(defMWeaponA_p.Item);
		// 			defQuiverA_e = sgCGears.GetSlottable(defQuiverA_p.Item);
		// 			AssertOrderReset();
		// 			AssertOrder(crfShieldA_e);
		// 			AssertOrder(defMWeaponA_e);
		// 			AssertOrder(defQuiverA_e);
				
		// 		PickUp(defShieldA_p, out picked);
		// 		SimHover(null, sgCGears, eventData);
		// 		defShieldA_p.OnPointerUpMock(eventData);
		// 		// ExpireProcesses();
		// 		ExpireProcesses(defShieldA_p, null, sgpAll, sgCGears);
		// 			AECGears(crfShieldA_p, defMWeaponA_p, defQuiverA_p, defShieldA_p);
				
		// 		sgm.SortSG(sgCGears, SlotGroup.ItemIDSorter);
		// 			defShieldA_e = sgCGears.GetSlottable(defShieldA_p.Item);
		// 			AssertMoveSlotIndex(sgCGears, crfShieldA_p, 0, 1, false);
		// 			AssertMoveSlotIndex(sgCGears, defMWeaponA_p, 1, 2, false);
		// 			AssertMoveSlotIndex(sgCGears, defQuiverA_p, 2, 3, false);
		// 			AssertMoveSlotIndex(sgCGears, defShieldA_p, 3, 0, false);
		// 		CompleteAllSBProcesses(sgCGears);
		// 			AssertOrderReset();
		// 			AssertOrder(defShieldA_e);
		// 			AssertOrder(crfShieldA_e);
		// 			AssertOrder(defMWeaponA_e);
		// 			AssertOrder(defQuiverA_e);
		// 			AECGears(defShieldA_p, crfShieldA_p, defMWeaponA_p, defQuiverA_p);
					

		// 	}
		// 	public void TestVoluntarySortOnSGPAll(){
		// 		AssertOrderReset();
		// 			AssertOrder(defBowA_p);
		// 			AssertOrder(defBowB_p);
		// 			AssertOrder(crfBowA_p);
		// 			AssertOrder(defWearA_p);
		// 			AssertOrder(defWearB_p);
		// 			AssertOrder(crfWearA_p);
		// 			AssertOrder(defShieldA_p);
		// 			AssertOrder(crfShieldA_p);
		// 			AssertOrder(defMWeaponA_p);
		// 			AssertOrder(crfMWeaponA_p);
		// 			AssertOrder(defQuiverA_p);
		// 			AssertOrder(defPackA_p);
		// 			AssertOrder(defParts_p);
		// 			AssertOrder(crfParts_p);
		// 		sgm.SortSG(sgpAll, SlotGroup.AcquisitionOrderSorter);
		// 			AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
		// 			AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));
		// 			AE(sgm.Transaction.GetType(), typeof(SortTransaction));
		// 			AE(sgm.PickedSB, null);
		// 			AE(sgm.SelectedSB, null);
		// 			AE(sgm.SelectedSG, null);

		// 			ASSG(sgpAll, SlotGroup.SortingState);
		// 			AE(sgpAll.CurProcess.GetType(), typeof(SGSortingProcess));

		// 			AE(sgpAll.SlotMovements.Count, 14);
		// 			AssertMoveSlotIndex(sgpAll, defBowA_p, 0, 0, true);
		// 			AssertMoveSlotIndex(sgpAll, defBowB_p, 1, 1, true);
		// 			AssertMoveSlotIndex(sgpAll, crfBowA_p, 2, 2, true);
		// 			AssertMoveSlotIndex(sgpAll, defWearA_p, 3, 3, true);
		// 			AssertMoveSlotIndex(sgpAll, defWearB_p, 4, 4, true);
		// 			AssertMoveSlotIndex(sgpAll, crfWearA_p, 5, 5, true);
		// 			AssertMoveSlotIndex(sgpAll, defShieldA_p, 6, 8, false);
		// 			AssertMoveSlotIndex(sgpAll, crfShieldA_p, 7, 9, false);
		// 			AssertMoveSlotIndex(sgpAll, defMWeaponA_p, 8, 10, false);
		// 			AssertMoveSlotIndex(sgpAll, crfMWeaponA_p, 9, 11, false);
		// 			AssertMoveSlotIndex(sgpAll, defQuiverA_p, 10, 12, false);
		// 			AssertMoveSlotIndex(sgpAll, defPackA_p, 11, 13, false);
		// 			AssertMoveSlotIndex(sgpAll, defParts_p, 12, 6, false);
		// 			AssertMoveSlotIndex(sgpAll, crfParts_p, 13, 7, false);
					

		// 		CompleteAllSBProcesses(sgpAll);
		// 			AB(sgm.SelectedSGDoneTransaction, true);
		// 			AE(sgm.CurState, SlotGroupManager.FocusedState);
		// 		AssertOrderReset();
		// 			AssertOrder(defBowA_p);
		// 			AssertOrder(defBowB_p);
		// 			AssertOrder(crfBowA_p);
		// 			AssertOrder(defWearA_p);
		// 			AssertOrder(defWearB_p);
		// 			AssertOrder(crfWearA_p);
		// 			AssertOrder(defParts_p);
		// 			AssertOrder(crfParts_p);
		// 			AssertOrder(defShieldA_p);
		// 			AssertOrder(crfShieldA_p);
		// 			AssertOrder(defMWeaponA_p);
		// 			AssertOrder(crfMWeaponA_p);
		// 			AssertOrder(defQuiverA_p);
		// 			AssertOrder(defPackA_p);
					
		// 		AssertFocused();

		// 		sgm.SortSG(sgpAll, SlotGroup.ItemIDSorter);
		// 			AssertMoveSlotIndex(sgpAll, defBowA_p, 0, 0, true);
		// 			AssertMoveSlotIndex(sgpAll, defBowB_p, 1, 1, true);
		// 			AssertMoveSlotIndex(sgpAll, crfBowA_p, 2, 2, true);
		// 			AssertMoveSlotIndex(sgpAll, defWearA_p, 3, 3, true);
		// 			AssertMoveSlotIndex(sgpAll, defWearB_p, 4, 4, true);
		// 			AssertMoveSlotIndex(sgpAll, crfWearA_p, 5, 5, true);
		// 			AssertMoveSlotIndex(sgpAll, defParts_p, 6, 12, false);
		// 			AssertMoveSlotIndex(sgpAll, crfParts_p, 7, 13, false);
		// 			AssertMoveSlotIndex(sgpAll, defShieldA_p, 8, 6, false);
		// 			AssertMoveSlotIndex(sgpAll, crfShieldA_p, 9, 7, false);
		// 			AssertMoveSlotIndex(sgpAll, defMWeaponA_p, 10, 8, false);
		// 			AssertMoveSlotIndex(sgpAll, crfMWeaponA_p, 11, 9, false);
		// 			AssertMoveSlotIndex(sgpAll, defQuiverA_p, 12, 10, false);
		// 			AssertMoveSlotIndex(sgpAll, defPackA_p, 13, 11, false);
		// 		CompleteAllSBProcesses(sgpAll);
				
		// 		AssertOrderReset();
		// 			AssertOrder(defBowA_p);
		// 			AssertOrder(defBowB_p);
		// 			AssertOrder(crfBowA_p);
		// 			AssertOrder(defWearA_p);
		// 			AssertOrder(defWearB_p);
		// 			AssertOrder(crfWearA_p);
		// 			AssertOrder(defShieldA_p);
		// 			AssertOrder(crfShieldA_p);
		// 			AssertOrder(defMWeaponA_p);
		// 			AssertOrder(crfMWeaponA_p);
		// 			AssertOrder(defQuiverA_p);
		// 			AssertOrder(defPackA_p);
		// 			AssertOrder(defParts_p);
		// 			AssertOrder(crfParts_p);
		// 	}
		// 	public void TestInstantSort(){
		// 		// PrintItems(sgpAll);
		// 			AssertOrderReset();
		// 			AssertOrder(defBowA_p);
		// 			AssertOrder(defBowB_p);
		// 			AssertOrder(crfBowA_p);
		// 			AssertOrder(defWearA_p);
		// 			AssertOrder(defWearB_p);
		// 			AssertOrder(crfWearA_p);
		// 			AssertOrder(defShieldA_p);
		// 			AssertOrder(crfShieldA_p);
		// 			AssertOrder(defMWeaponA_p);
		// 			AssertOrder(crfMWeaponA_p);
		// 			AssertOrder(defQuiverA_p);
		// 			AssertOrder(defPackA_p);
		// 			AssertOrder(defParts_p);
		// 			AssertOrder(crfParts_p);
		// 		sgpAll.SetSorter(SlotGroup.AcquisitionOrderSorter);
		// 		sgpAll.InstantSort();
		// 		// PrintItems(sgpAll);
		// 			AssertOrderReset();
		// 			AssertOrder(defBowA_p);
		// 			AssertOrder(defBowB_p);
		// 			AssertOrder(crfBowA_p);
		// 			AssertOrder(defWearA_p);
		// 			AssertOrder(defWearB_p);
		// 			AssertOrder(crfWearA_p);
		// 			AssertOrder(defParts_p);
		// 			AssertOrder(crfParts_p);
		// 			AssertOrder(defShieldA_p);
		// 			AssertOrder(crfShieldA_p);
		// 			AssertOrder(defMWeaponA_p);
		// 			AssertOrder(crfMWeaponA_p);
		// 			AssertOrder(defQuiverA_p);
		// 			AssertOrder(defPackA_p);
		// 	}
		// 	public void TestSwap(){
		// 		// TestHover();
		// 		/*
		// 		*/
		// 		/*	fill equipping defQuiverA_p
		// 		*/
		// 			AE(sgCGears.Slots.Count, 4);
		// 			AE(sgm.GetEquippedCarriedGears().Count, 2);
					
					
		// 			AB(sgm.GetEquippedCarriedGears().Contains((CarriedGearInstanceMock)defShieldA_e.Item), true);
		// 			AB(sgm.GetEquippedCarriedGears().Contains((CarriedGearInstanceMock)defMWeaponA_e.Item), true);
					
		// 			AssertFocused();

		// 			PickUp(defQuiverA_p, out picked);
		// 				//AssertPostPickFilter(defQuiverA_p);
		// 				ASSB(defQuiverA_p, Slottable.PickedAndSelectedState);
		// 				AE(sgm.PickedSB, defQuiverA_p);
		// 				AE(sgm.SelectedSB, defQuiverA_p);
		// 				AE(sgm.SelectedSG, sgpAll);
		// 				AE(sgm.Transaction.GetType(), typeof(RevertTransaction));

		// 			sgm.SimSBHover(null, eventData);
		// 			sgm.SimSGHover(sgCGears, eventData);

		// 				ASSB(defQuiverA_p, Slottable.PickedAndDeselectedState);
		// 				AE(sgm.PickedSB, defQuiverA_p);
		// 				AE(sgm.SelectedSB, null);
		// 				AE(sgm.SelectedSG, sgCGears);
		// 				AE(sgm.Transaction.GetType(), typeof(FillEquipTransaction));

		// 				AE(sgm.PickedSBDoneTransaction, true);
		// 				AE(sgm.SelectedSBDoneTransaction, true);
		// 				AE(sgm.OrigSGDoneTransaction, true);
		// 				AE(sgm.SelectedSGDoneTransaction, true);

		// 				AE(sgm.CurState, SlotGroupManager.ProbingState);
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMProbingStateProcess));

		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 				ASSG(sgCGears, SlotGroup.SelectedState);
					
		// 			/*	pre transaction
		// 			*/
		// 				AB(defQuiverA_p.IsEquipped, false);
		// 				AB(sgCGears.Inventory.Items.Contains(defQuiverA_p.Item), false);
		// 				AE(sgCGears.GetSlottable((InventoryItemInstanceMock)defQuiverA_p.Item), null);

		// 			defQuiverA_p.OnPointerUpMock(eventData);
		// 			/*	post transaction
		// 			*/
		// 				AE(sgm.PickedSBDoneTransaction, false);
		// 				AE(sgm.SelectedSBDoneTransaction, true);
		// 				AE(sgm.OrigSGDoneTransaction, false);
		// 				AE(sgm.SelectedSGDoneTransaction, false);

		// 				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));
		// 				AE(sgm.GetSlotGroup(defQuiverA_p), sgpAll);
						
		// 				AB(sgpAll.Inventory.Items.Contains(defQuiverA_p.Item), true);
		// 				AE(sgpAll.Slots.Count, 14);
		// 				AE(sgpAll.GetSlottable((InventoryItemInstanceMock)defQuiverA_p.Item), defQuiverA_p);
		// 				AB(sgCGears.Inventory.Items.Contains(defQuiverA_p.Item), true);
		// 				AE(sgCGears.Slots.Count, 4);
		// 				AB(sgCGears.GetSlottable((InventoryItemInstanceMock)defQuiverA_p.Item) != null, false);

						
		// 				ASSB(defQuiverA_p, Slottable.EquippingState);
		// 				AE(defQuiverA_p.PrevState, Slottable.PickedAndDeselectedState);
		// 				AB(defQuiverA_p.IsEquipped, false);
		// 				AE(defQuiverA_p.CurProcess.GetType(), typeof(SBEquippingProcess));
		// 				AE(defQuiverA_p.DestinationSG, sgCGears);
		// 				AE(defQuiverA_p.DestinationSlot, /*sgCGears.GetSlot(defQuiverA_e)*/sgCGears.GetNextEmptySlot());
					

		// 			/*	pre completion SBA
		// 			*/
		// 			defQuiverA_p.CurProcess.Expire();
		// 			/*	post completion SBA
		// 			*/
		// 				AB(sgm.PickedSBDoneTransaction, true);
		// 				AB(sgm.SelectedSBDoneTransaction, true);
		// 				AB(sgm.OrigSGDoneTransaction, false);
		// 				AB(sgm.SelectedSGDoneTransaction, false);
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));
		// 				AB(sgm.CurProcess.IsRunning, true);
		// 				AB(sgm.CurProcess.IsExpired, false);
						
		// 				ASSG(sgpAll, SlotGroup.PerformingTransactionState);
		// 				AE(sgpAll.CurProcess.GetType(), typeof(SGUpdateActionProcess));
		// 				AB(sgpAll.CurProcess.IsRunning, true);
		// 				AB(sgpAll.CurProcess.IsExpired, false);
		// 				ASSG(sgCGears, SlotGroup.PerformingTransactionState);
		// 				AE(sgCGears.CurProcess.GetType(), typeof(SGUpdateActionProcess));
		// 				AB(sgCGears.CurProcess.IsRunning, true);
		// 				AB(sgCGears.CurProcess.IsExpired, false);

		// 				ASSB(defQuiverA_p, Slottable.EquippingState);
		// 				AE(defQuiverA_p.PrevState, Slottable.PickedAndDeselectedState);//
		// 				AE(defQuiverA_p.CurProcess.GetType(), typeof(SBEquippingProcess));
		// 				AB(defQuiverA_p.CurProcess.IsRunning, false);
		// 				AB(defQuiverA_p.CurProcess.IsExpired, true);

		// 			sgpAll.CurProcess.Expire();
		// 				AB(sgm.PickedSBDoneTransaction, true);
		// 				AB(sgm.SelectedSBDoneTransaction, true);
		// 				AB(sgm.OrigSGDoneTransaction, true);
		// 				AB(sgm.SelectedSGDoneTransaction, false);
		// 				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));

		// 				ASSG(sgpAll, SlotGroup.PerformingTransactionState);
		// 				AE(sgpAll.CurProcess.GetType(), typeof(SGUpdateActionProcess));
		// 				AE(sgpAll.CurProcess.IsRunning, false);
		// 				AE(sgpAll.CurProcess.IsExpired, true);
						
		// 				ASSG(sgCGears, SlotGroup.PerformingTransactionState);
		// 				AE(sgCGears.CurProcess.GetType(), typeof(SGUpdateActionProcess));
		// 				AE(sgCGears.CurProcess.IsRunning, true);
		// 				AE(sgCGears.CurProcess.IsExpired, false);

		// 			sgCGears.CurProcess.Expire();
		// 			Slottable defQuiverA_e = sgCGears.GetSlottable((InventoryItemInstanceMock)defQuiverA_p.Item);
		// 				AB(sgm.PickedSBDoneTransaction, true);
		// 				AB(sgm.SelectedSBDoneTransaction, true);
		// 				AB(sgm.OrigSGDoneTransaction, true);
		// 				AB(sgm.SelectedSGDoneTransaction, true);
		// 				AE(sgm.CurState, SlotGroupManager.FocusedState);
		// 				AE(sgm.CurProcess, null);

		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 				AE(sgpAll.PrevState, SlotGroup.PerformingTransactionState);
		// 				AE(sgpAll.CurProcess, null);
						
		// 				ASSG(sgCGears, SlotGroup.FocusedState);
		// 				AE(sgCGears.PrevState, SlotGroup.PerformingTransactionState);
		// 				AE(sgCGears.CurProcess, null);
						
		// 				AB(defQuiverA_p.IsEquipped, true);
		// 				ASSB(defQuiverA_p, Slottable.EquippedAndDefocusedState);
		// 				AE(defQuiverA_p.PrevState, Slottable.EquippingState);
		// 				AE(defQuiverA_p.CurProcess, null);
		// 				AB(defQuiverA_e.IsEquipped, true);
		// 				ASSB(defQuiverA_e, Slottable.EquippedAndDeselectedState);

		// 				AE(sgm.GetEquippedCarriedGears().Count , 3);
		// 		/*	fill equipping defPackA_p
		// 		*/
		// 				AssertFocused();
		// 			PickUp(defPackA_p, out picked);
		// 			sgm.SimSBHover(null, eventData);
		// 			sgm.SimSGHover(sgCGears, eventData);
		// 				AE(sgm.PickedSB, defPackA_p);
		// 				AE(sgm.SelectedSB, null);
		// 				AE(sgm.SelectedSG, sgCGears);

		// 				AE(sgm.CurState, SlotGroupManager.ProbingState);
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMProbingStateProcess));
		// 				AE(sgm.Transaction.GetType(), typeof(FillEquipTransaction));

		// 				AE(sgpAll.PrevState, SlotGroup.SelectedState);
		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 				AE(sgpAll.CurProcess.GetType(), typeof(SGDehighlightProcess));

		// 				AE(sgCGears.PrevState, SlotGroup.FocusedState);
		// 				ASSG(sgCGears, SlotGroup.SelectedState);
		// 				AE(sgCGears.CurProcess.GetType(), typeof(SGHighlightProcess));

		// 				AE(defPackA_p.PrevState, Slottable.PickedAndSelectedState);
		// 				ASSB(defPackA_p, Slottable.PickedAndDeselectedState);
		// 				AE(defPackA_p.CurProcess.GetType(), typeof(SBDehighlightProcess));
		// 				AE(defPackA_p.DestinationSG, null);
		// 				AE(defPackA_p.DestinationSlot, null);

		// 				AB(sgCGears.GetNextEmptySlot() != null, true);
					
		// 			defPackA_p.OnPointerUpMock(eventData);
		// 			sgpAll.CurProcess.Expire();
		// 			defPackA_p.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();

		// 				AE(sgm.PickedSB, null);
		// 				AE(sgm.SelectedSB, null);
		// 				AE(sgm.SelectedSG, null);

		// 				AE(sgm.CurState, SlotGroupManager.FocusedState);
		// 				AE(sgm.CurProcess, null);
		// 				AE(sgm.Transaction, null);

		// 				AE(sgpAll.PrevState, SlotGroup.PerformingTransactionState);
		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 				AE(sgpAll.CurProcess, null);

		// 				AE(sgCGears.PrevState, SlotGroup.PerformingTransactionState);
		// 				ASSG(sgCGears, SlotGroup.FocusedState);
		// 				AE(sgCGears.CurProcess, null);

		// 				AE(defPackA_p.PrevState, Slottable.EquippingState);
		// 				ASSB(defPackA_p, Slottable.EquippedAndDefocusedState);
		// 				AE(defPackA_p.CurProcess, null);
		// 				AB(defPackA_p.IsEquipped, true);
		// 				AE(defPackA_p.DestinationSG, null);
		// 				AE(defPackA_p.DestinationSlot, null);

		// 				AB(sgCGears.GetNextEmptySlot() != null, false);
		// 				AE(sgm.GetEquippedCarriedGears().Count, 4);
		// 				AB(sgCGears.GetSlottable((InventoryItemInstanceMock)defPackA_p.Item) == null, false);

		// 				Slottable defPackA_e = sgCGears.GetSlottable((InventoryItemInstanceMock)defPackA_p.Item);
		// 				ASSB(defPackA_e, Slottable.EquippedAndDeselectedState);

		// 				AssertFocused();
		// 		/*	make sure no more Fill transaction is allowed
		// 		*/
		// 			AB(crfShieldA_e == null, true);
		// 			AB(sgm.GetSlotGroup(crfShieldA_p), sgpAll);
		// 				AE(sgCGears.GetNextEmptySlot(), null);
		// 				AB(sgCGears.AcceptsFilter(crfShieldA_p), true);
		// 				AB(sgCGears.Filter is SGCarriedGearFilter, true);
		// 			PickUp(crfShieldA_p, out picked);
		// 				ASSG(sgCGears, SlotGroup.DefocusedState);
		// 			sgm.SimSBHover(null, eventData);
		// 			sgm.SimSGHover(sgCGears, eventData);

		// 				AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
		// 				AE(sgm.PickedSB, crfShieldA_p);
		// 				AE(sgm.SelectedSB, null);
		// 				AE(sgm.SelectedSG, null);
		// 				AE(sgCGears.GetNextEmptySlot(), null);
		// 			crfShieldA_p.OnPointerUpMock(eventData);
		// 				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));
		// 				AE(crfShieldA_p.CurProcess.GetType(), typeof(SBUnpickingProcess));
		// 			crfShieldA_p.CurProcess.Expire();
		// 		/*	remove defShieldA_e
		// 		*/
		// 			AssertFocused();
		// 			PickUp(defShieldA_e, out picked);
		// 				AE(sgm.CurState, SlotGroupManager.ProbingState);
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMProbingStateProcess));
		// 				AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
		// 				AE(sgm.PickedSB, defShieldA_e);
		// 				AE(sgm.SelectedSB, defShieldA_e);
		// 				AE(sgm.SelectedSG, sgCGears);

		// 				ASSG(sgCGears, SlotGroup.SelectedState);
		// 				AE(sgCGears.PrevState, SlotGroup.FocusedState);
		// 				AE(sgCGears.CurProcess.GetType(),typeof(SGHighlightProcess));

		// 				ASSB(defShieldA_e, Slottable.PickedAndSelectedState);
		// 				AE(defShieldA_e.PrevState, Slottable.WaitForPickUpState);
		// 				AE(defShieldA_e.CurProcess.GetType(), typeof(SBPickUpProcess));

		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 			sgm.SimSBHover(null, eventData);
		// 			sgm.SimSGHover(sgpAll, eventData);
		// 				AE(sgm.CurState, SlotGroupManager.ProbingState);
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMProbingStateProcess));
		// 				AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
		// 				AE(sgm.PickedSB, defShieldA_e);
		// 				AE(sgm.SelectedSB, null);
		// 				AE(sgm.SelectedSG, sgpAll);

		// 				ASSG(sgCGears, SlotGroup.FocusedState);
		// 				AE(sgCGears.PrevState, SlotGroup.SelectedState);
		// 				AE(sgCGears.CurProcess.GetType(),typeof(SGDehighlightProcess));
		// 				AB(sgCGears.Inventory.Items.Contains(defShieldA_p.Item), true);
		// 				AB(sgCGears.GetSlottable(defShieldA_e.Item), defShieldA_e);

		// 				ASSB(defShieldA_e, Slottable.PickedAndDeselectedState);
		// 				AE(defShieldA_e.PrevState, Slottable.PickedAndSelectedState);
		// 				AE(defShieldA_e.CurProcess.GetType(), typeof(SBDehighlightProcess));

		// 				ASSG(sgpAll, SlotGroup.SelectedState);
		// 				AE(sgpAll.PrevState, SlotGroup.FocusedState);
		// 				AE(sgpAll.CurProcess.GetType(), typeof(SGHighlightProcess));
		// 			defShieldA_e.OnPointerUpMock(eventData);
		// 				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));
		// 				AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
		// 				AE(sgm.PickedSB, defShieldA_e);
		// 				AE(sgm.SelectedSB, null);
		// 				AE(sgm.SelectedSG, sgpAll);
		// 				AE(sgm.PickedSBDoneTransaction, false);
		// 				AE(sgm.SelectedSBDoneTransaction, true);
		// 				AE(sgm.OrigSGDoneTransaction, false);
		// 				AE(sgm.SelectedSGDoneTransaction, false);

		// 				EquipmentSet focusedEquipSet = (EquipmentSet)sgm.RootPage.EquipBundle.GetFocusedBundleElement();
		// 				AB(focusedEquipSet.ContainsElement(sgCGears), true);

		// 				ASSG(sgCGears, SlotGroup.PerformingTransactionState);
		// 				AE(sgCGears.PrevState, SlotGroup.FocusedState);
		// 				AE(sgCGears.CurProcess.GetType(),typeof(SGUpdateActionProcess));
		// 				AE(sgCGears.Inventory.Items.Contains(defShieldA_p.Item), false);

		// 				ASSB(defShieldA_e, Slottable.RemovingState);
		// 				AE(defShieldA_e.PrevState, Slottable.PickedAndDeselectedState);
		// 				AE(defShieldA_e.CurProcess.GetType(), typeof(SBRemovingProcess));
		// 				AE(defShieldA_e.DestinationSG, sgpAll);
		// 				AE(defShieldA_e.DestinationSlot, sgpAll.GetSlot(defShieldA_p));//

		// 				AE(sgm.GetSlotGroup(defShieldA_e), sgCGears);
		// 				AE(defShieldA_e.IsEquipped, true);
		// 				AE(defShieldA_p.IsEquipped, true);

		// 				ASSG(sgpAll, SlotGroup.PerformingTransactionState);
		// 				AE(sgpAll.PrevState, SlotGroup.SelectedState);
		// 				AE(sgpAll.CurProcess.GetType(), typeof(SGUpdateActionProcess));
					
		// 				AE(sgm.PickedSBDoneTransaction, false);
		// 				AE(sgm.SelectedSBDoneTransaction, true);
		// 				AE(sgm.OrigSGDoneTransaction, false);
		// 				AE(sgm.SelectedSGDoneTransaction, false);
		// 			defShieldA_e.CurProcess.Expire();
		// 				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));
		// 				AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
		// 				AE(sgm.PickedSB, defShieldA_e);
		// 				AE(sgm.SelectedSB, null);
		// 				AE(sgm.SelectedSG, sgpAll);
		// 				AE(sgm.PickedSBDoneTransaction, true);
		// 				AE(sgm.SelectedSBDoneTransaction, true);
		// 				AE(sgm.OrigSGDoneTransaction, false);
		// 				AE(sgm.SelectedSGDoneTransaction, false);
		// 			sgpAll.CurProcess.Expire();
		// 				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));
		// 				AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
		// 				AE(sgm.PickedSB, defShieldA_e);
		// 				AE(sgm.SelectedSB, null);
		// 				AE(sgm.SelectedSG, sgpAll);
		// 				AE(sgm.PickedSBDoneTransaction, true);
		// 				AE(sgm.SelectedSBDoneTransaction, true);
		// 				AE(sgm.OrigSGDoneTransaction, false);
		// 				AE(sgm.SelectedSGDoneTransaction, true);
		// 			sgCGears.CurProcess.Expire();
		// 				AE(sgm.PickedSBDoneTransaction, true);
		// 				AE(sgm.SelectedSBDoneTransaction, true);
		// 				AE(sgm.OrigSGDoneTransaction, true);
		// 				AE(sgm.SelectedSGDoneTransaction, true);
		// 				AE(sgm.CurProcess, null);
		// 				AE(sgm.Transaction, null);
		// 				AE(sgm.PickedSB, null);
		// 				AE(sgm.SelectedSB, null);
		// 				AE(sgm.SelectedSG, null);
		// 				AE(sgm.CurState, SlotGroupManager.FocusedState);

		// 				ASSG(sgCGears, SlotGroup.FocusedState);
		// 				AE(sgCGears.PrevState, SlotGroup.PerformingTransactionState);
		// 				AE(sgCGears.CurProcess, null);
		// 				AB(sgCGears.Inventory.Items.Contains(defShieldA_p.Item), false);
		// 				AE(sgCGears.Slots.Count, 4);
		// 				AE(sgm.GetEquippedCarriedGears().Count, 3);
		// 				AE(sgCGears.GetSlottable((InventoryItemInstanceMock)defShieldA_p.Item), null);
		// 				AB(sgCGears.GetNextEmptySlot() != null, true);

		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 				AE(sgpAll.PrevState, SlotGroup.PerformingTransactionState);
		// 				AE(sgpAll.CurProcess, null);
		// 				AB(sgpAll.Inventory.Items.Contains(defShieldA_p.Item), true);
				
		// 				AB(defShieldA_e == null, true);
		// 				AE(defShieldA_p.IsEquipped, false);
		// 				ASSB(defShieldA_p, Slottable.FocusedState);
		// 		/*	remove defMWeaponA_e
		// 		*/
		// 			// AssertFocused();
		// 			AB(sgCGears.Inventory.Items.Contains(defMWeaponA_p.Item), true);
		// 			AE(sgm.GetEquippedCarriedGears().Count, 3);
		// 			PickUp(defMWeaponA_e, out picked);
		// 			sgm.SimSBHover(null, eventData);
		// 			sgm.SimSGHover(sgpAll, eventData);
		// 			defMWeaponA_e.OnPointerUpMock(eventData);
		// 			defMWeaponA_e.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			AE(sgm.GetEquippedCarriedGears().Count, 2);
		// 			AE(sgm.GetCGEmptySlots().Count, 2);
		// 			AB(defMWeaponA_e != null, false);
		// 			AB(defMWeaponA_p.IsEquipped, false);
		// 			AB(sgpAll.Inventory.Items.Contains(defMWeaponA_p.Item), true);
		// 			AB(sgCGears.Inventory.Items.Contains(defMWeaponA_p.Item), false);
		// 			//AM(defMWeaponA_p, sgCGears, false);
		// 			//AM(defShieldA_p, sgCGears, false);
		// 			//AM(defQuiverA_p, sgCGears, true);
		// 			//AM(defPackA_p, sgCGears, true);
		// 			//AM(defBowA_p, sgBow, true);
		// 			//AM(defWearA_p, sgWear, true);
		// 		/*	remove defQuiverA_e
		// 		*/
		// 			PickUp(defQuiverA_e, out picked);
		// 			sgm.SimSBHover(null, eventData);
		// 			sgm.SimSGHover(sgpAll, eventData);
		// 			defQuiverA_e.OnPointerUpMock(eventData);
		// 			defQuiverA_e.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 				AB(defQuiverA_p.IsEquipped, false);
		// 				AE(sgm.GetCGEmptySlots().Count, 3);
		// 				AE(sgm.GetEquippedCarriedGears().Count, 1);
		// 				AB(defQuiverA_e == null, true);
		// 				//AM(defQuiverA_p, sgCGears, false);
		// 				//AM(defPackA_p, sgCGears, true);
		// 		/*	remove defPackA_e
		// 		*/
		// 			PickUp(defPackA_e, out picked);
		// 			sgm.SimSBHover(null, eventData);
		// 			sgm.SimSGHover(sgpAll, eventData);
		// 			defPackA_e.OnPointerUpMock(eventData);
		// 			defPackA_e.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 				AB(defPackA_p.IsEquipped, false);
		// 				AE(sgm.GetCGEmptySlots().Count, 4);
		// 				AE(sgm.GetEquippedCarriedGears().Count, 0);
		// 				//AM(defPackA_p, sgCGears, false);
		// 		/*	fill equip defShieldA_p
		// 		*/	
		// 			AB(crfShieldA_e == null, true);
		// 			AB(crfShieldA_p.IsEquipped, false);
		// 			PickUp(crfShieldA_p, out picked);
		// 			sgm.SimSBHover(null, eventData);
		// 			sgm.SimSGHover(sgCGears, eventData);
		// 			crfShieldA_p.OnPointerUpMock(eventData);
		// 			crfShieldA_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();
		// 				crfShieldA_e = sgCGears.GetSlottable(crfShieldA_p.Item);
		// 				AB(crfShieldA_p.IsEquipped, true);
		// 				//AM(crfShieldA_p, sgCGears, true);
		// 				AE(sgm.GetCGEmptySlots().Count, 3);
		// 				AE(sgm.GetEquippedCarriedGears().Count, 1);
		// 		/*	fill equip crfMWeaponA_p
		// 		*/
		// 			AB(crfMWeaponA_e == null, true);
		// 			AB(crfMWeaponA_p.IsEquipped, false);
		// 			PickUp(crfMWeaponA_p, out picked);
		// 			sgm.SimSBHover(null, eventData);
		// 			sgm.SimSGHover(sgCGears, eventData);
		// 			crfMWeaponA_p.OnPointerUpMock(eventData);
		// 			crfMWeaponA_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();
		// 				crfMWeaponA_e = sgCGears.GetSlottable(crfMWeaponA_p.Item);
		// 				AB(crfMWeaponA_p.IsEquipped, true);
		// 				//AM(crfMWeaponA_p, sgCGears, true);
		// 				AE(sgm.GetCGEmptySlots().Count, 2);
		// 				AE(sgm.GetEquippedCarriedGears().Count, 2);
		// 		/*	swap equip crfBowA_p
		// 		*/
		// 			PickUp(crfBowA_p, out picked);
		// 				ASSG(sgBow, SlotGroup.FocusedState);
		// 				AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
		// 			// sgm.SimSBHover(null, eventData);
		// 			// sgm.SimSGHover(sgBow, eventData);
		// 			SimHover(null, sgBow, eventData);
		// 				AE(sgm.PickedSB, crfBowA_p);
		// 				AE(sgm.SelectedSB, defBowA_e);//
		// 				AE(sgm.SelectedSG, sgBow);
		// 				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
		// 			crfBowA_p.OnPointerUpMock(eventData);
		// 				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
		// 				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));
		// 					AE(sgm.PickedSBDoneTransaction, false);
		// 					AE(sgm.SelectedSBDoneTransaction, false);
		// 					AE(sgm.OrigSGDoneTransaction, false);
		// 					AE(sgm.SelectedSGDoneTransaction, false);

		// 				ASSG(sgpAll, SlotGroup.PerformingTransactionState);
		// 				AE(sgpAll.CurProcess.GetType(), typeof(SGUpdateActionProcess));

		// 				ASSG(sgBow, SlotGroup.PerformingTransactionState);
		// 				AE(sgBow.CurProcess.GetType(), typeof(SGUpdateActionProcess));

		// 				ASSB(defBowA_e, Slottable.RemovingState);
		// 				AE(defBowA_e.CurProcess.GetType(), typeof(SBRemovingProcess));
		// 				ASSB(crfBowA_p, Slottable.EquippingState);
		// 				AE(crfBowA_p.CurProcess.GetType(), typeof(SBEquippingProcess));

		// 				AE(crfBowA_p.DestinationSG, sgBow);
		// 				AE(crfBowA_p.DestinationSlot, sgBow.Slots[0]);
		// 				AE(defBowA_e.DestinationSG, sgpAll);
		// 				AE(defBowA_e.DestinationSlot, sgpAll.GetSlot(crfBowA_p));
		// 			crfBowA_p.CurProcess.Expire();
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));
		// 					AE(sgm.PickedSBDoneTransaction, true);
		// 					AE(sgm.SelectedSBDoneTransaction, false);
		// 					AE(sgm.OrigSGDoneTransaction, false);
		// 					AE(sgm.SelectedSGDoneTransaction, false);
		// 				AE(crfBowA_p.DestinationSG, null);
		// 				AE(crfBowA_p.DestinationSlot, null);

		// 			defBowA_e.CurProcess.Expire();
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));
		// 					AE(sgm.PickedSBDoneTransaction, true);
		// 					AE(sgm.SelectedSBDoneTransaction, true);
		// 					AE(sgm.OrigSGDoneTransaction, false);
		// 					AE(sgm.SelectedSGDoneTransaction, false);
		// 				AE(defBowA_e.DestinationSG, null);
		// 				AE(defBowA_e.DestinationSlot, null);

		// 			sgpAll.CurProcess.Expire();
		// 				AE(sgm.CurProcess.GetType(), typeof(SGMActionProcess));
		// 					AE(sgm.PickedSBDoneTransaction, true);
		// 					AE(sgm.SelectedSBDoneTransaction, true);
		// 					AE(sgm.OrigSGDoneTransaction, true);
		// 					AE(sgm.SelectedSGDoneTransaction, false);

		// 			sgBow.CurProcess.Expire();
		// 				AE(sgm.CurState, SlotGroupManager.FocusedState);
		// 				AE(sgm.CurProcess, null);
		// 					AE(sgm.PickedSBDoneTransaction, true);
		// 					AE(sgm.SelectedSBDoneTransaction, true);
		// 					AE(sgm.OrigSGDoneTransaction, true);
		// 					AE(sgm.SelectedSGDoneTransaction, true);
		// 				AB(sgm.Transaction == null, true);

		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 					AE(sgpAll.Slots.Count, 14);
		// 				ASSG(sgBow, SlotGroup.FocusedState);
		// 					AE(sgBow.Slots.Count, 1);
						
		// 				AE(sgBow.Inventory.Items.Contains(defBowA_p.Item), false);
		// 				AE(defBowA_p.IsEquipped, false);
		// 				ASSB(defBowA_p, Slottable.FocusedState);
		// 					AB(defBowA_p.IsEquipped, false);
		// 					AB(defBowA_e == null, true);
		// 				Slottable crfBowA_e = sgBow.GetSlottable(crfBowA_p.Item);
		// 				ASSB(crfBowA_e, Slottable.EquippedAndDeselectedState);
		// 					AB(crfBowA_e.IsEquipped, true);
		// 					AB(crfBowA_p.IsEquipped, true);
						
		// 				AE(sgm.GetSlotGroup(defBowA_p), sgpAll);
		// 				AE(sgm.GetSlotGroup(crfBowA_e), sgBow);
		// 				AE(sgm.GetEquippedBow(), crfBowA_e.Item);
							
		// 		/*	swapping some more
		// 		*/
		// 			PickUp(defBowB_p, out picked);
		// 			SimHover(null, sgBow, eventData);
		// 			defBowB_p.OnPointerUpMock(eventData);
		// 			defBowB_p.CurProcess.Expire();
		// 			crfBowA_e.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgBow.CurProcess.Expire();
		// 				AE(sgm.CurState, SlotGroupManager.FocusedState);
		// 				AE(sgm.CurProcess, null);
		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 				ASSG(sgBow, SlotGroup.FocusedState);
		// 				ASSB(defBowB_p, Slottable.EquippedAndDefocusedState);
		// 				Slottable defBowB_e = sgBow.GetSlottable(defBowB_p.Item);
		// 				ASSB(defBowB_e, Slottable.EquippedAndDeselectedState);
		// 				AB(crfBowA_e == null, true);
		// 				ASSB(crfBowA_p, Slottable.FocusedState);

		// 				AE(sgm.GetEquippedBow(), defBowB_p.Item);
		// 				AE(sgBow.Slots[0].Sb, defBowB_e);
		// 			PickUp(defBowA_p, out picked);
		// 			SimHover(null, sgBow, eventData);
		// 			defBowA_p.OnPointerUpMock(eventData);
		// 			defBowA_p.CurProcess.Expire();
		// 			defBowB_e.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgBow.CurProcess.Expire();
		// 				AE(sgm.CurState, SlotGroupManager.FocusedState);
		// 				AE(sgm.CurProcess, null);
		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 				ASSG(sgBow, SlotGroup.FocusedState);
		// 				ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 				defBowA_e = sgBow.GetSlottable(defBowA_p.Item);
		// 				ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
		// 				AB(defBowB_e == null, true);
		// 				ASSB(defBowB_p, Slottable.FocusedState);

		// 				AE(sgm.GetEquippedBow(), defBowA_p.Item);
		// 				AE(sgBow.Slots[0].Sb, defBowA_e);
		// 		/*	swap equip defWearB_p and then to crfWearA_p
		// 		*/
		// 			PickUp(defWearB_p, out picked);
		// 			SimHover(null, sgWear, eventData);
		// 			defWearB_p.OnPointerUpMock(eventData);
		// 			defWearB_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgWear.CurProcess.Expire();
		// 			defWearA_e.CurProcess.Expire();
		// 				AE(sgm.CurState, SlotGroupManager.FocusedState);
		// 				AE(sgm.CurProcess, null);
		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 				ASSG(sgWear, SlotGroup.FocusedState);
		// 				ASSB(defWearB_p, Slottable.EquippedAndDefocusedState);
		// 				Slottable defWearB_e = sgWear.GetSlottable(defWearB_p.Item);
		// 				ASSB(defWearB_e, Slottable.EquippedAndDeselectedState);
		// 				AB(defWearA_e == null, true);
		// 				ASSB(defWearA_p, Slottable.FocusedState);
		// 				AE(sgm.GetEquippedWear(), defWearB_p.Item);
		// 				AE(sgWear.Slots[0].Sb, defWearB_e);
		// 			PickUp(crfWearA_p, out picked);
		// 			SimHover(null, sgWear, eventData);
		// 			crfWearA_p.OnPointerUpMock(eventData);
		// 			crfWearA_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgWear.CurProcess.Expire();
		// 			defWearB_e.CurProcess.Expire();
		// 				AE(sgm.CurState, SlotGroupManager.FocusedState);
		// 				AE(sgm.CurProcess, null);
		// 				ASSG(sgpAll, SlotGroup.FocusedState);
		// 				ASSG(sgWear, SlotGroup.FocusedState);
		// 				ASSB(crfWearA_p, Slottable.EquippedAndDefocusedState);
		// 				Slottable crfWearA_e = sgWear.GetSlottable(crfWearA_p.Item);
		// 				ASSB(crfWearA_e, Slottable.EquippedAndDeselectedState);
		// 				AB(defWearB_e == null, true);
		// 				ASSB(defWearB_p, Slottable.FocusedState);
		// 				AE(sgm.GetEquippedWear(), crfWearA_p.Item);
		// 				AE(sgWear.Slots[0].Sb, crfWearA_e);
		// 		/*	swap bow explicitly on equipped sb back to defBowA_p
		// 		*/
		// 				AE(sgm.GetEquippedBow(), defBowA_e.Item);
		// 				AE(sgBow.Slots[0].Sb, defBowA_e);
		// 			PickUp(defBowB_p, out picked);
		// 			SimHover(defBowA_e, sgBow, eventData);
		// 				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
		// 			SimHover(defBowB_p, sgpAll, eventData);
		// 			bool reverted = false;
		// 			Revert(defBowB_p, out reverted);
		// 				AB(reverted, true);

		// 				//AssertEquippedBow(defBowA_p, defBowB_p);
		// 			PickUp(defBowB_p, out picked);
		// 			SimHover(defBowA_e, sgBow, eventData);
		// 				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
		// 			defBowB_p.OnPointerUpMock(eventData);
		// 			defBowB_p.CurProcess.Expire();
		// 			defBowA_e.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgBow.CurProcess.Expire();
		// 				//AssertEquippedBow(defBowB_p, defBowA_p);
					
		// 			defBowB_e = sgBow.Slots[0].Sb;
		// 			PickUp(defBowA_p, out picked);
		// 			SimHover(defBowB_e, sgBow, eventData);
		// 			defBowA_p.OnPointerUpMock(eventData);
		// 			defBowA_p.CurProcess.Expire();
		// 			defBowB_e.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgBow.CurProcess.Expire();
		// 				//AssertEquippedBow(defBowA_p, defBowB_p);
		// 		/*	swap wears explicitly to defWearB_p
		// 		*/
		// 			PickUp(defWearB_p, out picked);
		// 			SimHover(crfWearA_e, sgWear, eventData);
		// 			defWearB_p.OnPointerUpMock(eventData);
		// 			defWearB_p.CurProcess.Expire();
		// 			crfWearA_e.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgWear.CurProcess.Expire();
		// 				//AssertEquippedWear(defWearB_p, crfWearA_p);
		// 		/*	explicity swap on pool bow defBowB_p
		// 		*/
		// 			AE(sgm.GetEquippedBow(), defBowA_p.Item);
		// 			defBowA_e = sgBow.GetSlottable(defBowA_p.Item);
		// 				ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
		// 			PickUp(defBowA_e, out picked);
		// 				AB(picked, true);
		// 			SimHover(defBowB_p, sgpAll, eventData);
		// 				AB(sgm.Transaction == null, false);
		// 				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
		// 					AE(sgm.PickedSB, defBowA_e);
		// 					AE(sgm.SelectedSB, defBowB_p);
		// 					AE(sgm.SelectedSG, sgpAll);
		// 			defBowA_e.OnPointerUpMock(eventData);
		// 			defBowA_e.CurProcess.Expire();
		// 			defBowB_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgBow.CurProcess.Expire();
		// 				//AssertEquippedBow(defBowB_p, defBowA_p);
		// 			/*	make sure it reverts back when dropped on sg only
		// 			*/
		// 					defBowB_e = sgBow.GetSlottable(defBowB_p.Item);
		// 				PickUp(defBowB_e, out picked);
		// 				SimHover(null, sgpAll, eventData);
		// 					AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
		// 				Revert(defBowB_e, out reverted);
		// 					AB(reverted, true);
		// 		/*	then back to defWearA_p
		// 		*/
		// 			PickUp(defBowB_e, out picked);
		// 			SimHover(defBowA_p, sgpAll, eventData);
		// 			defBowB_e.OnPointerUpMock(eventData);
		// 			defBowB_e.CurProcess.Expire();
		// 			defBowA_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgBow.CurProcess.Expire();
		// 				//AssertEquippedBow(defBowA_p, defBowB_p);
		// 		/*	explicitly swap wear from defWearB_e to crfWearA_p
		// 		*/
		// 				AE(sgm.GetEquippedWear(), defWearB_p.Item);
		// 				defWearB_e = sgWear.Slots[0].Sb;
		// 			PickUp(defWearB_e, out picked);
		// 			SimHover(crfWearA_p, sgpAll, eventData);
		// 			defWearB_e.OnPointerUpMock(eventData);
		// 			defWearB_e.CurProcess.Expire();
		// 			crfWearA_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgWear.CurProcess.Expire();
		// 				//AssertEquippedWear(crfWearA_p, defWearB_p);
		// 				AB(picked, true);
		// 		/*	explicitly swap wear from crfWearA_e to defWearA_p
		// 		*/
		// 			crfWearA_e = sgWear.Slots[0].Sb;
		// 			PickUp(crfWearA_e, out picked);
		// 			SimHover(defWearA_p, sgpAll, eventData);
		// 			crfWearA_e.OnPointerUpMock(eventData);
		// 			crfWearA_e.CurProcess.Expire();
		// 			defWearA_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgWear.CurProcess.Expire();
		// 				//AssertEquippedWear(defWearA_p, crfWearA_p);
		// 		/*	explicitly swap from defQuiverA_p to defShieldA_e, then from defQuiverA_p to 		crfMWeaponA_e
		// 		*/
		// 				AssertFocused();
		// 				AE(sgm.GetEquippedCarriedGears().Count, 2);
		// 				AECGears(crfShieldA_p, crfMWeaponA_p, null, null);
		// 			PickUp(defShieldA_p, out picked);
		// 			crfShieldA_e = sgCGears.GetSlottable(crfShieldA_p.Item);
		// 			SimHover(crfShieldA_e, sgCGears, eventData);
		// 				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
		// 			defShieldA_p.OnPointerUpMock(eventData);
		// 			defShieldA_p.CurProcess.Expire();
		// 			crfShieldA_e.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();
		// 				AssertFocused();
		// 				AECGears(defShieldA_p, crfMWeaponA_p, null, null);
					
		// 			PickUp(defQuiverA_p, out picked);
		// 			crfMWeaponA_e = sgCGears.GetSlottable(crfMWeaponA_p.Item);
		// 			SimHover(crfMWeaponA_e, sgCGears, eventData);
		// 				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
		// 			defQuiverA_p.OnPointerUpMock(eventData);
		// 			crfMWeaponA_e.CurProcess.Expire();
		// 			defQuiverA_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();
		// 				AssertFocused();
		// 				AECGears(defQuiverA_p, defShieldA_p, null, null);
		// 		/*	explicit swap from defShieldA_e to defPackA_p, then from defPackA_e to 			crfShieldA_p
		// 		*/
		// 			defShieldA_e = sgCGears.GetSlottable(defShieldA_p.Item);
		// 			PickUp(defShieldA_e, out picked);
		// 			SimHover(defPackA_p, sgpAll, eventData);
		// 				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
		// 			defShieldA_e.OnPointerUpMock(eventData);
		// 			defShieldA_e.CurProcess.Expire();
		// 			defPackA_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();
		// 				AssertFocused();
		// 				AECGears(defQuiverA_p, defPackA_p, null, null);
					
		// 			defPackA_e = sgCGears.GetSlottable(defPackA_p.Item);
		// 			PickUp(defPackA_e, out picked);
		// 			SimHover(crfShieldA_p, sgpAll, eventData);
		// 			defPackA_e.OnPointerUpMock(eventData);
		// 			defPackA_e.CurProcess.Expire();
		// 			crfShieldA_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();
		// 				AssertFocused();
		// 				AECGears(defQuiverA_p, crfShieldA_p, null, null);
		// 		/*	fill equip defMWeaponA_p and defPackA_p
		// 		*/
		// 			PickUp(defMWeaponA_p, out picked);
		// 			SimHover(null, sgCGears, eventData);
		// 			defMWeaponA_p.OnPointerUpMock(eventData);
		// 			defMWeaponA_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();
		// 				AssertFocused();
		// 				AECGears(defQuiverA_p, crfShieldA_p, defMWeaponA_p, null);
		// 			PickUp(defPackA_p, out picked);
		// 			SimHover(null, sgCGears, eventData);
		// 			defPackA_p.OnPointerUpMock(eventData);
		// 			defPackA_p.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();
		// 				AssertFocused();
		// 				AECGears(defQuiverA_p, crfShieldA_p, defMWeaponA_p, defPackA_p);
		// 		/*	assert equipment
		// 		*/
		// 			AssertEquipped(defBowA_p);
		// 			AssertEquipped(defWearA_p);
		// 			AECGears(defQuiverA_p, crfShieldA_p, defMWeaponA_p, defPackA_p);
		// 		/*	explicitly swap from defShieldA_p to crfShieldA_e while there's no empty slot
		// 		*/	
		// 			PickUp(defShieldA_p, out picked);
		// 			crfShieldA_e = sgCGears.GetSlottable(crfShieldA_p.Item);
		// 				ASSG(sgCGears, SlotGroup.DefocusedState);
		// 			SimHover(crfShieldA_e, sgCGears, eventData);
		// 				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
		// 				ASSB(defShieldA_p, Slottable.PickedAndDeselectedState);
		// 				AB(sgm.SelectedSG == null, true);
		// 			defShieldA_p.OnPointerUpMock(eventData);
		// 			defShieldA_p.CurProcess.Expire();
		// 			crfShieldA_e.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 				ASSG(sgCGears, SlotGroup.PerformingTransactionState);
		// 				AE(sgCGears.CurProcess.GetType(), typeof(SGUpdateActionProcess));
		// 			sgCGears.CurProcess.Expire();
		// 				AB(sgm.PickedSBDoneTransaction, true);
		// 				AB(sgm.SelectedSBDoneTransaction, true);
		// 				AB(sgm.OrigSGDoneTransaction, true);
		// 				AB(sgm.SelectedSGDoneTransaction, true);
		// 				AssertFocused();
		// 				AECGears(defQuiverA_p, defShieldA_p, defMWeaponA_p, defPackA_p);
		// 		/*	explicitly swap from defMWeaponA_e to crfMWeaponA_p while maxed out
		// 		*/
		// 			defMWeaponA_e = sgCGears.GetSlottable(defMWeaponA_p.Item);
		// 			PickUp(defMWeaponA_e, out picked);
		// 			SimHover(crfMWeaponA_p, sgpAll, eventData);
		// 				AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
		// 			defMWeaponA_e.OnPointerUpMock(eventData);
					
		// 			defMWeaponA_e.CurProcess.Expire();
		// 			crfMWeaponA_p.CurProcess.Expire();
		// 			sgCGears.CurProcess.Expire();
		// 			sgpAll.CurProcess.Expire();
		// 				AssertFocused();
		// 				AECGears(defQuiverA_p, defShieldA_p, crfMWeaponA_p, defPackA_p);
		// 		AssertEquipped(defBowA_p);
		// 		AssertEquipped(defWearA_p);
		// 		AECGears(defQuiverA_p, defShieldA_p, crfMWeaponA_p, defPackA_p);
				
		// 	}
		// 	public void TestSGProcesses(){
		// 		sgm.Defocus();
		// 		sgm.Focus();

		// 		AssertFocused();
		// 			ASSG(sgpAll, SlotGroup.FocusedState);
		// 			AE(sgpAll.PrevState, SlotGroup.DefocusedState);
		// 			AE(sgpAll.CurProcess.GetType(), typeof(SGGreyinProcess));
		// 			ASSG(sgBow, SlotGroup.FocusedState);
		// 			AE(sgBow.PrevState, SlotGroup.DefocusedState);
		// 			AE(sgBow.CurProcess.GetType(), typeof(SGGreyinProcess));
		// 			ASSG(sgWear, SlotGroup.FocusedState);
		// 			AE(sgWear.PrevState, SlotGroup.DefocusedState);
		// 			AE(sgWear.CurProcess.GetType(), typeof(SGGreyinProcess));
		// 			ASSG(sgCGears, SlotGroup.FocusedState);
		// 			AE(sgCGears.PrevState, SlotGroup.DefocusedState);
		// 			AE(sgCGears.CurProcess.GetType(), typeof(SGGreyinProcess));
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 			AE(sgpParts.PrevState, SlotGroup.DefocusedState);
		// 			AE(sgpParts.CurProcess.GetType(), typeof(SGInstantGreyoutProcess));
		// 		sgm.Focus();
		// 			ASSG(sgpAll, SlotGroup.FocusedState);
		// 			AE(sgpAll.PrevState, SlotGroup.FocusedState);
		// 			AE(sgpAll.CurProcess.GetType(), typeof(SGGreyinProcess));
		// 			ASSG(sgBow, SlotGroup.FocusedState);
		// 			AE(sgBow.PrevState, SlotGroup.FocusedState);
		// 			AE(sgBow.CurProcess.GetType(), typeof(SGGreyinProcess));
		// 			ASSG(sgWear, SlotGroup.FocusedState);
		// 			AE(sgWear.PrevState, SlotGroup.FocusedState);
		// 			AE(sgWear.CurProcess.GetType(), typeof(SGGreyinProcess));
		// 			ASSG(sgCGears, SlotGroup.FocusedState);
		// 			AE(sgCGears.PrevState, SlotGroup.FocusedState);
		// 			AE(sgCGears.CurProcess.GetType(), typeof(SGGreyinProcess));
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 			AE(sgpParts.PrevState, SlotGroup.DefocusedState);
		// 			AE(sgpParts.CurProcess.GetType(), typeof(SGInstantGreyoutProcess));
		// 		sgm.Defocus();
		// 			ASSG(sgpAll, SlotGroup.DefocusedState);
		// 			AE(sgpAll.PrevState, SlotGroup.FocusedState);
		// 			AE(sgpAll.CurProcess.GetType(), typeof(SGGreyoutProcess));
		// 			ASSG(sgBow, SlotGroup.DefocusedState);
		// 			AE(sgBow.PrevState, SlotGroup.FocusedState);
		// 			AE(sgBow.CurProcess.GetType(), typeof(SGGreyoutProcess));
		// 			ASSG(sgWear, SlotGroup.DefocusedState);
		// 			AE(sgWear.PrevState, SlotGroup.FocusedState);
		// 			AE(sgWear.CurProcess.GetType(), typeof(SGGreyoutProcess));
		// 			ASSG(sgCGears, SlotGroup.DefocusedState);
		// 			AE(sgCGears.PrevState, SlotGroup.FocusedState);
		// 			AE(sgCGears.CurProcess.GetType(), typeof(SGGreyoutProcess));
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 			AE(sgpParts.PrevState, SlotGroup.DefocusedState);
		// 			AE(sgpParts.CurProcess.GetType(), typeof(SGInstantGreyoutProcess));
		// 		sgm.Deactivate();
		// 			ASSG(sgpAll, SlotGroup.DeactivatedState);
		// 			AE(sgpAll.PrevState, SlotGroup.DefocusedState);
		// 			// AE(sgpAll.CurProcess.GetType(), typeof(SGGreyoutProcess));
		// 			AB(sgpAll.CurProcess == null, true);

		// 			ASSG(sgBow, SlotGroup.DeactivatedState);
		// 			AE(sgBow.PrevState, SlotGroup.DefocusedState);
		// 			// AE(sgBow.CurProcess.GetType(), typeof(SGGreyoutProcess));
		// 			AB(sgBow.CurProcess == null, true);

		// 			ASSG(sgWear, SlotGroup.DeactivatedState);
		// 			AE(sgWear.PrevState, SlotGroup.DefocusedState);
		// 			// AE(sgWear.CurProcess.GetType(), typeof(SGGreyoutProcess));
		// 			AB(sgWear.CurProcess == null, true);
					
		// 			ASSG(sgCGears, SlotGroup.DeactivatedState);
		// 			AE(sgCGears.PrevState, SlotGroup.DefocusedState);
		// 			// AE(sgCGears.CurProcess.GetType(), typeof(SGGreyoutProcess));
		// 			AB(sgCGears.CurProcess == null, true);
					
		// 			ASSG(sgpParts, SlotGroup.DeactivatedState);
		// 			AE(sgpParts.PrevState, SlotGroup.DefocusedState);
		// 			// AE(sgpParts.CurProcess.GetType(), typeof(SGInstantGreyoutProcess));
		// 			AB(sgpParts.CurProcess == null, true);
		// 		sgm.Focus();
		// 			ASSG(sgpAll, SlotGroup.FocusedState);
		// 			AE(sgpAll.PrevState, SlotGroup.DeactivatedState);
		// 			AE(sgpAll.CurProcess.GetType(), typeof(SGInstantGreyinProcess));
		// 			ASSG(sgBow, SlotGroup.FocusedState);
		// 			AE(sgBow.PrevState, SlotGroup.DeactivatedState);
		// 			AE(sgBow.CurProcess.GetType(), typeof(SGInstantGreyinProcess));
		// 			ASSG(sgWear, SlotGroup.FocusedState);
		// 			AE(sgWear.PrevState, SlotGroup.DeactivatedState);
		// 			AE(sgWear.CurProcess.GetType(), typeof(SGInstantGreyinProcess));
		// 			ASSG(sgCGears, SlotGroup.FocusedState);
		// 			AE(sgCGears.PrevState, SlotGroup.DeactivatedState);
		// 			AE(sgCGears.CurProcess.GetType(), typeof(SGInstantGreyinProcess));
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 			AE(sgpParts.PrevState, SlotGroup.DeactivatedState);
		// 			AE(sgpParts.CurProcess.GetType(), typeof(SGInstantGreyoutProcess));
				
		// 		PickUp(defBowB_p, out picked);
		// 			ASSG(sgpAll, SlotGroup.SelectedState);
		// 			AE(sgpAll.PrevState, SlotGroup.FocusedState);
		// 			AE(sgpAll.CurProcess.GetType(), typeof(SGHighlightProcess));
		// 		sgm.SimSGHover(sgBow, eventData);
		// 			ASSG(sgpAll, SlotGroup.FocusedState);
		// 			AE(sgpAll.PrevState, SlotGroup.SelectedState);
		// 			AE(sgpAll.CurProcess.GetType(), typeof(SGDehighlightProcess));
		// 			ASSG(sgBow, SlotGroup.SelectedState);
		// 			AE(sgBow.PrevState, SlotGroup.FocusedState);
		// 			AE(sgBow.CurProcess.GetType(), typeof(SGHighlightProcess));
		// 		sgm.SimSGHover(sgpAll, eventData);
		// 		bool reverted = false;
		// 		Revert(defBowB_p, out reverted);
		// 		AssertFocused();
		// 		AB(picked, true);
		// 		AB(reverted, true);

		// 	}
		// 	public void TestSBProcesses(){
		// 		/*	FocusedState
		// 		*/
		// 			sgm.Defocus();
		// 			sgm.Focus();
		// 			ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 			AE(defBowA_p.PrevState, Slottable.EquippedAndDefocusedState);
		// 			AB(defBowA_p.CurProcess == null, true);
		// 			ASSB(defBowB_p, Slottable.FocusedState);
		// 			AE(defBowB_p.PrevState, Slottable.DefocusedState);
		// 			AE(defBowB_p.CurProcess.GetType(), typeof(SBGreyinProcess));
		// 			ASSB(defParts_p, Slottable.DefocusedState);
		// 			AE(defParts_p.PrevState, Slottable.DefocusedState);
		// 			AB(defParts_p.CurProcess == null, true);
		// 			ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
		// 			AE(defBowA_e.PrevState, Slottable.EquippedAndDefocusedState);
		// 			AE(defBowA_e.CurProcess.GetType(), typeof(SBGreyinProcess));

		// 			sgpAll.ToggleAutoSort(false);
		// 			PickUp(defBowB_p, out picked);
		// 			sgm.SimSBHover(defWearB_p, eventData);
		// 			ASSB(defWearB_p, Slottable.SelectedState);
		// 			sgm.SimSBHover(defBowB_p, eventData);
		// 			ASSB(defWearB_p, Slottable.FocusedState);
		// 			AE(defWearB_p.PrevState, Slottable.SelectedState);
		// 			AE(defWearB_p.CurProcess.GetType(), typeof(SBDehighlightProcess));
		// 			bool reverted;
		// 			Revert(defBowB_p, out reverted);

		// 			sgpAll.ToggleAutoSort(true);
		// 			AssertFocused();

		// 			ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 			defBowA_p.SetState(Slottable.FocusedState);
		// 			AE(defBowA_p.PrevState, Slottable.EquippedAndDefocusedState);
		// 			ASSB(defBowA_p, Slottable.FocusedState);
		// 			AE(defBowA_p.CurProcess.GetType(), typeof(SBUnequipAndGreyinProcess));
		// 			defBowA_p.SetState(Slottable.EquippedAndDefocusedState);
		// 			AssertFocused();
					
		// 			defBowB_p.SetState(Slottable.EquippedAndSelectedState);
		// 			defBowB_p.SetState(Slottable.FocusedState);
		// 			AE(defBowB_p.PrevState, Slottable.EquippedAndSelectedState);
		// 			ASSB(defBowB_p, Slottable.FocusedState);
		// 			AE(defBowB_p.CurProcess.GetType(), typeof(SBUnequipAndDehighlightProcess));
		// 			AssertFocused();

		// 			sgm.Deactivate();
		// 			sgm.Focus();
		// 			AE(defBowB_p.PrevState, Slottable.DeactivatedState);
		// 			ASSB(defBowB_p, Slottable.FocusedState);
		// 			AE(defBowB_p.CurProcess, null);
		// 			AssertFocused();

		// 			PickUp(defBowB_p, out picked);
		// 			defBowB_p.OnPointerUpMock(eventData);
		// 			ASSB(defBowB_p, Slottable.MovingState);
		// 			AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
		// 			AE(defBowB_p.CurProcess.GetType(), typeof(MoveProcess));
		// 			defBowB_p.CurProcess.Expire();
		// 			AssertFocused();
		// 			AE(defBowB_p.PrevState, Slottable.MovingState);
		// 			AE(defBowB_p.CurProcess.GetType(), typeof(SBUnpickProcess));
		// 		/*	EquippedAndDeselectedState
		// 		*/
		// 			AssertFocused();
		// 			sgm.Defocus();
		// 			sgm.Focus();
		// 			ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
		// 			AE(defBowA_e.PrevState, Slottable.EquippedAndDefocusedState);
		// 			AE(defBowA_e.CurProcess.GetType(), typeof(SBGreyinProcess));
		// 			sgm.Deactivate();
		// 			sgm.Focus();
		// 			ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
		// 			AE(defBowA_e.PrevState, Slottable.DeactivatedState);
		// 			AB(defBowA_e.CurProcess == null, true);

		// 			defBowA_e.SetState(Slottable.DefocusedState);
		// 			defBowA_e.SetState(Slottable.EquippedAndDeselectedState);
		// 			ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
		// 			AE(defBowA_e.PrevState, Slottable.DefocusedState);
		// 			AE(defBowA_e.CurProcess.GetType(), typeof(SBEquipAndGreyinProcess));
					
		// 			defBowA_e.SetState(Slottable.SelectedState);
		// 			defBowA_e.SetState(Slottable.EquippedAndDeselectedState);
		// 			ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
		// 			AE(defBowA_e.PrevState, Slottable.SelectedState);
		// 			AE(defBowA_e.CurProcess.GetType(), typeof(SBEquipAndDehighlightProcess));
		// 		/*	EquippedAndSelectedState
		// 		*/
		// 			AssertFocused();
		// 			sgpAll.ToggleAutoSort(false);
		// 			PickUp(defBowB_p, out picked);
		// 			sgm.SimSBHover(defBowA_p, eventData);
		// 			ASSB(defBowA_p, Slottable.EquippedAndSelectedState);
		// 			AE(defBowA_p.PrevState, Slottable.EquippedAndDeselectedState);
		// 			AE(defBowA_p.CurProcess.GetType(), typeof(SBHighlightProcess));
		// 			sgm.SimSBHover(defBowB_p, eventData);
		// 			Revert(defBowB_p, out reverted);
		// 			sgpAll.ToggleAutoSort(true);
		// 			AssertFocused();


		// 		/*	EquippedAndDefocusedState
		// 		*/
		// 			AssertFocused();
		// 			sgm.Defocus();
		// 			ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
		// 			AE(defShieldA_e.PrevState, Slottable.EquippedAndDeselectedState);
		// 			AE(defShieldA_e.CurProcess.GetType(), typeof(SBGreyoutProcess));
		// 			defShieldA_e.SetState(Slottable.FocusedState);
		// 			ASSB(defShieldA_e, Slottable.FocusedState);
		// 			defShieldA_e.SetState(Slottable.EquippedAndDefocusedState);
		// 			AE(defShieldA_e.PrevState, Slottable.FocusedState);
		// 			ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
		// 			AE(defShieldA_e.CurProcess.GetType(), typeof(SBEquipAndGreyoutProcess));
					
		// 			sgm.Focus();
		// 	}
		// /*	CarriedGearsTesting	*/
		// 	public void TestCarriedGearsSetup(){
		// 		/*	setting up
		// 		*/
		// 			AB(sgCGears != null, true);
		// 			AE(sgCGears.Filter.GetType(), typeof(SGCarriedGearFilter));
		// 			// AE(sgCGears.Sorter.GetType(), typeof(SGItemIndexSorter));
		// 			// AE(sgCGears.UpdateEquipStatusCommand.GetType(), typeof(UpdateEquipStatusForEquipSGCommand));
		// 			AE(sgCGears.Inventory, sgBow.Inventory);
		// 			AB(sgCGears.IsShrinkable, true);
		// 			AB(sgCGears.IsExpandable, false);

		// 			AE(((EquipmentSet)sgm.RootPage.EquipBundle.GetFocusedBundleElement()).Elements.Count, 3);
		// 			AB(sgm.RootPage.EquipBundle.GetFocusedBundleElement().ContainsElement(sgCGears), true);
		// 			AE(sgCGears.Slots.Count, 4);
		// 			AE(sgm.GetEquippedCarriedGears().Count, 2);

		// 			AE(sgpAll.Slots.Count, 14);
		// 			AB(defShieldA_p == null, false);
		// 			AE(sgm.GetSlotGroup(defShieldA_p), sgpAll);
		// 			AB(crfShieldA_p == null, false);
		// 			AE(sgm.GetSlotGroup(crfShieldA_p), sgpAll);
		// 			AB(defMWeaponA_p == null, false);
		// 			AE(sgm.GetSlotGroup(defMWeaponA_p), sgpAll);
		// 			AB(crfMWeaponA_p == null, false);
		// 			AE(sgm.GetSlotGroup(crfMWeaponA_p), sgpAll);
		// 			AB(defShieldA_e == null, false);
		// 			AE(sgm.GetSlotGroup(defShieldA_e), sgCGears);
		// 			AB(defMWeaponA_e == null, false);
		// 			AE(sgm.GetSlotGroup(defMWeaponA_e), sgCGears);
					
		// 			ASSG(sgpAll, SlotGroup.FocusedState);
		// 			ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(crfShieldA_p, Slottable.FocusedState);
		// 			ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(crfMWeaponA_p, Slottable.FocusedState);

		// 			ASSG(sgCGears, SlotGroup.FocusedState);
		// 			ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);

		// 			sgm.Defocus();

		// 			ASSG(sgpAll, SlotGroup.DefocusedState);
		// 			ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(crfShieldA_p, Slottable.DefocusedState);
		// 			ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(crfMWeaponA_p, Slottable.DefocusedState);

		// 			ASSG(sgCGears, SlotGroup.DefocusedState);
		// 			ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
		// 			ASSB(defMWeaponA_e, Slottable.EquippedAndDefocusedState);

		// 			sgm.Focus();
		// 			sgm.SetFocusedPoolSG(sgpParts);
		// 			ASSG(sgpAll, SlotGroup.DefocusedState);
		// 			ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(crfShieldA_p, Slottable.DefocusedState);
		// 			ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 			ASSB(crfMWeaponA_p, Slottable.DefocusedState);

		// 			ASSG(sgCGears, SlotGroup.FocusedState);
		// 			ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
					
		// 			sgm.SetFocusedPoolSG(sgpAll);
		// 	}		
		// /*	Test hover	*/
		// 	bool picked;
		// 	bool reverted;
		// 	Slottable selectedSB;
		// 	SlotGroup selectedSG;
		// 	SlotSystemTransaction transaction;
		// 	public void TestHover(){
		// 		SlotGroup origSG = sgm.GetSlotGroup(defBowB_p);
		// 		Slottable targetSB;
		// 		// /*	test reorder
		// 			// */
		// 			// AB(origSG.AutoSort, true);
		// 			// origSG.AutoSort = false;

		// 			// TestSimHover(defBowBSB_p, defWearBSB_p, origSG, out picked, out selectedSBChanged, out selectedSGChanged, out transaction);
		// 			// 	AB(picked, true);
		// 			// 	AB(selectedSBChanged, true);
		// 			// 	AB(selectedSGChanged, false);
		// 			// 	AE(transaction.GetType(), typeof(ReorderTransaction));
					
		// 			// TestSimHover(defBowBSB_p, crfBowASB_p, origSG, out picked, out selectedSBChanged, out selectedSGChanged, out transaction);
		// 			// 	AB(picked, true);
		// 			// 	AB(selectedSBChanged, true);
		// 			// 	AB(selectedSGChanged, false);
		// 			// 	AE(transaction.GetType(), typeof(ReorderTransaction));
		// 		// /*	test swap
		// 			// */
		// 			// TestSimHover(defBowBSB_p, defBowASB_e, sgBow, out picked, out selectedSBChanged, out selectedSGChanged, out transaction);
		// 			// 	AB(picked, true);
		// 			// 	AB(selectedSBChanged, true);
		// 			// 	AB(selectedSGChanged, true);
		// 			// 	AE(transaction.GetType(), typeof(SwapTransaction));
					
		// 			// TestSimHover(defWearBSB_p, defWearASB_e, sgWear, out picked, out selectedSBChanged, out selectedSGChanged, out transaction);
		// 			// 	AB(picked, true);
		// 			// 	AB(selectedSBChanged, true);
		// 			// 	AB(selectedSGChanged, true);
		// 			// 	AE(transaction.GetType(), typeof(SwapTransaction));
		// 		/**/

		// 		// TestPostPickFilter();
		// 		TestSimHoverOnAllSB();
		// 			// TestSimHoverOnAllSB(defBowASB_p);
		// 			// TestSimHoverOnAllSB(defBowBSB_p);
		// 			// TestSimHoverOnAllSB(crfBowASB_p);
		// 			// TestSimHoverOnAllSB(defWearASB_p);
		// 			// TestSimHoverOnAllSB(defWearBSB_p);
		// 			// TestSimHoverOnAllSB(crfWearASB_p);
		// 			// TestSimHoverOnAllSB(defBowASB_e);
		// 			// TestSimHoverOnAllSB(defWearASB_e);
		// 			// TestSimHoverOnAllSB(defBowASB_p);
				
					
		// 		/*
		// 		*/
		// 			// TestHoverDefBowASBE();
		// 			// TestHoverDefBowASBP();
		// 			// TestHoverDefBowBSBP();
		// 			// TestHoverDefWearASBE();
		// 			//TestHoverPartsInSGPParts();
		// 			// TestHoverDefShieldA();
		// 	}
		// 	public void TestPostPickFilter(){
		// 		// AB(sgpAll.m_autoSort, true);
		// 		bool picked;
		// 		bool reverted;
		// 		PickUp(defBowB_p, out picked);
		// 			ASSG(sgpAll, SlotGroup.SelectedState);
		// 				ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(defBowB_p, Slottable.PickedAndSelectedState);
		// 				ASSB(crfBowA_p, Slottable.DefocusedState);
		// 				ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(defWearB_p, Slottable.DefocusedState);
		// 				ASSB(crfWearA_p, Slottable.DefocusedState);
		// 				ASSB(defParts_p, Slottable.DefocusedState);
		// 				ASSB(crfParts_p, Slottable.DefocusedState);

		// 				ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(crfShieldA_p, Slottable.DefocusedState);
		// 				ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(crfMWeaponA_p, Slottable.DefocusedState);

		// 			ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
		// 			AB(sgm.RootPage.EquipBundle.GetFocusedBundleElement().ContainsElement(sgWear), true);
		// 			AB(sgWear.AcceptsFilter(defBowB_p), false);
		// 			ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
		// 			ASSB(defParts_p2, Slottable.DefocusedState);
		// 			ASSB(crfParts_p2, Slottable.DefocusedState);

		// 			ASSG(sgCGears, SlotGroup.DefocusedState);
		// 				ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
		// 				ASSB(defMWeaponA_e, Slottable.EquippedAndDefocusedState);

		// 		Revert(defBowB_p, out reverted);

		// 		AssertFocused();
		// 		sgpAll.ToggleAutoSort(false);
		// 		PickUp(defBowB_p, out picked);
		// 			ASSG(sgpAll, SlotGroup.SelectedState);
		// 				ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(defBowB_p, Slottable.PickedAndSelectedState);
		// 				ASSB(crfBowA_p, Slottable.FocusedState);
		// 				ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(defWearB_p, Slottable.FocusedState);
		// 				ASSB(crfWearA_p, Slottable.FocusedState);
		// 				ASSB(defParts_p, Slottable.FocusedState);
		// 				ASSB(crfParts_p, Slottable.FocusedState);

		// 				ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(crfShieldA_p, Slottable.FocusedState);
		// 				ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(crfMWeaponA_p, Slottable.FocusedState);

		// 			ASSG(sgBow, SlotGroup.FocusedState);
		// 				ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSG(sgWear, SlotGroup.DefocusedState);
		// 				ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
		// 			ASSG(sgCGears, SlotGroup.DefocusedState);
		// 				ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
		// 				ASSB(defMWeaponA_e, Slottable.EquippedAndDefocusedState);
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 				ASSB(defParts_p2, Slottable.DefocusedState);
		// 				ASSB(crfParts_p2, Slottable.DefocusedState);
		// 		Revert(defBowB_p, out reverted);
		// 		AssertFocused();
		// 		//

		// 			ASSG(sgpAll, SlotGroup.FocusedState);
		// 				ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(defBowB_p, Slottable.FocusedState);
		// 				ASSB(crfBowA_p, Slottable.FocusedState);
		// 				ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(defWearB_p, Slottable.FocusedState);
		// 				ASSB(crfWearA_p, Slottable.FocusedState);
		// 				ASSB(defParts_p, Slottable.DefocusedState);
		// 				ASSB(crfParts_p, Slottable.DefocusedState);
		// 				ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(crfShieldA_p, Slottable.FocusedState);
		// 				ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 			ASSG(sgBow, SlotGroup.FocusedState);
		// 				ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSG(sgWear, SlotGroup.FocusedState);
		// 				ASSB(defWearA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSG(sgCGears, SlotGroup.FocusedState);
		// 				ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
		// 				ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 				ASSB(defParts_p2, Slottable.DefocusedState);
		// 				ASSB(crfParts_p2, Slottable.DefocusedState);
				
				
		// 		PickUp(defBowA_e, out picked);
				
		// 			ASSG(sgpAll, SlotGroup.FocusedState);
		// 				ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(defBowB_p, Slottable.FocusedState);
		// 				ASSB(crfBowA_p, Slottable.FocusedState);
		// 				ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(defWearB_p, Slottable.DefocusedState);
		// 				ASSB(crfWearA_p, Slottable.DefocusedState);
		// 				ASSB(defParts_p, Slottable.DefocusedState);
		// 				ASSB(crfParts_p, Slottable.DefocusedState);
		// 				ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(crfShieldA_p, Slottable.DefocusedState);
		// 				ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(crfMWeaponA_p, Slottable.DefocusedState);
		// 			ASSG(sgBow, SlotGroup.SelectedState);
		// 				ASSB(defBowA_e, Slottable.PickedAndSelectedState);
		// 			ASSG(sgWear, SlotGroup.DefocusedState);
		// 				ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
		// 			ASSG(sgCGears, SlotGroup.DefocusedState);
		// 				ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
		// 				ASSB(defMWeaponA_e, Slottable.EquippedAndDefocusedState);
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 				ASSB(defParts_p2, Slottable.DefocusedState);
		// 				ASSB(crfParts_p2, Slottable.DefocusedState);
					
		// 		Revert(defBowA_e, out reverted);
		// 		AssertFocused();

		// 		PickUp(defShieldA_p, out picked);
		// 			ASSG(sgpAll, SlotGroup.FocusedState);
		// 			ASSG(sgBow, SlotGroup.FocusedState);
		// 			ASSG(sgWear, SlotGroup.FocusedState);
		// 			ASSG(sgCGears, SlotGroup.FocusedState);
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 		Revert(defShieldA_p, out reverted);
		// 		AB(picked, false);
		// 		AB(reverted, false);

		// 		PickUp(crfShieldA_p, out picked);
		// 			ASSG(sgpAll, SlotGroup.SelectedState);
		// 				ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(defBowB_p, Slottable.FocusedState);
		// 				ASSB(crfBowA_p, Slottable.FocusedState);
		// 				ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(defWearB_p, Slottable.FocusedState);
		// 				ASSB(crfWearA_p, Slottable.FocusedState);
		// 				ASSB(defParts_p, Slottable.FocusedState);
		// 				ASSB(crfParts_p, Slottable.FocusedState);
		// 				ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(crfShieldA_p, Slottable.PickedAndSelectedState);
		// 				ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
		// 				ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 			ASSG(sgBow, SlotGroup.DefocusedState);
		// 				ASSB(defBowA_e, Slottable.EquippedAndDefocusedState);
		// 			ASSG(sgWear, SlotGroup.DefocusedState);
		// 				ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
		// 			ASSG(sgCGears, SlotGroup.FocusedState);
		// 				ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
		// 				ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 				ASSB(defParts_p2, Slottable.DefocusedState);
		// 				ASSB(crfParts_p2, Slottable.DefocusedState);
		// 		Revert(crfShieldA_p, out reverted);
		// 		AB(picked, true);
		// 		AB(reverted, true);
		// 		AssertFocused();

		// 		sgpAll.ToggleAutoSort(true);
		// 		sgCGears.ToggleAutoSort(false);
		// 		PickUp(defShieldA_e, out picked);
		// 		AB(picked, true);
		// 			ASSG(sgpAll, SlotGroup.FocusedState);
		// 				ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(defBowB_p, Slottable.DefocusedState);
		// 				ASSB(crfBowA_p, Slottable.DefocusedState);
		// 				ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(defWearB_p, Slottable.DefocusedState);
		// 				ASSB(crfWearA_p, Slottable.DefocusedState);
		// 				ASSB(defParts_p, Slottable.DefocusedState);
		// 				ASSB(crfParts_p, Slottable.DefocusedState);
		// 					AB(object.ReferenceEquals(defShieldA_e.Item, defShieldA_p.Item), true);
		// 					AB(sgm.GetSlotGroup(defShieldA_e).IsShrinkable, true);
		// 					AB(SlotSystem.Utility.HaveCommonItemFamily(defShieldA_e, defShieldA_p), true);
		// 				ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);//
		// 				ASSB(crfShieldA_p, Slottable.FocusedState);
		// 				ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
		// 				ASSB(crfMWeaponA_p, Slottable.FocusedState);
		// 			ASSG(sgBow, SlotGroup.DefocusedState);
		// 				ASSB(defBowA_e, Slottable.EquippedAndDefocusedState);
		// 			ASSG(sgWear, SlotGroup.DefocusedState);
		// 				ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
		// 			ASSG(sgCGears, SlotGroup.SelectedState);
		// 				ASSB(defShieldA_e, Slottable.PickedAndSelectedState);
		// 				ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
		// 			ASSG(sgpParts, SlotGroup.DefocusedState);
		// 				ASSB(defParts_p2, Slottable.DefocusedState);
		// 				ASSB(crfParts_p2, Slottable.DefocusedState);
		// 		AB(reverted, true);
		// 		Revert(defShieldA_e, out reverted);
		// 		AssertFocused();
		// 	}
		// 	/*	spot tests hover	*/
		// 		public void TestHoverDefShieldA(){
		// 			bool picked;
		// 			bool reverted;
		// 			AssertFocused();
		// 			sgm.SetFocusedPoolSG(sgpParts);
		// 			AssertFocused();
		// 			sgm.SetFocusedPoolSG(sgpAll);
		// 			AssertFocused();

		// 			PickUp(defShieldA_p, out picked);
		// 			Revert(defShieldA_p, out reverted);
		// 			AssertFocused();
		// 			AB(picked, false);
		// 			AB(reverted, false);


		// 			/*	picking crfShieldA_p
		// 			*/
		// 				Slottable pickedSB = crfShieldA_p;
		// 				Slottable target;
		// 				/*	sgpAll
		// 				*/
		// 					target = defBowA_p;
		// 						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
		// 					target = pickedSB;
		// 						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AssertSimHover<RevertTransaction>(true, pickedSB, sgpAll, false);
							
		// 					sgpAll.ToggleAutoSort(false);
		// 					target = defBowA_p;
		// 						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AssertSimHover<ReorderTransaction>(true, target, sgpAll, false);
		// 					target = defShieldA_p;
		// 						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AssertSimHover<ReorderTransaction>(true, target, sgpAll, false);
		// 					target = pickedSB;
		// 						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AssertSimHover<RevertTransaction>(true, pickedSB, sgpAll, false);
		// 				/*	equip sgs
		// 				*/
		// 					target = defBowA_e;
		// 						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AssertSimHover<RevertTransaction>(true, null, null, false);
		// 					target = defShieldA_e;
		// 						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AssertSimHover<SwapTransaction>(true, target, sgCGears, false);
		// 					target = defMWeaponA_e;
		// 						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AssertSimHover<SwapTransaction>(true, target, sgCGears, false);


					
					
		// 			/*	picking defMWeaponA_p
		// 			*/
		// 				pickedSB = defMWeaponA_p;
		// 				target = defBowB_p;
		// 					TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 					AssertSimHover<RevertTransaction>(false, null, null, true);
		// 			/*	picking defMWeaponA_e
		// 			*/
		// 				pickedSB = defMWeaponA_e;
		// 				target = defBowB_p;
		// 					TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
		// 				target = defMWeaponA_p;
		// 					TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 					AssertSimHover<UnequipTransaction>(true, defMWeaponA_p, sgpAll, false);
		// 				target = crfShieldA_p;
		// 					TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 					AssertSimHover<SwapTransaction>(true, target, sgpAll, false);
		// 				target = crfMWeaponA_p;
		// 					TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 					AssertSimHover<SwapTransaction>(true, target, sgpAll, false);
		// 				target = defShieldA_p;
		// 					TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
		// 				target = defBowA_e;
		// 					TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 					AssertSimHover<RevertTransaction>(true, null, null, false);
		// 				target = defWearA_e;
		// 					TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 					AssertSimHover<RevertTransaction>(true, null, null, false);
						
		// 				target = defShieldA_e;
		// 					TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 					AssertSimHover<RevertTransaction>(true, null, sgCGears, false);
						
		// 		}
		// 		public void TestHoverDefBowASBE(){
		// 			SlotGroup origSG = sgBow;
		// 			/*	defBowASB_e
		// 			*/
		// 				/*	AutoSort false
		// 				*/
		// 					Slottable targetSB = defBowA_p;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, sgpAll);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = defBowB_p;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, defBowB_p);
		// 						AE(selectedSG, sgpAll);
		// 						AE(transaction.GetType(), typeof(SwapTransaction));

		// 					targetSB = crfBowA_p;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, crfBowA_p);
		// 						AE(selectedSG, sgpAll);
		// 						AE(transaction.GetType(), typeof(SwapTransaction));

		// 					targetSB = defWearA_p;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, sgpAll);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = defWearB_p;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, sgpAll);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = crfWearA_p;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, sgpAll);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));

		// 					targetSB = defParts_p2;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, null);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = crfParts_p2;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, null);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = defBowA_e;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, defBowA_e);
		// 						AE(selectedSG, sgBow);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = defWearA_e;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, null);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = defShieldA_p;
		// 						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
		// 					targetSB = crfShieldA_p;
		// 						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
		// 					targetSB = defShieldA_e;
		// 						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AssertSimHover<RevertTransaction>(true, null, null, false);
							
							
		// 					sgm.SetFocusedPoolSG(sgpParts);
							
		// 						targetSB = defParts_p2;
		// 						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 							AB(picked, true);
		// 							AE(selectedSB, null);
		// 							AE(selectedSG, sgpParts);
		// 							AE(transaction.GetType(), typeof(RevertTransaction));
								
		// 						targetSB = crfParts_p2;
		// 						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 							AB(picked, true);
		// 							AE(selectedSB, null);
		// 							AE(selectedSG, sgpParts);
		// 							AE(transaction.GetType(), typeof(RevertTransaction));
		// 				/*	AutoSort true
		// 				*/
		// 					sgm.SetFocusedPoolSG(sgpAll);
		// 					origSG.ToggleAutoSort(true);
		// 					targetSB = defBowA_p;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, sgpAll);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = defBowB_p;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, defBowB_p);
		// 						AE(selectedSG, sgpAll);
		// 						AE(transaction.GetType(), typeof(SwapTransaction));

		// 					targetSB = crfBowA_p;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, crfBowA_p);
		// 						AE(selectedSG, sgpAll);
		// 						AE(transaction.GetType(), typeof(SwapTransaction));

		// 					targetSB = defWearA_p;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, sgpAll);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = defWearB_p;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, sgpAll);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = crfWearA_p;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, sgpAll);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));

		// 					targetSB = defParts_p2;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, null);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = crfParts_p2;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, null);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = defBowA_e;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, defBowA_e);
		// 						AE(selectedSG, sgBow);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					targetSB = defWearA_e;
		// 					TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 						AB(picked, true);
		// 						AE(selectedSB, null);
		// 						AE(selectedSG, null);
		// 						AE(transaction.GetType(), typeof(RevertTransaction));
							
		// 					sgm.SetFocusedPoolSG(sgpParts);
							
		// 						targetSB = defParts_p2;
		// 						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 							AB(picked, true);
		// 							AE(selectedSB, null);
		// 							AE(selectedSG, sgpParts);
		// 							AE(transaction.GetType(), typeof(RevertTransaction));
								
		// 						targetSB = crfParts_p2;
		// 						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 							AB(picked, true);
		// 							AE(selectedSB, null);
		// 							AE(selectedSG, sgpParts);
		// 							AE(transaction.GetType(), typeof(RevertTransaction));
		// 		}
		// 		public void TestHoverDefBowASBP(){
		// 			Slottable targetSB;
		// 			targetSB = defBowA_p;
		// 					TestSimHover(defBowA_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 					AssertSimHover<RevertTransaction>(false, null, null, true);
		// 				targetSB = defBowB_p;
		// 					TestSimHover(defBowA_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 					AssertSimHover<RevertTransaction>(false, null, null, true);
		// 				targetSB = defBowA_e;
		// 					TestSimHover(defBowA_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 					AssertSimHover<RevertTransaction>(false, null, null, true);
		// 		}
		// 		public void TestHoverDefBowBSBP(){
		// 			sgpAll.ToggleAutoSort(true);
		// 			sgm.SetFocusedPoolSG(sgpAll);
		// 			AssertFocused();

		// 			Slottable targetSB;
		// 			targetSB = defBowA_p;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
		// 			targetSB = defBowB_p;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, defBowB_p, sgpAll, false);
		// 			targetSB = crfBowA_p;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
		// 			targetSB = defBowA_e;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<SwapTransaction>(true, defBowA_e, sgBow, false);
		// 			targetSB = defWearA_e;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, null, false);
		// 			targetSB = defShieldA_e;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, null, false);
		// 			targetSB = defShieldA_p;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
					

		// 			sgpAll.ToggleAutoSort(false);

		// 			targetSB = defBowA_p;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<ReorderTransaction>(true, defBowA_p, sgpAll, false);
		// 			targetSB = crfBowA_p;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<ReorderTransaction>(true, crfBowA_p, sgpAll, false);
		// 			targetSB = defBowB_p;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, defBowB_p, sgpAll, false);
		// 			targetSB = defWearA_p;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<ReorderTransaction>(true, defWearA_p, sgpAll, false);
		// 			targetSB = defWearB_p;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<ReorderTransaction>(true, defWearB_p, sgpAll, false);
		// 			targetSB = crfWearA_p;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<ReorderTransaction>(true, crfWearA_p, sgpAll, false);
		// 			targetSB = defParts_p;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<ReorderTransaction>(true, defParts_p, sgpAll, false);
		// 			targetSB = crfParts_p;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<ReorderTransaction>(true, crfParts_p, sgpAll, false);
		// 			targetSB = defBowA_e;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<SwapTransaction>(true, defBowA_e, sgBow, false);
		// 			targetSB = defWearA_e;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, null, false);
		// 			targetSB = defParts_p2;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, null, false);
		// 			targetSB = crfParts_p2;
		// 				TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, null, false);

		// 		}
		// 		public void TestHoverDefWearASBE(){
		// 			sgm.SetFocusedPoolSG(sgpAll);
		// 			AssertFocused();
		// 			Slottable target;

		// 			target = defBowA_p;
		// 				TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
		// 			target = defBowB_p;
		// 				TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
		// 			target = crfBowA_p;
		// 				TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
		// 			target = defWearA_p;
		// 				TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
		// 			target = defWearB_p;
		// 				TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<SwapTransaction>(true, defWearB_p, sgpAll, false);
		// 			target = crfWearA_p;
		// 				TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<SwapTransaction>(true, crfWearA_p, sgpAll, false);
		// 			target = defParts_p;
		// 				TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
		// 			target = crfParts_p;
		// 				TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
					
		// 			target = defBowA_e;
		// 				TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, null, false);
		// 			target = defWearA_e;
		// 				TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, defWearA_e, sgWear, false);
		// 			target = defParts_p2;
		// 				TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, null, false);
					
		// 			sgm.SetFocusedPoolSG(sgpParts);
		// 			target = defParts_p2;
		// 				TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, sgpParts, false);
		// 			sgm.SetFocusedPoolSG(sgpAll);


		// 		}
		// 		public void TestHoverPartsInSGPParts(){
		// 			sgm.SetFocusedPoolSG(sgpParts);
		// 			AssertFocused();
		// 			Slottable target;

		// 			target = defParts_p2;
		// 				TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, target, sgpParts, false);
		// 			target = crfParts_p2;
		// 				TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, sgpParts, false);
					
		// 			sgpParts.ToggleAutoSort(false);
		// 			target = defParts_p2;
		// 				TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, target, sgpParts, false);
		// 			target = crfParts_p2;
		// 				TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<ReorderTransaction>(true, target, sgpParts, false);
		// 			sgpParts.ToggleAutoSort(true);
					
		// 			target = defParts_p;
		// 				TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, null, false);
		// 			target = defBowA_e;
		// 				TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, null, false);
		// 			target = defWearA_e;
		// 				TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
		// 				AssertSimHover<RevertTransaction>(true, null, null, false);
		// 		}
		// 	public void AssertSimHover<T>(bool picked, Slottable sb, SlotGroup sg, bool TSNull){
		// 		if(picked) AB(picked, true); else AB(picked, false);
		// 		if(sb != null) AE(selectedSB, sb); else AB(selectedSB == null, true);
		// 		if(sg != null) AE(selectedSG, sg); else AB(selectedSG == null, true);
		// 		if(!TSNull) AE(transaction.GetType(), typeof(T)); else AB(transaction == null, true);
		// 	}
		// 	public void TestSimHoverOnAllSB(){
		// 		foreach(Slottable sb in SlottableList()){
		// 			foreach(Slottable hoveredSB in SlottableList()){
		// 				SlotGroup destSG = sgm.GetSlotGroup(hoveredSB);
		// 				bool picked;
		// 				Slottable selectedSB;
		// 				SlotGroup selectedSG;
		// 				SlotSystemTransaction transaction;
		// 				TestSimHover(sb, hoveredSB, destSG, out picked, out selectedSB, out selectedSG, out transaction);
		// 			}
		// 		}
		// 	}
		// 	public void TestSimHover(Slottable pickedSB, Slottable hoveredSB, SlotGroup hoveredSG, out bool picked, out Slottable selectedSB, out SlotGroup selectedSG, out SlotSystemTransaction transaction){
		// 		SlotGroup origSG = sgm.GetSlotGroup(pickedSB);// == SelectedSG
				
		// 		if(pickedSB.CurState == Slottable.FocusedState || pickedSB.CurState == Slottable.EquippedAndDeselectedState){

		// 			PickUp(pickedSB, out picked);//picked = true;
		// 			ASSB(pickedSB, Slottable.PickedAndSelectedState);
		// 			//AssertPostPickFilter(pickedSB);
					
		// 			if(hoveredSB != null){
		// 				SlotGroup destSG = sgm.GetSlotGroup(hoveredSB);
						
		// 				if(hoveredSB == pickedSB){
							
		// 					selectedSB = pickedSB;
		// 					selectedSG = origSG;
		// 					sgm.SimSBHover(hoveredSB, eventData);

		// 					AE(hoveredSB.PrevState, Slottable.PickedAndSelectedState);
		// 					ASSB(hoveredSB, Slottable.PickedAndSelectedState);
		// 					AB((hoveredSB.CurProcess.GetType() == typeof(SBPickUpProcess) || hoveredSB.CurProcess.GetType() == typeof(SBHighlightProcess)), true);
		// 					// AE(hoveredSB.CurProcess.GetType(), typeof(PickedUpAndSelectedProcess));
		// 					AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
		// 					transaction = sgm.Transaction;

		// 				}else{//	hover is not null nor same as pickedSb
		// 					if(origSG == destSG){
		// 						selectedSG = origSG;
		// 						// ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
		// 						if(hoveredSB.CurState == Slottable.FocusedState){
		// 							selectedSB = hoveredSB;
		// 							sgm.SimSBHover(hoveredSB, eventData);
		// 							sgm.SimSGHover(hoveredSG, eventData);
		// 							ASSB(hoveredSB, Slottable.SelectedState);
		// 							AE(hoveredSB.CurProcess.GetType(), typeof(SBHighlightProcess));
		// 							ASSB(pickedSB, Slottable.PickedAndDeselectedState);
		// 							AE(pickedSB.CurProcess.GetType(), typeof(SBDehighlightProcess));

		// 							AE(sgm.Transaction.GetType(), typeof(ReorderTransaction));
		// 							transaction = sgm.Transaction;

		// 							sgm.SimSBHover(pickedSB, eventData);

		// 						}else if(hoveredSB.CurState == Slottable.EquippedAndDeselectedState){
		// 							selectedSB = hoveredSB;
		// 							sgm.SimSBHover(hoveredSB, eventData);
		// 							sgm.SimSGHover(hoveredSG, eventData);
		// 							ASSB(hoveredSB, Slottable.EquippedAndSelectedState);
		// 							AE(hoveredSB.CurProcess.GetType(), typeof(SBHighlightProcess));
		// 							ASSB(pickedSB, Slottable.PickedAndDeselectedState);
		// 							AE(pickedSB.CurProcess.GetType(), typeof(SBDehighlightProcess));

		// 							AE(sgm.Transaction.GetType(), typeof(ReorderTransaction));
		// 							transaction = sgm.Transaction;

		// 							sgm.SimSBHover(pickedSB, eventData);

		// 						}else{
		// 							selectedSB = null;
		// 							SlottableState preState = hoveredSB.CurState;
		// 							sgm.SimSBHover(hoveredSB, eventData);
		// 							sgm.SimSGHover(hoveredSG, eventData);
		// 							ASSB(hoveredSB, preState);
		// 							AE(pickedSB.PrevState, Slottable.PickedAndSelectedState);
		// 							ASSB(pickedSB, Slottable.PickedAndDeselectedState);
		// 							AE(pickedSB.CurProcess.GetType(), typeof(SBDehighlightProcess));

		// 							AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
		// 							transaction = sgm.Transaction;

		// 							sgm.SimSBHover(pickedSB, eventData);
		// 						}		
		// 					}else{
		// 						/*	destSG != null, != origSG
		// 							selectedSG == null || == destSG || != origSG
		// 							hoveredSB != null, != pickdSB
		// 							fill, swap, stack
		// 							not revert
		// 						*/
		// 						AE(sgm.SelectedSB, pickedSB);
		// 						AE(sgm.SelectedSG, origSG);
		// 						// ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
		// 						if(hoveredSB.CurState == Slottable.FocusedState || hoveredSB.CurState == Slottable.EquippedAndDeselectedState || hoveredSB.CurState == Slottable.PickedAndDeselectedState){
		// 							sgm.SimSBHover(hoveredSB, eventData);
		// 								AE(sgm.SelectedSB, hoveredSB);
		// 								selectedSB = hoveredSB;
		// 								if(hoveredSB.CurState != Slottable.PickedAndSelectedState){
		// 									if(hoveredSB.IsEquipped)
		// 										ASSB(hoveredSB, Slottable.EquippedAndSelectedState);
		// 									else
		// 										ASSB(hoveredSB, Slottable.SelectedState);
		// 								}else
		// 									ASSB(hoveredSB, Slottable.PickedAndSelectedState);
									
		// 							if(hoveredSB.Item.IsStackable)
		// 								AE(sgm.Transaction.GetType(), typeof(StackTransaction));
		// 							else{
		// 								if(object.ReferenceEquals(hoveredSB.Item, pickedSB.Item) && sgm.GetSlotGroup(hoveredSB).IsShrinkable)
		// 									AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
		// 								else
		// 									AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
		// 							}
		// 							transaction = sgm.Transaction;
		// 						}else{// hoveredSB not in a state to be hovered
		// 							SlottableState preState = hoveredSB.CurState;
		// 							selectedSB = null;
		// 							sgm.SimSBHover(hoveredSB, eventData);
		// 								AE(sgm.SelectedSB, null);
		// 								ASSB(hoveredSB, preState);
									
		// 							AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
		// 							transaction = sgm.Transaction;
		// 						}


		// 						ASSB(pickedSB, Slottable.PickedAndDeselectedState);
		// 						/*	selected, eqSelected, pickSele, 
		// 						*/
		// 						if(hoveredSG.CurState == SlotGroup.FocusedState){
		// 							selectedSG = hoveredSG;
		// 							sgm.SimSGHover(hoveredSG, eventData);
		// 								AE(sgm.SelectedSG, hoveredSG);
		// 								ASSG(hoveredSG, SlotGroup.SelectedState);
		// 						}else{
		// 							selectedSG = null;
		// 							SlotGroupState preState = hoveredSG.CurState;
		// 							sgm.SimSGHover(hoveredSG, eventData);
		// 								AE(sgm.SelectedSG, null);
		// 								ASSG(hoveredSG, preState);
		// 						}

		// 						sgm.SimSBHover(pickedSB, eventData);
		// 						sgm.SimSGHover(origSG, eventData);
		// 					}
		// 				}
		// 			}
		// 			else{//	hoveredSB == null
		// 				selectedSB = null;
		// 				selectedSG = hoveredSG;
		// 				sgm.SimSBHover(hoveredSB, eventData);
		// 				sgm.SimSGHover(hoveredSG, eventData);
		// 				transaction = sgm.Transaction;
		// 				// transaction = new RevertTransaction(pickedSB);
		// 				// if(hoveredSG == null){
							
		// 				// 	/*	revert
		// 				// 	*/
		// 				// }else if(hoveredSG == origSG){
							
		// 				// }else{
							
		// 				// 	if(hoveredSG == origSG){// same sg, no selectable sb under cursor
								

		// 				// 		/*	revert
		// 				// 		*/
		// 				// 	}else{//	hoveredSG not null nor the same as orig

								
		// 				// 	}
		// 				// }
		// 				/*	for reverting
		// 				*/
		// 				sgm.SimSBHover(pickedSB, eventData);
		// 				sgm.SimSGHover(origSG, eventData);
		// 			}
		// 			bool reverted;
		// 			Revert(pickedSB, out reverted);
		// 			AB(reverted, true);
		// 			AssertFocused();
					
		// 		}else{//	pickedSB is not in a state to be picked up
		// 			picked = false;
		// 			selectedSB = null;
		// 			selectedSG = null;
		// 			transaction = null;

		// 			if(pickedSB.CurState == Slottable.DefocusedState || pickedSB.CurState == Slottable.EquippedAndDefocusedState){
		// 				pickedSB.OnPointerDownMock(eventData);
		// 				ASSB(pickedSB, Slottable.WaitForPointerUpState);
		// 				pickedSB.OnPointerUpMock(eventData);
		// 				if(pickedSB.IsEquipped)
		// 					ASSB(pickedSB, Slottable.EquippedAndDefocusedState);
		// 				else
		// 					ASSB(pickedSB, Slottable.DefocusedState);
		// 			}else{
		// 				/*	not testable
		// 				*/
		// 				SlottableState preState = pickedSB.CurState;
		// 				pickedSB.OnPointerDownMock(eventData);
		// 				ASSB(pickedSB, preState);
		// 			}
		// 			AssertFocused();
		// 		}
		// 	}
		// 	/*	hovering	*/
		// 		public void TestPickupAndRevertAll(){
		// 			int hoveredCount = 0;
		// 			TestHoverAll(ref hoveredCount);
		// 			AE(hoveredCount, 6);

		// 			hoveredCount = 0;
		// 			sgm.SetFocusedPoolSG(sgpParts);
		// 			TestHoverAll(ref hoveredCount);
		// 			AE(hoveredCount, 4);

		// 			AB(HoverTestPassed(defBowA_p), false);
		// 			AB(HoverTestPassed(defBowB_p), false);
		// 			AB(HoverTestPassed(crfBowA_p), false);
		// 			AB(HoverTestPassed(defWearA_p), false);
		// 			AB(HoverTestPassed(defWearB_p), false);
		// 			AB(HoverTestPassed(crfWearA_p), false);
		// 			AB(HoverTestPassed(defParts_p), false);
		// 			AB(HoverTestPassed(crfParts_p), false);
		// 			AB(HoverTestPassed(defBowA_e), true);
		// 			AB(HoverTestPassed(defWearA_e), true);
		// 			AB(HoverTestPassed(defParts_p2), true);
		// 			AB(HoverTestPassed(crfParts_p2), true);
					
		// 			hoveredCount = 0;
		// 			sgm.SetFocusedPoolSG(sgpAll);
		// 			TestHoverAll(ref hoveredCount);
		// 			AE(hoveredCount, 6);
					
		// 			AB(HoverTestPassed(defBowA_p), false);
		// 			AB(HoverTestPassed(defBowB_p), true);
		// 			AB(HoverTestPassed(crfBowA_p), true);
		// 			AB(HoverTestPassed(defWearA_p), false);
		// 			AB(HoverTestPassed(defWearB_p), true);
		// 			AB(HoverTestPassed(crfWearA_p), true);
		// 			AB(HoverTestPassed(defParts_p), false);
		// 			AB(HoverTestPassed(crfParts_p), false);
		// 			AB(HoverTestPassed(defBowA_e), true);
		// 			AB(HoverTestPassed(defWearA_e), true);
		// 			AB(HoverTestPassed(defParts_p2), false);
		// 			AB(HoverTestPassed(crfParts_p2), false);
		// 		}
		// 		public bool HoverTestPassed(Slottable sb){
		// 			bool result = false;
		// 			TestHoverSequence(sb, out result);
		// 			return result;
		// 		}
		// 		public void TestHoverAll(ref int count){
		// 			foreach(Slottable sb in SlottableList()){
		// 				bool hovered = false;
		// 				TestHoverSequence(sb, out hovered);
		// 				if(hovered)
		// 					count ++;
		// 			}

		// 		}
		// 		public void TestHoverSequence(Slottable sb, out bool hovered){
		// 			bool pickedUp = false;
		// 			bool reverted = false;
		// 			PickUp(sb, out pickedUp);
		// 			Revert(sb, out reverted);
		// 			hovered = pickedUp && reverted;
		// 		}	
	/**/
}
