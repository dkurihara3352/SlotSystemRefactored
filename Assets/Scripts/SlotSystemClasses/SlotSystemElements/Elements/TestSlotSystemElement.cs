using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TestSlotSystemElement: SlotSystemElement{
		public string message = "";
		public override void InstantDefocus(){
			message = "InstantDefocus called";
		}
		public override void InstantFocus(){
			message = "InstantFocus called";
		}
		public override void InstantSelect(){
			message = "InstantSelect called";
		}
	}
}
