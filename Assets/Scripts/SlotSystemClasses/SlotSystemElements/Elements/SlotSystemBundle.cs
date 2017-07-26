﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SlotSystemBundle : SlotSystemElement, ISlotSystemBundle{
		/*	fields	*/
		public ISlotSystemElement focusedElement{
			get{return m_focusedElement;}
			}ISlotSystemElement m_focusedElement = null;
			public void SetFocusedBundleElement(ISlotSystemElement element){
				if(this.Contains(element))
					m_focusedElement = element;
				else
					throw new System.InvalidOperationException("SlotSystemBundleMB.SetFocusedBundleElement: trying to set focsed element that is not one of its members");
			}
		/*	methods	*/
		public void Initialize(string name, IEnumerable<ISlotSystemElement> elements){
			m_eName = SlotSystemUtil.Bold(name);
			m_elements = elements;
			InitializeStates();
		}
		public override void Focus(){
			base.Focus();
			if(m_focusedElement != null)
				m_focusedElement.Focus();
			foreach(ISlotSystemElement ele in this){
				if(ele != m_focusedElement)
				ele.Defocus();
			}
		}
		public override void Defocus(){
			base.Defocus();
			foreach(ISlotSystemElement ele in this){
				ele.Defocus();
			}
		}	
	}
	public interface ISlotSystemBundle: ISlotSystemElement{
		ISlotSystemElement focusedElement{get;}
		void SetFocusedBundleElement(ISlotSystemElement element);
	}
}
