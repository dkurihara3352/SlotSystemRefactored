﻿using UnityEngine;
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
				BowInstance stubBowInst_A = MakeBowInstance(0);
				BowInstance stubBowInst_B = MakeBowInstance(0);
				ISlottable stubSB_A = MakeSubSB();
				ISlottable stubSB_B = MakeSubSB();
				ISlotGroup stubSG_A = MakeSubSG();
				ISlotGroup stubSG_B = MakeSubSG();
				stubSB_A.sg.Returns(stubSG_A);
				stubSB_B.sg.Returns(stubSG_B);
				stubSB_A.itemInst.Returns(stubBowInst_A);
				stubSB_B.itemInst.Returns(stubBowInst_B);
				stubSG_A.AcceptsFilter(stubSB_B).Returns(true);
				stubSG_B.AcceptsFilter(stubSB_A).Returns(true);
				stubSG_A.Filter.Returns(new SGBowFilter());
				stubSG_B.Filter.Returns(new SGBowFilter());
				
				Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.True);
			}
			[Test]
			public void AreSwappable_SameSGsAndMutuallyAcceptingAndNotStackable_ReturnsFalse(){
				BowInstance stubBowInst_A = MakeBowInstance(0);
				BowInstance stubBowInst_B = MakeBowInstance(0);
				ISlottable stubSB_A = MakeSubSB();
				ISlottable stubSB_B = MakeSubSB();
				ISlotGroup stubSG = MakeSubSG();
				stubSB_A.sg.Returns(stubSG);
				stubSB_B.sg.Returns(stubSG);
				stubSB_A.itemInst.Returns(stubBowInst_A);
				stubSB_B.itemInst.Returns(stubBowInst_B);
				stubSG.AcceptsFilter(stubSB_B).Returns(true);
				stubSG.AcceptsFilter(stubSB_A).Returns(true);
				stubSG.Filter.Returns(new SGBowFilter());
				stubSG.Filter.Returns(new SGBowFilter());
				
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
				stubSB_A.sg.Returns(stubSG_A);
				stubSB_B.sg.Returns(stubSG_B);
				stubSB_A.itemInst.Returns(stubBowInst_A);
				stubSB_B.itemInst.Returns(stubBowInst_B);
				stubSG_A.AcceptsFilter(stubSB_B).Returns(false);
				stubSG_B.AcceptsFilter(stubSB_A).Returns(false);
				stubSG_A.Filter.Returns(new SGBowFilter());
				stubSG_B.Filter.Returns(new SGBowFilter());
				
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
				stubSB_A.sg.Returns(stubSG_A);
				stubSB_B.sg.Returns(stubSG_B);
				stubSB_A.itemInst.Returns(stubParts_A);
				stubSB_B.itemInst.Returns(stubParts_B);
				stubSG_A.AcceptsFilter(stubSB_B).Returns(false);
				stubSG_B.AcceptsFilter(stubSB_A).Returns(false);
				stubSG_A.Filter.Returns(new SGBowFilter());
				stubSG_B.Filter.Returns(new SGBowFilter());
				
				Assert.That(SlotSystemUtil.AreSwappable(stubSB_A, stubSB_B), Is.False);
			}
		}
	}
}