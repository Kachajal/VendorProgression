using System;
using System.Collections.Generic;
using System.Linq;
using Harmony12;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Shields;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Kingdom;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using UnityEngine;

namespace VendorProgression
{
    public class ProgressionLogic
    {
        public static readonly Dictionary<string, string> VendorTableIds = new Dictionary<string, string>()
        {
            {"Arsinoe","afa2c7f292b8e1c4d9c835f0e8047dd3"},
            {"Hassuf", "8c17a31b6a9a6eb4cbb668902e9edcb1"},
            {"Verdel", "7de959347266092448d8a72089ef9778"},
            {"Elina", "8bc41a2cbf853b544bba4fde93dd3b5e"},
        };

        public static readonly Dictionary<string, string> WeaponEnchantmentIds = new Dictionary<string, string>()
        {
            {"Enhancement1", "d42fc23b92c640846ac137dc26e000d4"},
            {"Enhancement2", "eb2faccc4c9487d43b3575d7e77ff3f5"},
            {"Enhancement3", "80bb8a737579e35498177e1e3c75899b"},
            {"Enhancement4", "783d7d496da6ac44f9511011fc5f1979"},
            {"Enhancement5", "bdba267e951851449af552aa9f9e3992"},
            {"StrengthComposite","c3209eb058d471548928a200d70765e0"},
            {"StrengthThrown","c4d213911e9616949937e1520c80aaf3"},
            {"Oversized", "d8e1ebc1062d8cc42abff78783856b0d"},
            {"Flaming", "30f90becaaac51f41bf56641966c4121"},
            {"Frost", "421e54078b7719d40915ce0672511d0b"},
            {"Shock", "7bda5277d36ad114f9f9fd21d0dab658"},
            {"Corrosive", "633b38ff1d11de64a91d490c683ab1c8"},
            {"Thundering", "690e762f7704e1f4aa1ac69ef0ce6a96"},
        };
        
        public static readonly Dictionary<string, string> ArmorEnchantmentIds = new Dictionary<string, string>()
        {
            {"ArmorEnhancementBonus1", "a9ea95c5e02f9b7468447bc1010fe152"},
            {"ArmorEnhancementBonus2", "758b77a97640fd747abf149f5bf538d0"},
            {"ArmorEnhancementBonus3", "9448d3026111d6d49b31fc85e7f3745a"},
            {"ArmorEnhancementBonus4", "eaeb89df5be2b784c96181552414ae5a"},
            {"ArmorEnhancementBonus5", "6628f9d77fd07b54c911cd8930c0d531"},
            {"MithralArmorEnchant", "7b95a819181574a4799d93939aa99aff"},
            {"AdamantineArmorHeavyEnchant", "933456ff83c454146a8bf434e39b1f93"},
            {"AdamantineArmorMediumEnchant", "aa25531ab5bb58941945662aa47b73e7"},
            {"AdamantineArmorLightEnchant", "5faa3aaee432ac444b101de2b7b0faf7"},
            {"ShieldEnhancementBonus1","e90c252e08035294eba39bafce76c119"},
            {"ShieldEnhancementBonus2","7b9f2f78a83577d49927c78be0f7fbc1"},
            {"ShieldEnhancementBonus3","ac2e3a582b5faa74aab66e0a31c935a9"},
            {"ShieldEnhancementBonus4","a5d27d73859bd19469a6dde3b49750ff"},
            {"ShieldEnhancementBonus5","84d191a748edef84ba30c13b8ab83bd9"},
        };

        private static readonly Dictionary<string, string> SpellListIds = new Dictionary<string, string>()
        {
            {"Wizard", "ba0401fdeb4062f40a7aa95b6f07fe89"},
            {"Cleric", "8443ce803d2d31347897a3d85cc32f53"},
            {"Druid", "bad8638d40639d04fa2f80a1cac67d6b"},
            {"Paladin", "9f5be2f7ea64fe04eb40878347b147bc"},
            {"Ranger", "29f3c338532390546bc5347826a655c4"},
            {"Bard", "25a5013493bdcf74bb2424532214d0c8"},
            {"Inquisitor", "57c894665b7895c499b3dce058c284b3"},
        };
        
        private static readonly Dictionary<int, string> WeaponEnhancementLevels = new Dictionary<int, string>()
        {
            {1, "d42fc23b92c640846ac137dc26e000d4"},
            {2, "eb2faccc4c9487d43b3575d7e77ff3f5"},
            {3, "80bb8a737579e35498177e1e3c75899b"},
            {4, "783d7d496da6ac44f9511011fc5f1979"},
            {5, "bdba267e951851449af552aa9f9e3992"},
        };

        private static readonly List<string> AllowedGenericWeaponEnchantments = new List<string>
        {
            WeaponEnchantmentIds["StrengthComposite"],
            WeaponEnchantmentIds["StrengthThrown"],
            WeaponEnchantmentIds["Oversized"]
        };
        
        
        public static readonly Dictionary<int, string> ArmorEnhancementLevels = new Dictionary<int, string>()
        {
            {1, "a9ea95c5e02f9b7468447bc1010fe152"},
            {2, "758b77a97640fd747abf149f5bf538d0"},
            {3, "9448d3026111d6d49b31fc85e7f3745a"},
            {4, "eaeb89df5be2b784c96181552414ae5a"},
            {5, "6628f9d77fd07b54c911cd8930c0d531"}
        };
        
        public static readonly List<string> MaterialArmorEnchantments = new List<string>
            {
                ArmorEnchantmentIds["MithralArmorEnchant"],
                ArmorEnchantmentIds["AdamantineArmorHeavyEnchant"],
                ArmorEnchantmentIds["AdamantineArmorMediumEnchant"],
                ArmorEnchantmentIds["AdamantineArmorLightEnchant"]
            };
        
        public static readonly List<WeaponCategory> WeaponCategoriesDisabled = new List<WeaponCategory>()
        {
            WeaponCategory.WeaponLightShield,
            WeaponCategory.WeaponHeavyShield,
            WeaponCategory.SpikedLightShield,
            WeaponCategory.SpikedHeavyShield,
            WeaponCategory.OtherNaturalWeapons,
            WeaponCategory.Bite,
            WeaponCategory.Bomb,
            WeaponCategory.Claw,
            WeaponCategory.Gore,
            WeaponCategory.Ray,
            WeaponCategory.Shuriken,
            WeaponCategory.Touch,
            WeaponCategory.KineticBlast,
            WeaponCategory.UnarmedStrike,
            WeaponCategory.HandCrossbow,
            WeaponCategory.Siangham,
            WeaponCategory.HeavyRepeatingCrossbow,
            WeaponCategory.LightRepeatingCrossbow,
        };

        public static readonly List<string> WeaponGUIDSDisallowed = new List<string>
        {
            "16ed105ee0430824aa7b5cc5f607ddde",
            "5b035fa4ef3b00b4cb874989f68de101",
            "0b415135435fb2345ba026723b91f256",
            "fa04c89b6a127074b97a6476095a09f0",  
            "aeed4b91f05b00747ab5f28e2785bcb7",  // Corrosive gnome hooked hammer +2 with wrong price
            "07050f33175031a499a47a537e1053f5"   // Corrosive gnome hooked hammer +1 with wrong price
        };
        // In the future just use an exhaustive list of allowed items, jfc.

        public static readonly List<string> ArmorGUIDSDisallowed = new List<string>
        {
            "de1916a857f28ff428f0283585a250e4", // Duplicated leather armor +4 with the wrong name
            "385be51e5706a55418384f70d8341371", // Nonmetal adamantine and mithral armors.
            "bea0adc2ebc043c4cbcd5f910b0516c2", // Definitely just using whitelists next time
            "c7d722804ea31e14fad47f257ca1f271",
            "1e27aa1665f0ba34bbb8cfbc534bdf9d",
            "4a3ae924f673e6e42b4047306f718fdd",
            "bae8f2593ea154445a4cf5ab85ff65df",
            "5906be44cc0cee84b9a35f2df2e2868d",
            "b5b5b754653139d4f9784780f9f25c84",
            "35a7e70380c58c54d8c122981b57bc3f",
            "dcbf665e8bcc7aa4d835e285348a3131",
            "6307dd6b138519144a5c645d898ca036",
            "61bea6873b9d9dc43868b26274513cc7",
            "7f6dcdd0816769c4a98040b22b84375e",
            "2876c733d8f9fdc4fbc546c4a1a4255f",
            "042cb9d946a728a4790f395c9b7b9ea6",
            "819fdeb8ec336d34b996bec32df554eb",
            "4a5683c13a5781540b7142ebec61852b",
            "87a23b06677bffe4b8743bc00216421a",
            "86ead91db2b47dd4486030e343f8a79e",
            "d629b83aaba78c247a37423bb2da6fcf",
            "0aea4beb2c17f8a4caa6779074c5bb74",
            "3fab8986c64d16c4d811eefa972572cc",
            "d8bd56780dae4d946a2890ff6c86207a",
            "ff4117b74c2f3d34f90a306f6fbcdf08",
            "2eee42d5f094af649affbdabf0c653b7",
            "cb390f2ae6df5724c9ec586842169ba4",
            "e1616329aa7c05845b538d0f28bcc7c8",
            "3885106d163f4ea4083887fdaa23aec6",
            "59fc699b1d7aff14f969919c7e6daca1",
            "99f57eda92fdc0d49be16ee2b4634d2d",
            "de03b0e4e279b304c95aee8a5439b7bf",
            "b75716068d7f2ac4ea0a66b1a8936061",
            "f316bef7bc9b4ca4db97150e9fad4d4f",
            "51f88ef02a90de545bdbe926a1932966",
            "c65ea2f4826d35f409cf0b9741367ff9",
            "fbe5f1d61460ad4428d5cc73e8baefb6",
            "e25629124629ee547a86f4541fae6e61",
            "deaf3d0f0e539414cbc2fa0222faec07",
            "0a7642af334dfd14b893173e9d06eeb1",
            "9da3ad3eb90caa64aa2926324f4f80f2",
            "70656d8db8a38a84b89c1fd63424abf9",
            "32dec52181398bc4ea8cc2d74c86fa93",
            "aa47dbe229678574f9685fcbdf17c9a4",
            "a057d77b4f780e6459229cabead9e7be",
            "9c7327c0221f04a4482f3860a6302a48",
            "7ca49db1cde980d489b6aa5112f8319f",
            "2f64aed9afb8937499f520ced8f4987e",
            "dc75ee756a1d73740b5a1b0be4cc37f6",
            "8e9417ea0c1aa884d9615a11dbd5c2b0", // Nonmetal adamantine/mithral end
        };
        static void AddItemsToVendorStock<T>(List<T> items, string vendorTableId, int amountToAdd = 99)
        {
            // Adds provided items to the stock of a SharedVendorTable with the provided GUID.
            // The items are added to existing in-game stock, not to the blueprint.
            
            var vendorTable = ResourcesLibrary.TryGetBlueprint<BlueprintSharedVendorTable>(vendorTableId);
            var components = ConvertItemsToLoot(items, amountToAdd);
            var adder = ScriptableObject
                .CreateInstance<Kingmaker.Designers.EventConditionActionSystem.Actions.AddItemsToVendor>();
            adder.Loot = components.GenerateItems();
            adder.SharedVendor = vendorTable;
            adder.RunAction();
        }

        static BlueprintUnitLoot ConvertItemsToLoot<T>(List<T> items, int amountToAdd = 99)
        {
            var result = ScriptableObject.CreateInstance<BlueprintUnitLoot>();
            var components = new BlueprintComponent[items.Count()];
            var itemsAdded = 0;
            foreach (var item in items)
            {
                var lootComponent = ScriptableObject.CreateInstance<LootItemsPackFixed>();
                var lootItem = new LootItem();
                Traverse.Create(lootItem).Field("m_Item").SetValue(item);
                Traverse.Create(lootItem).Field("m_Type").SetValue(LootItemType.Item);
                Traverse.Create(lootComponent).Field("m_Item").SetValue(lootItem);
                Traverse.Create(lootComponent).Field("m_Count").SetValue(amountToAdd);
                components[itemsAdded] = lootComponent;
                itemsAdded++;
            }

            result.ComponentsArray = components;
            return result;
        }

        public static List<BlueprintItemShield> GetFilterShields(List<string> allowedEnchantments1,
            bool uniqueByName = true)
        {
            // Gets all shields and returns only those that have an enchantment from the specified list.
            var shields = ResourcesLibrary.GetBlueprints<BlueprintItemShield>();
            var shieldsGeneric = shields.Where(shield =>
                shield.Icon != null
                && shield.SellPrice > 25
                && shield.Description == ""
                && shield.FlavorText == ""
                && shield.IsNonRemovable == false);
            var shieldsFiltered = shieldsGeneric;

            shieldsFiltered = shieldsFiltered.Where(shield =>
                (shield.Enchantments.Count == 1) &&
                shield.Enchantments.Any(enchantment => allowedEnchantments1.Contains(enchantment.AssetGuid)));

            if (uniqueByName)
            {
                shieldsFiltered = shieldsFiltered
                    .GroupBy(weapon => weapon.Name)
                    .Select(group => group.First());
            }

            return shieldsFiltered.ToList();
        }
        
        public static List<BlueprintItemArmor> GetFilterArmors(List<string> allowedEnchantments1,
            List<string> allowedEnchantments2 = null, bool uniqueByName = true)
        {
            // Gets all armors and filters them by provided enchantment GUID lists.
            // If two GUID lists are provided, then the armors must have an enchantment
            // from both lists.
            
            var armors = ResourcesLibrary.GetBlueprints<BlueprintItemArmor>();
            
            var armorsGeneric = armors.Where(armor =>
                armor.SellPrice > 25
                && armor.Description == ""
                && armor.FlavorText == ""
                && armor.IsNonRemovable == false
                && armor.Icon != null
                && !ArmorGUIDSDisallowed.Contains(armor.AssetGuid));

            

            var armorsFiltered = armorsGeneric;
            if (allowedEnchantments2 == null)
            {
                armorsFiltered = armorsFiltered.Where(armor =>
                    (armor.Enchantments.Count == 1 &&
                     armor.Enchantments.Any(enchantment => allowedEnchantments1.Contains(enchantment.AssetGuid))));
            }
            else
            {
                armorsFiltered = armorsFiltered.Where(armor =>
                    (armor.Enchantments.Count == 2 &&
                     armor.Enchantments.Any(enchantment => allowedEnchantments1.Contains(enchantment.AssetGuid)) &&
                     armor.Enchantments.Any(enchantment => allowedEnchantments2.Contains(enchantment.AssetGuid))));
            }
            
            if (uniqueByName)
            {
                armorsFiltered = armorsFiltered
                    .GroupBy(weapon => weapon.Name)
                    .Select(group => group.First()); 
            }
            return armorsFiltered.ToList();
        }

        public static List<BlueprintItemWeapon> GetFilterWeapons(List<String> allowedEnchantments1,
            List<string> allowedEnchantments2 = null, List<string> allowedEnchantments3 = null,
            WeaponCategory category = WeaponCategory.Touch, bool uniqueByName = true)
        {
            // Gets all armors and filters them by provided enchantment GUID lists.
            // If more than one GUID list is provided, then the weapons must have an enchantment
            // from all provided lists.
            
            var weapons = ResourcesLibrary.GetBlueprints<BlueprintItemWeapon>();
            
            var weaponsGeneric = weapons.Where(weapon =>
                (weapon.FlavorText == ""
                 && weapon.IsMagic
                 && weapon.SellPrice > 25
                 && weapon.DamageType.Physical.Material == 0 
                 && weapon.Icon != null
                 && weapon.Description == ""
                 && weapon.IsNonRemovable == false
                 && !WeaponCategoriesDisabled.Contains(weapon.Category)
                 && !WeaponGUIDSDisallowed.Contains(weapon.AssetGuid)));

            if (uniqueByName)
            {
                weaponsGeneric = weaponsGeneric
                    .GroupBy(weapon => weapon.Name)
                    .Select(group => group.First());
            }
            
            var weaponsFiltered = weaponsGeneric;

            if (category != WeaponCategory.Touch)
            {
                weaponsFiltered = weaponsFiltered.Where(weapon => weapon.Category == category);
            }
            
            if (allowedEnchantments2 == null)
            {
                weaponsFiltered = weaponsFiltered.Where(weapon =>
                    (weapon.Enchantments.Count == 1 &&
                     weapon.Enchantments.Any(enchantment => allowedEnchantments1.Contains(enchantment.AssetGuid))));    
            } else if (allowedEnchantments3 == null)
            {
                weaponsFiltered = weaponsFiltered.Where(weapon =>
                    (weapon.Enchantments.Count == 2 &&
                     weapon.Enchantments.Any(enchantment => allowedEnchantments1.Contains(enchantment.AssetGuid)) &&
                     weapon.Enchantments.Any(enchantment => allowedEnchantments2.Contains(enchantment.AssetGuid))));
            }
            else
            {
                weaponsFiltered = weaponsFiltered.Where(weapon =>
                    (weapon.Enchantments.Count == 3 &&
                     weapon.Enchantments.Any(enchantment => allowedEnchantments1.Contains(enchantment.AssetGuid)) &&
                     weapon.Enchantments.Any(enchantment => allowedEnchantments2.Contains(enchantment.AssetGuid)) &&
                     weapon.Enchantments.Any(enchantment => allowedEnchantments3.Contains(enchantment.AssetGuid))));
            }
            
            return weaponsFiltered.ToList();
        }


        public static List<BlueprintItemEquipmentUsable> GetFilterMagicUsables(int casterLevel, string scrollsOrWands = "scrolls",
            string arcaneOrDivine = "arcane")
        {
            // Gets all usables and filters them to return arcane or divine scrolls/wands of a particular caster level.
            
            var usables = ResourcesLibrary.GetBlueprints<BlueprintItemEquipmentUsable>();
            var wizardSpellList = ResourcesLibrary.TryGetBlueprint<BlueprintSpellList>(SpellListIds["Wizard"]);
            var bardSpellList = ResourcesLibrary.TryGetBlueprint<BlueprintSpellList>(SpellListIds["Bard"]);
            var clericSpellList = ResourcesLibrary.TryGetBlueprint<BlueprintSpellList>(SpellListIds["Cleric"]);
            var paladinSpellList = ResourcesLibrary.TryGetBlueprint<BlueprintSpellList>(SpellListIds["Paladin"]);
            var inquisitorSpellList = ResourcesLibrary.TryGetBlueprint<BlueprintSpellList>(SpellListIds["Inquisitor"]);
            var rangerSpellList = ResourcesLibrary.TryGetBlueprint<BlueprintSpellList>(SpellListIds["Ranger"]);
            var druidSpellList = ResourcesLibrary.TryGetBlueprint<BlueprintSpellList>(SpellListIds["Druid"]);
            
            usables = usables.Where(usable => usable.SellPrice > 0
                                              && usable.RequireUMDIfCasterHasNoSpellInSpellList == true);
            usables = usables.GroupBy(usable => usable.name).Select(group => group.First());
            
            usables = usables.Where(usable => usable.IsActuallyStackable == (scrollsOrWands.Equals("scrolls")));

            if (arcaneOrDivine == "arcane")
            {
                usables = usables.Where(usable => usable.Ability.IsInSpellList(wizardSpellList)
                                                  || usable.Ability.IsInSpellList(bardSpellList));
            }
            else
            {
                usables = usables.Where(usable => usable.Ability.IsInSpellList(clericSpellList)
                                                  || usable.Ability.IsInSpellList(paladinSpellList)
                                                  || usable.Ability.IsInSpellList(rangerSpellList)
                                                  || usable.Ability.IsInSpellList(inquisitorSpellList)
                                                  || usable.Ability.IsInSpellList(druidSpellList));
            }

            return usables.Where(usable => usable.CasterLevel == casterLevel).ToList();
        }
        


        static void AddArcaneStock(int rank)
        {
            var arcaneVendorId = VendorTableIds["Arsinoe"];
            switch (rank)
            {
                case 1:
                    AddItemsToVendorStock(GetFilterMagicUsables(1, "scrolls", "arcane"), arcaneVendorId);
                    AddItemsToVendorStock(GetFilterMagicUsables(2, "scrolls", "arcane"), arcaneVendorId);
                    break;    
                case 2:
                    AddItemsToVendorStock(GetFilterMagicUsables(3, "scrolls", "arcane"), arcaneVendorId);
                    AddItemsToVendorStock(GetFilterMagicUsables(4, "scrolls", "arcane"), arcaneVendorId);
                    AddItemsToVendorStock(GetFilterMagicUsables(1, "wands", "arcane"), arcaneVendorId, 50);
                    break;
                case 3:
                    AddItemsToVendorStock(GetFilterMagicUsables(5, "scrolls", "arcane"), arcaneVendorId);
                    AddItemsToVendorStock(GetFilterMagicUsables(6, "scrolls", "arcane"), arcaneVendorId);
                    AddItemsToVendorStock(GetFilterMagicUsables(2, "wands", "arcane"), arcaneVendorId, 50);
                    break;
                case 4:
                    AddItemsToVendorStock(GetFilterMagicUsables(7, "scrolls", "arcane"), arcaneVendorId, 30);
                    AddItemsToVendorStock(GetFilterMagicUsables(8, "scrolls", "arcane"), arcaneVendorId, 30);
                    AddItemsToVendorStock(GetFilterMagicUsables(3, "wands", "arcane"), arcaneVendorId, 40);
                    break;
                case 5:
                    AddItemsToVendorStock(GetFilterMagicUsables(9, "scrolls", "arcane"), arcaneVendorId, 15);
                    AddItemsToVendorStock(GetFilterMagicUsables(10, "scrolls", "arcane"), arcaneVendorId, 15);
                    AddItemsToVendorStock(GetFilterMagicUsables(4, "wands", "arcane"), arcaneVendorId, 40);
                    break;
                case 6:
                    AddItemsToVendorStock(GetFilterMagicUsables(11, "scrolls", "arcane"), arcaneVendorId, 10);
                    AddItemsToVendorStock(GetFilterMagicUsables(12, "scrolls", "arcane"), arcaneVendorId, 10);
                    AddItemsToVendorStock(GetFilterMagicUsables(5, "wands", "arcane"), arcaneVendorId, 20);
                    break;
                case 7:
                    AddItemsToVendorStock(GetFilterMagicUsables(13, "scrolls", "arcane"), arcaneVendorId, 4);
                    AddItemsToVendorStock(GetFilterMagicUsables(14, "scrolls", "arcane"), arcaneVendorId, 4);
                    AddItemsToVendorStock(GetFilterMagicUsables(6, "wands", "arcane"), arcaneVendorId, 20);
                    break;
                case 8:
                    AddItemsToVendorStock(GetFilterMagicUsables(15, "scrolls", "arcane"), arcaneVendorId, 2);
                    AddItemsToVendorStock(GetFilterMagicUsables(16, "scrolls", "arcane"), arcaneVendorId, 2);
                    AddItemsToVendorStock(GetFilterMagicUsables(7, "wands", "arcane"), arcaneVendorId, 5);
                    break;
                case 9:
                    AddItemsToVendorStock(GetFilterMagicUsables(15, "scrolls", "arcane"), arcaneVendorId, 2);
                    AddItemsToVendorStock(GetFilterMagicUsables(16, "scrolls", "arcane"), arcaneVendorId, 2);
                    AddItemsToVendorStock(GetFilterMagicUsables(8, "wands", "arcane"), arcaneVendorId, 5);
                    break;
                case 10:
                    AddItemsToVendorStock(GetFilterMagicUsables(15, "scrolls", "arcane"), arcaneVendorId, 2);
                    AddItemsToVendorStock(GetFilterMagicUsables(16, "scrolls", "arcane"), arcaneVendorId, 2);
                    AddItemsToVendorStock(GetFilterMagicUsables(9, "wands", "arcane"), arcaneVendorId, 5);
                    break;
            }
        }
        
        static void AddDivineStock(int rank)
        {
            var divineVendorId = VendorTableIds["Arsinoe"];
            switch (rank)
            {
                case 1:
                    AddItemsToVendorStock(GetFilterMagicUsables(1, "scrolls", "divine"), divineVendorId);
                    AddItemsToVendorStock(GetFilterMagicUsables(2, "scrolls", "divine"), divineVendorId);
                    break;    
                case 2:
                    AddItemsToVendorStock(GetFilterMagicUsables(3, "scrolls", "divine"), divineVendorId);
                    AddItemsToVendorStock(GetFilterMagicUsables(4, "scrolls", "divine"), divineVendorId);
                    AddItemsToVendorStock(GetFilterMagicUsables(1, "wands", "divine"), divineVendorId, 50);
                    break;
                case 3:
                    AddItemsToVendorStock(GetFilterMagicUsables(5, "scrolls", "divine"), divineVendorId);
                    AddItemsToVendorStock(GetFilterMagicUsables(6, "scrolls", "divine"), divineVendorId);
                    AddItemsToVendorStock(GetFilterMagicUsables(2, "wands", "divine"), divineVendorId, 50);
                    break;
                case 4:
                    AddItemsToVendorStock(GetFilterMagicUsables(7, "scrolls", "divine"), divineVendorId, 30);
                    AddItemsToVendorStock(GetFilterMagicUsables(8, "scrolls", "divine"), divineVendorId, 30);
                    AddItemsToVendorStock(GetFilterMagicUsables(3, "wands", "divine"), divineVendorId, 40);
                    break;
                case 5:
                    AddItemsToVendorStock(GetFilterMagicUsables(9, "scrolls", "divine"), divineVendorId, 15);
                    AddItemsToVendorStock(GetFilterMagicUsables(10, "scrolls", "divine"), divineVendorId, 15);
                    AddItemsToVendorStock(GetFilterMagicUsables(4, "wands", "divine"), divineVendorId, 40);
                    break;
                case 6:
                    AddItemsToVendorStock(GetFilterMagicUsables(11, "scrolls", "divine"), divineVendorId, 10);
                    AddItemsToVendorStock(GetFilterMagicUsables(12, "scrolls", "divine"), divineVendorId, 10);
                    AddItemsToVendorStock(GetFilterMagicUsables(5, "wands", "divine"), divineVendorId, 20);
                    break;
                case 7:
                    AddItemsToVendorStock(GetFilterMagicUsables(13, "scrolls", "divine"), divineVendorId, 4);
                    AddItemsToVendorStock(GetFilterMagicUsables(14, "scrolls", "divine"), divineVendorId, 4);
                    AddItemsToVendorStock(GetFilterMagicUsables(6, "wands", "divine"), divineVendorId, 20);
                    break;
                case 8:
                    AddItemsToVendorStock(GetFilterMagicUsables(15, "scrolls", "divine"), divineVendorId, 2);
                    AddItemsToVendorStock(GetFilterMagicUsables(16, "scrolls", "divine"), divineVendorId, 2);
                    AddItemsToVendorStock(GetFilterMagicUsables(7, "wands", "divine"), divineVendorId, 5);
                    break;
                case 9:
                    AddItemsToVendorStock(GetFilterMagicUsables(15, "scrolls", "divine"), divineVendorId, 2);
                    AddItemsToVendorStock(GetFilterMagicUsables(16, "scrolls", "divine"), divineVendorId, 2);
                    AddItemsToVendorStock(GetFilterMagicUsables(8, "wands", "divine"), divineVendorId, 5);
                    break;
                case 10:
                    AddItemsToVendorStock(GetFilterMagicUsables(15, "scrolls", "divine"), divineVendorId, 2);
                    AddItemsToVendorStock(GetFilterMagicUsables(16, "scrolls", "divine"), divineVendorId, 2);
                    AddItemsToVendorStock(GetFilterMagicUsables(9, "wands", "divine"), divineVendorId, 5);
                    break;
            }
        }

        static void AddMilitaryStock(int rank)
        {
            var militaryVendorId = VendorTableIds["Verdel"];
            switch (rank)
            {
                    case 1:
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[1]}),
                            militaryVendorId, 5);
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[1]},
                            AllowedGenericWeaponEnchantments), militaryVendorId, 5);
                        break;
                    case 2:
                        
                        break;
                    case 3:
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[2]}),
                            militaryVendorId, 5);
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[2]},
                            AllowedGenericWeaponEnchantments), militaryVendorId, 5);
                        break;
                    case 4:
                        break;
                    case 5:
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[3]}),
                            militaryVendorId, 5);
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[3]},
                            AllowedGenericWeaponEnchantments), militaryVendorId, 5);
                        break;
                    case 6:
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[1]},
                            new List<string> {WeaponEnchantmentIds["Corrosive"]}), militaryVendorId, 1);
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[1]},
                            new List<string> {WeaponEnchantmentIds["Corrosive"]}, AllowedGenericWeaponEnchantments),
                            militaryVendorId, 1);
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[1]},
                                new List<string> {WeaponEnchantmentIds["Frost"]}, AllowedGenericWeaponEnchantments,
                                WeaponCategory.Longbow),
                            militaryVendorId, 1);
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[1]},
                                new List<string> {WeaponEnchantmentIds["Shock"]}, AllowedGenericWeaponEnchantments,
                                WeaponCategory.Shortbow),
                            militaryVendorId, 1);
                        break;
                    case 7:
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[4]}),
                            militaryVendorId, 2);
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[4]}, 
                            AllowedGenericWeaponEnchantments), militaryVendorId, 2);
                        break;
                    case 8:
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[2]},
                            new List<string> {WeaponEnchantmentIds["Corrosive"]}), militaryVendorId, 1);
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[2]},
                            new List<string> {WeaponEnchantmentIds["Corrosive"]}, AllowedGenericWeaponEnchantments),
                            militaryVendorId, 1);
                        break;
                    case 9:
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[5]}),
                            militaryVendorId, 2);
                        AddItemsToVendorStock(GetFilterWeapons(new List<string> {WeaponEnhancementLevels[5]},
                            AllowedGenericWeaponEnchantments), militaryVendorId, 2);
                        break;
                    case 10:
                        break;
            }
        }
        
        static void AddStabilityStock(int rank)
        {
            var stabilityVendorId = VendorTableIds["Verdel"];
            switch (rank)
            {
                case 1:
                    AddItemsToVendorStock(
                        GetFilterArmors(new List<string> {ArmorEnchantmentIds["ArmorEnhancementBonus1"]}),
                        stabilityVendorId, 5);
                    AddItemsToVendorStock(
                        GetFilterShields(new List<string> {ArmorEnchantmentIds["ShieldEnhancementBonus1"]}),
                        stabilityVendorId, 5);
                    break;
                case 2:
                    /*AddItemsToVendorStock(
                        GetFilterArmors(new List<string> {ArmorEnchantmentIds["ArmorEnhancementBonus1"]},
                            MaterialArmorEnchantments),
                        stabilityVendorId, 2);*/
                    break;
                case 3:
                    AddItemsToVendorStock(
                        GetFilterArmors(new List<string> {ArmorEnchantmentIds["ArmorEnhancementBonus2"]}),
                        stabilityVendorId, 5);
                    AddItemsToVendorStock(
                        GetFilterShields(new List<string> {ArmorEnchantmentIds["ShieldEnhancementBonus2"]}),
                        stabilityVendorId, 5);
                    break;
                case 4:
                    /*AddItemsToVendorStock(
                        GetFilterArmors(new List<string> {ArmorEnchantmentIds["ArmorEnhancementBonus2"]},
                            MaterialArmorEnchantments),
                        stabilityVendorId, 2);*/
                    break;
                case 5:
                    AddItemsToVendorStock(
                        GetFilterArmors(new List<string> {ArmorEnchantmentIds["ArmorEnhancementBonus3"]}),
                        stabilityVendorId, 4);
                    AddItemsToVendorStock(
                        GetFilterShields(new List<string> {ArmorEnchantmentIds["ShieldEnhancementBonus3"]}),
                        stabilityVendorId, 4);
                    break;
                case 6:
                    /*AddItemsToVendorStock(
                        GetFilterArmors(new List<string> {ArmorEnchantmentIds["ArmorEnhancementBonus3"]},
                            MaterialArmorEnchantments),
                        stabilityVendorId, 2);*/
                    break;
                case 7:
                    AddItemsToVendorStock(
                        GetFilterArmors(new List<string> {ArmorEnchantmentIds["ArmorEnhancementBonus4"]}), 
                        stabilityVendorId, 2);
                    AddItemsToVendorStock(
                        GetFilterShields(new List<string> {ArmorEnchantmentIds["ShieldEnhancementBonus4"]}),
                        stabilityVendorId, 2);
                    break;
                case 8:
                    /*AddItemsToVendorStock(
                        GetFilterArmors(new List<string> {ArmorEnchantmentIds["ArmorEnhancementBonus4"]},
                            MaterialArmorEnchantments),
                        stabilityVendorId, 1);*/
                    break;
                case 9:
                    AddItemsToVendorStock(
                        GetFilterArmors(new List<string> {ArmorEnchantmentIds["ArmorEnhancementBonus5"]}), 
                        stabilityVendorId, 2);
                    AddItemsToVendorStock(
                        GetFilterShields(new List<string> {ArmorEnchantmentIds["ShieldEnhancementBonus5"]}),
                        stabilityVendorId, 2);
                    break;
                case 10:
                    /*AddItemsToVendorStock(
                        GetFilterArmors(new List<string> {ArmorEnchantmentIds["ArmorEnhancementBonus5"]},
                            MaterialArmorEnchantments),
                        stabilityVendorId, 1);*/
                    break;
            }
        }

        
        public static void AddStock()
        {
            // Adds whole reward stock on first gameload for a given save
            // Also ran to update stock on stat rankups
            if (Game.Instance.Player.Kingdom == null || Main.enabled == false)
            {
                return;
            }
            var stockUpToDate = Game.Instance.Player.MainCharacter.Value.FreeformData["stockUpToDate"];
            int arcaneRank = Game.Instance.Player.Kingdom.Stats.Arcane.Rank;
            int divineRank = Game.Instance.Player.Kingdom.Stats.Arcane.Rank;
            int militaryRank = Game.Instance.Player.Kingdom.Stats.Arcane.Rank;
            int stabilityRank = Game.Instance.Player.Kingdom.Stats.Arcane.Rank;
            if (stockUpToDate == 0)
            {
                for (var i = 0; i < arcaneRank; i++)
                {
                    AddArcaneStock(i);
                }

                for (var i = 0; i < divineRank; i++)
                {
                    AddDivineStock(i);
                }
                for (var i = 0; i < militaryRank; i++)
                {
                    AddMilitaryStock(i);
                }
                for (var i = 0; i < stabilityRank; i++)
                {
                    AddStabilityStock(i);
                }
                Game.Instance.Player.MainCharacter.Value.FreeformData["stockUpToDate"] = 1;
            }
            AddArcaneStock(arcaneRank);
            AddDivineStock(divineRank);
            AddMilitaryStock(militaryRank);
            AddStabilityStock(stabilityRank);
        }
        
        [HarmonyPatch(typeof(Kingmaker.Player), "PostLoad")]
        static class GameLoadGamePatch
        {
            static void Postfix()
            {
                if (Game.Instance.Player.MainCharacter.Value.FreeformData["stockUpToDate"] == 0)
                {
                    AddStock();
                }
            }
        }
        
        [HarmonyPatch(typeof(Kingmaker.Kingdom.Actions.KingdomActionImproveStat), "RunAction")]
        static class KingdomActionsImproveStatPatch
        {
            static void Postfix(Kingmaker.Kingdom.Actions.KingdomActionImproveStat __instance)
            {
                if (__instance.StatType == KingdomStats.Type.Arcane 
                    || __instance.StatType == KingdomStats.Type.Divine
                    || __instance.StatType == KingdomStats.Type.Military
                    || __instance.StatType == KingdomStats.Type.Stability)
                {
                    AddStock();
                }
                
            }
        }
    }
}