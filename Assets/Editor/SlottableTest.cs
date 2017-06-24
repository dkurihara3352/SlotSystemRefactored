using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections.Generic;
using Utility;
public class SlottableTest{
	public enum TestElement{SB, SG, SGM, TA}
	public class SlotSystemTestResult{
		public bool isPAS; public bool isTAS;
		public Slottable testingSB;
		public SlotGroup testTargetSG;
		public Slottable testTargetSB;
		public string Val;
		public SlotSystemTestResult(bool isPAS, Slottable testSB, bool isPickable){
			this.testingSB = testSB;
			this.isPAS = isPAS;
			this.Val = " isPiackable: " + (isPickable?"True":"False");
		}
		public SlotSystemTestResult(SlotSystemManager ssm, bool isPAS, bool isTAS, Slottable testSB, SlotGroup testTargetSG, Slottable testTargetSB, int i){
			this.isPAS = isPAS; this.isTAS = isTAS;
			this.testingSB = testSB;
			this.testTargetSG = testTargetSG; this.testTargetSB = testTargetSB;
			this.Val = i.ToString();
		}
		/*	thorough ver	*/
			public SlotSystemTestResult(SlotSystemManager ssm, bool isPAS, bool isTAS, 
				Slottable testSB, SlotGroup testTargetSG, Slottable testTargetSB, TestElement ele){
					this.isPAS = isPAS; this.isTAS = isTAS;
					this.testingSB = testSB;
					this.testTargetSG = testTargetSG; this.testTargetSB = testTargetSB;
					string selection = "testSB: " + Util.SBofSG(testSB) + 
					" tarSG: " + (testTargetSG == null? "null": testTargetSG.eName + " ") +
					" tarSB: " + (testTargetSB == null? "null": Util.SBofSG(testTargetSB)) + " ";
					if(ele == TestElement.SGM)
						this.Val = selection + Util.SSMDebug(ssm);
					else if(ele == TestElement.SG)
						this.Val = selection + Util.SGDebug(testTargetSG);
					else if(ele == TestElement.SB)
						this.Val = selection + Util.SBDebug(testSB);
					else if(ele == TestElement.TA)
						this.Val = selection + Util.TADebug(testSB, testTargetSG, testTargetSB);
				}
		public bool SameSGSB(SlotSystemTestResult other){
			if(testingSB == null){
				return this.testTargetSG == other.testTargetSG;
			}else{
				if(testTargetSG == null)
					return this.testingSB == other.testingSB;
				else{
					if(testTargetSB == null)
						return this.testTargetSG == other.testTargetSG && this.testingSB == other.testingSB;
					else
						return this.testTargetSG == other.testTargetSG && this.testingSB == other.testingSB && this.testTargetSB == other.testTargetSB;
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
						Debug.Log(bundle[0].Val);
				}else{
					foreach(SlotSystemTestResult res in bundle){
						if(!(suppressedVal != null && bundle[0].Val == suppressedVal))
							Debug.Log(
								("isPAS: " + (res.isPAS?Util.Blue("On"):Util.Red("Off"))) + 
								(" isTAS: " + (res.isTAS?Util.Blue("On"):Util.Red("Off"))) + "	" +
								res.Val);
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
		public void Capture(SlotSystemManager ssm, Slottable testSB, SlotGroup testTargetSG, Slottable testTargetSB, bool isPAS, bool isTAS, TestElement ele){
			SlotSystemTestResult res = new SlotSystemTestResult(ssm, isPAS, isTAS, testSB, testTargetSG, testTargetSB, ele);
			testResults.Add(res);
		}
		public void Print(string msg, SlotSystemManager ssm, Slottable testSB, SlotGroup testTargetSG, Slottable testTargetSB, bool isPAS, bool isTAS, TestElement ele){
			string ASSTr = "isPAS: " + (isPAS?Util.Blue("On"):Util.Red("Off")) + ", " +
				"isTAS: " + (isTAS?Util.Blue("On"):Util.Red("Off")) + " ";
			string testSBstr = "testSB: " + Util.SBofSG(testSB) + " ";
			string tarSGstr = "";
				if(testTargetSG != null)
					tarSGstr = "tarSG: " + testTargetSG.eName + " ";
			string tarSBstr = "";
				if(testTargetSB != null)
					tarSBstr = "tarSB: " + Util.SBofSG(testTargetSB) + " ";
			string selection = testSBstr + tarSGstr + tarSBstr;
			string disp = "";
			if(ele == TestElement.SB)
				disp = Util.SBDebug(testSB);
			else if(ele == TestElement.SG)
				disp = Util.SGDebug(testTargetSG);
			else if(ele == TestElement.SGM)
				disp = Util.SSMDebug(ssm);
			else if(ele == TestElement.TA)
				disp = Util.TADebug(testSB, testTargetSG, testTargetSB);
			Debug.Log(msg + " " + ASSTr + selection +disp);
		}
	/*	fields 	*/
		/*	other	*/
			bool picked;
			PointerEventDataMock eventData = new PointerEventDataMock();
		/*	ssm */
			GameObject ssmGO;
			SlotSystemManager ssm;
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
				SlotGroup sgeBow;
				GameObject sgWearGO;
				SlotGroup sgeWear;
				GameObject sgCGearsGO;
				SlotGroup sgeCGears;
			/*	generic	*/
				GameObject sggGO_12;
				SlotGroup sgg_12;
				GameObject sggGO_111;
				SlotGroup sgg_111;
				GameObject sggGO_112;
				SlotGroup sgg_112;
				GameObject sggGO_21;
				SlotGroup sgg_21;
				GameObject sggGO_22;
				SlotGroup sgg_22;
				GameObject sggGO_23;
				SlotGroup sgg_23;
				GameObject sggGO_24;
				SlotGroup sgg_24;
				GameObject sggGO_2511;
				SlotGroup sgg_2511;
				GameObject sggGO_2512;
				SlotGroup sgg_2512;
				GameObject sggGO_2521;
				SlotGroup sgg_2521;
				GameObject sggGO_2522;
				SlotGroup sgg_2522;
		/*	items	*/
			/*	sbp	*/
			Slottable defBowA_p;
			Slottable defBowB_p;
			Slottable crfBowA_p;
			Slottable defWearA_p;
			Slottable defWearB_p;
			Slottable crfWearA_p;
			Slottable defShieldA_p;
			Slottable crfShieldA_p;
			Slottable defMWeaponA_p;
			Slottable crfMWeaponA_p;
			Slottable defQuiverA_p;
			Slottable defPackA_p;
			Slottable defParts_p;
			Slottable crfParts_p;
			
			Slottable defBowA_p2;
			Slottable defBowB_p2;
			Slottable crfBowA_p2;
			Slottable defWearA_p2;
			Slottable defWearB_p2;
			Slottable crfWearA_p2;
			Slottable defShieldA_p2;
			Slottable crfShieldA_p2;
			Slottable defMWeaponA_p2;
			Slottable crfMWeaponA_p2;
			Slottable defQuiverA_p2;
			Slottable defPackA_p2;
			Slottable defParts_p2;
			Slottable crfParts_p2;
			/*	sbe	*/
			Slottable defBowA_e;
			Slottable defWearA_e;
			Slottable defShieldA_e;
			Slottable defMWeaponA_e;
		/*	inventories	*/
			PoolInventory poolInv;
			EquipmentSetInventory equipInv;
			GenericInventory genInv;
		
	[SetUp]
	public void Setup(){
		/*	Items	*/
			/*	bows	*/
				BowMock defBow = new BowMock();
				defBow.ItemID = 0;
				BowMock crfBow = new BowMock();
				crfBow.ItemID = 1;
				BowInstanceMock defBowA = new BowInstanceMock();//	equipped
				defBowA.Item = defBow;
				BowInstanceMock defBowB = new BowInstanceMock();
				defBowB.Item = defBow;
				BowInstanceMock crfBowA = new BowInstanceMock();
				crfBowA.Item = crfBow;
			/*	wears	*/
				WearMock defWear = new WearMock();
				defWear.ItemID = 100;
				WearMock crfWear = new WearMock();
				crfWear.ItemID = 101;
				WearInstanceMock defWearA = new WearInstanceMock();//	equipped
				defWearA.Item = defWear;
				WearInstanceMock defWearB = new WearInstanceMock();
				defWearB.Item = defWear;
				WearInstanceMock crfWearA = new WearInstanceMock();
				crfWearA.Item = crfWear;
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
				PartsInstanceMock defPartsB = new PartsInstanceMock();
				defPartsB.Item = defParts;
				defPartsB.Quantity = 5;
				PartsInstanceMock crfPartsA = new PartsInstanceMock();
				crfPartsA.Item = crfParts;
				crfPartsA.Quantity = 3;
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
		/*	Inventory	*/
			/*	pool inv	*/
				List<SlottableItem> items = new List<SlottableItem>();
					items.Add(defBowA);
					items.Add(defBowB);
					items.Add(crfBowA);
					items.Add(defWearA);
					items.Add(defWearB);
					items.Add(crfWearA);
					items.Add(defShieldA);
					items.Add(defMWeaponA);
					items.Add(defPartsA);
					items.Add(defPartsB);
					items.Add(crfPartsA);
					items.Add(crfShieldA);
					items.Add(crfMWeaponA);
					items.Add(defQuiverA);
					items.Add(defPackA);
				poolInv = new PoolInventory();
					foreach(SlottableItem item in items){
						poolInv.Add(item);
					}
						AE(poolInv.count, 14);
						int id = 0;
						foreach(SlottableItem item in poolInv){
							object.ReferenceEquals(item, items[id]);
							id ++;
							AB(poolInv.Contains(item), true);
						}
			/*	equip inv	*/
				List<CarriedGearInstanceMock> initCGears = new List<CarriedGearInstanceMock>();
				initCGears.Add(defShieldA);
				initCGears.Add(defMWeaponA);
				equipInv = new EquipmentSetInventory(defBowA, defWearA, initCGears, 4);
					List<SlottableItem> cgItems = new List<SlottableItem>();
						cgItems.Add(defBowA);
						cgItems.Add(defWearA);
						cgItems.Add(defShieldA);
						cgItems.Add(defMWeaponA);
					AE(equipInv.count, 4);
					id = 0;
					foreach(SlottableItem item in equipInv){
						object.ReferenceEquals(item, cgItems[id]);
						id++;
						AB(equipInv.Contains(item), true);
					}
			/*	generic inventory	*/
				genInv = new GenericInventory();
					AE(genInv.count, 0);
		/*	SGs	*/
			/*	sgpAll	*/
				sgpAllGO = new GameObject("PoolSlotGroup");
				sgpAll = sgpAllGO.AddComponent<SlotGroup>();
				sgpAll.Initialize("sgpAll",SlotGroup.NullFilter, poolInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
					ASSG(sgpAll,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpAll.isShrinkable, true);
					AB(sgpAll.isExpandable, true);
					AssertSGCounts(sgpAll,
						14,//slots
						14,//items
						14);//sbs
			/*	sgpBow	*/
				sgpBowGO = new GameObject("sgpBowGO");
				sgpBow = sgpBowGO.AddComponent<SlotGroup>();
				sgpBow.Initialize("sgpBow" ,SlotGroup.BowFilter, poolInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
					ASSG(sgpBow,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpBow.isShrinkable, true);
					AB(sgpBow.isExpandable, true);
					AssertSGCounts(sgpBow,
						3,//slots
						3,//items
						3);//sbs
			/*	sgpWear	*/
				sgpWearGO = new GameObject("sgpWearGO");
				sgpWear = sgpWearGO.AddComponent<SlotGroup>();
				sgpWear.Initialize("sgpWear", SlotGroup.WearFilter, poolInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
					ASSG(sgpWear,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpWear.isShrinkable, true);
					AB(sgpWear.isExpandable, true);
					AssertSGCounts(sgpWear,
						3,//slots
						3,//items
						3);//sbs
			/*	sgpCGears	*/
				sgpCGearsGO = new GameObject("sgpCGearsGO");
				sgpCGears = sgpCGearsGO.AddComponent<SlotGroup>();
				sgpCGears.Initialize("sgpCGears", SlotGroup.CGearsFilter, poolInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
					ASSG(sgpCGears,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpCGears.isShrinkable, true);
					AB(sgpCGears.isExpandable, true);
					AssertSGCounts(sgpCGears,
						6,//slots
						6,//items
						6);//sbs
			/*	sgpParts	*/
				sgpPartsGO = new GameObject("PartsSlotGroupPool");
				sgpParts = sgpPartsGO.AddComponent<SlotGroup>();
				sgpParts.Initialize("sgpParts", SlotGroup.PartsFilter, poolInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
					ASSG(sgpParts,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpParts.isShrinkable, true);
					AB(sgpParts.isExpandable, true);
					AssertSGCounts(sgpParts,
						2,//slots
						2,//items
						2);//sbs
			/*	sgBow	*/
				sgBowGO = new GameObject("BowSlotGroup");
				sgeBow = sgBowGO.AddComponent<SlotGroup>();
				sgeBow.Initialize("sgeBow", SlotGroup.BowFilter, equipInv, false, 1, SlotGroup.emptyCommand, SlotGroup.updateEquipAtExecutionCommand);
					ASSG(sgeBow,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgeBow.isShrinkable, false);
					AB(sgeBow.isExpandable, false);
					AssertSGCounts(sgeBow,
						1,//slots
						1,//items
						1);//sbs
			/*	sgWear	*/
				sgWearGO = new GameObject("WearSlotGroup");
				sgeWear = sgWearGO.AddComponent<SlotGroup>();
				sgeWear.Initialize("sgeWear", SlotGroup.WearFilter, equipInv, false, 1, SlotGroup.emptyCommand, SlotGroup.updateEquipAtExecutionCommand);
					ASSG(sgeWear,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgeWear.isShrinkable, false);
					AB(sgeWear.isExpandable, false);
					AssertSGCounts(sgeWear,
						1,//slots
						1,//items
						1);//sbs
			/*	sgCGears	*/
				sgCGearsGO = new GameObject("CarriedGearsSG");
				sgeCGears = sgCGearsGO.AddComponent<SlotGroup>();
				equipInv.SetEquippableCGearsCount(4);
				int slotCount = equipInv.equippableCGearsCount;
				sgeCGears.Initialize("sgeCGears", SlotGroup.CGearsFilter, equipInv, true, slotCount, SlotGroup.emptyCommand, SlotGroup.updateEquipAtExecutionCommand);
					ASSG(sgeCGears,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AB(sgeCGears.isShrinkable, true);
						AB(sgeCGears.isExpandable, false);
						AssertSGCounts(sgeCGears,
							4,//slots
							2,//items
							2);//sbs
			/*	sgGen	*/
				/*	sgg_12	*/
					sggGO_12 = new GameObject("sggGO_12");
					sgg_12 = sggGO_12.AddComponent<SlotGroup>();
					sgg_12.Initialize("sgg_12", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_12,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_12.isShrinkable, true);
						AE(sgg_12.isExpandable, true);
						AssertSGCounts(sgg_12, 0, 0, 0);
				/*	sgg_111	*/
					sggGO_111 = new GameObject("sggGO_111");
					sgg_111 = sggGO_111.AddComponent<SlotGroup>();
					sgg_111.Initialize("sgg_111", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_111,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_111.isShrinkable, true);
						AE(sgg_111.isExpandable, true);
						AssertSGCounts(sgg_111, 0, 0, 0);
				/*	sgg_112	*/
					sggGO_112 = new GameObject("sggGO_112");
					sgg_112 = sggGO_112.AddComponent<SlotGroup>();
					sgg_112.Initialize("sgg_112", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_112,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_112.isShrinkable, true);
						AE(sgg_112.isExpandable, true);
						AssertSGCounts(sgg_112, 0, 0, 0);
				/*	sgg_21	*/
					sggGO_21 = new GameObject("sggGO_21");
					sgg_21 = sggGO_21.AddComponent<SlotGroup>();
					sgg_21.Initialize("sgg_21", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_21,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_21.isShrinkable, true);
						AE(sgg_21.isExpandable, true);
						AssertSGCounts(sgg_21, 0, 0, 0);
				/*	sgg_22	*/
					sggGO_22 = new GameObject("sggGO_22");
					sgg_22 = sggGO_22.AddComponent<SlotGroup>();
					sgg_22.Initialize("sgg_22", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_22,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_22.isShrinkable, true);
						AE(sgg_22.isExpandable, true);
						AssertSGCounts(sgg_22, 0, 0, 0);
				/*	sgg_23	*/
					sggGO_23 = new GameObject("sggGO_23");
					sgg_23 = sggGO_23.AddComponent<SlotGroup>();
					sgg_23.Initialize("sgg_23", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_23,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_23.isShrinkable, true);
						AE(sgg_23.isExpandable, true);
						AssertSGCounts(sgg_23, 0, 0, 0);
				/*	sgg_24	*/
					sggGO_24 = new GameObject("sgg_24GO");
					sgg_24 = sggGO_24.AddComponent<SlotGroup>();
					sgg_24.Initialize("sgg_24", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_24,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_24.isShrinkable, true);
						AE(sgg_24.isExpandable, true);
						AssertSGCounts(sgg_24, 0, 0, 0);
				/*	sgg_2511	*/
					sggGO_2511 = new GameObject("sgg_2511GO");
					sgg_2511 = sggGO_2511.AddComponent<SlotGroup>();
					sgg_2511.Initialize("sgg_2511", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_2511,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_2511.isShrinkable, true);
						AE(sgg_2511.isExpandable, true);
						AssertSGCounts(sgg_2511, 0, 0, 0);
				/*	sgg_2512	*/
					sggGO_2512 = new GameObject("sgg_2512GO");
					sgg_2512 = sggGO_2512.AddComponent<SlotGroup>();
					sgg_2512.Initialize("sgg_2512", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_2512,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_2512.isShrinkable, true);
						AE(sgg_2512.isExpandable, true);
						AssertSGCounts(sgg_2512, 0, 0, 0);
				/*	sgg_2521	*/
					sggGO_2521 = new GameObject("sgg_2521GO");
					sgg_2521 = sggGO_2521.AddComponent<SlotGroup>();
					sgg_2521.Initialize("sgg_2521", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_2521,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_2521.isShrinkable, true);
						AE(sgg_2521.isExpandable, true);
						AssertSGCounts(sgg_2521, 0, 0, 0);
				/*	sgg_2522	*/
					sggGO_2522 = new GameObject("sgg_2522GO");
					sgg_2522 = sggGO_2522.AddComponent<SlotGroup>();
					sgg_2522.Initialize("sgg_2522", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_2522,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_2522.isShrinkable, true);
						AE(sgg_2522.isExpandable, true);
						AssertSGCounts(sgg_2522, 0, 0, 0);
		/*	SBs	*/
			defBowA_p = sgpAll.GetSB(defBowA);
			defBowB_p = sgpAll.GetSB(defBowB);
			crfBowA_p = sgpAll.GetSB(crfBowA);
			defWearA_p = sgpAll.GetSB(defWearA);
			defWearB_p = sgpAll.GetSB(defWearB);
			crfWearA_p = sgpAll.GetSB(crfWearA);
			defShieldA_p = sgpAll.GetSB(defShieldA);
			crfShieldA_p = sgpAll.GetSB(crfShieldA);
			defMWeaponA_p =	sgpAll.GetSB(defMWeaponA); 
			crfMWeaponA_p =	sgpAll.GetSB(crfMWeaponA);
			defQuiverA_p = sgpAll.GetSB(defQuiverA);
			defPackA_p = sgpAll.GetSB(defPackA);
			defParts_p = sgpAll.GetSB(defPartsA);
			crfParts_p = sgpAll.GetSB(crfPartsA);

			defBowA_p2 = sgpBow.GetSB(defBowA);
			defBowB_p2 = sgpBow.GetSB(defBowB);
			crfBowA_p2 = sgpBow.GetSB(crfBowA);
			defWearA_p2 = sgpWear.GetSB(defWearA);
			defWearB_p2 = sgpWear.GetSB(defWearB);
			crfWearA_p2 = sgpWear.GetSB(crfWearA);
			defShieldA_p2 = sgpCGears.GetSB(defShieldA);
			crfShieldA_p2 = sgpCGears.GetSB(crfShieldA);
			defMWeaponA_p2 = sgpCGears.GetSB(defMWeaponA); 
			crfMWeaponA_p2 = sgpCGears.GetSB(crfMWeaponA);
			defQuiverA_p2 = sgpCGears.GetSB(defQuiverA);
			defPackA_p2 = sgpCGears.GetSB(defPackA);
			defParts_p2 = sgpParts.GetSB(defPartsA);
			crfParts_p2 = sgpParts.GetSB(crfPartsA);

			defBowA_e = sgeBow.GetSB(defBowA);
			defWearA_e = sgeWear.GetSB(defWearA);
			defShieldA_e = sgeCGears.GetSB(defShieldA);
			defMWeaponA_e = sgeCGears.GetSB(defMWeaponA);
		/*	SSM	*/
			ssmGO = new GameObject("ssmGO");
			ssm = ssmGO.AddComponent<SlotSystemManager>();
			/*	equip bundle	*/
				GameObject eSetGO = new GameObject("eSetGO");
				EquipmentSet equipSetA = eSetGO.AddComponent<EquipmentSet>();
				equipSetA.Initialize(sgeBow, sgeWear, sgeCGears);
				IEnumerable<SlotSystemElement> eBunEles = new SlotSystemElement[]{equipSetA};
				GameObject eBunGO = new GameObject("eBunGO");
				SlotSystemBundle equipBundle = eBunGO.AddComponent<SlotSystemBundle>();
				equipBundle.Initialize("eBundle", eBunEles);
				equipBundle.SetFocusedBundleElement(equipSetA);
			/*	pool bundle	*/
				IEnumerable<SlotSystemElement> pBunEles = new SlotSystemElement[]{sgpAll, sgpBow, sgpWear, sgpCGears, sgpParts};
				GameObject pBunGO = new GameObject("pBunGO");
				SlotSystemBundle poolBundle = pBunGO.AddComponent<SlotSystemBundle>();
				poolBundle.Initialize("pBundle", pBunEles);
				poolBundle.SetFocusedBundleElement(sgpAll);
			/*	generic pages	*/
				/*	gPag_11	*/
					IEnumerable<SlotSystemElement> gPagEles_11 = new SlotSystemElement[]{sgg_111, sgg_112};
					GameObject gPageGO_11 = new GameObject("gPageGO_11");
					GenericPage gPage_11 = gPageGO_11.AddComponent<GenericPage>();
					gPage_11.Initialize("gPage_11", gPagEles_11);
				/*	gPag_251	*/
					IEnumerable<SlotSystemElement> gPagEles_251 = new SlotSystemElement[]{sgg_2511, sgg_2512};
					GameObject gPageGO_251 = new GameObject("gPageGO_251");
					GenericPage gPage_251 = gPageGO_251.AddComponent<GenericPage>();
					gPage_251.Initialize("gPage_251", gPagEles_251);
				/*	gPag_252	*/
					IEnumerable<SlotSystemElement> gPagEles_252 = new SlotSystemElement[]{sgg_2521, sgg_2522};
					GameObject gPageGO_252 = new GameObject("gPageGO_252");
					GenericPage gPage_252 = gPageGO_252.AddComponent<GenericPage>();
					gPage_252.Initialize("gPage_252", gPagEles_252);
				
			/*	generic bundle	*/
				/*	gBun_1	*/
					IEnumerable<SlotSystemElement> gBunEles_1 = new SlotSystemElement[]{gPage_11, sgg_12};
					GameObject gBunGO_1 = new GameObject("gBunGO_1");
					SlotSystemBundle gBundle_1 = gBunGO_1.AddComponent<SlotSystemBundle>();
					gBundle_1.Initialize("gBundle_1", gBunEles_1);
					gBundle_1.SetFocusedBundleElement(gPage_11);
				/*	gBun_25	*/
					IEnumerable<SlotSystemElement> gBunEles_25 = new SlotSystemElement[]{gPage_251, gPage_252};
					GameObject gBunGO_25 = new GameObject("gBunGO_25");
					SlotSystemBundle gBundle_25 = gBunGO_25.AddComponent<SlotSystemBundle>();
					gBundle_25.Initialize("gBundle_25", gBunEles_25);
					gBundle_25.SetFocusedBundleElement(gPage_251);
				/*	gBun_2	*/
					IEnumerable<SlotSystemElement> gBunEles_2 = new SlotSystemElement[]{sgg_21, sgg_22, sgg_23, sgg_24, gBundle_25};
					GameObject gBunGO_2 = new GameObject("gBunGO_2");
					SlotSystemBundle gBundle_2 = gBunGO_2.AddComponent<SlotSystemBundle>();
					gBundle_2.Initialize("gBundle_2", gBunEles_2);
					gBundle_2.SetFocusedBundleElement(sgg_21);
			/*	generic Bundles	*/
				IEnumerable<SlotSystemBundle> gBundles = new SlotSystemBundle[]{gBundle_1, gBundle_2};
			
		ssm.Initialize(poolBundle, equipBundle, gBundles);
			AssertSlotSystemSSMSetRight(ssm);
			AssertParenthood();
			AssertImmediateBundle();
			AssertContainInHierarchy();
			AssertBundlesPagesAndSGsMembership();
			AssertSBsMembership();
			AssertInitialize();
		ssm.Activate();
		AssertFocused();
		// PrintSystemHierarchyDetailed(ssm);
		// AssertInitiallyFocusedBundles();
	}
	public void PrintHierarchySimple(SlotSystemElement ele){
		ele.PerformInHierarchy(PrintElementsReallySimply);
	}
	public void PrintElementsReallySimply(SlotSystemElement ele){
		string res = "";
		if(ele is SlotSystemManager)
			res = "SSM";
		else if(ele is SlotSystemBundle)
			res = "Bundle";
		else if(ele is EquipmentSet)
			res = "eSet";
		else if(ele is GenericPage)
			res = "gPage";
		else if(ele is SlotGroup)
			res = "SG";
		else if(ele is Slottable)
			res = "SB";
		Debug.Log(res);
	}
	[Test]
	public void TestAll(){
		// PrintSystemHierarchySimple(sgm.rootPage);
		// PrintSystemHierarchyDetailed(sgm.rootPage);
		// done
			// CheckShrinkableAndExpandableOnAllSGs();
			// CheckPickcableOnAllSB();
			// CheckTransacitonWithSBSpecifiedOnAll();
			// CheckTransactionOnAllSG();
			// TestDraggedIconOnAll();
			// TestAcceptSGTACompOnAll();
			// TestReorderSBsMethod();
			// TestSGActionStateSequence();
			// TestSBStateTransitionOnAll();
			// TestSGMStateTransition();
			// TestVolSortOnSPGParts();//missing
			// TestFillShortcut();
			// TestSwapShortcut();
			// TestPermutation();
			// TestCombination();
			// TestSGCGearsCorrespondence();
			// AssertInitialize();
			// TestSGECorrespondence();
			// TestSlotSystemActivateDeactivate();
			// TestFindAndFocusElement();
			/*	TAs	*/
				TestVolSortOnAll();
				TestRevertOnAllSBs();
				TestReorderOnAll();
				TestFillOnAll();
				TestSwapOnAll();
		// TestAddAndRemoveAll();
		// TestSGGeneric();
	}
	public void TestFindAndFocusElement(){
		ssm.PerformInHierarchy(AssertFocusInBundle);
	}
		public void AssertFocusInBundle(SlotSystemElement target){
			ssm.FindAndFocusInBundle(target);
			SlotSystemBundle rootBundle = null;
			rootBundle = target.immediateBundle;
			if(rootBundle != null){
				while(true){
					if(rootBundle.immediateBundle == null)
						break;
					rootBundle = rootBundle.immediateBundle;
				}
					rootBundle.PerformInHierarchy(AssertDefocusedIfNotContainInHierarchy, target);
			}
		}
			public void AssertDefocusedIfNotContainInHierarchy(SlotSystemElement ele, object obj){
				if(!(ele is SlotGroup || ele is Slottable)){
					SlotSystemElement target = (SlotSystemElement)obj;
					foreach(SlotSystemElement e in ele){
						if(!(ele.ContainsInHierarchy(target) || ele == target))
							AssertDefocusedSelfAndBelow(ele);
					}
				}
			}
	public void AssertContainInHierarchy(){
		SlotSystemBundle pBun = ssm.poolBundle;
		ANotNull(pBun);
		List<SlotSystemElement> positives = new List<SlotSystemElement>();
		positives.Add(ssm);
		AssertCIHonAllAndNotInOther(positives, pBun);
		positives.Add(pBun);
		foreach(SlotSystemElement ele in pBun){
			SlotGroup sg = (SlotGroup)ele;
			AssertCIHonAllAndNotInOther(positives, sg);
			positives.Add(sg);
			foreach(Slottable sb in sg){
				AssertCIHonAllAndNotInOther(positives, sb);
			}
			positives.Remove(sg);
		}
		positives.Remove(pBun);
		SlotSystemBundle eBun = ssm.equipBundle;
		ANotNull(eBun);
		AssertCIHonAllAndNotInOther(positives, eBun);
		positives.Add(eBun);
		foreach(EquipmentSet eSet in eBun){
			AssertCIHonAllAndNotInOther(positives, eSet);
			positives.Add(eSet);
			foreach(SlotSystemElement ele in eSet){
				SlotGroup sg = (SlotGroup)ele;
				AssertCIHonAllAndNotInOther(positives, sg);
				positives.Add(sg);
				foreach(Slottable sb in sg){
					if(sb!= null)
					AssertCIHonAllAndNotInOther(positives, sb);
				}
				positives.Remove(sg);
			}
			positives.Remove(eSet);
		}
		positives.Remove(eBun);
		SlotSystemBundle gBun;
		foreach(SlotSystemBundle gB in ssm.otherBundles){
			gBun = gB;
			ANotNull(gBun);
			AssertCIHonAllAndNotInOther(positives, gBun);
			positives.Add(gBun);
			foreach(SlotSystemElement ele in gBun){
				if(ele is GenericPage){
					GenericPage gPage = (GenericPage)ele;
					AssertCIHonAllAndNotInOther(positives, gPage);
					positives.Add(gPage);
					foreach(SlotSystemElement ele2 in gPage){
						SlotGroup sg = (SlotGroup)ele2;
						AssertCIHonAllAndNotInOther(positives, sg);
						positives.Add(sg);
						foreach(Slottable sb in sg){
							if(sb!= null)
								AssertCIHonAllAndNotInOther(positives, sb);
						}
						positives.Remove(sg);
					}
					positives.Remove(gPage);
				}else if(ele is SlotGroup){
					SlotGroup sg = (SlotGroup)ele;
					AssertCIHonAllAndNotInOther(positives, sg);
					positives.Add(sg);
					foreach(Slottable sb in sg){
						if(sb!= null)
							AssertCIHonAllAndNotInOther(positives, sb);
					}
					positives.Remove(sg);
				}
			}
			positives.Remove(gBun);
		}
	}
		public void AssertCIHonAllAndNotInOther(IList<SlotSystemElement> positives, SlotSystemElement target){
			CheckCIH(ssm, positives, target);
			SlotSystemBundle pBun = ssm.poolBundle;
			CheckCIH(pBun, positives, target);
			foreach(SlotSystemElement ele in pBun){
				SlotGroup sg = (SlotGroup)ele;
				CheckCIH(sg, positives, target);
				foreach(Slottable sb in sg){
					CheckCIH(sb, positives, target);
				}
			}
			SlotSystemBundle eBun = ssm.equipBundle;
			CheckCIH(eBun, positives, target);
			foreach(EquipmentSet eSet in eBun){
				CheckCIH(eSet, positives, target);
				foreach(SlotSystemElement ele in eSet){
					SlotGroup sg = (SlotGroup)ele;
					CheckCIH(sg, positives, target);
					foreach(Slottable sb in sg){
						if(sb!= null)
							CheckCIH(sb, positives, target);
					}
				}
			}
			SlotSystemBundle gBun;
			foreach(SlotSystemBundle gB in ssm.otherBundles){
				gBun = gB;
				CheckCIH(gBun, positives, target);
				foreach(SlotSystemElement ele in gBun){
					if(ele is GenericPage){
						GenericPage gPage = (GenericPage)ele;
						CheckCIH(gPage, positives, target);
						foreach(SlotSystemElement ele2 in gPage){
							SlotGroup sg = (SlotGroup)ele2;
							CheckCIH(sg, positives, target);
							foreach(Slottable sb in sg){
								if(sb!= null)
									CheckCIH(sb, positives, target);
							}
						}
					}else if(ele is SlotGroup){
						SlotGroup sg = (SlotGroup)ele;
						CheckCIH(sg, positives, target);
						foreach(Slottable sb in sg){
							if(sb!= null)
								CheckCIH(sb, positives, target);
						}
					}
				}
			}
		}
		public void CheckCIH(SlotSystemElement subject, IList<SlotSystemElement> positives, SlotSystemElement target){
			if(positives.Contains(subject))
				AB(subject.ContainsInHierarchy(target), true);
			else
				AB(subject.ContainsInHierarchy(target), false);
		}
		public void PrintCIH(SlotSystemElement ele){
			Debug.Log(Util.Bold("Target: ") + ele.eName);
			ssm.PerformInHierarchy(CheckAndPrintCIH, ele);
		}
			public void CheckAndPrintCIH(SlotSystemElement ele, object obj){
				SlotSystemElement target = (SlotSystemElement)obj;
				string res = Indent(ele.level) + ele.eName;
				if(ele.ContainsInHierarchy(target))
					res += Util.Blue(": Contains");
				else
					res += Util.Red(": NOT Contains");
				Debug.Log(res);
			}
	public void AssertParenthood(){
		SlotSystemBundle pBun = ssm.poolBundle;
		SlotSystemBundle eBun = ssm.equipBundle;
		SlotSystemBundle gBun;
		ANotNull(pBun);
		ANotNull(eBun);
		ANull(ssm.parent);
		AE(pBun.parent, ssm);
		foreach(SlotSystemElement ele in pBun){
			SlotGroup sg = (SlotGroup)ele;
			AE(sg.parent, pBun);
			foreach(Slottable sb in sg){
				AE(sb.parent, sg);
			}
		}
		AE(eBun.parent, ssm);
		foreach(EquipmentSet eSet in eBun){
			AE(eSet.parent, eBun);
			foreach(SlotSystemElement ele in eSet){
				SlotGroup sg = (SlotGroup)ele;
				AE(sg.parent, eSet);
				foreach(Slottable sb in sg){
					if(sb!= null)
					AE(sb.parent, sg);
				}
			}
		}
		foreach(SlotSystemBundle gB in ssm.otherBundles){
			gBun = gB;
			AE(gBun.parent, ssm);
			foreach(SlotSystemElement ele in gBun){
				if(ele is GenericPage){
					GenericPage gPage = (GenericPage)ele;
					AE(gPage.parent, gBun);
					foreach(SlotSystemElement ele2 in gPage){
						SlotGroup sg = (SlotGroup)ele2;
						AE(sg.parent, gPage);
						foreach(Slottable sb in sg){
							if(sb!= null)
							AE(sb.parent, sg);
						}
					}
				}else if(ele is SlotGroup){
					SlotGroup sg = (SlotGroup)ele;
					AE(sg.parent, gBun);
					foreach(Slottable sb in sg){
						if(sb!= null)
						AE(sb.parent, sg);
					}
				}
			}
		}
	}
	public void AssertImmediateBundle(){
		SlotSystemBundle pBun = ssm.poolBundle;
		SlotSystemBundle eBun = ssm.equipBundle;
		SlotSystemBundle gBun;
		ANotNull(pBun);
		ANotNull(eBun);
		ANull(ssm.immediateBundle);
		ANull(pBun.immediateBundle);
		foreach(SlotSystemElement ele in pBun){
			SlotGroup sg = (SlotGroup)ele;
			AE(sg.immediateBundle, pBun);
			foreach(Slottable sb in sg){
				AE(sb.immediateBundle, pBun);
			}
		}
		ANull(eBun.immediateBundle);
		foreach(EquipmentSet eSet in eBun){
			AE(eSet.immediateBundle, eBun);
			foreach(SlotSystemElement ele in eSet){
				SlotGroup sg = (SlotGroup)ele;
				AE(sg.immediateBundle, eBun);
				foreach(Slottable sb in sg){
					if(sb!= null)
					AE(sb.immediateBundle, eBun);
				}
			}
		}
		foreach(SlotSystemBundle gB in ssm.otherBundles){
			gBun = gB;
			ANull(gBun.immediateBundle);
			foreach(SlotSystemElement ele in gBun){
				if(ele is GenericPage){
					GenericPage gPage = (GenericPage)ele;
					AE(gPage.immediateBundle, gBun);
					foreach(SlotSystemElement ele2 in gPage){
						SlotGroup sg = (SlotGroup)ele2;
						AE(sg.immediateBundle, gBun);
						foreach(Slottable sb in sg){
							if(sb!= null)
							AE(sb.immediateBundle, gBun);
						}
					}
				}else if(ele is SlotGroup){
					SlotGroup sg = (SlotGroup)ele;
					AE(sg.immediateBundle, gBun);
					foreach(Slottable sb in sg){
						if(sb!= null)
						AE(sb.immediateBundle, gBun);
					}
				}
			}
		}
	}
	public void PrintParent(SlotSystemElement ele){
		string parentName = ele.parent == null?"null":ele.parent.eName;
		Debug.Log(Indent(ele.level) + ele.eName + "'s parent is " + parentName);
		
	}
	public void AssertInitiallyFocusedBundles(){
		ssm.poolBundle.PerformInHierarchy(AssertFocusedSelfAndBelow);
		ssm.equipBundle.PerformInHierarchy(AssertFocusedSelfAndBelow);
		foreach(SlotSystemBundle bundle in ssm.otherBundles){
			if(bundle != null)
				bundle.PerformInHierarchy(AssertDefocusedSelfAndBelow);
		}
		PrintSystemHierarchyDetailed(ssm);
	}
	public void TestSlotSystemActivateDeactivate(){
		ssm.Deactivate();
			Debug.Log(Util.Bold(Util.Yamabuki("Root deactivated")));
			PrintSystemHierarchyDetailed(ssm);
		ssm.Activate();
			Debug.Log(Util.Bold(Util.Yamabuki("Root activated")));
			AssertFocused();
			PrintSystemHierarchyDetailed(ssm);
		ssm.poolBundle.Defocus();
			Debug.Log(Util.Bold(Util.Yamabuki("Pool defocused")));
			PrintSystemHierarchyDetailed(ssm);
			ssm.poolBundle.PerformInHierarchy(AssertDefocusedSelfAndBelow);
		ssm.equipBundle.Defocus();
			Debug.Log(Util.Bold(Util.Yamabuki("Equip defocused")));
			PrintSystemHierarchyDetailed(ssm);
			ssm.equipBundle.PerformInHierarchy(AssertDefocusedSelfAndBelow);
		ssm.Activate();
			Debug.Log(Util.Bold(Util.Yamabuki("root activated")));
			PrintSystemHierarchyDetailed(ssm);
			AssertFocused();
	}
	public void AssertFocusedSelfAndBelow(SlotSystemElement ele){
		if(ele is SlotGroup){
			SlotGroup sg = (SlotGroup)ele;
			ASGReset(sg);
			if(ssm.poolBundle.ContainsInHierarchy(sg)){//	if sgp
				if(sg == ssm.focusedSGP){
					ASSG(sg,
						null, SGFocused, null,
						null, SGWFA, null, false);
					foreach(Slottable sb in sg){
						if(sb != null){
							AE(sb.ssm, ssm);
							ASBReset(sb);
							if(!sg.isAutoSort){
									ASSB_s(sb, SBFocused, SBWFA);
							}else{
								if(sb.item is PartsInstanceMock && !(sg.Filter is SGPartsFilter))
									ASSB_s(sb, SBDefocused, SBWFA);
								else
									if(sb.isEquipped)
										ASSB_s(sb, SBDefocused, SBWFA);
									else{
										ASSB_s(sb, SBFocused, SBWFA);
									}
							}
						}
					}
				}else{/*  sg not focused	*/
					ASSG(sg,
						null, SGDefocused, null,
						null, SGWFA, null, false);
					foreach(Slottable sb in sg){
						if(sb != null){
							AE(sb.ssm, ssm);
							ASBReset(sb);
							ASSB_s(sb, SBDefocused, SBWFA);
						}
					}
				}
			}else if(ssm.equipBundle.ContainsInHierarchy(sg)){//	if sge
				if(ssm.focusedSGEs.Contains(sg)){
					ASSG(sg,
						null, SGFocused, null,
						null, SGWFA, null, false);
					foreach(Slottable sbe in sg){
						if(sbe != null){
							AE(sbe.ssm, ssm);
							ASBReset(sbe);
							ASSB_s(sbe, SBFocused, SBWFA);
						}
					}
				}else{
					ASSG(sg,
						null, SGFocused, null,
						null, SGWFA, null, false);
					foreach(Slottable sbe in sg){
						if(sbe != null){
							AE(sbe.ssm, ssm);
							ASBReset(sbe);
							ASSB_s(sbe, SBDefocused, SBWFA);
						}
					}
				}
			}
		}else if(ele is Slottable){
			Slottable sb = (Slottable)ele;
			SlotGroup sg = sb.sg;
			ASBReset(sb);
			if(ssm.poolBundle.ContainsInHierarchy(sg)){
				if(sg == ssm.focusedSGP){
					if(!sg.isAutoSort){
						ASSB_s(sb, SBFocused, SBWFA);
					}else{
						if(sb.item is PartsInstanceMock && !(sg.Filter is SGPartsFilter))
							ASSB_s(sb, SBDefocused, SBWFA);
						else
							if(sb.isEquipped)
								ASSB_s(sb, SBDefocused, SBWFA);
							else{
								ASSB_s(sb, SBFocused, SBWFA);
							}
					}
				}else{
					ASSB_s(sb, SBDefocused, SBWFA);
				}
			}else if(ssm.equipBundle.ContainsInHierarchy(sg)){
				if(ssm.focusedSGEs.Contains(sg)){
					ASSB_s(sb, SBFocused, SBWFA);
				}else{
					ASSB_s(sb, SBDefocused, SBWFA);
				}
			}
		}
	}
	public void AssertDefocusedSelfAndBelow(SlotSystemElement ele){
		if(ele is SlotGroup){
			ASGSelState((SlotGroup)ele, null, SGDefocused, null);
		}else if(ele is Slottable){
			ASBSelState((Slottable)ele, null, SBDefocused, null);
		}
	}
	public void AssertSlotSystemSSMSetRight(SlotSystemManager ssm){
		AE(ssm.ssm, ssm);
		AE(ssm.poolBundle.ssm, ssm);
			foreach(SlotSystemElement ele in ssm.poolBundle){
				SlotGroup sg = (SlotGroup)ele;
				AE(sg.ssm, ssm);
				foreach(Slottable sb in sg){
					if(sb != null)
						AE(sb.ssm, ssm);
				}
			}
		AE(ssm.equipBundle.ssm, ssm);
			foreach(SlotSystemElement ele in ssm.equipBundle){
				EquipmentSet eSet = (EquipmentSet)ele;
				AE(eSet.ssm, ssm);
				foreach(SlotSystemElement ele2 in eSet){
					SlotGroup sg = (SlotGroup)ele2;
					AE(sg.ssm, ssm);
					foreach(Slottable sb in sg){
						if(sb != null)
							AE(sb.ssm, ssm);
					}
				}
			}
		foreach(SlotSystemBundle gBundle in ssm.otherBundles){
			AE(gBundle.ssm, ssm);
			foreach(var ele in gBundle){
				if(ele is GenericPage){
					GenericPage gPage = (GenericPage)ele;
					AE(ele.ssm, ssm);
					foreach(var e in gPage){
						SlotGroup sg = (SlotGroup)e;
						AE(sg.ssm, ssm);
					}
				}else if(ele is SlotGroup){
						SlotGroup sg = (SlotGroup)ele;
						AE(sg.ssm, ssm);
				}
			}
		}		
	}
	public string Indent(int level){
		string res = "";
		for(int i = 0; i < level; i++){
			res += "	" ;
		}
		return res;
	}
	public void PrintElementSimple(SlotSystemElement ele){
		string eleName = ele.eName;
		Debug.Log(Indent(ele.level) + eleName);
	}
	public void PrintElementInDetail(SlotSystemElement ele){
		string eleName = ele.eName;
		if(ele is SlotGroup)
			eleName = Util.SGDebug((SlotGroup)ele);
		else if(ele is Slottable)
			eleName = Util.SBDebug((Slottable)ele);
		else
			eleName = Util.SSEDebug(ele);
		Debug.Log(Indent(ele.level) + eleName);
	}
	public void PrintSystemHierarchyDetailed(SlotSystemElement ele){
		ele.PerformInHierarchy(PrintElementInDetail);
	}
	public void PrintSystemHierarchySimple(SlotSystemElement ele){
		ele.PerformInHierarchy(PrintElementSimple);

	}
	public void Reorder(Slottable testSB, Slottable hovSB){
		if(testSB.isPickable){
			SlotSystemTransaction ta = ssm.GetTransaction(testSB, null, hovSB);
			if(ta.GetType() == typeof(ReorderTransaction)){
				SlotGroup origSG = testSB.sg;
					AssertFocused();
					AE(hovSB, ta.targetSB);
				PickUp(testSB, out picked);
					ASSSM(ssm,
						testSB, null, null, null, testSB, null, null, testSB, 
						SSMDeactivated, SSMFocused, null,
						SSMWFA, SSMProbing, typeof(SSMProbeProcess),
						typeof(RevertTransaction), false, true, true, true);
					ASSG(origSG,
						SGFocused, SGDefocused, typeof(SGGreyoutProcess),
						null, SGWFA, null, false);
					ASSB(testSB,
						SBSelected, SBDefocused, typeof(SBGreyoutProcess),
						SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
						null, null, null);
					ASSB(hovSB,
						null, SBFocused, null,
						null, SBWFA, null, true,
						null, null, null);
				SimHover(hovSB, null, eventData);
					ASSSM(ssm,
						testSB, hovSB, origSG, null, testSB, null, null, hovSB, 
						SSMDeactivated, SSMFocused, null,
						SSMWFA, SSMProbing, typeof(SSMProbeProcess),
						typeof(ReorderTransaction), false, true, false, true);
					ASSG(origSG,
						SGFocused, SGDefocused, typeof(SGGreyoutProcess),
						null, SGWFA, null, false);
					ASSB(testSB,
						SBSelected, SBDefocused, typeof(SBGreyoutProcess),
						SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
						null, null, null);
					ASSB(hovSB,
						SBFocused, SBSelected, typeof(SBHighlightProcess),
						null, SBWFA, null, true,
						null, null, null);
				LetGo();
					ASSSM(ssm,
						testSB, hovSB, origSG, null, testSB, null, null, hovSB, 
						SSMDeactivated, SSMFocused, null,
						SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
						typeof(ReorderTransaction), false, true, false, true);
					ASSG(origSG,
						SGFocused, SGDefocused, typeof(SGGreyoutProcess),
						SGWFA, SGReorder, typeof(SGTransactionProcess), true);
					ASSB(testSB,
						SBSelected, SBDefocused, typeof(SBGreyoutProcess),
						SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), true,
						null, null, null);
					ASSB(hovSB,
						SBFocused, SBSelected, typeof(SBHighlightProcess),
						SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), true,
						null, null, null);
				CompleteAllSBActProcesses(origSG);
					ASSSM(ssm,
						testSB, hovSB, origSG, null, testSB, null, null, hovSB, 
						SSMDeactivated, SSMFocused, null,
						SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
						typeof(ReorderTransaction), false, true, true, true);
				ssm.dIcon1.CompleteMovement();
					AssertFocused();
			}else
				throw new System.InvalidOperationException("SlottableTest.Reorder: given argumant comination does not yield Reorder Transaction");
		}else
			throw new System.InvalidOperationException("SlottableTest.Reorder: testSB not pickable");
	}
	public void Revert(Slottable testSB, SlotGroup hovSG, Slottable hovSB){
		if(testSB.isPickable){
			SlotSystemTransaction ta = ssm.GetTransaction(testSB, hovSG, hovSB);
			if(ta.GetType() == typeof(RevertTransaction)){
				SlotGroup origSG = testSB.sg;
				bool isOnSG = hovSG != null && hovSB ==null;
				bool isOnSB = hovSB != null && hovSG == null;
				if(isOnSG || isOnSB){
					SlotGroup targetSG = origSG;
					/* there's no sg1 in ta, Revert is kinda special in this respect */
						AssertFocused();
						ASSSM(testSB.ssm,
							null, null, null, null, null, null, null, null, 
							SSMDeactivated, SSMFocused, null,
							null, SSMWFA, null,
							null, true, true, true, true);
					PickUp(testSB, out picked);
						ASSSM(testSB.ssm,
							testSB, null, null, null, testSB, null, null, testSB,
							SSMDeactivated, SSMFocused, null,
							SSMWFA, SSMProbing, typeof(SSMProbeProcess),
							typeof(RevertTransaction), false, true, true, true);
						ASSB(testSB,
							SBSelected, SBDefocused, typeof(SBGreyoutProcess),
							SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
							null, null, null);
						ASSG(targetSG,
							null, SGDefocused, null,
							null, SGWFA, null, false);
						if(isOnSG && hovSG != origSG)
						ASSG(hovSG,
							null, SGDefocused, null,
							null, SGWFA, null, false);
						if(isOnSB && hovSB != testSB)
						ASSB(hovSB,
							null, SBDefocused, null,
							null, SBWFA, null, false,
							null, null, null);
					if(isOnSG)
						SimHover(null, hovSG, eventData);
					else
						SimHover(hovSB, null, eventData);

						ASSSM(testSB.ssm,
							testSB, null, null, null, testSB, null, isOnSG?hovSG:null, isOnSG?null:hovSB, 
							SSMDeactivated, SSMFocused, null,
							SSMWFA, SSMProbing, typeof(SSMProbeProcess),
							typeof(RevertTransaction), false, true, true, true);
						ASSG(targetSG,
							null, SGDefocused, null,
							null, SGWFA, null, false);
						ASSB(testSB,
							SBSelected, SBDefocused, typeof(SBGreyoutProcess),
							SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
							null, null, null);
						if(isOnSG && hovSG != origSG)
						ASSG(hovSG,
							null, SGDefocused, null,
							null, SGWFA, null, false);
						if(isOnSB && hovSB != testSB)
						ASSB(hovSB,
							null, SBDefocused, null,
							null, SBWFA, null, false,
							null, null, null);
					LetGo();
						ASSSM(testSB.ssm,
							testSB, null, null, null, testSB, null, isOnSG?hovSG:null, isOnSG?null:hovSB, 
							SSMDeactivated, SSMFocused, null,
							SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
							typeof(RevertTransaction), false, true, true, true);
						ASSG(targetSG,
							SGFocused, SGDefocused, typeof(SGGreyoutProcess),
							SGWFA, SGRevert, typeof(SGTransactionProcess), false);
						ASBSelState(testSB, SBSelected, SBDefocused, typeof(SBGreyoutProcess));
						if(isOnSB && testSB.isStackable && testSB == hovSB){
							ASBActState(testSB, SBWFNT, SBMoveWithin, typeof(SBMoveWithinProcess), testSB.actProcess.isRunning);
						}else
							ASBActState(testSB, SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), testSB.actProcess.isRunning);
						if(isOnSG && hovSG != origSG)
						ASSG(hovSG,
							null, SGDefocused, null,
							null, SGWFA, null, false);
						if(isOnSB && hovSB != testSB && hovSB.sg == origSG)
						ASSB(hovSB,
							null, SBDefocused, null,
							SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), hovSB.actProcess.isRunning,
							null, null, null);
					ssm.dIcon1.CompleteMovement();
						AssertFocused();
				}else
					throw new System.InvalidOperationException("SlottableTest.Revert: tarSG and tarSB not set correctly (either both are present or are missing)");
			}else
				throw new System.InvalidOperationException("SlottableTest.Revert: given combination of arguments does not result in Revert Transaction");
		}else
			throw new System.InvalidOperationException("SlottableTest.Revert: testSB not pickable");
	}
	/*	SGs testing	*/
		public void TestSGECorrespondence(){
			ssm.SetFocusedPoolSG(sgpAll);
				sgpAll.ToggleAutoSort(true);
					sgeBow.ToggleAutoSort(true);
					sgeWear.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpAll true, sge true"));
					TestEquippingFromTo(sgpAll, sgeBow);
					TestEquippingFromTo(sgpAll, sgeWear);
					sgeBow.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpAll true, sgBow false"));
					TestEquippingFromTo(sgpAll, sgeBow);
					sgeWear.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpAll true, sgWear false"));
					TestEquippingFromTo(sgpAll, sgeWear);
				sgpAll.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpAll false, sges false"));
					TestEquippingFromTo(sgpAll, sgeBow);
					TestEquippingFromTo(sgpAll, sgeWear);
					sgeBow.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpAll false, sgBow true"));
					TestEquippingFromTo(sgpAll, sgeBow);
					sgeWear.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpAll false, sgWear true"));
					TestEquippingFromTo(sgpAll, sgeWear);
			ssm.SetFocusedPoolSG(sgpBow);
				sgpBow.ToggleAutoSort(true);
				sgeBow.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpBow true, sgBow true"));
					TestEquippingFromTo(sgpBow, sgeBow);
				sgeBow.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpBow true, sgBow false"));
					TestEquippingFromTo(sgpBow, sgeBow);
				sgpBow.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpBow false, sgBow false"));
					TestEquippingFromTo(sgpBow, sgeBow);
				sgeBow.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpBow false, sgBow true"));
					TestEquippingFromTo(sgpBow, sgeBow);
			ssm.SetFocusedPoolSG(sgpWear);
				sgpWear.ToggleAutoSort(true);
				sgeWear.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpWear true, sgWear true"));
					TestEquippingFromTo(sgpWear, sgeWear);
				sgeWear.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpWear true, sgWear false"));
					TestEquippingFromTo(sgpWear, sgeWear);
				sgpWear.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpWear false, sgWear false"));
					TestEquippingFromTo(sgpWear, sgeWear);
				sgeWear.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpWear false, sgWear true"));
					TestEquippingFromTo(sgpWear, sgeWear);
			}public void TestEquippingFromTo(SlotGroup sgp, SlotGroup sge){
				foreach(Slottable sb in transactableSBs(sgp, sge, null, typeof(SwapTransaction))){
					// Print(sb);
					InventoryItemInstanceMock testItem = sb.itemInst;
					InventoryItemInstanceMock swapItem = ssm.GetTransaction(sb, sge, null).targetSB.itemInst;
					AssertFocused();
					Swap(sb, sgp, sge, null);
					// Fill(sb, sgp, sge, null);//make this swap
					Print((Slottable)sge[0]);
					PrintItemsArray(equipInv);
					AssertFocused();
					/*	reverse	*/
					Swap(sge.GetSB(testItem), sge, null, sgp.GetSB(swapItem));
					// Fill(sge.GetSB(testItem), sge, sgp, null);//swap
					AssertFocused();
				}
				foreach(Slottable sbe in sge){
					if(sbe != null){
						foreach(Slottable sbp in transactableSBs(sgp, null, sbe, typeof(SwapTransaction))){
							InventoryItemInstanceMock testItem = sbp.itemInst;
							InventoryItemInstanceMock swapItem = sbe.itemInst;
							Slottable testSbe = sge.GetSB(swapItem);
								AssertFocused();
							Swap(sbp, sgp, null, testSbe);
								Print((Slottable)sge[0]);
								PrintItemsArray(equipInv);
							/*	reverse	*/
							Swap(sge.GetSB(testItem), sge, null, sgp.GetSB(swapItem));
								AssertFocused();
						}
					}
				}
			}
		public void TestSGCGearsCorrespondence(){
				ssm.SetFocusedPoolSG(sgpAll);
				sgpAll.ToggleAutoSort(true);
				sgeCGears.ToggleAutoSort(true);
			TestEquippingSGCGearsFrom(sgpAll);
				sgeCGears.ToggleAutoSort(false);
			TestEquippingSGCGearsFrom(sgpAll);
				sgpAll.ToggleAutoSort(false);
			TestEquippingSGCGearsFrom(sgpAll);
				sgeCGears.ToggleAutoSort(true);
			TestEquippingSGCGearsFrom(sgpAll);
				
				ssm.SetFocusedPoolSG(sgpCGears);
				sgpCGears.ToggleAutoSort(true);
				sgeCGears.ToggleAutoSort(true);
			TestEquippingSGCGearsFrom(sgpCGears);
				sgeCGears.ToggleAutoSort(false);
			TestEquippingSGCGearsFrom(sgpCGears);
				sgpCGears.ToggleAutoSort(false);
			TestEquippingSGCGearsFrom(sgpCGears);
				sgeCGears.ToggleAutoSort(true);
			TestEquippingSGCGearsFrom(sgpCGears);
			}public void TestEquippingSGCGearsFrom(SlotGroup sgp){
				ClearSGCGearsTo(sgp);
				/*	increasing	*/
					for(int newSlotCount = 1; newSlotCount < 5; newSlotCount++){
						ssm.ChangeEquippableCGearsCount(newSlotCount, sgeCGears);
						AssertSGCounts(sgeCGears, newSlotCount, 0, 0);
						AssertFocused();
						foreach(IEnumerable<Slottable> sbsCombo in possibleSBsCombos(newSlotCount, transactableSBs(sgp, sgeCGears, null, typeof(FillTransaction)))){
							foreach(Slottable sb in sbsCombo){
								Fill(sb, sgp, sgeCGears, null);
							}
							AE(transactableSBs(sgp, sgeCGears, null, typeof(FillTransaction)).Count, 0);
							int count = newSlotCount -1;
							/*	reducing the slots count while there's still some sbs left, down to 1	*/
								while(count > 0){
									ssm.ChangeEquippableCGearsCount(count, sgeCGears);
									count --;
								}
								AE(sgeCGears.slots.Count, 1);
								ssm.ChangeEquippableCGearsCount(newSlotCount, sgeCGears);
								ClearSGCGearsTo(sgp);
							/*	refilling	*/
								foreach(Slottable sb in sbsCombo){
									Fill(sb, sgp, sgeCGears, null);
								}
							/*	empty the slots	*/
								foreach(Slottable sb in sgeCGears){
									if(sb != null)
										Fill(sb, sgeCGears, sgp, null);
								}
								AE(sgeCGears.actualSBsCount, 0);
						}
					}
				/*	decreasing	*/
					for(int newSlotCount = 4; newSlotCount > 0; newSlotCount--){
						ssm.ChangeEquippableCGearsCount(newSlotCount, sgeCGears);
						AssertSGCounts(sgeCGears, newSlotCount, 0, 0);
						AssertFocused();
						foreach(IEnumerable<Slottable> sbsCombo in possibleSBsCombos(newSlotCount, transactableSBs(sgp, sgeCGears, null, typeof(FillTransaction)))){
							foreach(Slottable sb in sbsCombo){
								Fill(sb, sgp, sgeCGears, null);
							}
							AE(transactableSBs(sgp, sgeCGears, null, typeof(FillTransaction)).Count, 0);
							foreach(Slottable sb in sgeCGears){
								if(sb != null)
									Fill(sb, sgeCGears, sgp, null);
							}
							AE(sgeCGears.actualSBsCount, 0);
						}
					}
			}
	/*	test support fields	*/
		List<Slottable> allSBsList{
			get{
				List<Slottable> sbs = new List<Slottable>();
				sbs.AddRange(sbpList);
				sbs.AddRange(sbp2List);
				sbs.AddRange(sbeList);
				return sbs;
			}
		}
		List<Slottable> sbpList{
			get{
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_p);
				sbs.Add(defBowB_p);
				sbs.Add(crfBowA_p);
				sbs.Add(defWearA_p);
				sbs.Add(defWearB_p);
				sbs.Add(crfWearA_p);
				sbs.Add(defShieldA_p);
				sbs.Add(crfShieldA_p);
				sbs.Add(defMWeaponA_p);
				sbs.Add(crfMWeaponA_p);
				sbs.Add(defParts_p);
				sbs.Add(crfParts_p);
				sbs.Add(defQuiverA_p);
				sbs.Add(defPackA_p);
				return sbs;
			}
		}
		List<Slottable> sbp2List{
			get{
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_p2);
				sbs.Add(defBowB_p2);
				sbs.Add(crfBowA_p2);
				sbs.Add(defWearA_p2);
				sbs.Add(defWearB_p2);
				sbs.Add(crfWearA_p2);
				sbs.Add(defShieldA_p2);
				sbs.Add(crfShieldA_p2);
				sbs.Add(defMWeaponA_p2);
				sbs.Add(crfMWeaponA_p2);
				sbs.Add(defQuiverA_p2);
				sbs.Add(defPackA_p2);
				sbs.Add(defParts_p2);
				sbs.Add(crfParts_p2);
				return sbs;
			}
		}
		List<Slottable> sbeList{
			get{
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_e);
				sbs.Add(defWearA_e);
				sbs.Add(defShieldA_e);
				sbs.Add(defMWeaponA_e);
				return sbs;
			}
		}
		public List<Slottable> testSBs{
			get{
				List<Slottable> res = new List<Slottable>();
					res.Add(defBowA_p);
					res.Add(crfBowA_p);
					res.Add(defBowB_p);
					res.Add(defWearA_p);
					res.Add(crfWearA_p);
					res.Add(defWearB_p);
					res.Add(defShieldA_p);
					res.Add(crfShieldA_p);
					res.Add(defMWeaponA_p);
					res.Add(crfMWeaponA_p);
					res.Add(defQuiverA_p);
					res.Add(defPackA_p);
					res.Add(defParts_p);
					res.Add(crfParts_p);
				return res;
			}
		}
		SlotGroup origSGCache;
	/*	working	*/
		public void TestSGGeneric(){
			// AE(sgm.sgBundles.Count, 3);
			// AE(sgm.genBundles.Count, 1);
			// AE(sgm.allSGPs.Count, 5);
			// AE(sgm.allSGEs.Count, 3);
			// AE(sgm.allSGGs.Count, 2);
			// AE(sgm.focusedBundles.Count, 2);
			// AE(sgm.focusedSGGs.Count, 2);
			// sgm.FocusBundle(genBundle);
			// AE(sgm.FocusedBundles.Count, 3);
		}
		public void TestAddAndRemoveAll(){
			PerformOnAllSGAfterFocusing(TestAddAndRemove);
			PrintTestResult(null);
			}
			public void TestAddAndRemove(SlotGroup testSG, bool isPAS){
				List<Slottable> sbs = testSG.toList;
					AssertFocused();
				// sgm.MoveSBs(testSG, null, sbs);
				// 	ASSGM(sgm,
				// 		null, null, testSG, null, null, null, null, null,
				// 		SGMDeactivated, SGMFocused, null,
				// 		SGMWFA, SGMTransaction, typeof(SGMTransactionProcess),
				// 		typeof())
			}

	/*	setup	*/
		public void AssertInitialize(){
			ASSSM(ssm,
				null, null, null, null, null, null, null, null,
				SSMDeactivated, SSMDeactivated, null,
				SSMWFA, SSMWFA, null,
				null, true, true, true, true);
			foreach(SlotGroup sg in ssm.allSGs){
				ASSG(sg,
					SGDeactivated, SGDeactivated, null,
					SGWFA, SGWFA, null, false);
					ASGReset(sg);
				foreach(Slottable sb in sg){
					if(sb != null){
						ASSB(sb,
							SBDeactivated, SBDeactivated, null,
							SBWFA, SBWFA, null, false,
							null, null, null);
						ANull(sb.CurEqpState);
						ANull(sb.PrevEqpState);
					}
				}
			}
		}
		public void AssertBundlesPagesAndSGsMembership(){
			ANotNull(ssm);
			AE(ssm.allSGs.Count, 19);
				AB(ssm.allSGs.Contains(sgpAll), true);
				AB(ssm.allSGs.Contains(sgpBow), true);
				AB(ssm.allSGs.Contains(sgpWear), true);
				AB(ssm.allSGs.Contains(sgpCGears), true);
				AB(ssm.allSGs.Contains(sgpParts), true);
				AB(ssm.allSGs.Contains(sgeBow), true);
				AB(ssm.allSGs.Contains(sgeWear), true);
				AB(ssm.allSGs.Contains(sgeCGears), true);
				AB(ssm.allSGs.Contains(sgg_12), true);
				AB(ssm.allSGs.Contains(sgg_111), true);
				AB(ssm.allSGs.Contains(sgg_112), true);
				AB(ssm.allSGs.Contains(sgg_21), true);
				AB(ssm.allSGs.Contains(sgg_22), true);
				AB(ssm.allSGs.Contains(sgg_23), true);
				AB(ssm.allSGs.Contains(sgg_24), true);
				AB(ssm.allSGs.Contains(sgg_2511), true);
				AB(ssm.allSGs.Contains(sgg_2512), true);
				AB(ssm.allSGs.Contains(sgg_2521), true);
				AB(ssm.allSGs.Contains(sgg_2522), true);
			AE(ssm.focusedSGs.Count, 7);
				AB(ssm.focusedSGs.Contains(sgpAll), true);
				AB(ssm.focusedSGs.Contains(sgeBow), true);
				AB(ssm.focusedSGs.Contains(sgeWear), true);
				AB(ssm.focusedSGs.Contains(sgeCGears), true);
				AB(ssm.focusedSGs.Contains(sgg_111), true);
				AB(ssm.focusedSGs.Contains(sgg_112), true);
				AB(ssm.focusedSGs.Contains(sgg_21), true);
			/*	pool	*/
			ANotNull(ssm.poolBundle);
			AE(ssm.allSGPs.Count, 5);
				AB(ssm.allSGPs.Contains(sgpAll), true);
				AB(ssm.allSGPs.Contains(sgpBow), true);
				AB(ssm.allSGPs.Contains(sgpWear), true);
				AB(ssm.allSGPs.Contains(sgpCGears), true);
				AB(ssm.allSGPs.Contains(sgpParts), true);
				AE(sgpAll.parent, ssm.poolBundle);
				AE(sgpBow.parent, ssm.poolBundle);
				AE(sgpWear.parent, ssm.poolBundle);
				AE(sgpCGears.parent, ssm.poolBundle);
				AE(sgpParts.parent, ssm.poolBundle);
			AB(sgpAll.isPool, true);
			AB(sgpBow.isPool, true);
			AB(sgpWear.isPool, true);
			AB(sgpCGears.isPool, true);
			AB(sgpParts.isPool, true);
			AB(sgpAll.isSGE, false);
			AB(sgpBow.isSGE, false);
			AB(sgpWear.isSGE, false);
			AB(sgpCGears.isSGE, false);
			AB(sgpParts.isSGE, false);
			AB(sgpAll.isSGG, false);
			AB(sgpBow.isSGG, false);
			AB(sgpWear.isSGG, false);
			AB(sgpCGears.isSGG, false);
			AB(sgpParts.isSGG, false);
			AE(ssm.focusedSGP, sgpAll);
			/*	equip	*/
			ANotNull(ssm.equipBundle);
			AE(ssm.allSGEs.Count, 3);
			AE(ssm.focusedSGEs.Count, 3);
				AB(ssm.focusedSGEs.Contains(sgeBow), true);
				AB(ssm.focusedSGEs.Contains(sgeWear), true);
				AB(ssm.focusedSGEs.Contains(sgeCGears), true);
				AE(ssm.focusedEqSet, ssm.equipBundle.focusedElement);
				AE(sgeBow.parent, ssm.focusedEqSet);
				AE(sgeWear.parent, ssm.focusedEqSet);
				AE(sgeCGears.parent, ssm.focusedEqSet);
			AB(sgeBow.isPool, false);
			AB(sgeWear.isPool, false);
			AB(sgeCGears.isPool, false);
			AB(sgeBow.isSGE, true);
			AB(sgeWear.isSGE, true);
			AB(sgeCGears.isSGE, true);
			AB(sgeBow.isSGG, false);
			AB(sgeWear.isSGG, false);
			AB(sgeCGears.isSGG, false);
			/*	generic	*/
				ANotNull(ssm.otherBundles);
				AE(ssm.allSGGs.Count, 11);
					AB(ssm.allSGGs.Contains(sgg_111), true);
					AB(ssm.allSGGs.Contains(sgg_112), true);
					AB(ssm.allSGGs.Contains(sgg_12), true);
					AB(ssm.allSGGs.Contains(sgg_21), true);
					AB(ssm.allSGGs.Contains(sgg_22), true);
					AB(ssm.allSGGs.Contains(sgg_23), true);
					AB(ssm.allSGGs.Contains(sgg_24), true);
					AB(ssm.allSGGs.Contains(sgg_2511), true);
					AB(ssm.allSGGs.Contains(sgg_2512), true);
					AB(ssm.allSGGs.Contains(sgg_2521), true);
					AB(ssm.allSGGs.Contains(sgg_2522), true);
				AE(ssm.focusedSGGs.Count, 3);
					AB(ssm.focusedSGGs.Contains(sgg_111), true);
					AB(ssm.focusedSGGs.Contains(sgg_112), true);
					AB(ssm.focusedSGGs.Contains(sgg_21), true);
				AE(sgg_111.parent.eName, Util.Bold("gPage_11"));
				AE(sgg_112.parent.eName, Util.Bold("gPage_11"));
				AE(sgg_12.parent.eName, Util.Bold("gBundle_1"));
				AE(sgg_21.parent.eName, Util.Bold("gBundle_2"));
				AE(sgg_22.parent.eName, Util.Bold("gBundle_2"));
				AE(sgg_23.parent.eName, Util.Bold("gBundle_2"));
				AE(sgg_24.parent.eName, Util.Bold("gBundle_2"));
				AE(sgg_2511.parent.eName, Util.Bold("gPage_251"));
				AE(sgg_2512.parent.eName, Util.Bold("gPage_251"));
				AE(sgg_2521.parent.eName, Util.Bold("gPage_252"));
				AE(sgg_2522.parent.eName, Util.Bold("gPage_252"));
				AB(sgg_111.isPool, false);
				AB(sgg_112.isPool, false);
				AB(sgg_12.isPool, false);
				AB(sgg_21.isPool, false);
				AB(sgg_22.isPool, false);
				AB(sgg_23.isPool, false);
				AB(sgg_24.isPool, false);
				AB(sgg_2511.isPool, false);
				AB(sgg_2512.isPool, false);
				AB(sgg_2521.isPool, false);
				AB(sgg_2522.isPool, false);
				AB(sgg_111.isSGE, false);
				AB(sgg_112.isSGE, false);
				AB(sgg_12.isSGE, false);
				AB(sgg_21.isSGE, false);
				AB(sgg_22.isSGE, false);
				AB(sgg_23.isSGE, false);
				AB(sgg_24.isSGE, false);
				AB(sgg_2511.isSGE, false);
				AB(sgg_2512.isSGE, false);
				AB(sgg_2521.isSGE, false);
				AB(sgg_2522.isSGE, false);
				AB(sgg_111.isSGG, true);
				AB(sgg_112.isSGG, true);
				AB(sgg_12.isSGG, true);
				AB(sgg_21.isSGG, true);
				AB(sgg_22.isSGG, true);
				AB(sgg_23.isSGG, true);
				AB(sgg_24.isSGG, true);
				AB(sgg_2511.isSGG, true);
				AB(sgg_2512.isSGG, true);
				AB(sgg_2521.isSGG, true);
				AB(sgg_2522.isSGG, true);
		}
		public void AssertSBsMembership(){
			foreach(Slottable sbp in sbpList){
				AE(sbp.sg, sgpAll);
				AE(ssm.ContainsInHierarchy(sbp), true);
				AE(ssm.poolBundle.ContainsInHierarchy(sbp), true);
				AE(ssm.equipBundle.ContainsInHierarchy(sbp), false);

				AE(sgpAll.ContainsInHierarchy(sbp), true);
				AE(sgpBow.ContainsInHierarchy(sbp), false);
				AE(sgpWear.ContainsInHierarchy(sbp), false);
				AE(sgpCGears.ContainsInHierarchy(sbp), false);
				AE(sgpParts.ContainsInHierarchy(sbp), false);
			}
			foreach(Slottable sbp2 in sbp2List){
				AE(ssm.ContainsInHierarchy(sbp2), true);
				AE(ssm.poolBundle.ContainsInHierarchy(sbp2), true);
				AE(sgpAll.ContainsInHierarchy(sbp2), false);
				if(sbp2.itemInst is BowInstanceMock){
					AE(sbp2.sg, sgpBow);
					AE(sgpBow.ContainsInHierarchy(sbp2), true);
					AE(sgpWear.ContainsInHierarchy(sbp2), false);
					AE(sgpCGears.ContainsInHierarchy(sbp2), false);
					AE(sgpParts.ContainsInHierarchy(sbp2), false);
				}else if(sbp2.itemInst is WearInstanceMock){
					AE(sbp2.sg, sgpWear);
					AE(sgpBow.ContainsInHierarchy(sbp2), false);
					AE(sgpWear.ContainsInHierarchy(sbp2), true);
					AE(sgpCGears.ContainsInHierarchy(sbp2), false);
					AE(sgpParts.ContainsInHierarchy(sbp2), false);
				}else if(sbp2.itemInst is CarriedGearInstanceMock){
					AE(sbp2.sg, sgpCGears);
					AE(sgpBow.ContainsInHierarchy(sbp2), false);
					AE(sgpWear.ContainsInHierarchy(sbp2), false);
					AE(sgpCGears.ContainsInHierarchy(sbp2), true);
					AE(sgpParts.ContainsInHierarchy(sbp2), false);
				}else if(sbp2.itemInst is PartsInstanceMock){
					AE(sbp2.sg, sgpParts);
					AE(sgpBow.ContainsInHierarchy(sbp2), false);
					AE(sgpWear.ContainsInHierarchy(sbp2), false);
					AE(sgpCGears.ContainsInHierarchy(sbp2), false);
					AE(sgpParts.ContainsInHierarchy(sbp2), true);
				}
			}
			foreach(Slottable sbe in sbeList){
				AE(ssm.ContainsInHierarchy(sbe), true);
				AE(ssm.poolBundle.ContainsInHierarchy(sbe), false);
				AE(ssm.equipBundle.ContainsInHierarchy(sbe), true);
				foreach(EquipmentSet eSet in ssm.equipmentSets){
					if(eSet == ssm.focusedEqSet)
						AE(eSet.ContainsInHierarchy(sbe), true);
					else
						AE(eSet.ContainsInHierarchy(sbe), false);
				}
				if(sbe.itemInst is BowInstanceMock){
					AE(sbe.sg, sgeBow);
					AE(sgeBow.ContainsInHierarchy(sbe), true);
					AE(sgeWear.ContainsInHierarchy(sbe), false);
					AE(sgeCGears.ContainsInHierarchy(sbe), false);
				}
				if(sbe.itemInst is WearInstanceMock){
					AE(sbe.sg, sgeWear);
					AE(sgeBow.ContainsInHierarchy(sbe), false);
					AE(sgeWear.ContainsInHierarchy(sbe), true);
					AE(sgeCGears.ContainsInHierarchy(sbe), false);
				}
				if(sbe.itemInst is CarriedGearInstanceMock){
					AE(sbe.sg, sgeCGears);
					AE(sgeBow.ContainsInHierarchy(sbe), false);
					AE(sgeWear.ContainsInHierarchy(sbe), false);
					AE(sgeCGears.ContainsInHierarchy(sbe), true);
				}
			}
			
		}
	/*	Test Transaction on All	*/
		public void TestSwapOnAll(){
			PerformOnAllSBs(CrossTestSwap);
			PrintTestResult(null);
			}
			public void CrossTestSwap(Slottable sb, bool isPAS){
				CrossTestSGPAndSGE(TestSwap, sb, isPAS);
				origSGCache = null;
			}
			public void TestSwap(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				SlotGroup origSG = testSB.sg;
				if(origSG != null)
					origSGCache = origSG;
				else
					origSG = origSGCache;
				InventoryItemInstanceMock testItem = testSB.itemInst;
				testSB = origSG.GetSB(testItem);
				if(testSB.isPickable){
					SlotSystemTransaction ta = ssm.GetTransaction(testSB, tarSG, null);
					if(ta is SwapTransaction){
						InventoryItemInstanceMock swapItem = ta.targetSB.itemInst;
						Swap(testSB, origSG, tarSG, null);
						/*	reverse	*/
						Swap(origSG.GetSB(swapItem), origSG, tarSG, null);
					}
					foreach(Slottable tarSB in tarSG){
						if(tarSB != null){
							testSB = origSG.GetSB(testItem);
							SlotSystemTransaction ta2 = ssm.GetTransaction(testSB, null, tarSB);
							if(ta2 is SwapTransaction){
								InventoryItemInstanceMock tarItem = tarSB.itemInst;
								Swap(testSB, origSG, null, tarSB);
								/*	reverse	*/
								Swap(tarSG.GetSB(testItem), tarSG, null, origSG.GetSB(tarItem));
							}
						}
					}
				}
			}
		public void TestFillOnAll(){
			PerformOnAllSBs(CrossTestFill);
			PrintTestResult(null);
			}public void CrossTestFill(Slottable sb, bool isPAS){
				CrossTestSGPAndSGE(TestFill, sb, isPAS);
				origSGCache = null;
			}
			public void TestFill(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				InventoryItemInstanceMock testItem = testSB.itemInst;
				SlotGroup origSG = testSB.sg;
				if(origSG != null)
					origSGCache = origSG;
				else
					origSG = origSGCache;
				testSB = origSG.GetSB(testItem);
				if(testSB.isPickable){
					if(ssm.GetTransaction(testSB, tarSG, null).GetType() == typeof(FillTransaction)){
							AssertFocused();
							/*	on SG	*/
								Fill(testSB, origSG, tarSG, null);
							/*	reverse	*/
								Fill(tarSG.GetSB(testItem), tarSG, origSG, null);
					}
					foreach(Slottable tarSB in tarSG){
						testSB = origSG.GetSB(testItem);
						if(tarSB != null){
							if(ssm.GetTransaction(testSB, null, tarSB).GetType() == typeof(FillTransaction)){
								/*	OnSB	*/
									Fill(testSB, origSG, null, tarSB);
								/*	reverse	*/
									Fill(tarSG.GetSB(testItem), tarSG, origSG, null);
							}
						}
					}
				}
			}
		public void TestVolSortOnAll(){
			PerformOnAllSGAfterFocusing(TestVolSort);
			}
			public void TestVolSort(SlotGroup sg, bool isPAS){
				if(!sg.isAutoSort){
					ssm.SortSG(sg, SlotGroup.InverseItemIDSorter);
						if(sg.actProcess != null && sg.actProcess.isRunning){
							ASSSM(ssm,
								null, null, sg, null, null, null, null, null, 
								SSMDeactivated, SSMFocused, null,
								SSMWFA, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(SortTransaction), true, true, false, true);
							CompleteAllSBActProcesses(sg);
						}
							AssertFocused();
							AssertSBsSorted(sg, SlotGroup.InverseItemIDSorter);

					ssm.SortSG(sg, SlotGroup.AcquisitionOrderSorter);
						if(sg.actProcess != null && sg.actProcess.isRunning){
							ASSSM(ssm,
								null, null, sg, null, null, null, null, null, 
								SSMDeactivated, SSMFocused, null,
								SSMWFA, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(SortTransaction), true, true, false, true);
							CompleteAllSBActProcesses(sg);
						}
							AssertFocused();
							AssertSBsSorted(sg, SlotGroup.AcquisitionOrderSorter);
					
					ssm.SortSG(sg, SlotGroup.ItemIDSorter);
						if(sg.actProcess != null && sg.actProcess.isRunning){
								ASSSM(ssm,
									null, null, sg, null, null, null, null, null, 
									SSMDeactivated, SSMFocused, null,
									SSMWFA, SSMTransaction, typeof(SSMTransactionProcess),
									typeof(SortTransaction), true, true, false, true);
							CompleteAllSBActProcesses(sg);
						}
							AssertFocused();
							AssertSBsSorted(sg, SlotGroup.ItemIDSorter);
				}
			}
		public void TestReorderOnAll(){
			PerformOnAllSBs(CrossTestReorder);
			PrintTestResult(null);
			}
			public void CrossTestReorder(Slottable testSB, bool isPAS){
				CrossTestSGPAndSGE(TestReorder, testSB ,isPAS);
			}
			public void TestReorder(SlotGroup targetSG, Slottable testSB, bool isPAS, bool isTAS){
				if(testSB.isPickable){
					SlotGroup origSG = testSB.sg;
					int initID = origSG.toList.IndexOf(testSB);
					foreach(Slottable targetSB in targetSG){
						if(ssm.GetTransaction(testSB, null, targetSB).GetType() == typeof(ReorderTransaction)){
							// Capture(testSB.ssm, testSB, null, targetSB, isPAS, isTAS, TestElement.SB);
							// Print(testSB.SGM, testSB, null, targetSB, isPAS, isTAS, TestElement.SB);
								Reorder(testSB, targetSB);
							/*	reverse	*/
								Reorder(testSB, (Slottable)origSG[initID]);
						}
					}
				}
			}
		public void TestRevertOnAllSBs(){
			PerformOnAllSBs(CrossTestRevert);
			PrintTestResult(null);
			}
			public void CrossTestRevert(Slottable pickedSB, bool isPAS){
				CrossTestSGPAndSGE(TestRevert, pickedSB, isPAS);
			}
			public void TestRevert(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				if(testSB.isPickable){
					SlotGroup origSG = testSB.sg;
					SlotSystemTransaction ta = ssm.GetTransaction(testSB, tarSG, null);
					if(ta is RevertTransaction){
						Revert(testSB, tarSG, null);
						foreach(Slottable tarSB in tarSG){
							if(tarSB != null){
								if(ssm.GetTransaction(testSB, null, tarSB).GetType() == typeof(RevertTransaction)){
									Revert(testSB, null, tarSB);
								}
							}
						}
					}
				}
			}
	/*	Transaction test preliminary	*/
		public void AssertEquippedOnAll(){
			AE(ssm.equippedBowInst, sgeBow.slots[0].sb.itemInst);
			AE(ssm.equippedWearInst, sgeWear.slots[0].sb.itemInst);
			AssertEquipped(ssm.equippedBowInst);
			AssertEquipped(ssm.equippedWearInst);
			AECGears(ssm.equippedCarriedGears, ssm.poolInv, ssm.equipInv);
		}
		public void TestReorderSBsMethod(){
			List<Slottable> sbs = testSBs;
			PrintSBsArray(sbs);
			sbs.Shuffle();
			PrintSBsArray(sbs);
			sbs.Reorder(sbs[0], sbs[3]);
			PrintSBsArray(sbs);
			sbs.Reorder(sbs[sbs.Count - 1], sbs[0]);
			PrintSBsArray(sbs);
		}
		public void TestDraggedIconOnAll(){
			PerformOnAllSBs(TestDraggedIcon);
			}
			public void TestDraggedIcon(Slottable sb, bool isPAS){
				DraggedIcon di = new DraggedIcon(sb);
				ssm.SetDIcon1(di);
				ssm.SetTransaction(new EmptyTransaction());
				ssm.transaction.Execute();
				ASSSM(ssm,
					null, null, null, null, sb, null, null, null, 
					SSMDeactivated, SSMFocused, null,
					SSMWFA, SSMTransaction, typeof(SSMTransactionProcess),
					typeof(EmptyTransaction), false, true, true, true);
				di.CompleteMovement();
				ASSSM(ssm,
					null, null, null, null, null, null, null, null, 
					SSMDeactivated, SSMFocused, null,
					SSMTransaction, SSMWFA, null,
					null, true, true, true, true);
			}
		public void TestAcceptSGTACompOnAll(){
			PerformOnAllSGAfterFocusing(TestAcceptSGTAComp);
			}
			public void TestAcceptSGTAComp(SlotGroup sg, bool isPAS){
				AssertFocused();
				ssm.SetSG1(sg);
				ASSSM(ssm,
					null, null, sg, null, null, null, null, null,
					SSMDeactivated, SSMFocused, null,
					null, SSMWFA, null,
					null, true, true, false, true);
				ssm.SetTransaction(new EmptyTransaction());
				ssm.transaction.Execute();
				ASSSM(ssm,
					null, null, sg, null, null, null, null, null,
					SSMDeactivated, SSMFocused, null,
					SSMWFA, SSMTransaction, typeof(SSMTransactionProcess),
					typeof(EmptyTransaction), true, true, false, true);
				ssm.AcceptSGTAComp(sg);
				ASSSM(ssm,
					null, null, null, null, null, null, null, null,
					SSMDeactivated, SSMFocused, null,
					SSMTransaction, SSMWFA, null,
					null, true, true, true, true);
			}
		public void TestSSMStateTransition(){
			/*	Selecttion state */
					ASSSM(ssm,
					null, null, null, null, null, null, null, null, 
					SSMDeactivated, SSMFocused, null,
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
				ssm.SetSelState(SlotSystemManager.ssmDefocusedState);
					ASSSM(ssm,
					null, null, null, null, null, null, null, null, 
					SSMFocused, SSMDefocused, typeof(SSMGreyoutProcess),
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
				ssm.SetSelState(SlotSystemManager.ssmFocusedState);
					ASSSM(ssm,
					null, null, null, null, null, null, null, null, 
					SSMDefocused, SSMFocused, typeof(SSMGreyinProcess),
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
				ssm.SetSelState(SlotSystemManager.ssmDeactivatedState);
					ASSSM(ssm,
					null, null, null, null, null, null, null, null, 
					SSMFocused, SSMDeactivated, null,
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
				ssm.SetSelState(SlotSystemManager.ssmDefocusedState);
					ASSSM(ssm,
					null, null, null, null, null, null, null, null, 
					SSMDeactivated, SSMDefocused, null,
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
				ssm.SetSelState(SlotSystemManager.ssmDeactivatedState);
					ASSSM(ssm,
					null, null, null, null, null, null, null, null, 
					SSMDefocused, SSMDeactivated, null,
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
				ssm.SetSelState(SlotSystemManager.ssmFocusedState);
					ASSSM(ssm,
					null, null, null, null, null, null, null, null, 
					SSMDeactivated, SSMFocused, null,
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
			/*	Action state */	// maybe after transaction is done 
				// 	ASSSM(sgm,
				// 	SSMDeactivated, SSMFocused, null,
				// 	SSMWFA, SSMWFA, null,
				// 	null, null,
				// 	true, true, true, true);
				// sgm.SetActState(SlotGroupManager.ProbingState)
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
			}
		public void TestSBStateTransitionOnAll(){
			PerformOnAllSBs(TestSBStateTransition);
			PrintTestResult(null);
		}
			public void TestSBStateTransition(Slottable sb, bool isPAS){
				/*	SelState */
					if(isPAS){
						if(sb.sg.isPool){
							if(sb.isEquipped || (sb.itemInst is PartsInstanceMock && !(sb.sg.Filter is SGPartsFilter))){
								if(sb.prevSelState == SBDeactivated){
									ASBSelState(sb, SBDeactivated, SBDefocused, null);

								}else
									ASBSelState(sb, SBFocused, SBDefocused, typeof(SBGreyoutProcess));

								sb.SetSelState(SBSelected);
									ASBSelState(sb, SBDefocused, SBSelected, typeof(SBHighlightProcess));
								sb.SetSelState(SBDefocused);
									ASBSelState(sb, SBSelected, SBDefocused, typeof(SBGreyoutProcess));
								sb.SetSelState(SBFocused);
									ASBSelState(sb, SBDefocused, SBFocused, typeof(SBGreyinProcess));
								return;
							}
						}
					}
					if(sb.prevSelState == SBDeactivated)
						ASBSelState(sb, SBDeactivated, SBFocused, null);
					else
						ASBSelState(sb, SBDefocused, SBFocused, typeof(SBGreyinProcess));
					sb.SetSelState(SBSelected);
						ASBSelState(sb, SBFocused, SBSelected, typeof(SBHighlightProcess));
					sb.SetSelState(SBFocused);
						ASBSelState(sb, SBSelected, SBFocused, typeof(SBDehighlightProcess));
					sb.SetSelState(SBDeactivated);
						ASBSelState(sb, SBFocused, SBDeactivated, null);
					sb.SetSelState(SBFocused);
						ASBSelState(sb, SBDeactivated, SBFocused, null);
					sb.SetSelState(SBDeactivated);
						ASBSelState(sb, SBFocused, SBDeactivated, null);
					sb.SetSelState(SBDefocused);
						ASBSelState(sb, SBDeactivated, SBDefocused, null);
					sb.SetSelState(SBDeactivated);
						ASBSelState(sb, SBDefocused, SBDeactivated, null);
					sb.SetSelState(SBSelected);
						ASBSelState(sb, SBDeactivated, SBSelected, null);
					sb.SetSelState(SBDeactivated);
						ASBSelState(sb, SBSelected, SBDeactivated, null);
				/*	Equip State	*/
				if(sb.sg.isPool){
					if(sb.isEquipped){
							ASBEqpState(sb, SBEquipped, SBEquipped, null);
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
					sb.SetEqpState(SBUnequipped);
						ASBEqpState(sb, SBEquipped, SBUnequipped, null);
					sb.SetEqpState(SBEquipped);
						ASBEqpState(sb, SBUnequipped, SBEquipped, null);
				}
			}
		public void TestPickUpTransitionOnAll(){
			PerformOnAllSBs(TestPickUpTransition);
		}
			public void TestPickUpTransition(Slottable sb, bool isPAS){
					AssertFocused();
					SlotGroup origSG = sb.sg;
				if(sb.isPickable){
					/*	tap	*/
						sb.OnPointerDownMock(eventData);
							ASSSM(ssm,
								null, null, null, null, null, null, null, null,
								SSMDeactivated, SSMFocused, null,
								null, SSMWFA, null,
								null, true, true, true, true);
							ASSG(origSG,
								null, SGFocused, null,
								null, SGWFA, null, false);
							ASSB(sb,
								SBFocused, SBSelected, typeof(SBHighlightProcess),
								SBWFA, SBWFPickUp, typeof(WaitForPickUpProcess), true,
								null, null, null);
						sb.OnPointerUpMock(eventData);
							ASSB(sb,
								SBFocused, SBSelected, typeof(SBHighlightProcess),
								SBWFPickUp, SBWFNT, typeof(WaitForNextTouchProcess), true,
								null, null, null);
						sb.actProcess.Expire();
							ASSB(sb,
								SBSelected, SBFocused, typeof(SBDehighlightProcess),
								SBWFNT, SBWFA, null, false,
								null, null, null);
						AssertFocused();
					/*	multi tap -> pickup	*/
						sb.OnPointerDownMock(eventData);
						sb.OnPointerUpMock(eventData);
						sb.OnPointerDownMock(eventData);
							ASSSM(ssm,
								sb, null, null, null, sb, null, null, sb,
								SSMDeactivated, SSMFocused, null,
								SSMWFA, SSMProbing, typeof(SSMProbeProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								null, SGWFA, null, false);
							ASSB(sb,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFNT, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);							
						sb.OnPointerUpMock(eventData);
						if(sb.isStackable){
							ASSB(sb,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBPickedUp, SBWFNT, typeof(WaitForNextTouchProcess), true,
								null, null, null);
							sb.actProcess.Expire();
						}
							/*reverting*/
							ASSSM(ssm,
								sb, null, null, null, sb, null, null, sb,
								SSMDeactivated, SSMFocused, null,
								SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								SGWFA, SGRevert, typeof(SGTransactionProcess), false);
							ASSB(sb,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								sb.isStackable?SBWFNT:SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), false,
								null, null, null);
						ssm.dIcon1.CompleteMovement();
						AssertFocused();
					/*	pickup -> release -> expire to revert	*/
						PickUp(sb, out picked);
						LetGo();
							ASSB(sb,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								sb.isStackable?SBWFNT:SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), false,
								null, null, null);
						ssm.dIcon1.CompleteMovement();
						AssertFocused();
					/* pickup -> release -> touch	*/
						if(sb.isStackable){
							PickUp(sb, out picked);
							sb.OnPointerUpMock(eventData);
							sb.OnPointerDownMock(eventData);
							LetGo();
							ssm.dIcon1.CompleteMovement();
							AssertFocused();
						}
				}else{
					sb.OnPointerDownMock(eventData);
						ASSSM(ssm,
							null, null, null, null, null, null, null, null,
							SSMDeactivated, SSMFocused, null,
							null, SSMWFA, null,
							null, true, true, true, true);
						ASSB(sb,
							null, SBDefocused, null,
							SBWFA, SBWFPointerUp, typeof(WaitForPointerUpProcess), true,
							null,null,null);
					sb.OnPointerUpMock(eventData);
					AssertFocused();
				}
			}
		public void CheckPickcableOnAllSB(){
			PerformOnAllSBs(CheckPickable);
			}public void CheckPickable(Slottable sb, bool isPas){
				bool pickable = sb.isPickable;
				Debug.Log(Util.SBofSG(sb)+ " isPickable: " + (pickable?Util.Blue("true"): Util.Red("false")));
			}
		public void CheckTransacitonWithSBSpecifiedOnAll(){
			PerformOnAllSBs(CheckTransactionWithSB);
			PrintTestResult(null);
			}public void CheckTransactionWithSB(Slottable sb, bool isPickedAS){
				CrossTestSGPAndSGE(CrossCheckTransactionWithSB, sb, isPickedAS);
			}
			public void CrossCheckTransactionWithSB(SlotGroup sg, Slottable pickedSB, bool isPickedAS, bool isTargetAS){
				if(pickedSB.isPickable){
					foreach(Slottable sb in sg){
						if(sb != null){
							Capture(sb.ssm, pickedSB, sg, sb, isPickedAS, isTargetAS, TestElement.TA);
						}
					}
				}
			}
		public void CheckShrinkableAndExpandableOnAllSGs(){
			foreach(SlotGroup sg in ssm.allSGs){
				string shrinkStr = " Shrinkable: " + sg.isShrinkable;
				if(sg.isShrinkable)	shrinkStr = Blue(shrinkStr);
				else shrinkStr = Red(shrinkStr);
				string expandStr = " Expandable: " + sg.isExpandable;
				if(sg.isExpandable)	expandStr = Blue(expandStr);
				else expandStr = Red(expandStr);
				Debug.Log(sg.eName + shrinkStr + ", " + expandStr);
			}
		}
		public void CheckSwappableOnAll(){
			PerformOnAllSBs(CrossCheckSwappable);
			PrintTestResult(0.ToString());
			}public void CrossCheckSwappable(Slottable sb, bool isPickedAS){
				CrossTestSGPAndSGE(CheckSwappable, sb, isPickedAS);
			}
			public void CheckSwappable(SlotGroup sg, Slottable sb, bool isPickedAS ,bool isTargetAS){
				int count = sg.SwappableSBs(sb).Count;
				SlotSystemTestResult newRes = new SlotSystemTestResult(sg.ssm, isPickedAS, isTargetAS, sb, sg, null, count);
				testResults.Add(newRes);
			}
		public void CheckTransactionOnAllSG(){
				PerformOnAllSBs(CrossCheckTransaction);
				PrintTestResult(null);
				}public void CrossCheckTransaction(Slottable sb, bool isPickedAS){
					CrossTestSGPAndSGE(CheckTransaction, sb, isPickedAS);
				}
				public void CheckTransaction(SlotGroup sg, Slottable sb, bool isPAS ,bool isTAS){
					if(sb.isPickable){
						Capture(sg.ssm, sb, sg, null, isPAS, isTAS, TestElement.TA);
					}
				}
	/*	thorough testing utility	*/
		public List<IEnumerable<Slottable>> possibleSBsCombos(int subsetCount, List<Slottable> sbs){
			List<IEnumerable<Slottable>> result = new List<IEnumerable<Slottable>>();
			foreach(IEnumerable<Slottable> combo in ListMethods.Combinations<Slottable>(subsetCount, sbs)){
				List<IEnumerable<Slottable>> perms = ListMethods.Permutations<Slottable>(combo);
				result.AddRange(perms);
			}
			return result;
			}public void TestPermutation(){
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_p);
				sbs.Add(defWearA_p);
				sbs.Add(defShieldA_p);
				sbs.Add(defQuiverA_p);
				sbs.Add(defParts_p);
				List<IEnumerable<Slottable>> sbsPerms = ListMethods.Permutations<Slottable>(sbs);
				foreach(IEnumerable<Slottable> sbList in sbsPerms){
					PrintSBsArray(sbList);
				}
			}
			public void TestCombination(){
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_p);
				sbs.Add(defWearA_p);
				sbs.Add(defQuiverA_p);
				sbs.Add(defMWeaponA_p);
				sbs.Add(defParts_p);
				
				List<IEnumerable<Slottable>> possibleCombos = possibleSBsCombos(4, sbs);
				foreach(IEnumerable<Slottable> combo in possibleCombos)
					PrintSBsArray(combo);
			}
		public List<Slottable> transactableSBs(SlotGroup origSG, SlotGroup tarSG, Slottable tarSB, System.Type ta){
			List<Slottable> result = new List<Slottable>();
			foreach(Slottable sb in origSG){
				if(sb != null && sb.isPickable){
					if(ssm.GetTransaction(sb, tarSG, tarSB).GetType() == ta){
						result.Add(sb);
					}
				}
			}
			return result;
		}
		public void PerformOnAllSBs(System.Action<Slottable, bool> act){
			foreach(SlotGroup sg in ssm.allSGs){
				ssm.FindAndFocusInBundle(sg);
				sg.ToggleAutoSort(true);
				foreach(Slottable sb in sg.toList){
					if(sb != null)
						act(sb, true);
				}
				sg.ToggleAutoSort(false);
				foreach(Slottable sb in sg.toList){
					if(sb != null)
						act(sb, false);
				}
			}
		}
		public void PerformOnAllSGAfterFocusing(System.Action<SlotGroup, bool> act){
			foreach(SlotGroup sg in ssm.allSGs){
				ssm.FindAndFocusInBundle(sg);
				sg.ToggleAutoSort(true);
				act(sg, true);
				sg.ToggleAutoSort(false);
				act(sg, false);
			}
		}
		public void CrossTestSGPAndSGE(System.Action<SlotGroup, Slottable, bool, bool> act, Slottable sb, bool isPickedAS){
			if(sb.sg.isPool){
				act(sb.sg, sb, isPickedAS, isPickedAS);
				foreach(EquipmentSet eSet in ssm.equipmentSets){
					ssm.SetFocusedEquipmentSet(eSet);
					foreach(SlotGroup sge in ssm.focusedSGEs){
						sge.ToggleAutoSort(true);
						act(sge, sb, isPickedAS, true);
						sge.ToggleAutoSort(false);
						act(sge, sb, isPickedAS, false);
					}
				}
			}else if(sb.sg.isSGE){
				foreach(SlotGroup sgp in ssm.allSGPs){
					ssm.SetFocusedPoolSG(sgp);
					sgp.ToggleAutoSort(true);
					act(sgp, sb, isPickedAS, true);
					sgp.ToggleAutoSort(false);
					act(sgp, sb, isPickedAS, false);
				}
				foreach(SlotGroup sge in ssm.focusedSGEs){
					sge.ToggleAutoSort(true);
					act(sge, sb, isPickedAS, true);
					sge.ToggleAutoSort(false);
					act(sge, sb, isPickedAS, false);
				}
			}
		}
		public void PerformOnAllSGs(System.Action<SlotGroup> act){
			foreach(SlotGroup sgp in ssm.allSGs){
				act(sgp);
			}
		}
	/*	actions	*/
		public void ClearSGCGearsTo(SlotGroup sgp){
			foreach(Slottable sb in sgeCGears){
				if(sb != null){
					Fill(sb, sgeCGears, sgp, null);
				}
			}
			AssertSGCounts(sgeCGears, ssm.equipInv.equippableCGearsCount, 0, 0);
			AssertFocused();
		}
		public void Swap(Slottable testSB, SlotGroup origSG, SlotGroup hovSG, Slottable hovSB){
			bool isOnSG = hovSG != null && hovSB == null;
			bool isOnSB = hovSB != null && hovSG == null;
			if(isOnSG || isOnSB){
				if(testSB.isPickable){
					SlotSystemTransaction ta = ssm.GetTransaction(testSB, hovSG, hovSB);
					if(ta.GetType() == typeof(SwapTransaction)){
						Slottable targetSB = isOnSB?hovSB:ta.targetSB;
						SlotGroup targetSG = isOnSG?hovSG:ta.sg2;
						AssertFocused();
							ASSSM(ssm,
								null, null, null, null, null, null, null, null,
								null, SSMFocused, null,
								null, SSMWFA, null,
								null, true, true, true, true);
							ASSG(origSG,
								null, SGFocused, null,
								null, SGWFA, null, false);
							ASSB(testSB,
								null, SBFocused, null,
								null, SBWFA, null, false,
								null, null, null);
						PickUp(testSB, out picked);
							ASSSM(ssm,
								testSB, null, null, null, testSB, null, null, testSB,
								null, SSMFocused, null,
								SSMWFA, SSMProbing, typeof(SSMProbeProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								null, SGWFA, null, false);
							ASSB(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);
						if(isOnSG)
							SimHover(null, targetSG, eventData);
						else if(isOnSB)
							SimHover(targetSB, null, eventData);
							ASSSM(ssm,
								testSB, targetSB, origSG, targetSG, testSB, null/*null until execution*/, isOnSG?hovSG:null, isOnSB?hovSB:null,
								null, SSMFocused, null,
								SSMWFA, SSMProbing, typeof(SSMProbeProcess),
								typeof(SwapTransaction), false, true/* */, false, false);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								null, SGWFA, null, false);
							ASSG(targetSG,
								null, SGSelected, typeof(SGHighlightProcess),
								null, SGWFA, null, false);
							ASSB(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);
							ASSB(targetSB,
								null, SBSelected, typeof(SBHighlightProcess),
								null, SBWFA, null, false,
								null, null, null);
						LetGo();
							ASSSM(ssm,
								testSB, targetSB, origSG, targetSG, testSB, targetSB, isOnSG?hovSG:null, isOnSB?hovSB:null,
								null, SSMFocused, null,
								SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(SwapTransaction), false, false, origSG.isAllTASBsDone?true:false, targetSG.isAllTASBsDone?true:false);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								SGWFA, SGSwap, typeof(SGTransactionProcess), origSG.isAllTASBsDone?false:true);
							ASSG(targetSG,
								null, SGSelected, typeof(SGHighlightProcess),
								SGWFA, SGSwap, typeof(SGTransactionProcess), targetSG.isAllTASBsDone?false:true);
							ASBSelState(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess));
								if(origSG.isPool)
									ASBActState(testSB,
										SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), testSB.actProcess.isRunning);
								else
									ASBActState(testSB,
										SBPickedUp, SBRemove, typeof(SBRemoveProcess), true);
								if(targetSG.isSGE)
									ASBEqpState(testSB,
										SBUnequipped, SBEquipped, typeof(SBEquipProcess));
							ASBSelState(targetSB,
								null, SBSelected, typeof(SBHighlightProcess));
								if(targetSG.isPool)
									ASBActState(targetSB,
										SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), targetSB.actProcess.isRunning);
								else
									ASBActState(targetSB,
										SBWFA, SBRemove, typeof(SBRemoveProcess), true);
							Slottable tarSBinOrigSG = origSG.GetSB(targetSB.itemInst);
							if(origSG.isPool)
								ASSB(tarSBinOrigSG,
									null, origSG.isAutoSort?SBDefocused:SBFocused, null,
									SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), tarSBinOrigSG.actProcess.isRunning,
									SBEquipped, SBUnequipped, typeof(SBUnequipProcess));
							else
								ASSB(tarSBinOrigSG,
									null, SBDefocused, null,
									SBWFA, SBAdd, typeof(SBAddProcess), true,
									SBUnequipped, SBEquipped, null);
							Slottable testSBinTarSG = targetSG.GetSB(testSB.itemInst);
							if(targetSG.isPool)/* 	unequipped	*/
								ASSB(testSBinTarSG,
									null, SBDefocused, null,
									SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), testSBinTarSG.actProcess.isRunning,
									SBEquipped, SBUnequipped, typeof(SBUnequipProcess));
							else/*	newly created	*/
								ASSB(testSBinTarSG,
									SBDeactivated, SBDefocused, null,
									SBWFA, SBAdd, typeof(SBAddProcess), true,
									SBUnequipped, SBEquipped, null);
						if(!origSG.isAllTASBsDone)
							CompleteAllSBActProcesses(origSG);
						if(!targetSG.isAllTASBsDone)
							CompleteAllSBActProcesses(targetSG);
						ssm.dIcon1.CompleteMovement();
						ssm.dIcon2.CompleteMovement();
						AssertFocused();
					}else
						throw new System.InvalidOperationException("SlottableTest.Swap: given combination of arguments does not result in SwapTransaction");
				}else
					throw new System.InvalidOperationException("SlottableTest.Swap: testSB is not pickable");
			}else
				throw new System.InvalidOperationException("SlottableTest.Swap: tarSG and tarSB not supplied correctly");
			}public void TestSwapShortcut(){
			PerformOnAllSBs(CrossTestSwapShortcut);
			PrintTestResult(null);
			}
			public void CrossTestSwapShortcut(Slottable sb, bool isPAS){
				CrossTestSGPAndSGE(TestSwapShortcut, sb, isPAS);
				origSGCache = null;
			}
			public void TestSwapShortcut(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				InventoryItemInstanceMock testItem = testSB.itemInst;
				SlotGroup origSG = testSB.sg;
				if(origSG != null)
					origSGCache = origSG;
				else
					origSG = origSGCache;
				testSB = origSG.GetSB(testItem);
				if(testSB.isPickable){
					SlotSystemTransaction ta = ssm.GetTransaction(testSB, tarSG, null);
					if(ta.GetType() == typeof(SwapTransaction)){
						InventoryItemInstanceMock swapItem = ta.targetSB.itemInst;
						Swap(testSB, origSG, tarSG, null);
						/*	reverse */
						Swap(origSG.GetSB(swapItem), origSG, tarSG, null);
						// Capture(ssm, testSB, tarSG, null, isPAS, isTAS, TestElement.SG);
					}
				}
				foreach(Slottable tarSB in tarSG){
					if(tarSB != null){
						testSB = origSG.GetSB(testItem);
						if(testSB.isPickable){
							SlotSystemTransaction ta = ssm.GetTransaction(testSB, null, tarSB);
							if(ta.GetType() == typeof(SwapTransaction)){
								Capture(ssm, testSB, null, tarSB, isPAS, isTAS, TestElement.SB);
								InventoryItemInstanceMock swapItem = tarSB.itemInst;
								Swap(testSB, origSG, null, tarSB);
								/*	reverse	*/
								Swap(tarSG.GetSB(testItem), tarSG, null, origSG.GetSB(swapItem));
							}
						}
					}
				}
			}

		public void Fill(Slottable testSB, SlotGroup origSG, SlotGroup hovSG, Slottable hovSB){
			if(testSB.isPickable){
				bool isOnSG = hovSG != null && hovSB == null;
				bool isOnSB = hovSB != null && hovSG == null;
				if(isOnSG || isOnSB){
					SlotSystemTransaction ta = ssm.GetTransaction(testSB, hovSG, hovSB);
					if(ta.GetType() == typeof(FillTransaction)){
						SlotGroup targetSG = isOnSG?hovSG:ta.sg2;
						AssertFocused();
							ASSSM(ssm,
								null, null, null, null, null, null, null, null,
								null, SSMFocused, null,
								null, SSMWFA, null,
								null, true, true, true, true);
							ASSG(origSG,
								null, SGFocused, null,
								null, SGWFA, null, false);
							ASSB(testSB,
								null, SBFocused, null,
								null, SBWFA, null, false,
								null, null, null);
						PickUp(testSB, out picked);
							ASSSM(ssm,
								testSB, null, null, null, testSB, null, null, testSB,
								null, SSMFocused, null,
								SSMWFA, SSMProbing, typeof(SSMProbeProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								null, SGWFA, null, false);
							ASSG(targetSG,
								null, SGFocused, null,
								null, SGWFA, null, false);
							ASSB(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);
							if(isOnSB && hovSB != testSB)
							ASSB(hovSB,
								null, SBDefocused, null,
								null, SBWFA, null, false,
								null, null, null);
						if(hovSG != null)
							SimHover(null, hovSG, eventData);
						else if(hovSB != null){
							SimHover(hovSB, null, eventData);
						}
							ASSSM(ssm,
								testSB, null, origSG, targetSG, testSB, null, isOnSG?hovSG:null, isOnSG?null:hovSB,
								null, SSMFocused, null,
								SSMWFA, SSMProbing, typeof(SSMProbeProcess),
								typeof(FillTransaction), false, true, false, false);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								null, SGWFA, null, false);
							ASSG(targetSG,
								SGFocused, SGSelected, typeof(SGHighlightProcess),
								null, SGWFA, null, false);
							ASSB(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);
							if(isOnSB && hovSB != testSB)
							ASSB(hovSB,
								null, SBDefocused, null,
								null, SBWFA, null, false,
								null, null, null);
						LetGo();
							ASSSM(ssm,
								testSB, null, origSG, targetSG, testSB, null, isOnSG?hovSG:null, isOnSG?null:hovSB,
								null, SSMFocused, null,
								SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(FillTransaction), false, true, origSG.isAllTASBsDone?true:false, targetSG.isAllTASBsDone?true:false);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								SGWFA, SGFill, typeof(SGTransactionProcess), !origSG.isAllTASBsDone);
							ASSG(targetSG,
								SGFocused, SGSelected, typeof(SGHighlightProcess),
								SGWFA, SGFill, typeof(SGTransactionProcess), !targetSG.isAllTASBsDone);
							ASBSelState(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess));
								if(origSG.isPool){
									ASBActState(testSB,
										SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), !origSG.isAllTASBsDone);
								}else{
									ASBActState(testSB,
										SBPickedUp, SBRemove, typeof(SBRemoveProcess), true);
								}
							Slottable testSBinTarSG = targetSG.GetSB(testSB.itemInst);
							ASBSelState(testSBinTarSG,
								null, SBDefocused, null);
								if(targetSG.isPool){
									ASBActState(testSBinTarSG,
										SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), !targetSG.isAllTASBsDone);
								}else{
									ASBActState(testSBinTarSG,
										SBWFA, SBAdd, typeof(SBAddProcess), true);
								}
							if(isOnSB && hovSB != testSB)
							ASSB(hovSB,
								null, SBDefocused, null,
								SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), hovSB.actProcess.isRunning,
								null, null, null);
						if(!origSG.isAllTASBsDone)
							CompleteAllSBActProcesses(origSG);
							ASSSM(ssm,
								testSB, null, origSG, targetSG, testSB, null, isOnSG?hovSG:null, isOnSG?null:hovSB,
								null, SSMFocused, null,
								SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(FillTransaction), false, true, true, targetSG.isAllTASBsDone?true:false);
						if(!targetSG.isAllTASBsDone)
							CompleteAllSBActProcesses(targetSG);
							ASSSM(ssm,
								testSB, null, origSG, targetSG, testSB, null, isOnSG?hovSG:null, isOnSG?null:hovSB,
								null, SSMFocused, null,
								SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(FillTransaction), false, true, true, true);
						ssm.dIcon1.CompleteMovement();
						AssertFocused();
					}else
						throw new System.InvalidOperationException("SlottableTest.Fill: given combination of arguments does not result in FillTransaction");
				}else
					throw new System.InvalidOperationException("SlottableTest.Fill: tarSG and tarSB not supplied correctly");
			}else
				throw new System.InvalidOperationException("SlottableTest.Fill: testSB is not pickable");
			}public void TestFillShortcut(){
				PerformOnAllSBs(CrossTestFillShortcut);
				PrintTestResult(null);
			}
			public void CrossTestFillShortcut(Slottable sb, bool isPAS){
				CrossTestSGPAndSGE(TestFillShortcut, sb, isPAS);
				origSGCache = null;
			}
			public void TestFillShortcut(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				InventoryItemInstanceMock testItem = testSB.itemInst;
				SlotGroup origSG = testSB.sg;
				if(origSG != null)
					origSGCache = origSG;
				else
					origSG = origSGCache;
				testSB = origSG.GetSB(testItem);
				if(testSB.isPickable){
					if(ssm.GetTransaction(testSB, tarSG, null).GetType() == typeof(FillTransaction)){
						Fill(testSB, origSG, tarSG, null);
						/*	rev */
						Fill(tarSG.GetSB(testItem), tarSG, origSG, null);
					}
					foreach(Slottable tarSB in tarSG){
						if(tarSB != null){
							testSB = origSG.GetSB(testItem);
							if(ssm.GetTransaction(testSB, null, tarSB).GetType() == typeof(FillTransaction)){
								Fill(testSB, origSG, null, tarSB);
								/*	rev */
								Fill(tarSG.GetSB(testItem), tarSG, origSG, null);
							}
						}
					}
				}
			}

		public void PickUp(Slottable sb, out bool pickedUp){
			SlotGroup origSG = sb.sg;
			AssertFocused();
			if(sb.isPickable){
				sb.OnPointerDownMock(eventData);
					ASSSM(sb.ssm,
						null, null, null, null, null, null, null, null, 
						null, SSMFocused, null,
						null, SSMWFA, null,
						null, true, true, true, true);
					ASSG(origSG,
						null, SGFocused, null,
						null, SGWFA, null, false);
					ASSB(sb,
						SBFocused, SBSelected, typeof(SBHighlightProcess),
						SBWFA, SBWFPickUp, typeof(WaitForPickUpProcess), true, 
						null, null, null);
				sb.actProcess.Expire();
					ASSSM(sb.ssm,
						sb, null, null, null, sb, null, null, sb, 
						null, SSMFocused, null,
						SSMWFA, SSMProbing, typeof(SSMProbeProcess),
						typeof(RevertTransaction), false, true, true, true);
					ASSG(origSG,
						SGFocused, SGDefocused, typeof(SGGreyoutProcess),
						null, SGWFA, null, false);
					ASSB(sb,
						SBSelected, SBDefocused, typeof(SBGreyoutProcess),
						SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true, 
						null, null, null);
				pickedUp = true;
			}else{
				pickedUp = false;
			}
		}
		public void PointerUp(){
			ssm.pickedSB.OnPointerUpMock(eventData);
		}
		public void LetGo(){
			Slottable pickedSB = ssm.pickedSB;
				ASSMActState(ssm, SSMWFA, SSMProbing, typeof(SSMProbeProcess));
				ASSG(pickedSB.sg,
					SGFocused, SGDefocused, typeof(SGGreyoutProcess),
					null, SGWFA, null, false);
				ASSB(pickedSB,
					SBSelected, SBDefocused, typeof(SBGreyoutProcess),
					null, SBPickedUp, null, false,
					null, null, null);
			pickedSB.OnPointerUpMock(eventData);
			if(ssm.pickedSB.curActState == Slottable.waitForNextTouchState){
				ASSMActState(ssm, SSMWFA, SSMProbing, typeof(SSMProbeProcess));
				ASSG(pickedSB.sg,
					SGFocused, SGDefocused, typeof(SGGreyoutProcess),
					null, SGWFA, null, false);
				ASSB(pickedSB,
					SBSelected, SBDefocused, typeof(SBGreyoutProcess),
					SBPickedUp, SBWFNT, typeof(WaitForNextTouchProcess), true,
					null, null, null);
				ssm.pickedSB.actProcess.Expire();
			}
				ASSMActState(ssm, SSMProbing, SSMTransaction, typeof(SSMTransactionProcess));
				ASGSelState(pickedSB.sg,
					SGFocused, SGDefocused, typeof(SGGreyoutProcess));
		}
		public void CompleteAllSBActProcesses(SlotGroup sg){
			foreach(Slottable sb in sg){
				if(sb != null){
					if(sb.actProcess.GetType() == typeof(SBRemoveProcess) ||
					sb.actProcess.GetType() == typeof(SBAddProcess) ||
					sb.actProcess.GetType() == typeof(SBMoveWithinProcess))
						if(sb.actProcess.isRunning)
							sb.actProcess.Expire();
				}
			}
			sg.CheckProcessCompletion();
		}
		public void SimHover(Slottable hovSB, SlotGroup hovSG, PointerEventDataMock eventData){
			/*	revised version
					sgm.SetHovered(sb, sg);
						=> update hovered fields
					sgm.UpdateTransaction();
						=> update target fields
							=> thus the selection states of prev and current tartgets
			*/
			/*	in actual implementation, this method is called whenever either sb or sg's boarder is crossed
			*/
			ssm.SetHoveredSB(hovSB); ssm.SetHoveredSG(hovSG);
			ssm.CreateTransactionResults();
			ssm.UpdateTransaction();
		}
	/*	Assertions	*/
		/*	SSM	*/
			public void ASSMSelState(SlotSystemManager ssm, SSMSelState prev, SSMSelState cur, System.Type selProcT){
				AE(ssm.curSelState, cur);
				if(prev != null){
					AE(ssm.prevSelState, prev);
					if(selProcT != null)
						AE(ssm.selProcess.GetType(), selProcT);
					else
						ANull(ssm.selProcess);
				}
			}
			public void ASSMActState(SlotSystemManager ssm, SSMActState prev, SSMActState cur, System.Type actProcT){
				AE(ssm.curActState, cur);
				if(prev != null){
					AE(ssm.prevActState, prev);
					if(actProcT != null)
						AE(ssm.actProcess.GetType(), actProcT);
					else
						ANull(ssm.actProcess);
				}
			}
			public void ASSSM(SlotSystemManager ssm,
				Slottable pickedSB, Slottable targetSB, SlotGroup sg1, SlotGroup sg2, 
				Slottable di1SB, Slottable di2SB, SlotGroup hoveredSG, Slottable hoveredSB,
				SSMSelState prevSel, SSMSelState curSel, System.Type selProcT,
				SSMActState prevAct, SSMActState curAct, System.Type actProcT,
				System.Type taType, bool d1Done, bool d2Done, bool sg1Done, bool sg2Done){
				AE(ssm.pickedSB, pickedSB);
				AE(ssm.targetSB, targetSB);
				AE(ssm.sg1, sg1);
				AE(ssm.sg2, sg2);
				if(di1SB != null)
					AE(ssm.dIcon1.item, di1SB.itemInst);
				else
					AE(ssm.dIcon1, null);
				if(di2SB != null)
					AE(ssm.dIcon2.item, di2SB.itemInst);
				else
					AE(ssm.dIcon2, null);
				AE(ssm.hoveredSG, hoveredSG);
				AE(ssm.hoveredSB, hoveredSB);
				ASSMSelState(ssm, prevSel, curSel, selProcT);
				ASSMActState(ssm, prevAct, curAct, actProcT);
				if(taType == null) ANull(ssm.transaction);
				else AE(ssm.transaction.GetType(), taType);
				AE(ssm.dIcon1Done, d1Done);
				AE(ssm.dIcon2Done, d2Done);
				AE(ssm.sg1Done, sg1Done);
				AE(ssm.sg2Done, sg2Done);
			}
			public void ASSSM(SlotSystemManager ssm,
				Slottable pickedSB, Slottable targetSB, SlotGroup sg1, SlotGroup sg2, 
				DraggedIcon di1, DraggedIcon di2, SlotGroup hoveredSG, Slottable hoveredSB,
				SSMSelState prevSel, SSMSelState curSel, System.Type selProcT,
				SSMActState prevAct, SSMActState curAct, System.Type actProcT){
				AE(ssm.pickedSB, pickedSB);
				AE(ssm.targetSB, targetSB);
				AE(ssm.sg1, sg1);
				AE(ssm.sg2, sg2);
				AE(ssm.dIcon1, di1);
				AE(ssm.dIcon2, di2);
				AE(ssm.hoveredSG, hoveredSG);
				AE(ssm.hoveredSB, hoveredSB);
				ASSMSelState(ssm, prevSel, curSel, selProcT);
				ASSMActState(ssm, prevAct, curAct, actProcT);
			}
		/*	SG	*/
			public void ASGSelState(SlotGroup sg, SGSelState prev, SGSelState cur, System.Type procT){
				AE(sg.curSelState, cur);
				if(prev != null){
					AE(sg.prevSelState, prev);
					if(procT != null)
						AE(sg.selProcess.GetType(), procT);
					else
						ANull(sg.selProcess);
				}
			}
			public void ASGActState(SlotGroup sg, SGActState prev, SGActState cur, System.Type procT, bool isRunning){
				AE(sg.curActState, cur);
				if(prev != null){
					AE(sg.prevActState, prev);
					if(procT != null){
						AE(sg.actProcess.GetType(), procT);
						AE(sg.actProcess.isRunning, isRunning);
					}
					else
						ANull(sg.actProcess);
				}
			}
			public void AssertSGCounts(SlotGroup sg, int slotsC, int itemC, int sbsC){
				AE(sg.slots.Count, slotsC);
				AE(sg.actualItemInsts.Count, itemC);
				AE(sg.actualSBsCount, sbsC);
			}
			public void AssertSBsSorted(SlotGroup sg, SGSorter sorter){
				List<Slottable> sbs = sg.toList;
				sorter.OrderSBsWithRetainedSize(ref sbs);
				AE(sbs.Count, sg.Count);
				for(int i = 0; i < sg.Count; i++){
					AE((Slottable)sg[i], sbs[i]);
				}
			}
			public void ASSG(SlotGroup sg, SGSelState prevSel, SGSelState curSel, System.Type selProcT, SGActState prevAct, SGActState curAct, System.Type actProcT, bool isRunning){
				ASGSelState(sg, prevSel, curSel, selProcT);
				ASGActState(sg, prevAct, curAct, actProcT, isRunning);
			}
			public void ASGReset(SlotGroup sg){
				ASGActState(sg, null, SGWFA, null, false);
				AE(sg.allTASBs, null);
				AE(sg.newSBs, null);
				AE(sg.newSlots, null);
			}
		/*	SB	*/
			public void ASSB(Slottable sb,
			SBSelState prevSel, SBSelState curSel , System.Type selProcT,
			SBActState prevAct, SBActState curAct, System.Type actProcT, bool isRunning,
			SBEqpState prevEqp, SBEqpState curEqp, System.Type eqpProcT){
				ASBSelState(sb, prevSel, curSel, selProcT);
				ASBActState(sb, prevAct, curAct, actProcT, isRunning);
				if(curEqp != null)
					ASBEqpState(sb, prevEqp, curEqp, eqpProcT);
			}
			public void ASSB_s(Slottable sb, SBSelState selState, SBActState actState){
				AE(sb.curSelState, selState);
				AE(sb.curActState, actState);
			}
			public void ASBSelState(Slottable sb, SBSelState prev, SBSelState cur, System.Type procT){
				if(prev != null){
					AE(sb.prevSelState, prev);
					if(procT != null)
						AE(sb.selProcess.GetType(), procT);
					else
						ANull(sb.selProcess);
				}
				AE(sb.curSelState, cur);
			}
			public void ASBActState(Slottable sb, SBActState prev, SBActState cur, System.Type procT, bool isRunning){
				if(prev != null){
					AE(sb.prevActState, prev);
					if(procT != null){
						AE(sb.actProcess.GetType(), procT);
						AE(sb.actProcess.isRunning, isRunning);
					}
					else
						ANull(sb.actProcess);
				}
				AE(sb.curActState, cur);
			}
			public void ASBEqpState(Slottable sb, SBEqpState prev, SBEqpState cur, System.Type procT){
				if(prev != null){
					AE(sb.PrevEqpState, prev);
					if(procT != null)
						AE(sb.eqpProcess.GetType(), procT);
					else
						ANull(sb.eqpProcess);
				}
				AE(sb.CurEqpState, cur);
			}
			public void ASBReset(Slottable sb){
				ASBActState(sb, null, SBWFA, null, false);
				AE(sb.pickedAmount, 0);
				AE(sb.newSlotID, -2);
			}
		/*	other	*/
			public void ANotNull(object obj){
				// Assert.That(obj, Is.Not.Null);
				AB(obj != null, true);
			}
			public void ANull(object obj){
				AB(obj == null, true);
			}
			public void AE(object inspected, object expected){
				if(expected != null)
					Assert.That(inspected, Is.EqualTo(expected));
				else
					ANull(inspected);
			}
			public void AB(bool inspectedBool, bool value){
				if(value)
					Assert.That(inspectedBool, Is.True);
				else
					Assert.That(inspectedBool, Is.False);
			}
			public void AssertEquipped(InventoryItemInstanceMock itemInst){
				if(itemInst is BowInstanceMock || itemInst is WearInstanceMock){
					/*	Bow or Wear?	*/
					bool isBow = false;
					if(itemInst is BowInstanceMock)
						isBow = true;
					System.Type typeToCheck;
					InventoryItemInstanceMock ssmEquipped;
					Slottable sbe;
					SlotGroup sge;
					SlotGroup sgp;
					if(isBow){
						typeToCheck = typeof(BowInstanceMock);
						ssmEquipped = ssm.equippedBowInst;
						sge = sgeBow;
						sgp = sgpBow;
					}else{
						typeToCheck = typeof(WearInstanceMock);
						ssmEquipped = ssm.equippedWearInst;
						sge = sgeWear;
						sgp = sgpWear;
					}
					foreach(Slottable sbp in sgpAll){
						if(sbp != null){
							InventoryItemInstanceMock sbpItem = sbp.itemInst;
							if(sbpItem.GetType() == typeToCheck)
							{
								if(sbpItem == itemInst){
									AB(sbp.isEquipped, true);
									AB(sgpAll.equippedSBs.Contains(sbp), true);
									AE(ssmEquipped, sbpItem);
									sbe = sge.GetSB(sbpItem);
									AB(sbe != null, true);
									AE(sge.equippedSBs.Count, 1);
									AB(sge.equippedSBs.Contains(sbe), true);
									ASSB(sbe,
										null, SBFocused, null,
										null, SBWFA, null, false, 
										null, SBEquipped, null);
									if(sgpAll == ssm.focusedSGP){
										if(sgpAll.isAutoSort){
											ASSB(sbp,
												null, SBDefocused, null,
												null, SBWFA, null, false,
												null, SBEquipped, null);
										}else{
											ASSB(sbp,
												null, SBFocused, null,
												null, SBWFA, null, false, 
												null, SBEquipped, null);
										}
									}else{
										ASSB(sbp,
											null, SBDefocused, null,
											null, SBWFA, null, false, 
											null, SBEquipped, null);
									}
								}else{// deemed not the equipped bow/wear
									AB(sbp.isEquipped, false);
									AB(sgpAll.equippedSBs.Contains(sbp), false);
									AB(ssmEquipped != sbpItem, true);
									ANull(sge.GetSB(sbpItem));
									if(sgpAll == ssm.focusedSGP){
										ASSB(sbp,
											null, SBFocused, null,
											null, SBWFA, null, false, 
											null, SBUnequipped, null);
									}else{
										ASSB(sbp,
											null, SBDefocused, null,
											null, SBWFA, null, false, 
											null, SBUnequipped,null);
									}
								}
							}
						}
					}
					foreach(Slottable sbp in sgp){
						if(sbp != null){
							InventoryItemInstanceMock sbpItem = sbp.itemInst;
							if(sbpItem == itemInst){
								AB(sbp.isEquipped, true);
								AB(sgp.equippedSBs.Contains(sbp), true);
								AE(ssmEquipped, sbpItem);
								sbe = sge.GetSB(sbpItem);
								AB(sbe != null, true);
								AE(sge.equippedSBs.Count, 1);
								AB(sge.equippedSBs.Contains(sbe), true);
								if(sgp == ssm.focusedSGP){
									if(sgp.isAutoSort)
										ASSB(sbp,
											null, SBDefocused, null,
											null, SBWFA, null, false,
											null, SBEquipped, null);
									else
										ASSB(sbp,
											null, SBFocused, null,
											null, SBWFA, null, false,
											null, SBEquipped, null);
								}else{
									ASSB(sbp,
										null, SBDefocused, null,
										null, SBWFA, null, false,
										null, SBEquipped, null);
								}
							}else{/* deemed not equipped	*/
								AB(sbp.isEquipped, false);
									AB(sgp.equippedSBs.Contains(sbp), false);
									AB(ssmEquipped != sbpItem, true);
									ANull(sge.GetSB(sbpItem));
								if(sgp == ssm.focusedSGP)
									ASSB(sbp,
										null, SBFocused, null,
										null, SBWFA, null, false,
										null, SBUnequipped, null);
								else
									ASSB(sbp,
										null, SBDefocused, null,
										null, SBWFA, null, false,
										null, SBUnequipped, null);
							}
						}
					}
				}
			}
			public void AECGears(List<CarriedGearInstanceMock> items, PoolInventory poolInv, EquipmentSetInventory equipInv){
				if(items != null){
					AE(ssm.equippedCarriedGears.Count, items.Count);
					AE(sgpAll.equippedSBs.Count, items.Count + 2);
					AE(sgpCGears.equippedSBs.Count, items.Count);
					AE(sgeCGears.equippedSBs.Count, items.Count);
					foreach(CarriedGearInstanceMock item in items){
						AB(ssm.equippedCarriedGears.Contains(item), true);
						AB(sgpAll.equippedSBs.Contains(sgpAll.GetSB(item)), true);
						ANull(sgpBow.GetSB(item));
						ANull(sgpWear.GetSB(item));
						ANull(sgpParts.GetSB(item));
						AB(sgpCGears.equippedSBs.Contains(sgpCGears.GetSB(item)), true);
						ANull(sgeBow.GetSB(item));
						ANull(sgeWear.GetSB(item));
						AB(sgeCGears.equippedSBs.Contains(sgeCGears.GetSB(item)), true);
					}
				}
				foreach(InventoryItemInstanceMock itemInInv in poolInv){
					if(itemInInv is CarriedGearInstanceMock){
						if(items.Contains((CarriedGearInstanceMock)itemInInv)){
							AB(itemInInv.isEquipped, true);
							AB(sgpAll.GetSB(itemInInv).isEquipped, true);
							AB(sgpCGears.GetSB(itemInInv).isEquipped, true);
						}else{//deemed not equipped
							AB(itemInInv.isEquipped, false);
							AB(sgpAll.GetSB(itemInInv).isEquipped, false);
							AB(sgpCGears.GetSB(itemInInv).isEquipped, false);
						}
					}
				}
				foreach(CarriedGearInstanceMock item in items){
					if(item != null){
						AE(equipInv.Contains(item), true);
					}
				}
				foreach(InventoryItemInstanceMock itemInInv in equipInv){
					if(itemInInv is CarriedGearInstanceMock){
						AB(itemInInv.isEquipped, true);
						AE(items.Contains((CarriedGearInstanceMock)itemInInv), true);
						AB(sgeCGears.GetSB(itemInInv).isEquipped, true);
					}
				}
			}
			public void AECGears(Slottable cg1, Slottable cg2, Slottable cg3, Slottable cg4){
				List<CarriedGearInstanceMock> checkedList = new List<CarriedGearInstanceMock>();
				if(cg1 != null)
					checkedList.Add((CarriedGearInstanceMock)cg1.item);
				else
					checkedList.Add(null);
				if(cg2 != null)
					checkedList.Add((CarriedGearInstanceMock)cg2.item);
				else
					checkedList.Add(null);
				if(cg3 != null)
					checkedList.Add((CarriedGearInstanceMock)cg3.item);
				else
					checkedList.Add(null);
				if(cg4 != null)
					checkedList.Add((CarriedGearInstanceMock)cg4.item);
				else
					checkedList.Add(null);
				
				int allowedCount = ((EquipmentSetInventory)sgeCGears.inventory).equippableCGearsCount;
				
				for(int i = 0; i < 4; i++){
					if(i +1 > allowedCount)
						if(checkedList[i] != null)
							throw new System.InvalidOperationException("Slottable at index " + i + " is not checked since it exceeds the max slot count");
				}
				for(int i = 0; i < allowedCount; i++){
					Slottable sb = sgeCGears.slots[i].sb;
					if(sb != null)
						AE(sb.item, checkedList[i]);
					else
						Assert.That(checkedList[i], Is.Null);
				}
				foreach(SlotGroup sgp in ssm.allSGPs){
					if(sgp.Filter is SGNullFilter || sgp.Filter is SGCGearsFilter){
						foreach(Slottable sbp in sgp){
							if(sbp != null){
								if(sbp.itemInst is CarriedGearInstanceMock){
									if(checkedList.Contains((CarriedGearInstanceMock)sbp.itemInst)){
										if(sgp == ssm.focusedSGP){
											if(sgp.isAutoSort){
												ASSB(sbp,
													null, SBDefocused, null,
													null, SBWFA, null, false,
													SBUnequipped, SBEquipped, typeof(SBEquipProcess));
											}else{
												ASSB(sbp,
													null, SBFocused, null,
													null, SBWFA, null, false,
													SBUnequipped, SBEquipped, typeof(SBEquipProcess));	
											}
										}else{
											ASSB(sbp,
												null, SBDefocused, null,
												null, SBWFA, null, false,
												SBUnequipped, SBEquipped, typeof(SBEquipProcess));	
										}
									}else{	/* deemed not equipped */
										if(sgp == ssm.focusedSGP){
											ASSB(sbp,
												null, SBFocused, null,
												null, SBWFA, null, false,
												null, SBUnequipped, null);
										}else{
											ASSB(sbp,
												null, SBDefocused, null,
												null, SBWFA, null, false,
												null, SBUnequipped, null);
										}
									}
								}
							}
						}
					}
				}
			}
			public void AssertFocused(){
				AssertEquippedOnAll();
				ASSSM(ssm,
					null, null, null, null, null, null, null, null,
					null, SSMFocused, null,
					null, SSMWFA, null,
					null, true, true, true, true);
				foreach(SlotGroup sgp in ssm.allSGPs){
					ASGReset(sgp);
					if(sgp == ssm.focusedSGP){
						ASSG(sgp,
							null, SGFocused, null,
							null, SGWFA, null, false);
						foreach(Slottable sb in sgp){
							if(sb != null){
								AE(sb.ssm, ssm);
								ASBReset(sb);
								if(!sgp.isAutoSort){
										ASSB_s(sb, SBFocused, SBWFA);
								}else{
									if(sb.item is PartsInstanceMock && !(sgp.Filter is SGPartsFilter))
										ASSB_s(sb, SBDefocused, SBWFA);
									else
										if(sb.isEquipped)
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
							null, SGWFA, null, false);
						foreach(Slottable sb in sgp){
							if(sb != null){
								AE(sb.ssm, ssm);
								ASBReset(sb);
								ASSB_s(sb, SBDefocused, SBWFA);
							}
						}
					}
				}
				foreach(SlotGroup sge in ssm.allSGEs){
					ASGReset(sge);
					if(ssm.focusedSGEs.Contains(sge)){
						ASSG(sge,
							null, SGFocused, null,
							null, SGWFA, null, false);
						foreach(Slottable sbe in sge){
							if(sbe != null){
								AE(sbe.ssm, ssm);
								ASBReset(sbe);
								ASSB_s(sbe, SBFocused, SBWFA);
							}
						}
					}else{
						ASSG(sge,
							null, SGFocused, null,
							null, SGWFA, null, false);
						foreach(Slottable sbe in sge){
							if(sbe != null){
								AE(sbe.ssm, ssm);
								ASBReset(sbe);
								ASSB_s(sbe, SBDefocused, SBWFA);
							}
						}
					}
				}
			}
	/*	shortcut	*/
		/*	Debug	*/
			public void PrintSBsArray(IEnumerable<Slottable> sbs){
				foreach(Slottable sb in sbs){
					if(sb != null)
						Util.Stack(Util.SBName(sb));
					else
						Util.Stack("null");
				}
				string str = Util.Stacked;
				Debug.Log("SBs: " + str);
			}
			public void PrintItemsArray(IEnumerable<SlottableItem> items){
				foreach(SlottableItem item in items){
					if(item is InventoryItemInstanceMock){
						InventoryItemInstanceMock itemInst = (InventoryItemInstanceMock)item;
						if(itemInst != null)
							Util.Stack(Util.ItemInstName(itemInst));
						else
							Util.Stack("null");
					}
				}
				string str = Util.Stacked;
				Debug.Log("itemInsts: " + str);
			}
			public void PrintSBs(List<Slottable> sbs){
				foreach(Slottable sb in sbs){
					Debug.Log(Util.SBDebug(sb));
				}
			}
			public void Print(Slottable sb){
				Debug.Log(Util.SBDebug(sb));
			}
			public void Print(SlotGroup sg){
				Debug.Log(Util.SGDebug(sg));
			}
			public void Print(SlotSystemManager ssm){
				Debug.Log(Util.SSMDebug(ssm));
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

		/*	SSM	*/
			SSMSelState SSMDeactivated{
				get{return SlotSystemManager.ssmDeactivatedState;}
			}
			SSMSelState SSMDefocused{
				get{return SlotSystemManager.ssmDefocusedState;}
			}
			SSMSelState SSMFocused{
				get{return SlotSystemManager.ssmFocusedState;}
			}
			SSMActState SSMWFA{
				get{return SlotSystemManager.ssmWaitForActionState;}
			}
			SSMActState SSMProbing{
				get{return SlotSystemManager.ssmProbingState;}
			}
			SSMActState SSMTransaction{
				get{return SlotSystemManager.ssmTransactionState;}
			}
		/*	SG	*/
			SGSelState SGFocused{
				get{return SlotGroup.sgFocusedState;}
			}
			SGSelState SGDefocused{
				get{return SlotGroup.sgDefocusedState;}
			}
			SGSelState SGDeactivated{
				get{return SlotGroup.sgDeactivatedState;}
			}
			SGSelState SGSelected{
				get{return SlotGroup.sgSelectedState;}
			}
			SGActState SGWFA{
				get{return SlotGroup.sgWaitForActionState;}
			}
			SGActState SGRevert{
				get{return SlotGroup.revertState;}
			}
			SGActState SGReorder{
				get{return SlotGroup.reorderState;}
			}
			SGActState SGFill{
				get{return SlotGroup.fillState;}
			}
			SGActState SGSwap{
				get{return SlotGroup.swapState;}
			}
			SGActState SGAdd{
				get{return SlotGroup.addState;}
			}
			SGActState SGRemove{
				get{return SlotGroup.removeState;}
			}
		/*	SB states shortcut	*/
			SBSelState SBFocused{
				get{return Slottable.sbFocusedState;}
			}
			SBSelState SBDefocused{
				get{return Slottable.sbDefocusedState;}
			}
			SBSelState SBDeactivated{
				get{return Slottable.sbDeactivatedState;}
			}
			SBSelState SBSelected{
				get{return Slottable.sbSelectedState;}
			}
			SBActState SBWFA{
				get{return Slottable.sbWaitForActionState;}
			}
			SBActState SBWFPickUp{
				get{return Slottable.waitForPickUpState;}
			}
			SBActState SBWFPointerUp{
				get{return Slottable.waitForPointerUpState;}
			}
			SBActState SBPickedUp{
				get{return Slottable.pickedUpState;}
			}
			SBActState SBWFNT{
				get{return Slottable.waitForNextTouchState;}
			}
			SBActState SBAdd{
				get{return Slottable.addedState;}
			}
			SBActState SBRemove{
				get{return Slottable.removedState;}
			}
			SBActState SBMoveWithin{
				get{return Slottable.moveWithinState;}
			}
			SBEqpState SBEquipped{
				get{return Slottable.equippedState;}
			}
			SBEqpState SBUnequipped{
				get{return Slottable.unequippedState;}
			}

}
