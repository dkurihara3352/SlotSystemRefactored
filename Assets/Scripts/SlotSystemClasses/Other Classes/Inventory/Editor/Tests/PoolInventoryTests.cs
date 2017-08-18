using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using Utility;
namespace SlotSystemTests{
    namespace OtherClassesTests{
        namespace InventoryTests{
			[TestFixture]
			[Category("Other")]
			public class PoolInventoryTests: SlotSystemTest {
				/*	Add	*/
					[TestCaseSource(typeof(AddNonStackableCases))]
					public void Add_NonStackable_IncreaseEntries(IEnumerable<IInventoryItemInstance> addedItems, List<IInventoryItemInstance> expected){
						PoolInventory poolInv = MakePoolInventory();

						foreach(var item in addedItems)
							poolInv.Add(item);
						
						Assert.That(poolInv.GetItems(), Is.EqualTo(expected));
						foreach(var item in poolInv.GetItems()){
							IInventoryItemInstance itemInst = (IInventoryItemInstance)item;
							Assert.That(itemInst.GetQuantity(), Is.EqualTo(1));
						}
					}
						class AddNonStackableCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								object[] addNonStackables;
									BowInstance bow_1 = MakeBowInstance(0);
									WearInstance wear_1 = MakeWearInstance(0);
									ShieldInstance shield_1 = MakeShieldInstance(0);
									MeleeWeaponInstance mWeapon_1 = MakeMWeaponInstance(0);
									QuiverInstance quiver_1 = MakeQuiverInstance(0);
									PackInstance pack_1 = MakePackInstance(0);
									IEnumerable<IInventoryItemInstance> added_1 = new IInventoryItemInstance[]{bow_1, wear_1, shield_1, mWeapon_1, quiver_1, pack_1};
									List<IInventoryItemInstance> expected_1 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{bow_1, wear_1, shield_1, mWeapon_1, quiver_1, pack_1});
									addNonStackables = new object[]{added_1, expected_1};
									yield return addNonStackables;
							}
						}
					[TestCaseSource(typeof(AddSameStackableCases))]
					public void Add_SameStackableDiffInsts_IncreasesQuantity(IEnumerable<IInventoryItemInstance> addedItems, List<IInventoryItemInstance> expected){
						PoolInventory poolInv = MakePoolInventory();
						int expectedCount = QuantitySum(addedItems);
						
						foreach(var item in addedItems)
							poolInv.Add(item);

						Assert.That(poolInv.GetItems(), Is.EqualTo(expected));
						Assert.That(expected[0].GetQuantity(), Is.EqualTo(expectedCount));
					}
						class AddSameStackableCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								PartsInstance stubPartsInst_A = MakePartsInstance(0, 2);
								PartsInstance stubPartsInst_B = MakePartsInstance(0, 1);
								PartsInstance stubPartsInst_C = MakePartsInstance(0, 10);
								PartsInstance stubPartsInst_D = MakePartsInstance(0, 3);
								IEnumerable<IInventoryItemInstance> case1Added = new IInventoryItemInstance[]{
									stubPartsInst_A, 
									stubPartsInst_B, 
									stubPartsInst_C, 
									stubPartsInst_D
								};
								List<IInventoryItemInstance> case1Exp = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{stubPartsInst_A});
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
						List<IInventoryItemInstance> expected = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{partsInst_A, partsInst_B});
						
						poolInv.Add(partsInst_A);
						poolInv.Add(partsInst_B);

						Assert.That(poolInv.GetItems(), Is.EqualTo(expected));
						Assert.That(partsInst_A.GetQuantity(), Is.EqualTo(1));
						Assert.That(partsInst_B.GetQuantity(), Is.EqualTo(1));
					}
					[Test]
					public void Add_NonStackableSameInst_ThrowsException(){
						PoolInventory inv = MakePoolInventory();
						BowInstance bowInst = MakeBowInstance(0);

						inv.Add(bowInst);
						System.Exception ex = Assert.Catch<System.InvalidOperationException>(() => inv.Add(bowInst));
						
						Assert.That(ex.Message, Is.StringContaining("cannot add multiple same IInventoryItemInstances. Try instantiate another instance with the same InventoryItem instead"));
						}
					[Test]
					public void Add_StackableSameInst_ThrowsException(){
						PoolInventory poolInv = MakePoolInventory();
						PartsInstance partsInst = MakePartsInstance(0, 1);

						poolInv.Add(partsInst);

						System.Exception ex = Assert.Catch<System.InvalidOperationException>(() => poolInv.Add(partsInst));
						Assert.That(ex.Message, Is.StringContaining("cannot add multiple same IInventoryItemInstances. Try instantiate another instance with the same InventoryItem instead"));
						}
					[Test]
					[ExpectedException(typeof(System.ArgumentNullException))]
					public void Add_Null_ThrowsException(){
						PoolInventory poolInv = MakePoolInventory();
						
						poolInv.Add(null);
						}
					[TestCaseSource(typeof(AddVariousCases))]
					public void Add_Various_PerformComplexBehaviour(IEnumerable<IInventoryItemInstance> addedItems, List<IInventoryItemInstance> expected, Dictionary<IInventoryItemInstance, int> itemQuantityDict){
						PoolInventory poolInv = MakePoolInventory();
						int[] origQuant = CacheOrigQuant(addedItems);
						foreach(var item in addedItems)
							poolInv.Add(item);
						
						Assert.That(poolInv.GetItems(), Is.EqualTo(expected));

						foreach(KeyValuePair<IInventoryItemInstance, int> pair in itemQuantityDict){
							Assert.That(pair.Key.GetQuantity(), Is.EqualTo(pair.Value));
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
								IEnumerable<IInventoryItemInstance> case1Added = new IInventoryItemInstance[]{
									bowInst_A, bowInst_B,
									partsInst_A_1, partsInst_A_2,
									partsInst_B_1, partsInst_B_2
								};
								List<IInventoryItemInstance> case1Expected = new List<IInventoryItemInstance>(
									new IInventoryItemInstance[]{
										bowInst_A, bowInst_B, partsInst_A_1, partsInst_B_1
									}
								);
								Dictionary<IInventoryItemInstance, int> case1Quant = new Dictionary<IInventoryItemInstance, int>();
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
						IEnumerable<IInventoryItemInstance> addedItems,
						IEnumerable<IInventoryItemInstance> removedItems,
						List<IInventoryItemInstance> expected){
							PoolInventory poolInv = MakePoolInventory();
							foreach(var item in addedItems)
								poolInv.Add(item);

							foreach(var item in removedItems)
								poolInv.Remove(item);
							
						Assert.That(poolInv.GetItems(), Is.EqualTo(expected));
					}
						class RemoveNonStackableCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								BowInstance bowInst_A_1 = MakeBowInstance(0);
								BowInstance bowInst_A_2 = MakeBowInstance(0);
								BowInstance bowInst_A_3 = MakeBowInstance(0);
								BowInstance bowInst_B_1 = MakeBowInstance(1);
								BowInstance bowInst_B_2 = MakeBowInstance(1);
								BowInstance bowInst_B_3 = MakeBowInstance(1);
								IEnumerable<IInventoryItemInstance> case1Added = new IInventoryItemInstance[]{
									bowInst_A_1, bowInst_A_2, bowInst_A_3,
									bowInst_B_1, bowInst_B_2, bowInst_B_3
								};
								IEnumerable<IInventoryItemInstance> case1Removed = new IInventoryItemInstance[]{
									bowInst_A_1, bowInst_A_3,
									bowInst_B_2
								};
								List<IInventoryItemInstance> case1Exp = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
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
					public void Remove_NonMember_IgnoresUpdate(IEnumerable<IInventoryItemInstance> added, IEnumerable<IInventoryItemInstance> removed, List<IInventoryItemInstance> expected){
						PoolInventory poolInv = MakePoolInventory();
						foreach(var item in added)
							poolInv.Add(item);

						foreach(var item in removed)
							poolInv.Remove(item);
						
						Assert.That(poolInv.GetItems(), Is.EqualTo(expected));
					}
						class RemoveNonMemberCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								BowInstance bowInst_A_1 = MakeBowInstance(0);
								BowInstance bowInst_A_2 = MakeBowInstance(0);
								BowInstance bowInst_A_3 = MakeBowInstance(0);
								BowInstance bowInst_B_1 = MakeBowInstance(1);
								BowInstance bowInst_B_2 = MakeBowInstance(1);
								BowInstance bowInst_B_3 = MakeBowInstance(1);
								
								IEnumerable<IInventoryItemInstance> case1Added = new IInventoryItemInstance[]{
									bowInst_A_1, bowInst_A_2, bowInst_B_1,
								};
								IEnumerable<IInventoryItemInstance> case1Removed = new IInventoryItemInstance[]{
									bowInst_A_3, bowInst_B_2, bowInst_B_3
								};
								List<IInventoryItemInstance> case1Exp = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
									bowInst_A_1, bowInst_A_2, bowInst_B_1
								});
								object[] case1 = new object[]{
									case1Added, case1Removed, case1Exp
								};
								yield return case1;
							}
						}
					[TestCaseSource(typeof(RemoveStackableDownToOneCases))]
					public void Remove_StackableDownToOne_DecreasesQuantity(IEnumerable<IInventoryItemInstance> added, IEnumerable<IInventoryItemInstance> removed, List<IInventoryItemInstance> expected, Dictionary<IInventoryItemInstance, int> quantDict){
						PoolInventory poolInv = MakePoolInventory();
						int[] origAddedQuants = CacheOrigQuant(added);
						int[] origRemovedQuants = CacheOrigQuant(removed);
						foreach(var item in added)
							poolInv.Add(item);

						foreach(var item in removed)
							poolInv.Remove(item);
						
						Assert.That(poolInv.GetItems(), Is.EqualTo(expected));
						foreach(KeyValuePair<IInventoryItemInstance, int> pair in quantDict){
							Assert.That(pair.Key.GetQuantity(), Is.EqualTo(pair.Value));
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
								IEnumerable<IInventoryItemInstance> case1Added = new IInventoryItemInstance[]{
									partsInst_A_1, partsInst_A_2, partsInst_B_1, partsInst_B_2
								};
								PartsInstance partsInst_A_r = MakePartsInstance(0, 5);
								PartsInstance partsInst_B_r = MakePartsInstance(1, 6);
								IEnumerable<IInventoryItemInstance> case1Removed = new IInventoryItemInstance[]{
									partsInst_A_r, partsInst_B_r
								};
								List<IInventoryItemInstance> case1Exp = new List<IInventoryItemInstance>(
									new IInventoryItemInstance[]{
										partsInst_A_1, partsInst_B_1
									});
								Dictionary<IInventoryItemInstance, int> case1Dict = new Dictionary<IInventoryItemInstance, int>();
								case1Dict.Add(partsInst_A_1, 2);
								case1Dict.Add(partsInst_B_1, 1);
								object case1  = new object[]{
									case1Added, case1Removed, case1Exp, case1Dict
								};
								yield return case1;
							}
						}
					[TestCaseSource(typeof(RemoveStackableDownToZeroCases))]
					public void Remove_StackableDownToZero_RemovesEntry(IEnumerable<IInventoryItemInstance> added, IEnumerable<IInventoryItemInstance> removed, List<IInventoryItemInstance> expected, Dictionary<IInventoryItemInstance, int> quantDict){
						PoolInventory poolInv = MakePoolInventory();
						int[] origAddedQuants = CacheOrigQuant(added);
						int[] origRemovedQuants = CacheOrigQuant(removed);
						foreach(var item in added)
							poolInv.Add(item);

						foreach(var item in removed)
							poolInv.Remove(item);
						
						Assert.That(poolInv.GetItems(), Is.EqualTo(expected));
						foreach(KeyValuePair<IInventoryItemInstance, int> pair in quantDict){
							Assert.That(pair.Key.GetQuantity(), Is.EqualTo(pair.Value));
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
								IEnumerable<IInventoryItemInstance> case1Added = new IInventoryItemInstance[]{
									partsInst_A_1, partsInst_A_2, partsInst_B_1, partsInst_B_2
								};
								PartsInstance partsInst_A_r = MakePartsInstance(0, 7);
								PartsInstance partsInst_B_r = MakePartsInstance(1, 7);
								IEnumerable<IInventoryItemInstance> case1Removed = new IInventoryItemInstance[]{
									partsInst_A_r, partsInst_B_r
								};
								List<IInventoryItemInstance> case1Exp = new List<IInventoryItemInstance>();
								Dictionary<IInventoryItemInstance, int> case1Dict = new Dictionary<IInventoryItemInstance, int>();
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
					public void Remove_Various_PerformComplexBehaviour(IEnumerable<IInventoryItemInstance> added, IEnumerable<IInventoryItemInstance> removed, List<IInventoryItemInstance> expected, Dictionary<IInventoryItemInstance, int> quantDict){
						PoolInventory poolInv = MakePoolInventory();
						int[] origAddedQuants = CacheOrigQuant(added);
						int[] origRemovedQuants = CacheOrigQuant(removed);
						foreach(var item in added)
							poolInv.Add(item);

						foreach(var item in removed)
							poolInv.Remove(item);
						
						Assert.That(poolInv.GetItems(), Is.EqualTo(expected));
						foreach(KeyValuePair<IInventoryItemInstance, int> pair in quantDict){
							Assert.That(pair.Key.GetQuantity(), Is.EqualTo(pair.Value));
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
								IEnumerable<IInventoryItemInstance> case1Added = new IInventoryItemInstance[]{
									partsInst_A_1, partsInst_A_2, partsInst_B_1, partsInst_B_2,
									bowInst_A_1, bowInst_A_2, bowInst_B_1, bowInst_B_2
								};
								PartsInstance partsInst_A_r = MakePartsInstance(0, 7);
								PartsInstance partsInst_B_r = MakePartsInstance(1, 3);
								IEnumerable<IInventoryItemInstance> case1Removed = new IInventoryItemInstance[]{
									partsInst_A_r, partsInst_B_r,
									bowInst_A_2, bowInst_B_2
								};
								List<IInventoryItemInstance> case1Exp = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
									partsInst_B_1,
									bowInst_A_1, bowInst_B_1
								});
								Dictionary<IInventoryItemInstance, int> case1Dict = new Dictionary<IInventoryItemInstance, int>();
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
					public void IndexItems_WhenItemsUpdated_UpdateIndex(IEnumerable<IInventoryItemInstance> added, IEnumerable<IInventoryItemInstance> removed, List<IInventoryItemInstance> expected, Dictionary<IInventoryItemInstance, int> indexDict){
						PoolInventory poolInv = MakePoolInventory();
						int[] origAddedQuants = CacheOrigQuant(added);
						int[] origRemovedQuants = CacheOrigQuant(removed);
						
						foreach(var item in added)
							poolInv.Add(item);
						foreach(var item in removed)
							poolInv.Remove(item);
						
						Assert.That(poolInv.GetItems(), Is.EqualTo(expected));
						foreach(KeyValuePair<IInventoryItemInstance, int> pair in indexDict){
							Assert.That(pair.Key.GetAcquisitionOrder(), Is.EqualTo(pair.Value));
						}
						RevertQuant(added, origAddedQuants);
						RevertQuant(removed, origRemovedQuants);
					}
						class IndexItemCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								object[] removeComplex;
									BowInstance bowA_1 = MakeBowInstance(0);
									BowInstance bowA_r_1 = MakeBowInstance(0);
									BowInstance bowB_1 = MakeBowInstance(1);
									BowInstance bowB_r_1 = MakeBowInstance(1);
									PartsInstance partsA_1 = MakePartsInstance(0, 3);
									PartsInstance partsAA_1 = MakePartsInstance(0, 4);
									PartsInstance partsB_1 = MakePartsInstance(1, 5);
									PartsInstance partsBB_1 = MakePartsInstance(1, 2);
									PartsInstance partsA_r_1 = MakePartsInstance(0, 7);
									PartsInstance partsB_r_1 = MakePartsInstance(1, 3);
									IEnumerable<IInventoryItemInstance> added_1 = new IInventoryItemInstance[]{
										partsA_1, partsAA_1, partsB_1, partsBB_1,
										bowA_1, bowA_r_1, bowB_1, bowB_r_1
									};
									IEnumerable<IInventoryItemInstance> removed_1 = new IInventoryItemInstance[]{
										partsA_r_1, partsB_r_1,
										bowA_r_1, bowB_r_1
									};
									List<IInventoryItemInstance> expItems_1 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
										partsB_1,
										bowA_1, bowB_1
									});
									Dictionary<IInventoryItemInstance, int> ids_1 = new Dictionary<IInventoryItemInstance, int>();
									ids_1.Add(partsB_1, 0);
									ids_1.Add(bowA_1, 1);
									ids_1.Add(bowB_1, 2);
									removeComplex  = new object[]{
										added_1, removed_1, expItems_1, ids_1
									};
									yield return removeComplex;
								object[] removeNone;
									BowInstance bowA_2 = MakeBowInstance(0);
									BowInstance bowA_r_2 = MakeBowInstance(0);
									BowInstance bowB_2 = MakeBowInstance(1);
									BowInstance bowB_r_2 = MakeBowInstance(1);
									PartsInstance partsA_2 = MakePartsInstance(0, 3);
									PartsInstance partsAA_2 = MakePartsInstance(0, 4);
									PartsInstance partsB_2 = MakePartsInstance(1, 5);
									PartsInstance partsBB_2 = MakePartsInstance(1, 2);
									IEnumerable<IInventoryItemInstance> added_2 = new IInventoryItemInstance[]{
										partsA_2, partsAA_2, partsB_2, partsBB_2,
										bowA_2, bowA_r_2, bowB_2, bowB_r_2
									};
									IEnumerable<IInventoryItemInstance> removed_2 = new IInventoryItemInstance[]{};
									List<IInventoryItemInstance> expItems_2 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
										partsA_2, partsB_2,
										bowA_2, bowA_r_2, bowB_2, bowB_r_2
									});
									Dictionary<IInventoryItemInstance, int> ids_2 = new Dictionary<IInventoryItemInstance, int>();
									ids_2.Add(partsA_2, 0);
									ids_2.Add(partsB_2, 1);
									ids_2.Add(bowA_2, 2);
									ids_2.Add(bowA_r_2, 3);
									ids_2.Add(bowB_2, 4);
									ids_2.Add(bowB_r_2, 5);
									removeNone  = new object[]{
										added_2, removed_2, expItems_2, ids_2
									};
									yield return removeNone;
								}
						}
				/*	helpers */
					int[] CacheOrigQuant(IEnumerable<IInventoryItemInstance> items){
						List<int> resList = new List<int>();
						foreach(var item in items)
							resList.Add(item.GetQuantity());
						return resList.ToArray();
					}
					void RevertQuant(IEnumerable<IInventoryItemInstance> items, int[] quants){
						int count = 0;
						foreach(var item in items){
							item.SetQuantity(quants[count++]);
						}
					}
					int QuantitySum(IEnumerable<IInventoryItemInstance> items){
						int sum = 0;
						foreach(var item in items){
							IInventoryItemInstance itemInst = (IInventoryItemInstance)item;
							sum += itemInst.GetQuantity();
						}
						return sum;
					}
					PoolInventory MakePoolInventory(){
						return new PoolInventory();
					}
			}
        }
    }
}
