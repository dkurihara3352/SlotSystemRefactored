using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class InstantCommands: IInstantCommands{
		public void ExecuteInstantFocus(){
			instantFocusCommand.Execute();
		}
			public ISSECommand instantFocusCommand{
				get{
					if(m_instantFocusCommand == null)
						m_instantFocusCommand = new InstantFocusCommand();
					return m_instantFocusCommand;
				}
			}
				ISSECommand m_instantFocusCommand;
		public void ExecuteInstantDefocus(){
			instantDefocusCommand.Execute();
		}
			public ISSECommand instantDefocusCommand{
				get{
					if(m_instantDefocusCommand == null)
						m_instantDefocusCommand = new InstantDefocusCommand();
					return m_instantDefocusCommand;
				}
			}
				ISSECommand m_instantDefocusCommand;
		public void ExecuteInstantSelect(){
			instantSelectCommand.Execute();
		}
			public ISSECommand instantSelectCommand{
				get{
					if(m_instantSelectCommand == null)
						m_instantSelectCommand = new InstantSelectCommand();
					return m_instantSelectCommand;
				}
			}
				ISSECommand m_instantSelectCommand;
	}
	public interface IInstantCommands{
		void ExecuteInstantFocus();
		void ExecuteInstantDefocus();
		void ExecuteInstantSelect();
	}
	public class InstantFocusCommand: ISSECommand{
		public void Execute(){}
	}
	public class InstantDefocusCommand: ISSECommand{
		public void Execute(){}
	}
	public class InstantSelectCommand: ISSECommand{
		public void Execute(){}
	}
	public interface ISSECommand{
		void Execute();
	}
}

