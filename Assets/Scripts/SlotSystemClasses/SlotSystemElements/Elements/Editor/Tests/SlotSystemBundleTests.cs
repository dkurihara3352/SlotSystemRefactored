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
		public class SlotSystemBundleTests: SlotSystemTest {
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
		}
	}
}

