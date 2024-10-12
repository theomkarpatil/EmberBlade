// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora.Managers
{
    [System.Serializable]
    public class Enemy
    {
        public GameObject prefab;
        public int count;
        public float spawnDelay;
    }

    [System.Serializable]
    public class Wave
    {
        public Enemy[] enemiesToSpawn;
    }

    public class EnemySpawner : Singleton<EnemySpawner>
    {
        [SerializeField] private Wave[] waves;
        
        [Space]
        [Space]
        [Space]
        [SerializeField] private float spawnRadius;
        [SerializeField] private float delayBetweenEachSpawn;
        [HideInInspector] public int totalEnemies;
        [HideInInspector] public int currentEnemies;

        [Space]
        [SerializeField] private Events.SoraEvent pauseGame;

        [HideInInspector] public int waveIndex;
        private bool awaitSpawning;
        private Gameplay.Player.TopDownPlayerController player;

        private Vector2[] spawnPoints =  { new Vector2(-15, 10), new Vector2(15, 10), new Vector2(-15, 10), new Vector2(15, -10), new Vector2(-8, -8), new Vector2(6, 4), new Vector2(10, 2), new Vector2(20, -15)};
        private int spawnIndex;

        private void OnEnable()
        {
            waveIndex = 0;
            spawnIndex = 0;
            player = FindObjectOfType<Gameplay.Player.TopDownPlayerController>();
        }

        private void Update()
        {
            if(GameManager.instance.gameState == EGameState.RUNNING && !awaitSpawning)
            {
                foreach (Enemy enemy in waves[waveIndex].enemiesToSpawn)
                {
                    totalEnemies += enemy.count;
                }
                currentEnemies = totalEnemies;

                awaitSpawning = true;
                StartCoroutine(Spawn());
            }

            if(awaitSpawning && currentEnemies <= 0)
            {
                Time.timeScale = 0.0f;
                GameManager.instance.gameState = EGameState.FACET_SELECTION;
                
                if (waveIndex == waves.Length - 1)
                {
                    GameManager.instance.OnGameOver();
                    return;
                }

                UIManager.instance.ShowWaveUI();
                AudioManager.instance.OnWaveCompletion();
                Cursor.visible = true;
            }
        }

        private IEnumerator Spawn()
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            spawnIndex = Random.Range(0, spawnPoints.Length);

            foreach (Enemy enemy in waves[waveIndex].enemiesToSpawn)
            {
                Enemy _enemy = enemy;

                // the delay after which a particular enemy type is spawned
                yield return new WaitForSeconds(_enemy.spawnDelay);
                for(int i = 0; i < _enemy.count; ++i)
                {
                    Vector3 _pos;
                    _pos.x = spawnPoints[spawnIndex % spawnPoints.Length].x;
                    _pos.y = _enemy.prefab.transform.position.y;
                    _pos.z = spawnPoints[spawnIndex % spawnPoints.Length].y;
                    //_go.transform.position = _pos;

                    GameObject _go = Instantiate(_enemy.prefab, _pos, Quaternion.identity);
                    spawnIndex++;

                    // the delay between each enemy's spawn
                    yield return new WaitForSeconds(delayBetweenEachSpawn);
                }
            }
        }

        public void StartNextWave()
        {
            waveIndex++;
            awaitSpawning = false;
            totalEnemies = 0;
            GameManager.instance.gameState = EGameState.RUNNING;

            Cursor.visible = false;
            Time.timeScale = 1.0f;
            TimeKeeper.instance.UpdateCurrentTime();

            ScoreManager.instance.UpdateScoreOnWaveCompletion();
            player.health = player.initialHealth;

            Gameplay.Overheating.instance.InitializeOverheating();
            AudioManager.instance.OnGameStart();
        }
    }
}