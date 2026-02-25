# VR-AR-Coding-For-Kids
# 🐍 PyKids: VR/AR Programming for Kids

Proyek VR/AR untuk mengenalkan konsep pemrograman Python kepada anak-anak (8-15 tahun) menggunakan Unity.

## 🚀 Fitur Utama
- **Learning Hub:** Materi Video, PPT, dan Infografis di setiap unit.
- **Interactive Coding:** Game 1-level per unit dengan sistem gerak maju, kanan, kiri, looping.
- **AI Hint:** Chatbot yang memberikan petunjuk jika pemain kesulitan.

## 👥 Anggota Tim
1. **Asyraf (Lead Unity):** Sistem logika & integrasi.
2. **Dhafin (UI/UX & AI):** Interface & fitur AI Hint.
3. **Bilal (Asset Artist):** Environment 3D & model robot.
4. **Khofif (Content Manager):** Penyusun kurikulum & aset media.

## 📅 Target Milestone
- **6 Maret:** Kumpul progres modul mandiri & pembagian tugas presentasi.
- **21 Maret:** Final Project Selesai (Fully Integrated).

## 🛠️ Tech Stack
- Unity 2022.3 LTS (URP)
- XR Interaction Toolkit
- GitHub + Git LFS

## Struktur Proyek
/PyKids-VR-AR/
├── .gitignore              <-- Template Unity untuk mengabaikan file sampah
├── README.md               <-- Dokumentasi dan panduan proyek
├── ProjectSettings/        <-- Pengaturan internal Unity (jangan diedit manual)
├── Packages/               <-- Daftar dependensi dan plugin Unity
└── Assets/                 <-- Ruang kerja utama tim
    ├── 01_Scenes/          <-- Scene utama & Scene eksperimen masing-masing
    ├── 02_Scripts/         <-- Asyraf: Logika C# & Sistem Robot
    ├── 03_UI_Interface/    <-- Dhafin: Sprite, Font, Canvas, & Modul AI Hint
    ├── 04_3D_Models/       <-- Bilal: File FBX, Tekstur, & Prefab Lingkungan
    ├── 05_Media_Content/   <-- Khofif: Video, PPT (PNG), & Infografis
    ├── 06_Animations/      <-- Animasi untuk robot dan transisi UI
    └── 07_Plugins/         <-- SDK: XR Interaction Toolkit & AI SDK
