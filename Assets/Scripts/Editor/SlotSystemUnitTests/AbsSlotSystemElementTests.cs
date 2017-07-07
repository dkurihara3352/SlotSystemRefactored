using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
[TestFixture]
public class AbsSlotSystemElementTests{
	/*	State and process */
		[Test]
		[Category("State and Process")]
		public void selProcess_ByDefault_IsNull(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			
			Assert.That(testSSE.selProcess, Is.Null);
		}
		/*	SelStates	*/
			/*	To Deactivated */
				[Test]
				[Category("State and Process")]
				public void SetSelState_NullToDeactivated_LeaveSelProcNull(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.deactivatedState);

					Assert.That(testSSE.selProcess, Is.Null);
				}
				[Test]
				[Category("State and Process")]
				public void SetSelState_ToDeactivated_SetsSelProcNull(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.deactivatedState);
					testSSE.SetSelState(AbsSlotSystemElement.focusedState);
					testSSE.SetSelState(AbsSlotSystemElement.defocusedState);
					testSSE.SetSelState(AbsSlotSystemElement.deactivatedState);

					Assert.That(testSSE.selProcess, Is.Null);
				}
			/*	To Defocus */
				[Test]
				[Category("State and Process")]
				public void SetSelState_DeactivatedToDefocused_SetsSelProcNull(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.deactivatedState);
					testSSE.SetSelState(AbsSlotSystemElement.defocusedState);

					Assert.That(testSSE.selProcess, Is.Null);
				}
				[Test]
				[Category("State and Process")]
				public void SetSelState_DeactivatedToDefocused_CallsInstantGreyout(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.deactivatedState);
					testSSE.SetSelState(AbsSlotSystemElement.defocusedState);

					Assert.That(testSSE.message, Is.StringContaining("InstantGreyout called"));
				}
				[Test]
				[Category("State and Process")]
				public void SetSelState_FocusedToDefocused_SetsSelProcGreyout(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.focusedState);
					testSSE.SetSelState(AbsSlotSystemElement.defocusedState);

					Assert.That(testSSE.selProcess, Is.TypeOf(typeof(SSEGreyoutProcess)));
				}
				[Test]
				[Category("State and Process")]
				public void SetSelState_SelectedToDefocused_SetsSelProcDehighlight(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.selectedState);
					testSSE.SetSelState(AbsSlotSystemElement.defocusedState);

					Assert.That(testSSE.selProcess, Is.TypeOf(typeof(SSEDehighlightProcess)));
				}
			/*	To Focus	*/
				[Test]
				[Category("State and Process")]
				public void SetSelState_NullToFocused_LeavesSelProcNull(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.focusedState);

					Assert.That(testSSE.selProcess, Is.Null);
				}
				[Test]
				[Category("State and Process")]
				public void SetSelState_DeactivatedToFocused_LeavesSelProcNull(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.deactivatedState);
					testSSE.SetSelState(AbsSlotSystemElement.focusedState);

					Assert.That(testSSE.selProcess, Is.Null);
				}
				[Test]
				[Category("State and Process")]
				public void SetSelState_DeactivatedToFocused_CallsInstantGreyin(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.deactivatedState);
					testSSE.SetSelState(AbsSlotSystemElement.focusedState);

					Assert.That(testSSE.message, Is.StringContaining("InstantGreyin called"));
				}
				[Test]
				[Category("State and Process")]
				public void SetSelState_DefocusedToFocused_SetsSelProcGreyin(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.defocusedState);
					testSSE.SetSelState(AbsSlotSystemElement.focusedState);

					Assert.That(testSSE.selProcess, Is.TypeOf(typeof(SSEGreyinProcess)));
				}
				[Test]
				[Category("State and Process")]
				public void SetSelState_SelectedToFocused_SetsSelProcDehighlight(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.selectedState);
					testSSE.SetSelState(AbsSlotSystemElement.focusedState);

					Assert.That(testSSE.selProcess, Is.TypeOf(typeof(SSEDehighlightProcess)));
				}
			/*	To Selected */
				[Test]
				[Category("State and Process")]
				public void SetSelState_NullToSelected_LeavesSelProcNull(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.selectedState);

					Assert.That(testSSE.selProcess, Is.Null);
				}
				[Test]
				[Category("State and Process")]
				public void SetSelState_DeactivatedToSelected_LeavesSelProcNull(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.deactivatedState);
					testSSE.SetSelState(AbsSlotSystemElement.selectedState);

					Assert.That(testSSE.selProcess, Is.Null);
				}
				[Test]
				[Category("State and Process")]
				public void SetSelState_DeactivatedToSelected_CallsInstantHighlight(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.deactivatedState);
					testSSE.SetSelState(AbsSlotSystemElement.selectedState);

					Assert.That(testSSE.message, Is.StringContaining("InstantHighlight called"));
				}
				[Test]
				[Category("State and Process")]
				public void SetSelState_FocusedToSelected_SetsSelProcHighlight(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.focusedState);
					testSSE.SetSelState(AbsSlotSystemElement.selectedState);

					Assert.That(testSSE.selProcess, Is.TypeOf(typeof(SSEHighlightProcess)));
				}
				[Test]
				[Category("State and Process")]
				public void SetSelState_DefocusedToSelected_SetsSelProcHighlight(){
					TestSlotSystemElement testSSE = MakeSlotSystemElement();
					testSSE.SetSelState(AbsSlotSystemElement.defocusedState);
					testSSE.SetSelState(AbsSlotSystemElement.selectedState);

					Assert.That(testSSE.selProcess, Is.TypeOf(typeof(SSEHighlightProcess)));
				}
		/*	Act States	*/
			//	no entry
	/*	Fields	*/
		[Test]
		[Category("Fields")]
		public void parent_ByDefault_IsNull(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();

			Assert.That(testSSE.parent, Is.Null);
		}
		[Category("Fields")]
		[Test]
		public void immediateBundle_WhenParentNull_ReturnsNull(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();

			Assert.That(testSSE.immediateBundle, Is.Null);
		}
		[Test]
		[Category("Fields")]
		public void immediateBundle_WhenParentIsBundle_ReturnsBundle(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			SlotSystemBundle stubBundle = MakeSlotSystemBundle();
			testSSE.parent = stubBundle;

			Assert.That(testSSE.immediateBundle, Is.SameAs(stubBundle));
		}
		[Test]
		[Category("Fields")]
		public void immediateBundle_NoBundleInAncestry_ReturnsNull(){
			SlotSystemElement stubEle_1 = Substitute.For<SlotSystemElement>();
			SlotSystemElement stubEle_2 = Substitute.For<SlotSystemElement>();
			stubEle_1.parent = stubEle_2;
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			testSSE.parent = stubEle_1;

			Assert.That(testSSE.immediateBundle, Is.Null);
		}
		[Test]
		[Category("Fields")]
		public void immediateBundle_SomeBundlesUpInAncestry_ReturnsMostProximal(){
			SlotSystemBundle stubBundle_further = MakeSlotSystemBundle();
			SlotSystemBundle stubBundle_closer = MakeSlotSystemBundle();
			SlotSystemElement stubEle_1 = MakeSlotSystemElement();
			SlotSystemElement stubEle_2 = MakeSlotSystemElement();

			stubEle_2.parent = stubEle_1;
			stubEle_1.parent = stubBundle_closer;
			stubBundle_closer.parent = stubBundle_further;
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			testSSE.parent = stubEle_2;

			Assert.That(testSSE.immediateBundle, Is.SameAs(stubBundle_closer));
		}
		[Test]
		[Category("Fields")]
		public void level_ParentNull_ReturnsZero(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();

			Assert.That(testSSE.level, Is.EqualTo(0));
		}
		[Test]
		[Category("Fields")]
		public void level_OneParent_ReturnsOne(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemElement stubEle = MakeSlotSystemElement();
			testSSE.parent = stubEle;

			Assert.That(testSSE.level, Is.EqualTo(1));
		}
		[Test]
		[Category("Fields")]
		public void level_TwoParent_ReturnsTwo(){
			TestSlotSystemElement stubEle_1 = MakeSlotSystemElement();
			TestSlotSystemElement stubEle_2 = MakeSlotSystemElement();
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			stubEle_2.parent = stubEle_1;
			testSSE.parent = stubEle_2;

			Assert.That(testSSE.level, Is.EqualTo(2));
		}
		[Test]
		[Category("Fields")]
		public void isBundleElement_ParentIsBundle_ReturnsTrue(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			SlotSystemBundle stubBundle = MakeSlotSystemBundle();
			testSSE.parent = stubBundle;

			Assert.That(testSSE.isBundleElement, Is.True);
		}
		[Test]
		[Category("Fields")]
		public void isBundleElement_ParentIsNotBundle_ReturnsFalse(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemElement stubSSE = MakeSlotSystemElement();
			testSSE.parent = stubSSE;

			Assert.That(testSSE.isBundleElement, Is.False);
		}
		[Test]
		[Category("Fields")]
		public void isPageElement_ParentIsPage_ReturnsTrue(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemPage stubPage = MakeSlotSystemPage();
			testSSE.parent = stubPage;

			Assert.That(testSSE.isPageElement, Is.True);
		}
		[Test]
		[Category("Fields")]
		public void isPageElement_ParentIsNotPage_ReturnsFalse(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemElement stubSSE = MakeSlotSystemElement();
			testSSE.parent = stubSSE;

			Assert.That(testSSE.isPageElement, Is.False);
		}
		[Test]
		[Category("Fields")]
		public void isToggledOn_ParentNotPage_ReturnsFalse(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemElement stubSSE = MakeSlotSystemElement();
			testSSE.parent = stubSSE;

			Assert.That(testSSE.isToggledOn, Is.False);
		}
		[Test]
		[Category("Fields")]
		public void isToggledOn_ParentIsPageAndElementToggledOn_ReturnsTrue(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemPage stubPage = MakeSlotSystemPage();
			SlotSystemPageElement stubPageElement = MakeSlotSystemPageElement(testSSE, true);
			stubPage.AddPageElement(testSSE, stubPageElement);
			testSSE.parent = stubPage;

			Assert.That(testSSE.isToggledOn, Is.True);
		}
		[Test]
		[Category("Fields")]
		public void isToggledOn_ParentIsPageAndElementToggledOff_ReturnsFalse(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemPage stubPage = MakeSlotSystemPage();
			SlotSystemPageElement stubPageElement = MakeSlotSystemPageElement(testSSE, false);
			stubPage.AddPageElement(testSSE, stubPageElement);
			testSSE.parent = stubPage;

			Assert.That(testSSE.isToggledOn, Is.False);
		}
		[Test]
		[Category("Fields")]
		public void isFocused_CurSelStateIsFocused_ReturnsTrue(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			testSSE.SetSelState(AbsSlotSystemElement.focusedState);

			Assert.That(testSSE.isFocused, Is.True);
			Assert.That(testSSE.isDefocused, Is.False);
			Assert.That(testSSE.isDeactivated, Is.False);
		}
		[Test]
		[Category("Fields")]
		public void isDefocused_CurSelStateIsDefocused_ReturnsTrue(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			testSSE.SetSelState(AbsSlotSystemElement.defocusedState);

			Assert.That(testSSE.isFocused, Is.False);
			Assert.That(testSSE.isDefocused, Is.True);
			Assert.That(testSSE.isDeactivated, Is.False);
		}
		[Test]
		[Category("Fields")]
		public void isDeactivated_CurSelStateIsDeactivated_ReturnsTrue(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			testSSE.SetSelState(AbsSlotSystemElement.deactivatedState);

			Assert.That(testSSE.isFocused, Is.False);
			Assert.That(testSSE.isDefocused, Is.False);
			Assert.That(testSSE.isDeactivated, Is.True);
		}
		[Test]
		[Category("Fields")]
		public void isFocusedInHierarchy_SelfFocusedAndNoParent_ReturnsTrue(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			testSSE.Focus();

			Assert.That(testSSE.isFocusedInHierarchy, Is.True);
		}
		[Test]
		[Category("Fields")]
		public void isFocusedInHierarchy_SelfNotFocused_ReturnsFalse(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();

			Assert.That(testSSE.isFocusedInHierarchy, Is.False);
		}
		[Test]
		[Category("Fields")]
		public void isFocusedInHierarchy_SelfFocusedParentNotFocused_ReturnsFalse(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemElement stubPar = MakeSlotSystemElement();
			testSSE.parent = stubPar;
			stubPar.Defocus();
			testSSE.Focus();
			
			Assert.That(testSSE.isFocusedInHierarchy, Is.False);
		}
		[Test]
		[Category("Fields")]
		public void isFocusedInHierarchy_SelfAndAllParentsFocused_ReturnsTrue(){
			TestSlotSystemElement stubPar1 = MakeSlotSystemElement();
			TestSlotSystemElement stubPar2 = MakeSlotSystemElement();
			TestSlotSystemElement stubPar3 = MakeSlotSystemElement();
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			testSSE.parent = stubPar3;
			stubPar3.SetElements(new SlotSystemElement[]{testSSE});
			stubPar3.parent = stubPar2;
			stubPar2.SetElements(new SlotSystemElement[]{stubPar3});
			stubPar2.parent = stubPar1;
			stubPar1.SetElements(new SlotSystemElement[]{stubPar2});
			stubPar1.Focus();
			
			Assert.That(testSSE.isFocusedInHierarchy, Is.True);
		}
		[Test]
		[Category("Fields")]
		public void isFocusedInHierarchy_SomeNonfocusedAncester_ReturnsFalse(){
			TestSlotSystemElement stubPar1 = MakeSlotSystemElement();
			TestSlotSystemElement stubPar2 = MakeSlotSystemElement();
			TestSlotSystemElement stubPar3 = MakeSlotSystemElement();
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			testSSE.parent = stubPar3;
			stubPar3.SetElements(new SlotSystemElement[]{testSSE});
			stubPar3.parent = stubPar2;
			stubPar2.SetElements(new SlotSystemElement[]{stubPar3});
			stubPar2.parent = stubPar1;
			stubPar1.SetElements(new SlotSystemElement[]{stubPar2});
			stubPar2.Focus();
			
			Assert.That(testSSE.isFocusedInHierarchy, Is.False);
		}
	/*	Methods	*/
		[Test]
		[Category("Methods")]
		public void Initialize_WhenCalled_SelStatesSetToSSEDeactivated(){
				TestSlotSystemElement testSSE = MakeSlotSystemElement();

				testSSE.Initialize();

				Assert.That(testSSE.prevSelState, Is.EqualTo(AbsSlotSystemElement.deactivatedState));
				Assert.That(testSSE.curSelState, Is.EqualTo(AbsSlotSystemElement.deactivatedState));
			}
		[Test]
		[Category("Methods")]
		public void ContainsInHierarchy_Null_ThrowsException(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();

			System.Exception ex = Assert.Throws<System.ArgumentNullException>(() => testSSE.ContainsInHierarchy(null));
		}
		[Test]
		[Category("Methods")]
		public void ContainsInHierarchy_Self_ReturnsFalse(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();

			bool result = testSSE.ContainsInHierarchy(testSSE);

			Assert.That(result, Is.False);
		}
		[Test]
		public void ContainsInHierarchy_Parent_ReturnsFalse(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemElement stubPar = MakeSlotSystemElement();
			testSSE.parent = stubPar;

			bool result = testSSE.ContainsInHierarchy(stubPar);

			Assert.That(result, Is.False);
		}
		[Test]
		[Category("Methods")]
		public void ContainsInHierarchy_DirectChild_ReturnsTrue(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemElement stubChild = MakeSlotSystemElement();
			stubChild.parent = testSSE;

			bool result = testSSE.ContainsInHierarchy(stubChild);

			Assert.That(result, Is.True);
		}
		[Test]
		[Category("Methods")]
		public void ContainsInHierarchy_DistantChild_ReturnsTrue(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemElement stubChild = MakeSlotSystemElement();
			TestSlotSystemElement stubGrandChild = MakeSlotSystemElement();
			stubGrandChild.parent = stubChild;
			stubChild.parent = testSSE;

			bool result = testSSE.ContainsInHierarchy(stubGrandChild);

			Assert.That(result, Is.True);
		}
		[Test]
		[Category("Methods")]
		public void Activate_WhenCalled_CallsChildrensActivate(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			SlotSystemElement mockChildA = Substitute.For<SlotSystemElement>();
			SlotSystemElement mockChildB = Substitute.For<SlotSystemElement>();
			SlotSystemElement mockChildC = Substitute.For<SlotSystemElement>();
			testSSE.SetElements(new SlotSystemElement[]{mockChildA, mockChildB, mockChildC});

			testSSE.Activate();

			mockChildA.Received().Activate();
			mockChildB.Received().Activate();
			mockChildC.Received().Activate();
		}
		[Test]
		[Category("Methods")]
		public void Deactivate_WhenCalled_CallsChildrensDeactivate(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			SlotSystemElement mockChildA = Substitute.For<SlotSystemElement>();
			SlotSystemElement mockChildB = Substitute.For<SlotSystemElement>();
			SlotSystemElement mockChildC = Substitute.For<SlotSystemElement>();
			testSSE.SetElements(new SlotSystemElement[]{mockChildA, mockChildB, mockChildC});

			testSSE.Deactivate();

			mockChildA.Received().Deactivate();
			mockChildB.Received().Deactivate();
			mockChildC.Received().Deactivate();
		}
		[Test]
		[Category("Methods")]
		public void Deactivate_WhenCalled_SetCurStateToDeactivated(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			
			testSSE.Deactivate();

			Assert.That(testSSE.curSelState, Is.SameAs(AbsSlotSystemElement.deactivatedState));
		}
		[Test]
		[Category("Methods")]
		public void Focus_WhenCalled_CallsChildrensFocus(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			SlotSystemElement mockChildA = Substitute.For<SlotSystemElement>();
			SlotSystemElement mockChildB = Substitute.For<SlotSystemElement>();
			SlotSystemElement mockChildC = Substitute.For<SlotSystemElement>();
			testSSE.SetElements(new SlotSystemElement[]{mockChildA, mockChildB, mockChildC});

			testSSE.Focus();

			mockChildA.Received().Focus();
			mockChildB.Received().Focus();
			mockChildC.Received().Focus();
		}
		[Test]
		[Category("Methods")]
		public void Focus_WhenCalled_SetCurStateToFocusd(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			
			testSSE.Focus();

			Assert.That(testSSE.curSelState, Is.SameAs(AbsSlotSystemElement.focusedState));
		}
		[Test]
		[Category("Methods")]
		public void Defocus_WhenCalled_CallsChildrensDefocus(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			SlotSystemElement mockChildA = Substitute.For<SlotSystemElement>();
			SlotSystemElement mockChildB = Substitute.For<SlotSystemElement>();
			SlotSystemElement mockChildC = Substitute.For<SlotSystemElement>();
			testSSE.SetElements(new SlotSystemElement[]{mockChildA, mockChildB, mockChildC});

			testSSE.Defocus();

			mockChildA.Received().Defocus();
			mockChildB.Received().Defocus();
			mockChildC.Received().Defocus();
		}
		[Test]
		[Category("Methods")]
		public void Defocus_WhenCalled_SetCurStateToDefocusd(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			
			testSSE.Defocus();

			Assert.That(testSSE.curSelState, Is.SameAs(AbsSlotSystemElement.defocusedState));
		}
		[Test]
		[Category("Methods")]
		public void PerformInHierarchyVer1_WhenCalled_PerformActionOnElements(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_1 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_1_1 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_1_2 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_2 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_2_1 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_2_2 = MakeSlotSystemElement();
			TestSlotSystemElement mockPar = MakeSlotSystemElement();
			mockChild_1.SetElements(new SlotSystemElement[]{mockChild_1_1, mockChild_1_2});
			mockChild_2.SetElements(new SlotSystemElement[]{mockChild_2_1, mockChild_2_2});
			testSSE.SetElements(new SlotSystemElement[]{mockChild_1, mockChild_2});
			mockPar.SetElements(new SlotSystemElement[]{testSSE});

			testSSE.PerformInHierarchy(x => ((TestSlotSystemElement)x).message = "performed");

			Assert.That(mockPar.message, Is.Empty);
			Assert.That(testSSE.message, Is.StringContaining("performed"));
			Assert.That(mockChild_1.message, Is.StringContaining("performed"));
			Assert.That(mockChild_1_1.message, Is.StringContaining("performed"));
			Assert.That(mockChild_1_2.message, Is.StringContaining("performed"));
			Assert.That(mockChild_2.message, Is.StringContaining("performed"));
			Assert.That(mockChild_2_1.message, Is.StringContaining("performed"));
			Assert.That(mockChild_2_2.message, Is.StringContaining("performed"));
		}
		[Test]
		[Category("Methods")]
		public void PerformInHierarchyVer2_WhenCalled_PerformActionOnElements(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_1 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_1_1 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_1_2 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_2 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_2_1 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_2_2 = MakeSlotSystemElement();
			TestSlotSystemElement mockPar = MakeSlotSystemElement();
			mockChild_1.SetElements(new SlotSystemElement[]{mockChild_1_1, mockChild_1_2});
			mockChild_2.SetElements(new SlotSystemElement[]{mockChild_2_1, mockChild_2_2});
			testSSE.SetElements(new SlotSystemElement[]{mockChild_1, mockChild_2});
			mockPar.SetElements(new SlotSystemElement[]{testSSE});

			System.Action<SlotSystemElement, object> act = (SlotSystemElement x, object s) => ((TestSlotSystemElement)x).message = (string)s;
			testSSE.PerformInHierarchy(act, "performed");

			Assert.That(mockPar.message, Is.Empty);
			Assert.That(testSSE.message, Is.StringContaining("performed"));
			Assert.That(mockChild_1.message, Is.StringContaining("performed"));
			Assert.That(mockChild_1_1.message, Is.StringContaining("performed"));
			Assert.That(mockChild_1_2.message, Is.StringContaining("performed"));
			Assert.That(mockChild_2.message, Is.StringContaining("performed"));
			Assert.That(mockChild_2_1.message, Is.StringContaining("performed"));
			Assert.That(mockChild_2_2.message, Is.StringContaining("performed"));
		}
		[Test]
		[Category("Methods")]
		public void PerformInHierarchyVer3_WhenCalled_PerformActionOnElements(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_1 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_1_1 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_1_2 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_2 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_2_1 = MakeSlotSystemElement();
			TestSlotSystemElement mockChild_2_2 = MakeSlotSystemElement();
			TestSlotSystemElement mockPar = MakeSlotSystemElement();
			mockChild_1.SetElements(new SlotSystemElement[]{mockChild_1_1, mockChild_1_2});
			mockChild_2.SetElements(new SlotSystemElement[]{mockChild_2_1, mockChild_2_2});
			testSSE.SetElements(new SlotSystemElement[]{mockChild_1, mockChild_2});
			mockPar.SetElements(new SlotSystemElement[]{testSSE});

			System.Action<SlotSystemElement, IList<string>> act = (SlotSystemElement x, IList<string> list) => {
				string concat = "";
				foreach(string s in list){
					concat += s;
				}
				((TestSlotSystemElement)x).message = concat;
			};
			List<string> stringList = new List<string>(new string[]{"per", "formed"});
			testSSE.PerformInHierarchy(act, stringList);

			Assert.That(mockPar.message, Is.Empty);
			Assert.That(testSSE.message, Is.StringContaining("performed"));
			Assert.That(mockChild_1.message, Is.StringContaining("performed"));
			Assert.That(mockChild_1_1.message, Is.StringContaining("performed"));
			Assert.That(mockChild_1_2.message, Is.StringContaining("performed"));
			Assert.That(mockChild_2.message, Is.StringContaining("performed"));
			Assert.That(mockChild_2_1.message, Is.StringContaining("performed"));
			Assert.That(mockChild_2_2.message, Is.StringContaining("performed"));
		}
		[Test]
		[Category("Methods")]
		public void Contains_NoElements_ReturnsFalse(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			SlotSystemElement stubEle = Substitute.For<SlotSystemElement>();
			
			Assert.That(testSSE.Contains(stubEle), Is.False);
		}
		[Test]
		[Category("Methods")]
		public void Contains_NonMember_ReturnsFalse(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			SlotSystemElement stubMember = Substitute.For<SlotSystemElement>();
			SlotSystemElement stubNonMember = Substitute.For<SlotSystemElement>();
			testSSE.SetElements(new SlotSystemElement[]{stubMember});
			
			Assert.That(testSSE.Contains(stubNonMember), Is.False);
		}
		[Test]
		[Category("Methods")]
		public void Contains_Member_ReturnsTrue(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			SlotSystemElement stubMember = Substitute.For<SlotSystemElement>();
			testSSE.SetElements(new SlotSystemElement[]{stubMember});
			
			Assert.That(testSSE.Contains(stubMember), Is.True);
		}
		[Test]
		[Category("Methods")]
		public void ToggleOnPageElement_IsPageElementAndElementNotToggledOn_TogglesPageElementOn(){
			TestSlotSystemElement testSSE = MakeSlotSystemElement();
			SlotSystemPageElement mockPageEle = MakeSlotSystemPageElement(testSSE, false);
			// SlotSystemPage stubPage = Substitute.For<SlotSystemPage>();
			TestSlotSystemPage stubPage = MakeSlotSystemPage();
			testSSE.parent = stubPage;
			// stubPage.GetPageElement(Arg.Any<SlotSystemElement>()).Returns(mockPageEle);
			stubPage.AddPageElement(testSSE, mockPageEle);

			testSSE.ToggleOnPageElement();

			Assert.That(mockPageEle.isFocusToggleOn, Is.True);
		}
	/*	helpers */
		TestSlotSystemElement MakeSlotSystemElement(){
			GameObject sseGO = new GameObject("sseGO");
			return sseGO.AddComponent<TestSlotSystemElement>();
		}
		SlotSystemBundle MakeSlotSystemBundle(){
			return new GameObject("ssBundleGO").AddComponent<SlotSystemBundle>();
		}
		TestSlotSystemPage MakeSlotSystemPage(){
			return new GameObject("pageGO").AddComponent<TestSlotSystemPage>();
		}
		SlotSystemPageElement MakeSlotSystemPageElement(SlotSystemElement ele, bool isFocusToggledOn){
			return new SlotSystemPageElement(ele, isFocusToggledOn);
		}
}
