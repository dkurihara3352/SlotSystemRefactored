﻿using UnityEngine;
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
				ISlottable mockSB = MakeSubSB();
				WaitForPointerUpProcess wfpuProc = new WaitForPointerUpProcess(mockSB, FakeCoroutine);

				wfpuProc.Expire();

				mockSB.Received().Defocus();
				}
			[Test]
			public void WaitForNextTouchProcess_Expire_SBIsPickedUp_CallsSBExecuteTransaction(){
				ISlottable stubSB = MakeSubSB();
				ITransactionManager mockTAM = MakeSubTAM();
				WaitForNextTouchProcess wfntProc = new WaitForNextTouchProcess(stubSB, FakeCoroutine, mockTAM);
				stubSB.isPickedUp.Returns(true);

				wfntProc.Expire();

				mockTAM.Received().ExecuteTransaction();
			}
			[Test]
			public void WaitForNextTouchProcess_Expire_SBIsNOTPickedUp_CallsSBVarious(){
				ISlottable mockSB = MakeSubSB();
				ITransactionManager stubTAM = MakeSubTAM();
				WaitForNextTouchProcess wfntProc = new WaitForNextTouchProcess(mockSB, FakeCoroutine, stubTAM);
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