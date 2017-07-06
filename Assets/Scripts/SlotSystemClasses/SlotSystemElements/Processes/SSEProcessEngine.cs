using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SSEProcessEngine{
		public SSEProcess process{
			get{return m_process;}
			}SSEProcess m_process;
		public void SetAndRunProcess(SSEProcess process){
			if(process != null)
				process.Stop();
			m_process = process;
			if(process != null)
				process.Start();
		}
	}
}
