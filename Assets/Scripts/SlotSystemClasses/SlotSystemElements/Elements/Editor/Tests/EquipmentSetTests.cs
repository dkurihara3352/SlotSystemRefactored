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
			[Test]
			public void SetHierarchy_TransformChildCountNot3_ThrowsException(){
				EquipmentSet eSet = MakeEquipmentSet();
					TestSlotSystemElement childSSEA = MakeTestSSE();
						childSSEA.transform.SetParent(eSet.transform);
					TestSlotSystemElement childSSEB = MakeTestSSE();
						childSSEB.transform.SetParent(eSet.transform);
					TestSlotSystemElement childSSEC = MakeTestSSE();
						childSSEC.transform.SetParent(eSet.transform);
					TestSlotSystemElement childSSED = MakeTestSSE();
						childSSED.transform.SetParent(eSet.transform);
				
				Exception ex = Assert.Catch<InvalidOperationException>(() => eSet.SetHierarchy());

				Assert.That(ex.Message, Is.StringContaining("transform children' count is not exactly 3"));
			}
			[Test]
			public void SetHierarchy_SomeChildWithoutSG_ThrowsException(){
				EquipmentSet eSet = MakeEquipmentSet();
					SlotGroup sgA = MakeSG();
						sgA.SetElements(new ISlotSystemElement[]{});
						sgA.transform.SetParent(eSet.transform);
					SlotGroup sgB = MakeSG();
						sgB.SetElements(new ISlotSystemElement[]{});
						sgB.transform.SetParent(eSet.transform);
					TestSlotSystemElement childSSEC = MakeTestSSE();
						childSSEC.transform.SetParent(eSet.transform);
				
				Exception ex = Assert.Catch<InvalidOperationException>(() => eSet.SetHierarchy());

				Assert.That(ex.Message, Is.StringContaining("some childrent does not have SG"));
			}
			[Test]
			public void SetHierarchy_SGsWithInvalidFilter_ThrowsException(){
				EquipmentSet eSet = MakeEquipmentSet();
					SlotGroup sgA = MakeSG();
						sgA.SetElements(new ISlotSystemElement[]{});
						sgA.SetFilter(new SGBowFilter());
						sgA.transform.SetParent(eSet.transform);
					SlotGroup sgB = MakeSG();
						sgB.SetElements(new ISlotSystemElement[]{});
						sgB.SetFilter(new SGWearFilter());
						sgB.transform.SetParent(eSet.transform);
					SlotGroup sgC = MakeSG();
						sgC.SetElements(new ISlotSystemElement[]{});
						sgC.SetFilter(new SGNullFilter());
						sgC.transform.SetParent(eSet.transform);
				
				Exception ex = Assert.Catch<InvalidOperationException>(() => eSet.SetHierarchy());

				Assert.That(ex.Message, Is.StringContaining("EquipmentSet.m_cGearsSG: not set, assign in the inspector first"));
			}
			[Test]
			public void SetHierarchy_ValidTransformChildren_SetsThemSGsAndSetsTheirParentThis(){
				EquipmentSet eSet = MakeEquipmentSet();
					SlotGroup xBowSG = MakeSG();
						xBowSG.SetElements(new ISlotSystemElement[]{});
						xBowSG.SetFilter(new SGBowFilter());
						xBowSG.transform.SetParent(eSet.transform);
					SlotGroup xWearSG = MakeSG();
						xWearSG.SetElements(new ISlotSystemElement[]{});
						xWearSG.SetFilter(new SGWearFilter());
						xWearSG.transform.SetParent(eSet.transform);
					SlotGroup xCGearsSG = MakeSG();
						xCGearsSG.SetElements(new ISlotSystemElement[]{});
						xCGearsSG.SetFilter(new SGCGearsFilter());
						xCGearsSG.transform.SetParent(eSet.transform);
				
				eSet.SetHierarchy();

				Assert.That(eSet.bowSG, Is.SameAs(xBowSG));
				Assert.That(eSet.wearSG, Is.SameAs(xWearSG));
				Assert.That(eSet.cGearsSG, Is.SameAs(xCGearsSG));

				Assert.That(xBowSG.parent, Is.SameAs(eSet));
				Assert.That(xWearSG.parent, Is.SameAs(eSet));
				Assert.That(xCGearsSG.parent, Is.SameAs(eSet));
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
