public static void GenerateClass(ClassType classType, CharacterData charDataToMod)
{
    //Run normal logic, but instead of using Gets for every ability type, use for GetAvailableWeapons from Contrarian (all weapons unlocked)
	charDataToMod.ClassType = classType;
	AbilityType[] availableWeapons = CharacterCreator.GetAvailableWeapons(classType);
	AbilityType abilityType = (availableWeapons.Length != 0) ? availableWeapons[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomWeapon", 0, availableWeapons.Length)] : AbilityType.None;
	charDataToMod.Weapon = abilityType;
	AbilityType[] availableSpells = CharacterCreator.GetAvailableWeapons(classType);
	AbilityType abilityType2 = (availableSpells.Length != 0) ? availableSpells[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomSpell", 0, availableSpells.Length)] : AbilityType.None;
	int num = 0;
	while (abilityType2 != AbilityType.None && abilityType2 == abilityType && num < 50)
	{
		num++;
		abilityType2 = availableSpells[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomSpell", 0, availableSpells.Length)];
	}
	if (num >= 50)
	{
		Debug.LogWarning("<color=yellow>Could not find non-duplicate spell in CharacterCreator.</color>");
	}
	charDataToMod.Spell = abilityType2;
	AbilityType[] availableTalents = CharacterCreator.GetAvailableWeapons(classType);
	AbilityType abilityType3 = (availableTalents.Length != 0) ? availableTalents[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTalent", 0, availableTalents.Length)] : AbilityType.None;
	num = 0;
	while (abilityType3 != AbilityType.None && (abilityType3 == abilityType || abilityType3 == abilityType2) && num < 50)
	{
		num++;
		abilityType3 = availableTalents[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTalent", 0, availableTalents.Length)];
	}
	if (num >= 50)
	{
    	Debug.LogWarning("<color=yellow>Could not find non-duplicate talent in CharacterCreator.</color>");
    }
	charDataToMod.Talent = abilityType3;
}