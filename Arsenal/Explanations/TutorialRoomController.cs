private void CreateStartingCharacter()
{
	PlayerController playerController = PlayerManager.GetPlayerController();
	CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
	currentCharacter.IsFemale = CharacterCreator.GetRandomGender(true);
	CharacterCreator.GenerateRandomLook(currentCharacter);
	string[] availableNames = CharacterCreator.GetAvailableNames(currentCharacter.IsFemale);
	string name = availableNames[UnityEngine.Random.Range(0, availableNames.Length)];
	currentCharacter.Name = name;
	currentCharacter.ClassType = ClassType.SwordClass;

    //Get all available weapons, by checking all weapons available for a Contrarian character
	AbilityType[] availableWeapons = CharacterCreator.GetAvailableWeapons(ClassType.CURIO_SHOPPE_CLASS);
    
    //Randomize for the first time
	currentCharacter.Spell = (currentCharacter.Talent = (currentCharacter.Weapon = availableWeapons[UnityEngine.Random.Range(0, availableWeapons.Length)]));
	
    //Keep re-rolling to avoid duplicates in the ability slots
    while (currentCharacter.Talent == currentCharacter.Spell)
	{
		currentCharacter.Talent = availableWeapons[UnityEngine.Random.Range(0, availableWeapons.Length)];
	}

    //Keep re-rolling to avoid duplicates in the ability slots
	while (currentCharacter.Weapon == currentCharacter.Spell)
	{
		currentCharacter.Weapon = availableWeapons[UnityEngine.Random.Range(0, availableWeapons.Length)];
	}
    
	playerController.LookController.InitializeLook(SaveManager.PlayerSaveData.CurrentCharacter);
	playerController.CharacterClass.ClassType = SaveManager.PlayerSaveData.CurrentCharacter.ClassType;
	playerController.CharacterClass.SetAbility(CastAbilityType.Weapon, SaveManager.PlayerSaveData.CurrentCharacter.Weapon, true);
	playerController.CharacterClass.SetAbility(CastAbilityType.Spell, SaveManager.PlayerSaveData.CurrentCharacter.Spell, true);
	playerController.CharacterClass.SetAbility(CastAbilityType.Talent, SaveManager.PlayerSaveData.CurrentCharacter.Talent, true);
	playerController.ResetCharacter();
	playerController.SetMana(0f, false, true, false);
}