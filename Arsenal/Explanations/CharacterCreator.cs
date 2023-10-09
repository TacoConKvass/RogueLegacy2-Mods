using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CharacterCreator
{
	public static void GenerateClass(ClassType classType, CharacterData charDataToMod)
	{
		//Run normal logic
		CharacterCreator.RerollArsenal(charDataToMod);
	}

	public static void ApplyRandomizeKitTrait(CharacterData charData, bool randomizeSpell, bool excludeCurrentAbilities, bool useLineageSeed)
	{
		//Run normal logic
		CharacterCreator.RerollArsenal(charData);
	}

	public static void RerollArsenal(CharacterData charData)
	{
		AbilityType[] availableWeapons = CharacterCreator.GetAvailableWeapons(ClassType.CURIO_SHOPPE_CLASS);
		availableWeapons.Add(AbilityType.KunaiWeapon);
		availableWeapons.Add(AbilityType.KiStrikeTalent);

		AbilityType[] array = availableWeapons;
		AbilityType weaponRoll = (array.Length != 0) ? array[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomWeapon", 0, array.Length)] : AbilityType.None;
		charData.Weapon = weaponRoll;

		AbilityType[] array2 = availableWeapons;
		AbilityType spellRoll = (array2.Length != 0) ? array2[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomSpell", 0, array2.Length)] : AbilityType.None;
		charData.Spell = spellRoll;

		AbilityType[] array3 = availableWeapons;
		AbilityType talentRoll = (array3.Length != 0) ? array3[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTalent", 0, array3.Length)] : AbilityType.None;
		charData.Talent = talentRoll;
	}
}