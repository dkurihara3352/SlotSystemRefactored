using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections;
using System.Collections.Generic;

namespace SlotSystemTests{
	namespace SBTests{
		[TestFixture]
		public class SBProcessesTests: AbsSlotSystemTest{

			[Test]
			public void WaitForPickUpProcess_Expire_WhenCalled_CallsSBPickUp(){
				FakeSB mockSB = MakeFakeSB();
				WaitForPickUpProcess wfpuProc = new WaitForPickUpProcess(mockSB, FakeCoroutine);
				mockSB.ResetCallCheck();
				
				wfpuProc.Expire();

				Assert.That(mockSB.isPickUpCalled, Is.True);

				mockSB.ResetCallCheck();
			}
			[Test]
			public void WaitForPointerUpProcess_Expire_WhenCalled_SetsSBSelStateDefocused(){
				FakeSB mockSB = MakeFakeSB();
				WaitForPointerUpProcess wfpuProc = new WaitForPointerUpProcess(mockSB, FakeCoroutine);

				wfpuProc.Expire();

				Assert.That(mockSB.curSelState, Is.SameAs(Slottable.sbDefocusedState));
			}
			[Test]
			public void WaitForNextTouchProcess_Expire_SBIsPickedUp_CallsSBExecuteTransaction(){
				FakeSB mockSB = MakeFakeSB();
				WaitForNextTouchProcess wfntProc = new WaitForNextTouchProcess(mockSB, FakeCoroutine);
				mockSB.SetIsPickedUp(true);
				mockSB.ResetCallCheck();

				wfntProc.Expire();

				Assert.That(mockSB.isExecuteTransactionCalled, Is.True);

				mockSB.ResetCallCheck();
			}
			[Test]
			public void WaitForNextTouchProcess_Expire_SBIsNOTPickedUp_CallsSBVarious(){
				FakeSB mockSB = MakeFakeSB();
				WaitForNextTouchProcess wfntProc = new WaitForNextTouchProcess(mockSB, FakeCoroutine);
				mockSB.SetIsPickedUp(false);
				mockSB.ResetCallCheck();

				wfntProc.Expire();

				IEnumerable<bool> callChecks = new bool[]{
					mockSB.isTapCalled,
					mockSB.isResetCalled,
					mockSB.isFocusCalled
				};
				Assert.That(callChecks, Is.All.True);

				mockSB.ResetCallCheck();
			}
			/*	helper	*/
		}
	}
}
