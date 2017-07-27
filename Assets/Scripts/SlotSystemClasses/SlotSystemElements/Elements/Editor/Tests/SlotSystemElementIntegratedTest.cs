﻿using UnityEngine;
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
				System.Func<IEnumeratorFake> mockFunc = Substitute.For<System.Func<IEnumeratorFake>>();
				sse.SetDeaCoroutine(mockFunc);
			sse.Defocus();

			sse.Deactivate();

			ISSESelProcess actual = sse.selProcess;
			Assert.That(actual, Is.TypeOf(typeof(SSEDeactivateProcess)));
			Assert.That(actual.sse, Is.SameAs(sse));
			mockFunc.Received().Invoke();
			Assert.That(actual.isRunning, Is.True);
		}
		[Test]
		public void Defocus_FromNonNullNorDef_SetsDefocusdProcAndCallsStart(){
			TestSlotSystemElement sse = MakeTestSSE();
				System.Func<IEnumeratorFake> mockFunc = Substitute.For<System.Func<IEnumeratorFake>>();
				sse.SetDefCoroutine(mockFunc);
			sse.Deactivate();

			sse.Defocus();

			ISSESelProcess actual = sse.selProcess;
			Assert.That(actual, Is.TypeOf(typeof(SSEDefocusProcess)));
			Assert.That(actual.sse, Is.SameAs(sse));
			mockFunc.Received().Invoke();
			Assert.That(actual.isRunning, Is.True);
		}
		[Test]
		public void Focus_FromNonNullNorFoc_SetsFocusdProcAndCallsStart(){
			TestSlotSystemElement sse = MakeTestSSE();
				System.Func<IEnumeratorFake> mockFunc = Substitute.For<System.Func<IEnumeratorFake>>();
				sse.SetFocCoroutine(mockFunc);
			sse.Deactivate();

			sse.Focus();

			ISSESelProcess actual = sse.selProcess;
			Assert.That(actual, Is.TypeOf(typeof(SSEFocusProcess)));
			Assert.That(actual.sse, Is.SameAs(sse));
			mockFunc.Received().Invoke();
			Assert.That(actual.isRunning, Is.True);
		}
		[Test]
		public void Select_FromNonNullNorSel_SetsSelectdProcAndCallsStart(){
			TestSlotSystemElement sse = MakeTestSSE();
				System.Func<IEnumeratorFake> mockFunc = Substitute.For<System.Func<IEnumeratorFake>>();
				sse.SetSelCoroutine(mockFunc);
			sse.Deactivate();

			sse.Select();

			ISSESelProcess actual = sse.selProcess;
			Assert.That(actual, Is.TypeOf(typeof(SSESelectProcess)));
			Assert.That(actual.sse, Is.SameAs(sse));
			mockFunc.Received().Invoke();
			Assert.That(actual.isRunning, Is.True);
		}
		[Test]
		public void SelStateSeqence(){
			TestSlotSystemElement sse = MakeTestSSE();
				System.Func<IEnumeratorFake> mockDeaCor = Substitute.For<System.Func<IEnumeratorFake>>();
				sse.SetDeaCoroutine(mockDeaCor);
				System.Func<IEnumeratorFake> mockDefCor = Substitute.For<System.Func<IEnumeratorFake>>();
				sse.SetDefCoroutine(mockDefCor);
				System.Func<IEnumeratorFake> mockFocCor = Substitute.For<System.Func<IEnumeratorFake>>();
				sse.SetFocCoroutine(mockFocCor);
				System.Func<IEnumeratorFake> mockSelCor = Substitute.For<System.Func<IEnumeratorFake>>();
				sse.SetSelCoroutine(mockSelCor);
			ISSESelProcess selProc = sse.selProcess;
			ISSESelProcess prevProc = null;

				Assert.That(selProc, Is.Null);
				Assert.That(sse.isCurSelStateNull, Is.True);
				Assert.That(sse.isSelStateInit, Is.True);

			sse.Deactivate();

				selProc = sse.selProcess;
				Assert.That(selProc, Is.Null);
				Assert.That(sse.isDeactivated, Is.True);
				Assert.That(sse.isSelStateInit, Is.True);
			
			sse.Deactivate();

				selProc = sse.selProcess;
				Assert.That(selProc, Is.Null);
				Assert.That(sse.isDeactivated, Is.True);
				Assert.That(sse.isSelStateInit, Is.True);
			
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
				Assert.That(sse.isCurSelStateNull, Is.True);
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
				Assert.That(sse.isSelStateInit, Is.True);
				Assert.That(prevProc, Is.Null);
				prevProc = selProc;
		}
	}
}
