using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
namespace SlotSystemTests{
    namespace OtherClassesTests{
        namespace InventoryTests{
            [TestFixture]
            [Category("Other")]
            public class EquipInventoryTests: SlotSystemTest{
                /*  Add */
                    [TestCaseSource(typeof(AddBowCases))]
                    public void Add_BowInst_EquipsBow(IEnumerable<BowInstance> added){
                        TestEquipmentSetInventory equipInv = MakeEquipInventory();

                        foreach(var item in added){
                            equipInv.Add(item);
                            Assert.That(equipInv.equippedBow, Is.SameAs(item));
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
                        TestEquipmentSetInventory equipInv = MakeEquipInventory();

                        foreach(var item in added){
                            equipInv.Add(item);
                            Assert.That(equipInv.equippedWear, Is.SameAs(item));
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
                    public void Add_CGearsNotExceedingLimit_EquipCGears(int equippableCount, IEnumerable<CarriedGearInstance> added, IEnumerable<CarriedGearInstance> expected){
                        TestEquipmentSetInventory equipInv = MakeEquipInventory(equippableCount);

                        foreach(var item in added)
                            equipInv.Add(item);
                        
                        Assert.That(equipInv.equippedCGears, Is.EqualTo(expected));
                        }
                        class AddCGearsNotExceedingLimitCases: IEnumerable{
                            public IEnumerator GetEnumerator(){
                                object[] fits_1;
                                    ShieldInstance shield_1 = MakeShieldInstance(0);
                                    IEnumerable<CarriedGearInstance> added_1 = new CarriedGearInstance[]{shield_1};
                                    fits_1 = new object[]{1, added_1, added_1};
                                    yield return fits_1;
                                object[] fits_2;
                                    ShieldInstance shield_2 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_2 = MakeMeleeWeaponInstance(0);
                                    IEnumerable<CarriedGearInstance> added_2 = new CarriedGearInstance[]{shield_2, mWeapon_2};
                                    fits_2 = new object[]{2, added_2, added_2};
                                    yield return fits_2;
                                object[] fits_3;
                                    ShieldInstance shield_3 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_3 = MakeMeleeWeaponInstance(0);
                                    QuiverInstance quiver_3 = MakeQuiverInstance(0);
                                    IEnumerable<CarriedGearInstance> added_3 = new CarriedGearInstance[]{shield_3, mWeapon_3, quiver_3};
                                    fits_3 = new object[]{3, added_3, added_3};
                                    yield return fits_3;
                                object[] fits_4;
                                    ShieldInstance shield_4 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_4 = MakeMeleeWeaponInstance(0);
                                    QuiverInstance quiver_4 = MakeQuiverInstance(0);
                                    PackInstance pack_4 = MakePackInstance(0);
                                    IEnumerable<CarriedGearInstance> added_4 = new CarriedGearInstance[]{shield_4, mWeapon_4, quiver_4, pack_4};
                                    fits_4 = new object[]{4, added_4, added_4};
                                    yield return fits_4;
                            }
                        }

                    [Test]
                    [ExpectedException(typeof(System.ArgumentNullException))]
                    public void Add_Null_ThrowsException(){
                        TestEquipmentSetInventory equipInv = MakeEquipInventory();

                        equipInv.Add(null);
                    }
                    [TestCaseSource(typeof(AddCGExeedingCases))]
                    public void Add_CGearsExceedingLimit_ThrowsException(List<CarriedGearInstance> fitted, CarriedGearInstance exceeded){
                        EquipmentSetInventory equipInv = MakeEquipInventory(fitted.Count);
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
                                    MeleeWeaponInstance mWeapon_2 = MakeMeleeWeaponInstance(0);
                                    ShieldInstance shield_nf_2 = MakeShieldInstance(0);
                                    List<CarriedGearInstance> fitted_2 = new List<CarriedGearInstance>(new CarriedGearInstance[]{shield_2, mWeapon_2});
                                    fits_2_plus_1 = new object[]{fitted_2, shield_nf_2};
                                    yield return fits_2_plus_1;
                                object[] fits_3_plus_1;
                                    ShieldInstance shield_3 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_3 = MakeMeleeWeaponInstance(0);
                                    QuiverInstance quiver_3 = MakeQuiverInstance(0);
                                    ShieldInstance shield_nf_3 = MakeShieldInstance(0);
                                    List<CarriedGearInstance> fitted_3 = new List<CarriedGearInstance>(new CarriedGearInstance[]{shield_3, mWeapon_3, quiver_3});
                                    fits_3_plus_1 = new object[]{fitted_3, shield_nf_3};
                                    yield return fits_3_plus_1;
                                object[] fits_4_plus_1;
                                    ShieldInstance shield_4 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_4 = MakeMeleeWeaponInstance(0);
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
                        TestEquipmentSetInventory esi = MakeEquipInventory();

                        esi.Remove(null);
                        }
                    [TestCaseSource(typeof(RemoveNonMemberCases))]
                    public void Remove_NonMember_IgnoresUpdate(List<InventoryItemInstance> added, IEnumerable<InventoryItemInstance> removed){
                        TestEquipmentSetInventory esi = MakeEquipInventory(added.Count);
                        foreach(var item in added)
                            esi.Add(item);
                        BowInstance bow = esi.equippedBow;
                        WearInstance wear = esi.equippedWear;
                        List<CarriedGearInstance> cgs = esi.equippedCGears;
                        foreach(var item in removed)
                            esi.Remove(item);
                        
                        Assert.That(esi.equippedBow, Is.EqualTo(bow));
                        Assert.That(esi.equippedWear, Is.EqualTo(wear));
                        Assert.That(esi.equippedCGears, Is.EqualTo(cgs));
                     }
                        class RemoveNonMemberCases: IEnumerable{
                            public IEnumerator GetEnumerator(){
                                BowInstance bow_m = MakeBowInstance(0);
                                BowInstance bow_nm = MakeBowInstance(0);
                                WearInstance wear_m = MakeWearInstance(0);
                                WearInstance wear_nm = MakeWearInstance(0);
                                ShieldInstance shield_m = MakeShieldInstance(0);
                                ShieldInstance shield_nm = MakeShieldInstance(0);
                                MeleeWeaponInstance mWeapon_m = MakeMeleeWeaponInstance(0);
                                QuiverInstance quiver_m = MakeQuiverInstance(0);
                                PackInstance pack_m = MakePackInstance(0);
                                List<InventoryItemInstance> case1Added = new List<InventoryItemInstance>(new InventoryItemInstance[]{
                                    bow_m, wear_m, shield_m, mWeapon_m, quiver_m, pack_m
                                });
                                object[] case1 = new object[]{case1Added, new InventoryItemInstance[]{bow_nm, wear_nm, shield_nm}};
                                yield return case1;
                            }
                        }
                    [TestCaseSource(typeof(RemoveMemberCases))]
                    public void Remove_Member_RemovesItem(List<InventoryItemInstance> added, IEnumerable<InventoryItemInstance> removed, BowInstance expBow, WearInstance expWear, List<CarriedGearInstance> expCGears){
                        TestEquipmentSetInventory equipInv = MakeEquipInventory(added.Count);
                        foreach(var item in added)
                            equipInv.Add(item);
                        
                        foreach(var item in removed)
                            equipInv.Remove(item);
                        
                        Assert.That(equipInv.equippedBow, Is.EqualTo(expBow));
                        Assert.That(equipInv.equippedWear, Is.EqualTo(expWear));
                        Assert.That(equipInv.equippedCGears, Is.EqualTo(expCGears));
                        }
                        class RemoveMemberCases: IEnumerable{
                            public IEnumerator GetEnumerator(){
                                object[] removeEveryting;
                                    BowInstance bow_1 = MakeBowInstance(0);
                                    WearInstance wear_1 = MakeWearInstance(0);
                                    ShieldInstance shield_1 = MakeShieldInstance(0);
                                    MeleeWeaponInstance mWeapon_1 = MakeMeleeWeaponInstance(0);
                                    QuiverInstance quiver_1 = MakeQuiverInstance(0);
                                    PackInstance pack_1 = MakePackInstance(0);
                                    List<InventoryItemInstance> added_1 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
                                        bow_1, wear_1, shield_1, mWeapon_1, quiver_1, pack_1
                                    });
                                    IEnumerable<InventoryItemInstance> removed_1 = new InventoryItemInstance[]{
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
                    public void Items_WhenCalled_ReturnsSumOfAllFields(List<InventoryItemInstance> added, IEnumerable<InventoryItemInstance> removed, IEnumerable<InventoryItemInstance> expected){
                        TestEquipmentSetInventory equipInv = MakeEquipInventory(added.Count);
                        foreach(var item in added)
                            equipInv.Add(item);
                        foreach(var item in removed)
                            equipInv.Remove(item);
                        
                        Assert.That(equipInv.Items, Is.EqualTo(expected));
                    }
                        class ItemsCases: IEnumerable{
                            public IEnumerator GetEnumerator(){
                                BowInstance bow_A = MakeBowInstance(0);
                                WearInstance wear_A = MakeWearInstance(0);
                                ShieldInstance shield_A = MakeShieldInstance(0);
                                MeleeWeaponInstance mWeapon_A = MakeMeleeWeaponInstance(0);
                                QuiverInstance quiver_A = MakeQuiverInstance(0);
                                PackInstance pack_A = MakePackInstance(0);

                                List<InventoryItemInstance> added = new List<InventoryItemInstance>(new InventoryItemInstance[]{
                                    bow_A, wear_A, shield_A, mWeapon_A, quiver_A, pack_A
                                });
                                IEnumerable<InventoryItemInstance> case1Removed = new InventoryItemInstance[]{
                                    bow_A
                                };
                                IEnumerable<InventoryItemInstance> case1Exp = new InventoryItemInstance[]{
                                    wear_A, shield_A, mWeapon_A, quiver_A, pack_A
                                };
                                object[] case1 = new object[]{added, case1Removed, case1Exp};
                                yield return case1;

                                IEnumerable<InventoryItemInstance> case2Removed = new InventoryItemInstance[]{
                                    wear_A
                                };
                                IEnumerable<InventoryItemInstance> case2Exp = new InventoryItemInstance[]{
                                    bow_A, shield_A, mWeapon_A, quiver_A, pack_A
                                };
                                object[] case2 = new object[]{added, case2Removed, case2Exp};
                                yield return case2;

                                IEnumerable<InventoryItemInstance> case3Removed = new InventoryItemInstance[]{
                                    shield_A, quiver_A
                                };
                                IEnumerable<InventoryItemInstance> case3Exp = new InventoryItemInstance[]{
                                    bow_A, wear_A, mWeapon_A, pack_A
                                };
                                object[] case3 = new object[]{added, case3Removed, case3Exp};
                                yield return case3;

                                IEnumerable<InventoryItemInstance> case4Removed = new InventoryItemInstance[]{
                                    bow_A, wear_A,shield_A, mWeapon_A, quiver_A, pack_A
                                };
                                IEnumerable<InventoryItemInstance> case4Exp = new InventoryItemInstance[]{
                                };
                                object[] case4 = new object[]{added, case4Removed, case4Exp};
                                yield return case4;
                            }
                        }
                /*  Helpers */
                    TestEquipmentSetInventory MakeEquipInventory(){
                        TestEquipmentSetInventory equipInv = new TestEquipmentSetInventory(null, null, new List<CarriedGearInstance>(), 1);
                        return equipInv;
                    }
                    TestEquipmentSetInventory MakeEquipInventory(int cgCount){
                        TestEquipmentSetInventory equipInv = new TestEquipmentSetInventory(null, null, new List<CarriedGearInstance>(), cgCount);
                        return equipInv;
                    }
                    class TestEquipmentSetInventory: EquipmentSetInventory{
                        public TestEquipmentSetInventory(BowInstance initBow, WearInstance initWear, List<CarriedGearInstance> initCGears, int initCGCount)
                        : base(initBow, initWear, initCGears, initCGCount){}
                        public BowInstance equippedBow{get{return m_equippedBow;}}
                        public WearInstance equippedWear{get{return m_equippedWear;}}
                        public List<CarriedGearInstance> equippedCGears{get{return m_equippedCGears;}}
                        public IEnumerable<InventoryItemInstance> Items{
                            get{
                                foreach(var item in m_items)
                                    yield return (InventoryItemInstance)item;
                            }
                        }
                    }
            }
        }
    }
}
