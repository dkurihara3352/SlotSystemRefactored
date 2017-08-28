using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class InstantCommands: IInstantCommands{
		public void ExecuteInstantFocus(){
			instantFocusCommand.Execute();
		}
			public IUICommand instantFocusCommand{
				get{
					if(m_instantFocusCommand == null)
						m_instantFocusCommand = new InstantFocusCommand();
					return m_instantFocusCommand;
				}
			}
				IUICommand m_instantFocusCommand;
		public void ExecuteInstantDefocus(){
			instantDefocusCommand.Execute();
		}
			public IUICommand instantDefocusCommand{
				get{
					if(m_instantDefocusCommand == null)
						m_instantDefocusCommand = new InstantDefocusCommand();
					return m_instantDefocusCommand;
				}
			}
				IUICommand m_instantDefocusCommand;
		public void ExecuteInstantSelect(){
			instantSelectCommand.Execute();
		}
			public IUICommand instantSelectCommand{
				get{
					if(m_instantSelectCommand == null)
						m_instantSelectCommand = new InstantSelectCommand();
					return m_instantSelectCommand;
				}
			}
				IUICommand m_instantSelectCommand;
	}
	public interface IInstantCommands{
		void ExecuteInstantFocus();
		void ExecuteInstantDefocus();
		void ExecuteInstantSelect();
	}
	public class InstantFocusCommand: IUICommand{
		public void Execute(){}
	}
	public class InstantDefocusCommand: IUICommand{
		public void Execute(){}
	}
	public class InstantSelectCommand: IUICommand{
		public void Execute(){}
	}
	public interface IUICommand{
		void Execute();
	}
}

