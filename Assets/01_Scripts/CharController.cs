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

    void Start()
    {
        posisiAwal = transform.position;
        rotasiAwal = transform.rotation;
    }

    // --- SENSOR KAKI KUCING (TRIGGER) ---
    void OnTriggerEnter(Collider other)
    {
        // Jika menginjak kotak Finish
        if (other.CompareTag("Finish"))
        {
            Debug.Log("🎉 MENANG! Kucing sampai di Garis Finish!");
            if (efekMenang != null) efekMenang.Play(); // Ledakkan confetti!
        }
        // Jika menginjak Koin
        else if (other.CompareTag("Coin"))
        {
            koinDiBawahKaki = other.gameObject; // Ingat koin ini!
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Jika kucing melangkah pergi meninggalkan koin
        if (other.CompareTag("Koin") && other.gameObject == koinDiBawahKaki)
        {
            koinDiBawahKaki = null; // Lupakan koinnya
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
                yield return StartCoroutine(AmbilKoin()); // Eksekusi aksi ambil koin

            yield return new WaitForSeconds(0.2f);
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
            Destroy(koinDiBawahKaki); // Hilangkan koinnya dari jalan
            koinDiBawahKaki = null;
            totalKoinDapat++;
            Debug.Log($"💰 Dring! Koin diambil! Total Koin: {totalKoinDapat}");
        }
        else
        {
            Debug.LogWarning("❌ Gagal! Kucing mencoba mengambil koin, tapi tidak ada koin di tempat ini.");
        }

        // Beri jeda sedikit agar aksi mengambil terlihat
        yield return new WaitForSeconds(0.5f);
    }

    // --- RESET ---
    public void ResetKarakter()
    {
        StopAllCoroutines();
        transform.position = posisiAwal;
        transform.rotation = rotasiAwal;
        totalKoinDapat = 0; // Reset koin saat ulang level
        koinDiBawahKaki = null;

        if (efekMenang != null) efekMenang.Stop();

        Debug.Log("🐈 Kucing dikembalikan ke garis Start!");
    }
}