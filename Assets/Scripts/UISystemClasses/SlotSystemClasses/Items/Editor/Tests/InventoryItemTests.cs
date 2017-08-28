using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UISystem;
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
			BowFake MakeBowFake(int id){
				BowFake fakeBow = new BowFake();
				fakeBow.SetItemID(id);
				return fakeBow;
			}
		}
	}
}
