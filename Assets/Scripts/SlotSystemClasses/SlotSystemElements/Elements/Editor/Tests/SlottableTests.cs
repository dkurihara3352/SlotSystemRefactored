using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections;
using System.Collections.Generic;
using System;
using Utility;
using NSubstitute;

namespace SlotSystemTests{
	namespace ElementsTests{
		[TestFixture]
		public class SlottableTests: AbsSlotSystemTest {
			/*	States*/
				/*	SelState	*/
					[Test]
					public void SetSelState_Null_CallsSelStateEngineSetState(){
						Slottable sb = MakeSB();
						SSEStateEngine mockEngine = MakeSubSSEStateEngine(sb);
						sb.selStateEngine = mockEngine;
						sb.SetSelState(null);

						mockEngine.Received().SetState(null);
					}
					[Test]
					public void SetSelState_SBSelState_CallsSelStateEngineSetState(){
						Slottable sb = MakeSB();
						SSEStateEngine mockEngine = MakeSubSSEStateEngine(sb);
						sb.selStateEngine = mockEngine;
						SBSelState stubSelState = MakeSubSBSelState();
						sb.SetSelState(stubSelState);

						mockEngine.Received().SetState(stubSelState);
					}
					[TestCaseSource(typeof(SetSelStateInvalidStateCases))]
					public void SetSelState_InvalidState_ThrowsException(SSEState state){
						Slottable sb = MakeSB();
						SSEStateEngine mockEngine = MakeSubSSEStateEngine(sb);
						sb.selStateEngine = mockEngine;

						Exception ex = Assert.Catch<ArgumentException>(() => sb.SetSelState(state));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetSelState: something other than SBSelState is beint attempted to be assigned"));
					}
						class SetSelStateInvalidStateCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBActState();
								yield return MakeSubSBEqpState();
								yield return MakeSubSBMrkState();
								yield return MakeSubSGSelState();
								yield return MakeSubSGActState();
								yield return MakeSubSSMSelState();
								yield return MakeSubSSMActState();
							}
						}
				/*	ActState	*/
					[Test]
					public void SetActState_Null_CallsActStateEngineSetState(){
						Slottable sb = MakeSB();
						SSEStateEngine mockEngine = MakeSubSSEStateEngine(sb);
						sb.actStateEngine = mockEngine;
						sb.SetActState(null);

						mockEngine.Received().SetState(null);
					}
					[Test]
					public void SetActState_SBActState_CallsActStateEngineSetState(){
						Slottable sb = MakeSB();
						SSEStateEngine mockEngine = MakeSubSSEStateEngine(sb);
						sb.actStateEngine = mockEngine;
						SBActState stubActState = MakeSubSBActState();
						sb.SetActState(stubActState);

						mockEngine.Received().SetState(stubActState);
					}
					[TestCaseSource(typeof(SetActStateInvalidStateCases))]
					public void SetActState_InvalidState_ThrowsException(SSEState state){
						Slottable sb = MakeSB();
						SSEStateEngine mockEngine = MakeSubSSEStateEngine(sb);
						sb.actStateEngine = mockEngine;

						Exception ex = Assert.Catch<ArgumentException>(() => sb.SetActState(state));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetActState: something other than SBActionState is being attempted to be assigned"));
					}
						class SetActStateInvalidStateCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBSelState();
								yield return MakeSubSBEqpState();
								yield return MakeSubSBMrkState();
								yield return MakeSubSGSelState();
								yield return MakeSubSGActState();
								yield return MakeSubSSMSelState();
								yield return MakeSubSSMActState();
							}
						}
				/*	EqpState	*/
					[Test]
					public void SetEqpState_Null_CallsEqpStateEngineSetState(){
						Slottable sb = MakeSB();
						SSEStateEngine mockEngine = MakeSubSSEStateEngine(sb);
						sb.eqpStateEngine = mockEngine;
						sb.SetEqpState(null);

						mockEngine.Received().SetState(null);
					}
					[Test]
					public void SetEqpState_SBEqpState_CallsEqpStateEngineSetState(){
						Slottable sb = MakeSB();
						SSEStateEngine mockEngine = MakeSubSSEStateEngine(sb);
						sb.eqpStateEngine = mockEngine;
						SBEqpState stubEqpState = MakeSubSBEqpState();
						sb.SetEqpState(stubEqpState);

						mockEngine.Received().SetState(stubEqpState);
					}
					[TestCaseSource(typeof(SetEqpStateInvalidStateCases))]
					public void SetEqpState_InvalidState_ThrowsException(SSEState state){
						Slottable sb = MakeSB();
						SSEStateEngine mockEngine = MakeSubSSEStateEngine(sb);
						sb.eqpStateEngine = mockEngine;

						Exception ex = Assert.Catch<ArgumentException>(() => sb.SetEqpState(state));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetEqpState: something other than SBEqpState is trying to be assinged"));
					}
						class SetEqpStateInvalidStateCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBSelState();
								yield return MakeSubSBActState();
								yield return MakeSubSBMrkState();
								yield return MakeSubSGSelState();
								yield return MakeSubSGActState();
								yield return MakeSubSSMSelState();
								yield return MakeSubSSMActState();
							}
						}
				/*	EqpState	*/
					[Test]
					public void SetMrkState_Null_CallsMrkStateEngineSetState(){
						Slottable sb = MakeSB();
						SSEStateEngine mockEngine = MakeSubSSEStateEngine(sb);
						sb.mrkStateEngine = mockEngine;
						sb.SetMrkState(null);

						mockEngine.Received().SetState(null);
					}
					[Test]
					public void SetMrkState_SBMrkState_CallsMrkStateEngineSetState(){
						Slottable sb = MakeSB();
						SSEStateEngine mockEngine = MakeSubSSEStateEngine(sb);
						sb.mrkStateEngine = mockEngine;
						SBMrkState stubMrkState = MakeSubSBMrkState();
						sb.SetMrkState(stubMrkState);

						mockEngine.Received().SetState(stubMrkState);
					}
					[TestCaseSource(typeof(SetMrkStateInvalidStateCases))]
					public void SetMrkState_InvalidState_ThrowsException(SSEState state){
						Slottable sb = MakeSB();
						SSEStateEngine mockEngine = MakeSubSSEStateEngine(sb);
						sb.mrkStateEngine = mockEngine;

						Exception ex = Assert.Catch<ArgumentException>(() => sb.SetMrkState(state));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetMrkState: something other than SBMrkState is trying to be assinged"));
					}
						class SetMrkStateInvalidStateCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBSelState();
								yield return MakeSubSBActState();
								yield return MakeSubSBEqpState();
								yield return MakeSubSGSelState();
								yield return MakeSubSGActState();
								yield return MakeSubSSMSelState();
								yield return MakeSubSSMActState();
							}
						}
			/*	Process	*/
				/*	SelProc	*/
					[Test]
					public void SetAndRunSelState_Null_CallsEngineWithNull(){
						Slottable sb = MakeSB();
						SSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.selProcEngine = mockEngine;
						sb.SetAndRunSelProcess(null);

						mockEngine.Received().SetAndRunProcess(null);
					}
					[Test]
					public void SetAndRunSelProcess_SBSelProcess_CallsSelProcEngineWithTheProc(){
						Slottable sb = MakeSB();
						SSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.selProcEngine = mockEngine;
						SBSelProcess stubProc = MakeSubSBSelProc();
						sb.SetAndRunSelProcess(stubProc);

						mockEngine.Received().SetAndRunProcess(stubProc);
					}
					[TestCaseSource(typeof(SetAndRunSelProcessInvalidProcessCases))]
					public void SetAndRunSelProcess_InvalidProcess_ThrowsException(SSEProcess proc){
						Slottable sb = MakeSB();
						SSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.selProcEngine = mockEngine;
						
						Exception ex = Assert.Catch<ArgumentException>( () => sb.SetAndRunSelProcess(proc));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetAndRunSelProcess: argument is not of type SBSelProcess"));
					}
						class SetAndRunSelProcessInvalidProcessCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBActProc();
								yield return MakeSubSBEqpProc();
								yield return MakeSubSBMrkProc();
								yield return MakeSubSGSelProc();
								yield return MakeSubSGActProc();
								yield return MakeSubSSMSelProc();
								yield return MakeSubSSMActProc();
							}
						}
				/*	ActProc	*/
					[Test]
					public void SetAndRunActState_Null_CallsEngineWithNull(){
						Slottable sb = MakeSB();
						SSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.actProcEngine = mockEngine;
						sb.SetAndRunActProcess(null);

						mockEngine.Received().SetAndRunProcess(null);
					}
					[Test]
					public void SetAndRunActProcess_SBActProcess_CallsActProcEngineWithTheProc(){
						Slottable sb = MakeSB();
						SSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.actProcEngine = mockEngine;
						SBActProcess stubProc = MakeSubSBActProc();
						sb.SetAndRunActProcess(stubProc);

						mockEngine.Received().SetAndRunProcess(stubProc);
					}
					[TestCaseSource(typeof(SetAndRunActProcessInvalidProcessCases))]
					public void SetAndRunActProcess_InvalidProcess_ThrowsException(SSEProcess proc){
						Slottable sb = MakeSB();
						SSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.actProcEngine = mockEngine;
						
						Exception ex = Assert.Catch<ArgumentException>( () => sb.SetAndRunActProcess(proc));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetAndRunActProcess: argument is not of type SBActProcess"));
					}
						class SetAndRunActProcessInvalidProcessCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBSelProc();
								yield return MakeSubSBEqpProc();
								yield return MakeSubSBMrkProc();
								yield return MakeSubSGSelProc();
								yield return MakeSubSGActProc();
								yield return MakeSubSSMSelProc();
								yield return MakeSubSSMActProc();
							}
						}
				/*	EqpProc	*/
					[Test]
					public void SetAndRunEqpState_Null_CallsEngineWithNull(){
						Slottable sb = MakeSB();
						SSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.eqpProcEngine = mockEngine;
						sb.SetAndRunEqpProcess(null);

						mockEngine.Received().SetAndRunProcess(null);
					}
					[Test]
					public void SetAndRunEqpProcess_SBEqpProcess_CallsEqpProcEngineWithTheProc(){
						Slottable sb = MakeSB();
						SSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.eqpProcEngine = mockEngine;
						SBEqpProcess stubProc = MakeSubSBEqpProc();
						sb.SetAndRunEqpProcess(stubProc);

						mockEngine.Received().SetAndRunProcess(stubProc);
					}
					[TestCaseSource(typeof(SetAndRunEqpProcessInvalidProcessCases))]
					public void SetAndRunEqpProcess_InvalidProcess_ThrowsException(SSEProcess proc){
						Slottable sb = MakeSB();
						SSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.eqpProcEngine = mockEngine;
						
						Exception ex = Assert.Catch<ArgumentException>( () => sb.SetAndRunEqpProcess(proc));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetAndRunEquipProcess: argument is not of type SBEqpProcess"));
					}
						class SetAndRunEqpProcessInvalidProcessCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBSelProc();
								yield return MakeSubSBActProc();
								yield return MakeSubSBMrkProc();
								yield return MakeSubSGSelProc();
								yield return MakeSubSGActProc();
								yield return MakeSubSSMSelProc();
								yield return MakeSubSSMActProc();
							}
						}
				/*	MrkProc	*/
					[Test]
					public void SetAndRunMrkState_Null_CallsEngineWithNull(){
						Slottable sb = MakeSB();
						SSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.mrkProcEngine = mockEngine;
						sb.SetAndRunMrkProcess(null);

						mockEngine.Received().SetAndRunProcess(null);
					}
					[Test]
					public void SetAndRunMrkProcess_SBMrkProcess_CallsMrkProcEngineWithTheProc(){
						Slottable sb = MakeSB();
						SSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.mrkProcEngine = mockEngine;
						SBMrkProcess stubProc = MakeSubSBMrkProc();
						sb.SetAndRunMrkProcess(stubProc);

						mockEngine.Received().SetAndRunProcess(stubProc);
					}
					[TestCaseSource(typeof(SetAndRunMrkProcessInvalidProcessCases))]
					public void SetAndRunMrkProcess_InvalidProcess_ThrowsException(SSEProcess proc){
						Slottable sb = MakeSB();
						SSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.mrkProcEngine = mockEngine;
						
						Exception ex = Assert.Catch<ArgumentException>( () => sb.SetAndRunMrkProcess(proc));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetAndRunEquipProcess: argument is not of type SBMrkProcess"));
					}
						class SetAndRunMrkProcessInvalidProcessCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBSelProc();
								yield return MakeSubSBActProc();
								yield return MakeSubSBEqpProc();
								yield return MakeSubSGSelProc();
								yield return MakeSubSGActProc();
								yield return MakeSubSSMSelProc();
								yield return MakeSubSSMActProc();
							}
						}

			/*	Fields	*/
			/*	helper	*/
			private static Slottable MakeSB(){
				GameObject sbGO = new GameObject("sbGO");
				Slottable sb = sbGO.AddComponent<Slottable>();
				return sb;
			}
			SSEStateEngine MakeSubSSEStateEngine(SlotSystemElement sse){
				return Substitute.For<SSEStateEngine>(sse);
			}
			SSEProcessEngine MakeSubSSEProcEngine(){
				return Substitute.For<SSEProcessEngine>();
			}
		}
	}
}
