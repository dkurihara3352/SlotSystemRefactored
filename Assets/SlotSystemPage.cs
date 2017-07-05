using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public abstract class SlotSystemPage : AbsSlotSystemElement{
		public void PageFocus(){
			foreach(SlotSystemPageElement pageEle in pageElements){
				if(pageEle.isFocusToggleOn)
					pageEle.element.Focus();
				else
					pageEle.element.Defocus();
			}
		}
		public void ToggleBack(){
			foreach(SlotSystemPageElement pageEle in pageElements){
				pageEle.isFocusToggleOn = pageEle.isFocusedOnActivate;
			}
		}
		public void TogglePageElementFocus(SlotSystemElement ele, bool toggle){
			foreach(SlotSystemPageElement pageEle in pageElements){
				if(pageEle.element == ele){
					if(toggle && !pageEle.isFocusToggleOn){
						pageEle.isFocusToggleOn = true;
					}else if(!toggle && pageEle.isFocusToggleOn){
						pageEle.isFocusToggleOn = false;
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
		public virtual SlotSystemPageElement GetPageElement(SlotSystemElement element){
			foreach(SlotSystemPageElement pageEle in pageElements){
				if(pageEle.element == element)
					return pageEle;
			}
			return null;
		}
	}
}
