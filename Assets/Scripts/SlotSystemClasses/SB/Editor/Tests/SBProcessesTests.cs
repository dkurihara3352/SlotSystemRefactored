using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections;
using System.Collections.Generic;
using NSubstitute;

namespace SlotSystemTests{
	namespace SBTests{
		[TestFixture]
		[Category("SB")]
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
				ISlottable mockSB = MakeSubSB();
				WaitForPointerUpProcess wfpuProc = new WaitForPointerUpProcess(mockSB, FakeCoroutine);

				wfpuProc.Expire();

				mockSB.Received().Defocus();
				}
			[Test]
			public void WaitForNextTouchProcess_Expire_SBIsPickedUp_CallsSBExecuteTransaction(){
				ISlottable mockSB = MakeSubSB();
				WaitForNextTouchProcess wfntProc = new WaitForNextTouchProcess(mockSB, FakeCoroutine);
				mockSB.isPickedUp.Returns(true);

				wfntProc.Expire();

				mockSB.Received().ExecuteTransaction();
				}
			[Test]
			public void WaitForNextTouchProcess_Expire_SBIsNOTPickedUp_CallsSBVarious(){
				ISlottable mockSB = MakeSubSB();
				WaitForNextTouchProcess wfntProc = new WaitForNextTouchProcess(mockSB, FakeCoroutine);
				mockSB.isPickedUp.Returns(false);

				wfntProc.Expire();

				mockSB.Received().Tap();
				mockSB.Received().Refresh();
				mockSB.Received().Focus();
				}
			/*	helper	*/
		}
	}
}
