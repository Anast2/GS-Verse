# Пайплайн: картинка из интернета -> Gaussian Splat в Unity

## Обзор

```
Картинка -> TRELLIS (AI) -> .glb -> Blender (BlenderNeRF) -> dataset.zip
                                  -> .glb/.obj (mesh)
                                                    |
                                        Google Colab (GaMeS) 
                                                    |
                                     point_cloud.ply + model_params.json + mesh_final.obj
                                                    |
                                              Unity (импорт)
```

---

## Шаг 1: Найти картинку объекта

Находим в интернете изображение нужного объекта (например, ключ). Лучше всего работают:
- Фото на белом/однотонном фоне
- Четкое изображение без лишних предметов
- Один объект в кадре

## Шаг 2: Сгенерировать 3D модель в TRELLIS

1. Открыть https://huggingface.co/spaces/trellis-community/TRELLIS
2. Загрузить картинку
3. Нажать **Generate** — AI создаст 3D модель из одной фотографии
4. Скачать результат как **`.glb`** файл (например `key.glb`)

**Что такое TRELLIS:** нейросеть, которая по одному изображению генерирует 3D mesh с текстурой.

## Шаг 3: Создать датасет в Blender (BlenderNeRF)

1. Импортировать `.glb` в Blender
2. Используя аддон **BlenderNeRF**, отрендерить мультиракурсные изображения
3. Экспортировать датасет — получишь zip с:
   - `train/` — папка с PNG изображениями (рендеры с разных ракурсов)
   - `transforms_train.json` — позиции камер
   - `transforms_test.json` — позиции тестовых камер

**Важно:** BlenderNeRF на Windows может создавать пути с обратными слешами (`train\0001.png`). Ноутбук автоматически исправляет это.

## Шаг 4: Обучить GaMeS в Google Colab

Используем ноутбук `notebooks/gsverse_asset_pipeline.ipynb`.

**Требования:** Google Colab с GPU (T4 или лучше). GaMeS использует CUDA — без NVIDIA GPU не работает.

### Что делает ноутбук (по шагам):

#### Step 1 — Проверка GPU
Убеждаемся что Colab дал нам GPU. Если нет — `Runtime > Change runtime type > T4 GPU`.

#### Step 2a — Загрузка датасета
Загружаем zip из BlenderNeRF. Ноутбук автоматически:
- Исправляет обратные слеши в путях (`\` -> `/`)
- Убирает расширение `.png` из путей (GaMeS добавляет его сам)
- Добавляет префикс `./` (GaMeS ожидает `./train/0001`, а не `train/0001` — код делает `file_path[2:]` чтобы убрать `./`)

**Если `transforms_test.json` ссылается на изображения которых нет** (например, test-сет не рендерился отдельно), нужно скопировать `transforms_train.json` как `transforms_test.json` и заменить пути `./test/` на `./train/`.

#### Step 2b — Загрузка mesh
Загружаем `.glb` или `.obj` файл. Ноутбук:
- Конвертирует `.glb` -> `.obj` через trimesh (если нужно)
- Кладет mesh в **корень** датасета как `mesh.obj`
- **Не создает** папку `sparse/0/` — иначе GaMeS думает что это Colmap-формат и падает

#### Step 3 — Установка GaMeS
Клонирует репозиторий gaussian-mesh-splatting и собирает CUDA-расширения.

**Известные проблемы и их решения:**

1. **`simple_knn.cu: FLT_MAX is undefined`** — в CUDA 12+ убрали автоматический include. Ноутбук патчит файл, добавляя `#include <cfloat>`.

2. **`No module named 'smplx'`** — GaMeS импортирует FLAME модуль который зависит от smplx. Ноутбук устанавливает его в зависимостях.

3. **`nvcc not found`** — в Colab nvcc может быть не на PATH. Ноутбук автоматически находит CUDA в `/usr/local/` и настраивает `CUDA_HOME`.

4. **CUDA-расширения не собираются** — попробовать `Runtime > Restart runtime` и перезапустить Step 3.

#### Step 4 — Обучение GaMeS
Запускает `train.py` с параметрами:
- `--gs_type gs_mesh` — режим Gaussian Mesh Splatting
- `--num_splats 5` — 5 сплатов на грань меша
- `--eval` — включает evaluation
- `-w` — включает запись метрик

Обучение занимает ~15-20 минут на T4 GPU. Результат — PSNR ~48 (хорошее качество).

#### Step 5 — Экспорт для Unity
Извлекает из обученной модели:
- `point_cloud.ply` — облако точек (Gaussian Splats)
- `model_params.json` — параметры (_alpha, _scale)
- `mesh_final.obj` — меш объекта

#### Step 6 — Скачивание
Упаковывает все в `unity_asset_key.zip` и скачивает.

---

## Шаг 5: Импорт в Unity

### 5.1 — Разложить файлы
1. Распаковать `unity_asset_key.zip`
2. `mesh_final.obj` -> `Assets/Resources/key.obj` (переименовать!)
3. `point_cloud.ply` и `model_params.json` -> `Assets/RoomScenes/darkRoom/key/`

### 5.2 — Создать GaussianSplat Asset
1. В Unity: **Tools > Gaussian Splats > Create GaussianSplatAsset**
2. Настройки:
   - **Processing Mode** = **GaMeS**
   - **L-Handed Coordinate System** = галочка
   - **Input point cloud** = `point_cloud.ply`
   - **Input json params** = `model_params.json`
   - **Path to obj** = `key` (имя файла в Resources, без расширения)
   - Выбрать папку для результата (например `Assets/RoomScenes/darkRoom/key/`)
3. Нажать **Create Asset**

Unity создаст набор файлов:
| Файл | Что хранит |
|---|---|
| `*-point_cloud.asset` | Главный asset (GaussianSplatRenderer) |
| `*_pos.bytes` | Позиции сплатов |
| `*_col.bytes` | Цвета |
| `*_shs.bytes` | Освещение (Spherical Harmonics) |
| `*_scale.bytes` | Размеры |
| `*_alpha.bytes` | Прозрачность |
| `*_oth.bytes` | Прочие параметры |
| `*_chk.bytes` | Chunk данные |

### 5.3 — Поместить в сцену
1. Hierarchy > **Create Empty** > назови объект (например `key_gauss`)
2. **Add Component > GaussianSplatRenderer**
3. В поле **Asset** перетащи созданный `.asset` файл
4. Настрой **Transform > Position** и **Scale** в Inspector

**Подбор размера:** посмотри Scale других объектов в сцене. Например, Fox имеет Scale `(0.18, 0.18, 0.18)`. Начни с такого же значения и подкорректируй.

**Если объект не видно:** выдели объект в Hierarchy и нажми **F** — камера подлетит к нему.

---

## Шаг 6: Настройка пазла (Escape Room)

### Скрипты (уже готовы в `Assets/Scripts/EscapeRoom/`)

| Скрипт | Что делает |
|---|---|
| `PuzzleStep.cs` | Базовый класс для всех пазлов |
| `PuzzleManager.cs` | Управляет последовательностью пазлов |
| `ShakeDetector.cs` | Пазл: потряси объект -> выпадает предмет |
| `KeySlot.cs` | Пазл: вставь ключ в замок |
| `Door.cs` | Анимация открытия двери |

### Упрощенный вариант: потряси лису -> выпадает ключ

#### На key_gauss:
1. Добавь тег: Inspector > **Tag > Add Tag** > создай `Key` > выбери его
2. **Add Component > Rigidbody** (Use Gravity = true)
3. **Add Component > Box Collider**
4. **Add Component > XR Grab Interactable** (чтобы брать в VR)

#### На Fox:
1. **Add Component > XR Grab Interactable** (если нет)
2. **Add Component > ShakeDetector**
3. В Inspector:
   - **Hidden Item** = перетащи `key_gauss`
   - **Puzzle Name** = `Shake Fox`
   - **Hint Text** = `Shake the fox!`

ShakeDetector при старте спрячет ключ. Когда потрясешь лису — ключ появится и упадет.

**Тестирование без VR:** в редакторе Unity нажми **Space** во время Play — ключ выпадет (отладочная клавиша, работает только в Editor).

---

## Справочник форматов

| Файл | Формат | Для чего |
|---|---|---|
| Картинка | .png/.jpg | Исходное изображение объекта |
| key.glb | GLB | 3D модель из TRELLIS |
| dataset.zip | ZIP | Датасет из BlenderNeRF (изображения + transforms) |
| mesh.obj | OBJ | Меш для GaMeS |
| point_cloud.ply | PLY | Gaussian Splats (облако точек) |
| model_params.json | JSON | Параметры GaMeS (_alpha, _scale) |
| *.asset + *.bytes | Unity | Финальные файлы для сцены |

---

## Что такое Gaussian Splatting (простое объяснение)

Обычный 3D объект (`.obj`) — это как фигурка из пластилина. Есть форма, но нет "живой" поверхности.

GaMeS берет этот `.obj` и "обклеивает" его тысячами маленьких цветных пятнышек (splats). Эти пятнышки запоминают как объект выглядит — цвет, блики, тени. А раз они привязаны к мешу — объект можно деформировать руками в VR.

### Зачем .obj если есть .glb
`.glb` — выходной формат TRELLIS. GaMeS работает только с `.obj` (простой текстовый формат — вершины и грани).
