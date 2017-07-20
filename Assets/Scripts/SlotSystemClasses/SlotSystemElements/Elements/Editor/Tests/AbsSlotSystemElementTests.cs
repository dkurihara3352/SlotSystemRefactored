using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
namespace SlotSystemTests{
	namespace SSEElementsTests{
		[TestFixture]
		public class AbsSlotSystemElementTests: AbsSlotSystemTest{
			/*	State and process */
				/* States */
					[Test]
					[Category("State and Process")]
					public void selProcess_ByDefault_IsNull(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						
						Assert.That(testSSE.selProcess, Is.Null);
					}
					/*	SelStates	*/
						[TestCaseSource(typeof(SetSelState_NonNullNorNonSSESelStateCases))][Category("State and Process")]
						[ExpectedException(typeof(System.ArgumentException))]
						public void SetSelState_NonNullNorNonSSESelState_ThrowsException(SSEState state){
							TestSlotSystemElement sse = MakeTestSSE();
							
							sse.SetSelState(state);
						}
							class SetSelState_NonNullNorNonSSESelStateCases: IEnumerable{
								public IEnumerator GetEnumerator(){
									yield return Substitute.For<SSEActState>();
									yield return Substitute.For<SBSelState>();
								}
							}
						[TestCaseSource(typeof(SetSelState_NullOrSSESelStateCases))][Category("State and Process")]
						public void SetSelState_NullOrSSESelState_CallsSSEStateEngineSetState(SSEState state){
							TestSlotSystemElement sse = MakeTestSSE();
								ISSEStateEngine engine = Substitute.For<ISSEStateEngine>();
								sse.SetSelStateEngine(engine);
							
							sse.SetSelState(state);

							engine.Received().SetState(state);
						}
							class SetSelState_NullOrSSESelStateCases: IEnumerable{
								public IEnumerator GetEnumerator(){
									yield return null;
									yield return Substitute.For<SSESelState>();
								}
							}
						[TestCaseSource(typeof(FromToProcCases))][Category("State and Process")]
						public void SetSelState_Various_SetsSelProcAccordingly(SSEState from, SSEState to, ISSEProcess proc){
							TestSlotSystemElement sse = MakeTestSSE();
							sse.SetSelState(from);

							sse.SetSelState(to);

							ISSEProcess actual = sse.selProcess;
							if(proc != null)
								Assert.That(actual, Is.TypeOf(proc.GetType()));
							else
								Assert.That(actual, Is.Null);
						}
							class FromToProcCases: IEnumerable{
								public IEnumerator GetEnumerator(){
									ISlotSystemElement sse = Substitute.For<ISlotSystemElement>();

									SSEState deactivated = AbsSlotSystemElement.deactivatedState;
									SSEState focused = AbsSlotSystemElement.focusedState;
									SSEState defocused = AbsSlotSystemElement.defocusedState;
									SSEState selected = AbsSlotSystemElement.selectedState;
									
									ISSEProcess greyin = new SSEGreyinProcess(sse, FakeCoroutine);
									ISSEProcess greyout = new SSEGreyoutProcess(sse, FakeCoroutine);
									ISSEProcess highlight = new SSEHighlightProcess(sse, FakeCoroutine);
									ISSEProcess dehighlight = new SSEDehighlightProcess(sse, FakeCoroutine);

									yield return new object[]{null, null, null};
									yield return new object[]{null, deactivated, null};
									yield return new object[]{null, focused, null};
									yield return new object[]{null, defocused, null};
									yield return new object[]{null, selected, null};

									yield return new object[]{deactivated, null, null};
									yield return new object[]{deactivated, deactivated, null};
									yield return new object[]{deactivated, focused, null};
									yield return new object[]{deactivated, defocused, null};
									yield return new object[]{deactivated, selected, null};
									
									yield return new object[]{focused, null, null};
									yield return new object[]{focused, deactivated, null};
									yield return new object[]{focused, focused, null};
									yield return new object[]{focused, defocused, greyout};
									yield return new object[]{focused, selected, highlight};
									
									yield return new object[]{defocused, null, null};
									yield return new object[]{defocused, deactivated, null};
									yield return new object[]{defocused, focused, greyin};
									yield return new object[]{defocused, defocused, null};
									yield return new object[]{defocused, selected, highlight};

									yield return new object[]{selected, null, null};
									yield return new object[]{selected, deactivated, null};
									yield return new object[]{selected, focused, dehighlight};
									yield return new object[]{selected, defocused, greyout};
									yield return new object[]{selected, selected, null};
									
								}
							}
						[TestCaseSource(typeof(FromToMethodCases))][Category("State and Process")]
						public void SetSelState_Various_CallsInstantMethods(SSEState from, SSEState to, InstantMethods method){
							TestSlotSystemElement sse = MakeTestSSE();
							string expected = "default";
							switch(method){
								case InstantMethods.none: expected = ""; break;
								case InstantMethods.greyin: expected = "InstantGreyin called"; break;
								case InstantMethods.greyout: expected = "InstantGreyout called"; break;
								case InstantMethods.highlight: expected = "InstantHighlight called"; break;
								default: break;
							}
							sse.SetSelState(from);

							sse.SetSelState(to);
							
							string actual = sse.message;

							Assert.That(actual, Is.StringContaining(expected));
						}
							public enum InstantMethods{none, greyin, greyout, highlight};
							class FromToMethodCases: IEnumerable{
								public IEnumerator GetEnumerator(){
									SSEState deactivated = AbsSlotSystemElement.deactivatedState;
									SSEState focused = AbsSlotSystemElement.focusedState;
									SSEState defocused = AbsSlotSystemElement.defocusedState;
									SSEState selected = AbsSlotSystemElement.selectedState;

									yield return new object[]{null, null, InstantMethods.none};
									yield return new object[]{null, deactivated, InstantMethods.none};
									yield return new object[]{null, focused, InstantMethods.none};
									yield return new object[]{null, defocused, InstantMethods.none};
									yield return new object[]{null, selected, InstantMethods.none};

									yield return new object[]{deactivated, null, InstantMethods.none};
									yield return new object[]{deactivated, deactivated, InstantMethods.none};
									yield return new object[]{deactivated, focused, InstantMethods.greyin};
									yield return new object[]{deactivated, defocused, InstantMethods.greyout};
									yield return new object[]{deactivated, selected, InstantMethods.highlight};
									
									yield return new object[]{focused, null, InstantMethods.none};
									yield return new object[]{focused, deactivated, InstantMethods.none};
									yield return new object[]{focused, focused, InstantMethods.none};
									yield return new object[]{focused, defocused, InstantMethods.none};
									yield return new object[]{focused, selected, InstantMethods.none};
									
									yield return new object[]{defocused, null, InstantMethods.none};
									yield return new object[]{defocused, deactivated, InstantMethods.none};
									yield return new object[]{defocused, focused, InstantMethods.none};
									yield return new object[]{defocused, defocused, InstantMethods.none};
									yield return new object[]{defocused, selected, InstantMethods.none};
									
									yield return new object[]{selected, null, InstantMethods.none};
									yield return new object[]{selected, deactivated, InstantMethods.none};
									yield return new object[]{selected, focused, InstantMethods.none};
									yield return new object[]{selected, defocused, InstantMethods.none};
									yield return new object[]{selected, selected, InstantMethods.none};
								}
							}
						
					/*	Act States	*/
						[TestCaseSource(typeof(SetActState_NonNullNorNonSSEActStateCases))][Category("State and Process")]
						[ExpectedException(typeof(System.ArgumentException))]
						public void SetActState_NonNullNorNonSSEActState_ThrowsException(SSEState state){
							TestSlotSystemElement sse = MakeTestSSE();
							
							sse.SetActState(state);
						}
							class SetActState_NonNullNorNonSSEActStateCases: IEnumerable{
								public IEnumerator GetEnumerator(){
									yield return Substitute.For<SSESelState>();
									yield return Substitute.For<SBActState>();
								}
							}
						[TestCaseSource(typeof(SetActState_NullOrSSESelStateCases))][Category("State and Process")]
						public void SetActState_NullOrSSESelState_CallsSSEStateEngineSetState(SSEState state){
							TestSlotSystemElement sse = MakeTestSSE();
								ISSEStateEngine engine = Substitute.For<ISSEStateEngine>();
								sse.SetActStateEngine(engine);
							
							sse.SetActState(state);

							engine.Received().SetState(state);
						}
							class SetActState_NullOrSSESelStateCases: IEnumerable{
								public IEnumerator GetEnumerator(){
									yield return null;
									yield return Substitute.For<SSEActState>();
								}
							}
				/* Process */
					[TestCaseSource(typeof(SetAndRunSelProcess_NOTNullNorSSESelProcessCases))][Category("State and Process")]
					[ExpectedException(typeof(System.ArgumentException))]
					public void SetAndRunSelProcess_NOTNullNorSSESelProcess_ThrowsException(ISSEProcess process){
						TestSlotSystemElement sse = MakeTestSSE();
						
						sse.SetAndRunSelProcess(process);
					}
						class SetAndRunSelProcess_NOTNullNorSSESelProcessCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return Substitute.For<SSEActProcess>();
								yield return Substitute.For<ISSEProcess>();
							}
						}
					[TestCaseSource(typeof(SetAndRunSelProcess_NullOrSSESelProcessCases))][Category("State and Process")]
					public void SetAndRunSelProcess_NullOrSSESelProcess_CallsSelProcEngineSAR(ISSEProcess process){
						TestSlotSystemElement sse = MakeTestSSE();
							ISSEProcessEngine engine = Substitute.For<ISSEProcessEngine>();
							sse.SetSelProcEngine(engine);
						
						sse.SetAndRunSelProcess(process);

						engine.Received().SetAndRunProcess(process);
					}
						class SetAndRunSelProcess_NullOrSSESelProcessCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return Substitute.For<SSESelProcess>();
								yield return null;
							}
						}
					[TestCaseSource(typeof(SetAndRunActProcess_NOTNullNorSSEActProcessCases))][Category("State and Process")]
					[ExpectedException(typeof(System.ArgumentException))]
					public void SetAndRunActProcess_NOTNullNorSSEActProcess_ThrowsException(ISSEProcess process){
						TestSlotSystemElement sse = MakeTestSSE();
						
						sse.SetAndRunActProcess(process);
					}
						class SetAndRunActProcess_NOTNullNorSSEActProcessCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return Substitute.For<SSESelProcess>();
								yield return Substitute.For<ISSEProcess>();
							}
						}
					[TestCaseSource(typeof(SetAndRunActProcess_NullOrSSEActProcessCases))][Category("State and Process")]
					public void SetAndRunActProcess_NullOrSSEActProcess_CallsActProcEngineSAR(ISSEProcess process){
						TestSlotSystemElement sse = MakeTestSSE();
							ISSEProcessEngine engine = Substitute.For<ISSEProcessEngine>();
							sse.SetActProcEngine(engine);
						
						sse.SetAndRunActProcess(process);

						engine.Received().SetAndRunProcess(process);
					}
						class SetAndRunActProcess_NullOrSSEActProcessCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return Substitute.For<SSEActProcess>();
								yield return null;
							}
						}
			/*	Fields	*/
				[TestCase(1)]
				[TestCase(10)]
				[TestCase(100)]
				[Category("Fields")]
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
				[Category("Fields")]
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
				[Category("Fields")]
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
				[Test][Category("Fields")]
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
				[Category("Fields")]
				public void level_ParentNull_ReturnsZero(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					Assert.That(testSSE.level, Is.EqualTo(0));
				}
				[Test]
				[Category("Fields")]
				public void level_OneParent_ReturnsOne(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubEle = MakeTestSSE();
					testSSE.SetParent(stubEle);

					Assert.That(testSSE.level, Is.EqualTo(1));
				}
				[Test]
				[Category("Fields")]
				public void level_TwoParent_ReturnsTwo(){
					TestSlotSystemElement stubEle_1 = MakeTestSSE();
					TestSlotSystemElement stubEle_2 = MakeTestSSE();
					TestSlotSystemElement testSSE = MakeTestSSE();
					stubEle_2.SetParent(stubEle_1);
					testSSE.SetParent(stubEle_2);

					Assert.That(testSSE.level, Is.EqualTo(2));
				}
				[Test]
				[Category("Fields")]
				public void isBundleElement_ParentIsBundle_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					SlotSystemBundle stubBundle = MakeSlotSystemBundle();
					testSSE.SetParent(stubBundle);

					Assert.That(testSSE.isBundleElement, Is.True);
				}
				[Test]
				[Category("Fields")]
				public void isBundleElement_ParentIsNotBundle_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubSSE = MakeTestSSE();
					testSSE.SetParent(stubSSE);

					Assert.That(testSSE.isBundleElement, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isPageElement_ParentIsPage_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemPage stubPage = Substitute.For<ISlotSystemPage>();
					testSSE.SetParent(stubPage);

					Assert.That(testSSE.isPageElement, Is.True);
				}
				[Test]
				[Category("Fields")]
				public void isPageElement_ParentIsNotPage_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubSSE = MakeTestSSE();
					testSSE.SetParent(stubSSE);

					Assert.That(testSSE.isPageElement, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isToggledOn_ParentNotPage_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubSSE = MakeTestSSE();
					testSSE.SetParent(stubSSE);

					Assert.That(testSSE.isToggledOn, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isToggledOn_ParentIsPageAndElementToggledOn_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemPage stubPage = Substitute.For<ISlotSystemPage>();
					stubPage.GetPageElement(testSSE).isFocusToggleOn.Returns(true);
					testSSE.SetParent(stubPage);

					Assert.That(testSSE.isToggledOn, Is.True);
				}
				[Test]
				[Category("Fields")]
				public void isToggledOn_ParentIsPageAndElementToggledOff_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemPage stubPage = Substitute.For<ISlotSystemPage>();
					stubPage.GetPageElement(testSSE).isFocusToggleOn.Returns(false);
					testSSE.SetParent(stubPage);

					Assert.That(testSSE.isToggledOn, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isFocused_CurSelStateIsFocused_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.SetSelState(AbsSlotSystemElement.focusedState);

					Assert.That(testSSE.isFocused, Is.True);
					Assert.That(testSSE.isDefocused, Is.False);
					Assert.That(testSSE.isDeactivated, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isDefocused_CurSelStateIsDefocused_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.SetSelState(AbsSlotSystemElement.defocusedState);

					Assert.That(testSSE.isFocused, Is.False);
					Assert.That(testSSE.isDefocused, Is.True);
					Assert.That(testSSE.isDeactivated, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isDeactivated_CurSelStateIsDeactivated_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.SetSelState(AbsSlotSystemElement.deactivatedState);

					Assert.That(testSSE.isFocused, Is.False);
					Assert.That(testSSE.isDefocused, Is.False);
					Assert.That(testSSE.isDeactivated, Is.True);
				}
				[Test][Category("Fields")]
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
				[Category("Fields")]
				public void isFocusedInHierarchy_SelfFocusedAndNoParent_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.Focus();

					Assert.That(testSSE.isFocusedInHierarchy, Is.True);
				}
				[Test]
				[Category("Fields")]
				public void isFocusedInHierarchy_SelfNotFocused_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					Assert.That(testSSE.isFocusedInHierarchy, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isFocusedInHierarchy_SelfFocusedParentNotFocused_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubPar = MakeTestSSE();
					testSSE.SetParent(stubPar);
					stubPar.Defocus();
					testSSE.Focus();
					
					Assert.That(testSSE.isFocusedInHierarchy, Is.False);
				}
				[Test]
				[Category("Fields")]
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
					stubPar1.Focus();
					
					Assert.That(testSSE.isFocusedInHierarchy, Is.True);
				}
				[Test]
				[Category("Fields")]
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
				[Category("Methods")]
				public void Initialize_WhenCalled_SelStatesSetToSSEDeactivated(){
						TestSlotSystemElement testSSE = MakeTestSSE();

						testSSE.Initialize();

						Assert.That(testSSE.prevSelState, Is.EqualTo(AbsSlotSystemElement.deactivatedState));
						Assert.That(testSSE.curSelState, Is.EqualTo(AbsSlotSystemElement.deactivatedState));
					}
				//
				[Test][Category("Methods")]
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
				[Category("Methods")]
				[ExpectedException(typeof(System.ArgumentNullException))]
				public void ContainsInHierarchy_Null_ThrowsException(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					testSSE.ContainsInHierarchy(null);
				}
				[Test]
				[Category("Methods")]
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
				[Category("Methods")]
				public void ContainsInHierarchy_DirectChild_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubChild = MakeTestSSE();
					stubChild.SetParent(testSSE);

					bool result = testSSE.ContainsInHierarchy(stubChild);

					Assert.That(result, Is.True);
				}
				[Test]
				[Category("Methods")]
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
				[Category("Methods")]
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
				[Category("Methods")]
				public void Deactivate_WhenCalled_CallsChildrensDeactivate(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement mockChildA = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildB = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildC = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{mockChildA, mockChildB, mockChildC});

					testSSE.Deactivate();

					mockChildA.Received().Deactivate();
					mockChildB.Received().Deactivate();
					mockChildC.Received().Deactivate();
				}
				[Test]
				[Category("Methods")]
				public void Deactivate_WhenCalled_SetCurStateToDeactivated(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					
					testSSE.Deactivate();

					Assert.That(testSSE.curSelState, Is.SameAs(AbsSlotSystemElement.deactivatedState));
				}
				[Test]
				[Category("Methods")]
				public void Focus_WhenCalled_CallsChildrensFocus(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement mockChildA = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildB = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildC = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{mockChildA, mockChildB, mockChildC});

					testSSE.Focus();

					mockChildA.Received().Focus();
					mockChildB.Received().Focus();
					mockChildC.Received().Focus();
				}
				[Test]
				[Category("Methods")]
				public void Focus_WhenCalled_SetCurStateToFocusd(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					
					testSSE.Focus();

					Assert.That(testSSE.curSelState, Is.SameAs(AbsSlotSystemElement.focusedState));
				}
				[Test]
				[Category("Methods")]
				public void Defocus_WhenCalled_CallsChildrensDefocus(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement mockChildA = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildB = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildC = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{mockChildA, mockChildB, mockChildC});

					testSSE.Defocus();

					mockChildA.Received().Defocus();
					mockChildB.Received().Defocus();
					mockChildC.Received().Defocus();
				}
				[Test]
				[Category("Methods")]
				public void Defocus_WhenCalled_SetCurStateToDefocusd(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					
					testSSE.Defocus();

					Assert.That(testSSE.curSelState, Is.SameAs(AbsSlotSystemElement.defocusedState));
				}
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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
								[Test][Category("Methods")]
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
						Assert.That(down_0List, Is.EqualTo(down_0));
					List<ISlotSystemElement> down_0_0List = new List<ISlotSystemElement>();
						sse_0_0.PerformInHierarchy(PutInList, down_0_0List);
						Assert.That(down_0_0List, Is.EqualTo(down_0_0));
					List<ISlotSystemElement> down_0_0_0List = new List<ISlotSystemElement>();
						sse_0_0_0.PerformInHierarchy(PutInList, down_0_0_0List);
						Assert.That(down_0_0_0List, Is.EqualTo(down_0_0_0));
					List<ISlotSystemElement> down_0_0_0_0List = new List<ISlotSystemElement>();
						sse_0_0_0_0.PerformInHierarchy(PutInList, down_0_0_0_0List);
						Assert.That(down_0_0_0_0List, Is.EqualTo(down_0_0_0_0));
					List<ISlotSystemElement> down_0_0_0_0_0List = new List<ISlotSystemElement>();
						sse_0_0_0_0_0.PerformInHierarchy(PutInList, down_0_0_0_0_0List);
						Assert.That(down_0_0_0_0_0List, Is.EqualTo(new ISlotSystemElement[]{sse_0_0_0_0_0}));
					List<ISlotSystemElement> down_0_1List = new List<ISlotSystemElement>();
						sse_0_1.PerformInHierarchy(PutInList, down_0_1List);
						Assert.That(down_0_1List, Is.EqualTo(down_0_1));
					List<ISlotSystemElement> down_0_1_0List = new List<ISlotSystemElement>();
						sse_0_1_0.PerformInHierarchy(PutInList, down_0_1_0List);
						Assert.That(down_0_1_0List, Is.EqualTo(down_0_1_0));
					List<ISlotSystemElement> down_0_1_0_0List = new List<ISlotSystemElement>();
						sse_0_1_0_0.PerformInHierarchy(PutInList, down_0_1_0_0List);
						Assert.That(down_0_1_0_0List, Is.EqualTo(down_0_1_0_0));
					List<ISlotSystemElement> down_0_1_0_0_0List = new List<ISlotSystemElement>();
						sse_0_1_0_0_0.PerformInHierarchy(PutInList, down_0_1_0_0_0List);
						Assert.That(down_0_1_0_0_0List, Is.EqualTo(new ISlotSystemElement[]{sse_0_1_0_0_0}));
				}
					void PutInList<ISlotSystemElement>(ISlotSystemElement ele, IList<ISlotSystemElement> list){
						list.Add(ele);
					}
				[Test]
				[Category("Methods")]
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
				[Category("Methods")]
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
				[Category("Methods")]
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
				[Category("Methods")]
				public void Contains_NoElements_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement stubEle = Substitute.For<ISlotSystemElement>();
					
					Assert.That(testSSE.Contains(stubEle), Is.False);
				}
				[Test]
				[Category("Methods")]
				public void Contains_NonMember_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement stubMember = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement stubNonMember = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{stubMember});
					
					Assert.That(testSSE.Contains(stubNonMember), Is.False);
				}
				[Test]
				[Category("Methods")]
				public void Contains_Member_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement stubMember = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{stubMember});
					
					Assert.That(testSSE.Contains(stubMember), Is.True);
				}
				[Test]
				[Category("Methods")]
				public void ToggleOnPageElement_IsPageElementAndElementNotToggledOn_TogglesPageElementOn(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemPage stubPage = Substitute.For<ISlotSystemPage>();
					ISlotSystemPageElement mockPageEle = Substitute.For<ISlotSystemPageElement>();
					mockPageEle.isFocusToggleOn = false;
					stubPage.GetPageElement(testSSE).Returns(mockPageEle);
					testSSE.SetParent(stubPage);

					testSSE.ToggleOnPageElement();

					mockPageEle.Received().isFocusToggleOn = true;
				}
			/*	helpers */
				TestSlotSystemElement MakeTestSSE(){
					GameObject sseGO = new GameObject("sseGO");
					return sseGO.AddComponent<TestSlotSystemElement>();
				}
				SlotSystemBundle MakeSlotSystemBundle(){
					return new GameObject("ssBundleGO").AddComponent<SlotSystemBundle>();
				}
		}
	}
}
