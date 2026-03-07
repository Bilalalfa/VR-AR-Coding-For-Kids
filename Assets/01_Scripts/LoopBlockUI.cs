using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LoopBlockUI : MonoBehaviour
{
    private CommandBlock commandBlock;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        commandBlock = GetComponent<CommandBlock>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.AddListener(OnMasukSocket);
    }

    void OnDisable()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnMasukSocket);
    }

    private void OnMasukSocket(SelectEnterEventArgs args)
    {
        // Jika balok ini masuk ke Socket Papan Program...
        if (args.interactorObject is XRSocketInteractor)
        {
            // Panggil Manajer UI untuk memunculkan layar di depan pemain!
            if (LoopUIManager.Instance != null)
            {
                LoopUIManager.Instance.BukaMenu(commandBlock);
            }
        }
    }
}