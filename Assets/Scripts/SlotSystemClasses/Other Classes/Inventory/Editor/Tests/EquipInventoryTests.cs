using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using Utility;
namespace SlotSystemTests{
    namespace OtherClassesTests{
        namespace InventoryTests{
            [TestFixture]
            [Category("Other")]
            public class EquipInventoryTests: SlotSystemTest{
                /*  Add */
                    [TestCaseSource(typeof(AddBowCases))]
                    public void Add_BowInst_EquipsBow(IEnumerable<BowInstance> added){
                        EquipmentSetInventory equipInv = new EquipmentSetInventory(MakeBowInstance(0), MakeWearInstance(0), new List<CarriedGearInstance>(), 0);

                        foreach(var item in added){
                            equipInv.Add(item);
                            Assert.That(equipInv.GetEquippedBow(), Is.SameAs(item));
                        }
                    }
                        class AddBowCases: IEnumerable{
                            public IEnumerator GetEnumerator(){
                                BowInstance bowInst_A_1 = MakeBowInstance(0);
                                BowInstance bowInst_A_2 = MakeBowInstance(0);
                                BowInstance bowInst_B_1 = MakeBowInstance(1);
                                BowInstance bowInst_B_2 = MakeBowInstance(1);
                                IEnumerable<BowInstance> case1Added = new BowInstance[]{
                                    bowInst_B_1, bowInst_A_2, bowInst_B_2, bowInst_A_1
                                };
                                object[] case1 = new object[]{case1Added};
                                yield return case1;
                            }
                        }
                    [TestCaseSource(typeof(AddWearInstCases))]
                    public void Add_WearInst_EquipsWear(IEnumerable<WearInstance> added){
                        EquipmentSetInventory equipInv = new EquipmentSetInventory(MakeBowInstance(0), MakeWearInstance(0), new List<CarriedGearInstance>(), 0);

                        foreach(var item in added){
                            equipInv.Add(item);
                            Assert.That(equipInv.GetEquippedWear(), Is.SameAs(item));
                        }
                    }
                        class AddWearInstCases: IEnumerable{
                            public IEnumerator GetEnumerator(){
                                WearInstance wearInst_A_1 = MakeWearInstance(0);
                                WearInstance wearInst_A_2 = MakeWearInstance(0);
                                WearInstance wearInst_B_1 = MakeWearInstance(1);
                                WearInstance wearInst_B_2 = MakeWearInstance(1);
                                IEnumerable<WearInstance> case1Added = new WearInstance[]{
                                    wearInst_B_1, wearInst_A_2, wearInst_B_2, wearInst_A_1
                                };
                                object[] case1 = new object[]{case1Added};
                                yield return case1;
                            }
                        }
                    [TestCaseSource(typeof(AddCGearsNotExceedingLimitCases))]
                    public void Add_CGearsNotExceedingLimit_EquipCGears(int equippableCount, List<CarriedGearInstance> added, IEnumerable<CarriedGearInstance> expected){
                        EquipmentSetInventory equipInv = new EquipmentSetInventory(MakeBowInstance(0), MakeWearInstance(0), new List<CarriedGearInstance>(), added.Count);

                        foreach(var item in added)
                            equipInv.Add(item);
                        
                        Assert.That(equipInv.GetEquippedCarriedGears(), Is.EqualTo(expected));
                    }
                        class AddCGearsNotExceedingLimitCases: IEnumerable{
                            public IEnumerator GetEnumerator(){
                                object[] fits_1;
                                    ShieldInstance shield_1 = MakeShieldInstance(0);
                                    List<CarriedGearInstance> added_1 = new List<CarriedGearInstance>(new CarriedGearInstance[]{shield_1});
                                    fits_1 = new object[]{1, added_1, added_1};
                                    yield return fits_1;
                                object[] fits_2;
                                    ShieldInstance shield_2 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_2 = MakeMWeaponInstance(0);
                                    List<CarriedGearInstance> added_2 = new List<CarriedGearInstance>(new CarriedGearInstance[]{shield_2, mWeapon_2});
                                    fits_2 = new object[]{2, added_2, added_2};
                                    yield return fits_2;
                                object[] fits_3;
                                    ShieldInstance shield_3 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_3 = MakeMWeaponInstance(0);
                                    QuiverInstance quiver_3 = MakeQuiverInstance(0);
                                    List<CarriedGearInstance> added_3 = new List<CarriedGearInstance>(new CarriedGearInstance[]{shield_3, mWeapon_3, quiver_3});
                                    fits_3 = new object[]{3, added_3, added_3};
                                    yield return fits_3;
                                object[] fits_4;
                                    ShieldInstance shield_4 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_4 = MakeMWeaponInstance(0);
                                    QuiverInstance quiver_4 = MakeQuiverInstance(0);
                                    PackInstance pack_4 = MakePackInstance(0);
                                    List<CarriedGearInstance> added_4 = new List<CarriedGearInstance>(new CarriedGearInstance[]{shield_4, mWeapon_4, quiver_4, pack_4});
                                    fits_4 = new object[]{4, added_4, added_4};
                                    yield return fits_4;
                            }
                        }
                    [Test]
                    [ExpectedException(typeof(System.ArgumentNullException))]
                    public void Add_Null_ThrowsException(){
                        EquipmentSetInventory equipInv = new EquipmentSetInventory(MakeBowInstance(0), MakeWearInstance(0), new List<CarriedGearInstance>(), 0);

                        equipInv.Add(null);
                    }
                    [TestCaseSource(typeof(AddCGExeedingCases))]
                    public void Add_CGearsExceedingLimit_ThrowsException(List<CarriedGearInstance> fitted, CarriedGearInstance exceeded){
                        EquipmentSetInventory equipInv = new EquipmentSetInventory(MakeBowInstance(0), MakeWearInstance(0), new List<CarriedGearInstance>(), fitted.Count);
                        foreach(var item in fitted)
                            equipInv.Add(item);
                        
                        System.Exception ex = Assert.Catch<System.InvalidOperationException>(() => equipInv.Add(exceeded));
                        Assert.That(ex.Message, Is.StringContaining("trying to add a CarriedGear exceeding the maximum allowed count"));
                    }
                        class AddCGExeedingCases: IEnumerable{
                            public IEnumerator GetEnumerator(){
                                object[] fits_1_plus_1;
                                    ShieldInstance shield_1 = MakeShieldInstance(0);
                                    ShieldInstance shield_nf_1 = MakeShieldInstance(0);
                                    List<CarriedGearInstance> fitted_1 = new List<CarriedGearInstance>(new CarriedGearInstance[]{shield_1});
                                    fits_1_plus_1 = new object[]{fitted_1, shield_nf_1};
                                    yield return fits_1_plus_1;
                                object[] fits_2_plus_1;
                                    ShieldInstance shield_2 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_2 = MakeMWeaponInstance(0);
                                    ShieldInstance shield_nf_2 = MakeShieldInstance(0);
                                    List<CarriedGearInstance> fitted_2 = new List<CarriedGearInstance>(new CarriedGearInstance[]{shield_2, mWeapon_2});
                                    fits_2_plus_1 = new object[]{fitted_2, shield_nf_2};
                                    yield return fits_2_plus_1;
                                object[] fits_3_plus_1;
                                    ShieldInstance shield_3 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_3 = MakeMWeaponInstance(0);
                                    QuiverInstance quiver_3 = MakeQuiverInstance(0);
                                    ShieldInstance shield_nf_3 = MakeShieldInstance(0);
                                    List<CarriedGearInstance> fitted_3 = new List<CarriedGearInstance>(new CarriedGearInstance[]{shield_3, mWeapon_3, quiver_3});
                                    fits_3_plus_1 = new object[]{fitted_3, shield_nf_3};
                                    yield return fits_3_plus_1;
                                object[] fits_4_plus_1;
                                    ShieldInstance shield_4 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_4 = MakeMWeaponInstance(0);
                                    QuiverInstance quiver_4 = MakeQuiverInstance(0);
                                    PackInstance pack_4 = MakePackInstance(0);
                                    ShieldInstance shield_nf_4 = MakeShieldInstance(0);
                                    List<CarriedGearInstance> fitted_4 = new List<CarriedGearInstance>(new CarriedGearInstance[]{shield_4, mWeapon_4, quiver_4, pack_4});
                                    fits_4_plus_1 = new object[]{fitted_4, shield_nf_4};
                                    yield return fits_4_plus_1;
                            }
                        }
                /*  Remove  */
                    [Test]
                    [ExpectedException(typeof(System.ArgumentNullException))]
                    public void Remove_Null_ThrowsException(){
                        EquipmentSetInventory equipInv = new EquipmentSetInventory(MakeBowInstance(0), MakeWearInstance(0), new List<CarriedGearInstance>(), 0);

                        equipInv.Remove(null);
                    }
                    [TestCaseSource(typeof(RemoveNonMemberCases))]
                    public void Remove_NonMember_IgnoresUpdate(List<IInventoryItemInstance> added, IEnumerable<IInventoryItemInstance> removed){
                        EquipmentSetInventory equipInv = new EquipmentSetInventory(MakeBowInstance(0), MakeWearInstance(0), new List<CarriedGearInstance>(), added.Count);
                        foreach(var item in added)
                            equipInv.Add(item);
                        BowInstance bow = equipInv.GetEquippedBow();
                        WearInstance wear = equipInv.GetEquippedWear();
                        List<CarriedGearInstance> cgs = equipInv.GetEquippedCarriedGears();
                        foreach(var item in removed)
                            equipInv.Remove(item);
                        
                        Assert.That(equipInv.GetEquippedBow(), Is.SameAs(bow));
                        Assert.That(equipInv.GetEquippedWear(), Is.SameAs(wear));
                        Assert.That(equipInv.GetEquippedCarriedGears(), Is.EqualTo(cgs));
                     }
                        class RemoveNonMemberCases: IEnumerable{
                            public IEnumerator GetEnumerator(){
                                BowInstance bow_m = MakeBowInstance(0);
                                BowInstance bow_nm = MakeBowInstance(0);
                                WearInstance wear_m = MakeWearInstance(0);
                                WearInstance wear_nm = MakeWearInstance(0);
                                ShieldInstance shield_m = MakeShieldInstance(0);
                                ShieldInstance shield_nm = MakeShieldInstance(0);
                                MeleeWeaponInstance mWeapon_m = MakeMWeaponInstance(0);
                                QuiverInstance quiver_m = MakeQuiverInstance(0);
                                PackInstance pack_m = MakePackInstance(0);
                                List<IInventoryItemInstance> case1Added = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
                                    bow_m, wear_m, shield_m, mWeapon_m, quiver_m, pack_m
                                });
                                object[] case1 = new object[]{case1Added, new IInventoryItemInstance[]{bow_nm, wear_nm, shield_nm}};
                                yield return case1;
                            }
                        }
                    [TestCaseSource(typeof(RemoveMemberCases))]
                    public void Remove_Member_RemovesItem(List<IInventoryItemInstance> added, IEnumerable<IInventoryItemInstance> removed, BowInstance expBow, WearInstance expWear, List<CarriedGearInstance> expCGears){
                        EquipmentSetInventory equipInv = new EquipmentSetInventory(MakeBowInstance(0), MakeWearInstance(0), new List<CarriedGearInstance>(), added.Count);
                        foreach(var item in added)
                            equipInv.Add(item);
                        
                        foreach(var item in removed)
                            equipInv.Remove(item);
                        
                        Assert.That(equipInv.GetEquippedBow(), Is.EqualTo(expBow));
                        Assert.That(equipInv.GetEquippedWear(), Is.EqualTo(expWear));
                        bool equality = equipInv.GetEquippedCarriedGears().MemberEquals(expCGears);
                        Assert.That(equality, Is.True);
                        }
                        class RemoveMemberCases: IEnumerable{
                            public IEnumerator GetEnumerator(){
                                object[] removeEveryting;
                                    BowInstance bow_1 = MakeBowInstance(0);
                                    WearInstance wear_1 = MakeWearInstance(0);
                                    ShieldInstance shield_1 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_1 = MakeMWeaponInstance(0);
                                    QuiverInstance quiver_1 = MakeQuiverInstance(0);
                                    PackInstance pack_1 = MakePackInstance(0);
                                    List<IInventoryItemInstance> added_1 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
                                        bow_1, wear_1, shield_1, mWeapon_1, quiver_1, pack_1
                                    });
                                    IEnumerable<IInventoryItemInstance> removed_1 = new IInventoryItemInstance[]{
                                        bow_1, wear_1, shield_1, mWeapon_1, quiver_1, pack_1
                                    };
                                    List<CarriedGearInstance> expCGs_1 = new List<CarriedGearInstance>(new CarriedGearInstance[]{
                                        
                                    });
                                    removeEveryting = new object[]{added_1, removed_1, null, null, expCGs_1};
                                    yield return removeEveryting;
                            }
                        }

                /*  items   */
                    [TestCaseSource(typeof(ItemsCases))]
                    public void Items_WhenCalled_ReturnsSumOfAllFields(List<IInventoryItemInstance> added, IEnumerable<IInventoryItemInstance> removed, IEnumerable<IInventoryItemInstance> expected){
                        EquipmentSetInventory equipInv = new EquipmentSetInventory(MakeBowInstance(0), MakeWearInstance(0), new List<CarriedGearInstance>(), added.Count);
                        foreach(var item in added)
                            equipInv.Add(item);
                        foreach(var item in removed)
                            equipInv.Remove(item);
                        
                        Assert.That(equipInv.GetItems(), Is.EqualTo(expected));
                    }
                        class ItemsCases: IEnumerable{
                            public IEnumerator GetEnumerator(){
                                object[] case0;
                                    BowInstance bowA_0 = MakeBowInstance(0);
                                    WearInstance wearA_0 = MakeWearInstance(0);
                                    ShieldInstance shieldA_0 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeaponA_0 = MakeMWeaponInstance(0);
                                    QuiverInstance quiverA_0 = MakeQuiverInstance(0);
                                    PackInstance packA_0 = MakePackInstance(0);
                                    List<IInventoryItemInstance> added_0 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
                                        bowA_0, wearA_0, shieldA_0, mWeaponA_0, quiverA_0, packA_0
                                    });
                                    IEnumerable<IInventoryItemInstance> removed_0 = new IInventoryItemInstance[]{
                                        bowA_0
                                    };
                                    IEnumerable<IInventoryItemInstance> exp_0 = new IInventoryItemInstance[]{
                                        wearA_0, shieldA_0, mWeaponA_0, quiverA_0, packA_0
                                    };
                                    case0 = new object[]{added_0, removed_0, exp_0};
                                    yield return case0;
                                object[] case1;
                                    BowInstance bowA_1 = MakeBowInstance(0);
                                    WearInstance wearA_1 = MakeWearInstance(0);
                                    ShieldInstance shieldA_1 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeaponA_1 = MakeMWeaponInstance(0);
                                    QuiverInstance quiverA_1 = MakeQuiverInstance(0);
                                    PackInstance packA_1 = MakePackInstance(0);
                                    List<IInventoryItemInstance> added_1 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
                                        bowA_1, wearA_1, shieldA_1, mWeaponA_1, quiverA_1, packA_1
                                    });
                                    IEnumerable<IInventoryItemInstance> removed_1 = new IInventoryItemInstance[]{
                                        wearA_1
                                    };
                                    IEnumerable<IInventoryItemInstance> exp_1 = new IInventoryItemInstance[]{
                                        bowA_1, shieldA_1, mWeaponA_1, quiverA_1, packA_1
                                    };
                                    case1 = new object[]{added_1, removed_1, exp_1};
                                    yield return case1;
                                object[] case2;
                                    BowInstance bowA_2 = MakeBowInstance(0);
                                    WearInstance wearA_2 = MakeWearInstance(0);
                                    ShieldInstance shieldA_2 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeaponA_2 = MakeMWeaponInstance(0);
                                    QuiverInstance quiverA_2 = MakeQuiverInstance(0);
                                    PackInstance packA_2 = MakePackInstance(0);
                                    List<IInventoryItemInstance> added_2 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
                                        bowA_2, wearA_2, shieldA_2, mWeaponA_2, quiverA_2, packA_2
                                    });
                                    IEnumerable<IInventoryItemInstance> removed_2 = new IInventoryItemInstance[]{
                                        shieldA_2, quiverA_2
                                    };
                                    IEnumerable<IInventoryItemInstance> exp_2 = new IInventoryItemInstance[]{
                                        bowA_2, wearA_2, mWeaponA_2, packA_2
                                    };
                                    case2 = new object[]{added_2, removed_2, exp_2};
                                    yield return case2;
                                object[] case3;
                                    BowInstance bowA_3 = MakeBowInstance(0);
                                    WearInstance wearA_3 = MakeWearInstance(0);
                                    ShieldInstance shieldA_3 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeaponA_3 = MakeMWeaponInstance(0);
                                    QuiverInstance quiverA_3 = MakeQuiverInstance(0);
                                    PackInstance packA_3 = MakePackInstance(0);
                                    List<IInventoryItemInstance> added_3 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
                                        bowA_3, wearA_3, shieldA_3, mWeaponA_3, quiverA_3, packA_3
                                    });
                                    IEnumerable<IInventoryItemInstance> removed_3 = new IInventoryItemInstance[]{
                                        bowA_3, wearA_3, shieldA_3, mWeaponA_3, quiverA_3, packA_3
                                    };
                                    IEnumerable<IInventoryItemInstance> exp_3 = new IInventoryItemInstance[]{
                                    };
                                    case3 = new object[]{added_3, removed_3, exp_3};
                                    yield return case3;
                            }
                        }
                /*  Helpers */
            }
        }
    }
}
