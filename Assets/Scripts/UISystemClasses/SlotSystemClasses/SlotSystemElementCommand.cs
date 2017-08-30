using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface ISBEventArgsCommand: ISlotSystemElementCommand{
		void Execute(ISlottable pickedSB);
	}
	public interface ISGEventArgsCommand: ISlotSystemElementCommand{
		void Execute(ISlotGroup hoveredSG);
	}
	public interface ISlotEventArgsCommand: ISlotSystemElementCommand{
		void Execute(ISlot slot);
	}
	public interface ISlotSystemElementCommand{
	}
	public interface ISSEEventCommandsRepo{
		ISBEventArgsCommand OnSBPickedUpCommand();
		ISlotEventArgsCommand OnSlotHoverEnteredCommand();
		ISGEventArgsCommand OnSGHoverEnteredCommand();
		ISBEventArgsCommand OnSBDroppedCommand();
	}

	public class EmptySSECommandsRepo: ISSEEventCommandsRepo{
		public ISBEventArgsCommand OnSBPickedUpCommand(){
			if(_onSBPickedUpCommand == null)
				_onSBPickedUpCommand = new OnSBPickedUpCommand_empty();
			return _onSBPickedUpCommand;
		}
			ISBEventArgsCommand _onSBPickedUpCommand;
		public ISlotEventArgsCommand OnSlotHoverEnteredCommand(){
			if(_onSlotHoverEnteredCommand == null)
				_onSlotHoverEnteredCommand = new OnSlotHoverEnteredCommand_empty();
			return _onSlotHoverEnteredCommand;
		}
			ISlotEventArgsCommand _onSlotHoverEnteredCommand;
		public ISGEventArgsCommand OnSGHoverEnteredCommand(){
			if(_onSGHoverEnteredCommand == null)
				_onSGHoverEnteredCommand = new OnSGHoverEnteredCommand_empty();
			return _onSGHoverEnteredCommand;
		}
			ISGEventArgsCommand _onSGHoverEnteredCommand;
		public ISBEventArgsCommand OnSBDroppedCommand(){
			if(_onSBDroppedCommand == null)
				_onSBDroppedCommand = new OnSBDroppedCommand_empty();
			return _onSBDroppedCommand;
		}
			ISBEventArgsCommand _onSBDroppedCommand;
	}
		public class OnSBPickedUpCommand_empty: ISBEventArgsCommand{
			public void Execute(ISlottable sb){}
		}
		public class OnSlotHoverEnteredCommand_empty: ISlotEventArgsCommand{
			public void Execute(ISlot slot){}
		}
		public class OnSGHoverEnteredCommand_empty: ISGEventArgsCommand{
			public void Execute(ISlotGroup sg){}
		}
		public class OnSBDroppedCommand_empty: ISBEventArgsCommand{
			public void Execute(ISlottable sb){}
		}
	public interface ITapCommand{
		void Execute();
	}
	public class TapCommand: ITapCommand{
		ISlotSystemElement sse;
		public TapCommand(ISlotSystemElement sse){
			this.sse = sse;
		}
		public void Execute(){}
	}
}
