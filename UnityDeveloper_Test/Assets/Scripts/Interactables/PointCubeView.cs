using Services;
using UnityEngine;

namespace Interactables
{
    public class PointCubeView : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            int currentPoint = GameService.Instance.GetPlayerController().TotalPointCubeCollected;
            EventService.Instance.OnPoinCubeColletedEvent.InvokeEvent(++currentPoint);
            gameObject.SetActive(false);
        }
    }
}