using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UISystem;
using NSubstitute;

namespace SlotSystemTests{
	namespace OtherClassesTests{
		[TestFixture]
		public class SlotSystemUtilTests: SlotSystemTest{
			[Test]
			public void SBsAreSwappable_SameSG_Returns_False(){
				ISlotGroup sharedSG = MakeSubSG();
				ISlottable pickedSB = MakeSubSB();
					pickedSB.SlotGroup().Returns(sharedSG);
				ISlottable otherSB = MakeSubSB();
					otherSB.SlotGroup().Returns(sharedSG);
				bool actual = SlotSystemUtil.SBsAreSwappable(pickedSB, otherSB);

				Assert.That(actual, Is.Not.False);
			}
			[Test]
			public void SBsAreSwappable_DifferentSG_EitherItemIsStackable_Returns_False(){
				ISlottable pickedSB = MakeSubSB();
					ISlotGroup pickedSG = MakeSubSG();
					pickedSB.SlotGroup().Returns(pickedSG);
					ISlottableItem pickedItem = Substitute.For<ISlottableItem>();
						pickedItem.IsStackable().Returns(false);
					pickedSB.Item().Returns(pickedItem);
				ISlottable otherSB = MakeSubSB();
					ISlotGroup otherSG = MakeSubSG();
					otherSB.SlotGroup().Returns(otherSG);
					ISlottableItem otherItem = Substitute.For<ISlottableItem>();
						otherItem.IsStackable().Returns(true);
					otherSB.Item().Returns(otherItem);
				
				bool actual = SlotSystemUtil.SBsAreSwappable(pickedSB, otherSB);

				Assert.That(actual, Is.Not.False);
			}
			[Test]
			public void SBsAreSwappable_DifferentSG_BothItemsAreNotStackable_NotMutuallyAccepting_Returns_False(){
				ISlottable pickedSB = MakeSubSB();
					ISlotGroup pickedSG = MakeSubSG();
					pickedSB.SlotGroup().Returns(pickedSG);
					ISlottableItem pickedItem = Substitute.For<ISlottableItem>();
						pickedItem.IsStackable().Returns(false);
					pickedSB.Item().Returns(pickedItem);
				ISlottable otherSB = MakeSubSB();
					ISlotGroup otherSG = MakeSubSG();
					otherSB.SlotGroup().Returns(otherSG);
					ISlottableItem otherItem = Substitute.For<ISlottableItem>();
						otherItem.IsStackable().Returns(false);
					otherSB.Item().Returns(otherItem);
				pickedSG.AcceptsItem(otherItem).Returns(true);
				otherSG.AcceptsItem(pickedItem).Returns(false);
				
				bool actual = SlotSystemUtil.SBsAreSwappable(pickedSB, otherSB);

				Assert.That(actual, Is.Not.False);
			}
			[Test]
			public void SBsAreSwappable_DifferentSG_BothItemsAreNotStackable_MutuallyAccepting_Returns_True(){
				ISlottable pickedSB = MakeSubSB();
					ISlotGroup pickedSG = MakeSubSG();
					pickedSB.SlotGroup().Returns(pickedSG);
					ISlottableItem pickedItem = Substitute.For<ISlottableItem>();
						pickedItem.IsStackable().Returns(false);
					pickedSB.Item().Returns(pickedItem);
				ISlottable otherSB = MakeSubSB();
					ISlotGroup otherSG = MakeSubSG();
					otherSB.SlotGroup().Returns(otherSG);
					ISlottableItem otherItem = Substitute.For<ISlottableItem>();
						otherItem.IsStackable().Returns(false);
					otherSB.Item().Returns(otherItem);
				pickedSG.AcceptsItem(otherItem).Returns(true);
				otherSG.AcceptsItem(pickedItem).Returns(true);
				
				bool actual = SlotSystemUtil.SBsAreSwappable(pickedSB, otherSB);

				Assert.That(actual, Is.Not.True);
			}
		}
	}
}
