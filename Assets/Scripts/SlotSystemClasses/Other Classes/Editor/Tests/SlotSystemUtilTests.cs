using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using NSubstitute;

namespace SlotSystemTests{
	namespace OtherClassesTests{
		[TestFixture]
		public class SlotSystemUtilTests: SlotSystemTest{

			[Test]
			public void AreSwappable_DifferentSGsAndMutuallyAcceptingAndNotStackable_ReturnsTrue(){
				BowInstance stubBowInst_A = MakeBowInstance(0);
				BowInstance stubBowInst_B = MakeBowInstance(0);
				ISlottable stubSB_A = MakeSubSB();
				ISlottable stubSB_B = MakeSubSB();
				ISlotGroup stubSG_A = MakeSubSG();
				ISlotGroup stubSG_B = MakeSubSG();
				IFilterHandler stubSG_AFilterHandler = Substitute.For<IFilterHandler>();
				IFilterHandler stubSG_BFilterHandler = Substitute.For<IFilterHandler>();
				stubSG_A.GetFilterHandler().Returns(stubSG_AFilterHandler);
				stubSG_B.GetFilterHandler().Returns(stubSG_BFilterHandler);
				stubSB_A.GetSG().Returns(stubSG_A);
				stubSB_B.GetSG().Returns(stubSG_B);
				stubSB_A.GetItem().Returns(stubBowInst_A);
				stubSB_B.GetItem().Returns(stubBowInst_B);
				stubSG_AFilterHandler.AcceptsFilter(stubSB_B).Returns(true);
				stubSG_BFilterHandler.AcceptsFilter(stubSB_A).Returns(true);
				stubSG_AFilterHandler.GetFilter().Returns(new SGBowFilter());
				stubSG_BFilterHandler.GetFilter().Returns(new SGBowFilter());
				
				Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.True);
			}
			[Test]
			public void AreSwappable_SameSGsAndMutuallyAcceptingAndNotStackable_ReturnsFalse(){
				BowInstance stubBowInst_A = MakeBowInstance(0);
				BowInstance stubBowInst_B = MakeBowInstance(0);
				ISlottable stubSB_A = MakeSubSB();
				ISlottable stubSB_B = MakeSubSB();
				ISlotGroup stubSG = MakeSubSG();
				IFilterHandler stubSGFilterHandler = Substitute.For<IFilterHandler>();
				stubSG.GetFilterHandler().Returns(stubSGFilterHandler);
				stubSB_A.GetSG().Returns(stubSG);
				stubSB_B.GetSG().Returns(stubSG);
				stubSB_A.GetItem().Returns(stubBowInst_A);
				stubSB_B.GetItem().Returns(stubBowInst_B);
				stubSGFilterHandler.AcceptsFilter(stubSB_B).Returns(true);
				stubSGFilterHandler.AcceptsFilter(stubSB_A).Returns(true);
				stubSGFilterHandler.GetFilter().Returns(new SGBowFilter());
				stubSGFilterHandler.GetFilter().Returns(new SGBowFilter());
				
				Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.False);
				}
			[Test]
			public void AreSwappable_DiffSGsAndMutuallyNOTAcceptingAndNOTStackable_ReturnsFalse(){
				BowInstance stubBowInst_A = MakeBowInstance(0);
				BowInstance stubBowInst_B = MakeBowInstance(0);
				ISlottable stubSB_A = MakeSubSB();
				ISlottable stubSB_B = MakeSubSB();
				ISlotGroup stubSG_A = MakeSubSG();
				ISlotGroup stubSG_B = MakeSubSG();
				IFilterHandler stubSG_AFilterHandler = Substitute.For<IFilterHandler>();
				IFilterHandler stubSG_BFilterHandler = Substitute.For<IFilterHandler>();
				stubSG_A.GetFilterHandler().Returns(stubSG_AFilterHandler);
				stubSG_B.GetFilterHandler().Returns(stubSG_BFilterHandler);
				stubSB_A.GetSG().Returns(stubSG_A);
				stubSB_B.GetSG().Returns(stubSG_B);
				stubSB_A.GetItem().Returns(stubBowInst_A);
				stubSB_B.GetItem().Returns(stubBowInst_B);
				stubSG_AFilterHandler.AcceptsFilter(stubSB_B).Returns(false);
				stubSG_BFilterHandler.AcceptsFilter(stubSB_A).Returns(false);
				stubSG_AFilterHandler.GetFilter().Returns(new SGBowFilter());
				stubSG_BFilterHandler.GetFilter().Returns(new SGBowFilter());
				
				Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.False);
				}
			[Test]
			public void AreSwappable_DiffSGsAndMutuallyNOTAcceptingAndISStackable_ReturnsFalse(){
				PartsInstance stubParts_A = MakePartsInstance(0, 1);
				PartsInstance stubParts_B = MakePartsInstance(0, 1);
				ISlottable stubSB_A = MakeSubSB();
				ISlottable stubSB_B = MakeSubSB();
				ISlotGroup stubSG_A = MakeSubSG();
				ISlotGroup stubSG_B = MakeSubSG();
				IFilterHandler stubSG_AFilterHandler = Substitute.For<IFilterHandler>();
				IFilterHandler stubSG_BFilterHandler = Substitute.For<IFilterHandler>();
				stubSG_A.GetFilterHandler().Returns(stubSG_AFilterHandler);
				stubSG_B.GetFilterHandler().Returns(stubSG_BFilterHandler);
				stubSB_A.GetSG().Returns(stubSG_A);
				stubSB_B.GetSG().Returns(stubSG_B);
				stubSB_A.GetItem().Returns(stubParts_A);
				stubSB_B.GetItem().Returns(stubParts_B);
				stubSG_AFilterHandler.AcceptsFilter(stubSB_B).Returns(false);
				stubSG_BFilterHandler.AcceptsFilter(stubSB_A).Returns(false);
				stubSG_AFilterHandler.GetFilter().Returns(new SGBowFilter());
				stubSG_BFilterHandler.GetFilter().Returns(new SGBowFilter());
				
				Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.False);
				}
		}
	}
}
