public class Shotgun_Ability : AimedAbility_RL, IAttack, IAbility
{
    public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{

        //I didn't want to do it this way, but I don't remember how I did it last time
		this.frameTimer = 0;
	}

    protected override void Update()
	{
		// Run normal code here, then add this at the end
        // Didn't want to post it here, as I don't want to show too much of the code, as it's not my property
        // And it's unnecessary to show it here

		if (SaveManager.PlayerSaveData.CurrentCharacter.Weapon != AbilityType.ShotgunWeapon && base.CurrentAmmo == 0)
		{
            // Every game update the timer increases by one
			this.frameTimer++;

            // After 120 updates, reload the weapon
			if (this.frameTimer == 120)
			{
				base.CurrentAmmo = base.MaxAmmo;
				this.frameTimer = 0;
			}
		}
	}

	// Just a class member, outside any method, but inside the class
    private int frameTimer;
}