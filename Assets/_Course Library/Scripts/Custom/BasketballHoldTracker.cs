using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Tracks when an XRDirectInteractor hand is holding an object with the tag "basketball."
/// </summary>
public class BasketballHoldTracker : MonoBehaviour
{
    private XRDirectInteractor interactor;
    private bool isHoldingBasketball = false;

    void Start()
    {
        interactor = GetComponent<XRDirectInteractor>();
        if (interactor == null)
        {
            Debug.LogError("XRDirectInteractor component is missing from this GameObject.");
            return;
        }

        interactor.selectEntered.AddListener(OnSelectEntered);
        interactor.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject != null && args.interactableObject.transform.CompareTag("Basketball"))
        {
            isHoldingBasketball = true;
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactableObject != null && args.interactableObject.transform.CompareTag("Basketball"))
        {
            isHoldingBasketball = false;
        }
    }

    public bool IsHoldingBasketball()
    {
        return isHoldingBasketball;
    }
}
