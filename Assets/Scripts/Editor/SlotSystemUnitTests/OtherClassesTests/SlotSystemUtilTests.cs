using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using NSubstitute;

[TestFixture]
public class SlotSystemUtilTests {

	[Test]
	public void AreSwappable_DifferentSGsAndMutuallyAcceptingAndNotStackable_ReturnsTrue(){
		SlotSystemManager stubSSM = Substitute.For<SlotSystemManager>();
		BowFake bowFake = new BowFake();
		BowInstance stubBowInst_A = new BowInstance();
		BowInstance stubBowInst_B = new BowInstance();
		stubBowInst_A.Item = bowFake;
		stubBowInst_B.Item = bowFake;
		Slottable stubSB_A = MakeSB(stubSSM, stubBowInst_A);
		Slottable stubSB_B = MakeSB(stubSSM, stubBowInst_B);
		SlotGroup stubSG_A = MakeSG(new SGBowFilter());
		SlotGroup stubSG_B = MakeSG(new SGBowFilter());
		stubSSM.FindParent(stubSB_A).Returns(stubSG_A);
		stubSSM.FindParent(stubSB_B).Returns(stubSG_B);

		// Assert.That(stubSG_A, Is.Not.SameAs(stubSG_B));
		// Assert.That(stubSG_A == stubSG_B, Is.False);
		// Assert.That(stubSG_A.AcceptsFilter(stubSB_A), Is.True);
		// Assert.That(stubSG_A.AcceptsFilter(stubSB_B), Is.True);
		// Assert.That(stubSG_B.AcceptsFilter(stubSB_A), Is.True);
		// Assert.That(stubSG_B.AcceptsFilter(stubSB_B), Is.True);
		// Assert.That(stubSB_A.isStackable, Is.False);
		// Assert.That(stubSB_B.isStackable, Is.False);

		Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.True);
	}
	[Test]
	public void AreSwappable_SameSGsAndMutuallyAcceptingAndNotStackable_ReturnsFalse(){
		SlotSystemManager stubSSM = Substitute.For<SlotSystemManager>();
		BowFake bowFake = new BowFake();
		BowInstance stubBowInst_A = new BowInstance();
		BowInstance stubBowInst_B = new BowInstance();
		stubBowInst_A.Item = bowFake;
		stubBowInst_B.Item = bowFake;
		Slottable stubSB_A = MakeSB(stubSSM, stubBowInst_A);
		Slottable stubSB_B = MakeSB(stubSSM, stubBowInst_B);
		SlotGroup stubSG_A = MakeSG(new SGBowFilter());
		stubSSM.FindParent(stubSB_A).Returns(stubSG_A);
		stubSSM.FindParent(stubSB_B).Returns(stubSG_A);

		Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.False);
	}
	[Test]
	public void AreSwappable_DiffSGsAndMutuallyNOTAcceptingAndNOTStackable_ReturnsFalse(){
		SlotSystemManager stubSSM = Substitute.For<SlotSystemManager>();
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
		stubSSM.FindParent(stubSB_A).Returns(stubSG_A);
		stubSSM.FindParent(stubSB_B).Returns(stubSG_B);

		Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.False);
	}
	[Test]
	public void AreSwappable_DiffSGsAndMutuallyNOTAcceptingAndISStackable_ReturnsFalse(){
		SlotSystemManager stubSSM = Substitute.For<SlotSystemManager>();
		PartsFake partsFake = new PartsFake();
		PartsInstance stubPartsInst_A = new PartsInstance();
		PartsInstance stubPartsInst_B = new PartsInstance();

		stubPartsInst_A.Item = partsFake;
		stubPartsInst_B.Item = partsFake;
		Slottable stubSB_A = MakeSB(stubSSM, stubPartsInst_A);
		Slottable stubSB_B = MakeSB(stubSSM, stubPartsInst_B);
		SlotGroup stubSG_A = MakeSG(new SGPartsFilter());
		SlotGroup stubSG_B = MakeSG(new SGPartsFilter());
		stubSSM.FindParent(stubSB_A).Returns(stubSG_A);
		stubSSM.FindParent(stubSB_B).Returns(stubSG_B);

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
