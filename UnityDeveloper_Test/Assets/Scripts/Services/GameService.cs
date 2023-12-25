using Player;
using System;
using System.Collections;
using UI;
using UnityEngine;
using Utilities;

namespace Services
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        private PlayerController playerController;

        [Header("Views")]
        [SerializeField] private PlayerView playerView;
        [SerializeField] private GameUIView gameUIView;

        [Header("Scriptable Objects")]
        [SerializeField] private PlayerModel playerData;

        [Header("Objective Data")]
        [SerializeField] private int totalPointCube = 10;
        [SerializeField] private float totalTime = 120f; // Total time in min
        [SerializeField] private LayerMask groundLayer;
        
        private float currentTime;
        private Coroutine countDown;

        private void Start()
        {
            playerController = new PlayerController(playerView, playerData);
            GetGameUI().TotalPoinCubeToCollectText(totalPointCube);
            countDown = StartCoroutine(GameplayTimer());
        }

        public PlayerController GetPlayerController() => playerController;

        public GameUIView GetGameUI() => gameUIView;

        public IEnumerator GameplayTimer()
        {
            currentTime = totalTime;
            while (currentTime >= 0)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
                string timeString = timeSpan.ToString(@"mm\:ss");
                GetGameUI().UpdateObjectiveCompletionTimerText("Remaining Time : " + timeString);

                currentTime--;
                yield return new WaitForSeconds(1);

            }
            GameOver();
        }

        private void GameOver()
        {
            if (GetPlayerController().TotalPointCubeCollected != totalPointCube ||  GetPlayerController().PlayerDeath(playerView.transform, groundLayer))
                GetGameUI().GameOver();
        }
    }
}