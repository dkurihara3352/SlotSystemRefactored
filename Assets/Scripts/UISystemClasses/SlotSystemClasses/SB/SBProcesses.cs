using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public abstract class SBProcess: UIProcess{
		public SBProcess(Func<IEnumeratorFake> coroutine): base(coroutine){
		}
	}
	/* Sel Proc */
		public interface ISBSelProcess: IUIProcess{}
			public class SBMakeSelectableProcess: SBProcess, ISBSelProcess{
				public SBMakeSelectableProcess(Func<IEnumeratorFake> coroutine): base(coroutine){}
			}
			public class SBMakeUnselectableProcess: SBProcess, ISBSelProcess{
				public SBMakeUnselectableProcess(Func<IEnumeratorFake> coroutine): base(coroutine){}
			}
			public class SBSelectProcess: SBProcess, ISBSelProcess{
				public SBSelectProcess(Func<IEnumeratorFake> coroutine): base(coroutine){}
			}
		public interface ISBSelCoroutineRepo{
			Func<IEnumeratorFake> MakeSelectableCoroutine();
			Func<IEnumeratorFake> MakeUnselectableCoroutine();
			Func<IEnumeratorFake> SelectCoroutine();
		}
		public class SBSelCoroutineRepo: ISBSelCoroutineRepo{
			public Func<IEnumeratorFake> MakeSelectableCoroutine(){
				return _makeSelectableCoroutine;
			}
				IEnumeratorFake _makeSelectableCoroutine(){
					return null;
				}
			public Func<IEnumeratorFake> MakeUnselectableCoroutine(){
				return _makeUnselectableCoroutine;
			}
				IEnumeratorFake _makeUnselectableCoroutine(){
					return null;
				}
			public Func<IEnumeratorFake> SelectCoroutine(){
				return _selectCoroutine;
			}
				IEnumeratorFake _selectCoroutine(){
					return null;
				}
		}
	/* Act Proc */
		public interface ISBActProcess: IUIProcess{}
			
			public class SBWaitForActionProcess: SBProcess, ISBActProcess{
				public SBWaitForActionProcess(Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
			public class SBTravelProcess: SBProcess, ISBActProcess{
				public SBTravelProcess(Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
			public class SBLiftProcess: SBProcess, ISBActProcess{
				public SBLiftProcess(Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
			public class SBLandProcess: SBProcess, ISBActProcess{
				public SBLandProcess(Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
			public class SBAppearProcess: SBProcess, ISBActProcess{
				public SBAppearProcess(Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
			public class SBDisappearProcess: SBProcess, ISBActProcess{
				public SBDisappearProcess(Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
		public interface ISBEqpProcess: IUIProcess{}
			public class SBEquipProcess: SBProcess, ISBEqpProcess{
				public SBEquipProcess(Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
			public class SBUnequipProcess: SBProcess, ISBEqpProcess{
				public SBUnequipProcess(Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
	public class SBActCoroutineRepo: ISBActCoroutineRepo{
		public Func<IEnumeratorFake> WaitForActionCoroutine(){
			return _waitForActionCoroutine;
		}
			IEnumeratorFake _waitForActionCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> TravelCoroutine(){
			return _travelCoroutine;
		}
			IEnumeratorFake _travelCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> LiftCoroutine(){
			return _liftCoroutine;
		}
			IEnumeratorFake _liftCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> LandCoroutine(){
			return _landCoroutine;
		}
			IEnumeratorFake _landCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> AppearCoroutine(){
			return _appearCoroutine;
		}
			IEnumeratorFake _appearCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> DisappearCoroutine(){
			return _disappearCoroutine;
		}
			IEnumeratorFake _disappearCoroutine(){
				return null;
			}
	}
	public interface ISBActCoroutineRepo{
		Func<IEnumeratorFake> WaitForActionCoroutine();
		Func<IEnumeratorFake> TravelCoroutine();
		Func<IEnumeratorFake> LiftCoroutine();
		Func<IEnumeratorFake> LandCoroutine();
		Func<IEnumeratorFake> AppearCoroutine();
		Func<IEnumeratorFake> DisappearCoroutine();
	}
	public class SBEqpCoroutineRepo: ISBEqpCoroutineRepo{
		public Func<IEnumeratorFake> GetEquipCoroutine(){
			return _equipCoroutine;
		}
			IEnumeratorFake _equipCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> GetUnequipCoroutine(){
			return _unequipCoroutine;
		}
			IEnumeratorFake _unequipCoroutine(){
				return null;
			}
	}
	public interface ISBEqpCoroutineRepo{
		Func<IEnumeratorFake> GetEquipCoroutine();
		Func<IEnumeratorFake> GetUnequipCoroutine();
	}
}