using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Utility;
using NSubstitute;
namespace UtilityClassTests{
	[TestFixture]
	public class SwitchableStateEngineTests{
		[Test]
		public void SetState_ByDefault_SetsBothPrevAndCurState(){
			TestStateEngine testEngine = MakeTestStateEngine(null, null);
			TestSwitchableState fakeState = MakeTestState();

			testEngine.SetTestState(fakeState);

			Assert.That(testEngine.prevState, Is.EqualTo(fakeState));
			Assert.That(testEngine.curState, Is.EqualTo(fakeState));
		}
		[Test]
		public void SetState_SettingDifferentState_UpdatesPrevAndCurStates(){
			TestSwitchableState fakeStateA = MakeTestState(); 
			TestStateEngine testEngine = MakeTestStateEngine(null, fakeStateA);
			TestSwitchableState fakeStateB = MakeTestState();
			
			testEngine.SetTestState(fakeStateB);

			Assert.That(testEngine.prevState, Is.EqualTo(fakeStateA));
			Assert.That(testEngine.curState, Is.EqualTo(fakeStateB));
		}
		[Test]
		public void SetState_SettingSameState_IgnoresUpdate(){
			TestSwitchableState fakeStateA = MakeTestState(); 
			TestStateEngine testEngine = MakeTestStateEngine(null, fakeStateA);
			
			testEngine.SetTestState(fakeStateA);

			Assert.That(testEngine.prevState, Is.Null);
			Assert.That(testEngine.curState, Is.EqualTo(fakeStateA));
		}
		[Test]
		public void SetState_SettingSomeStateToNull_SetsCurStateNullAndUpdatePrev(){
			TestSwitchableState fakeStateA = MakeTestState(); 
			TestStateEngine testEngine = MakeTestStateEngine(null, fakeStateA);
			
			testEngine.SetTestState(null);

			Assert.That(testEngine.prevState, Is.EqualTo(fakeStateA));
			Assert.That(testEngine.curState, Is.Null);
		}
		[Test]
		public void SetState_SettingSomeStateToNullTwice_ClearsStates(){
			TestSwitchableState fakeStateA = MakeTestState(); 
			TestStateEngine testEngine = MakeTestStateEngine(null, fakeStateA);
			
			testEngine.SetTestState(null);
			testEngine.SetTestState(null);

			Assert.That(testEngine.prevState, Is.Null);
			Assert.That(testEngine.curState, Is.Null);
		}
		[Test]
		public void SetState_SettingNonNull_CallsEnterStateOnCurState(){
			TestStateEngine testEngine = MakeTestStateEngine(null, null);
			SwitchableState mockState = Substitute.For<SwitchableState>();

			testEngine.SetTestState(mockState);

			testEngine.curState.Received().EnterState(Arg.Any<IStateHandler>());
		}
		[Test]
		public void SetState_SettingDifferentState_CallsExitStateOnPrevState(){
			TestStateEngine testEngine = MakeTestStateEngine(null, null);
			SwitchableState stubState = Substitute.For<SwitchableState>();
			SwitchableState mockState = Substitute.For<SwitchableState>();

			testEngine.SetTestState(mockState);
			testEngine.SetTestState(stubState);

			testEngine.prevState.Received().ExitState(Arg.Any<IStateHandler>());
		}
		[Test]
		public void SetState_SettingDifferentState_CallsEnterStateOnCurState(){
			TestStateEngine testEngine = MakeTestStateEngine(null, null);
			SwitchableState stubState = Substitute.For<SwitchableState>();
			SwitchableState mockState = Substitute.For<SwitchableState>();

			testEngine.SetTestState(stubState);
			testEngine.SetTestState(mockState);

			testEngine.curState.Received().EnterState(Arg.Any<IStateHandler>());
		}
		[Test]
		public void SetState_SettingSomeStateToNull_CallsExitStateOnPrev(){
			SwitchableState mockState = Substitute.For<SwitchableState>();
			TestStateEngine testEngine = MakeTestStateEngine(null, mockState);

			testEngine.SetTestState(null);

			testEngine.prevState.Received().ExitState(Arg.Any<IStateHandler>());
		}
		[Test]
		public void SetState_SettingSameState_DoesNotCallStates(){
			SwitchableState mockPrevState = Substitute.For<SwitchableState>();
			SwitchableState mockCurState = Substitute.For<SwitchableState>();
			TestStateEngine testStateEngine = MakeTestStateEngine(mockPrevState, mockCurState);

			testStateEngine.SetTestState(mockCurState);

			testStateEngine.prevState.DidNotReceive().ExitState(Arg.Any<IStateHandler>());
			testStateEngine.curState.DidNotReceive().EnterState(Arg.Any<IStateHandler>());
			testStateEngine.curState.DidNotReceive().ExitState(Arg.Any<IStateHandler>());
		}
		TestStateEngine MakeTestStateEngine(SwitchableState prev, SwitchableState cur){
			return new TestStateEngine(prev, cur);
		}
		TestSwitchableState MakeTestState(){
			return new TestSwitchableState();
		}
		class TestStateEngine: SwitchableStateEngine{
			public void SetTestState(SwitchableState state){
				SetState(state);
			}
			public TestStateEngine(SwitchableState prev, SwitchableState cur){
				m_prevState = prev; m_curState = cur;
				handler = Substitute.For<IStateHandler>();
			}
		}
		class TestSwitchableState: SwitchableState{
			public void EnterState(IStateHandler handler){}
			public void ExitState(IStateHandler handler){}
		}
	}
}
