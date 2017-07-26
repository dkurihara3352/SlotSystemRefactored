using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using Utility;
namespace SlotSystemTests{
	namespace SSEElementsTests{
		[TestFixture]
		[Category("OtherElements")]
		public class SlotSystemElementTests: SlotSystemTest{
			/*	State and process */
				/* States */
					[Test]
					public void SelStateFields_ByDefault_AreSetDefault(){
						TestSlotSystemElement sse = MakeTestSSE();

						Assert.That(sse.isDeactivated, Is.False);
						Assert.That(sse.isFocused, Is.False);
						Assert.That(sse.isDefocused, Is.False);
						Assert.That(sse.isSelected, Is.False);
					}
					[Test]
					public void Deactivate_WhenCalled_SetsCurSelStateDeactivated(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						
						testSSE.Deactivate();

						Assert.That(testSSE.isDeactivated, Is.True);
						Assert.That(testSSE.isDefocused, Is.False);
						Assert.That(testSSE.isFocused, Is.False);
						Assert.That(testSSE.isSelected, Is.False);
						}
					[Test]
					public void Deactivate_IsSelStateInit_DoesNotSetSelProc(){
						TestSlotSystemElement testSSE = MakeTestSSE();

						testSSE.Deactivate();

						Assert.That(testSSE.selProcess, Is.Null);
						}
					[Test]
					public void Deactivate_IsNotSelStateInit_SetsSelProcDeactivateProc(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						testSSE.Defocus();

						testSSE.Deactivate();

						Assert.That(testSSE.selProcess, Is.TypeOf(typeof(SSEDeactivateProcess)));
						}
					[Test]
					public void Focus_WhenCalled_SetsCurSelStateFocused(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						
						testSSE.Focus();

						Assert.That(testSSE.isDeactivated, Is.False);
						Assert.That(testSSE.isDefocused, Is.False);
						Assert.That(testSSE.isFocused, Is.True);
						Assert.That(testSSE.isSelected, Is.False);
						}
					[Test]
					public void Focus_IsSelStateInit_DoesNotSetSelProc(){
						TestSlotSystemElement sse = MakeTestSSE();

						sse.Focus();

						Assert.That(sse.selProcess, Is.Null);
						}
					[Test]
					public void Focus_IsSelStateInit_CallsInstantFocus(){
						TestSlotSystemElement sse = MakeTestSSE();
							ISSECommand mockComm = Substitute.For<ISSECommand>();
							sse.SetInstantFocusCommand(mockComm);

						sse.Focus();

						mockComm.Received().Execute();
						}
					[Test]
					public void Focus_IsNotSelStateInit_SetsSelProcFocus(){
						TestSlotSystemElement sse = MakeTestSSE();
						sse.Deactivate();

						sse.Focus();

						Assert.That(sse.selProcess, Is.TypeOf(typeof(SSEFocusProcess)));
						}
					[Test]
					public void Defocus_WhenCalled_SetCurStateToDefocusd(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						
						testSSE.Defocus();

						Assert.That(testSSE.isDeactivated, Is.False);
						Assert.That(testSSE.isDefocused, Is.True);
						Assert.That(testSSE.isFocused, Is.False);
						Assert.That(testSSE.isSelected, Is.False);
						}
					[Test]
					public void Defocus_IsSelStateInit_DoesNotSetSelProc(){
						TestSlotSystemElement sse = MakeTestSSE();

						sse.Defocus();

						Assert.That(sse.selProcess, Is.Null);
						}
					[Test]
					public void Defocus_IsSelStateInit_CallsInstantDefocus(){
						TestSlotSystemElement sse = MakeTestSSE();
							ISSECommand mockComm = Substitute.For<ISSECommand>();
							sse.SetInstantDefocusCommand(mockComm);

						sse.Defocus();

						mockComm.Received().Execute();
						}
					[Test]
					public void Defocus_IsNotSelStateInit_SetsSelProcDefocus(){
						TestSlotSystemElement sse = MakeTestSSE();
						sse.Deactivate();

						sse.Defocus();

						Assert.That(sse.selProcess, Is.TypeOf(typeof(SSEDefocusProcess)));
						}
					[Test]
					public void Select_WhenCalled_SetCurStateToSelected(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						
						testSSE.Select();

						Assert.That(testSSE.isDeactivated, Is.False);
						Assert.That(testSSE.isDefocused, Is.False);
						Assert.That(testSSE.isFocused, Is.False);
						Assert.That(testSSE.isSelected, Is.True);
						}
					[Test]
					public void Select_IsSelStateInit_DoesNotSetSelProc(){
						TestSlotSystemElement sse = MakeTestSSE();

						sse.Select();

						Assert.That(sse.selProcess, Is.Null);
						}
					[Test]
					public void Select_IsSelStateInit_CallsInstantSelect(){
						TestSlotSystemElement sse = MakeTestSSE();
							ISSECommand mockComm = Substitute.For<ISSECommand>();
							sse.SetInstantSelectCommand(mockComm);

						sse.Select();

						mockComm.Received().Execute();
						}
					[Test]
					public void Select_IsNotSelStateInit_SetsSelProcSelect(){
						TestSlotSystemElement sse = MakeTestSSE();
						sse.Deactivate();

						sse.Select();

						Assert.That(sse.selProcess, Is.TypeOf(typeof(SSESelectProcess)));
						}
				/* Process */
					[TestCaseSource(typeof(SetAndRunSelProcess_ISSESelProcessOrNullCases))]
					public void SetAndRunSelProcess_ISSESelProcessOrNull_CallsSelProcEngineSAR(ISSESelProcess process){
						TestSlotSystemElement sse = MakeTestSSE();
							ISSEProcessEngine<ISSESelProcess> engine = Substitute.For<ISSEProcessEngine<ISSESelProcess>>();
							sse.SetSelProcEngine(engine);
						
						sse.SetAndRunSelProcess(process);

						engine.Received().SetAndRunProcess(process);
						}
						class SetAndRunSelProcess_ISSESelProcessOrNullCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return Substitute.For<ISSESelProcess>();
								yield return null;
							}
						}
			/* Events */
				[Test]
				[ExpectedException(typeof(System.InvalidOperationException))]
				public void OnHoverEnterMock_curSelStateNull_ThrowsException(){
					TestSlotSystemElement sse = MakeTestSSE();

					sse.OnHoverEnterMock();
				}
				[Test]
				public void OnHoverEnterMock_curSelStateNotNull_CallsCurSelStateOnHoverEnterMock(){
					TestSlotSystemElement sse = MakeTestSSE();
					ISSESelState mockState = Substitute.For<ISSESelState>();
					sse.SetDeactivatedState(mockState);
					sse.Deactivate();

					sse.OnHoverEnterMock();

					mockState.Received().OnHoverEnterMock(sse, Arg.Any<PointerEventDataFake>());
				}
				[Test]
				[ExpectedException(typeof(System.InvalidOperationException))]
				public void OnHoverExitMock_curSelStateNull_ThrowsException(){
					TestSlotSystemElement sse = MakeTestSSE();

					sse.OnHoverExitMock();
				}
				[Test]
				public void OnHoverExitMock_curSelStateNotNull_CallsCurSelStateOnHoverExitMock(){
					TestSlotSystemElement sse = MakeTestSSE();
					ISSESelState mockState = Substitute.For<ISSESelState>();
					sse.SetDeactivatedState(mockState);
					sse.Deactivate();

					sse.OnHoverExitMock();

					mockState.Received().OnHoverExitMock(sse, Arg.Any<PointerEventDataFake>());
				}
			/*	Fields	*/
				[TestCase(1)]
				[TestCase(10)]
				[TestCase(100)]
				public void indexer_Valid_ReturnsEnumeratorElement(int count){
					TestSlotSystemElement sse = MakeTestSSE();
						IEnumerable<ISlotSystemElement> elements = CreateSSEs(count);
						List<ISlotSystemElement> list = new List<ISlotSystemElement>(elements);
						sse.SetElements(elements);
					
					for(int i = 0; i< count; i++)
						Assert.That(sse[i], Is.Not.SameAs(list[i]));
					}
				[TestCase(1)]
				[TestCase(10)]
				[TestCase(100)]
				[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
				public void indexer_Invalid_ThrowsException(int count){
					TestSlotSystemElement sse = MakeTestSSE();
						IEnumerable<ISlotSystemElement> elements = CreateSSEs(count);
						sse.SetElements(elements);
					
					ISlotSystemElement invalid = sse[count + 1];
					invalid = null;
					}
				public IEnumerable<ISlotSystemElement> CreateSSEs(int count){
					for(int i = 0; i < count; i++)
						yield return Substitute.For<ISlotSystemElement>();
					}
				
				[Test]
				public void immediateBundle_Various_ReturnsAccordingly(){
					TestSlotSystemElement sse_0 = MakeTestSSE();						
					TestSlotSystemElement sse_1 = MakeTestSSE();
					TestSlotSystemElement sse_2 = MakeTestSSE();
					SlotSystemBundle bundleA = MakeSSBundle();
					TestSlotSystemElement sse_3 = MakeTestSSE();
					SlotSystemBundle bundleB = MakeSSBundle();
					TestSlotSystemElement sse_4 = MakeTestSSE();
					TestSlotSystemElement sse_5 = MakeTestSSE();
						sse_0.SetParent(null);
						sse_1.SetParent(sse_0);
						sse_2.SetParent(sse_1);
						bundleA.SetParent(sse_2);
						sse_3.SetParent(bundleA);
						bundleB.SetParent(sse_3);
						sse_4.SetParent(bundleB);
						sse_5.SetParent(sse_4);
					
					Assert.That(sse_0.immediateBundle, Is.Null);
					Assert.That(sse_1.immediateBundle, Is.Null);
					Assert.That(sse_2.immediateBundle, Is.Null);
					Assert.That(bundleA.immediateBundle, Is.Null);
					Assert.That(sse_3.immediateBundle, Is.SameAs(bundleA));
					Assert.That(bundleB.immediateBundle, Is.SameAs(bundleA));
					Assert.That(sse_4.immediateBundle, Is.SameAs(bundleB));
					Assert.That(sse_5.immediateBundle, Is.SameAs(bundleB));
					}
				[Test]
				public void level_Various_ReturnsAccordingly(){
					TestSlotSystemElement sse_0 = MakeTestSSE();						
					TestSlotSystemElement sse_1 = MakeTestSSE();
					TestSlotSystemElement sse_2 = MakeTestSSE();
					SlotSystemBundle bundleA = MakeSSBundle();
					TestSlotSystemElement sse_3 = MakeTestSSE();
					SlotSystemBundle bundleB = MakeSSBundle();
					TestSlotSystemElement sse_4 = MakeTestSSE();
					TestSlotSystemElement sse_5 = MakeTestSSE();
						sse_0.SetParent(null);
						sse_1.SetParent(sse_0);
						sse_2.SetParent(sse_1);
						bundleA.SetParent(sse_2);
						sse_3.SetParent(bundleA);
						bundleB.SetParent(sse_3);
						sse_4.SetParent(bundleB);
						sse_5.SetParent(sse_4);
					
					Assert.That(sse_0.level, Is.EqualTo(0));
					Assert.That(sse_1.level, Is.EqualTo(1));
					Assert.That(sse_2.level, Is.EqualTo(2));
					Assert.That(bundleA.level, Is.EqualTo(3));
					Assert.That(sse_3.level, Is.EqualTo(4));
					Assert.That(bundleB.level, Is.EqualTo(5));
					Assert.That(sse_4.level, Is.EqualTo(6));
					Assert.That(sse_5.level, Is.EqualTo(7));
					}
				[Test]
				public void level_ParentNull_ReturnsZero(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					Assert.That(testSSE.level, Is.EqualTo(0));
					}
				[Test]
				public void level_OneParent_ReturnsOne(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubEle = MakeTestSSE();
					testSSE.SetParent(stubEle);

					Assert.That(testSSE.level, Is.EqualTo(1));
					}
				[Test]
				public void level_TwoParent_ReturnsTwo(){
					TestSlotSystemElement stubEle_1 = MakeTestSSE();
					TestSlotSystemElement stubEle_2 = MakeTestSSE();
					TestSlotSystemElement testSSE = MakeTestSSE();
					stubEle_2.SetParent(stubEle_1);
					testSSE.SetParent(stubEle_2);

					Assert.That(testSSE.level, Is.EqualTo(2));
					}
				[Test]
				public void isBundleElement_ParentIsBundle_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					SlotSystemBundle stubBundle = MakeSlotSystemBundle();
					testSSE.SetParent(stubBundle);

					Assert.That(testSSE.isBundleElement, Is.True);
					}
				[Test]
				public void isBundleElement_ParentIsNotBundle_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubSSE = MakeTestSSE();
					testSSE.SetParent(stubSSE);

					Assert.That(testSSE.isBundleElement, Is.False);
					}
				[Test]
				public void isFocusedInHierarchy_Various_ReturnsAccordingly(){
					TestSlotSystemElement sseF_0 = MakeTestSSE();
						TestSlotSystemElement sseF_0_0 = MakeTestSSE();
							TestSlotSystemElement sseF_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sseD_0_0_0_0 = MakeTestSSE();
									TestSlotSystemElement sseF_0_0_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sseF_0_0_0_1 = MakeTestSSE();
						TestSlotSystemElement sseD_0_1 = MakeTestSSE();
							TestSlotSystemElement sseF_0_1_0 = MakeTestSSE();
								TestSlotSystemElement sseF_0_1_0_0 = MakeTestSSE();
									TestSlotSystemElement sseF_0_1_0_0_0 = MakeTestSSE();
					sseF_0.SetParent(null);
						sseF_0_0.SetParent(sseF_0);
							sseF_0_0_0.SetParent(sseF_0_0);
								sseD_0_0_0_0.SetParent(sseF_0_0_0);
									sseF_0_0_0_0_0.SetParent(sseD_0_0_0_0);
								sseF_0_0_0_1.SetParent(sseF_0_0_0);
						sseD_0_1.SetParent(sseF_0);
							sseF_0_1_0.SetParent(sseD_0_1);
								sseF_0_1_0_0.SetParent(sseF_0_1_0);
									sseF_0_1_0_0_0.SetParent(sseF_0_1_0_0);
					sseF_0.Focus();
					sseF_0_0.Focus();
					sseF_0_0_0.Focus();
					sseD_0_0_0_0.Defocus();
					sseF_0_0_0_0_0.Focus();
					sseF_0_0_0_1.Focus();
					sseD_0_1.Defocus();
					sseF_0_1_0.Focus();
					sseF_0_1_0_0.Focus();
					sseF_0_1_0_0_0.Focus();
					IEnumerable<ISlotSystemElement> elements = new ISlotSystemElement[0];
					sseF_0.SetElements(elements);
					sseF_0_0.SetElements(elements);
					sseF_0_0_0.SetElements(elements);
					sseD_0_0_0_0.SetElements(elements);
					sseF_0_0_0_0_0.SetElements(elements);
					sseF_0_0_0_1.SetElements(elements);
					sseD_0_1.SetElements(elements);
					sseF_0_1_0.SetElements(elements);
					sseF_0_1_0_0.SetElements(elements);
					sseF_0_1_0_0_0.SetElements(elements);

					Assert.That(sseF_0.isFocusedInHierarchy, Is.True);
					Assert.That(sseF_0_0.isFocusedInHierarchy, Is.True);
					Assert.That(sseF_0_0_0.isFocusedInHierarchy, Is.True);
					Assert.That(sseD_0_0_0_0.isFocusedInHierarchy, Is.False);
					Assert.That(sseF_0_0_0_0_0.isFocusedInHierarchy, Is.False);
					Assert.That(sseF_0_0_0_1.isFocusedInHierarchy, Is.True);
					Assert.That(sseD_0_1.isFocusedInHierarchy, Is.False);
					Assert.That(sseF_0_1_0.isFocusedInHierarchy, Is.False);
					Assert.That(sseF_0_1_0_0.isFocusedInHierarchy, Is.False);
					Assert.That(sseF_0_1_0_0_0.isFocusedInHierarchy, Is.False);
					
					}
				[Test]
				public void isFocusedInHierarchy_SelfFocusedAndNoParent_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.Focus();

					Assert.That(testSSE.isFocusedInHierarchy, Is.True);
					}
				[Test]
				public void isFocusedInHierarchy_SelfNotFocused_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					Assert.That(testSSE.isFocusedInHierarchy, Is.False);
					}
				[Test]
				public void isFocusedInHierarchy_SelfFocusedParentNotFocused_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubPar = MakeTestSSE();
					testSSE.SetParent(stubPar);
					stubPar.Defocus();
					testSSE.Focus();
					
					Assert.That(testSSE.isFocusedInHierarchy, Is.False);
					}
				[Test]
				public void isFocusedInHierarchy_SelfAndAllParentsFocused_ReturnsTrue(){
					TestSlotSystemElement stubPar1 = MakeTestSSE();
					TestSlotSystemElement stubPar2 = MakeTestSSE();
					TestSlotSystemElement stubPar3 = MakeTestSSE();
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.SetParent(stubPar3);
					stubPar3.SetElements(new ISlotSystemElement[]{testSSE});
					stubPar3.SetParent(stubPar2);
					stubPar2.SetElements(new ISlotSystemElement[]{stubPar3});
					stubPar2.SetParent(stubPar1);
					stubPar1.SetElements(new ISlotSystemElement[]{stubPar2});
					stubPar1.PerformInHierarchy(stubPar1.FocusInHi);
					
					Assert.That(testSSE.isFocusedInHierarchy, Is.True);
					}
				[Test]
				public void isFocusedInHierarchy_SomeNonfocusedAncester_ReturnsFalse(){
					TestSlotSystemElement stubPar1 = MakeTestSSE();
					TestSlotSystemElement stubPar2 = MakeTestSSE();
					TestSlotSystemElement stubPar3 = MakeTestSSE();
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.SetParent(stubPar3);
					stubPar3.SetElements(new ISlotSystemElement[]{testSSE});
					stubPar3.SetParent(stubPar2);
					stubPar2.SetElements(new ISlotSystemElement[]{stubPar3});
					stubPar2.SetParent(stubPar1);
					stubPar1.SetElements(new ISlotSystemElement[]{stubPar2});
					stubPar2.Focus();
					
					Assert.That(testSSE.isFocusedInHierarchy, Is.False);
					}
			/*	Methods	*/
				[Test]
				public void SetElements_WhenCalled_SetsElements(){
					TestSlotSystemElement sse = MakeTestSSE();
						TestSlotSystemElement childA = MakeTestSSE();
							childA.transform.SetParent(sse.transform);
						TestSlotSystemElement childB = MakeTestSSE();
							childB.transform.SetParent(sse.transform);
						TestSlotSystemElement childC = MakeTestSSE();
							childC.transform.SetParent(sse.transform);
						IEnumerable<ISlotSystemElement> expectedEles = new ISlotSystemElement[]{childA, childB, childC};
					
					sse.SetElements();

					bool equality = sse.MemberEquals(expectedEles);
					Assert.That(equality, Is.True);
					}
				[Test]
				public void ContainsInHierarchy_Various_ReturnsAccordingly(){
					TestSlotSystemElement sse_0 = MakeTestSSE();
						TestSlotSystemElement sse_0_0 = MakeTestSSE();
							TestSlotSystemElement sse_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_0_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_1 = MakeTestSSE();
						TestSlotSystemElement sse_0_1 = MakeTestSSE();
							TestSlotSystemElement sse_0_1_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_1_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_1_0_0_0 = MakeTestSSE();
					sse_0.SetParent(null);
						sse_0_0.SetParent(sse_0);
							sse_0_0_0.SetParent(sse_0_0);
								sse_0_0_0_0.SetParent(sse_0_0_0);
									sse_0_0_0_0_0.SetParent(sse_0_0_0_0);
								sse_0_0_0_1.SetParent(sse_0_0_0);
						sse_0_1.SetParent(sse_0);
							sse_0_1_0.SetParent(sse_0_1);
								sse_0_1_0_0.SetParent(sse_0_1_0);
									sse_0_1_0_0_0.SetParent(sse_0_1_0_0);
					
					Assert.That(sse_0.ContainsInHierarchy(sse_0_0_0_0_0), Is.True);
						Assert.That(sse_0_0.ContainsInHierarchy(sse_0), Is.False);
						Assert.That(sse_0_0.ContainsInHierarchy(sse_0_1), Is.False);
						Assert.That(sse_0_0.ContainsInHierarchy(sse_0_0_0_0_0), Is.True);
						Assert.That(sse_0_0.ContainsInHierarchy(sse_0_1_0_0), Is.False);
					}
				[Test]
				[ExpectedException(typeof(System.ArgumentNullException))]
				public void ContainsInHierarchy_Null_ThrowsException(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					testSSE.ContainsInHierarchy(null);
					}
				[Test]
				public void ContainsInHierarchy_Self_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					bool result = testSSE.ContainsInHierarchy(testSSE);

					Assert.That(result, Is.False);
					}
				[Test]
				public void ContainsInHierarchy_Parent_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubPar = MakeTestSSE();
					testSSE.SetParent(stubPar);

					bool result = testSSE.ContainsInHierarchy(stubPar);

					Assert.That(result, Is.False);
					}
				[Test]
				public void ContainsInHierarchy_DirectChild_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubChild = MakeTestSSE();
					stubChild.SetParent(testSSE);

					bool result = testSSE.ContainsInHierarchy(stubChild);

					Assert.That(result, Is.True);
					}
				[Test]
				public void ContainsInHierarchy_DistantChild_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubChild = MakeTestSSE();
					TestSlotSystemElement stubGrandChild = MakeTestSSE();
					stubGrandChild.SetParent(stubChild);
					stubChild.SetParent(testSSE);

					bool result = testSSE.ContainsInHierarchy(stubGrandChild);

					Assert.That(result, Is.True);
					}
				[Test]	
				public void Activate_WhenCalled_CallsChildrensActivate(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement mockChildA = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildB = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildC = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{mockChildA, mockChildB, mockChildC});

					testSSE.Activate();

					mockChildA.Received().Activate();
					mockChildB.Received().Activate();
					mockChildC.Received().Activate();
					}
				[Test]
				public void PerformInHierarchyVer1_Various_PerformsAccordingly(){
					TestSlotSystemElement sse_0 = MakeTestSSE();
						TestSlotSystemElement sse_0_0 = MakeTestSSE();
							TestSlotSystemElement sse_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_0_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_1 = MakeTestSSE();
						TestSlotSystemElement sse_0_1 = MakeTestSSE();
							TestSlotSystemElement sse_0_1_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_1_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_1_0_0_0 = MakeTestSSE();
					sse_0.SetParent(null);
						sse_0_0.SetParent(sse_0);
							sse_0_0_0.SetParent(sse_0_0);
								sse_0_0_0_0.SetParent(sse_0_0_0);
									sse_0_0_0_0_0.SetParent(sse_0_0_0_0);
								sse_0_0_0_1.SetParent(sse_0_0_0);
						sse_0_1.SetParent(sse_0);
							sse_0_1_0.SetParent(sse_0_1);
								sse_0_1_0_0.SetParent(sse_0_1_0);
									sse_0_1_0_0_0.SetParent(sse_0_1_0_0);
					sse_0.SetElements(new ISlotSystemElement[]{sse_0_0, sse_0_1});
						sse_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0});
							sse_0_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0_0, sse_0_0_0_1});
								sse_0_0_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0_0_0});
									sse_0_0_0_0_0.SetElements(new ISlotSystemElement[]{});
								sse_0_0_0_1.SetElements(new ISlotSystemElement[]{});
						sse_0_1.SetElements(new ISlotSystemElement[]{sse_0_1_0});
							sse_0_1_0.SetElements(new ISlotSystemElement[]{sse_0_1_0_0});
								sse_0_1_0_0.SetElements(new ISlotSystemElement[]{sse_0_1_0_0_0});
									sse_0_1_0_0_0.SetElements(new ISlotSystemElement[]{});
					IEnumerable<ISlotSystemElement> down_0 = new ISlotSystemElement[]{
						sse_0, 
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1, 
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_0 = new ISlotSystemElement[]{
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<ISlotSystemElement> down_0_0_0 = new ISlotSystemElement[]{
						sse_0_0_0,
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<ISlotSystemElement> down_0_0_0_0 = new ISlotSystemElement[]{
						sse_0_0_0_0, 
						sse_0_0_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1 = new ISlotSystemElement[]{
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1_0 = new ISlotSystemElement[]{
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1_0_0 = new ISlotSystemElement[]{
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					sse_0.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0_0.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0_0_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_0_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0_0_0.PerformInHierarchy(SetHi);
						Assert.That(sse_0_0_0_0_0.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(e != sse_0_0_0_0_0)
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0_1)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_1).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0_1_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_1_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0_0.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0_1_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_1_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0_0_0.PerformInHierarchy(SetHi);
						Assert.That(sse_0_1_0_0_0.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(e != sse_0_1_0_0_0)
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					}
					void SetHi(ISlotSystemElement ele){
						TestSlotSystemElement testEle = (TestSlotSystemElement)ele;
						testEle.message = "Hi";
					}
					void ClearHi(IEnumerable<ISlotSystemElement> eles){
						foreach(ISlotSystemElement ele in eles){
							TestSlotSystemElement testEle = (TestSlotSystemElement)ele;
							testEle.message = "";
						}
					}
				[Test]
				public void PerformInHierarchyVer2_Various_PerformsAccordingly(){
					TestSlotSystemElement sse_0 = MakeTestSSE();
						TestSlotSystemElement sse_0_0 = MakeTestSSE();
							TestSlotSystemElement sse_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_0_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_1 = MakeTestSSE();
						TestSlotSystemElement sse_0_1 = MakeTestSSE();
							TestSlotSystemElement sse_0_1_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_1_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_1_0_0_0 = MakeTestSSE();
					sse_0.SetParent(null);
						sse_0_0.SetParent(sse_0);
							sse_0_0_0.SetParent(sse_0_0);
								sse_0_0_0_0.SetParent(sse_0_0_0);
									sse_0_0_0_0_0.SetParent(sse_0_0_0_0);
								sse_0_0_0_1.SetParent(sse_0_0_0);
						sse_0_1.SetParent(sse_0);
							sse_0_1_0.SetParent(sse_0_1);
								sse_0_1_0_0.SetParent(sse_0_1_0);
									sse_0_1_0_0_0.SetParent(sse_0_1_0_0);
					sse_0.SetElements(new ISlotSystemElement[]{sse_0_0, sse_0_1});
						sse_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0});
							sse_0_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0_0, sse_0_0_0_1});
								sse_0_0_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0_0_0});
									sse_0_0_0_0_0.SetElements(new ISlotSystemElement[]{});
								sse_0_0_0_1.SetElements(new ISlotSystemElement[]{});
						sse_0_1.SetElements(new ISlotSystemElement[]{sse_0_1_0});
							sse_0_1_0.SetElements(new ISlotSystemElement[]{sse_0_1_0_0});
								sse_0_1_0_0.SetElements(new ISlotSystemElement[]{sse_0_1_0_0_0});
									sse_0_1_0_0_0.SetElements(new ISlotSystemElement[]{});
					IEnumerable<ISlotSystemElement> down_0 = new ISlotSystemElement[]{
						sse_0, 
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1, 
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_0 = new ISlotSystemElement[]{
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<ISlotSystemElement> down_0_0_0 = new ISlotSystemElement[]{
						sse_0_0_0,
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<ISlotSystemElement> down_0_0_0_0 = new ISlotSystemElement[]{
						sse_0_0_0_0, 
						sse_0_0_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1 = new ISlotSystemElement[]{
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1_0 = new ISlotSystemElement[]{
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1_0_0 = new ISlotSystemElement[]{
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					sse_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0_0_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_0_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0_0_0.PerformInHierarchy(SetHi, "Hi");
						Assert.That(sse_0_0_0_0_0.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(e != sse_0_0_0_0_0)
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0_1)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_1).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0_1_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_1_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0_1_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_1_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0_0_0.PerformInHierarchy(SetHi, "Hi");
						Assert.That(sse_0_1_0_0_0.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(e != sse_0_1_0_0_0)
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					}
					void SetHi(ISlotSystemElement ele, object str){
						string s = (string)str;
						((TestSlotSystemElement)ele).message = s;
					}
				[Test]
				public void PerformInHierarchyVer3_Various_PerformsAccordingly(){
					TestSlotSystemElement sse_0 = MakeTestSSE();
						TestSlotSystemElement sse_0_0 = MakeTestSSE();
							TestSlotSystemElement sse_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_0_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_1 = MakeTestSSE();
						TestSlotSystemElement sse_0_1 = MakeTestSSE();
							TestSlotSystemElement sse_0_1_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_1_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_1_0_0_0 = MakeTestSSE();
					sse_0.SetParent(null);
						sse_0_0.SetParent(sse_0);
							sse_0_0_0.SetParent(sse_0_0);
								sse_0_0_0_0.SetParent(sse_0_0_0);
									sse_0_0_0_0_0.SetParent(sse_0_0_0_0);
								sse_0_0_0_1.SetParent(sse_0_0_0);
						sse_0_1.SetParent(sse_0);
							sse_0_1_0.SetParent(sse_0_1);
								sse_0_1_0_0.SetParent(sse_0_1_0);
									sse_0_1_0_0_0.SetParent(sse_0_1_0_0);
					sse_0.SetElements(new ISlotSystemElement[]{sse_0_0, sse_0_1});
						sse_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0});
							sse_0_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0_0, sse_0_0_0_1});
								sse_0_0_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0_0_0});
									sse_0_0_0_0_0.SetElements(new ISlotSystemElement[]{});
								sse_0_0_0_1.SetElements(new ISlotSystemElement[]{});
						sse_0_1.SetElements(new ISlotSystemElement[]{sse_0_1_0});
							sse_0_1_0.SetElements(new ISlotSystemElement[]{sse_0_1_0_0});
								sse_0_1_0_0.SetElements(new ISlotSystemElement[]{sse_0_1_0_0_0});
									sse_0_1_0_0_0.SetElements(new ISlotSystemElement[]{});
					IEnumerable<ISlotSystemElement> down_0 = new ISlotSystemElement[]{
						sse_0, 
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1, 
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_0 = new ISlotSystemElement[]{
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<ISlotSystemElement> down_0_0_0 = new ISlotSystemElement[]{
						sse_0_0_0,
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<ISlotSystemElement> down_0_0_0_0 = new ISlotSystemElement[]{
						sse_0_0_0_0, 
						sse_0_0_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1 = new ISlotSystemElement[]{
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1_0 = new ISlotSystemElement[]{
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1_0_0 = new ISlotSystemElement[]{
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					List<ISlotSystemElement> down_0List = new List<ISlotSystemElement>();
						sse_0.PerformInHierarchy(PutInList, down_0List);
						Assert.That(down_0List.MemberEquals(down_0), Is.True);
					List<ISlotSystemElement> down_0_0List = new List<ISlotSystemElement>();
						sse_0_0.PerformInHierarchy(PutInList, down_0_0List);
						Assert.That(down_0_0List.MemberEquals(down_0_0), Is.True);
					List<ISlotSystemElement> down_0_0_0List = new List<ISlotSystemElement>();
						sse_0_0_0.PerformInHierarchy(PutInList, down_0_0_0List);
						Assert.That(down_0_0_0List.MemberEquals(down_0_0_0), Is.True);
					List<ISlotSystemElement> down_0_0_0_0List = new List<ISlotSystemElement>();
						sse_0_0_0_0.PerformInHierarchy(PutInList, down_0_0_0_0List);
						Assert.That(down_0_0_0_0List.MemberEquals(down_0_0_0_0), Is.True);
					List<ISlotSystemElement> down_0_0_0_0_0List = new List<ISlotSystemElement>();
						sse_0_0_0_0_0.PerformInHierarchy(PutInList, down_0_0_0_0_0List);
						Assert.That(down_0_0_0_0_0List.MemberEquals(new ISlotSystemElement[]{sse_0_0_0_0_0}), Is.True);
					List<ISlotSystemElement> down_0_1List = new List<ISlotSystemElement>();
						sse_0_1.PerformInHierarchy(PutInList, down_0_1List);
						Assert.That(down_0_1List.MemberEquals(down_0_1), Is.True);
					List<ISlotSystemElement> down_0_1_0List = new List<ISlotSystemElement>();
						sse_0_1_0.PerformInHierarchy(PutInList, down_0_1_0List);
						Assert.That(down_0_1_0List.MemberEquals(down_0_1_0), Is.True);
					List<ISlotSystemElement> down_0_1_0_0List = new List<ISlotSystemElement>();
						sse_0_1_0_0.PerformInHierarchy(PutInList, down_0_1_0_0List);
						Assert.That(down_0_1_0_0List.MemberEquals(down_0_1_0_0), Is.True);
					List<ISlotSystemElement> down_0_1_0_0_0List = new List<ISlotSystemElement>();
						sse_0_1_0_0_0.PerformInHierarchy(PutInList, down_0_1_0_0_0List);
						Assert.That(down_0_1_0_0_0List.MemberEquals(new ISlotSystemElement[]{sse_0_1_0_0_0}), Is.True);
					}
					void PutInList<ISlotSystemElement>(ISlotSystemElement ele, IList<ISlotSystemElement> list){
						list.Add(ele);
					}
				[Test]
				public void PerformInHierarchyVer1_WhenCalled_PerformActionOnElements(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement mockChild_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_1_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_1_2 = MakeTestSSE();
					TestSlotSystemElement mockChild_2 = MakeTestSSE();
					TestSlotSystemElement mockChild_2_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_2_2 = MakeTestSSE();
					TestSlotSystemElement mockPar = MakeTestSSE();
					mockChild_1.SetElements(new ISlotSystemElement[]{mockChild_1_1, mockChild_1_2});
					mockChild_2.SetElements(new ISlotSystemElement[]{mockChild_2_1, mockChild_2_2});
					testSSE.SetElements(new ISlotSystemElement[]{mockChild_1, mockChild_2});
					mockPar.SetElements(new ISlotSystemElement[]{testSSE});

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
				public void PerformInHierarchyVer2_WhenCalled_PerformActionOnElements(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement mockChild_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_1_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_1_2 = MakeTestSSE();
					TestSlotSystemElement mockChild_2 = MakeTestSSE();
					TestSlotSystemElement mockChild_2_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_2_2 = MakeTestSSE();
					TestSlotSystemElement mockPar = MakeTestSSE();
					mockChild_1.SetElements(new ISlotSystemElement[]{mockChild_1_1, mockChild_1_2});
					mockChild_2.SetElements(new ISlotSystemElement[]{mockChild_2_1, mockChild_2_2});
					testSSE.SetElements(new ISlotSystemElement[]{mockChild_1, mockChild_2});
					mockPar.SetElements(new ISlotSystemElement[]{testSSE});

					System.Action<ISlotSystemElement, object> act = (ISlotSystemElement x, object s) => ((TestSlotSystemElement)x).message = (string)s;
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
				public void PerformInHierarchyVer3_WhenCalled_PerformActionOnElements(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement mockChild_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_1_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_1_2 = MakeTestSSE();
					TestSlotSystemElement mockChild_2 = MakeTestSSE();
					TestSlotSystemElement mockChild_2_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_2_2 = MakeTestSSE();
					TestSlotSystemElement mockPar = MakeTestSSE();
					mockChild_1.SetElements(new ISlotSystemElement[]{mockChild_1_1, mockChild_1_2});
					mockChild_2.SetElements(new ISlotSystemElement[]{mockChild_2_1, mockChild_2_2});
					testSSE.SetElements(new ISlotSystemElement[]{mockChild_1, mockChild_2});
					mockPar.SetElements(new ISlotSystemElement[]{testSSE});

					System.Action<ISlotSystemElement, IList<string>> act = (ISlotSystemElement x, IList<string> list) => {
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
				public void Contains_NoElements_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement stubEle = Substitute.For<ISlotSystemElement>();
					
					Assert.That(testSSE.Contains(stubEle), Is.False);
					}
				[Test]
				public void Contains_NonMember_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement stubMember = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement stubNonMember = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{stubMember});
					
					Assert.That(testSSE.Contains(stubNonMember), Is.False);
					}
				[Test]
				public void Contains_Member_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement stubMember = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{stubMember});
					
					Assert.That(testSSE.Contains(stubMember), Is.True);
					}
			//
				[TestCaseSource(typeof(ActivateRecursivelyCases))]
				public void ActivateRecursively_WhenCalled_FocusSelfAndAllMatchInHi(TestSlotSystemElement sse, IEnumerable<ISlotSystemElement> xOn, IEnumerable<ISlotSystemElement> xOff){
					sse.PerformInHierarchy(sse.SetElementsInHi);
					sse.PerformInHierarchy(sse.SetParentInHi);

					sse.ActivateRecursively();
					
					foreach(ISlotSystemElement ele in xOn)
						Assert.That(ele.isFocused, Is.True);
					foreach(ISlotSystemElement ele in xOff)
						Assert.That(ele.isFocused, Is.False);
					}
					class ActivateRecursivelyCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case0;
								TestSlotSystemElement sse_0 = MakeTestSSE();
									TestSlotSystemElement sse0_0 = MakeTestSSE();//off
										TestSlotSystemElement sse00_0 = MakeTestSSE();
										TestSlotSystemElement sse01_0 = MakeTestSSE();
									TestSlotSystemElement sse1_0 = MakeTestSSE();
										TestSlotSystemElement sse10_0 = MakeTestSSE();
											TestSlotSystemElement sse100_0 = MakeTestSSE();//off
												TestSlotSystemElement sse1000_0 = MakeTestSSE();
											TestSlotSystemElement sse101_0 = MakeTestSSE();
										TestSlotSystemElement sse11_0 = MakeTestSSE();
										sse0_0.transform.SetParent(sse_0.transform);
											sse0_0.isActivatedOnDefault = false;
										sse00_0.transform.SetParent(sse0_0.transform);
										sse01_0.transform.SetParent(sse0_0.transform);
										sse1_0.transform.SetParent(sse_0.transform);
										sse10_0.transform.SetParent(sse1_0.transform);
										sse100_0.transform.SetParent(sse10_0.transform);
											sse100_0.isActivatedOnDefault = false;
										sse1000_0.transform.SetParent(sse100_0.transform);
										sse101_0.transform.SetParent(sse10_0.transform);
										sse11_0.transform.SetParent(sse1_0.transform);
								IEnumerable<ISlotSystemElement> xOn_0 = new ISlotSystemElement[]{
									sse_0,
									sse1_0,
									sse10_0,
									sse101_0,
									sse11_0
								};
								IEnumerable<ISlotSystemElement> xOff_0 = new ISlotSystemElement[]{
									sse0_0,
									sse00_0,
									sse01_0,
									sse100_0,
									sse1000_0,
								};
								case0 = new object[]{sse_0, xOn_0, xOff_0};
								yield return case0;
							object[] case1;
								TestSlotSystemElement sse_1 = MakeTestSSE();//off
									TestSlotSystemElement sse0_1 = MakeTestSSE();
										TestSlotSystemElement sse00_1 = MakeTestSSE();
										TestSlotSystemElement sse01_1 = MakeTestSSE();
									TestSlotSystemElement sse1_1 = MakeTestSSE();
										TestSlotSystemElement sse10_1 = MakeTestSSE();
											TestSlotSystemElement sse100_1 = MakeTestSSE();
												TestSlotSystemElement sse1000_1 = MakeTestSSE();
											TestSlotSystemElement sse101_1 = MakeTestSSE();
										TestSlotSystemElement sse11_1 = MakeTestSSE();
										sse_1.isActivatedOnDefault = false;
										sse0_1.transform.SetParent(sse_1.transform);
										sse00_1.transform.SetParent(sse0_1.transform);
										sse01_1.transform.SetParent(sse0_1.transform);
										sse1_1.transform.SetParent(sse_1.transform);
										sse10_1.transform.SetParent(sse1_1.transform);
										sse100_1.transform.SetParent(sse10_1.transform);
										sse1000_1.transform.SetParent(sse100_1.transform);
										sse101_1.transform.SetParent(sse10_1.transform);
										sse11_1.transform.SetParent(sse1_1.transform);
								IEnumerable<ISlotSystemElement> xOn_1 = new ISlotSystemElement[]{
								};
								IEnumerable<ISlotSystemElement> xOff_1 = new ISlotSystemElement[]{
									sse_1,
									sse1_1,
									sse10_1,
									sse101_1,
									sse11_1,
									sse0_1,
									sse00_1,
									sse01_1,
									sse100_1,
									sse1000_1,
								};
								case1 = new object[]{sse_1, xOn_1, xOff_1};
								yield return case1;
							object[] case2;
								TestSlotSystemElement sse_2 = MakeTestSSE();
									TestSlotSystemElement sse0_2 = MakeTestSSE();
										TestSlotSystemElement sse00_2 = MakeTestSSE();
										TestSlotSystemElement sse01_2 = MakeTestSSE();
									TestSlotSystemElement sse1_2 = MakeTestSSE();
										TestSlotSystemElement sse10_2 = MakeTestSSE();
											TestSlotSystemElement sse100_2 = MakeTestSSE();
												TestSlotSystemElement sse1000_2 = MakeTestSSE();
											TestSlotSystemElement sse101_2 = MakeTestSSE();
										TestSlotSystemElement sse11_2 = MakeTestSSE();
										sse0_2.transform.SetParent(sse_2.transform);
										sse00_2.transform.SetParent(sse0_2.transform);
										sse01_2.transform.SetParent(sse0_2.transform);
										sse1_2.transform.SetParent(sse_2.transform);
										sse10_2.transform.SetParent(sse1_2.transform);
										sse100_2.transform.SetParent(sse10_2.transform);
										sse1000_2.transform.SetParent(sse100_2.transform);
										sse101_2.transform.SetParent(sse10_2.transform);
										sse11_2.transform.SetParent(sse1_2.transform);
								IEnumerable<ISlotSystemElement> xOn_2 = new ISlotSystemElement[]{
									sse_2,
									sse0_2,
									sse00_2,
									sse01_2,
									sse1_2,
									sse10_2,
									sse100_2,
									sse1000_2,
									sse101_2,
									sse11_2
								};
								IEnumerable<ISlotSystemElement> xOff_2 = new ISlotSystemElement[]{
								};
								case2 = new object[]{sse_2, xOn_2, xOff_2};
								yield return case2;
							object[] case3;
								TestSlotSystemElement sse_3 = MakeTestSSE();
									TestSlotSystemElement sse0_3 = MakeTestSSE();
										TestSlotSystemElement sse00_3 = MakeTestSSE();
										TestSlotSystemElement sse01_3 = MakeTestSSE();
									TestSlotSystemElement sse1_3 = MakeTestSSE();//off
										TestSlotSystemElement sse10_3 = MakeTestSSE();//on
											TestSlotSystemElement sse100_3 = MakeTestSSE();//off
												TestSlotSystemElement sse1000_3 = MakeTestSSE();
											TestSlotSystemElement sse101_3 = MakeTestSSE();
										TestSlotSystemElement sse11_3 = MakeTestSSE();
										sse0_3.transform.SetParent(sse_3.transform);
										sse00_3.transform.SetParent(sse0_3.transform);
										sse01_3.transform.SetParent(sse0_3.transform);
										sse1_3.transform.SetParent(sse_3.transform);
											sse1_3.isActivatedOnDefault = false;
										sse10_3.transform.SetParent(sse1_3.transform);
											sse10_3.isActivatedOnDefault = true;
										sse100_3.transform.SetParent(sse10_3.transform);
											sse100_3.isActivatedOnDefault = false;
										sse1000_3.transform.SetParent(sse100_3.transform);
										sse101_3.transform.SetParent(sse10_3.transform);
										sse11_3.transform.SetParent(sse1_3.transform);
								IEnumerable<ISlotSystemElement> xOn_3 = new ISlotSystemElement[]{
									sse_3,
									sse0_3,
									sse00_3,
									sse01_3
								};
								IEnumerable<ISlotSystemElement> xOff_3 = new ISlotSystemElement[]{
									sse1_3,
									sse10_3,
									sse100_3,
									sse1000_3,
									sse101_3,
									sse11_3
								};
								case3 = new object[]{sse_3, xOn_3, xOff_3};
								yield return case3;
						}
					}
			/*	helpers */
				SlotSystemBundle MakeSlotSystemBundle(){
					return new GameObject("ssBundleGO").AddComponent<SlotSystemBundle>();
				}
		}
	}
}
