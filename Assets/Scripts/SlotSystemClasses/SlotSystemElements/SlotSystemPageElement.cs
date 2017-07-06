using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SlotSystemPageElement{
		public SlotSystemElement element{
			get{return m_element;}
			}SlotSystemElement m_element;
		public bool isFocusedOnActivate{
			get{return m_isFocusedOnActivate;}
			}bool m_isFocusedOnActivate;
		public bool isFocusToggleOn{
			get{return m_isFocusToggleOn;}
			set{m_isFocusToggleOn = value;}
			}bool m_isFocusToggleOn;
		public SlotSystemPageElement(SlotSystemElement element, bool isFocusToggleOn){
			m_element = element;
			m_isFocusToggleOn = isFocusToggleOn;
			m_isFocusedOnActivate = isFocusToggleOn;				
		}
	}
}