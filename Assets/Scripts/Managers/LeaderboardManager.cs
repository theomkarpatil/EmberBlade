// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using LootLocker.Requests;

namespace Sora.Managers
{    
    public class LeaderboardManager : Singleton<LeaderboardManager>
    {
        [SerializeField] private TMP_Text[] names;
        [SerializeField] private TMP_Text[] scores;

        [SerializeField] private TMP_Text currRank;
        [SerializeField] private TMP_Text currName;
        [SerializeField] private TMP_Text currScore;

        [Space]
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private GameObject leaderboardUI;

        [Header("Lootlocker Variables")]
        [SerializeField] private string leaderboardID;

        private void OnEnable()
        {
            StartCoroutine(LoginRoutine());
        }

        private void Start()
        {
            for(int i = 0; i < 3; i++)
            {
                names[i].text = PlayerPrefs.GetString("n" + i, "Player1");
                scores[i].text = PlayerPrefs.GetFloat("s" + i, 0.0f).ToString();
            }
        }

        public void ShowScore()
        {
            if (nameField.text.Length >= 1)
                StartCoroutine(UpdateScores());
        }

        private IEnumerator UpdateScores()
        {
            int pScore = (int)GameManager.instance.GetFinalScore();

            yield return SubmitScoreRoutine(pScore);
            yield return FetchLeaderboard();            
        }

        private IEnumerator LoginRoutine()
        {
            bool complete = false;
            LootLockerSDKManager.StartGuestSession((response) =>
            {
                if (response.success)
                {
                    Debug.Log("LootLocker:: Player login successful");
                    complete = true;
                }
                else
                {
                    complete = true;
                    Debug.Log("LootLocker:: Player failed to start session. Login unsuccessful.");
                }

            });

            yield return new WaitWhile(() => complete == false);
        }

        private IEnumerator SubmitScoreRoutine(int score)
        {
            bool complete = false;
            string playerID = nameField.text;

            LootLockerSDKManager.SubmitScore(playerID, score, leaderboardID, (response) =>
            {
                if (response.success)
                {
                    Debug.Log("LootLocker:: Score submitted for player " + nameField.text);

                    currName.text = playerID;
                    currRank.text = response.rank.ToString();
                    currScore.text = score.ToString();
                }
                else
                    Debug.Log("LootLocker:: Failed to submit score for player " + nameField.text);
                
                    complete = true;
            });

            yield return new WaitWhile(() => complete == false);
        }

        private IEnumerator FetchLeaderboard()
        {
            bool complete = false;
            LootLockerSDKManager.GetScoreList(leaderboardID, 3, (response) =>
            {
                if (response.success)
                {
                    LootLockerLeaderboardMember[] results = response.items;

                    for (int i = 0; i < 3; ++i)
                    {
                        names[i].text = results[i].member_id;
                        scores[i].text = results[i].score.ToString();
                    }

                    leaderboardUI.SetActive(true);
                }
                else
                    Debug.Log("Lootlocker:: Failed to fetch scores");
            });

            yield return new WaitWhile(() => complete == true);
        }
    }
}