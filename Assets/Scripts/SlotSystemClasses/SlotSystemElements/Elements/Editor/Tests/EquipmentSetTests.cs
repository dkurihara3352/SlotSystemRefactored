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
						sgA.SetSBHandler(Substitute.For<ISBHandler>());
						sgA.SetElements(new ISlotSystemElement[]{});
						sgA.SetFilterHandler(Substitute.For<IFilterHandler>());
						sgA.transform.SetParent(eSet.transform);
					SlotGroup sgB = MakeSG();
						sgB.SetSBHandler(Substitute.For<ISBHandler>());
						sgB.SetElements(new ISlotSystemElement[]{});
						sgB.SetFilterHandler(Substitute.For<IFilterHandler>());
						sgB.transform.SetParent(eSet.transform);
					TestSlotSystemElement childSSEC = MakeTestSSE();
						childSSEC.transform.SetParent(eSet.transform);
				
				Exception ex = Assert.Catch<InvalidOperationException>(() => eSet.SetHierarchy());

				Assert.That(ex.Message, Is.StringContaining("some childrent does not have SG"));
			}
			[Test]
			public void SetHierarchy_SGsWithInvalidFilter_ThrowsException(){
				EquipmentSet eSet = MakeEquipmentSet();
					SlotGroup sgA = MakeSG_FilterHandler_RSBHandler();
						sgA.SetElements(new ISlotSystemElement[]{});
						IFilterHandler sgAFilterHandler = Substitute.For<IFilterHandler>();
						sgAFilterHandler.GetFilter().Returns(new SGBowFilter());
						sgA.SetFilterHandler(sgAFilterHandler);
						sgA.transform.SetParent(eSet.transform);
					SlotGroup sgB = MakeSG_FilterHandler_RSBHandler();
						sgB.SetElements(new ISlotSystemElement[]{});
						IFilterHandler sgBFilterHandler = Substitute.For<IFilterHandler>();
						sgBFilterHandler.GetFilter().Returns(new SGWearFilter());
						sgB.SetFilterHandler(sgBFilterHandler);
						sgB.transform.SetParent(eSet.transform);
					SlotGroup sgC = MakeSG_FilterHandler_RSBHandler();
						sgC.SetElements(new ISlotSystemElement[]{});
						IFilterHandler sgCFilterHandler = Substitute.For<IFilterHandler>();
						sgCFilterHandler.GetFilter().Returns(new SGNullFilter());
						sgC.SetFilterHandler(sgCFilterHandler);
						sgC.transform.SetParent(eSet.transform);
				
				Exception ex = Assert.Catch<InvalidOperationException>(() => eSet.SetHierarchy());

				Assert.That(ex.Message, Is.StringContaining("EquipmentSet.m_cGearsSG: not set, assign in the inspector first"));
			}
			[Test]
			public void SetHierarchy_ValidTransformChildren_SetsThemSGsAndSetsTheirParentThis(){
				EquipmentSet eSet = MakeEquipmentSet();
					SlotGroup xBowSG = MakeSG_FilterHandler_RSBHandler();
						xBowSG.SetElements(new ISlotSystemElement[]{});
						IFilterHandler xBowSGFilterHandler = Substitute.For<IFilterHandler>();
						xBowSGFilterHandler.GetFilter().Returns(new SGBowFilter());
						xBowSG.SetFilterHandler(xBowSGFilterHandler);
						xBowSG.transform.SetParent(eSet.transform);
					SlotGroup xWearSG = MakeSG_FilterHandler_RSBHandler();
						xWearSG.SetElements(new ISlotSystemElement[]{});
						IFilterHandler xWearSGFilterHandler = Substitute.For<IFilterHandler>();
						xWearSGFilterHandler.GetFilter().Returns(new SGWearFilter());
						xWearSG.SetFilterHandler(xWearSGFilterHandler);
						xWearSG.transform.SetParent(eSet.transform);
					SlotGroup xCGearsSG = MakeSG_FilterHandler_RSBHandler();
						xCGearsSG.SetElements(new ISlotSystemElement[]{});
						IFilterHandler xCGearsSGFilterHandler = Substitute.For<IFilterHandler>();
						xCGearsSGFilterHandler.GetFilter().Returns(new SGCGearsFilter());
						xCGearsSG.SetFilterHandler(xCGearsSGFilterHandler);
						xCGearsSG.transform.SetParent(eSet.transform);
				
				eSet.SetHierarchy();

				Assert.That(eSet.bowSG, Is.SameAs(xBowSG));
				Assert.That(eSet.wearSG, Is.SameAs(xWearSG));
				Assert.That(eSet.cGearsSG, Is.SameAs(xCGearsSG));

				Assert.That(xBowSG.GetParent(), Is.SameAs(eSet));
				Assert.That(xWearSG.GetParent(), Is.SameAs(eSet));
				Assert.That(xCGearsSG.GetParent(), Is.SameAs(eSet));
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
