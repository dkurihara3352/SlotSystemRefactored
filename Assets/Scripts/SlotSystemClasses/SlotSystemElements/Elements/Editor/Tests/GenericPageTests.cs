using UnityEngine;
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
		public class GenericPageTests: AbsSlotSystemTest {

			[Test]
			public void Initialize_WhenCalled_SetsFields(){
				GenericPage gPage = MakeGenPage();
				IEnumerable<ISlotSystemPageElement> pEles = new ISlotSystemPageElement[]{};
				InitializeTestCases expected = new InitializeTestCases("gPageName", pEles);
				
				gPage.Initialize("gPageName", pEles);

				InitializeTestCases actual = new InitializeTestCases(gPage.eName, gPage.pageElements);

				bool equality = actual.Equals(expected);
				Assert.That(equality, Is.True);
			}
				class InitializeTestCases: IEquatable<InitializeTestCases>{
					public string eName;
					public IEnumerable<ISlotSystemPageElement> pEles;
					public InitializeTestCases(string name, IEnumerable<ISlotSystemPageElement> pEles){
						this.eName = name;
						this.pEles = pEles;
					}
					public bool Equals(InitializeTestCases other ){
						bool flag = true;
						flag &= this.eName.Contains(other.eName);
						IEnumerator rator = this.pEles.GetEnumerator();
						IEnumerator otherRator = other.pEles.GetEnumerator();
						while(rator.MoveNext() && otherRator.MoveNext()){
							flag = object.ReferenceEquals(rator.Current, otherRator.Current);
						}
						return flag;
					}
				}
			[Test]
			public void Focus_WhenCalled_SetsSelStateFocused(){
				GenericPage gPage = MakeGenPage();
				ISlotSystemPageElement pEle_A = MakeSubPageElement();
					ISlotSystemElement ele_A = MakeSubSSE();
					pEle_A.element.Returns(ele_A);
				ISlotSystemPageElement pEle_B = MakeSubPageElement();
					ISlotSystemElement ele_B = MakeSubSSE();
					pEle_B.element.Returns(ele_B);
				IEnumerable<ISlotSystemPageElement> pEles = new ISlotSystemPageElement[]{
					pEle_A, pEle_B
				};
				gPage.Initialize("someName", pEles);

				gPage.Focus();

				Assert.That(gPage.curSelState, Is.SameAs(AbsSlotSystemElement.focusedState));
			}
			[TestCase(true, true)]
			[TestCase(true, false)]
			[TestCase(false, false)]
			[TestCase(false, true)]
			public void Focus_WhenCalled_SetsPElesFocusedOrDefocused(
				bool isON_A, bool isON_B){
				GenericPage gPage = MakeGenPage();
				ISlotSystemPageElement pEle_A = MakeSubPageElement();
					ISlotSystemElement ele_A = MakeSubSSE();
					pEle_A.element.Returns(ele_A);
					pEle_A.isFocusToggleOn.Returns(isON_A);
				ISlotSystemPageElement pEle_B = MakeSubPageElement();
					ISlotSystemElement ele_B = MakeSubSSE();
					pEle_B.element.Returns(ele_B);
					pEle_B.isFocusToggleOn.Returns(isON_B);
				IEnumerable<ISlotSystemPageElement> pEles = new ISlotSystemPageElement[]{
					pEle_A, pEle_B
				};
				gPage.Initialize("someName", pEles);

				gPage.Focus();
				if(isON_A)
					pEle_A.Received().Focus();
				else
					pEle_A.Received().Defocus();
				if(isON_B)
					pEle_B.Received().Focus();
				else
					pEle_B.Received().Defocus();	
			}
			[Test]
			public void Deactivate_WhenCalled_SetsSelStateDeactivated(){
				GenericPage gPage = MakeGenPage();
				ISlotSystemPageElement pEle_A = MakeSubPageElement();
					ISlotSystemElement ele_A = MakeSubSSE();
					pEle_A.element.Returns(ele_A);
				ISlotSystemPageElement pEle_B = MakeSubPageElement();
					ISlotSystemElement ele_B = MakeSubSSE();
					pEle_B.element.Returns(ele_B);
				IEnumerable<ISlotSystemPageElement> pEles = new ISlotSystemPageElement[]{
					pEle_A, pEle_B
				};
				gPage.Initialize("someName", pEles);
				gPage.Focus();

				gPage.Deactivate();
				
				Assert.That(gPage.curSelState, Is.SameAs(AbsSlotSystemElement.deactivatedState));
			}
			[TestCase(
				true, true,
				false, false,
				true, true
			)]
			[TestCase(
				false, false,
				true, true,
				false, false
			)]
			public void Deactivate_WhenCalled_TogglesBackPElesToDefault(
				bool def_A, bool def_B,
				bool prev_A, bool prev_B,
				bool exp_A, bool exp_B){
				GenericPage gPage = MakeGenPage();
				ISlotSystemPageElement pEle_A = MakeSubPageElement();
					ISlotSystemElement ele_A = MakeSubSSE();
					pEle_A.element.Returns(ele_A);
					pEle_A.isFocusedOnActivate.Returns(def_A);
				ISlotSystemPageElement pEle_B = MakeSubPageElement();
					ISlotSystemElement ele_B = MakeSubSSE();
					pEle_B.element.Returns(ele_B);
					pEle_B.isFocusedOnActivate.Returns(def_B);
				IEnumerable<ISlotSystemPageElement> pEles = new ISlotSystemPageElement[]{
					pEle_A, pEle_B
				};
				gPage.Initialize("someName", pEles);
				pEle_A.isFocusToggleOn.Returns(prev_A);
				pEle_B.isFocusToggleOn.Returns(prev_B);

				gPage.Deactivate();
				
				pEle_A.Received().isFocusToggleOn = exp_A;
				pEle_B.Received().isFocusToggleOn = exp_B;
			}
		}
	}
}
