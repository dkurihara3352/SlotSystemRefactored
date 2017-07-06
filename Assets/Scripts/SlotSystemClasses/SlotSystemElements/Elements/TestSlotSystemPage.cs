using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TestSlotSystemPage: SlotSystemPage{
		protected override IEnumerable<SlotSystemElement> elements{get{return m_elements;}}
		IEnumerable<SlotSystemElement> m_elements;
	}
}
