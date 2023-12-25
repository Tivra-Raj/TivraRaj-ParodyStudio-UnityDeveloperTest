using TMPro;
using UnityEngine;

namespace UI
{
    public class GameUIView : MonoBehaviour
    {
        [Header("Game Hud")]
        [SerializeField] private TextMeshProUGUI totalPointCubeToCollectText;
        [SerializeField] private TextMeshProUGUI toatalPointCubeCollectedText;

        [Header("Game Over Parameters")]
        [SerializeField] private GameObject gameoverGameobject;
        public static bool GameIsOver = false;

        [Header("Delivery Timer")]
        [SerializeField] private TextMeshProUGUI ObjectiveCompletionTimeText;

        private void Start()
        {
            SetGameOverUIActive(false);
        }

        public void TotalPoinCubeToCollectText(int value)
        {
            totalPointCubeToCollectText.SetText("Total PointCube = " + value.ToString());
        }

        public void UpdateTotalPointCubeCollectedText(int pointCubeCollected)
        {
            toatalPointCubeCollectedText.SetText("PointCube Collected " + pointCubeCollected.ToString());
        }

        public void UpdateObjectiveCompletionTimerText(string text)
        {
            ObjectiveCompletionTimeText.SetText(text);
        }

        public void GameOver()
        {
            SetGameOverUIActive(true);
            GameIsOver = true;
        }

        public void SetGameOverUIActive(bool value)
        {
            gameoverGameobject.SetActive(value);
        }
    }
}