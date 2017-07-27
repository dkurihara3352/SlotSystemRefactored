using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Utility;
namespace SlotSystemTests{
	namespace ElementsTests{
		[TestFixture]
		[Category("OtherElements")]
		public class EquipmentSetTests: SlotSystemTest {
			[Test][Category("Methods")]
			public void SetElements_WhenCalled_DoNothing(){
				EquipmentSet eSet = MakeEquipmentSet();
					TestSlotSystemElement sseA = MakeTestSSE();
						sseA.transform.SetParent(eSet.transform);
					TestSlotSystemElement sseB = MakeTestSSE();
						sseB.transform.SetParent(eSet.transform);
					TestSlotSystemElement sseC = MakeTestSSE();
						sseC.transform.SetParent(eSet.transform);
				IEnumerable<ISlotSystemElement> expected = new ISlotSystemElement[]{null, null, null};
				
				eSet.SetHierarchy();
				
				bool equality = eSet.MemberEquals(expected);
				Assert.That(equality, Is.True);
			}
			[Test]
			public void Elements_AfterInspectorSetUp_ReturnsSGs(){
				EquipmentSet eSet = MakeEquipmentSet();
					ISlotGroup bowSG = MakeSubSG();
					ISlotGroup wearSG = MakeSubSG();
					ISlotGroup cGearsSG = MakeSubSG();
				ElementsTestCase expected = new ElementsTestCase(bowSG, wearSG, cGearsSG);

				eSet.InspectorSetUp(bowSG, wearSG, cGearsSG);

				ElementsTestCase actual = new ElementsTestCase(eSet.bowSG, eSet.wearSG, eSet.cGearsSG);
				bool equality = actual.Equals(expected);
				Assert.That(equality, Is.True);
			}
				class ElementsTestCase: IEquatable<ElementsTestCase>{
					public ElementsTestCase(ISlotGroup bowSG, ISlotGroup wearSG, ISlotGroup cGearsSG){
						this.bowSG = bowSG;
						this.wearSG = wearSG;
						this.cGearsSG = cGearsSG;
					}
					public ISlotGroup bowSG;
					public ISlotGroup wearSG;
					public ISlotGroup cGearsSG;
					public bool Equals(ElementsTestCase other){
						bool flag = true;
						flag &= object.ReferenceEquals(this.bowSG, other.bowSG);
						flag &= object.ReferenceEquals(this.wearSG, other.wearSG);
						flag &= object.ReferenceEquals(this.cGearsSG, other.cGearsSG);
						return flag;
					}
				}
		}
	}
}
