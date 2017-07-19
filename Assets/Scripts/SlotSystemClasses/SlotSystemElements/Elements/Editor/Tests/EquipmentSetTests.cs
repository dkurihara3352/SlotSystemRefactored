﻿using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using System;
using System.Collections;
using System.Collections.Generic;
namespace SlotSystemTests{
	namespace ElementsTests{
		[TestFixture]
		public class EquipmentSetTests: AbsSlotSystemTest {
			[Test]
			public void Initialize_WhenCalled_SetsFields(){
				EquipmentSet eSet = MakeEquipmentSet();
				ISlotSystemPageElement bowSGPE = MakeSubPageElement();
					ISlotGroup bowSG = MakeSubSG();
					bowSGPE.element.Returns(bowSG);
				ISlotSystemPageElement wearSGPE = MakeSubPageElement();
					ISlotGroup wearSG = MakeSubSG();
					wearSGPE.element.Returns(wearSG);
				ISlotSystemPageElement cGearsSGPE = MakeSubPageElement();
					ISlotGroup cGearsSG = MakeSubSG();
					cGearsSGPE.element.Returns(cGearsSG);
				IEnumerable<ISlotSystemPageElement> pageEles = new ISlotSystemPageElement[]{
					bowSGPE, wearSGPE, cGearsSGPE
				};
				InitializeFieldsTestCase expected = new InitializeFieldsTestCase(bowSG, wearSG, cGearsSG, pageEles);

				eSet.Initialize(bowSGPE, wearSGPE, cGearsSGPE);

				InitializeFieldsTestCase actual = new InitializeFieldsTestCase(eSet.bowSG, eSet.wearSG, eSet.cGearsSG, eSet.pageElements);
				bool equality = actual.Equals(expected);
				Assert.That(equality, Is.True);
			}
				class InitializeFieldsTestCase: IEquatable<InitializeFieldsTestCase>{
					public InitializeFieldsTestCase(ISlotGroup bowSG, ISlotGroup wearSG, ISlotGroup cGearsSG, IEnumerable<ISlotSystemPageElement> pageEles){
						this.bowSG = bowSG;
						this.wearSG = wearSG;
						this.cGearsSG = cGearsSG;
						this.pageEles = pageEles;
					}
					public ISlotGroup bowSG;
					public ISlotGroup wearSG;
					public ISlotGroup cGearsSG;
					public IEnumerable<ISlotSystemPageElement> pageEles;
					public bool Equals(InitializeFieldsTestCase other){
						bool flag = true;
						flag &= object.ReferenceEquals(this.bowSG, other.bowSG);
						flag &= object.ReferenceEquals(this.wearSG, other.wearSG);
						flag &= object.ReferenceEquals(this.cGearsSG, other.cGearsSG);
						IEnumerator rator = this.pageEles.GetEnumerator();
						IEnumerator otherRator = other.pageEles.GetEnumerator();
						while(rator.MoveNext() && otherRator.MoveNext()){
							flag &= object.ReferenceEquals(rator.Current, otherRator.Current);
						}
						return flag;
					}
					
				}
			[Test]
			public void Elements_AfterInit_ReturnsSetPageElesSGs(){
				EquipmentSet eSet = MakeEquipmentSet();
				ISlotSystemPageElement bowSGPE = MakeSubPageElement();
					ISlotGroup bowSG = MakeSubSG();
					bowSGPE.element.Returns(bowSG);
				ISlotSystemPageElement wearSGPE = MakeSubPageElement();
					ISlotGroup wearSG = MakeSubSG();
					wearSGPE.element.Returns(wearSG);
				ISlotSystemPageElement cGearsSGPE = MakeSubPageElement();
					ISlotGroup cGearsSG = MakeSubSG();
					cGearsSGPE.element.Returns(cGearsSG);
				
				ElementsTestCase expected = new ElementsTestCase(bowSG, wearSG, cGearsSG);

				eSet.Initialize(bowSGPE, wearSGPE, cGearsSGPE);

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
			[Test]
			public void Focus_WhenCalled_SetsSelStateFocused(){
				EquipmentSet eSet = MakeEquipmentSet();
				IEnumerable<ISlotSystemPageElement> pEles = new ISlotSystemPageElement[]{};
				eSet.pageElements = pEles;

				eSet.Focus();

				Assert.That(eSet.curSelState, Is.SameAs(AbsSlotSystemElement.focusedState));
			}
			[Test]
			public void Focus_WhenCalled_CallsPElesAccordingly(){
				EquipmentSet eSet = MakeEquipmentSet();
				ISlotSystemPageElement bowSGPE = MakeSubPageElement();
					bowSGPE.isFocusToggleOn.Returns(true);
				ISlotSystemPageElement wearSGPE = MakeSubPageElement();
					wearSGPE.isFocusToggleOn.Returns(false);
				ISlotSystemPageElement cGearsSGPE = MakeSubPageElement();	
					cGearsSGPE.isFocusToggleOn.Returns(false);
				IEnumerable<ISlotSystemPageElement> pEles = new ISlotSystemPageElement[]{
					bowSGPE, wearSGPE, cGearsSGPE
				};
				eSet.pageElements = pEles;

				eSet.Focus();

				bowSGPE.Received().Focus();
				wearSGPE.Received().Defocus();
				cGearsSGPE.Received().Defocus();
			}
			[Test]
			public void Deactivate_WhenCalled_SetsSelStateDeactivated(){
				EquipmentSet eSet = MakeEquipmentSet();
				ISlotSystemPageElement bowSGPE = MakeSubPageElement();
					ISlotGroup bowSG = MakeSubSG();
					bowSGPE.element.Returns(bowSG);
				ISlotSystemPageElement wearSGPE = MakeSubPageElement();
					ISlotGroup wearSG = MakeSubSG();
					wearSGPE.element.Returns(wearSG);
				ISlotSystemPageElement cGearsSGPE = MakeSubPageElement();
					ISlotGroup cGearsSG = MakeSubSG();
					cGearsSGPE.element.Returns(cGearsSG);
				eSet.Initialize(bowSGPE, wearSGPE, cGearsSGPE);
				eSet.Focus();

				eSet.Deactivate();

				Assert.That(eSet.curSelState, Is.SameAs(AbsSlotSystemElement.deactivatedState));
			}
			[Test]
			public void Deactivate_WhenCalled_SetsSelStateDeactivatedRecursively(){
				EquipmentSet eSet = MakeEquipmentSet();
				ISlotSystemPageElement bowSGPE = MakeSubPageElement();
					ISlotGroup bowSG = MakeSubSG();
					bowSGPE.element.Returns(bowSG);
				ISlotSystemPageElement wearSGPE = MakeSubPageElement();
					ISlotGroup wearSG = MakeSubSG();
					wearSGPE.element.Returns(wearSG);
				ISlotSystemPageElement cGearsSGPE = MakeSubPageElement();
					ISlotGroup cGearsSG = MakeSubSG();
					cGearsSGPE.element.Returns(cGearsSG);
				eSet.Initialize(bowSGPE, wearSGPE, cGearsSGPE);
				eSet.Focus();

				eSet.Deactivate();

				bowSG.Received().Deactivate();
				wearSG.Received().Deactivate();
				cGearsSG.Received().Deactivate();
			}
			[TestCase(true, true, true, false, false, false, true, true, true)]
			[TestCase(false, false, false, false, false, false, false, false, false)]
			[TestCase(true, true, true, true, true, true, true, true, true)]
			[TestCase(true, false, false, false, true, true, true, false, false)]
			public void Deactivate_WhenCalled_TogglesPElesBackToDefault(
				bool bowDef, bool wearDef, bool cGDef, 
				bool bowPrev, bool wearPrev, bool cGPrev,
				bool bowExp, bool wearExp, bool cGExp){
				EquipmentSet eSet = MakeEquipmentSet();
				ISlotSystemPageElement bowSGPE = MakeSubPageElement();
					ISlotGroup bowSG = MakeSubSG();
					bowSGPE.element.Returns(bowSG);
				ISlotSystemPageElement wearSGPE = MakeSubPageElement();
					ISlotGroup wearSG = MakeSubSG();
					wearSGPE.element.Returns(wearSG);
				ISlotSystemPageElement cGearsSGPE = MakeSubPageElement();
					ISlotGroup cGearsSG = MakeSubSG();
					cGearsSGPE.element.Returns(cGearsSG);
				bowSGPE.isFocusedOnActivate.Returns(bowDef);
				wearSGPE.isFocusedOnActivate.Returns(wearDef);
				cGearsSGPE.isFocusedOnActivate.Returns(cGDef);
				bowSGPE.isFocusToggleOn.Returns(bowPrev);
				wearSGPE.isFocusToggleOn.Returns(wearPrev);
				cGearsSGPE.isFocusToggleOn.Returns(cGPrev);
				eSet.Initialize(bowSGPE, wearSGPE, cGearsSGPE);

				eSet.Deactivate();

				bowSGPE.Received().isFocusToggleOn = bowExp;
				wearSGPE.Received().isFocusToggleOn = wearExp;
				cGearsSGPE.Received().isFocusToggleOn = cGExp;
			}
		}
	}
}