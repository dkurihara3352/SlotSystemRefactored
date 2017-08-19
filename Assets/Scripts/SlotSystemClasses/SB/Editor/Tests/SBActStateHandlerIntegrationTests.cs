using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using SlotSystem;
using Utility;
namespace SlotSystemTests{
	namespace SlottableTests{
		[TestFixture]
		[Category("Integration")]
		public class SlottableIntegrationTests : SlotSystemTest{
			[Test]
			public void ActStateHandlerStates_ByDefault_AreNull(){
				ISBActStateHandler actStateHandler = new SBActStateHandler(MakeSubSB(), MakeSubTAM());

				Assert.That(actStateHandler.IsActStateNull(), Is.True);
				Assert.That(actStateHandler.WasActStateNull(), Is.True);
			}
			[Test]
			public void processes_ByDefault_AreNull(){
				Slottable sb = MakeSBWithRealStateHandlers();
				ISBActStateHandler actStateHandler = new SBActStateHandler(MakeSubSB(), MakeSubTAM());

				Assert.That(actStateHandler.GetActProcess(), Is.Null);
			}
		/* helpers */

			PointerEventDataFake eventData{get{return new PointerEventDataFake();}}
			void AssertRefreshCalled(ISlottable sb){
				Assert.That(sb.IsWaitingForAction(), Is.True);
				Assert.That(sb.GetItemHandler().GetPickedAmount(), Is.EqualTo(0));
				Assert.That(sb.GetNewSlotID(), Is.EqualTo(-2));
			}
			void SetUpForRefreshCall(ISlottable sb){
				sb.GetItemHandler().SetPickedAmount(20);
				sb.SetNewSlotID(6);
			}
			public void AssertSBActProcessIsSetAndRunning(ISlottable sb, Type procType, System.Func<IEnumeratorFake> coroutine){
				ISBActProcess actualProc = sb.GetActProcess();
				Assert.That(actualProc, Is.TypeOf(procType));
				Assert.That(actualProc.IsRunning(), Is.True);
				coroutine.Received().Invoke();
			}
		}
	}
}

