using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public abstract class SlotSystemPage : AbsSlotSystemElement{
		public void PageFocus(){
			//SetSelState in inherited class, then call this with base.Focus();
			foreach(SlotSystemPageElement pageEle in pageElements){
				// if(pageEle.isFocusedOnActivate || pageEle.isFocusToggleOn)
				if(pageEle.isFocusToggleOn)
					pageEle.element.Focus();
				else
					pageEle.element.Defocus();
			}
		}
		public void ToggleBack(){
			//call this in Deactivate
			foreach(SlotSystemPageElement pageEle in pageElements){
				pageEle.isFocusToggleOn = pageEle.isFocusedOnActivate;
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
			ssm.Focus();
		}
		public IEnumerable<SlotSystemPageElement> pageElements{
				get{
					return m_pageElements;
				}
			}protected IEnumerable<SlotSystemPageElement> m_pageElements;
		public SlotSystemPageElement GetPageElement(SlotSystemElement element){
			foreach(SlotSystemPageElement pageEle in pageElements){
				if(pageEle.element == element)
					return pageEle;
			}
			return null;
		}
	}
}
