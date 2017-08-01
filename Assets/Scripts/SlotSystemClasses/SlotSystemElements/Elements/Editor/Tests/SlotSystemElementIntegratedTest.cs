using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using Utility;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SlotSystemTests{
	[TestFixture]
	[Category("Integration")]
	public class SlotSystemElementIntegratedTest: SlotSystemTest {
		[Test]
		public void Deactivate_FromNonNullNorDea_SetsDeactivatedProcAndCallsStart(){
			TestSlotSystemElement sse = MakeTestSSE();
				System.Func<IEnumeratorFake> mockDeaCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
				ISSECoroutineFactory stubCorFactory = Substitute.For<ISSECoroutineFactory>();
					stubCorFactory.MakeDeactivateCoroutine().Returns(mockDeaCoroutine);
				sse.SetCoroutineFactory(stubCorFactory);
			sse.Defocus();

			sse.Deactivate();

			AssertSSESelProcIsSetAndIsRunning(sse, typeof(SSEDeactivateProcess), mockDeaCoroutine);
		}
		[Test]
		public void Defocus_FromNonNullNorDef_SetsDefocusdProcAndCallsStart(){
			TestSlotSystemElement sse = MakeTestSSE();
				System.Func<IEnumeratorFake> mockDefCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
				ISSECoroutineFactory stubCorFactory = Substitute.For<ISSECoroutineFactory>();
					stubCorFactory.MakeDefocusCoroutine().Returns(mockDefCoroutine);
				sse.SetCoroutineFactory(stubCorFactory);
			sse.Deactivate();

			sse.Defocus();

			AssertSSESelProcIsSetAndIsRunning(sse, typeof(SSEDefocusProcess), mockDefCoroutine);
		}
		[Test]
		public void Focus_FromNonNullNorFoc_SetsFocusdProcAndCallsStart(){
			TestSlotSystemElement sse = MakeTestSSE();
				System.Func<IEnumeratorFake> mockFocCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
				ISSECoroutineFactory stubCorFactory = Substitute.For<ISSECoroutineFactory>();
					stubCorFactory.MakeFocusCoroutine().Returns(mockFocCoroutine);
				sse.SetCoroutineFactory(stubCorFactory);
			sse.Deactivate();

			sse.Focus();

			AssertSSESelProcIsSetAndIsRunning(sse, typeof(SSEFocusProcess), mockFocCoroutine);
		}
		[Test]
		public void Select_FromNonNullNorSel_SetsSelectdProcAndCallsStart(){
			TestSlotSystemElement sse = MakeTestSSE();
				System.Func<IEnumeratorFake> mockSelCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
				ISSECoroutineFactory stubCorFactory = Substitute.For<ISSECoroutineFactory>();
					stubCorFactory.MakeSelectCoroutine().Returns(mockSelCoroutine);
				sse.SetCoroutineFactory(stubCorFactory);
			sse.Deactivate();

			sse.Select();

			AssertSSESelProcIsSetAndIsRunning(sse, typeof(SSESelectProcess), mockSelCoroutine);
		}
		[Test]
		public void SelStateSeqence(){
			TestSlotSystemElement sse = MakeTestSSE();
				System.Func<IEnumeratorFake> mockDeaCor = Substitute.For<System.Func<IEnumeratorFake>>();
				System.Func<IEnumeratorFake> mockDefCor = Substitute.For<System.Func<IEnumeratorFake>>();
				System.Func<IEnumeratorFake> mockFocCor = Substitute.For<System.Func<IEnumeratorFake>>();
				System.Func<IEnumeratorFake> mockSelCor = Substitute.For<System.Func<IEnumeratorFake>>();
				ISSECoroutineFactory stubCorFactory = Substitute.For<ISSECoroutineFactory>();
					stubCorFactory.MakeDeactivateCoroutine().Returns(mockDeaCor);
					stubCorFactory.MakeDefocusCoroutine().Returns(mockDefCor);
					stubCorFactory.MakeFocusCoroutine().Returns(mockFocCor);
					stubCorFactory.MakeSelectCoroutine().Returns(mockSelCor);
				sse.SetCoroutineFactory(stubCorFactory);
			ISSESelProcess selProc = sse.selProcess;
			ISSESelProcess prevProc = null;

				Assert.That(selProc, Is.Null);
				Assert.That(sse.isSelStateNull, Is.True);
				Assert.That(sse.wasSelStateNull, Is.True);

			sse.Deactivate();

				selProc = sse.selProcess;
				Assert.That(selProc, Is.Null);
				Assert.That(sse.isDeactivated, Is.True);
				Assert.That(sse.wasSelStateNull, Is.True);
			
			sse.Deactivate();

				selProc = sse.selProcess;
				Assert.That(selProc, Is.Null);
				Assert.That(sse.isDeactivated, Is.True);
				Assert.That(sse.wasSelStateNull, Is.True);
			
			sse.Defocus();

				selProc = sse.selProcess;
				Assert.That(selProc, Is.TypeOf(typeof(SSEDefocusProcess)));
				Assert.That(selProc.sse, Is.SameAs(sse));
				Assert.That(selProc.isRunning, Is.True);
				mockDefCor.Received(1).Invoke();
				Assert.That(sse.isDefocused, Is.True);
				Assert.That(sse.wasDeactivated, Is.True);
				prevProc = selProc;
			
			sse.Defocus();

				selProc = sse.selProcess;
				Assert.That(selProc, Is.TypeOf(typeof(SSEDefocusProcess)));
				Assert.That(selProc.sse, Is.SameAs(sse));
				Assert.That(selProc.isRunning, Is.True);
				mockDefCor.Received(1).Invoke();
				Assert.That(sse.isDefocused, Is.True);
				Assert.That(sse.wasDeactivated, Is.True);
				Assert.That(prevProc, Is.TypeOf(typeof(SSEDefocusProcess)));
				Assert.That(prevProc.sse, Is.SameAs(sse));
				Assert.That(prevProc.isRunning, Is.True);
				prevProc = selProc;
			
			sse.Focus();

				selProc = sse.selProcess;
				Assert.That(selProc, Is.TypeOf(typeof(SSEFocusProcess)));
				Assert.That(selProc.sse, Is.SameAs(sse));
				Assert.That(selProc.isRunning, Is.True);
				mockFocCor.Received(1).Invoke();
				Assert.That(sse.isFocused, Is.True);
				Assert.That(sse.wasDefocused, Is.True);
				Assert.That(prevProc, Is.TypeOf(typeof(SSEDefocusProcess)));
				Assert.That(prevProc.sse, Is.SameAs(sse));
				Assert.That(prevProc.isRunning, Is.False);
				prevProc = selProc;

			sse.Defocus();

				selProc = sse.selProcess;
				Assert.That(selProc, Is.TypeOf(typeof(SSEDefocusProcess)));
				Assert.That(selProc.sse, Is.SameAs(sse));
				Assert.That(selProc.isRunning, Is.True);
				mockDefCor.Received(2).Invoke();
				Assert.That(sse.isDefocused, Is.True);
				Assert.That(sse.wasFocused, Is.True);
				Assert.That(prevProc, Is.TypeOf(typeof(SSEFocusProcess)));
				Assert.That(prevProc.sse, Is.SameAs(sse));
				Assert.That(prevProc.isRunning, Is.False);
				prevProc = selProc;
			
			sse.Deactivate();
				
				selProc = sse.selProcess;
				Assert.That(selProc, Is.TypeOf(typeof(SSEDeactivateProcess)));
				Assert.That(selProc.sse, Is.SameAs(sse));
				Assert.That(selProc.isRunning, Is.True);
				mockDeaCor.Received(1).Invoke();
				Assert.That(sse.isDeactivated, Is.True);
				Assert.That(sse.wasDefocused, Is.True);
				Assert.That(prevProc, Is.TypeOf(typeof(SSEDefocusProcess)));
				Assert.That(prevProc.sse, Is.SameAs(sse));
				Assert.That(prevProc.isRunning, Is.False);
				prevProc = selProc;
			
			sse.ClearCurSelState();
				
				selProc = sse.selProcess;
				Assert.That(selProc, Is.Null);
				mockDeaCor.Received(1).Invoke();
				Assert.That(sse.isSelStateNull, Is.True);
				Assert.That(sse.wasDeactivated, Is.True);
				Assert.That(prevProc, Is.TypeOf(typeof(SSEDeactivateProcess)));
				Assert.That(prevProc.sse, Is.SameAs(sse));
				Assert.That(prevProc.isRunning, Is.False);
				prevProc = selProc;
			
			sse.Select();

				selProc = sse.selProcess;
				Assert.That(selProc, Is.Null);
				mockSelCor.DidNotReceive().Invoke();
				Assert.That(sse.isSelected, Is.True);
				Assert.That(sse.wasSelStateNull, Is.True);
				Assert.That(prevProc, Is.Null);
				prevProc = selProc;
		}
		/* Helpers */
			public void AssertSSESelProcIsSetAndIsRunning(ISlotSystemElement sse, Type procType, Func<IEnumeratorFake> mockCoroutine){
				ISSESelProcess actual = sse.selProcess;
				Assert.That(actual, Is.TypeOf(procType));
				Assert.That(actual.sse, Is.SameAs(sse));
				mockCoroutine.Received().Invoke();
				Assert.That(actual.isRunning, Is.True);
			}
	}
}
