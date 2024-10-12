// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Sora.Gameplay
{
    public enum EFacetType
    {
        DAMAGE_MELEE,
        DAMAGE_RANGED,
        ALL_DAMAGE,
        BURN_DAMAGE,
        BURN_DURATION,
        MOVEMENT_SPEED,
        DASH_COOLDOWN,
        PLAYER_HEALTH,
        COUNT,
        NULL
    }

    public class Facet : MonoBehaviour
    {
        [HideInInspector] public EFacetType facetType;
        [HideInInspector] public float facetValue;
        public TMP_Text facetDescription;
        public TMP_Text facetValueText;
                
        public void OnFacetSelection()
        {
            switch (facetType)
            {
                case EFacetType.DAMAGE_MELEE:
                    {
                        DamageValues.instance.playerMeleeDamage *= facetValue;
                    }
                    break;
                case EFacetType.DAMAGE_RANGED:
                    {
                        DamageValues.instance.playerRangedDamage *= facetValue;
                    }
                    break;
                case EFacetType.ALL_DAMAGE:
                    {
                        DamageValues.instance.playerRangedDamage *= facetValue;
                        DamageValues.instance.playerMeleeDamage *= facetValue;
                    }
                    break;
                case EFacetType.BURN_DAMAGE:
                    {
                        DamageValues.instance.burnDamage *= facetValue;
                    }
                    break;
                case EFacetType.BURN_DURATION:
                    {
                        DamageValues.instance.burnDuration += facetValue;
                    }
                    break;
                case EFacetType.MOVEMENT_SPEED:
                    {
                        FindObjectOfType<Player.TopDownPlayerController>().ModifyMovementSpeed(facetValue);
                    }
                    break;
                case EFacetType.DASH_COOLDOWN:
                    {
                        FindObjectOfType<Player.TopDownPlayerController>().ModifyDashCooldown(facetValue);
                    }
                    break;
                case EFacetType.PLAYER_HEALTH:
                    {
                        FindObjectOfType<Player.TopDownPlayerController>().UpdateMaxHealth(facetValue);
                    }
                    break;
            }
        }
    }
}