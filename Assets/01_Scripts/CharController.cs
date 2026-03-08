using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [Header("Pengaturan Grid & Gerakan")]
    public float moveDistance = 1f;
    public float moveDuration = 0.5f;

    [Tooltip("Atur seberapa tinggi efek pantulan kucing saat melangkah maju")]
    public float tinggiLompatan = 1.0f; // Bisa Anda kecilkan jadi 0.5 jika dirasa terlalu tinggi

    [Header("Efek Visual & Suara")]
    public ParticleSystem efekMenang;
    public AudioClip suaraKoin;
    public GameObject efekAmbilKoinPrefab;

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
        for (int i = 0; i < program.Count; i++)
        {
            CommandType cmd = program[i];

            if (isJatuh) break;

            switch (cmd)
            {
                case CommandType.Maju:
                    yield return StartCoroutine(MoveForward());
                    yield return StartCoroutine(CekJurang());
                    break;

                case CommandType.PutarKanan:
                    yield return StartCoroutine(Turn(90f));
                    break;

                case CommandType.PutarKiri:
                    yield return StartCoroutine(Turn(-90f));
                    break;

                case CommandType.AmbilKoin:
                    yield return StartCoroutine(AmbilKoin());
                    break;

                    // (Perintah Lompat khusus sudah dihapus sesuai permintaan)
            }

            // Jeda antar perintah (kecuali balok terakhir)
            if (i < program.Count - 1)
            {
                yield return new WaitForSeconds(0.2f);
            }
        }

        // --- EVALUASI AKHIR ---
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

    // --- SENSOR JURANG ---
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

    // --- GERAKAN MAJU (SEKARANG DENGAN EFEK LOMPAT / BOUNCE) ---
    private IEnumerator MoveForward()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.forward * moveDistance;
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            // Hitung persentase waktu dari 0.0 ke 1.0
            float persentase = elapsedTime / moveDuration;

            // Hitung posisi mendatar (maju lurus)
            Vector3 posisiSaatIni = Vector3.Lerp(startPos, endPos, persentase);

            // 🔴 SUNTIKAN EFEK LOMPAT: Tambahkan ketinggian melengkung (parabola) dengan rumus Sinus
            posisiSaatIni.y += Mathf.Sin(persentase * Mathf.PI) * tinggiLompatan;

            // Terapkan posisi ke tubuh Kucing
            transform.position = posisiSaatIni;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Pastikan mendarat persis di titik akhir
        transform.position = endPos;
    }

    // --- GERAKAN PUTAR ---
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

    // --- AMBIL KOIN ---
    private IEnumerator AmbilKoin()
    {
        if (koinDiBawahKaki != null)
        {
            if (suaraKoin != null)
            {
                AudioSource.PlayClipAtPoint(suaraKoin, transform.position);
            }

            if (efekAmbilKoinPrefab != null)
            {
                GameObject efekLedakan = Instantiate(efekAmbilKoinPrefab, koinDiBawahKaki.transform.position, Quaternion.identity);
                efekLedakan.SetActive(true);
                Destroy(efekLedakan, 2f);
            }

            koinDiBawahKaki.SetActive(false);
            koinYangDiambil.Add(koinDiBawahKaki);
            koinDiBawahKaki = null;
            totalKoinDapat++;
            Debug.Log($"💰 Koin diambil! Total: {totalKoinDapat}");
        }

        yield return new WaitForSeconds(0.5f);
    }

    // --- RESET GAME ---
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