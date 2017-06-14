using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility{
	public class UtilityClasses{
	}
	public interface StateHandler{}
	public interface SwitchableState{
		void EnterState(StateHandler handler);
		void ExitState(StateHandler handler);
	}
	public abstract class SwitchableStateEngine{
		protected StateHandler handler;
		public SwitchableState prevState;
		public SwitchableState curState;
		protected void SetState(SwitchableState state){
			if(curState != null){
				if(curState != state){
					curState.ExitState(handler);
					prevState = curState;
				}
				curState = state;
				if(curState != null){
					curState.EnterState(handler);
				}
			}else{// used as initialization
				prevState = state;
				curState = state;
				if(state != null)
					state.EnterState(handler);
			}
		}
	} 
	public static class ListMethods{
		private static System.Random rng = new System.Random();
		// public static void Shuffle<T>(IList<T> list){
		// 	int n = list.Count;  
		// 	while (n > 1) {  
		// 		n--;  
		// 		int k = rng.Next(n + 1);  
		// 		T value = list[k];  
		// 		list[k] = list[n];  
		// 		list[n] = value;  
		// 	}  
		// }
		public static void Reorder<T>(this IList<T> list, T ele, T other){
			list.Remove(ele);
			list.Insert(list.IndexOf(other) + 1,ele);
		}
		public static void Shuffle<T>(this IList<T> list)
		{  
			int n = list.Count;  
			while (n > 1) {  
				n--;  
				int k = rng.Next(n + 1);  
				T value = list[k];  
				list[k] = list[n];  
				list[n] = value;  
			}  
		}
	}
}
