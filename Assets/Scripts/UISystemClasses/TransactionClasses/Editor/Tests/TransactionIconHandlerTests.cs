using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using UISystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Utility;
namespace SlotSystemTests{
	[TestFixture]
	[Category("TAM")]
	public class TransactionIconHandlerTests: SlotSystemTest{
		[Test]
		public void AcceptsDITAComp_ValidDI_SetsDone(){
			TransactionIconHandler iconHandler = new TransactionIconHandler(MakeSubTAMStateHandler());
			ISlottable stubSB = MakeSubSB();
			iconHandler.SetDIcon1(stubSB);

			iconHandler.AcceptDITAComp(stubSB);

			Assert.That(iconHandler.IsDIcon1Done(), Is.True);
			}
		[Test]
		public void SetDIcon1_ToNonNull_SetsDIcon1DoneFalse(){
			TransactionIconHandler iconHandler = new TransactionIconHandler(MakeSubTAMStateHandler());
			ISlottable stubSB = MakeSubSB();

			iconHandler.SetDIcon1(stubSB);

			Assert.That(iconHandler.IsDIcon1Done(), Is.False);
			}
		[Test]
		public void SetDIcon1_ToNull_SetsDIcon1DoneTrue(){
			TransactionIconHandler iconHandler = new TransactionIconHandler(MakeSubTAMStateHandler());
			ISlottable stubSB = MakeSubSB();

			iconHandler.SetDIcon1(stubSB);
			iconHandler.SetDIcon1(null);

			Assert.That(iconHandler.IsDIcon1Done(), Is.True);
			}
		[Test]
		public void SetDIcon2_ToNonNull_SetsDIcon2DoneFalse(){
			TransactionIconHandler iconHandler = new TransactionIconHandler(MakeSubTAMStateHandler());
			ISlottable stubSB = MakeSubSB();

			iconHandler.SetDIcon2(stubSB);

			Assert.That(iconHandler.IsDIcon2Done(), Is.False);
			}
		[Test]
		public void SetDIcon2_ToNull_SetsDIcon2DoneTrue(){
			TransactionIconHandler iconHandler = new TransactionIconHandler(MakeSubTAMStateHandler());
			ISlottable stubSB = MakeSubSB();

			iconHandler.SetDIcon2(stubSB);
			iconHandler.SetDIcon2(null);

			Assert.That(iconHandler.IsDIcon2Done(), Is.True);
			}
	}
}
