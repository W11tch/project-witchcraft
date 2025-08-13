1. Create and Prepare the Prefab

    Create a new GameObject in your scene and design your object.

    Add a Box Collider component to the root of the GameObject. Ensure the collider's size and center accurately reflect the object.

    Add the PlaceableObject script to the GameObject.

    Add the VisualsController script to the GameObject.

    Drag the GameObject from the Hierarchy into your Project window to create a prefab.

    Delete the original GameObject from the Hierarchy.

2. Create the BuildingData Asset

    In your Project window, go to the folder Assets/Scripts/BuildingSystem/ScriptableObjects/.

    Right-click, select Create > Building System > Building Data.

    Name the new asset something descriptive, like MyNewObject_BuildingData.

3. Link the Components in the Inspector

    Click on your new prefab to open its Inspector.

    On the PlaceableObject script component, drag your new BuildingData asset into the Building Data field.

    Click on your new BuildingData asset.

    Drag your prefab into the Prefab field.

    Set the Placement Type to ActualLevel or GroundLevel or either and assign a Key (e.g., Alpha3).

4. Register the New Object

    Select the BuildingSystem GameObject in your scene's Hierarchy.

    In the Inspector, find the BuildingSystem script.

    In the Available Buildings list, click the "+" button to add a new element.

    Drag your new BuildingData asset into the new element's slot.