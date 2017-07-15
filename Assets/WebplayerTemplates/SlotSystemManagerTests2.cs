using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using System;
using System.Collections;
using System.Collections.Generic;
namespace SlotSystemTests{
	namespace ElementsTests{
		[TestFixture]
		public class SlotSystemManagerTests2: AbsSlotSystemTest{

			[Test]
			public void allSGs_Always_ReturnsSumOfAllSGs(){
				// SlotSystemManager ssm = MakeSubSSM();
				SlotSystemManager ssm = Substitute.ForPartsOf<SlotSystemManager>();
				List<ISlotGroup> sgps;
					ISlotGroup sgpA = MakeSubSG();
					ISlotGroup sgpB = MakeSubSG();
					ISlotGroup sgpC = MakeSubSG();
					sgps = new List<ISlotGroup>(new ISlotGroup[]{
						sgpA, sgpB, sgpC
					});
				List<ISlotGroup> sges;
					ISlotGroup sgeA = MakeSubSG();
					ISlotGroup sgeB = MakeSubSG();
					ISlotGroup sgeC = MakeSubSG();
					sges = new List<ISlotGroup>(new ISlotGroup[]{
						sgeA, sgeB, sgeC
					});
				List<ISlotGroup> sggs;
					ISlotGroup sggA = MakeSubSG();
					ISlotGroup sggB = MakeSubSG();
					ISlotGroup sggC = MakeSubSG();
					sggs = new List<ISlotGroup>(new ISlotGroup[]{
						sggA, sggB, sggC
					});
				ssm.When(x => x.allSGPs()).DoNotCallBase();
				ssm.When(x => x.allSGEs()).DoNotCallBase();
				ssm.When(x => x.allSGGs()).DoNotCallBase();
				ssm.allSGPs().Returns(sgps);
				ssm.allSGEs().Returns(sges);
				ssm.allSGGs().Returns(sggs);
				// ssm.allSGPs.Returns(sgps);
				// ssm.allSGEs.Returns(sges);
				// ssm.allSGGs.Returns(sggs);
				List<ISlotGroup> expected = new List<ISlotGroup>(new ISlotGroup[]{
					sgpA, sgpB, sgpC,
					sgeA, sgeB, sgeC,
					sggA, sggB, sggC
				});

				List<ISlotGroup> actual = ssm.allSGs();

				Assert.That(actual, Is.EqualTo(expected));
			}
			// [Test]
			// public void allSGPs_Always_CallsPoolBundlePIHAddInSGList(){
			// 	SlotSystemManager ssm = MakeSubSSM();
			// 	ISlotSystemBundle pBun = MakeSubBundle();
			// 	ssm.poolBundle.Returns(pBun);

			// 	List<ISlotGroup> lst = ssm.allSGPs;

			// 	pBun.Received().PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
			// }
			// [Test]
			// public void allSGEs_Always_CallsEquipBundlePIHAddInSGList(){
			// 	SlotSystemManager ssm = MakeSubSSM();
			// 	ISlotSystemBundle eBun = MakeSubBundle();
			// 	ssm.equipBundle.Returns(eBun);

			// 	List<ISlotGroup> lst = ssm.allSGEs;

			// 	eBun.Received().PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
			// }
		}
	}
}
