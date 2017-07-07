using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SSMState: SSEState{
		protected SlotSystemManager ssm{
			get{
				return (SlotSystemManager)base.sse;
			}
		}
	}
}
