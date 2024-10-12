// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sora.UI
{
    public class OverheatingUI : Managers.Singleton<OverheatingUI>
    {
        [SerializeField] private Image barFill;
        [SerializeField] private GameObject overHeatedText;
        [SerializeField] private GameObject rangedMouseUI;
        [SerializeField] private GameObject notOverHeatedUI;

        private Gameplay.Overheating overHeat;
        private Gameplay.Player.TopDownPlayerController player;


        private void Start()
        {
            overHeat = Gameplay.Overheating.instance;
        }

        private void Update()
        {
            barFill.fillAmount = overHeat.GetOverheatingPercentage();

            if (barFill.fillAmount >= 1.0f)
            {
                overHeatedText.SetActive(true);
                rangedMouseUI.SetActive(true);
            }
            else
            {
                rangedMouseUI.SetActive(false);
                overHeatedText.SetActive(false);
            }
        }

        public void ShowUnderheatedError()
        {
            notOverHeatedUI.SetActive(true);
            StartCoroutine(DisableUnderheatedUI());
        }

        private IEnumerator DisableUnderheatedUI()
        {
            yield return new WaitForSecondsRealtime(2.0f);
            notOverHeatedUI.SetActive(false);
        }
    }
}