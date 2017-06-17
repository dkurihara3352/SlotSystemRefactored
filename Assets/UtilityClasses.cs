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
		public static List<List<T>> Combinations<T>(int n, List<T> set){
			List<List<T>> result = new List<List<T>>();
			foreach(var intCombo in Combinations(n, set.Count)){
				List<T> tCombo = new List<T>();
				foreach(int i in intCombo){
					tCombo.Add(set[i-1]);
				}
				result.Add(tCombo);
			}
			return result;
		}
		public static IEnumerable<int[]> Combinations(int m, int n){
			int[] result = new int[m];
			Stack<int> stack = new Stack<int>();
			stack.Push(0);
			while (stack.Count > 0){
				int index = stack.Count - 1;
				int value = stack.Pop();
				while (value < n) {
					result[index++] = ++value;
					stack.Push(value);
					if (index == m) {
						yield return result;
						break;
					}
				}
			}
		}
		public static List<List<T>> Permutations<T>(List<T> elements){
			List<List<T>> res = new List<List<T>>();
			Generate(elements.Count, ref elements, ref res);
			return res;
		}
			public static void Generate<T>(int n, ref List<T> list, ref List<List<T>> result){
				if(n == 1){
					result.Add(list);
				}
				else{
					for(int i = 0; i < n -1; i++){
						Generate(n -1, ref list, ref result);
						int resid = n%2;
						if(resid == 0){
							list = SwappedList(list, i, n-1);
						}else{
							list = SwappedList(list, 0, n-1);
						}
					}
					Generate(n-1, ref list, ref result);
				}
			}
			public static List<T> SwappedList<T>(List<T> list, int a, int b){
				List<T> result = new List<T>(list);
				T temp = result[a];
				result[a] = result[b];
				result[b] = temp;
				return result;
			}
		private static System.Random rng = new System.Random();
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
