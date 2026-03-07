using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [Header("Pengaturan Grid (Tile Size)")]
    public float moveDistance = 1f;
    public float moveDuration = 0.5f;

    [Header("Efek Visual (Opsional)")]
    [Tooltip("Tarik partikel kembang api/confetti ke sini")]
    public ParticleSystem efekMenang;

    private Vector3 posisiAwal;
    private Quaternion rotasiAwal;

    // Memori untuk mengingat koin mana yang sedang diinjak
    private GameObject koinDiBawahKaki = null;
    public int totalKoinDapat = 0;

    // --- BUKU CATATAN KOIN ---
    // Menyimpan daftar koin yang sudah diambil agar bisa dimunculkan lagi saat Reset
    private List<GameObject> koinYangDiambil = new List<GameObject>();

    void Start()
    {
        // Menyimpan titik awal kucing saat game dimulai
        posisiAwal = transform.position;
        rotasiAwal = transform.rotation;
    }

    // --- SENSOR KAKI KUCING (TRIGGER) ---
    void OnTriggerEnter(Collider other)
    {
        // CCTV: Laporan Kucing menabrak objek (Bisa dihapus jika Console sudah terlalu ramai)
        Debug.Log($"👀 Kucing menyentuh: '{other.gameObject.name}' | Tag: '{other.tag}'");

        // Jika menginjak kotak Finish
        if (other.CompareTag("Finish"))
        {
            Debug.Log("🎉 MENANG! Kucing sampai di Garis Finish!");
            if (efekMenang != null) efekMenang.Play();
        }
        // Jika menginjak Koin
        else if (other.CompareTag("Koin"))
        {
            koinDiBawahKaki = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Jika kucing melangkah pergi meninggalkan koin tanpa mengambilnya
        if (other.CompareTag("Koin") && other.gameObject == koinDiBawahKaki)
        {
            koinDiBawahKaki = null;
        }
    }

    // --- SISTEM EKSEKUSI ---
    public void StartExecution(List<CommandType> program)
    {
        StopAllCoroutines();
        StartCoroutine(ExecuteProgramCoroutine(program));
    }

    private IEnumerator ExecuteProgramCoroutine(List<CommandType> program)
    {
        foreach (CommandType cmd in program)
        {
            if (cmd == CommandType.Maju)
                yield return StartCoroutine(MoveForward());
            else if (cmd == CommandType.PutarKanan)
                yield return StartCoroutine(Turn(90f));
            else if (cmd == CommandType.PutarKiri)
                yield return StartCoroutine(Turn(-90f));
            else if (cmd == CommandType.AmbilKoin)
                yield return StartCoroutine(AmbilKoin());

            yield return new WaitForSeconds(0.2f); // Jeda antar gerakan biar mulus
        }
        Debug.Log("🏁 Kucing Selesai Menjalankan Semua Perintah!");
    }

    // --- LOGIKA AKSI ---
    private IEnumerator MoveForward()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.forward * moveDistance;
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }

    private IEnumerator Turn(float angle)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, angle, 0);
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRot;
    }

    private IEnumerator AmbilKoin()
    {
        // Cek apakah ada koin di bawah kaki saat ini
        if (koinDiBawahKaki != null)
        {
            // 1. Sembunyikan koinnya (JANGAN DI-DESTROY!)
            koinDiBawahKaki.SetActive(false);

            // 2. Catat koin ini ke dalam memori agar bisa di-reset nanti
            koinYangDiambil.Add(koinDiBawahKaki);

            koinDiBawahKaki = null;
            totalKoinDapat++;
            Debug.Log($"💰 Dring! Koin diambil! Total Koin: {totalKoinDapat}");
        }
        else
        {
            Debug.LogWarning("❌ Gagal! Kucing mencoba mengambil koin, tapi tidak ada koin di bawah kakinya.");
        }

        yield return new WaitForSeconds(0.5f); // Animasi jeda waktu ambil koin
    }

    // --- RESET ---
    public void ResetKarakter()
    {
        StopAllCoroutines();

        // Kembalikan Kucing ke posisi awal
        transform.position = posisiAwal;
        transform.rotation = rotasiAwal;
        totalKoinDapat = 0;
        koinDiBawahKaki = null;

        // --- BANGKITKAN KEMBALI SEMUA KOIN YANG SUDAH DIAMBIL ---
        foreach (GameObject koin in koinYangDiambil)
        {
            if (koin != null)
            {
                koin.SetActive(true); // Munculkan kembali koinnya!
            }
        }
        // Bersihkan buku catatan setelah semua koin dikembalikan
        koinYangDiambil.Clear();
        // ---------------------------------------------------------

        // Matikan kembang api jika sedang menyala
        if (efekMenang != null) efekMenang.Stop();

        Debug.Log("🐈 Kucing dikembalikan ke Start, dan semua Koin dimunculkan kembali!");
    }
}