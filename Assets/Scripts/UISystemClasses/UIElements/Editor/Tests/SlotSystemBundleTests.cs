using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using UISystem;
using System;
using System.Collections;
using System.Collections.Generic;
namespace SlotSystemTests{
	namespace ElementsTests{
		[TestFixture]
		[Category("OtherElements")]
		public class SlotSystemBundleTests: SlotSystemTest {
			[TestCaseSource(typeof(SetFocusedBundleElementMemberCases))]
			public void SetFocusedBundleElement_Member_SetsItAsTheFocused(UIBundle bun, IUIElement member){
				bun.SetFocusedElement(member);

				Assert.That(bun.GetFocusedElement(), Is.SameAs(member));
			}
				class SetFocusedBundleElementMemberCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] case0;
							UIBundle bun_0 = MakeSSBundle();
								TestUIElement sseA_0 = MakeTestSSE();									
								TestUIElement sseB_0 = MakeTestSSE();
								TestUIElement sseC_0 = MakeTestSSE();
								sseA_0.transform.SetParent(bun_0.transform);
								sseB_0.transform.SetParent(bun_0.transform);
								sseC_0.transform.SetParent(bun_0.transform);
							bun_0.SetHierarchy();
							case0 = new object[]{bun_0, sseB_0};
							yield return case0;
					}
				}
		}
	}
}

