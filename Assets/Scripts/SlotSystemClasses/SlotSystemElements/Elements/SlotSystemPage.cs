using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public abstract class SlotSystemPage : AbsSlotSystemElement, ISlotSystemPage{
		public void PageFocus(){
			foreach(ISlotSystemPageElement pageEle in pageElements){
				if(pageEle.isFocusToggleOn)
					pageEle.Focus();
				else
					pageEle.Defocus();
			}
		}
		public void ToggleBack(){
			foreach(ISlotSystemPageElement pageEle in pageElements){
				pageEle.isFocusToggleOn = pageEle.isFocusedOnActivate;
			}
		}
		public void TogglePageElementFocus(ISlotSystemElement ele, bool toggle){
			foreach(ISlotSystemPageElement pageEle in pageElements){
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
		public IEnumerable<ISlotSystemPageElement> pageElements{
				get{
					return m_pageElements;
				}
				set{m_pageElements = value;}
			}protected IEnumerable<ISlotSystemPageElement> m_pageElements;
		public virtual ISlotSystemPageElement GetPageElement(ISlotSystemElement element){
			foreach(ISlotSystemPageElement pageEle in pageElements){
				if(pageEle.element == element)
					return pageEle;
			}
			return null;
		}
	}
	public interface ISlotSystemPage: IAbsSlotSystemElement{
		IEnumerable<ISlotSystemPageElement> pageElements{get;set;}
		void PageFocus();
		void ToggleBack();
		void TogglePageElementFocus(ISlotSystemElement ele, bool toggle);
		ISlotSystemPageElement GetPageElement(ISlotSystemElement element);
	}
}
