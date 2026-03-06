using UnityEngine;
using TMPro;

public class LoopUIManager : MonoBehaviour
{
    public static LoopUIManager Instance;

    [Header("Referensi UI")]
    public TextMeshProUGUI angkaText;
    public GameObject panelVisual;

    [Header("Pengaturan Follow VR")]
    [Tooltip("Jarak layar dari wajah pemain (dalam meter)")]
    public float jarakDariWajah = 0.8f;
    [Tooltip("Seberapa cepat layar mengejar wajah pemain. Semakin kecil, semakin mulus/lambat.")]
    public float kecepatanFollow = 5f;

    private CommandBlock currentBlock;
    private Transform kameraVR;
    private bool isMenuAktif = false; // Tombol on/off untuk mesin pengejar

    void Awake()
    {
        if (Instance == null) Instance = this;

        if (panelVisual != null) panelVisual.SetActive(false);

        // Otomatis mencari mata pemain (Kamera VR)
        if (Camera.main != null) kameraVR = Camera.main.transform;
    }

    void Update()
    {
        // Mesin Follow: Hanya berjalan jika layar sedang menyala
        if (isMenuAktif && panelVisual != null && kameraVR != null)
        {
            // 1. Tentukan titik target: Di depan wajah, turun sedikit agar tidak menutupi mata
            Vector3 targetPosisi = kameraVR.position + (kameraVR.forward * jarakDariWajah) - new Vector3(0, 0.2f, 0);

            // 2. Bergerak mulus (Lerp) menuju target
            panelVisual.transform.position = Vector3.Lerp(panelVisual.transform.position, targetPosisi, Time.deltaTime * kecepatanFollow);

            // 3. Berputar mulus (Slerp) agar layarnya selalu menatap balik ke wajah pemain
            // Kita balik arahnya (posisi layar dikurang posisi kamera) agar teksnya tidak terbalik seperti cermin
            Quaternion targetRotasi = Quaternion.LookRotation(panelVisual.transform.position - kameraVR.position);
            panelVisual.transform.rotation = Quaternion.Slerp(panelVisual.transform.rotation, targetRotasi, Time.deltaTime * kecepatanFollow);
        }
    }

    // Fungsi dipanggil saat balok masuk Socket
    public void BukaMenu(CommandBlock block)
    {
        currentBlock = block;
        UpdateLayar();

        if (kameraVR != null)
        {
            // Teleportasi instan ke depan wajah SAAT PERTAMA MUNCUL agar tidak terbang dari jauh
            panelVisual.transform.position = kameraVR.position + (kameraVR.forward * jarakDariWajah) - new Vector3(0, 0.2f, 0);
            panelVisual.transform.rotation = Quaternion.LookRotation(panelVisual.transform.position - kameraVR.position);
        }

        panelVisual.SetActive(true);
        isMenuAktif = true; // Nyalakan mesin Follow!
    }

    // Fungsi untuk tombol OK/SUBMIT
    public void TutupMenu()
    {
        if (panelVisual != null) panelVisual.SetActive(false);
        currentBlock = null;
        isMenuAktif = false; // Matikan mesin Follow untuk menghemat memori
        Debug.Log("✅ Angka Loop Disimpan & Menu Ditutup!");
    }

    public void TambahAngka()
    {
        if (currentBlock != null)
        {
            currentBlock.loopCount++;
            UpdateLayar();
        }
    }

    public void KurangAngka()
    {
        if (currentBlock != null && currentBlock.loopCount > 1)
        {
            currentBlock.loopCount--;
            UpdateLayar();
        }
    }

    private void UpdateLayar()
    {
        if (angkaText != null && currentBlock != null)
        {
            angkaText.text = currentBlock.loopCount.ToString();
        }
    }
}