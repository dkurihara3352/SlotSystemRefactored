using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Utility;
using System.Collections.Generic;
[TestFixture]
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
