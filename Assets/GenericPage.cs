using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class GenericPage : AbsSlotSystemElement{
		protected override IEnumerable<SlotSystemElement> elements{
				get{return m_elements;}
				}IEnumerable<SlotSystemElement> m_elements;
		public void Initialize(IEnumerable<SlotSystemElement> elements){
			m_eName = Util.Bold("genPage");
			m_elements = elements;
			base.Initialize();
		}	
	}
}
