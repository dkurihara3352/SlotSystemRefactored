using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class SlotSystemElement : UIElement, ISlotSystemElement{
		ISlotSystemManager GetSlotSystemManager(){
			return _ssm;
		}
		public void SetSSM(ISlotSystemManager ssm){
			_ssm = ssm;
		}
			ISlotSystemManager _ssm;
		public SlotSystemElement(){
			ISlotSystemManager ssm = GetSlotSystemManager();
			ssm.SBPickedUp += OnSBPickedUp;
			ssm.SlotHoverEntered += OnSlotHoverEntered;
			ssm.SGHoverEntered += OnSGHoverEntered;
			ssm.SBDropped += OnSBDropped;
		}
		~SlotSystemElement(){
			ISlotSystemManager ssm = GetSlotSystemManager();
			ssm.SBPickedUp -= OnSBPickedUp;
			ssm.SlotHoverEntered -= OnSlotHoverEntered;
			ssm.SGHoverEntered -= OnSGHoverEntered;
			ssm.SBDropped -= OnSBDropped;
		}
		public void OnSBPickedUp(object ssm, SBEventArgs e){
			OnSBPickUpCommand().Execute(e.slottable);
		}
			SBEventArgsCommand OnSBPickUpCommand(){
				Debug.Assert(_onSBPickUpCommand != null);
				return _onSBPickUpCommand;
			}
			SBEventArgsCommand _onSBPickUpCommand;
			public void SetOnSBPickUpCommand(SBEventArgsCommand comm){
				_onSBPickUpCommand = comm;
			}
		public void OnSlotHoverEntered(object ssm, SlotEventArgs e){
			OnSlotHoverEnterCommand().Execute(e.slot);
		}
			SlotEventArgsCommand OnSlotHoverEnterCommand(){
				Debug.Assert(_onSlotHoverEnterCommand != null);
				return _onSlotHoverEnterCommand;
			}
			SlotEventArgsCommand _onSlotHoverEnterCommand;
			public void SetOnSlotHoverEnterCommand(SlotEventArgsCommand comm){
				_onSlotHoverEnterCommand = comm;
			}
		public void OnSGHoverEntered(object ssm, SGEventArgs e){
			OnSGHoverEnterCommand().Execute(e.slotGroup);
		}
			SGEventArgsCommand OnSGHoverEnterCommand(){
				Debug.Assert(_onSGHoverEnterCommand != null);
				return _onSGHoverEnterCommand;
			}
			SGEventArgsCommand _onSGHoverEnterCommand;
			public void SetOnSGHoverEnterCommand(SGEventArgsCommand comm){
				_onSGHoverEnterCommand = comm;
			}
		public void OnSBDropped(object ssm, SBEventArgs e){
			OnSBDropCommand().Execute(e.slottable);
		}
			SBEventArgsCommand OnSBDropCommand(){
				Debug.Assert(_onSBDropCommand != null);
				return _onSBDropCommand;
			}
			SBEventArgsCommand _onSBDropCommand;
			public void SetOnSBDropCommand(SBEventArgsCommand comm){
				_onSBDropCommand = comm;
			}
	}
	public interface ISlotSystemElement{
		void SetSSM(ISlotSystemManager ssm);
		void OnSBPickedUp(object source, SBEventArgs e);
		void OnSlotHoverEntered(object source, SlotEventArgs e);
		void OnSGHoverEntered(object source, SGEventArgs e);
		void OnSBDropped(object source, SBEventArgs e);
	}
}
