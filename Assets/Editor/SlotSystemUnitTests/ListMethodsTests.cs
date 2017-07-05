using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Utility;
using System.Collections.Generic;
using System.Collections;
[TestFixture]
[Ignore]
public class ListMethodsTests {
	[Test]
	public void MakeOrderedList_WhenCalled_ReturnsOrderedList(){
		List<int> orderedList = MakeOrderedList(4);

		Assert.That(orderedList, Is.Ordered);
	}
	[Test]
	public void MakeOrderedList_WhenCalled_ReturnsNonEmptyList(){
		List<int> orderedList = MakeOrderedList(4);

		Assert.That(orderedList, Is.Not.Empty);
	}
	[TestCase(0, 3, new int[]{1, 2, 3, 0})]
	[TestCase(7, 2, new int[]{0, 1, 7, 2, 3, 4, 5, 6})]
	[TestCase(12, 0, new int[]{12, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11})]
	[TestCase(0, 3, new int[]{1, 2, 3, 0, 4})]
	public void Reorder_WhenCalled_ReorderElement(int i, int j, IList<int> expected){
		List<int> orderedList = MakeOrderedList(expected.Count);
		
		orderedList.Reorder(i, j);

		Assert.That(orderedList, Is.EqualTo(expected));
	}
	[TestCase(0, 1, new int[]{1, 0, 2})]
	[TestCase(2, 0, new int[]{2, 1, 0})]
	[TestCase(4, 1, new int[]{0, 4, 2, 3, 1})]
	[TestCase(3, 12, new int[]{0, 1, 2, 12, 4, 5, 6, 7, 8, 9, 10, 11, 3})]
	public void SwappedList_WhenCalled_ReturnsSwappedList(int i, int j, IList<int> expected){
		List<int> orderedList = MakeOrderedList(expected.Count);
		IEnumerable<int> result = new int[]{};

		result = ListMethods.SwappedList(orderedList, i, j);

		Assert.That(result, Is.EqualTo(expected));
	}
	[TestCaseSource(typeof(PermCases))]
	public void Permutations_WhenCalled_ReturnsPermutations(IEnumerable<int> orig, IEnumerable<IEnumerable<int>> expected){
		
		List<IEnumerable<int>> perms = ListMethods.Permutations(orig);

		Assert.That(perms, Is.EquivalentTo(expected));
	}
		class PermCases: IEnumerable{
			public IEnumerator GetEnumerator(){
				IEnumerable<int> case1Orig = new int[]{0, 1, 2};
				List<IEnumerable<int>> case1Exp = new List<IEnumerable<int>>();
				case1Exp.Add(new int[]{0, 1, 2});
				case1Exp.Add(new int[]{0, 2, 1});
				case1Exp.Add(new int[]{1, 0, 2});
				case1Exp.Add(new int[]{1, 2, 0});
				case1Exp.Add(new int[]{2, 0, 1});
				case1Exp.Add(new int[]{2, 1, 0});
				object[] case1 = new object[]{case1Orig, case1Exp};
				yield return case1;

				IEnumerable<int> case2Orig = new int[]{0, 1, 2, 3};
				List<IEnumerable<int>> case2Exp = new List<IEnumerable<int>>();
				case2Exp.Add(new int[]{0, 1, 2, 3});
				case2Exp.Add(new int[]{0, 1, 3, 2});
				case2Exp.Add(new int[]{0, 2, 1, 3});
				case2Exp.Add(new int[]{0, 2, 3, 1});
				case2Exp.Add(new int[]{0, 3, 1, 2});
				case2Exp.Add(new int[]{0, 3, 2, 1});
				case2Exp.Add(new int[]{1, 0, 2, 3});
				case2Exp.Add(new int[]{1, 0, 3, 2});
				case2Exp.Add(new int[]{1, 2, 0, 3});
				case2Exp.Add(new int[]{1, 2, 3, 0});
				case2Exp.Add(new int[]{1, 3, 0, 2});
				case2Exp.Add(new int[]{1, 3, 2, 0});
				case2Exp.Add(new int[]{2, 0, 1, 3});
				case2Exp.Add(new int[]{2, 0, 3, 1});
				case2Exp.Add(new int[]{2, 1, 0, 3});
				case2Exp.Add(new int[]{2, 1, 3, 0});
				case2Exp.Add(new int[]{2, 3, 0, 1});
				case2Exp.Add(new int[]{2, 3, 1, 0});
				case2Exp.Add(new int[]{3, 0, 1, 2});
				case2Exp.Add(new int[]{3, 0, 2, 1});
				case2Exp.Add(new int[]{3, 1, 0, 2});
				case2Exp.Add(new int[]{3, 1, 2, 0});
				case2Exp.Add(new int[]{3, 2, 0, 1});
				case2Exp.Add(new int[]{3, 2, 1, 0});
				object[] case2 = new object[]{case2Orig, case2Exp};
				yield return case2;
			}
		}
	[TestCaseSource(typeof(CombCases))]
	public void Combinations_WhenCalled_ReturnsCombinations(int n, List<int> orig, List<IEnumerable<int>> expected){
		List<List<int>> result = ListMethods.Combinations(n, orig);

		Assert.That(result, Is.EquivalentTo(expected));
	}
		class CombCases: IEnumerable{
			public IEnumerator GetEnumerator(){
				int n_case1 = 2;
				List<int> orig_case1 = new List<int>(new int[]{0, 1, 2});
				List<IEnumerable<int>> exp_case1 = new List<IEnumerable<int>>();
				exp_case1.Add(new int[]{0, 1});
				exp_case1.Add(new int[]{0, 2});
				exp_case1.Add(new int[]{1, 2});
				object[] case1 = new object[]{n_case1, orig_case1, exp_case1};
				yield return case1;

				int n_case2 = 3;
				List<int> orig_case2 = new List<int>(new int[]{0, 1, 2, 3});
				List<IEnumerable<int>> exp_case2 = new List<IEnumerable<int>>();
				exp_case2.Add(new int[]{0, 1, 2});
				exp_case2.Add(new int[]{0, 1, 3});
				exp_case2.Add(new int[]{0, 2, 3});
				exp_case2.Add(new int[]{1, 2, 3});
				object[] case2 = new object[]{n_case2, orig_case2, exp_case2};
				yield return case2;

				int n_case3 = 3;
				List<int> orig_case3 = new List<int>(new int[]{0, 1, 2, 3, 4});
				List<IEnumerable<int>> exp_case3 = new List<IEnumerable<int>>();
				exp_case3.Add(new int[]{0, 1, 2});
				exp_case3.Add(new int[]{0, 1, 3});
				exp_case3.Add(new int[]{0, 1, 4});
				exp_case3.Add(new int[]{0, 2, 3});
				exp_case3.Add(new int[]{0, 2, 4});
				exp_case3.Add(new int[]{0, 3, 4});
				exp_case3.Add(new int[]{1, 2, 3});
				exp_case3.Add(new int[]{1, 2, 4});
				exp_case3.Add(new int[]{1, 3, 4});
				exp_case3.Add(new int[]{2, 3, 4});
				object[] case3 = new object[]{n_case3, orig_case3, exp_case3};
				yield return case3;
			}
		}
	public List<int> MakeOrderedList(){
		List<int> result = new List<int>();
		System.Random rng = new System.Random();
		int randomCount = rng.Next(1, 10);
		for(int i = 0; i< randomCount; i++){
			result.Add(i);
		}
		return result;
	}
	public List<int> MakeOrderedList(int count){
		List<int> result = new List<int>();
		for(int i = 0; i < count; i++){
			result.Add(i);
		}
		return result;
	}
}
