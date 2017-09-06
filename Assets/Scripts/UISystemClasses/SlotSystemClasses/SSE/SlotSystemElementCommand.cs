using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface ISSEEventArgsCommand: ISlotSystemElementCommand{
		void Execute(SSEEventArgs e);
	}
	public interface ISlotSystemElementCommand{
	}
	/* Event Commands Repo */
	public interface ISSEEventCommandsRepo{
		void InitializeCommands();
		ISSEEventArgsCommand OnSBPickedUpCommand();
		ISSEEventArgsCommand OnSBHoverEnteredCommand();
		ISSEEventArgsCommand OnSlotHoverEnteredCommand();
		ISSEEventArgsCommand OnSGHoverEnteredCommand();
		ISSEEventArgsCommand OnSBDroppedCommand();
	}
	public abstract class SSEEventCommandsRepo: ISSEEventCommandsRepo{
		public abstract void InitializeCommands();
		public ISSEEventArgsCommand OnSBPickedUpCommand(){
			return _onSBPickedUpCommand;
		}
		protected void SetOnItemPickedUpCommand(ISSEEventArgsCommand comm){
			_onSBPickedUpCommand = comm;
		}
			ISSEEventArgsCommand _onSBPickedUpCommand;
		public ISSEEventArgsCommand OnSBHoverEnteredCommand(){
			return _onSBHoverEnteredCommand;
		}
		protected void SetOnSBHoverEnteredCommand(ISSEEventArgsCommand comm){
			_onSBHoverEnteredCommand = comm;
		}
			ISSEEventArgsCommand _onSBHoverEnteredCommand;
		public ISSEEventArgsCommand OnSlotHoverEnteredCommand(){
			return _onSlotHoverEnteredCommand;
		}
		protected void SetOnSlotHoverEnteredCommand(ISSEEventArgsCommand comm){
			_onSlotHoverEnteredCommand = comm;
		}
			ISSEEventArgsCommand _onSlotHoverEnteredCommand;
		public ISSEEventArgsCommand OnSGHoverEnteredCommand(){
			return _onSGHoverEnteredCommand;
		}
		protected void SetOnSGHoverEnteredCommand(ISSEEventArgsCommand comm){
			_onSGHoverEnteredCommand = comm;
		}
			ISSEEventArgsCommand _onSGHoverEnteredCommand;
		public ISSEEventArgsCommand OnSBDroppedCommand(){
			return _onSBDroppedCommand;
		}
		protected void SetOnItemDroppedCommand(ISSEEventArgsCommand comm){
			_onSBDroppedCommand = comm;
		}
			ISSEEventArgsCommand _onSBDroppedCommand;
	}

	public class EmptySSECommandsRepo: SSEEventCommandsRepo{
		public override void InitializeCommands(){
			SetOnItemPickedUpCommand(new OnSBPickedUpCommand_empty());
			SetOnSBHoverEnteredCommand(new OnSBHoverEnteredCommand_empty());
			SetOnSlotHoverEnteredCommand(new OnSlotHoverEnteredCommand_empty());
			SetOnSGHoverEnteredCommand(new OnSGHoverEnteredCommand_empty());
			SetOnItemDroppedCommand(new OnSBDroppedCommand_empty());
		}
	}
		public class OnSBPickedUpCommand_empty: ISSEEventArgsCommand{
			public void Execute(SSEEventArgs e){}
		}
		public class OnSBHoverEnteredCommand_empty: ISSEEventArgsCommand{
			public void Execute(SSEEventArgs e){}
		}
		public class OnSlotHoverEnteredCommand_empty: ISSEEventArgsCommand{
			public void Execute(SSEEventArgs e){}
		}
		public class OnSGHoverEnteredCommand_empty: ISSEEventArgsCommand{
			public void Execute(SSEEventArgs e){}
		}
		public class OnSBDroppedCommand_empty: ISSEEventArgsCommand{
			public void Execute(SSEEventArgs e){}
		}
}
