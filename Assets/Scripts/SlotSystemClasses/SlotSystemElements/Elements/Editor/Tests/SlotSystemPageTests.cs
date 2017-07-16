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
		public class SlotSystemPageTests: AbsSlotSystemTest {

			[Test]
			public void PageFocus_Various_CallsPageElesAppropriately(){
				TestSlotSystemPage ssp = MakeTestSSPage();
				ISlotSystemPageElement mockPEle_A = MakeSubPageElement();
					mockPEle_A.isFocusToggleOn.Returns(true);
				ISlotSystemPageElement mockPEle_B = MakeSubPageElement();
					mockPEle_B.isFocusToggleOn.Returns(false);
				IEnumerable<ISlotSystemPageElement> pEles = new ISlotSystemPageElement[]{
					mockPEle_A, mockPEle_B
				};
				ssp.SetPageElements(pEles);

				ssp.PageFocus();

				mockPEle_A.Received().Focus();
				mockPEle_B.Received().Defocus();
			}
			[Test]
			public void ToggleBack_WhenCalled_SetsPElesDefaultToggle(){
				TestSlotSystemPage ssp = MakeTestSSPage();
				ISlotSystemPageElement mockPEle_A = MakeSubPageElement();
					mockPEle_A.isFocusedOnActivate.Returns(true);
				ISlotSystemPageElement mockPEle_B = MakeSubPageElement();
					mockPEle_B.isFocusedOnActivate.Returns(false);
				IEnumerable<ISlotSystemPageElement> pEles = new ISlotSystemPageElement[]{
					mockPEle_A, mockPEle_B
				};
				ssp.SetPageElements(pEles);

				ssp.ToggleBack();

				mockPEle_A.Received().isFocusToggleOn = true;
				mockPEle_B.Received().isFocusToggleOn = false;
			}
			[TestCase(true, false, true, false)]
			[TestCase(true, true, false, true)]
			[TestCase(false, false, false, false)]
			[TestCase(false, true, true, true)]
			public void TogglePageElementFocus_WhenCalled_FindAndTogglePEleAndCallsSSMFocus(bool isFocusToggelOn, bool toggle, bool called, bool expected){
				TestSlotSystemPage ssp = MakeTestSSPage();
				ISlotSystemManager mockSSM = MakeSubSSM();
				ssp.ssm = mockSSM;
				ISlotSystemPageElement stubPEle_A = MakeSubPageElement();
				ISlotSystemPageElement mockPEle_B = MakeSubPageElement();
				ISlotSystemPageElement stubPEle_C = MakeSubPageElement();
				ISlotSystemElement stubEle_A = MakeSubSSE();
				ISlotSystemElement stubEle_B = MakeSubSSE();
				ISlotSystemElement stubEle_C = MakeSubSSE();
				stubPEle_A.element.Returns(stubEle_A);
				mockPEle_B.element.Returns(stubEle_B);
				stubPEle_C.element.Returns(stubEle_C);
				mockPEle_B.isFocusToggleOn.Returns(isFocusToggelOn);
				IEnumerable<ISlotSystemPageElement> eles = new ISlotSystemPageElement[]{
					stubPEle_A, mockPEle_B, stubPEle_C
				};
				ssp.SetPageElements(eles);

				ssp.TogglePageElementFocus(stubEle_B, toggle);

				if(called)
					mockPEle_B.Received().isFocusToggleOn = expected;
				else
					mockPEle_B.DidNotReceive().isFocusToggleOn = expected;
				mockSSM.Received().Focus();
			}
			[TestCaseSource(typeof(GetPageElementCases))]
			public void GetPageElement_WhenCalled_FindAndReturnsMatchedPEle(IEnumerable<ISlotSystemPageElement> eles, ISlotSystemElement key, ISlotSystemPageElement expected){
				TestSlotSystemPage ssp = MakeTestSSPage();
				ssp.SetPageElements(eles);

				ISlotSystemPageElement sspe = ssp.GetPageElement(key);

				Assert.That(sspe, Is.SameAs(expected));
			}
				class GetPageElementCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						ISlotSystemPageElement stubPEle_A = MakeSubPageElement();
						ISlotSystemElement	stubSSE_A = MakeSubSSE();
						stubPEle_A.element.Returns(stubSSE_A);
						ISlotSystemPageElement stubPEle_B = MakeSubPageElement();
						ISlotSystemElement	stubSSE_B = MakeSubSSE();
						stubPEle_B.element.Returns(stubSSE_B);
						ISlotSystemPageElement stubPEle_C = MakeSubPageElement();
						ISlotSystemElement	stubSSE_C = MakeSubSSE();
						stubPEle_C.element.Returns(stubSSE_C);
						IEnumerable<ISlotSystemPageElement> eles = new ISlotSystemPageElement[]{
							stubPEle_A, stubPEle_B, stubPEle_C
						};
						yield return new object[]{
							eles, stubSSE_A, stubPEle_A
						};
						yield return new object[]{
							eles, stubSSE_B, stubPEle_B
						};
						yield return new object[]{
							eles, stubSSE_C, stubPEle_C
						};
					}
				}
		}
	}
}
