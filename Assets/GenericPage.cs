using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class GenericPage : AbsSlotSystemElement{
		protected override IEnumerable<SlotSystemElement> elements{
				get{return m_elements;}
				}IEnumerable<SlotSystemElement> m_elements;
		public void Initialize(string name, IEnumerable<SlotSystemElement> elements){
			m_eName = Util.Bold(name);
			m_elements = elements;
			base.Initialize();
		}	
	}
}
