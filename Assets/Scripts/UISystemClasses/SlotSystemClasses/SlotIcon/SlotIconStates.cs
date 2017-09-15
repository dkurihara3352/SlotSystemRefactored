using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace UISystem{
	/* Increment States */
		public interface IIconIncrementState: ISwitchableState{
		}
		public abstract class IconIncrementState: IIconIncrementState{
			public IconIncrementState( IIconIncrementStateEngine stateEngine){
				SetStateEngine( stateEngine);
			}
			protected IIconIncrementStateEngine StateEngine(){
				return _stateEngine;
			}
			void SetStateEngine( IIconIncrementStateEngine stateEngine){
				_stateEngine = stateEngine;
			}
			IIconIncrementStateEngine _stateEngine;
			public abstract void Enter();
			public virtual void Exit(){}
			public abstract bool CanEnter();
			protected void RunWaitForIncrementProcess(){
				StateEngine().SetAndRunIconProcess( new IconWaitForIncrementProcess( StateEngine().WaitForIncrementCoroutine()));
			}
			protected void RunGetReadyForIncrementProcess(){
				StateEngine().SetAndRunIconProcess( new IconGetReadyForIncrementProcess( StateEngine().GetReadyForIncrementCoroutine()));
			}
		}
		public class IconWaitingForIncrementState: IconIncrementState{
			public IconWaitingForIncrementState( IIconIncrementStateEngine engine): base( engine){}
			public override bool CanEnter(){
				if( StateEngine().IsReadyForIncrement())
					return true;
				else
					return false;
			}
			public override void Enter(){
				if(StateEngine().WasReadyForIncrement())
					RunWaitForIncrementProcess();
			}
		}
		public class IconReadyForIncrementState: IconIncrementState{
			public IconReadyForIncrementState( IIconIncrementStateEngine engine): base( engine){}
			public override bool CanEnter(){
				if( StateEngine().IsWaitingForIncrement())
					return true;
				else
					return false;
			}
			public override void Enter(){
				if(StateEngine().WasWaitingForIncrement())
					RunGetReadyForIncrementProcess();
			}
		}
	/* Hover State */
		public interface IIconHoverState: ISwitchableState{
		}
		public abstract class IconHoverState: IIconHoverState{
			public IconHoverState( IIconHoverStateEngine stateEngine){
				SetStateEngine( stateEngine);
			}
			protected IIconHoverStateEngine StateEngine(){
				return _stateEngine;
			}
			void SetStateEngine( IIconHoverStateEngine stateEngine){
				_stateEngine = stateEngine;
			}
			IIconHoverStateEngine _stateEngine;
			public abstract void Enter();
			public virtual void Exit(){}
			public abstract bool CanEnter();
			protected void RunWaitForHoverProcess(){
				StateEngine().SetAndRunIconProcess( new IconDehoverProcess( StateEngine().DehoverCoroutine(), StateEngine()));
			}
			protected void RunGetReadyForHoverProcess(){
				StateEngine().SetAndRunIconProcess( new IconHoverProcess( StateEngine().HoverCoroutine()));
			}
		}
		public class IconDehoveringState: IconHoverState{
			public IconDehoveringState( IIconHoverStateEngine engine): base( engine){}
			public override bool CanEnter(){
				if( StateEngine().IsHovering())
					return true;
				else
					return false;
			}
			public override void Enter(){
				if(StateEngine().WasHovering())
					RunWaitForHoverProcess();
			}
		}
		public class IconHoveringState: IconHoverState{
			public IconHoveringState( IIconHoverStateEngine engine): base( engine){}
			public override bool CanEnter(){
				if( StateEngine().IsDehovering())
					return true;
				else
					return false;
			}
			public override void Enter(){
				if(StateEngine().WasDehovering())
					RunGetReadyForHoverProcess();
			}
		}
	/* Switch */
	public class IconIncrementStateSwitch: SwitchableStateSwitch<IIconIncrementState>{
	}
	public class IconHoverStateSwitch: SwitchableStateSwitch<IIconHoverState>{}
	/* Process */
	public class IconProcess: UIProcess{
		public IconProcess( IEnumeratorFake coroutine): base( coroutine){}
	}
	public class IconWaitForIncrementProcess: IconProcess{
		public IconWaitForIncrementProcess( IEnumeratorFake coroutine): base( coroutine){}
	}
	public class IconGetReadyForIncrementProcess: IconProcess{
		public IconGetReadyForIncrementProcess( IEnumeratorFake coroutine): base( coroutine){}
	}
	public class IconDehoverProcess: IconProcess{
		IIconHoverStateEngine hoverStateEngine;
		public IconDehoverProcess( IEnumeratorFake coroutine, IIconHoverStateEngine hoverStateEngine): base( coroutine){
			this.hoverStateEngine = hoverStateEngine;
		}
		public override void Expire(){
			hoverStateEngine.SwapItemBackToOriginal();
		}
	}
	public class IconHoverProcess: IconProcess{
		public IconHoverProcess( IEnumeratorFake coroutine): base( coroutine){}
	}
}

