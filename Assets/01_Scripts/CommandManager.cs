using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CommandManager : MonoBehaviour
{
    [Header("Referensi Sistem")]
    [Tooltip("Tarik objek Kucing ke kotak ini")]
    public CharController charController;

    [Header("Daftar Sockets (Urutkan dari Kiri ke Kanan)")]
    public List<XRSocketInteractor> programSockets;

    // --- FUNGSI UTAMA (TOMBOL PLAY) ---
    public void CompileAndExecute()
    {
        List<CommandType> compiledProgram = new List<CommandType>();
        Debug.Log("MENGANALISA PAPAN PROGRAM...");

        int pendingLoops = 1;

        for (int i = 0; i < programSockets.Count; i++)
        {
            XRSocketInteractor socket = programSockets[i];

            if (socket == null) continue; // Pengaman slot kosong

            if (socket.hasSelection)
            {
                // Ambil objek 3D yang nempel di soket
                GameObject attachedObject = socket.interactablesSelected[0].transform.gameObject;
                CommandBlock block = attachedObject.GetComponent<CommandBlock>();

                if (block != null)
                {
                    // Cek apakah ini balok Loop Custom
                    if (block.commandType == CommandType.LoopCustom)
                    {
                        pendingLoops = block.loopCount;
                        Debug.Log($"[LOOP] Ditemukan Loop! Balok selanjutnya digandakan {pendingLoops} kali.");
                    }
                    else
                    {
                        // Masukkan balok perintah sebanyak jumlah loop yang tersimpan
                        for (int loopCount = 0; loopCount < pendingLoops; loopCount++)
                        {
                            compiledProgram.Add(block.commandType);
                        }
                        Debug.Log($"[SUKSES] Membaca blok {block.commandType} ({pendingLoops}x)");

                        // Kembalikan loop ke 1 agar balok berikutnya tidak ikut tergandakan
                        pendingLoops = 1;
                    }
                }
            }
        }

        if (compiledProgram.Count > 0)
        {
            if (charController != null)
            {
                // Eksekusi jalan!
                charController.StartExecution(compiledProgram);
            }
        }
        else
        {
            Debug.LogError("Papan kosong atau hanya berisi balok Loop!");
        }
    }

    // --- FUNGSI RESET (TOMBOL RESET) ---
    public void ResetProgram()
    {
        Debug.Log("MENGHAPUS SEMUA BLOK DARI PAPAN...");

        // 1. Bersihkan Balok di Papan dengan Aman (Bebas Error C++)
        foreach (XRSocketInteractor socket in programSockets)
        {
            if (socket == null) continue;

            if (socket.hasSelection)
            {
                // Ambil data balok yang sedang menempel (Gunakan tipe interface XR yang benar)
                IXRSelectInteractable attachedInteractable = socket.interactablesSelected[0];
                GameObject attachedObject = attachedInteractable.transform.gameObject;

                // TAHAP KRUSIAL: Paksa Socket melepaskan baloknya secara resmi sebelum dihancurkan!
                if (socket.interactionManager != null)
                {
                    socket.interactionManager.SelectCancel((IXRSelectInteractor)socket, attachedInteractable);
                }

                // Setelah terlepas dengan aman, baru boleh dihancurkan
                Destroy(attachedObject);
            }
        }

        // 2. Pulangkan Kucing ke Garis Start
        if (charController != null)
        {
            charController.ResetKarakter();
        }

        Debug.Log("[RESET] Papan bersih tanpa Error, dan Kucing kembali ke Start!");
    }
}