# Modders Note - Cursed Gambling:

This was an interesting one to work on. Suggested by Waffle Coptor / Detra63, because he wanted to make a no gold playthrough of the game.

There are a number if things modified in the game:

# 1. Trait_EV class:

    public static Vector2 BONUS_CHEST_GOLD_DIE_ROLL = new Vector2(0f, 7f);
    public static float BONUS_CHEST_GOLD_DIE_MOD = 0.5f;

was changed into

    public static Vector2 BONUS_CHEST_GOLD_DIE_ROLL = new Vector2(0f, 0f);
    public static float BONUS_CHEST_GOLD_DIE_MOD = 0f;

BONUS_CHEST_GOLD_DIE_ROLL is responsible for the percentage value of gold you acquire from the chest, after multiplying the random output from that range by BONUS_CHEST_GOLD_DIE_MOD.

# 2. CharcterCreator class, GetRandomTraits method:
	
	return new Vector2Int((int)traitType, (int)traitType2);

was changed into

	return new Vector2Int(880, (int)traitType2)

880 is the int value of TraitType.BonusChestGold which is compulsive gambling.

# 3. TutorialRoomController class, CreateStartingCharacter method:

This is a secret, look into the code to find out, or just start a new save file ;)

# 4. Economy_EV class, GetGoldGain method:

	return num;

was changed into
	
	return -1f;

to ensure the player can’t collect any gold.