using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
namespace SlotSystemTests{
	namespace OtherClassesTests{
		[TestFixture]
		[Category("Other")]
		public class InventoryItemTests{
			[Test]
			public void Equals_ToSelf_ReturnsTrue(){
				BowFake fakeBow = MakeBowFake(0);
				bool equality = fakeBow.Equals(fakeBow);

				Assert.That(equality, Is.True);
				}
			[Test]
			public void Equals_ToAnotherWithSameID_ReturnsTrue(){
				BowFake fakeBow = MakeBowFake(0);
				BowFake stubBow = MakeBowFake(0);
				bool equality = fakeBow.Equals(stubBow);

				Assert.That(equality, Is.True);
				}
			[Test]
			public void Equals_ToAnotherWithDifferentID_ReturnsFalse(){
				BowFake fakeBow = MakeBowFake(0);
				BowFake stubBow = MakeBowFake(1);
				bool equality = fakeBow.Equals(stubBow);

				Assert.That(equality, Is.False);
				}
			[Test]
			public void CompareTo_Self_ReturnsZero(){
				BowFake fakeBow = MakeBowFake(0);
				int result = fakeBow.CompareTo(fakeBow);

				Assert.That(result, Is.EqualTo(0));
				}
			[Test]
			public void CompareTo_AnotherWithGreaterID_ReturnsNegative(){
				BowFake fakeBow = MakeBowFake(0);
				BowFake anotherBow = MakeBowFake(1);
				int result = fakeBow.CompareTo(anotherBow);

				Assert.That(result, Is.LessThan(0));
				}
			[Test]
			public void CompareTo_AnotherWithLesserID_ReturnsPositive(){
				BowFake fakeBow = MakeBowFake(1);
				BowFake anotherBow = MakeBowFake(0);
				int result = fakeBow.CompareTo(anotherBow);

				Assert.That(result, Is.GreaterThan(0));
				}
			[Test]
			public void CompareTo_AnotherWithSameID_ReturnsZero(){
				BowFake fakeBow = MakeBowFake(1);
				BowFake anotherBow = MakeBowFake(1);
				int result = fakeBow.CompareTo(anotherBow);

				Assert.That(result, Is.EqualTo(0));
				}
			BowFake MakeBowFake(int id){
				BowFake fakeBow = new BowFake();
				fakeBow.ItemID = id;
				return fakeBow;
			}
		}
	}
}
