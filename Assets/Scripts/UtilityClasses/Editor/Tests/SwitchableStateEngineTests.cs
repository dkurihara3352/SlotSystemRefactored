using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Utility;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
namespace UtilityClassTests{
	[TestFixture]
	[Category("Utility")]
	public class SwitchableStateEngineTests: AbsSlotSystemTest{
		[TestCaseSource(typeof(SetState_VariousCases))]
		public void SetState_Various_Various(
			SwitchableState from, 
			SwitchableState to, 
			SwitchableState expPrev, 
			SwitchableState expCur, 
			bool exitCalled, 
			bool enterCalled)
		{
			IStateHandler handler = Substitute.For<IStateHandler>();
			TestStateEngine engine = MakeTestStateEngine(null, from, handler);

			engine.SetState(to);

			Assert.That(engine.curState, Is.SameAs(expCur));
			Assert.That(engine.prevState, Is.SameAs(expPrev));
			if(from != null)
				if(exitCalled)
					from.Received().ExitState(handler);
				else
					from.DidNotReceive().ExitState(handler);
			if(to != null)
				if(enterCalled)
					to.Received().EnterState(handler);
				else
					to.DidNotReceive().EnterState(handler);
				
		}
			class SetState_VariousCases: IEnumerable{
				public IEnumerator GetEnumerator(){
					SwitchableState stateA = Substitute.For<SwitchableState>();
					SwitchableState stateB = Substitute.For<SwitchableState>();
					yield return SetState_VariousCase(
						from: null, to: null, 
						expPrev: null, expCur: null, 
						exitCalled: false, enterCalled: false
					);
					yield return SetState_VariousCase(
						from: null, to: stateA, 
						expPrev: null, expCur: stateA, 
						exitCalled: false, enterCalled: true
					);
					yield return SetState_VariousCase(
						from: stateA, to: null, 
						expPrev: stateA, expCur: null, 
						exitCalled: true, enterCalled: false
					);
					yield return SetState_VariousCase(
						from: stateA, to: stateA, 
						expPrev: null, expCur: stateA, 
						exitCalled: false, enterCalled: false
					);
					yield return SetState_VariousCase(
						from: stateA, to: stateB, 
						expPrev: stateA, expCur: stateB, 
						exitCalled: true, enterCalled: true
					);					
				}
			}
			static object[] SetState_VariousCase(SwitchableState from, SwitchableState to, SwitchableState expPrev, SwitchableState expCur, bool exitCalled, bool enterCalled){
				return new object[]{from, to, expPrev, expCur, exitCalled, enterCalled};
			}
		/* Helpers */
			TestStateEngine MakeTestStateEngine(SwitchableState prev, SwitchableState cur, IStateHandler handler){
				return new TestStateEngine(prev, cur, handler);
			}
			class TestStateEngine: SwitchableStateEngine{
				public void SetTestState(SwitchableState state){
					SetState(state);
				}
				public TestStateEngine(SwitchableState prev, SwitchableState cur){
					m_prevState = prev; m_curState = cur;
					handler = Substitute.For<IStateHandler>();
				}
				public TestStateEngine(SwitchableState prev, SwitchableState cur, IStateHandler handler){
					m_prevState = prev; m_curState = cur;
					this.handler = handler;
				}
			}
	}
}
