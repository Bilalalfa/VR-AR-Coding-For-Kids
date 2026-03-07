using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class XRBlockSpawner : MonoBehaviour
{
    [Header("Pengaturan Dispenser")]
    [Tooltip("Centang jika balok ini adalah 'Master' yang ada di meja (tidak boleh habis)")]
    public bool isDispenser = true;

    private Vector3 startPos;
    private Quaternion startRot;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        // Ambil komponen VR Grab
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Catat posisi dan rotasi awal saat game dimulai
        startPos = transform.position;
        startRot = transform.rotation;

        // Minta Unity menjalankan fungsi OnGrabbed saat balok ini dipegang oleh tangan VR
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Jika balok ini adalah Dispenser/Master, maka lakukan kloning!
        if (isDispenser)
        {
            // 1. Munculkan kembaran baru persis di titik awal balok ini
            GameObject clone = Instantiate(gameObject, startPos, startRot);

            // 2. Pastikan kembarannya yang tertinggal di meja tetap bertugas sebagai Dispenser
            XRBlockSpawner cloneScript = clone.GetComponent<XRBlockSpawner>();
            if (cloneScript != null)
            {
                cloneScript.isDispenser = true;
            }

            // 3. MATIKAN mode dispenser pada balok yang SEDANG KITA PEGANG ini.
            // Ini sangat penting agar saat balok ini dicabut dari Socket nanti, dia tidak beranak lagi.
            this.isDispenser = false;
        }
    }

    void OnDestroy()
    {
        // Bersihkan memori saat objek hancur
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        }
    }
}