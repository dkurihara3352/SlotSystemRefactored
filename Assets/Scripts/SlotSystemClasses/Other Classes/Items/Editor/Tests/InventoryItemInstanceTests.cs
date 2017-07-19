﻿using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;

namespace SlotSystemTests{
	namespace OtherClassesTests{
		[TestFixture]
		public class InventoryItemInstanceTests: AbsSlotSystemTest {

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
			[Test]
			public void CompareTo_IIWithGreaterID_ReturnsNegative(){
				BowInstance stubBowInstA = MakeBowInstance(0);
				BowInstance stubBowInstB = MakeBowInstance(1);

				int result = stubBowInstA.CompareTo(stubBowInstB);

				Assert.That(result, Is.LessThan(0));
			}
			[Test]
			public void CompareTo_IIWithLesserID_ReturnsPositive(){
				BowInstance stubBowInstA = MakeBowInstance(1);
				BowInstance stubBowInstB = MakeBowInstance(0);

				int result = stubBowInstA.CompareTo(stubBowInstB);

				Assert.That(result, Is.GreaterThan(0));
			}
			[Test]
			public void CompareTo_IIWithSameIDAndOrder_ReturnsZero(){
				BowInstance stubBowInstA = MakeBowInstance(0);
				BowInstance stubBowInstB = MakeBowInstance(0);
				stubBowInstA.SetAcquisitionOrder(0);
				stubBowInstB.SetAcquisitionOrder(0);

				int result = stubBowInstA.CompareTo(stubBowInstB);

				Assert.That(result, Is.EqualTo(0));
			}
			[Test]
			public void CompareTo_IIWithSameIDAndGreaterOrder_ReturnsNegative(){
				BowInstance stubBowInstA = MakeBowInstance(0);
				BowInstance stubBowInstB = MakeBowInstance(0);
				stubBowInstA.SetAcquisitionOrder(0);
				stubBowInstB.SetAcquisitionOrder(1);

				int result = stubBowInstA.CompareTo(stubBowInstB);

				Assert.That(result, Is.LessThan(0));
			}
			[Test]
			public void CompareTo_IIWithSameIDAndLesserOrder_ReturnsPositive(){
				BowInstance stubBowInstA = MakeBowInstance(0);
				BowInstance stubBowInstB = MakeBowInstance(0);
				stubBowInstA.SetAcquisitionOrder(1);
				stubBowInstB.SetAcquisitionOrder(0);

				int result = stubBowInstA.CompareTo(stubBowInstB);

				Assert.That(result, Is.GreaterThan(0));
			}
		}
	}
}