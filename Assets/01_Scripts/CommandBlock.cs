using UnityEngine;

public enum CommandType
{
    Maju,
    PutarKanan,
    PutarKiri,
    AmbilKoin,
    LoopCustom // Tambahkan ini sebagai pengganti Loop2x/3x
}

public class CommandBlock : MonoBehaviour
{
    public CommandType commandType;

    [Header("Khusus Balok Loop")]
    [Tooltip("Jumlah perulangan (hanya dipakai jika tipe balok adalah LoopCustom)")]
    public int loopCount = 2; // Default nilainya 2
}