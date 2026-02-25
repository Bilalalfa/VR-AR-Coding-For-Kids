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
├── .gitignore              <-- Template standar Unity untuk mengabaikan file sampah (Library/Temp).
├── README.md               <-- Dokumentasi utama proyek.
├── ProjectSettings/        <-- Pengaturan internal Unity (jangan diedit manual).
├── Packages/               <-- Daftar dependensi dan plugin Unity.
└── Assets/                 <-- Ruang kerja utama tim:
    ├── 01_Scenes/          <-- Scene utama permainan dan scene eksperimen.
    ├── 02_Scripts/         <-- Asyraf: Arsitektur kode C#, logika drag-and-drop, dan sistem robot.
    ├── 03_UI_Interface/    <-- Dhafin: Sprite, Font, Canvas menu, dan modul AI Hint.
    ├── 04_3D_Models/       <-- Bilal: File FBX, tekstur, dan Prefab lingkungan laboratorium.
    ├── 05_Media_Content/   <-- Khofif: Aset materi (Video tutorial, Slide PPT, dan Infografis).
    ├── 06_Animations/      <-- Klip animasi untuk robot dan transisi antarmuka.
    └── 07_Plugins/         <-- SDK eksternal (XR Interaction Toolkit & AI SDK).
