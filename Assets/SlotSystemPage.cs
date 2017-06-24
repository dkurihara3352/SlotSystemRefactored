using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public abstract class SlotSystemPage : AbsSlotSystemElement{
		public void PageFocus(){
			//SetSelState in inherited class, then call this with base.Focus();
			foreach(SlotSystemPageElement pageEle in pageElements){
				if(pageEle.isFocusedOnActivate || pageEle.isFocusToggleOn)
					pageEle.element.Focus();
				else
					pageEle.element.Defocus();
			}
		}
		public void TogglePageElementFocus(SlotSystemElement ele, bool toggle){
			foreach(SlotSystemPageElement pageEle in pageElements){
				if(pageEle.element == ele){
					if(toggle && !pageEle.isFocusToggleOn){
						pageEle.isFocusToggleOn = true;
						// pageEle.element.Focus();
					}else if(!toggle && pageEle.isFocusToggleOn){
						pageEle.isFocusToggleOn = false;
						// pageEle.element.Defocus();
					}
				}
			}
			Focus();
		}
		public IEnumerable<SlotSystemPageElement> pageElements{
				get{
					return m_pageElements;
				}
			}protected IEnumerable<SlotSystemPageElement> m_pageElements;
	}
}
