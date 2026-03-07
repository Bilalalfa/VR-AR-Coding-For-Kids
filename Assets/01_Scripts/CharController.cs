using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [Header("Pengaturan Grid (Tile Size)")]
    public float moveDistance = 1f;
    public float moveDuration = 0.5f;

    [Header("Efek Kemenangan")]
    public ParticleSystem efekMenang;

    [Header("Efek Koin")]
    [Tooltip("Masukkan file suara (.mp3/.wav) ke sini")]
    public AudioClip suaraKoin;
    // (Efek visual koin dihilangkan agar skrip lebih bersih dan stabil)

    private Vector3 posisiAwal;
    private Quaternion rotasiAwal;
    private Rigidbody rb;

    private GameObject koinDiBawahKaki = null;
    public int totalKoinDapat = 0;
    private List<GameObject> koinYangDiambil = new List<GameObject>();

    private bool sudahMenang = false;
    private bool isJatuh = false;

    void Start()
    {
        posisiAwal = transform.position;
        rotasiAwal = transform.rotation;
        rb = GetComponent<Rigidbody>();

        // Paksa matikan efek menang di awal game
        if (efekMenang != null)
        {
            efekMenang.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish")) sudahMenang = true;
        else if (other.CompareTag("Koin")) koinDiBawahKaki = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finish")) sudahMenang = false;
        else if (other.CompareTag("Koin") && other.gameObject == koinDiBawahKaki) koinDiBawahKaki = null;
    }

    public void StartExecution(List<CommandType> program)
    {
        StopAllCoroutines();
        sudahMenang = false;
        isJatuh = false;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        StartCoroutine(ExecuteProgramCoroutine(program));
    }

    private IEnumerator ExecuteProgramCoroutine(List<CommandType> program)
    {
        // 🔴 UPDATE: Menggunakan For-Loop untuk mengecek apakah ini balok terakhir
        for (int i = 0; i < program.Count; i++)
        {
            CommandType cmd = program[i];

            if (isJatuh) break;

            if (cmd == CommandType.Maju)
            {
                yield return StartCoroutine(MoveForward());
                yield return StartCoroutine(CekJurang());
            }
            else if (cmd == CommandType.PutarKanan)
                yield return StartCoroutine(Turn(90f));
            else if (cmd == CommandType.PutarKiri)
                yield return StartCoroutine(Turn(-90f));
            else if (cmd == CommandType.AmbilKoin)
                yield return StartCoroutine(AmbilKoin());

            // Beri jeda 0.2 detik HANYA JIKA ini BUKAN perintah terakhir.
            // Jika ini perintah terakhir, langsung lompat ke evaluasi akhir (instan)!
            if (i < program.Count - 1)
            {
                yield return new WaitForSeconds(0.2f);
            }
        }

        // --- EVALUASI AKHIR (Dieksekusi tanpa delay) ---
        if (!isJatuh)
        {
            if (sudahMenang)
            {
                Debug.Log("🎉 MENANG!");
                if (efekMenang != null)
                {
                    efekMenang.gameObject.SetActive(true);
                    efekMenang.Play();
                }
            }
            else
            {
                Debug.Log("❌ GAGAL: Berhenti sebelum Finish.");
            }
        }
    }

    private IEnumerator CekJurang()
    {
        bool adaLantai = false;
        Vector3 titikLangit = transform.position + Vector3.up * 3f;
        RaycastHit[] semuaTabrakan = Physics.SphereCastAll(titikLangit, 0.3f, Vector3.down, 5f, -1, QueryTriggerInteraction.Collide);

        foreach (RaycastHit hit in semuaTabrakan)
        {
            if (hit.collider.transform.IsChildOf(this.transform) || hit.collider.gameObject == this.gameObject) continue;
            if (hit.collider.CompareTag("Koin")) continue;

            if (hit.collider.CompareTag("Jalan") || hit.collider.CompareTag("Finish"))
            {
                adaLantai = true;
                break;
            }
        }

        if (!adaLantai)
        {
            Debug.LogWarning("⚠️ JATUH!");
            isJatuh = true;
            sudahMenang = false;
            if (rb != null) rb.isKinematic = false;
            yield return new WaitForSeconds(1.5f);
            ResetKarakter();
        }
    }

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
        if (koinDiBawahKaki != null)
        {
            if (suaraKoin != null)
            {
                AudioSource.PlayClipAtPoint(suaraKoin, transform.position);
            }

            koinDiBawahKaki.SetActive(false);
            koinYangDiambil.Add(koinDiBawahKaki);
            koinDiBawahKaki = null;
            totalKoinDapat++;
            Debug.Log($"💰 Koin diambil! Total: {totalKoinDapat}");
        }

        yield return new WaitForSeconds(0.5f);
    }

    public void ResetKarakter()
    {
        StopAllCoroutines();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = posisiAwal;
        transform.rotation = rotasiAwal;
        totalKoinDapat = 0;
        koinDiBawahKaki = null;
        sudahMenang = false;
        isJatuh = false;

        foreach (GameObject koin in koinYangDiambil)
        {
            if (koin != null) koin.SetActive(true);
        }
        koinYangDiambil.Clear();

        if (efekMenang != null)
        {
            efekMenang.Stop();
            efekMenang.gameObject.SetActive(false);
        }

        Debug.Log("🐈 Kucing di-reset!");
    }
}