﻿using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
namespace SlotSystemTests{
    namespace OtherClassesTests{
        namespace InventoryTests{
			[TestFixture]
			public class PoolInventoryTests: AbsSlotSystemTest {
				/*	Add	*/
					[TestCaseSource(typeof(AddNonStackableCases))]
					public void Add_NonStackable_IncreaseEntries(IEnumerable<InventoryItemInstance> addedItems, List<InventoryItemInstance> expected){
						PoolInventory poolInv = MakePoolInventory();
						foreach(var item in addedItems)
							poolInv.Add(item);
						
						Assert.That(ItemList(poolInv), Is.EqualTo(expected));
						foreach(var item in poolInv){
							InventoryItemInstance itemInst = (InventoryItemInstance)item;
							Assert.That(itemInst.quantity, Is.EqualTo(1));
						}
					}
						class AddNonStackableCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								BowInstance bowInst_A = MakeBowInstance(0);
								BowInstance bowInst_A_1 = MakeBowInstance(0);
								BowInstance bowInst_A_2 = MakeBowInstance(0);
								object[] case1 = new object[]{
									new InventoryItemInstance[]{bowInst_A, bowInst_A_1, bowInst_A_2},
									new List<InventoryItemInstance>(new InventoryItemInstance[]{bowInst_A, bowInst_A_1, bowInst_A_2})
								};
								yield return case1;
							}
						}
					
					[TestCaseSource(typeof(AddSameStackableCases))]
					public void Add_SameStackableDiffInsts_IncreasesQuantity(IEnumerable<InventoryItemInstance> addedItems, List<InventoryItemInstance> expected){
						PoolInventory poolInv = MakePoolInventory();
						int expectedCount = QuantitySum(addedItems);
						
						foreach(var item in addedItems)
							poolInv.Add((InventoryItemInstance)item);

						Assert.That(ItemList(poolInv), Is.EqualTo(expected));
						Assert.That(expected[0].quantity, Is.EqualTo(expectedCount));
					}
						class AddSameStackableCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								PartsInstance stubPartsInst_A = MakePartsInstance(0, 2);
								PartsInstance stubPartsInst_B = MakePartsInstance(0, 1);
								PartsInstance stubPartsInst_C = MakePartsInstance(0, 10);
								PartsInstance stubPartsInst_D = MakePartsInstance(0, 3);
								IEnumerable<InventoryItemInstance> case1Added = new InventoryItemInstance[]{stubPartsInst_A, stubPartsInst_B, stubPartsInst_C, stubPartsInst_D};
								List<InventoryItemInstance> case1Exp = new List<InventoryItemInstance>(new InventoryItemInstance[]{stubPartsInst_A});
								object[] case1 = new object[]{
									case1Added, case1Exp
								};
								yield return case1;
							}
						}
					[Test]
					public void Add_DiffStackable_IncreasesEntries(){
						PoolInventory poolInv = MakePoolInventory();
						PartsInstance partsInst_A = MakePartsInstance(0, 1);
						PartsInstance partsInst_B = MakePartsInstance(1, 1);
						List<InventoryItemInstance> expected = new List<InventoryItemInstance>(new InventoryItemInstance[]{partsInst_A, partsInst_B});
						
						poolInv.Add(partsInst_A);
						poolInv.Add(partsInst_B);

						Assert.That(ItemList(poolInv), Is.EqualTo(expected));
						Assert.That(partsInst_A.quantity, Is.EqualTo(1));
						Assert.That(partsInst_B.quantity, Is.EqualTo(1));
					}
					[Test]
					public void Add_NonStackableSameInst_ThrowsException(){
						PoolInventory inv = MakePoolInventory();
						BowInstance bowInst = MakeBowInstance(0);

						inv.Add(bowInst);
						System.Exception ex = Assert.Catch<System.InvalidOperationException>(() => inv.Add(bowInst));
						
						Assert.That(ex.Message, Is.StringContaining("cannot add multiple same InventoryItemInstances. Try instantiate another instance with the same InventoryItem instead"));
					}
					[Test]
					public void Add_StackableSameInst_ThrowsException(){
						PoolInventory poolInv = MakePoolInventory();
						PartsInstance partsInst = MakePartsInstance(0, 1);

						poolInv.Add(partsInst);

						System.Exception ex = Assert.Catch<System.InvalidOperationException>(() => poolInv.Add(partsInst));
						Assert.That(ex.Message, Is.StringContaining("cannot add multiple same InventoryItemInstances. Try instantiate another instance with the same InventoryItem instead"));
					}
					[Test]
					[ExpectedException(typeof(System.ArgumentNullException))]
					public void Add_Null_ThrowsException(){
						PoolInventory poolInv = MakePoolInventory();
						
						poolInv.Add(null);
					}
					[TestCaseSource(typeof(AddVariousCases))]
					public void Add_Various_PerformComplexBehaviour(IEnumerable<InventoryItemInstance> addedItems, List<InventoryItemInstance> expected, Dictionary<InventoryItemInstance, int> itemQuantityDict){
						PoolInventory poolInv = MakePoolInventory();
						int[] origQuant = CacheOrigQuant(addedItems);
						foreach(var item in addedItems)
							poolInv.Add(item);
						
						Assert.That(ItemList(poolInv), Is.EqualTo(expected));

						foreach(KeyValuePair<InventoryItemInstance, int> pair in itemQuantityDict){
							Assert.That(pair.Key.quantity, Is.EqualTo(pair.Value));
						}

						RevertQuant(addedItems, origQuant);
					}
						class AddVariousCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								BowInstance bowInst_A = MakeBowInstance(0);
								BowInstance bowInst_B = MakeBowInstance(0);
								PartsInstance partsInst_A_1 = MakePartsInstance(0, 1);
								PartsInstance partsInst_A_2 = MakePartsInstance(0, 14);
								PartsInstance partsInst_B_1 = MakePartsInstance(1, 7);
								PartsInstance partsInst_B_2 = MakePartsInstance(1, 8);
								IEnumerable<InventoryItemInstance> case1Added = new InventoryItemInstance[]{
									bowInst_A, bowInst_B,
									partsInst_A_1, partsInst_A_2,
									partsInst_B_1, partsInst_B_2
								};
								List<InventoryItemInstance> case1Expected = new List<InventoryItemInstance>(
									new InventoryItemInstance[]{
										bowInst_A, bowInst_B, partsInst_A_1, partsInst_B_1
									}
								);
								Dictionary<InventoryItemInstance, int> case1Quant = new Dictionary<InventoryItemInstance, int>();
								case1Quant.Add(bowInst_A, 1);
								case1Quant.Add(bowInst_B, 1);
								case1Quant.Add(partsInst_A_1, 15);
								case1Quant.Add(partsInst_B_1, 15);
								object[] case1 = new object[]{
									case1Added, case1Expected, case1Quant
								};
								yield return case1;
							}
						}
				/*	Remove	*/
					[TestCaseSource(typeof(RemoveNonStackableCases))]
					public void Remove_NonStackable_RemovesEntry(
						IEnumerable<InventoryItemInstance> addedItems,
						IEnumerable<InventoryItemInstance> removedItems,
						List<InventoryItemInstance> expectedResult){
							PoolInventory poolInv = MakePoolInventory();
							foreach(var item in addedItems)
								poolInv.Add(item);

							foreach(var item in removedItems)
								poolInv.Remove(item);
							
							Assert.That(ItemList(poolInv), Is.EqualTo(expectedResult));
					}
						class RemoveNonStackableCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								BowInstance bowInst_A_1 = MakeBowInstance(0);
								BowInstance bowInst_A_2 = MakeBowInstance(0);
								BowInstance bowInst_A_3 = MakeBowInstance(0);
								BowInstance bowInst_B_1 = MakeBowInstance(1);
								BowInstance bowInst_B_2 = MakeBowInstance(1);
								BowInstance bowInst_B_3 = MakeBowInstance(1);
								IEnumerable<InventoryItemInstance> case1Added = new InventoryItemInstance[]{
									bowInst_A_1, bowInst_A_2, bowInst_A_3,
									bowInst_B_1, bowInst_B_2, bowInst_B_3
								};
								IEnumerable<InventoryItemInstance> case1Removed = new InventoryItemInstance[]{
									bowInst_A_1, bowInst_A_3,
									bowInst_B_2
								};
								List<InventoryItemInstance> case1Exp = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									bowInst_A_2, bowInst_B_1, bowInst_B_3
								});
								object[] case1 = new object[]{
									case1Added, case1Removed, case1Exp
								};
								yield return case1;
							}
						}
					[Test]
					[ExpectedException(typeof(System.ArgumentNullException))]
					public void Remove_Null_ThrowsException(){
						PoolInventory poolInv = MakePoolInventory();
						
						poolInv.Remove(null);
					}
					[TestCaseSource(typeof(RemoveNonMemberCases))]
					public void Remove_NonMember_IgnoresUpdate(IEnumerable<InventoryItemInstance> added, IEnumerable<InventoryItemInstance> removed, List<InventoryItemInstance> expected){
						PoolInventory poolInv = MakePoolInventory();
						foreach(var item in added)
							poolInv.Add(item);

						foreach(var item in removed)
							poolInv.Remove(item);
						
						Assert.That(ItemList(poolInv), Is.EqualTo(expected));
					}
						class RemoveNonMemberCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								BowInstance bowInst_A_1 = MakeBowInstance(0);
								BowInstance bowInst_A_2 = MakeBowInstance(0);
								BowInstance bowInst_A_3 = MakeBowInstance(0);
								BowInstance bowInst_B_1 = MakeBowInstance(1);
								BowInstance bowInst_B_2 = MakeBowInstance(1);
								BowInstance bowInst_B_3 = MakeBowInstance(1);
								
								IEnumerable<InventoryItemInstance> case1Added = new InventoryItemInstance[]{
									bowInst_A_1, bowInst_A_2, bowInst_B_1,
								};
								IEnumerable<InventoryItemInstance> case1Removed = new InventoryItemInstance[]{
									bowInst_A_3, bowInst_B_2, bowInst_B_3
								};
								List<InventoryItemInstance> case1Exp = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									bowInst_A_1, bowInst_A_2, bowInst_B_1
								});
								object[] case1 = new object[]{
									case1Added, case1Removed, case1Exp
								};
								yield return case1;
							}
						}
					[TestCaseSource(typeof(RemoveStackableDownToOneCases))]
					public void Remove_StackableDownToOne_DecreasesQuantity(IEnumerable<InventoryItemInstance> added, IEnumerable<InventoryItemInstance> removed, List<InventoryItemInstance> expected, Dictionary<InventoryItemInstance, int> quantDict){
						PoolInventory poolInv = MakePoolInventory();
						int[] origAddedQuants = CacheOrigQuant(added);
						int[] origRemovedQuants = CacheOrigQuant(removed);
						foreach(var item in added)
							poolInv.Add(item);

						foreach(var item in removed)
							poolInv.Remove(item);
						
						Assert.That(ItemList(poolInv), Is.EqualTo(expected));
						foreach(KeyValuePair<InventoryItemInstance, int> pair in quantDict){
							Assert.That(pair.Key.quantity, Is.EqualTo(pair.Value));
						}

						RevertQuant(added, origAddedQuants);
						RevertQuant(removed, origRemovedQuants);
					}
						class RemoveStackableDownToOneCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								PartsInstance partsInst_A_1 = MakePartsInstance(0, 3);
								PartsInstance partsInst_A_2 = MakePartsInstance(0, 4);
								PartsInstance partsInst_B_1 = MakePartsInstance(1, 5);
								PartsInstance partsInst_B_2 = MakePartsInstance(1, 2);
								IEnumerable<InventoryItemInstance> case1Added = new InventoryItemInstance[]{
									partsInst_A_1, partsInst_A_2, partsInst_B_1, partsInst_B_2
								};
								PartsInstance partsInst_A_r = MakePartsInstance(0, 5);
								PartsInstance partsInst_B_r = MakePartsInstance(1, 6);
								IEnumerable<InventoryItemInstance> case1Removed = new InventoryItemInstance[]{
									partsInst_A_r, partsInst_B_r
								};
								List<InventoryItemInstance> case1Exp = new List<InventoryItemInstance>(
									new InventoryItemInstance[]{
										partsInst_A_1, partsInst_B_1
									});
								Dictionary<InventoryItemInstance, int> case1Dict = new Dictionary<InventoryItemInstance, int>();
								case1Dict.Add(partsInst_A_1, 2);
								case1Dict.Add(partsInst_B_1, 1);
								object case1  = new object[]{
									case1Added, case1Removed, case1Exp, case1Dict
								};
								yield return case1;
							}
						}
					[TestCaseSource(typeof(RemoveStackableDownToZeroCases))]
					public void Remove_StackableDownToZero_RemovesEntry(IEnumerable<InventoryItemInstance> added, IEnumerable<InventoryItemInstance> removed, List<InventoryItemInstance> expected, Dictionary<InventoryItemInstance, int> quantDict){
						PoolInventory poolInv = MakePoolInventory();
						int[] origAddedQuants = CacheOrigQuant(added);
						int[] origRemovedQuants = CacheOrigQuant(removed);
						foreach(var item in added)
							poolInv.Add(item);

						foreach(var item in removed)
							poolInv.Remove(item);
						
						Assert.That(ItemList(poolInv), Is.EqualTo(expected));
						foreach(KeyValuePair<InventoryItemInstance, int> pair in quantDict){
							Assert.That(pair.Key.quantity, Is.EqualTo(pair.Value));
						}

						RevertQuant(added, origAddedQuants);
						RevertQuant(removed, origRemovedQuants);
					}
						class RemoveStackableDownToZeroCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								PartsInstance partsInst_A_1 = MakePartsInstance(0, 3);
								PartsInstance partsInst_A_2 = MakePartsInstance(0, 4);
								PartsInstance partsInst_B_1 = MakePartsInstance(1, 5);
								PartsInstance partsInst_B_2 = MakePartsInstance(1, 2);
								IEnumerable<InventoryItemInstance> case1Added = new InventoryItemInstance[]{
									partsInst_A_1, partsInst_A_2, partsInst_B_1, partsInst_B_2
								};
								PartsInstance partsInst_A_r = MakePartsInstance(0, 7);
								PartsInstance partsInst_B_r = MakePartsInstance(1, 7);
								IEnumerable<InventoryItemInstance> case1Removed = new InventoryItemInstance[]{
									partsInst_A_r, partsInst_B_r
								};
								List<InventoryItemInstance> case1Exp = new List<InventoryItemInstance>();
								Dictionary<InventoryItemInstance, int> case1Dict = new Dictionary<InventoryItemInstance, int>();
								object case1  = new object[]{
									case1Added, case1Removed, case1Exp, case1Dict
								};
								yield return case1;
							}
						}
					[Test]
					public void Remove_StackableDownBelowZero_ThrowsException(){
						PoolInventory poolInv = MakePoolInventory();
						PartsInstance partsInst = MakePartsInstance(0, 3);
						PartsInstance partsInst_r = MakePartsInstance(0, 4);
						poolInv.Add(partsInst);

						System.Exception ex = Assert.Catch<System.InvalidOperationException>(() => poolInv.Remove(partsInst_r));
						Assert.That(ex.Message, Is.StringContaining("PoolInventory.Remove: cannot remove by greater quantity than there is"));
					}
					[TestCaseSource(typeof(RemoveVariousCases))]
					public void Remove_Various_PerformComplexBehaviour(IEnumerable<InventoryItemInstance> added, IEnumerable<InventoryItemInstance> removed, List<InventoryItemInstance> expected, Dictionary<InventoryItemInstance, int> quantDict){
						PoolInventory poolInv = MakePoolInventory();
						int[] origAddedQuants = CacheOrigQuant(added);
						int[] origRemovedQuants = CacheOrigQuant(removed);
						foreach(var item in added)
							poolInv.Add(item);

						foreach(var item in removed)
							poolInv.Remove(item);
						
						Assert.That(ItemList(poolInv), Is.EqualTo(expected));
						foreach(KeyValuePair<InventoryItemInstance, int> pair in quantDict){
							Assert.That(pair.Key.quantity, Is.EqualTo(pair.Value));
						}

						RevertQuant(added, origAddedQuants);
						RevertQuant(removed, origRemovedQuants);
					}
						class RemoveVariousCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								BowInstance bowInst_A_1 = MakeBowInstance(0);
								BowInstance bowInst_A_2 = MakeBowInstance(0);
								BowInstance bowInst_B_1 = MakeBowInstance(1);
								BowInstance bowInst_B_2 = MakeBowInstance(1);
								PartsInstance partsInst_A_1 = MakePartsInstance(0, 3);
								PartsInstance partsInst_A_2 = MakePartsInstance(0, 4);
								PartsInstance partsInst_B_1 = MakePartsInstance(1, 5);
								PartsInstance partsInst_B_2 = MakePartsInstance(1, 2);
								IEnumerable<InventoryItemInstance> case1Added = new InventoryItemInstance[]{
									partsInst_A_1, partsInst_A_2, partsInst_B_1, partsInst_B_2,
									bowInst_A_1, bowInst_A_2, bowInst_B_1, bowInst_B_2
								};
								PartsInstance partsInst_A_r = MakePartsInstance(0, 7);
								PartsInstance partsInst_B_r = MakePartsInstance(1, 3);
								IEnumerable<InventoryItemInstance> case1Removed = new InventoryItemInstance[]{
									partsInst_A_r, partsInst_B_r,
									bowInst_A_2, bowInst_B_2
								};
								List<InventoryItemInstance> case1Exp = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									partsInst_B_1,
									bowInst_A_1, bowInst_B_1
								});
								Dictionary<InventoryItemInstance, int> case1Dict = new Dictionary<InventoryItemInstance, int>();
								case1Dict.Add(partsInst_B_1, 4);
								case1Dict.Add(bowInst_A_1, 1);
								case1Dict.Add(bowInst_B_1, 1);
								object case1  = new object[]{
									case1Added, case1Removed, case1Exp, case1Dict
								};
								yield return case1;
							}
						}

				/*	Indexing	*/
					[TestCaseSource(typeof(IndexItemCases))]
					public void IndexItems_WhenItemsUpdated_UpdateIndex(IEnumerable<InventoryItemInstance> added, IEnumerable<InventoryItemInstance> removed, List<InventoryItemInstance> expResult, Dictionary<InventoryItemInstance, int> indexDict){
						PoolInventory poolInv = MakePoolInventory();
						int[] origAddedQuants = CacheOrigQuant(added);
						int[] origRemovedQuants = CacheOrigQuant(removed);
						
						foreach(var item in added)
							poolInv.Add(item);
						foreach(var item in removed)
							poolInv.Remove(item);
						
						Assert.That(ItemList(poolInv), Is.EqualTo(expResult));
						foreach(KeyValuePair<InventoryItemInstance, int> pair in indexDict){
							Assert.That(pair.Key.AcquisitionOrder, Is.EqualTo(pair.Value));
						}
						RevertQuant(added, origAddedQuants);
						RevertQuant(removed, origRemovedQuants);
					}
						class IndexItemCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								BowInstance bowInst_A_1 = MakeBowInstance(0);
								BowInstance bowInst_A_2 = MakeBowInstance(0);
								BowInstance bowInst_B_1 = MakeBowInstance(1);
								BowInstance bowInst_B_2 = MakeBowInstance(1);
								PartsInstance partsInst_A_1 = MakePartsInstance(0, 3);
								PartsInstance partsInst_A_2 = MakePartsInstance(0, 4);
								PartsInstance partsInst_B_1 = MakePartsInstance(1, 5);
								PartsInstance partsInst_B_2 = MakePartsInstance(1, 2);
								IEnumerable<InventoryItemInstance> case1Added = new InventoryItemInstance[]{
									partsInst_A_1, partsInst_A_2, partsInst_B_1, partsInst_B_2,
									bowInst_A_1, bowInst_A_2, bowInst_B_1, bowInst_B_2
								};
								PartsInstance partsInst_A_r = MakePartsInstance(0, 7);
								PartsInstance partsInst_B_r = MakePartsInstance(1, 3);
								IEnumerable<InventoryItemInstance> case1Removed = new InventoryItemInstance[]{
									partsInst_A_r, partsInst_B_r,
									bowInst_A_2, bowInst_B_2
								};
								List<InventoryItemInstance> case1Exp = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									partsInst_B_1,
									bowInst_A_1, bowInst_B_1
								});
								Dictionary<InventoryItemInstance, int> case1Dict = new Dictionary<InventoryItemInstance, int>();
								case1Dict.Add(partsInst_B_1, 0);
								case1Dict.Add(bowInst_A_1, 1);
								case1Dict.Add(bowInst_B_1, 2);
								object case1  = new object[]{
									case1Added, case1Removed, case1Exp, case1Dict
								};
								yield return case1;

								IEnumerable<InventoryItemInstance> case2Added = new InventoryItemInstance[]{
									partsInst_A_1, partsInst_A_2, partsInst_B_1, partsInst_B_2,
									bowInst_A_1, bowInst_A_2, bowInst_B_1, bowInst_B_2
								};
								IEnumerable<InventoryItemInstance> case2Removed = new InventoryItemInstance[]{};
								List<InventoryItemInstance> case2Exp = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									partsInst_A_1, partsInst_B_1,
									bowInst_A_1, bowInst_A_2, bowInst_B_1, bowInst_B_2
								});
								Dictionary<InventoryItemInstance, int> case2Dict = new Dictionary<InventoryItemInstance, int>();
								case2Dict.Add(partsInst_A_1, 0);
								case2Dict.Add(partsInst_B_1, 1);
								case2Dict.Add(bowInst_A_1, 2);
								case2Dict.Add(bowInst_A_2, 3);
								case2Dict.Add(bowInst_B_1, 4);
								case2Dict.Add(bowInst_B_2, 5);
								object case2  = new object[]{
									case2Added, case2Removed, case2Exp, case2Dict
								};
								yield return case2;
							}
						}
				/*	helpers */
					int[] CacheOrigQuant(IEnumerable<InventoryItemInstance> items){
						List<int> resList = new List<int>();
						foreach(var item in items)
							resList.Add(item.quantity);
						return resList.ToArray();
					}
					void RevertQuant(IEnumerable<InventoryItemInstance> items, int[] quants){
						int count = 0;
						foreach(var item in items){
							item.quantity = quants[count++];
						}
					}
					int QuantitySum(IEnumerable<InventoryItemInstance> items){
						int sum = 0;
						foreach(var item in items){
							InventoryItemInstance itemInst = (InventoryItemInstance)item;
							sum += itemInst.quantity;
						}
						return sum;
					}
					PoolInventory MakePoolInventory(){
						return new PoolInventory();
					}
					List<InventoryItemInstance> ItemList(PoolInventory inv){
						List<InventoryItemInstance> result = new List<InventoryItemInstance>();
						foreach(var item in inv){
							result.Add((InventoryItemInstance)item);
						}
						return result;
					}
					public List<InventoryItemInstance> CopyInvItems(IEnumerable<InventoryItemInstance> orig){
						List<InventoryItemInstance> result = new List<InventoryItemInstance>();
						foreach(var item in orig){
							if(item is BowInstance){
								BowFake bowFake = (BowFake)((BowInstance)item).Item;
								int itemID = bowFake.ItemID;
								result.Add(MakeBowInstance(itemID));
							}else if(item is PartsInstance){
								PartsFake partsFake = (PartsFake)(((PartsInstance)item).Item);
								int quant = item.quantity;
								int itemID = partsFake.ItemID;
								result.Add(MakePartsInstance(itemID, quant));
							}
						}
						return result;
					}
			}
        }
    }
}