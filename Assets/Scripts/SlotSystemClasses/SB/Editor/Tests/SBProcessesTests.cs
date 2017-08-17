using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections;
using System.Collections.Generic;
using NSubstitute;

namespace SlotSystemTests{
	namespace SlottableTests{
		[TestFixture]
		public class SBProcessesTests: SlotSystemTest{
			[Test]
			public void WaitForPickUpProcess_Expire_WhenCalled_CallsSBPickUp(){
				ISlottable mockSB = MakeSubSB();
				WaitForPickUpProcess wfpuProc = new WaitForPickUpProcess(mockSB, FakeCoroutine);
				
				wfpuProc.Expire();

				mockSB.Received().PickUp();
				}
			[Test]
			public void WaitForPointerUpProcess_Expire_WhenCalled_CallsSBDefocus(){
				ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
				WaitForPointerUpProcess wfpuProc = new WaitForPointerUpProcess(selStateHandler, FakeCoroutine);

				wfpuProc.Expire();

				selStateHandler.Received().Defocus();
				}
			[Test]
			public void WaitForNextTouchProcess_Expire_SBIsPickedUp_CallsSBExecuteTransaction(){
				ISlottable stubSB = MakeSubSB();
				ITransactionManager mockTAM = MakeSubTAM();
				WaitForNextTouchProcess wfntProc = new WaitForNextTouchProcess(stubSB, Substitute.For<ISSESelStateHandler>(), mockTAM, FakeCoroutine);
				stubSB.IsPickedUp().Returns(true);

				wfntProc.Expire();

				mockTAM.Received().ExecuteTransaction();
			}
			[Test]
			public void WaitForNextTouchProcess_Expire_SBIsNOTPickedUp_CallsSBVarious(){
				ISlottable mockSB = MakeSubSB();
				ITransactionManager stubTAM = MakeSubTAM();
				ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
				WaitForNextTouchProcess wfntProc = new WaitForNextTouchProcess(mockSB, selStateHandler, stubTAM, FakeCoroutine);
				mockSB.IsPickedUp().Returns(false);

				wfntProc.Expire();

				mockSB.Received().Tap();
				mockSB.Received().Refresh();
				selStateHandler.Received().Focus();
			}
			/*	helper	*/
		}
	}
}
