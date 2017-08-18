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
	[Category("SSM")]
	public class AllElementsProviderTests: SlotSystemTest{
		[Test]
		public void AllSGs_Always_CallsPIHAddInSGListInSequence(){
			AllElementsProvider allElementProv;
				ISlotSystemManager stubSSM = MakeSubSSM();
					ISlotSystemBundle pBun = MakeSubBundle();
					ISlotSystemBundle eBun = MakeSubBundle();
					IEnumerable<ISlotSystemBundle> gBuns;
						ISlotSystemBundle gBunA = MakeSubBundle();
						ISlotSystemBundle gBunB = MakeSubBundle();
						ISlotSystemBundle gBunC = MakeSubBundle();
						gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};
					stubSSM.GetPoolBundle().Returns(pBun);
					stubSSM.GetEquipBundle().Returns(eBun);
					stubSSM.GetOtherBundles().Returns(gBuns);
				allElementProv = new AllElementsProvider(stubSSM);

			List<ISlotGroup> list = allElementProv.GetAllSGs();
			Received.InOrder(() => {
				pBun.PerformInHierarchy(allElementProv.AddInSGList, Arg.Any<List<ISlotGroup>>());
				eBun.PerformInHierarchy(allElementProv.AddInSGList, Arg.Any<List<ISlotGroup>>());
				foreach(var gBun in gBuns){
					gBun.PerformInHierarchy(allElementProv.AddInSGList, Arg.Any<List<ISlotGroup>>());
				}
			});
		}
		[Test]
		public void AddInSGList_Always_VerifySGsAndStoreThemInTheList(){
			AllElementsProvider allEProv = new AllElementsProvider(MakeSubSSM());
				ISlottable sb = MakeSubSB();
				ISlotSystemManager stubSSM = MakeSubSSM();
				ISlotSystemElement ele = MakeSubSSE();
				ISlotGroup sgA = MakeSubSG();
				ISlotGroup sgB = MakeSubSG();
				IEnumerable<ISlotSystemElement> sses = new ISlotSystemElement[]{
					sb, stubSSM, ele, sgA, ele, sgB
				};
			IEnumerable<ISlotGroup> expected = new ISlotGroup[]{sgA, sgB};
			List<ISlotGroup> sgs = new List<ISlotGroup>();

			foreach(var e in sses)
				allEProv.AddInSGList(e, sgs);
			
			Assert.That(sgs.MemberEquals(expected), Is.True);
		}
		[Test]
		public void AllSGPs_Always_CallsPoolBundlePIHAddInSGList(){
			AllElementsProvider allEProv;
				ISlotSystemManager stubSSM = MakeSubSSM();
					ISlotSystemBundle pBun = MakeSubBundle();
				stubSSM.GetPoolBundle().Returns(pBun);
			allEProv = new AllElementsProvider(stubSSM);

			List<ISlotGroup> list = allEProv.allSGPs;
			pBun.Received().PerformInHierarchy(allEProv.AddInSGList, Arg.Any<List<ISlotGroup>>());
		}
		[Test]
		public void AllSGEs_Always_CallsEquipBundlePIHAddINSGList(){
			AllElementsProvider allEProv;
				ISlotSystemManager stubSSM = MakeSubSSM();
					ISlotSystemBundle eBun = MakeSubBundle();
				stubSSM.GetEquipBundle().Returns(eBun);
			allEProv = new AllElementsProvider(stubSSM);

			List<ISlotGroup> list = allEProv.allSGEs;
			eBun.Received().PerformInHierarchy(allEProv.AddInSGList, Arg.Any<List<ISlotGroup>>());
		}
		[Test]
		public void AllSGGs_Always_CallsAllGenBundlesPIHAddInSGList(){
			AllElementsProvider allEProv;
				ISlotSystemManager stubSSM = MakeSubSSM();
					IEnumerable<ISlotSystemBundle> gBuns;
						ISlotSystemBundle gBunA = MakeSubBundle();
						ISlotSystemBundle gBunB = MakeSubBundle();
						ISlotSystemBundle gBunC = MakeSubBundle();
					gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};
				stubSSM.GetOtherBundles().Returns(gBuns);
			allEProv = new AllElementsProvider(stubSSM);

			List<ISlotGroup> list = allEProv.allSGGs;

			foreach(var gBun in gBuns){
				gBun.Received().PerformInHierarchy(allEProv.AddInSGList, Arg.Any<List<ISlotGroup>>());
			}
		}
		[Test]
		public void AddSBsToRes_WhenCalled_FindSBsAndAddThemIntoTheList(){
			AllElementsProvider allEProv = new AllElementsProvider(MakeSubSSM());
				ISlotGroup sg = MakeSubSG();
				ISlottable sbA = MakeSubSB();
				ISlotSystemElement ele = Substitute.For<ISlotSystemElement>();
				ISlottable sbB = MakeSubSB();
				ISlotSystemBundle bundle = MakeSubBundle();
				IEnumerable<ISlotSystemElement> elements = new ISlotSystemElement[]{
					sg, sbA, ele, sbB, bundle
				};
			IEnumerable<ISlottable> expected = new ISlottable[]{
				sbA, sbB
			};
			List<ISlottable> actual = new List<ISlottable>();

			foreach(var e in elements)
				allEProv.AddSBToRes(e, actual);
			
			Assert.That(actual.MemberEquals(expected), Is.True);
		}
		[Test]
		public void allSBs_WhenCalled_CallsAllBundlesPIHAddSBtoRes(){
			AllElementsProvider allEProv;
				ISlotSystemManager stubSSM = MakeSubSSM();
					ISlotSystemBundle pBun = MakeSubBundle();
					ISlotSystemBundle eBun = MakeSubBundle();
					IEnumerable<ISlotSystemBundle> gBuns;
						ISlotSystemBundle gBunA = MakeSubBundle();
						ISlotSystemBundle gBunB = MakeSubBundle();
						ISlotSystemBundle gBunC = MakeSubBundle();
						gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};
					IEnumerable<ISlotSystemElement> ssmEles = new ISlotSystemElement[]{
						pBun, eBun, gBunA, gBunB, gBunC
					};
				stubSSM.GetEnumerator().Returns(ssmEles.GetEnumerator());
			allEProv = new AllElementsProvider(stubSSM);
				stubSSM.When(x => x.PerformInHierarchy(allEProv.AddSBToRes, Arg.Any<List<ISlottable>>())).Do(x => {
					foreach(var e in stubSSM)
						e.PerformInHierarchy(allEProv.AddSBToRes, new List<ISlottable>());
				});
			
			List<ISlottable> list = allEProv.GetAllSBs();

			pBun.Received().PerformInHierarchy(allEProv.AddSBToRes, Arg.Any<List<ISlottable>>());
			eBun.Received().PerformInHierarchy(allEProv.AddSBToRes, Arg.Any<List<ISlottable>>());
			foreach(var gBun in gBuns)
				gBun.Received().PerformInHierarchy(allEProv.AddSBToRes, Arg.Any<List<ISlottable>>());
		}
	}
}
