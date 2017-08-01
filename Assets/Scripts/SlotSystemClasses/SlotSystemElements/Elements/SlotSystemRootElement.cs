using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SlotSystemRootElement : SlotSystemElement{
		public override bool isBundleElement{
			get{return false;}
		}
		public override bool isFocusedInHierarchy{
			get{return false;}
		}
		public override ISlotSystemBundle immediateBundle{
			get{return null;}
		}
		public override ISlotSystemElement parent{
			get{return null;}
		}
		public void SetElements(){
		}
		public override void SetParent(ISlotSystemElement par){
		}
		public override void SetHierarchy(){
		}
		public override bool Contains(ISlotSystemElement element){
			return false;
		}
		public override bool ContainsInHierarchy(ISlotSystemElement ele){
			return false;
		}
		public override bool isActivatedOnDefault{
			get{return true;}
			set{}
		}
		public override bool isFocusableInHierarchy{
			get{return true;}
		}
	}
}
