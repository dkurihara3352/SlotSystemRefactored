using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility{
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
		public static List<IEnumerable<T>> Permutations<T>(IEnumerable<T> elements){
			List<IEnumerable<T>> res = new List<IEnumerable<T>>();
			int count = 0;
			foreach(var ele in elements)
				count ++;
			Generate(count, ref elements, ref res);
			return res;
		}
			public static void Generate<T>(int n, ref IEnumerable<T> ie, ref List<IEnumerable<T>> result){
				if(n == 1){
					result.Add(ie);
				}
				else{
					for(int i = 0; i < n -1; i++){
						Generate(n -1, ref ie, ref result);
						int resid = n%2;
						if(resid == 0){
							ie = SwappedList(ie, i, n-1);
						}else{
							ie = SwappedList(ie, 0, n-1);
						}
					}
					Generate(n-1, ref ie, ref result);
				}
			}
			public static IEnumerable<T> SwappedList<T>(IEnumerable<T> ie, int i, int j){
				List<T> result = new List<T>(ie);
				T temp = result[i];
				result[i] = result[j];
				result[j] = temp;
				return result;
			}
		private static System.Random rng = new System.Random();
		public static void Reorder<T>(this IList<T> list, T ele, T other){
			bool toRight = list.IndexOf(ele) < list.IndexOf(other);
			list.Remove(ele);
			int insertedIndex = toRight?list.IndexOf(other) + 1:list.IndexOf(other);
			list.Insert(insertedIndex, ele);
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
		public static void Trim<T>(this List<T> list){
			list.RemoveAll(element => element == null);
		}
		public static void Fill<T>(this IList<T> list, T element){
			foreach(var ele in list){
				if(ele == null){
					list[list.IndexOf(ele)] = element;
					return;
				}
			}
			list.Add(element);
		}
		public static int Count<T>(this IEnumerable<T> coll){
			IEnumerator rator = coll.GetEnumerator();
			int count = 0;
			while(rator.MoveNext())
				count++;
			return count;
		}
		public static bool MemberEquals<T>(this IEnumerable<T> coll, IEnumerable<T> other){
			if(coll ==null){
				if(other == null)
					return true;
				else
					return false;
			}
			else{
				if(other == null) return false;
				else{
					if(coll.Count() != other.Count()) return false;
					IEnumerator rator = coll.GetEnumerator();
					IEnumerator otherRator = other.GetEnumerator();
					while(rator.MoveNext()&& otherRator.MoveNext()){
						if(rator.Current == null){
							if(otherRator.Current == null)
								continue;
							else
								return false;
						}
						else{
							if(otherRator.Current == null)
								return false;
							else
								if(rator.Current.Equals(otherRator.Current))
									continue;
								else
									return false;
						}
					}
					return true;
				}
			}
		}
	}
}
	
