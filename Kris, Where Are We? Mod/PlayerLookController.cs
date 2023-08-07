using System;
using System.Collections;
using UnityEngine;

public class PlayerLookController : LookController
{
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
	 	//Color data taken from a screenshot from Deltarune
		if (charData.ClassType == ClassType.SwordClass)
		{
  			//Create Kris' skin color, and aply it to the head mesh
			Color skinColor = new Color(0.4588f, 0.9843f, 0.9294f);
			this.SetMainColor(base.HeadGeo, skinColor);

   			//Create Kris' hair color, and apply it to the hair, helmet, and chest hair meshes
      			//(although I don't know whee the last one is used)
			Color hairColor = new Color(0.0413f, 0.0413f, 0.2313f);
			this.SetMainColor(base.HelmetGeo, hairColor);
			this.SetMainColor(base.HelmetHairGeo, hairColor);
			this.SetMainColor(base.ChestHairGeo, hairColor);

   			//Create red color, and apply it to the eye meshes
   			Color eyeColor = new Color(1f, 0f, 0f, 1f);
			this.SetMainColor(base.LeftEyeGeo, eyeColor);
			this.SetMainColor(base.RightEyeGeo, eyeColor);

      			//Create Kris' cape color, and apply it to the cape mesh
      			Color capeColor = new Color(0.4156f, 0.4823f, 0.7686f);
			this.SetMainColor(base.CapeGeo, capeColor);

   			//Make the character have small limbs
   			base.Animator.SetFloat("LimbType", 1f);
			base.Animator.SetFloat("BoneStructureType", 1f);
		}
		base.GlassesGeo.SetActive(charData.HasGlasses);
	}
}
