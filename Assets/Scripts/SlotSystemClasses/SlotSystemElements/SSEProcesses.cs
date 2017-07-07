using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface SSEProcess{
		bool isRunning{get;}
		System.Func<IEnumeratorFake> coroutineFake{set;}
		void Start();
		void Stop();
		void Expire();
	}
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
		public abstract class SSESelProcess: AbsSSEProcess{}
			public class SSEGreyinProcess: SSESelProcess{
				public SSEGreyinProcess(SlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = sse;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSEGreyoutProcess: SSESelProcess{
				public SSEGreyoutProcess(SlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = sse;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSEHighlightProcess: SSESelProcess{
				public SSEHighlightProcess(SlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = sse;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSEDehighlightProcess: SSESelProcess{
				public SSEDehighlightProcess(SlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = sse;
					this.coroutineFake = coroutineMock;
				}
			}
		public abstract class SSEActProcess: AbsSSEProcess{}
}