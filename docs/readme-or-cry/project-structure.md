# GS-Verse — Структура проекта

## Верхний уровень

```
GS-Verse/
├── package/          # Основной переиспользуемый Unity-пакет (ядро системы)
├── projects/         # Unity-проект с демо-сценами
├── scripts/          # Python-скрипты для подготовки ассетов
└── docs/             # Документация и изображения
```

---

## `/package` — ядро системы

Содержит реализацию Gaussian Splatting как Unity-пакет:

- **Runtime/** — основные C# скрипты рендеринга:
  - `GaussianSplatRenderer.cs` — главный компонент рендеринга
  - `GaussianGaMeSSplatAsset.cs` — ассет для mesh-параметризованных сплатов
  - `GpuSorting.cs` — сортировка сплатов по глубине на GPU
- **Editor/** — инструменты редактора Unity:
  - `GaussianSplatAssetCreator.cs` — диалог создания ассетов из PLY/JSON
  - Инструменты перемещения/вращения/масштабирования
- **Shaders/** — HLSL шейдеры и compute-шейдеры для рендеринга
- **Shared/** — общий код:
  - `SplatMathUtils.cs` — математика для деформаций
  - `IDeformable.cs` — интерфейс деформируемых объектов

---

## `/projects/GaussianExample/Assets` — Unity-проект

```
Assets/
├── Scripts/          # 15 C# скриптов для взаимодействия и физики
├── RoomScenes/       # Сцены (darkRoom, bearRoom)
├── Resources/        # Меши OBJ (скачиваются отдельно с Google Drive)
├── rooms/            # 3D модели (radio и др.)
├── Materials/        # Материалы объектов
├── PlyImporter/      # Сторонняя библиотека импорта PLY
├── ComplexColliders/ # Утилиты коллайдеров
└── Samples/XR/       # Примеры XR Interaction Toolkit
```

### Ключевые скрипты в `Assets/Scripts/`:

| Скрипт | Что делает |
|---|---|
| `GSVerse.cs` | Управляет мешем + физическими коллайдерами |
| `SplatDeformate.cs` | Пружинная физика деформаций (растяжение) |
| `SplatPressDeformate.cs` | Деформация при нажатии |
| `XRTriggerStretch.cs` | Растяжение через VR-контроллер |
| `XRTriggerRotate.cs` | Вращение через контроллер |
| `ForceModeManager.cs` | Управление режимами физики |
| `GaussianSplatAssetUpdater.cs` | Обновление сплатов при изменении меша |

---

## `/scripts` — Python-утилиты

- `import_params.py` — извлекает параметры из обученной GaMeS модели в JSON
- `reexport_mesh.py` — конвертирует меши (нужно для TRELLIS-ассетов)

---

## Как всё связано

```
Python (GaMeS/SuGaR) → mesh.obj + point_cloud.ply + model_params.json
         ↓
Unity Editor → GaussianSplatAssetCreator создаёт ассет
         ↓
GaussianSplatRenderer рендерит сплаты
         ↓
SplatDeformate / XRTrigger* обрабатывают VR-взаимодействие
         ↓
GPU пересчитывает позиции сплатов по изменённому мешу
```
