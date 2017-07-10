using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using NSubstitute;

namespace SlotSystemTests{
	namespace OtherClassesTests{
		[TestFixture]
		public class SlotSystemUtilTests: AbsSlotSystemTest{

			[Test]
			public void AreSwappable_DifferentSGsAndMutuallyAcceptingAndNotStackable_ReturnsTrue(){
				TestSSM stubSSM = MakeFakeSSM();
				BowFake bowFake = new BowFake();
				BowInstance stubBowInst_A = new BowInstance();
				BowInstance stubBowInst_B = new BowInstance();
				stubBowInst_A.Item = bowFake;
				stubBowInst_B.Item = bowFake;
				Slottable stubSB_A = MakeSB(stubSSM, stubBowInst_A);
				Slottable stubSB_B = MakeSB(stubSSM, stubBowInst_B);
				SlotGroup stubSG_A = MakeSG(new SGBowFilter());
				SlotGroup stubSG_B = MakeSG(new SGBowFilter());
				stubSSM.AddParentChild(stubSB_A, stubSG_A);
				stubSSM.AddParentChild(stubSB_B, stubSG_B);

				Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.True);
			}
			[Test]
			public void AreSwappable_SameSGsAndMutuallyAcceptingAndNotStackable_ReturnsFalse(){
				TestSSM stubSSM = MakeFakeSSM();
				BowFake bowFake = new BowFake();
				BowInstance stubBowInst_A = new BowInstance();
				BowInstance stubBowInst_B = new BowInstance();
				stubBowInst_A.Item = bowFake;
				stubBowInst_B.Item = bowFake;
				Slottable stubSB_A = MakeSB(stubSSM, stubBowInst_A);
				Slottable stubSB_B = MakeSB(stubSSM, stubBowInst_B);
				SlotGroup stubSG_A = MakeSG(new SGBowFilter());
				stubSSM.AddParentChild(stubSB_A, stubSG_A);
				stubSSM.AddParentChild(stubSB_B, stubSG_A);

				Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.False);
			}
			[Test]
			public void AreSwappable_DiffSGsAndMutuallyNOTAcceptingAndNOTStackable_ReturnsFalse(){
				TestSSM stubSSM = MakeFakeSSM();
				BowFake bowFake = new BowFake();
				BowInstance stubBowInst = new BowInstance();
				stubBowInst.Item = bowFake;
				WearFake wearFake = new WearFake();
				WearInstance stubWearInst = new WearInstance();
				stubWearInst.Item = wearFake;
				Slottable stubSB_A = MakeSB(stubSSM, stubBowInst);
				Slottable stubSB_B = MakeSB(stubSSM, stubWearInst);
				SlotGroup stubSG_A = MakeSG(new SGBowFilter());
				SlotGroup stubSG_B = MakeSG(new SGWearFilter());
				stubSSM.AddParentChild(stubSB_A, stubSG_A);
				stubSSM.AddParentChild(stubSB_B, stubSG_B);

				Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.False);
			}
			[Test]
			public void AreSwappable_DiffSGsAndMutuallyNOTAcceptingAndISStackable_ReturnsFalse(){
				TestSSM stubSSM = MakeFakeSSM();
				PartsFake partsFake = new PartsFake();
				PartsInstance stubPartsInst_A = new PartsInstance();
				PartsInstance stubPartsInst_B = new PartsInstance();
				stubPartsInst_A.Item = partsFake;
				stubPartsInst_B.Item = partsFake;
				Slottable stubSB_A = MakeSB(stubSSM, stubPartsInst_A);
				Slottable stubSB_B = MakeSB(stubSSM, stubPartsInst_B);
				SlotGroup stubSG_A = MakeSG(new SGPartsFilter());
				SlotGroup stubSG_B = MakeSG(new SGPartsFilter());
				stubSSM.AddParentChild(stubSB_A, stubSG_A);
				stubSSM.AddParentChild(stubSB_B, stubSG_B);

				Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.False);
			}
			Slottable MakeSB(SlotSystemManager ssm, InventoryItemInstance itemInst){
				GameObject sbGO = new GameObject("sbGO");
				Slottable sb = sbGO.AddComponent<Slottable>();
				sb.Initialize(itemInst);
				sb.SetSSM(ssm);
				return sb;
			}
			SlotGroup MakeSG(SGFilter filter){
				GameObject sgGO = new GameObject("sgGO");
				SlotGroup sg = sgGO.AddComponent<SlotGroup>();
				sg.Initialize("sg", filter, new PoolInventory(), false, 0, new SGEmptyCommand(), new SGEmptyCommand());
				return sg;
			}
		}
	}
}
