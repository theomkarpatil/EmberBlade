// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sora.Managers;
using Sora.Gameplay;

namespace Sora.Managers
{
    public class FacetManager : Singleton<FacetManager>
    {
        [SerializeField] private Facet facetLeft;
        [SerializeField] private Facet facetRight;

        private EFacetType previousLeft = EFacetType.NULL;
        private EFacetType previousRight = EFacetType.NULL;

        public void RequestingFacets()
        {
            facetLeft.facetType = (EFacetType)Random.Range(0, (int)EFacetType.COUNT);
            while (facetLeft.facetType == previousLeft || facetLeft.facetType == previousRight)
                facetLeft.facetType = (EFacetType)Random.Range(0, (int)EFacetType.COUNT);

            facetRight.facetType = (EFacetType)Random.Range(0, (int)EFacetType.COUNT);
            while (facetLeft.facetType == facetRight.facetType || facetRight.facetType == previousLeft || facetRight.facetType == previousRight)
                facetRight.facetType = (EFacetType)Random.Range(0, (int)EFacetType.COUNT);

            previousLeft = facetLeft.facetType;
            previousRight = facetRight.facetType;

            AssignFacetProperties(ref facetLeft);
            AssignFacetProperties(ref facetRight);
        }

        private void AssignFacetProperties(ref Facet facet)
        {
            switch (facet.facetType)
            {
                case EFacetType.DAMAGE_MELEE:
                    {
                        facet.facetDescription.text = "Increase\n Melee Damage";
                        facet.facetValueText.text = "+20%";
                        facet.facetValue = 1.2f;
                    }
                    break;
                case EFacetType.DAMAGE_RANGED:
                    {
                        facet.facetDescription.text = "Increase\n Ranged Damage";
                        facet.facetValueText.text = "+20%";
                        facet.facetValue = 1.2f;
                    }
                    break;
                case EFacetType.ALL_DAMAGE:
                    {
                        facet.facetDescription.text = "Increase\n Outgoing Damage";
                        facet.facetValueText.text = "+12%";
                        facet.facetValue = 1.12f;
                    }
                    break;
                case EFacetType.BURN_DAMAGE:
                    {
                        facet.facetDescription.text = "Increase\n Burn Damage";
                        facet.facetValueText.text = "+40%";
                        facet.facetValue = 1.4f;
                    }
                    break;
                case EFacetType.BURN_DURATION:
                    {
                        facet.facetDescription.text = "Increase\n Burn Duration";
                        facet.facetValueText.text = "+1 sec";
                        facet.facetValue = 1;
                    }
                    break;
                case EFacetType.MOVEMENT_SPEED:
                    {
                        facet.facetDescription.text = "Increase\n Movement Speed";
                        facet.facetValueText.text = "+15%";
                        facet.facetValue = 1.15f;
                    }
                    break;
                case EFacetType.DASH_COOLDOWN:
                    {
                        facet.facetDescription.text = "Reduce\n Dash Cooldown";
                        facet.facetValueText.text = "-0.5 sec";
                        facet.facetValue = -0.5f;
                    }
                    break;
                case EFacetType.PLAYER_HEALTH:
                    {
                        facet.facetDescription.text = "Increase\n Max HP";
                        facet.facetValueText.text = "+20%";
                        facet.facetValue = 1.2f;
                    }
                    break;
            }
        }
    }
}