using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Utility;
using System.Collections.Generic;
using System.Collections;
namespace UtilityClassTests{
	[TestFixture]
	[Category("Utility")]
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

			bool equality = orderedList.MemberEquals(expected);
			Assert.That(equality, Is.True);
			}
		[TestCase(0, 1, new int[]{1, 0, 2})]
		[TestCase(2, 0, new int[]{2, 1, 0})]
		[TestCase(4, 1, new int[]{0, 4, 2, 3, 1})]
		[TestCase(3, 12, new int[]{0, 1, 2, 12, 4, 5, 6, 7, 8, 9, 10, 11, 3})]
		public void SwappedList_WhenCalled_ReturnsSwappedList(int i, int j, IList<int> expected){
			List<int> orderedList = MakeOrderedList(expected.Count);
			IEnumerable<int> result = new int[]{};

			result = ListMethods.SwappedList(orderedList, i, j);

			bool equality = result.MemberEquals(expected);
			Assert.That(equality, Is.True);
			}
		[TestCaseSource(typeof(PermCases))]
		public void Permutations_WhenCalled_ReturnsPermutations(IEnumerable<int> orig, IEnumerable<IEnumerable<int>> expected){
			
			List<IEnumerable<int>> perms = ListMethods.Permutations(orig);

			Assert.That(perms, Is.EquivalentTo(expected));
			}
			class PermCases: IEnumerable{
				public IEnumerator GetEnumerator(){
					IEnumerable<int> case1Orig = new int[]{0, 1, 2};
					List<IEnumerable<int>> case1Exp = new List<IEnumerable<int>>(
						new IEnumerable<int>[]{
							new int[]{0, 1, 2},
							new int[]{0, 2, 1},
							new int[]{1, 0, 2},
							new int[]{1, 2, 0},
							new int[]{2, 0, 1},
							new int[]{2, 1, 0}
						}
					);
					object[] case1 = new object[]{case1Orig, case1Exp};
					yield return case1;

					IEnumerable<int> case2Orig = new int[]{0, 1, 2, 3};
					List<IEnumerable<int>> case2Exp = new List<IEnumerable<int>>(
						new IEnumerable<int>[]{
							new int[]{0, 1, 2, 3},
							new int[]{0, 1, 3, 2},
							new int[]{0, 2, 1, 3},
							new int[]{0, 2, 3, 1},
							new int[]{0, 3, 1, 2},
							new int[]{0, 3, 2, 1},
							new int[]{1, 0, 2, 3},
							new int[]{1, 0, 3, 2},
							new int[]{1, 2, 0, 3},
							new int[]{1, 2, 3, 0},
							new int[]{1, 3, 0, 2},
							new int[]{1, 3, 2, 0},
							new int[]{2, 0, 1, 3},
							new int[]{2, 0, 3, 1},
							new int[]{2, 1, 0, 3},
							new int[]{2, 1, 3, 0},
							new int[]{2, 3, 0, 1},
							new int[]{2, 3, 1, 0},
							new int[]{3, 0, 1, 2},
							new int[]{3, 0, 2, 1},
							new int[]{3, 1, 0, 2},
							new int[]{3, 1, 2, 0},
							new int[]{3, 2, 0, 1},
							new int[]{3, 2, 1, 0}
						}
					);
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
		[TestCaseSource(typeof(TrimCases))]
		public void Trim_WhenCalled_SetsExpected(List<TestElement> list, List<TestElement> expected){
			list.Trim();

			bool equality = list.MemberEquals(expected);
			Assert.That(equality, Is.True);
			}
			class TrimCases: IEnumerable{
				public IEnumerator GetEnumerator(){
					TestElement ele_1 = new TestElement();
					TestElement ele_2 = new TestElement();
					TestElement ele_3 = new TestElement();
					TestElement ele_4 = new TestElement();
					List<TestElement> case1List = new List<TestElement>(new TestElement[]{ ele_1, null, ele_2, ele_3, null, null, ele_4});
					List<TestElement> case1Exp = new List<TestElement>(new TestElement[]{ele_1, ele_2, ele_3, ele_4});
					yield return new object[]{case1List, case1Exp};
				}
			}
		[Test][ExpectedException(typeof(System.NullReferenceException))]
		public void Fill_listNull_ThrowsException(){
			List<TestElement> nullList = null;
			nullList.Fill(new TestElement());
		}
		[Test]
		public void Fill_ElementNull_ThrowsException(){
			List<TestElement> emptyList = new List<TestElement>();

			System.Exception ex = Assert.Catch<System.ArgumentNullException>(() => emptyList.Fill(null));

			Assert.That(ex.Message, Is.StringContaining("element to be filled is null"));
		}
		[TestCaseSource(typeof(FillCases))]
		public void Fill_WhenCalled_SetsExpected(List<TestElement> list, TestElement added, List<TestElement> expected){
			List<TestElement> testList = new List<TestElement>(list);
			testList.Fill(added);
			
			bool equality = testList.MemberEquals(expected);
			Assert.That(equality, Is.True);
		}
			class FillCases: IEnumerable{
				public IEnumerator GetEnumerator(){
					object[] fillInTheFirstEmpty;
						TestElement ele1_0 = new TestElement();
						TestElement ele2_0 = new TestElement();
						TestElement ele3_0 = new TestElement();
						TestElement ele4_0 = new TestElement();
						List<TestElement> list_0 = new List<TestElement>(new TestElement[]{null, ele1_0, ele2_0, null, ele3_0});
						List<TestElement> exp_0 = new List<TestElement>(new TestElement[]{ele4_0, ele1_0, ele2_0, null, ele3_0});
						fillInTheFirstEmpty = new object[]{list_0, ele4_0, exp_0};
						yield return fillInTheFirstEmpty;
					object[] fillOut;
						TestElement ele1_1 = new TestElement();
						TestElement ele2_1 = new TestElement();
						TestElement ele3_1 = new TestElement();
						TestElement ele4_1 = new TestElement();
						List<TestElement> list_1 = new List<TestElement>(new TestElement[]{ele1_1, ele2_1, null, ele3_1});
						List<TestElement> exp_1 = new List<TestElement>(new TestElement[]{ele1_1, ele2_1, ele4_1, ele3_1});
						fillOut = new object[]{list_1, ele4_1, exp_1};
						yield return fillOut;
					object[] contatenate;
						TestElement ele1_2 = new TestElement();
						TestElement ele2_2 = new TestElement();
						TestElement ele3_2 = new TestElement();
						TestElement ele4_2 = new TestElement();
						List<TestElement> list_2 = new List<TestElement>(new TestElement[]{ele1_2, ele2_2, ele3_2});
						List<TestElement> exp_2 = new List<TestElement>(new TestElement[]{ele1_2, ele2_2, ele3_2, ele4_2});
						contatenate = new object[]{list_2, ele4_2, exp_2};
						yield return contatenate;
					object[] createEntryForEmpty;
						TestElement ele0_3 = new TestElement();
						List<TestElement> list_3 = new List<TestElement>();
						List<TestElement> exp_3 = new List<TestElement>(new TestElement[]{ele0_3});
						createEntryForEmpty = new object[]{
							list_3, ele0_3, exp_3
						};
						yield return createEntryForEmpty;
				}
			}
		[TestCaseSource(typeof(Count_AlwaysCases))]
		public void Count_Always_ReturnsNumberOfEntriesIncludingNull(IEnumerable<TestElement> eles, int expectedCount){
			Assert.That(eles.Count, Is.EqualTo(expectedCount));
			}
			class Count_AlwaysCases: IEnumerable{
				public IEnumerator GetEnumerator(){
					object[] allTestElements;
						IEnumerable<TestElement> eles_0 = new TestElement[]{
							new TestElement(),
							new TestElement(),
							new TestElement(),
							new TestElement()
						};
						allTestElements = new object[]{eles_0, 4};
						yield return allTestElements;
					object[] withSomeNulls;
						IEnumerable<TestElement> eles_1 = new TestElement[]{
							new TestElement(),
							null,
							new TestElement(),
							new TestElement(),
							null,
							null,
							new TestElement()
						};
						withSomeNulls = new object[]{eles_1, 7};
						yield return withSomeNulls;
					object[] allNull;
						IEnumerable<TestElement> eles_2 = new TestElement[]{
							null,
							null,
							null,
							null
						};
						allNull = new object[]{eles_2, 4};
						yield return allNull;
				}
			}
		[TestCaseSource(typeof(MemberEquals_ValueTypeCases))]
		public void MemberEquals_ValueType_ReturnsAccordingly(IEnumerable<int> coll, IEnumerable<int> other, bool expected){
			bool actual = coll.MemberEquals(other);

			Assert.That(actual, Is.EqualTo(expected));
		}
			class MemberEquals_ValueTypeCases: IEnumerable{
				public IEnumerator GetEnumerator(){
					object[] identical_T;
						IEnumerable<int> coll_0 = new int[]{0, 1, 2, 5, 7};
						IEnumerable<int> other_0 = new int[]{0, 1, 2, 5, 7};
						identical_T = new object[]{coll_0, other_0, true};
						yield return identical_T;
					object[] partial_F;
						IEnumerable<int> coll_1 = new int[]{0, 1, 2};
						IEnumerable<int> other_1 = new int[]{0, 1, 2, 5, 7};
						partial_F = new object[]{coll_1, other_1, false};
						yield return partial_F;
					object[] equivalent_F;
						IEnumerable<int> coll_2 = new int[]{2, 7, 5, 1, 0};
						IEnumerable<int> other_2 = new int[]{0, 1, 2, 5, 7};
						equivalent_F = new object[]{coll_2, other_2, false};
						yield return equivalent_F;
					object[] bothEmpty_T;
						IEnumerable<int> coll_3 = new int[]{};
						IEnumerable<int> other_3 = new int[]{};
						bothEmpty_T = new object[]{coll_3, other_3, true};
						yield return bothEmpty_T;
					object[] eitherEmpty_F;
						IEnumerable<int> coll_4 = new int[]{};
						IEnumerable<int> other_4 = new int[]{0, 1, 2, 5, 7};
						eitherEmpty_F = new object[]{coll_4, other_4, false};
						yield return eitherEmpty_F;
				}
			}
		[TestCaseSource(typeof(MemberEquals_CustomClassCases))]
		public void MemberEquals_CustomClass_ReturnsAccordingly(IEnumerable<MemEqClass> coll, IEnumerable<MemEqClass> other, bool expected){
			bool actual = coll.MemberEquals(other);

			Assert.That(actual, Is.EqualTo(expected));
		}
			class MemberEquals_CustomClassCases: IEnumerable{
				public IEnumerator GetEnumerator(){
					object[] identical_T;
						MemEqClass me0_0 = new MemEqClass();
						MemEqClass me1_0 = new MemEqClass();
						MemEqClass me2_0 = new MemEqClass();
						MemEqClass me3_0 = new MemEqClass();
						MemEqClass me4_0 = new MemEqClass();
						IEnumerable<MemEqClass> coll_0 = new MemEqClass[]{
							me0_0, 
							me1_0, 
							me2_0, 
							me3_0, 
							me4_0
						};
						IEnumerable<MemEqClass> other_0 = new MemEqClass[]{
							me0_0, 
							me1_0, 
							me2_0, 
							me3_0, 
							me4_0
						};
						identical_T = new object[]{coll_0, other_0, true};
						yield return identical_T;
					object[] partial_F;
						MemEqClass me0_1 = new MemEqClass();
						MemEqClass me1_1 = new MemEqClass();
						MemEqClass me2_1 = new MemEqClass();
						MemEqClass me3_1 = new MemEqClass();
						MemEqClass me4_1 = new MemEqClass();
						IEnumerable<MemEqClass> coll_1 = new MemEqClass[]{
							me0_1, 
							me1_1, 
							me2_1
						};
						IEnumerable<MemEqClass> other_1 = new MemEqClass[]{
							me0_1, 
							me1_1, 
							me2_1, 
							me3_1, 
							me4_1
						};
						partial_F = new object[]{coll_1, other_1, false};
						yield return partial_F;
					object[] equivalent_F;
						MemEqClass me0_2 = new MemEqClass();
						MemEqClass me1_2 = new MemEqClass();
						MemEqClass me2_2 = new MemEqClass();
						MemEqClass me3_2 = new MemEqClass();
						MemEqClass me4_2 = new MemEqClass();
						IEnumerable<MemEqClass> coll_2 = new MemEqClass[]{
							me0_2,
							me1_2,
							me2_2,
							me3_2,
							me4_2
						};
						IEnumerable<MemEqClass> other_2 = new MemEqClass[]{
							me0_2, 
							me1_2, 
							me4_2,
							me3_2, 
							me2_2 
						};
						equivalent_F = new object[]{coll_2, other_2, false};
						yield return equivalent_F;
					object[] bothEmpty_T;
						IEnumerable<MemEqClass> coll_3 = new MemEqClass[]{
						};
						IEnumerable<MemEqClass> other_3 = new MemEqClass[]{
						};
						bothEmpty_T = new object[]{coll_3, other_3, true};
						yield return bothEmpty_T;
					object[] nullSameSize_T;
						IEnumerable<MemEqClass> coll_4 = new MemEqClass[]{
							null,
							null,
							null,
							null
						};
						IEnumerable<MemEqClass> other_4 = new MemEqClass[]{
							null,
							null,
							null,
							null
						};
						nullSameSize_T = new object[]{coll_4, other_4, true};
						yield return nullSameSize_T;
					object[] nullDifSize_F;
						IEnumerable<MemEqClass> coll_5 = new MemEqClass[]{
							null,
							null
						};
						IEnumerable<MemEqClass> other_5 = new MemEqClass[]{
							null,
							null,
							null,
							null
						};
						nullDifSize_F = new object[]{coll_5, other_5, false};
						yield return nullDifSize_F;
					object[] someToEmpty_F;
						MemEqClass me0_6 = new MemEqClass();
						MemEqClass me1_6 = new MemEqClass();
						MemEqClass me2_6 = new MemEqClass();
						MemEqClass me3_6 = new MemEqClass();
						MemEqClass me4_6 = new MemEqClass();
						IEnumerable<MemEqClass> coll_6 = new MemEqClass[]{
							me0_6,
							me1_6,
							me2_6,
							me3_6,
							me4_6
						};
						IEnumerable<MemEqClass> other_6 = new MemEqClass[]{
						};
						someToEmpty_F = new object[]{coll_6, other_6, false};
						yield return someToEmpty_F;
					object[] someToNull_F;
						MemEqClass me0_7 = new MemEqClass();
						MemEqClass me1_7 = new MemEqClass();
						MemEqClass me2_7 = new MemEqClass();
						MemEqClass me3_7 = new MemEqClass();
						MemEqClass me4_7 = new MemEqClass();
						IEnumerable<MemEqClass> coll_7 = new MemEqClass[]{
							me0_7,
							me1_7,
							me2_7,
							me3_7,
							me4_7
						};
						IEnumerable<MemEqClass> other_7 = new MemEqClass[]{
							null,
							null,
							null,
							null,
							null
						};
						someToNull_F = new object[]{coll_7, other_7, false};
						yield return someToNull_F;
					object[] identicalWithNull_T;
						MemEqClass me0_8 = new MemEqClass();
						MemEqClass me1_8 = new MemEqClass();
						MemEqClass me2_8 = new MemEqClass();
						MemEqClass me3_8 = new MemEqClass();
						MemEqClass me4_8 = new MemEqClass();
						IEnumerable<MemEqClass> coll_8 = new MemEqClass[]{
							me0_8,
							null,
							me1_8,
							me2_8,
							me3_8,
							null,
							null,
							me4_8
						};
						IEnumerable<MemEqClass> other_8 = new MemEqClass[]{
							me0_8,
							null,
							me1_8,
							me2_8,
							me3_8,
							null,
							null,
							me4_8
						};
						identicalWithNull_T = new object[]{coll_8, other_8, true};
						yield return identicalWithNull_T;
					object[] partialWithNull_F;
						MemEqClass me0_9 = new MemEqClass();
						MemEqClass me1_9 = new MemEqClass();
						MemEqClass me2_9 = new MemEqClass();
						MemEqClass me3_9 = new MemEqClass();
						MemEqClass me4_9 = new MemEqClass();
						IEnumerable<MemEqClass> coll_9 = new MemEqClass[]{
							me0_9,
							null,
							me1_9,
							me2_9,
							me3_9,
							null,
							null,
							me4_9
						};
						IEnumerable<MemEqClass> other_9 = new MemEqClass[]{
							me0_9,
							null,
							me1_9,
							me2_9,
							null,
							null,
							null,
							me4_9
						};
						partialWithNull_F = new object[]{coll_9, other_9, false};
						yield return partialWithNull_F;
					object[] equivalentWithNull_F;
						MemEqClass me0_10 = new MemEqClass();
						MemEqClass me1_10 = new MemEqClass();
						MemEqClass me2_10 = new MemEqClass();
						MemEqClass me3_10 = new MemEqClass();
						MemEqClass me4_10 = new MemEqClass();
						IEnumerable<MemEqClass> coll_10 = new MemEqClass[]{
							me0_10,
							null,
							me1_10,
							me2_10,
							me3_10,
							null,
							null,
							me4_10
						};
						IEnumerable<MemEqClass> other_10 = new MemEqClass[]{
							me1_10,
							null,
							null,
							me3_10,
							null,
							me2_10,
							me4_10,
							me0_10,
						};
						equivalentWithNull_F = new object[]{coll_10, other_10, false};
						yield return equivalentWithNull_F;
					object[] nullToEmpty_F;
						IEnumerable<MemEqClass> coll_11 = new MemEqClass[]{
							null,
							null
						};
						IEnumerable<MemEqClass> other_11 = new MemEqClass[]{
						};
						nullToEmpty_F = new object[]{coll_11, other_11, false};
						yield return nullToEmpty_F;
				}
			}
		public class MemEqClass{}	
		public class TestElement{}
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
}

