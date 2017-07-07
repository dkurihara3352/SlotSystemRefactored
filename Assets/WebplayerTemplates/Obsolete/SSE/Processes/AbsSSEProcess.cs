using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class AbsSSEProcess: SSEProcess{
		public bool isRunning{
			get{return m_isRunning;}
			} bool m_isRunning;
		public System.Func<IEnumeratorFake> coroutineFake{
			set{m_coroutineMock = value;}
			}System.Func<IEnumeratorFake> m_coroutineMock;
		protected SlotSystemElement sse{
			get{return m_sse;}
			set{m_sse = value;}
			} SlotSystemElement m_sse;
		public virtual void Start(){
			m_isRunning = true;
			m_coroutineMock();
		}
		public virtual void Stop(){
			if(isRunning)
				m_isRunning = false;
		}
		public virtual void Expire(){
			if(isRunning)
				m_isRunning = false;
		}
	}
}