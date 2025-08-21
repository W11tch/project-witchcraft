# Hierarchy Snapshot for Scene: Main
**Generated on:** 21/08/2025 14:56:44
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
**Components (5):**
- **Component:** `Transform`
- **Script:** `PlayerInput`
    - **Reference:** `m_Actions` -> `ScriptableObject: InputSystem_Actions`
- **Script:** `PlayerMovement`
- **Script:** `PlayerController`
- **Component:** `CharacterController`
**Children: (1 total)**
### ModelePlayer
- **Tag:** `Player`
- **Layer:** `Player`
**Components (4):**
- **Component:** `Transform`
- **Component:** `MeshFilter`
- **Component:** `MeshRenderer`
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

## PlacementManager
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `PlacementManager`
**Children: (1 total)**
### PlacedObjects
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (1):**
- **Component:** `Transform`


## BuildingManager
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `BuildingManager`

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

## InventoryManager
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `InventoryManager`
    - **Reference:** `resourceDatabase` -> `ScriptableObject: ResourceDatabase`

## Canvas
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `Canvas`
- **Script:** `CanvasScaler`
- **Script:** `GraphicRaycaster`
**Children: (3 total)**
### DebugPanel
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
**Children: (4 total)**
#### Text (TMP)
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `TextMeshProUGUI`
    - **Reference:** `m_fontAsset` -> `ScriptableObject: LiberationSans SDF`

#### Debug_GiveResources_Button
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


#### FlyModeToggle
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (2):**
- **Component:** `RectTransform`
- **Script:** `Toggle`
**Children: (2 total)**
##### Background
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
**Children: (1 total)**
###### Checkmark
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`


##### Label
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Text`


#### DestroyModeToggle
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (2):**
- **Component:** `RectTransform`
- **Script:** `Toggle`
**Children: (2 total)**
##### Background
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
**Children: (1 total)**
###### Checkmark
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`


##### Label
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Text`



### PausePanel
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
- **Script:** `PauseUI`
**Children: (2 total)**
#### SaveLoadPanel
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
- **Script:** `SaveLoadUI`
**Children: (3 total)**
##### SaveButton
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
- **Script:** `Button`
**Children: (1 total)**
###### Text (TMP)
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `TextMeshProUGUI`
    - **Reference:** `m_fontAsset` -> `ScriptableObject: LiberationSans SDF`


##### LoadButton
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
- **Script:** `Button`
**Children: (1 total)**
###### Text (TMP)
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `TextMeshProUGUI`
    - **Reference:** `m_fontAsset` -> `ScriptableObject: LiberationSans SDF`


##### ClearSaveButton
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
- **Script:** `Button`
**Children: (1 total)**
###### Text (TMP)
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `TextMeshProUGUI`
    - **Reference:** `m_fontAsset` -> `ScriptableObject: LiberationSans SDF`



#### Text (TMP)
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
- **Script:** `InventoryUI`
**Children: (2 total)**
#### Text (TMP)
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (3):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `TextMeshProUGUI`
    - **Reference:** `m_fontAsset` -> `ScriptableObject: LiberationSans SDF`

#### InventoryDisplayPanel
- **Tag:** `Untagged`
- **Layer:** `UI`
**Components (4):**
- **Component:** `RectTransform`
- **Component:** `CanvasRenderer`
- **Script:** `Image`
- **Script:** `InventoryDisplay`
**Children: (1 total)**
##### Inventory Text
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

## DebugController
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `DebugToolsUI`
    - **Reference:** `resourceDatabase` -> `ScriptableObject: ResourceDatabase`
    - **Reference:** `debugPanel` -> `GameObject: DebugPanel`

## GameManager
- **Tag:** `Untagged`
- **Layer:** `Default`
**Components (2):**
- **Component:** `Transform`
- **Script:** `GameManager`

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
- `ResourceDatabase`
- `LiberationSans SDF`

