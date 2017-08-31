using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class SlotSystemElement : UIElement, ISlotSystemElement{
		public ISlotSystemManager SSM(){
			return _ssm;
		}
		public void SetSSM(ISlotSystemManager ssm){
			_ssm = ssm;
		}
			ISlotSystemManager _ssm;
		public SlotSystemElement(RectTransformFake rectTrans, ISSEEventCommandsRepo repo): base(rectTrans){
			ISlotSystemManager ssm = SSM();
			SetSSEEventCommandsRepo(repo);
			ssm.SBPickedUp += OnSBPickedUp;
			ssm.SlotHoverEntered += OnSlotHoverEntered;
			ssm.SGHoverEntered += OnSGHoverEntered;
			ssm.SBDropped += OnSBDropped;
		}
		~SlotSystemElement(){
			ISlotSystemManager ssm = SSM();
			ssm.SBPickedUp -= OnSBPickedUp;
			ssm.SlotHoverEntered -= OnSlotHoverEntered;
			ssm.SGHoverEntered -= OnSGHoverEntered;
			ssm.SBDropped -= OnSBDropped;
		}
		public virtual void HoverEnter(){}
		public virtual bool IsHovered(){return false;}
		ISSEEventCommandsRepo CommandsRepo(){
			Debug.Assert(_commandsRepo != null);
			return _commandsRepo;
		}
			protected ISSEEventCommandsRepo _commandsRepo;
		public void SetSSEEventCommandsRepo(ISSEEventCommandsRepo repo){
			_commandsRepo = repo;
		}
		public void OnSBPickedUp(object ssm, SBEventArgs e){
			OnSBPickUpCommand().Execute(e.slottable);
		}
			ISBEventArgsCommand OnSBPickUpCommand(){
				Debug.Assert(CommandsRepo() != null);
				return CommandsRepo().OnSBPickedUpCommand();
			}
		public void OnSlotHoverEntered(object ssm, SlotEventArgs e){
			OnSlotHoverEnterCommand().Execute(e.slot);
		}
			ISlotEventArgsCommand OnSlotHoverEnterCommand(){
				Debug.Assert(CommandsRepo() != null);
				return CommandsRepo().OnSlotHoverEnteredCommand();
			}
		public void OnSGHoverEntered(object ssm, SGEventArgs e){
			OnSGHoverEnterCommand().Execute(e.slotGroup);
		}
			ISGEventArgsCommand OnSGHoverEnterCommand(){
				Debug.Assert(CommandsRepo() != null);
				return CommandsRepo().OnSGHoverEnteredCommand();
			}
		public void OnSBDropped(object ssm, SBEventArgs e){
			OnSBDropCommand().Execute(e.slottable);
		}
			ISBEventArgsCommand OnSBDropCommand(){
				Debug.Assert(CommandsRepo() != null);
				return CommandsRepo().OnSBDroppedCommand();
			}
	}
	public interface ISlotSystemElement: IUIElement{
		ISlotSystemManager SSM();
		void SetSSM(ISlotSystemManager ssm);
		void HoverEnter();
		bool IsHovered();
		void SetSSEEventCommandsRepo(ISSEEventCommandsRepo repo);
		void OnSBPickedUp(object source, SBEventArgs e);
		void OnSlotHoverEntered(object source, SlotEventArgs e);
		void OnSGHoverEntered(object source, SGEventArgs e);
		void OnSBDropped(object source, SBEventArgs e);
	}
}
