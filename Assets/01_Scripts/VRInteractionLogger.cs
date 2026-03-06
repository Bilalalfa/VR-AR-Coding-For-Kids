using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRInteractionLogger : MonoBehaviour
{
    // Menggunakan XRBaseInteractable agar fleksibel untuk Grab maupun Simple Interactable
    private XRBaseInteractable interactable;

    void Awake()
    {
        // Mencari komponen interactable apa saja yang menempel di objek ini
        interactable = GetComponent<XRBaseInteractable>();

        if (interactable == null)
        {
            Debug.LogWarning($"[Logger] Awas! Objek {gameObject.name} tidak punya komponen interaksi VR.");
        }
    }

    void OnEnable()
    {
        // Mendaftarkan fungsi log saat skrip aktif
        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(OnHoverEnter);
            interactable.hoverExited.AddListener(OnHoverExit);
            interactable.selectEntered.AddListener(OnSelectEnter);
            interactable.selectExited.AddListener(OnSelectExit);
        }
    }

    void OnDisable()
    {
        // Membersihkan pendaftaran saat skrip mati agar tidak membebani memori (mencegah error)
        if (interactable != null)
        {
            interactable.hoverEntered.RemoveListener(OnHoverEnter);
            interactable.hoverExited.RemoveListener(OnHoverExit);
            interactable.selectEntered.RemoveListener(OnSelectEnter);
            interactable.selectExited.RemoveListener(OnSelectExit);
        }
    }

    // --- FUNGSI PENCATATAN (CCTV) ---

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        Debug.Log($"[LOG VR] 🖐️ Tangan menyentuh/menyorot: {gameObject.name}");
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        Debug.Log($"[LOG VR] 💨 Tangan menjauh dari: {gameObject.name}");
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        Debug.Log($"[LOG VR] 🔘 Tombol/Objek DITEKAN (Select): {gameObject.name}");
    }

    private void OnSelectExit(SelectExitEventArgs args)
    {
        Debug.Log($"[LOG VR] 🔼 Tekanan dilepas dari: {gameObject.name}");
    }
}