using System;
using System.Collections;
using UnityEngine;

public class PlayerLookController : LookController
{
	public bool IsClassLookInitialized { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		this.m_onEquippedChanged = new Action<MonoBehaviour, EventArgs>(this.OnEquippedChanged);
	}

    public override void Initialize()
	{
		base.Initialize();
		this.m_playerController = base.GetComponent<PlayerController>();
		this.m_storedBaseScale = base.gameObject.transform.localScale;
		if (PlayerLookController.m_critMatPropertyBlock == null)
		{
			PlayerLookController.m_critMatPropertyBlock = new MaterialPropertyBlock();
			ColorUtility.TryParseHtmlString("#FFB000", out PlayerLookController.m_critStartColor);
			ColorUtility.TryParseHtmlString("#795C00", out PlayerLookController.m_critEndColor);
		}
	}

	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.EquippedChanged, this.m_onEquippedChanged);
	}

	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.EquippedChanged, this.m_onEquippedChanged);
	}

	protected virtual IEnumerator Start()
	{
		if (this.m_loadFromSaveFile)
		{
			yield return new WaitUntil(() => this.m_playerController.IsInitialized && this.m_playerController.CharacterClass.IsInitialized);
			CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
			if (this.m_playerController.CharacterClass.OverrideSaveFileValues)
			{
				currentCharacter.ClassType = this.m_playerController.CharacterClass.ClassType;
				currentCharacter.Weapon = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false).AbilityType;
			}
			if (currentCharacter.Weapon == AbilityType.None)
			{
				BaseAbility_RL ability = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false);
				if (ability != null)
				{
					currentCharacter.Weapon = ability.AbilityType;
				}
			}
			this.InitializeLook(currentCharacter);
			if (SaveManager.PlayerSaveData.MushroomStateApplied)
			{
				this.m_playerController.SetMushroomState(SaveManager.PlayerSaveData.MushroomState, false, false);
			}
		}
		yield break;
	}

	public override void InitializeLook(CharacterData charData)
	{
		base.InitializeLook(charData);
		this.InitializeScale(charData);
		this.InitializeTraitLook(charData);
		this.InitializeEquipmentLook(charData);
		base.UpdateCustomMeshArray();
		base.ApplyOutlineScale();
		this.IsClassLookInitialized = true;
	}

	public void InitializeScale(CharacterData charData)
	{
		Vector3 storedBaseScale = new Vector3(1.4f, 1.4f, 1f);
		if (this.m_useCurrentScaleAsBase)
		{
			storedBaseScale = this.m_storedBaseScale;
		}
		if (!this.m_ignoreScaleTraits)
		{
			if (charData.TraitOne == TraitType.YouAreLarge || charData.TraitTwo == TraitType.YouAreLarge)
			{
				storedBaseScale.x *= 1.5f;
				storedBaseScale.y *= 1.5f;
			}
			else if (charData.TraitOne == TraitType.YouAreSmall || charData.TraitTwo == TraitType.YouAreSmall)
			{
				storedBaseScale.x *= 0.55f;
				storedBaseScale.y *= 0.55f;
			}
		}
		if (this.m_clampPlayerScale)
		{
			storedBaseScale.x = Mathf.Clamp(storedBaseScale.x, 0.77f, 2.1f);
			storedBaseScale.y = Mathf.Clamp(storedBaseScale.y, 0.77f, 2.1f);
		}
		base.transform.localScale = storedBaseScale;
	}

	private void SetMainColor(SkinnedMeshRenderer renderer, Color color)
	{
		renderer.GetPropertyBlock(base.PropertyBlock);
		base.PropertyBlock.SetColor(ShaderID_RL._MainColor, color);
		renderer.SetPropertyBlock(base.PropertyBlock);
	}

	public void InitializeTraitLook(CharacterData charData)
	{
		base.Animator.SetFloat("BoneStructureType", 0f);
		base.Animator.SetFloat("LimbType", 0f);
		if (charData.TraitOne == TraitType.PlayerKnockedLow || charData.TraitTwo == TraitType.PlayerKnockedLow)
		{
			base.Animator.SetFloat("BoneStructureType", 2f);
		}
		if (charData.TraitOne == TraitType.PlayerKnockedFar || charData.TraitTwo == TraitType.PlayerKnockedFar)
		{
			base.Animator.SetFloat("BoneStructureType", 1f);
		}
		if (charData.TraitOne == TraitType.EnemyKnockedLow || charData.TraitTwo == TraitType.EnemyKnockedLow)
		{
			base.Animator.SetFloat("LimbType", 1f);
		}
		if (charData.TraitOne == TraitType.EnemyKnockedFar || charData.TraitTwo == TraitType.EnemyKnockedFar)
		{
			base.Animator.SetFloat("LimbType", 2f);
		}
		if (charData.ClassType == ClassType.CannonClass)
		{
			if (base.RightEyeGeo.gameObject.activeSelf)
			{
				base.RightEyeGeo.gameObject.SetActive(false);
			}
		}
		else if (!base.RightEyeGeo.gameObject.activeSelf)
		{
			base.RightEyeGeo.gameObject.SetActive(true);
		}
		if (charData.TraitOne == TraitType.Vampire || charData.TraitTwo == TraitType.Vampire)
		{
			Color color = new Color(1f, 1f, 1f, 1f);
			this.SetMainColor(base.HeadGeo, color);
			Color color2 = new Color(0.15f, 0.17f, 0.23f, 1f);
			this.SetMainColor(base.HelmetHairGeo, color2);
			this.SetMainColor(base.ChestHairGeo, color2);
			Color color3 = new Color(1f, 0f, 0f, 1f);
			this.SetMainColor(base.LeftEyeGeo, color3);
			this.SetMainColor(base.RightEyeGeo, color3);
			base.MouthGeo.sharedMaterial = LookLibrary.VampireFangsMaterial;
		}
		if (charData.TraitOne == TraitType.YouAreBlue || charData.TraitTwo == TraitType.YouAreBlue)
		{
			Color color4 = new Color(0.27450982f, 0.6313726f, 1f);
			this.SetMainColor(base.HeadGeo, color4);
		}
		if (charData.TraitOne == TraitType.BounceTerrain || charData.TraitTwo == TraitType.BounceTerrain)
		{
			base.HeadGeo.sharedMaterial = LookLibrary.ClownHeadMaterial;
			Color color5 = new Color(1f, 1f, 1f, 1f);
			this.SetMainColor(base.HeadGeo, color5);
			Color color6 = new Color(1f, 0.17f, 0.23f, 1f);
			this.SetMainColor(base.HelmetHairGeo, color6);
			this.SetMainColor(base.ChestHairGeo, color6);
			base.LeftEyeGeo.sharedMaterial = LookLibrary.ClownEyesMaterial;
			base.RightEyeGeo.sharedMaterial = LookLibrary.ClownEyesMaterial;
			base.MouthGeo.sharedMaterial = LookLibrary.ClownMouthMaterial;
		}

        //This part was added to change all knights to have Kris' characteristics
		if (charData.ClassType == ClassType.SwordClass)
		{
			Color skinColor = new Color(0.4588f, 0.9843f, 0.9294f);
			this.SetMainColor(base.HeadGeo, skinColor);
			Color hairColor = new Color(0.0413f, 0.0413f, 0.2313f);
			this.SetMainColor(base.HelmetHairGeo, hairColor);
			this.SetMainColor(base.ChestHairGeo, hairColor);
			Color eyeColor = new Color(1f, 0f, 0f, 1f);
			this.SetMainColor(base.LeftEyeGeo, eyeColor);
			this.SetMainColor(base.RightEyeGeo, eyeColor);
		}
		base.GlassesGeo.SetActive(charData.HasGlasses);
	}

	public void InitializeEquipmentLook(CharacterData charData)
	{
		LookCreator.InitializeHelmetLook(charData.HeadEquipmentType, this);
		LookCreator.InitializeArmorLook(charData.ChestEquipmentType, this);
		bool hasCantAttackTrait = charData.TraitOne == TraitType.CantAttack || charData.TraitTwo == TraitType.CantAttack;
		LookCreator.InitializeWeaponLook(charData.EdgeEquipmentType, this, charData.Weapon, hasCantAttackTrait);
		LookCreator.InitializeCapeLook(charData.CapeEquipmentType, this);
		base.UpdateCustomMeshArray();
		base.ApplyOutlineScale();
		if (this.m_playerController)
		{
			this.m_playerController.RecreateRendererArray();
			this.m_playerController.ResetRendererArrayColor();
			this.m_playerController.BlinkPulseEffect.ResetAllBlackFills();
		}
	}

	private void OnEquippedChanged(MonoBehaviour sender, EventArgs args)
	{
		if (this.m_playerController == null)
		{
			if (PlayerManager.IsInstantiated)
			{
				this.m_playerController = PlayerManager.GetPlayerController();
			}
			if (this.m_playerController == null)
			{
				return;
			}
		}
		EquippedChangeEventArgs equippedChangeEventArgs = args as EquippedChangeEventArgs;
		switch (equippedChangeEventArgs.EquipmentCategoryType)
		{
		case EquipmentCategoryType.Weapon:
		{
			AbilityType weaponType = AbilityType.None;
			BaseAbility_RL ability = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false);
			if (ability != null)
			{
				weaponType = ability.AbilityType;
			}
			bool hasCantAttackTrait = SaveManager.PlayerSaveData.HasTrait(TraitType.CantAttack);
			LookCreator.InitializeWeaponLook(equippedChangeEventArgs.EquippedType, this, weaponType, hasCantAttackTrait);
			break;
		}
		case EquipmentCategoryType.Head:
			LookCreator.InitializeHelmetLook(equippedChangeEventArgs.EquippedType, this);
			break;
		case EquipmentCategoryType.Chest:
			LookCreator.InitializeArmorLook(equippedChangeEventArgs.EquippedType, this);
			break;
		case EquipmentCategoryType.Cape:
			LookCreator.InitializeCapeLook(equippedChangeEventArgs.EquippedType, this);
			break;
		}
		base.UpdateCustomMeshArray();
		base.ApplyOutlineScale();
		if (this.m_playerController)
		{
			this.m_playerController.RecreateRendererArray();
			this.m_playerController.ResetRendererArrayColor();
			this.m_playerController.BlinkPulseEffect.ResetAllBlackFills();
		}
	}

	public void SetCritBlinkEffectEnabled(bool enable, CritBlinkEffectTriggerType effectTriggerType)
	{
		if (enable)
		{
			this.m_critEffectTriggerBitMask |= (int)effectTriggerType;
			if (!this.m_critBlinkEnabled)
			{
				if (this.m_critBlinkCoroutine != null)
				{
					base.StopCoroutine(this.m_critBlinkCoroutine);
				}
				this.m_critBlinkCoroutine = null;
				this.m_critBlinkCoroutine = base.StartCoroutine(this.CritBlinkEffectCoroutine());
				this.m_critBlinkEnabled = true;
				return;
			}
		}
		else
		{
			this.m_critEffectTriggerBitMask &= (int)(~(int)effectTriggerType);
			if (this.m_critBlinkEnabled && this.m_critEffectTriggerBitMask == 0)
			{
				if (this.m_critBlinkCoroutine != null)
				{
					base.StopCoroutine(this.m_critBlinkCoroutine);
				}
				this.m_critBlinkCoroutine = null;
				SkinnedMeshRenderer currentWeaponGeo = this.m_playerController.LookController.CurrentWeaponGeo;
				if (currentWeaponGeo)
				{
					currentWeaponGeo.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
					PlayerLookController.m_critMatPropertyBlock.SetColor(ShaderID_RL._AddColor, this.m_storedWeapon1Color);
					currentWeaponGeo.SetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
				}
				SkinnedMeshRenderer secondaryWeaponGeo = this.m_playerController.LookController.SecondaryWeaponGeo;
				if (secondaryWeaponGeo)
				{
					secondaryWeaponGeo.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
					PlayerLookController.m_critMatPropertyBlock.SetColor(ShaderID_RL._AddColor, this.m_storedWeapon2Color);
					secondaryWeaponGeo.SetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
				}
				this.m_critBlinkEnabled = false;
			}
		}
	}

	public void ForceDisableCritBlinkEffect()
	{
		this.m_critEffectTriggerBitMask = 0;
		if (this.m_critBlinkCoroutine != null)
		{
			base.StopCoroutine(this.m_critBlinkCoroutine);
		}
		this.m_critBlinkCoroutine = null;
		if (this.m_critBlinkEnabled)
		{
			SkinnedMeshRenderer currentWeaponGeo = this.m_playerController.LookController.CurrentWeaponGeo;
			if (currentWeaponGeo)
			{
				currentWeaponGeo.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
				PlayerLookController.m_critMatPropertyBlock.SetColor(ShaderID_RL._AddColor, this.m_storedWeapon1Color);
				currentWeaponGeo.SetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
			}
			SkinnedMeshRenderer secondaryWeaponGeo = this.m_playerController.LookController.SecondaryWeaponGeo;
			if (secondaryWeaponGeo)
			{
				secondaryWeaponGeo.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
				PlayerLookController.m_critMatPropertyBlock.SetColor(ShaderID_RL._AddColor, this.m_storedWeapon2Color);
				secondaryWeaponGeo.SetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
			}
			this.m_critBlinkEnabled = false;
		}
	}

		private IEnumerator CritBlinkEffectCoroutine()
	{
		SkinnedMeshRenderer weapon1Renderer = this.m_playerController.LookController.CurrentWeaponGeo;
		if (weapon1Renderer)
		{
			weapon1Renderer.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
			this.m_storedWeapon1Color = PlayerLookController.m_critMatPropertyBlock.GetColor(ShaderID_RL._AddColor);
		}
		SkinnedMeshRenderer weapon2Renderer = this.m_playerController.LookController.SecondaryWeaponGeo;
		if (weapon2Renderer)
		{
			weapon2Renderer.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
			this.m_storedWeapon2Color = PlayerLookController.m_critMatPropertyBlock.GetColor(ShaderID_RL._AddColor);
		}
		float intervalStartTime = Time.time;
		bool reverseTween = false;
		for (;;)
		{
			float num = (Time.time - intervalStartTime) / 0.2f;
			Color value;
			if (!reverseTween)
			{
				value = Color.Lerp(PlayerLookController.m_critStartColor, PlayerLookController.m_critEndColor, num);
			}
			else
			{
				value = Color.Lerp(PlayerLookController.m_critEndColor, PlayerLookController.m_critStartColor, num);
			}
			if (num >= 1f)
			{
				intervalStartTime = Time.time;
				reverseTween = !reverseTween;
			}
			if (weapon1Renderer)
			{
				weapon1Renderer.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
				PlayerLookController.m_critMatPropertyBlock.SetColor(ShaderID_RL._AddColor, value);
				weapon1Renderer.SetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
			}
			if (weapon2Renderer)
			{
				weapon2Renderer.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
				PlayerLookController.m_critMatPropertyBlock.SetColor(ShaderID_RL._AddColor, value);
				weapon2Renderer.SetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
			}
			yield return null;
		}
		yield break;
	}

	[SerializeField]
	private bool m_clampPlayerScale;

	[SerializeField]
	private bool m_useCurrentScaleAsBase;

	[SerializeField]
	private bool m_ignoreScaleTraits;

	[SerializeField]
	private bool m_loadFromSaveFile;

	private Vector3 m_storedBaseScale;

	private PlayerController m_playerController;

	private Action<MonoBehaviour, EventArgs> m_onEquippedChanged;

	private static MaterialPropertyBlock m_critMatPropertyBlock;

	private const float CRIT_PULSE_INTERVAL = 0.2f;

	private static Color m_critStartColor;

	private static Color m_critEndColor;

	private Coroutine m_critBlinkCoroutine;

	private bool m_critBlinkEnabled;

	private Color m_storedWeapon1Color;

	private Color m_storedWeapon2Color;

	private int m_critEffectTriggerBitMask;
}
