using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Wajib ditambahkan untuk memanggil sistem VR

public class PenghapusBalok : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        // Mengambil komponen VR dari balok ini
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Memasang telinga pendengar: "Kabari aku kalau balok ini dilepas dari genggaman!"
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.AddListener(CekSetelahDilepas);
        }
    }

    private void CekSetelahDilepas(SelectExitEventArgs args)
    {
        // Jika yang melepaskan balok ini adalah Soket (misal: balok diambil dari soket), biarkan saja.
        if (args.interactorObject is XRSocketInteractor) return;

        // Beri jeda 0.1 detik. 
        // Kenapa? Karena saat tangan melepas, soket butuh waktu sepersekian detik untuk 'menangkap' balok.
        Invoke("HancurkanJikaTidakDiSoket", 0.1f);
    }

    private void HancurkanJikaTidakDiSoket()
    {
        // Setelah 0.1 detik, cek lagi: 
        // Apakah balok ini SEKARANG sedang dipegang/ditangkap oleh sesuatu (termasuk soket)?
        if (!grabInteractable.isSelected)
        {
            Debug.Log("🗑️ Balok dibuang karena tidak masuk soket!");

            // Hancurkan balok ini dari dunia
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Bersihkan memori saat balok hancur agar tidak error
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.RemoveListener(CekSetelahDilepas);
        }
    }
}