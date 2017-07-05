using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
[TestFixture]
public class AbsSlotSystemElementTests{
	[Test]
	public void Initialize_WhenCalled_SelStatesSetToSSEDeactivated(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();

		testSSE.Initialize();

		Assert.That(testSSE.prevSelState, Is.EqualTo(AbsSlotSystemElement.deactivatedState));
		Assert.That(testSSE.curSelState, Is.EqualTo(AbsSlotSystemElement.deactivatedState));
	}
	[Test]
	public void parent_ByDefault_IsNull(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();

		Assert.That(testSSE.parent, Is.Null);
	}
	[Test]
	public void immediateBundle_WhenParentNull_ReturnsNull(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();

		Assert.That(testSSE.immediateBundle, Is.Null);
	}
	[Test]
	public void immediateBundle_WhenParentIsBundle_ReturnsBundle(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		SlotSystemBundle stubBundle = MakeSlotSystemBundle();
		testSSE.parent = stubBundle;

		Assert.That(testSSE.immediateBundle, Is.SameAs(stubBundle));
	}
	[Test]
	public void immediateBundle_NoBundleInAncestry_ReturnsNull(){
		SlotSystemElement stubEle_1 = Substitute.For<SlotSystemElement>();
		SlotSystemElement stubEle_2 = Substitute.For<SlotSystemElement>();
		stubEle_1.parent = stubEle_2;
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		testSSE.parent = stubEle_1;

		Assert.That(testSSE.immediateBundle, Is.Null);
	}
	[Test]
	public void immediateBundle_SomeBundlesUpInAncestry_ReturnsMostProximal(){
		SlotSystemBundle stubBundle_further = MakeSlotSystemBundle();
		SlotSystemBundle stubBundle_closer = MakeSlotSystemBundle();
		SlotSystemElement stubEle_1 = MakeSlotSystemElement();
		SlotSystemElement stubEle_2 = MakeSlotSystemElement();

		stubEle_2.parent = stubEle_1;
		stubEle_1.parent = stubBundle_closer;
		stubBundle_closer.parent = stubBundle_further;
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		testSSE.parent = stubEle_2;

		Assert.That(testSSE.immediateBundle, Is.SameAs(stubBundle_closer));
	}
	[Test]
	public void level_ParentNull_ReturnsZero(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();

		Assert.That(testSSE.level, Is.EqualTo(0));
	}
	[Test]
	public void level_OneParent_ReturnsOne(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		TestSlotSystemElement stubEle = MakeSlotSystemElement();
		testSSE.parent = stubEle;

		Assert.That(testSSE.level, Is.EqualTo(1));
	}
	[Test]
	public void level_TwoParent_ReturnsTwo(){
		TestSlotSystemElement stubEle_1 = MakeSlotSystemElement();
		TestSlotSystemElement stubEle_2 = MakeSlotSystemElement();
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		stubEle_2.parent = stubEle_1;
		testSSE.parent = stubEle_2;

		Assert.That(testSSE.level, Is.EqualTo(2));
	}
	[Test]
	public void isBundleElement_ParentIsBundle_ReturnsTrue(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		SlotSystemBundle stubBundle = MakeSlotSystemBundle();
		testSSE.parent = stubBundle;

		Assert.That(testSSE.isBundleElement, Is.True);
	}
	[Test]
	public void isBundleElement_ParentIsNotBundle_ReturnsFalse(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		TestSlotSystemElement stubSSE = MakeSlotSystemElement();
		testSSE.parent = stubSSE;

		Assert.That(testSSE.isBundleElement, Is.False);
	}
	[Test]
	public void isPageElement_ParentIsPage_ReturnsTrue(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		TestSlotSystemPage stubPage = MakeSlotSystemPage();
		testSSE.parent = stubPage;

		Assert.That(testSSE.isPageElement, Is.True);
	}
	[Test]
	public void isPageElement_ParentIsNotPage_ReturnsFalse(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		TestSlotSystemElement stubSSE = MakeSlotSystemElement();
		testSSE.parent = stubSSE;

		Assert.That(testSSE.isPageElement, Is.False);
	}
	[Test]
	public void isToggledOn_ParentNotPage_ReturnsFalse(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		TestSlotSystemElement stubSSE = MakeSlotSystemElement();
		testSSE.parent = stubSSE;

		Assert.That(testSSE.isToggledOn, Is.False);
	}
	[Test]
	public void isToggledOn_ParentIsPageAndElementToggledOn_ReturnsTrue(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		SlotSystemPage stubPage = Substitute.For<SlotSystemPage>();
		SlotSystemPageElement stubPageElement = MakeSlotSystemPageElement(testSSE, true);
		stubPage.GetPageElement(Arg.Any<SlotSystemElement>()).Returns(stubPageElement);
		testSSE.parent = stubPage;

		Assert.That(testSSE.isToggledOn, Is.True);
	}
	[Test]
	public void isToggledOn_ParentIsPageAndElementToggledOff_ReturnsFalse(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		SlotSystemPage stubPage = Substitute.For<SlotSystemPage>();
		SlotSystemPageElement stubPageElement = MakeSlotSystemPageElement(testSSE, false);
		stubPage.GetPageElement(Arg.Any<SlotSystemElement>()).Returns(stubPageElement);
		testSSE.parent = stubPage;

		Assert.That(testSSE.isToggledOn, Is.False);
	}
	[Test]
	public void isFocused_CurSelStateIsFocused_ReturnsTrue(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		testSSE.SetSelState(AbsSlotSystemElement.focusedState);

		Assert.That(testSSE.isFocused, Is.True);
		Assert.That(testSSE.isDefocused, Is.False);
		Assert.That(testSSE.isDeactivated, Is.False);
	}
	[Test]
	public void isDefocused_CurSelStateIsDefocused_ReturnsTrue(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		testSSE.SetSelState(AbsSlotSystemElement.defocusedState);

		Assert.That(testSSE.isFocused, Is.False);
		Assert.That(testSSE.isDefocused, Is.True);
		Assert.That(testSSE.isDeactivated, Is.False);
	}
	[Test]
	public void isDeactivated_CurSelStateIsDeactivated_ReturnsTrue(){
		TestSlotSystemElement testSSE = MakeSlotSystemElement();
		testSSE.SetSelState(AbsSlotSystemElement.deactivatedState);

		Assert.That(testSSE.isFocused, Is.False);
		Assert.That(testSSE.isDefocused, Is.False);
		Assert.That(testSSE.isDeactivated, Is.True);
	}

	class TestSlotSystemElement: AbsSlotSystemElement{
		protected override IEnumerable<SlotSystemElement> elements{get{return m_elements;}}
		IEnumerable<SlotSystemElement> m_elements;
	}
	TestSlotSystemElement MakeSlotSystemElement(){
		GameObject sseGO = new GameObject("sseGO");
		return sseGO.AddComponent<TestSlotSystemElement>();
	}
	SlotSystemBundle MakeSlotSystemBundle(){
		return new GameObject("ssBundleGO").AddComponent<SlotSystemBundle>();
	}
	TestSlotSystemPage MakeSlotSystemPage(){
		return new GameObject("pageGO").AddComponent<TestSlotSystemPage>();
	}
	class TestSlotSystemPage: SlotSystemPage{
		protected override IEnumerable<SlotSystemElement> elements{get{return m_elements;}}
		IEnumerable<SlotSystemElement> m_elements;
	}
	SlotSystemPageElement MakeSlotSystemPageElement(SlotSystemElement ele, bool isFocusToggledOn){
		return new SlotSystemPageElement(ele, isFocusToggledOn);
	}
}
