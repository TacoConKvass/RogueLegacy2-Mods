public static void ArchmageRoll(CharacterData charData)
{
		AbilityType[] spells_available = CharacterCreator.GetAvailableSpells(ClassType.CURIO_SHOPPE_CLASS);

		charData.Weapon = ((spells_available.Length != 0) ? spells_available[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomWeapon", 0, spells_available.Length)] : AbilityType.None);
		spells_available.Remove(charData.Weapon);

		charData.Talent = ((spells_available.Length != 0) ? spells_available[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTalent", 0, spells_available.Length)] : AbilityType.None);
		spells_available.Remove(charData.Talent);

		charData.Spell = ((spells_available.Length != 0) ? spells_available[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomSpells", 0, spells_available.Length)] : AbilityType.None);
		spells_available.Remove(charData.Spell);
}

public static void GenerateRandomLook(CharacterData charData)
{
    //... Normal logic
	if (charData.ClassType == ClassType.MagicWandClass || charData.ClassType == ClassType.AstroClass || charData.ClassType == ClassType.LuteClass)
	{
		CharacterCreator.ArchmageRoll(charData); // Reroll ability slots to only spells
	}
}

public static void ApplyRandomizeKitTrait(CharacterData charData, bool randomizeSpell, bool excludeCurrentAbilities, bool useLineageSeed)
{
    if (charData.ClassType == ClassType.MagicWandClass || charData.ClassType == ClassType.AstroClass || charData.ClassType == ClassType.LuteClass)
	{
		CharacterCreator.ArchmageRoll(charData); // Reroll ability slots to only spells
	}
}
