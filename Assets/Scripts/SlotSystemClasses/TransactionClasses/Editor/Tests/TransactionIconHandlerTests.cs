using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
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
			DraggedIcon stubDI = new DraggedIcon(MakeSubSB(), MakeSubIconHandler());
			iconHandler.SetDIcon1(stubDI);

			iconHandler.AcceptDITAComp(stubDI);

			Assert.That(iconHandler.dIcon1Done, Is.True);
			}
		[Test]
		public void SetDIcon1_ToNonNull_SetsDIcon1DoneFalse(){
			TransactionIconHandler iconHandler = new TransactionIconHandler(MakeSubTAMStateHandler());
			DraggedIcon stubDI = new DraggedIcon(MakeSubSB(), MakeSubIconHandler());

			iconHandler.SetDIcon1(stubDI);

			Assert.That(iconHandler.dIcon1Done, Is.False);
			}
		[Test]
		public void SetDIcon1_ToNull_SetsDIcon1DoneTrue(){
			TransactionIconHandler iconHandler = new TransactionIconHandler(MakeSubTAMStateHandler());
			DraggedIcon stubDI = new DraggedIcon(MakeSubSB(), MakeSubIconHandler());

			iconHandler.SetDIcon1(stubDI);
			iconHandler.SetDIcon1(null);

			Assert.That(iconHandler.dIcon1Done, Is.True);
			}
		[Test]
		public void SetDIcon2_ToNonNull_SetsDIcon2DoneFalse(){
			TransactionIconHandler iconHandler = new TransactionIconHandler(MakeSubTAMStateHandler());
			DraggedIcon stubDI = new DraggedIcon(MakeSubSB(), MakeSubIconHandler());

			iconHandler.SetDIcon2(stubDI);

			Assert.That(iconHandler.dIcon2Done, Is.False);
			}
		[Test]
		public void SetDIcon2_ToNull_SetsDIcon2DoneTrue(){
			TransactionIconHandler iconHandler = new TransactionIconHandler(MakeSubTAMStateHandler());
			DraggedIcon stubDI = new DraggedIcon(MakeSubSB(), MakeSubIconHandler());

			iconHandler.SetDIcon2(stubDI);
			iconHandler.SetDIcon2(null);

			Assert.That(iconHandler.dIcon2Done, Is.True);
			}
	}
}
