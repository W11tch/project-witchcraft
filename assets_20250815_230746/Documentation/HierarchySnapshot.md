# Hierarchy Snapshot for Scene: Main
**Generated on:** 15/08/2025 23:07:45
---

## Main Camera
- **Tag:** `MainCamera`
- **Layer:** `Default`
**Components (5):**
- **Component:** `Transform`
- **Component:** `Camera`
- **Component:** `AudioListener`
- **Script:** `UniversalAdditionalCameraData`
- **Script:** `CameraMovement`

## Directional Light
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (3):**
- **Component:** `Transform`
- **Component:** `Light`
- **Script:** `UniversalAdditionalLightData`

## Global Volume
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `Volume`
    - **Reference:** `sharedProfile` -> `ScriptableObject: SampleSceneProfile`

## SkyboxRotator
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `SkyboxRotator`

## Player
- **Tag:** `Player`
- **Layer:** `Player`
**Components (4):**
- **Component:** `Transform`
- **Script:** `PlayerInput`
    - **Reference:** `m_Actions` -> `ScriptableObject: InputSystem_Actions`
- **Script:** `PlayerMovement`
- **Script:** `PlayerController`
**Children: (1 total)**
### ModelePlayer
- **Tag:** `Player`
- **Layer:** `Player`
**Components (5):**
- **Component:** `Transform`
- **Component:** `MeshFilter`
- **Component:** `MeshRenderer`
- **Component:** `CapsuleCollider`
- **Component:** `Rigidbody`


## EventSystem
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (3):**
- **Component:** `Transform`
- **Script:** `EventSystem`
- **Script:** `InputSystemUIInputModule`
    - **Reference:** `m_ActionsAsset` -> `ScriptableObject: DefaultInputActions`
    - **Reference:** `m_PointAction` -> `ScriptableObject: UI/Point`
    - **Reference:** `m_MoveAction` -> `ScriptableObject: UI/Navigate`
    - **Reference:** `m_SubmitAction` -> `ScriptableObject: UI/Submit`
    - **Reference:** `m_CancelAction` -> `ScriptableObject: UI/Cancel`
    - **Reference:** `m_LeftClickAction` -> `ScriptableObject: UI/Click`
    - **Reference:** `m_MiddleClickAction` -> `ScriptableObject: UI/MiddleClick`
    - **Reference:** `m_RightClickAction` -> `ScriptableObject: UI/RightClick`
    - **Reference:** `m_ScrollWheelAction` -> `ScriptableObject: UI/ScrollWheel`
    - **Reference:** `m_TrackedDevicePositionAction` -> `ScriptableObject: UI/TrackedDevicePosition`
    - **Reference:** `m_TrackedDeviceOrientationAction` -> `ScriptableObject: UI/TrackedDeviceOrientation`

## Placed Objects
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (1):**
- **Component:** `Transform`

## PlacementManager
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `PlacementManager`

## BuildingManager
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (3):**
- **Component:** `Transform`
- **Script:** `BuildingManager`
    - **List Reference:** `buildingMenuItems` -> `ScriptableObject: BrownCube_Buildingdata`
    - **List Reference:** `buildingMenuItems` -> `ScriptableObject: Building/SelectBuilding`
    - **List Reference:** `buildingMenuItems` -> `ScriptableObject: DarkCube_BuildingData`
    - **List Reference:** `buildingMenuItems` -> `ScriptableObject: Building/SelectBuilding`
    - **List Reference:** `buildingMenuItems` -> `ScriptableObject: GreyCube_BuildingData`
    - **List Reference:** `buildingMenuItems` -> `ScriptableObject: Building/SelectBuilding`
- **Script:** `PlayerInput`
    - **Reference:** `m_Actions` -> `ScriptableObject: InputSystem_Actions`

## GroundPlane
- **Tag:** `Untagged`
- **Layer:** `Ground`
**Components (4):**
- **Component:** `Transform`
- **Component:** `MeshFilter`
- **Component:** `MeshRenderer`
- **Component:** `BoxCollider`

## WorldGridManager
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `WorldGridManager`

## ObjectPooler
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `ObjectPooler`

## EventManager
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `EventManager`

## ResourceManager
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `ResourceManager`
    - **Reference:** `debugResource1` -> `ScriptableObject: BrownCubeResource`
    - **Reference:** `debugResource2` -> `ScriptableObject: DarkCubeResource`
    - **Reference:** `debugResource3` -> `ScriptableObject: GreyCubeResource`

## Canvas
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `Canvas`
- **Script:** `CanvasScaler`
- **Script:** `GraphicRaycaster`
**Children: (2 total)**
### SaveLoadPanel
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
- **Script:** `SaveLoadUI`
**Children: (3 total)**
#### SaveButton
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
- **Script:** `Button`
**Children: (1 total)**
##### Text (TMP)
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `TextMeshProUGUI`
    - **Reference:** `m_fontAsset` -> `ScriptableObject: LiberationSans SDF`


#### LoadButton
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
- **Script:** `Button`
**Children: (1 total)**
##### Text (TMP)
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `TextMeshProUGUI`
    - **Reference:** `m_fontAsset` -> `ScriptableObject: LiberationSans SDF`


#### ClearSaveButton
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
- **Script:** `Button`
**Children: (1 total)**
##### Text (TMP)
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `TextMeshProUGUI`
    - **Reference:** `m_fontAsset` -> `ScriptableObject: LiberationSans SDF`



### InventoryPanel
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
- **Script:** `ResourceDisplay`
**Children: (1 total)**
#### Text (TMP)
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `TextMeshProUGUI`
    - **Reference:** `m_fontAsset` -> `ScriptableObject: LiberationSans SDF`



## SaveManager
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `SaveManager`

# Referenced ScriptableObject Assets
---
The following ScriptableObjects were found to be referenced by scripts in this scene:
- `SampleSceneProfile`
- `InputSystem_Actions`
- `DefaultInputActions`
- `UI/Point`
- `UI/Navigate`
- `UI/Submit`
- `UI/Cancel`
- `UI/Click`
- `UI/MiddleClick`
- `UI/RightClick`
- `UI/ScrollWheel`
- `UI/TrackedDevicePosition`
- `UI/TrackedDeviceOrientation`
- `BrownCube_Buildingdata`
- `Building/SelectBuilding`
- `DarkCube_BuildingData`
- `GreyCube_BuildingData`
- `BrownCubeResource`
- `DarkCubeResource`
- `GreyCubeResource`
- `LiberationSans SDF`

