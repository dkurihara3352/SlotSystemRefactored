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
				ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
				WaitForPickUpProcess wfpuProc = new WaitForPickUpProcess(actStateHandler, FakeCoroutine);
				
				wfpuProc.Expire();

				actStateHandler.Received().PickUp();
			}
			[Test]
			public void WaitForPointerUpProcess_Expire_WhenCalled_CallsSBDefocus(){
				ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
				WaitForPointerUpProcess wfpuProc = new WaitForPointerUpProcess(selStateHandler, FakeCoroutine);

				wfpuProc.Expire();

				selStateHandler.Received().Defocus();
				}
			[Test]
			public void WaitForNextTouchProcess_Expire_IsPickedUp_Calls_ExecuteTransaction(){
				ISlottable stubSB = MakeSubSB();
				ITransactionManager mockTAM = MakeSubTAM();
				WaitForNextTouchProcess wfntProc = new WaitForNextTouchProcess(stubSB, mockTAM, FakeCoroutine);
				stubSB.IsPickedUp().Returns(true);

				wfntProc.Expire();

				mockTAM.Received().ExecuteTransaction();
			}
			[Test]
			public void WaitForNextTouchProcess_Expire_SBIsNOTPickedUp_Calls_Various(){
				WaitForNextTouchProcess wfntProc;
					ISlottable mockSB = MakeSubSB();
						ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
						mockSB.GetSelStateHandler().Returns(selStateHandler);
					mockSB.IsPickedUp().Returns(false);
				wfntProc = new WaitForNextTouchProcess(mockSB, MakeSubTAM(), FakeCoroutine);
				

				wfntProc.Expire();

				mockSB.Received().Tap();
				mockSB.Received().Refresh();
				selStateHandler.Received().Focus();
			}
			/*	helper	*/
		}
	}
}
