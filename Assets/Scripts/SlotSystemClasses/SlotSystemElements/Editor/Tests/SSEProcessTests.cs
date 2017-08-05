using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using SlotSystem;
namespace SlotSystemTests{
	namespace SSEElementsTests{
		[TestFixture]
		[Category("OtherElements")]
		public class SSEProcessTests: SlotSystemTest {
			[TestCaseSource(typeof(SetAndRunProcess_VariousCases))]
			public void SSEProcessEngine_SetAndRunProcess_Various_Various(
				ISSEProcess from, 
				ISSEProcess to, 
				ISSEProcess expectedProcess,
				bool isStopCalled, 
				bool isStartCalled)
			{
				SSEProcessEngine<ISSEProcess> engine = MakeProcessEngineWithDefaultProc(from);
				
				engine.SetAndRunProcess(to);

				if(from != null)
					if(isStopCalled) 
						from.Received().Stop();
					else 
						from.DidNotReceive().Stop();
				if(to != null)
					if(isStartCalled) 
						to.Received().Start();
					else 
						to.DidNotReceive().Start();
				Assert.That(engine.process, Is.SameAs(expectedProcess));
			}
				class SetAndRunProcess_VariousCases: IEnumerable{ public IEnumerator GetEnumerator(){
					object[] n_n_n_F_F;
						n_n_n_F_F =  new object[]{
							null, null, null, false, false};
						yield return n_n_n_F_F;
					object[] n_A_A_F_T;
							ISSEProcess procA_c2 = Substitute.For<ISSEProcess>();
							procA_c2.Equals(procA_c2).Returns(true);
						n_A_A_F_T = new object[]{
							null, procA_c2, procA_c2, false, true};
						yield return n_A_A_F_T;
					object[] A_A_A_F_F;
							ISSEProcess procA_c3 = Substitute.For<ISSEProcess>();
							procA_c3.Equals(procA_c3).Returns(true);
						A_A_A_F_F = new object[]{
							procA_c3, procA_c3, procA_c3, false, false};
						yield return A_A_A_F_F;
					object[] B_B_B_F_F;
						ISSEProcess procB_c4 = Substitute.For<ISSEProcess>();
						procB_c4.Equals(procB_c4).Returns(true);

						B_B_B_F_F = new object[]{
							procB_c4, procB_c4, procB_c4, false, false};
						yield return B_B_B_F_F;
					object[] A_n_n_T_F;
							ISSEProcess procA_c5 = Substitute.For<ISSEProcess>();
							procA_c5.Equals(procA_c5).Returns(true);
						A_n_n_T_F =  new object[]{
							procA_c5, null, null, true, false};
						yield return A_n_n_T_F;
					object[] A_B_B_T_T;
							ISSEProcess procA_c6 = Substitute.For<ISSEProcess>();
							ISSEProcess procB_c6 = Substitute.For<ISSEProcess>();
								procA_c6.Equals(procB_c6).Returns(false);
								procA_c6.Equals(procA_c6).Returns(true);
								procB_c6.Equals(procA_c6).Returns(false);
								procB_c6.Equals(procB_c6).Returns(true);
						A_B_B_T_T = new object[]{
							procA_c6, procB_c6, procB_c6, true, true};
						yield return A_B_B_T_T;
					}
				}
			[TestCaseSource(typeof(SSEProcess_EqualsCases))]
			public void SSEProcess_Equals_Various_ReturnsAccordingly(ISSEProcess a, ISSEProcess b, bool expected){
				bool actual = a.Equals(b);

				Assert.That(actual, Is.EqualTo(expected));
			}
				class SSEProcess_EqualsCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] dea_dea_T;
							dea_dea_T = new object[]{
								new SSEDeactivateProcess(MakeSubSSE(), FakeCoroutine), 
								new SSEDeactivateProcess(MakeSubSSE(), FakeCoroutine), 
								true
							};
							yield return dea_dea_T;
						object[] dea_def_F;
							dea_def_F = new object[]{
								new SSEDeactivateProcess(MakeSubSSE(), FakeCoroutine), 
								new SSEDefocusProcess(MakeSubSSE(), FakeCoroutine), 
								false
							};
							yield return dea_def_F;
						object[] dea_foc_F;
							dea_foc_F = new object[]{
								new SSEDeactivateProcess(MakeSubSSE(), FakeCoroutine), 
								new SSEFocusProcess(MakeSubSSE(), FakeCoroutine), 
								false
							};
							yield return dea_foc_F;
						object[] dea_sel_F;
							dea_sel_F = new object[]{
								new SSEDeactivateProcess(MakeSubSSE(), FakeCoroutine), 
								new SSESelectProcess(MakeSubSSE(), FakeCoroutine), 
								false
							};
							yield return dea_sel_F;

						object[] foc_dea_F;
							foc_dea_F = new object[]{
								new SSEFocusProcess(MakeSubSSE(), FakeCoroutine), 
								new SSEDeactivateProcess(MakeSubSSE(), FakeCoroutine), 
								false
							};
							yield return foc_dea_F;
						object[] foc_def_F;
							foc_def_F = new object[]{
								new SSEFocusProcess(MakeSubSSE(), FakeCoroutine), 
								new SSEDefocusProcess(MakeSubSSE(), FakeCoroutine), 
								false
							};
							yield return foc_def_F;
						object[] foc_foc_T;
							foc_foc_T = new object[]{
								new SSEFocusProcess(MakeSubSSE(), FakeCoroutine), 
								new SSEFocusProcess(MakeSubSSE(), FakeCoroutine), 
								true
							};
							yield return foc_foc_T;
						object[] foc_sel_F;
							foc_sel_F = new object[]{
								new SSEFocusProcess(MakeSubSSE(), FakeCoroutine), 
								new SSESelectProcess(MakeSubSSE(), FakeCoroutine), 
								false
							};
							yield return foc_sel_F;

						object[] def_dea_F;
							def_dea_F = new object[]{
								new SSEDefocusProcess(MakeSubSSE(), FakeCoroutine), 
								new SSEDeactivateProcess(MakeSubSSE(), FakeCoroutine), 
								false
							};
							yield return def_dea_F;
						object[] def_def_T;
							def_def_T = new object[]{
								new SSEDefocusProcess(MakeSubSSE(), FakeCoroutine), 
								new SSEDefocusProcess(MakeSubSSE(), FakeCoroutine), 
								true
							};
							yield return def_def_T;
						object[] def_foc_F;
							def_foc_F = new object[]{
								new SSEDefocusProcess(MakeSubSSE(), FakeCoroutine), 
								new SSEFocusProcess(MakeSubSSE(), FakeCoroutine), 
								false
							};
							yield return def_foc_F;
						object[] def_sel_F;
							def_sel_F = new object[]{
								new SSEDefocusProcess(MakeSubSSE(), FakeCoroutine), 
								new SSESelectProcess(MakeSubSSE(), FakeCoroutine), 
								false
							};
							yield return def_sel_F;
						
						object[] sel_dea_F;
							sel_dea_F = new object[]{
								new SSESelectProcess(MakeSubSSE(), FakeCoroutine), 
								new SSEDeactivateProcess(MakeSubSSE(), FakeCoroutine), 
								false
							};
							yield return sel_dea_F;
						object[] sel_def_F;
							sel_def_F = new object[]{
								new SSESelectProcess(MakeSubSSE(), FakeCoroutine), 
								new SSEDefocusProcess(MakeSubSSE(), FakeCoroutine), 
								false
							};
							yield return sel_def_F;
						object[] sel_foc_F;
							sel_foc_F = new object[]{
								new SSESelectProcess(MakeSubSSE(), FakeCoroutine), 
								new SSEFocusProcess(MakeSubSSE(), FakeCoroutine), 
								false
							};
							yield return sel_foc_F;
						object[] sel_sel_T;
							sel_sel_T = new object[]{
								new SSESelectProcess(MakeSubSSE(), FakeCoroutine), 
								new SSESelectProcess(MakeSubSSE(), FakeCoroutine), 
								true
							};
							yield return sel_sel_T;
					}
				}
			[Test]
			public void SSEProcess_Fields_ByDefault_AreSetWithDefault(){
				ISSESelStateHandler handler = Substitute.For<ISSESelStateHandler>();
				System.Func<IEnumeratorFake> fakeCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
				SSEProcess proc = new SSEProcess(handler, fakeCoroutine);

				Assert.That(proc.isRunning, Is.False);
				Assert.That(proc.handler, Is.SameAs(handler));
				Assert.That(proc.coroutineFake, Is.SameAs(fakeCoroutine));
			}
			[Test]
			public void SSEProcess_Start_WhenCalled_SetsIsRunningTrueAndCallsCoroutine(){
				ISSESelStateHandler handler = Substitute.For<ISSESelStateHandler>();
				System.Func<IEnumeratorFake> fakeCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
				SSEProcess proc = new SSEProcess(handler, fakeCoroutine);

				proc.Start();

				Assert.That(proc.isRunning, Is.True);
				fakeCoroutine.Received().Invoke();
			}
			[Test]
			public void SSEProcess_Stop_IsRunning_SetsIsRunningFalse(){
				ISSESelStateHandler handler = Substitute.For<ISSESelStateHandler>();
				System.Func<IEnumeratorFake> fakeCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
				SSEProcess proc = new SSEProcess(handler, fakeCoroutine);
				proc.Start();

				proc.Stop();

				Assert.That(proc.isRunning, Is.False);
			}
			[Test]
			public void SSEProcess_Expire_IsRunning_SetsIsRunningFalse(){
				ISSESelStateHandler handler = Substitute.For<ISSESelStateHandler>();
				System.Func<IEnumeratorFake> fakeCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
				SSEProcess proc = new SSEProcess(handler, fakeCoroutine);
				proc.Start();

				proc.Expire();

				Assert.That(proc.isRunning, Is.False);
			}
			/* Helper */
				SSEProcessEngine<ISSEProcess> MakeProcessEngineWithDefaultProc(ISSEProcess from){
					return new SSEProcessEngine<ISSEProcess>(from);
				}
		}
	}
}
