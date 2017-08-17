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
	public class TransactionSGHandlerTests: SlotSystemTest {
		[Test]
		public void AcceptsSGTAComp_ValidSG_SetsDone(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup stubSG = MakeSubSG();
			sgHandler.SetSG1(stubSG);

			sgHandler.AcceptSGTAComp(stubSG);

			Assert.That(sgHandler.sg2Done, Is.True);
			}
		[Test]
		public void SetSG1_NullToSome_SetsSG1(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup stubSG = MakeSubSG();

			sgHandler.SetSG1(stubSG);

			Assert.That(sgHandler.sg1, Is.SameAs(stubSG));
			}
		[Test]
		public void SetSG1_NullToSome_SetsSG1DoneFalse(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup stubSG = MakeSubSG();

			sgHandler.SetSG1(stubSG);

			Assert.That(sgHandler.sg1Done, Is.False);
			}
		[Test]
		public void SetSG1_OtherToSome_SetsSG1(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup prevSG = MakeSubSG();
			ISlotGroup stubSG = MakeSubSG();

			sgHandler.SetSG1(prevSG);
			sgHandler.SetSG1(stubSG);

			Assert.That(sgHandler.sg1, Is.SameAs(stubSG));
			}
		[Test]
		public void SetSG1_OtherToSome_SetsSG1DoneFalse(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup prevSG = MakeSubSG();
			ISlotGroup stubSG = MakeSubSG();

			sgHandler.SetSG1(prevSG);
			sgHandler.SetSG1(stubSG);

			Assert.That(sgHandler.sg1Done, Is.False);
			}
		[Test]
		public void SetSG1_SomeToNull_SetsSG1Null(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup stubSG = MakeSubSG();
			sgHandler.SetSG1(stubSG);

			sgHandler.SetSG1(null);

			Assert.That(sgHandler.sg1, Is.Null);
			}
		[Test]
		public void SetSG1_SomeToNull_SetsSG1DoneTrue(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup stubSG = MakeSubSG();
			sgHandler.SetSG1(stubSG);

			sgHandler.SetSG1(null);

			Assert.That(sgHandler.sg1Done, Is.True);
			}
		[Test]
		public void SetSG2_NullToSome_SetsSG2(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup stubSG = MakeSubSG();

			sgHandler.SetSG2(stubSG);

			Assert.That(sgHandler.sg2, Is.SameAs(stubSG));
			}
		[Test]
		public void SetSG2_NullToSome_SetsSG2DoneFalse(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup stubSG = MakeSubSG();

			sgHandler.SetSG2(stubSG);

			Assert.That(sgHandler.sg2Done, Is.False);
			}
		[Test]
		public void SetSG2_NullToSome_CallSG2Select(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup mockSG = MakeSubSG();
				ISSESelStateHandler sgSelStateHandler = Substitute.For<ISSESelStateHandler>();
				mockSG.GetSelStateHandler().Returns(sgSelStateHandler);

			sgHandler.SetSG2(mockSG);

			sgSelStateHandler.Received().Select();
		}
		[Test]
		public void SetSG2_OtherToSome_SetsSG2(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup prevSG = MakeSubSG();
			ISlotGroup stubSG = MakeSubSG();

			sgHandler.SetSG2(prevSG);
			sgHandler.SetSG2(stubSG);

			Assert.That(sgHandler.sg2, Is.SameAs(stubSG));
			}
		[Test]
		public void SetSG2_OtherToSome_SetsSG2DoneFalse(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup prevSG = MakeSubSG();
			ISlotGroup stubSG = MakeSubSG();

			sgHandler.SetSG2(prevSG);
			sgHandler.SetSG2(stubSG);

			Assert.That(sgHandler.sg2Done, Is.False);
			}
		[Test]
		public void SetSG2_OtherToSome_CallsSG2Select(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup prevSG = MakeSubSG();
			ISlotGroup stubSG = MakeSubSG();
				ISSESelStateHandler sgSelStateHandler = Substitute.For<ISSESelStateHandler>();
				stubSG.GetSelStateHandler().Returns(sgSelStateHandler);

			sgHandler.SetSG2(prevSG);
			sgHandler.SetSG2(stubSG);

			sgSelStateHandler.Received().Select();
		}
		[Test]
		public void SetSG2_SomeToNull_SetsSG2Null(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup stubSG = MakeSubSG();
			sgHandler.SetSG2(stubSG);

			sgHandler.SetSG2(null);

			Assert.That(sgHandler.sg2, Is.Null);
			}
		[Test]
		public void SetSG2_SomeToNull_SetsSG2DoneTrue(){
			TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
			ISlotGroup stubSG = MakeSubSG();
			sgHandler.SetSG2(stubSG);

			sgHandler.SetSG2(null);

			Assert.That(sgHandler.sg2Done, Is.True);
			}
	}
}
