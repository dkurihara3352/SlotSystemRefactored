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
	public class SwitchableStateEngineTests: SlotSystemTest{
		[TestCaseSource(typeof(SetState_VariousCases))]
		public void SetState_Various_Various(
			ISwitchableState from, 
			ISwitchableState to, 
			ISwitchableState expPrev, 
			ISwitchableState expCur, 
			bool exitCalled, 
			bool enterCalled)
		{
			TestStateEngine engine = MakeTestStateEngine(null, from);

			engine.SetState(to);

			Assert.That(engine.GetCurState(), Is.SameAs(expCur));
			Assert.That(engine.GetPrevState(), Is.SameAs(expPrev));
			if(from != null)
				if(exitCalled)
					from.Received().ExitState();
				else
					from.DidNotReceive().ExitState();
			if(to != null)
				if(enterCalled)
					to.Received().EnterState();
				else
					to.DidNotReceive().EnterState();
				
		}
			class SetState_VariousCases: IEnumerable{
				public IEnumerator GetEnumerator(){
					object[] n_n_n_n_F_F;
						n_n_n_n_F_F = new object[]{
							null, null, null,null, false, false
						};
						yield return n_n_n_n_F_F;
					object[] n_A_n_A_F_T;
						ISwitchableState stateA_1 = Substitute.For<ISwitchableState>();
						n_A_n_A_F_T = new object[]{
							null, stateA_1, null, stateA_1, false, true
						};
						yield return n_A_n_A_F_T;
					object[] A_n_A_n_T_F;
						ISwitchableState stateA_2 = Substitute.For<ISwitchableState>();
						A_n_A_n_T_F = new object[]{
							stateA_2, null, stateA_2, null, true, false
						};
						yield return A_n_A_n_T_F;
					object[] A_A_n_A_F_F;
						ISwitchableState stateA_3 = Substitute.For<ISwitchableState>();
						A_A_n_A_F_F = new object[]{
							stateA_3, stateA_3, null, stateA_3, false, false
						};
						yield return A_A_n_A_F_F;
					object[] A_B_A_B_T_T;
						ISwitchableState stateA_4 = Substitute.For<ISwitchableState>();
						ISwitchableState stateB_4 = Substitute.For<ISwitchableState>();
						A_B_A_B_T_T = new object[]{
							stateA_4, stateB_4, stateA_4, stateB_4, true, true
						};
						yield return A_B_A_B_T_T;		
				}
			}
		/* Helpers */
			TestStateEngine MakeTestStateEngine(ISwitchableState prev, ISwitchableState cur){
				return new TestStateEngine(prev, cur);
			}
			class TestStateEngine: SwitchableStateEngine<ISwitchableState>{
				public void SetTestState(ISwitchableState state){
					SetState(state);
				}
				public TestStateEngine(ISwitchableState prev, ISwitchableState cur){
					_prevState = prev; _curState = cur;
				}
			}
	}
}
