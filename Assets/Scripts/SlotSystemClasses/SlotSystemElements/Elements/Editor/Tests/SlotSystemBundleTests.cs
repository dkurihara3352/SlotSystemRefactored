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
		public class SlotSystemBundleTests: AbsSlotSystemTest {
			[TestCaseSource(typeof(SetFocusedBundleElementMemberCases))]
			public void SetFocusedBundleElement_Member_SetsItAsTheFocused(IEnumerable<ISlotSystemElement> eles, ISlotSystemElement member){
				SlotSystemBundle bun = MakeSSBundle();
				bun.Initialize("someName", eles);

				bun.SetFocusedBundleElement(member);

				Assert.That(bun.focusedElement, Is.SameAs(member));
			}
				class SetFocusedBundleElementMemberCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						ISlotSystemElement eleA = MakeSubSSE();
						ISlotSystemElement eleB = MakeSubSSE();
						ISlotSystemElement eleC = MakeSubSSE();
						IEnumerable<ISlotSystemElement> eles = new ISlotSystemElement[]{
							eleA, eleB, eleC
						};
						yield return new object[]{
							eles, eleA
						};
						yield return new object[]{
							eles, eleB
						};
						yield return new object[]{
							eles, eleC
						};
					}
				}
			[TestCaseSource(typeof(FocusCases))]
			public void Focus_WhenCalled_CallsElesAccordingly(IEnumerable<ISlotSystemElement> eles, ISlotSystemElement focused){
				SlotSystemBundle bun = MakeSSBundle();
				bun.Initialize("someName", eles);
				bun.SetFocusedBundleElement(focused);

				bun.Focus();

				foreach(var ele in bun.elements){
					if(ele == focused)
						ele.Received().Focus();
					else
						ele.Received().Defocus();
				}
			}
				class FocusCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						ISlotSystemElement eleA = MakeSubSSE();
						ISlotSystemElement eleB = MakeSubSSE();
						ISlotSystemElement eleC = MakeSubSSE();
						IEnumerable<ISlotSystemElement> eles = new ISlotSystemElement[]{
							eleA, eleB, eleC
						};
						yield return new object[]{
							eles, eleA
						};
						yield return new object[]{
							eles, eleB
						};
						yield return new object[]{
							eles, eleC
						};
					}
				}
		}
	}
}

