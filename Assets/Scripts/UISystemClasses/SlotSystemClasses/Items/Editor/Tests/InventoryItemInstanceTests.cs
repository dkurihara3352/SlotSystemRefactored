using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UISystem;

namespace SlotSystemTests{
	namespace OtherClassesTests{
		[TestFixture]
		[Category("Other")]
		public class InventoryItemInstanceTests: SlotSystemTest {

			[Test]
			public void Equals_ToSameStackableItem_ReturnsTrue(){
				PartsInstance partsInstA = MakePartsInstance(0, 1);
				PartsInstance partsInstB = MakePartsInstance(0, 1);
				bool equality = partsInstA == partsInstB;

				Assert.That(equality, Is.True);
				}
			[Test]
			public void objectReferenceEquals_ToSameStackableItem_ReturnsFalse(){
				PartsInstance partsInstA = MakePartsInstance(0, 1);
				PartsInstance partsInstB = MakePartsInstance(0, 1);
				bool equality = object.ReferenceEquals(partsInstA, partsInstB);

				Assert.That(equality, Is.False);
				}
			[Test]
			public void Equals_ToDifferentStackableItem_ReturnsFalse(){
				PartsInstance partsInstA = MakePartsInstance(0, 1);
				PartsInstance partsInstB = MakePartsInstance(1, 1);
				bool equality = partsInstA == partsInstB;

				Assert.That(equality, Is.False);
				}
			[Test]
			public void Equals_ToSameNonStackableItem_ReturnsFalse(){
				BowInstance stubBowInstA = MakeBowInstance(0);
				BowInstance stubBowInstB = MakeBowInstance(0);
				bool equality = stubBowInstA == stubBowInstB;

				Assert.That(equality, Is.False);
				}
			[Test]
			public void Equals_ToSelf_ReturnsTrue(){
				BowInstance stubBowInstA = MakeBowInstance(0);
				BowInstance stubBowInstB = stubBowInstA;
				bool equality = stubBowInstA == stubBowInstB;

				Assert.That(equality, Is.True);
				}
		}
	}
}
