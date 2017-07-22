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
            public class EquipInventoryTests: SlotSystemTest{
                /*  Add */
                    [TestCaseSource(typeof(AddBowCases))]
                    public void Add_BowInst_EquipsBow(IEnumerable<BowInstance> added, BowInstance expected){
                        TestEquipmentSetInventory equipInv = MakeEquipInventory();

                        foreach(var item in added)
                            equipInv.Add(item);

                        Assert.That(equipInv.equippedBow, Is.SameAs(expected));
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
                                object[] case1 = new object[]{case1Added, bowInst_A_1};
                                yield return case1;
                            }
                        }
                    [TestCaseSource(typeof(AddWearInstCases))]
                    public void Add_WearInst_EquipsWear(IEnumerable<WearInstance> added, WearInstance expected){
                        TestEquipmentSetInventory equipInv = MakeEquipInventory();

                        foreach(var item in added)
                            equipInv.Add(item);

                        Assert.That(equipInv.equippedWear, Is.SameAs(expected));
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
                                object[] case1 = new object[]{case1Added, wearInst_A_1};
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
                                ShieldInstance shieldInst_A = MakeShieldInstance(0);
                                MeleeWeaponInstance mWeaponInst_A = MakeMeleeWeaponInstance(0);
                                QuiverInstance quiverInst_A = MakeQuiverInstance(0);
                                PackInstance packInst_A = MakePackInstance(0);
                                IEnumerable<CarriedGearInstance> case1_1Added = new CarriedGearInstance[]{shieldInst_A};
                                IEnumerable<CarriedGearInstance> case1_2Added = new CarriedGearInstance[]{mWeaponInst_A};
                                IEnumerable<CarriedGearInstance> case1_3Added = new CarriedGearInstance[]{quiverInst_A};
                                IEnumerable<CarriedGearInstance> case1_4Added = new CarriedGearInstance[]{packInst_A};
                                yield return new object[]{1, case1_1Added, case1_1Added};
                                yield return new object[]{1, case1_2Added, case1_2Added};
                                yield return new object[]{1, case1_3Added, case1_3Added};
                                yield return new object[]{1, case1_4Added, case1_4Added};

                                IEnumerable<CarriedGearInstance> case2Added = new CarriedGearInstance[]{
                                    shieldInst_A, mWeaponInst_A
                                };
                                IEnumerable<CarriedGearInstance> case3Added = new CarriedGearInstance[]{
                                    shieldInst_A, mWeaponInst_A, quiverInst_A
                                };
                                IEnumerable<CarriedGearInstance> case4Added = new CarriedGearInstance[]{
                                    shieldInst_A, mWeaponInst_A, quiverInst_A, packInst_A
                                };
                                yield return new object[]{2, case2Added, case2Added};
                                yield return new object[]{3, case3Added, case3Added};
                                yield return new object[]{4, case4Added, case4Added};
                            }
                        }

                    [Test]
                    [ExpectedException(typeof(System.ArgumentNullException))]
                    public void Add_Null_ThrowsException(){
                        TestEquipmentSetInventory equipInv = MakeEquipInventory();

                        // System.Exception ex = Assert.Catch<System.ArgumentNullException>(() => equipInv.Add(null));
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
                                ShieldInstance shield_A = MakeShieldInstance(0);
                                ShieldInstance shield_A_2 = MakeShieldInstance(0);
                                MeleeWeaponInstance mWeapon_A = MakeMeleeWeaponInstance(0);
                                QuiverInstance quiver_A = MakeQuiverInstance(0);
                                PackInstance pack_A = MakePackInstance(0);
                                List<CarriedGearInstance> case1Fitted = new List<CarriedGearInstance>(new CarriedGearInstance[]{
                                    shield_A, mWeapon_A, quiver_A, pack_A
                                });
                                object[] case1 = new object[]{case1Fitted, shield_A_2};
                                yield return case1;
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
                                BowInstance bow_A = MakeBowInstance(0);
                                BowInstance bow_A_1 = MakeBowInstance(0);
                                WearInstance wear_A = MakeWearInstance(0);
                                WearInstance wear_A_1 = MakeWearInstance(0);
                                ShieldInstance shield_A = MakeShieldInstance(0);
                                ShieldInstance shield_A_1 = MakeShieldInstance(0);
                                MeleeWeaponInstance mWeapon_A = MakeMeleeWeaponInstance(0);
                                QuiverInstance quiver_A = MakeQuiverInstance(0);
                                PackInstance pack_A = MakePackInstance(0);
                                List<InventoryItemInstance> case1Added = new List<InventoryItemInstance>(new InventoryItemInstance[]{
                                    bow_A, wear_A, shield_A, mWeapon_A, quiver_A, pack_A
                                });
                                object[] case1 = new object[]{case1Added, new InventoryItemInstance[]{bow_A_1}};
                                yield return case1;
                                object[] case2 = new object[]{case1Added, new InventoryItemInstance[]{wear_A_1}};
                                yield return case2;
                                object[] case3 = new object[]{case1Added, new InventoryItemInstance[]{shield_A_1}};
                                yield return case3;
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
                                List<CarriedGearInstance> case1ExpCGs = new List<CarriedGearInstance>(new CarriedGearInstance[]{
                                    shield_A, mWeapon_A, quiver_A, pack_A
                                });
                                object[] case1 = new object[]{added, case1Removed, null, wear_A, case1ExpCGs};
                                yield return case1;

                                IEnumerable<InventoryItemInstance> case2Removed = new InventoryItemInstance[]{
                                    wear_A
                                };
                                object[] case2 = new object[]{added, case2Removed, bow_A, null, case1ExpCGs};
                                yield return case2;

                                IEnumerable<InventoryItemInstance> case3Removed = new InventoryItemInstance[]{
                                    shield_A, quiver_A
                                };
                                List<CarriedGearInstance> case3ExpCGs = new List<CarriedGearInstance>(new CarriedGearInstance[]{
                                    mWeapon_A, pack_A
                                });
                                object[] case3 = new object[]{added, case3Removed, bow_A, wear_A, case3ExpCGs};
                                yield return case3;


                                IEnumerable<InventoryItemInstance> case4Removed = new InventoryItemInstance[]{
                                    bow_A, wear_A,shield_A, mWeapon_A, quiver_A, pack_A
                                };
                                List<CarriedGearInstance> case4ExpCGs = new List<CarriedGearInstance>();
                                object[] case4 = new object[]{added, case4Removed, null, null, case4ExpCGs};
                                yield return case4;
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
