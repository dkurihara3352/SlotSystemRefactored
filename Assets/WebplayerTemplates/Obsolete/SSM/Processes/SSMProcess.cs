using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SSMProcess: AbsSSEProcess{
		protected SlotSystemManager ssm{
			get{return (SlotSystemManager)sse;}
		}
	}
}
