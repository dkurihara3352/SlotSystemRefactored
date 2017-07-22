using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SSEProcess: ISSEProcess{
		public virtual bool isRunning{ get{return m_isRunning;} }bool m_isRunning;
		public System.Func<IEnumeratorFake> coroutineFake{ set{m_coroutineMock = value;}} System.Func<IEnumeratorFake> m_coroutineMock;
		protected ISlotSystemElement sse{ get{return m_sse;} set{m_sse = value;}} ISlotSystemElement m_sse;
		public virtual void Start(){
			m_isRunning = true;
			m_coroutineMock();
		}
		public virtual void Stop(){ if(isRunning) m_isRunning = false; }
		public virtual void Expire(){if(isRunning) m_isRunning = false; }
	}
		public interface ISSEProcess{
			bool isRunning{get;}
			System.Func<IEnumeratorFake> coroutineFake{set;}
			void Start();
			void Stop();
			void Expire();
		}
	public class SSEProcessEngine<T>: ISSEProcessEngine<T> where T: ISSEProcess{
		public virtual T process{ get{return m_process;}}protected T m_process;
		public virtual void SetAndRunProcess(T process){
			if(process != null)
				process.Stop();
			m_process = process;
			if(process != null)
				process.Start();
		}
	}
		public interface ISSEProcessEngine<T> where T: ISSEProcess{
			T process{get;}
			void SetAndRunProcess(T process);
		}
		public abstract class SSESelProcess: SSEProcess, ISSESelProcess{}
		public interface ISSESelProcess: ISSEProcess{}
			public class SSEDeactivateProcess: SSESelProcess{
				/* 	Change color, alpha etc from whatever to deactivated value (probably make it disappear)
					if any indicator for selection is there, fade it out
				*/
				public SSEDeactivateProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = sse;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSEFocusProcess: SSESelProcess{
				/* 	Change color, alpha etc from whatever to focus value
					if its hidden, make it appear gradually
					if any indicator for selection is there, fade it out
				*/
				public SSEFocusProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = sse;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSEDefocusProcess: SSESelProcess{
				/* 	Change color, alpha etc from whatever to defocus value
					if its hidden, make it appear gradually
					if any indicator for selection is there, fade it out
				*/
				public SSEDefocusProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = sse;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSESelectProcess: SSESelProcess{
				/* 	Change color, alpha etc from whatever to select value
					if its hidden, make it appear gradually
					if any indicator for selection is not there, fade it in
				*/
				public SSESelectProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = sse;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSEGreyinProcess: SSESelProcess{
				public SSEGreyinProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = sse;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSEGreyoutProcess: SSESelProcess{
				public SSEGreyoutProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = sse;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSEHighlightProcess: SSESelProcess{
				public SSEHighlightProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = sse;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSEDehighlightProcess: SSESelProcess{
				public SSEDehighlightProcess(ISlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = sse;
					this.coroutineFake = coroutineMock;
				}
			}
}