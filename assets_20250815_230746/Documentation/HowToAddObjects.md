How to Add a New Buildable Object

This guide explains the complete, up-to-date process for adding a new object to the building system.

Step 1: Create and Prepare the Prefab

    Create the GameObject: Design your object in the scene.

    Add Components: Add the following essential components to the root of your GameObject:

        Box Collider: Adjust its size and center to accurately match the object's shape.

        PlaceableObject script.

        VisualsController script.

    Create the Prefab: Drag the configured GameObject from the Hierarchy window into your Assets/Prefabs folder.

    Clean Up: You can now safely delete the original GameObject from the Hierarchy.

Step 2: Create the Data Assets

The system now uses three separate assets to define a buildable object and its cost.

    Create the ResourceType Asset:

        If the resource for this object (e.g., "Wood") doesn't exist yet, navigate to the Assets/Resources/Core/Resources folder.

        Right-click and select Create > Resources > Resource Type.

        Name it after the resource (e.g., WoodResource).

        Select the new asset and give it a user-friendly Display Name in the Inspector (e.g., "Wood").

    Create the BuildingData Asset:

        Navigate to the Assets/Resources/BuildingSystem/BuildingData folder.

        Right-click and select Create > Building System > Building Data.

        Name it descriptively (e.g., MyNewObject_Data).

        Select the new asset. In the Inspector:

            Drag your prefab into the Prefab slot.

            Set its Placement Type.

            Give it a unique Pool Tag (e.g., MyNewObject).

            Set the Cost. Set the list Size (e.g., to 1), then assign the ResourceType (e.g., WoodResource) and the Amount required.

    Create the BuildingMenuItem Asset:

        Navigate to Assets/Scripts/BuildingSystem/ScriptableObjects/.

        Right-click and select Create > Building System > Building Menu Item.

        Name it descriptively (e.g., MyNewObject_MenuItem).

        Select this new asset. In the Inspector:

            Drag your new MyNewObject_Data asset into the Building Data slot.

            For the Select Action, assign the SelectBuilding action from your Input Actions asset.

            For the Key Identifier, type the key that should select this object (e.g., 4).

Step 3: Link the Prefab and Data

    Select your new prefab in the Assets/Prefabs folder.

    In the Inspector, find the Placeable Object (Script) component.

    Drag your MyNewObject_Data asset into the Building Data slot.

Step 4: Register the New Object with the Game Systems

    Register with the Building System:

        Select the BuildingManager GameObject in your Hierarchy.

        In the Inspector, find the Building Menu Items list.

        Click the + button and drag your new MyNewObject_MenuItem asset into the new slot.

    Register with the Object Pooler:

        Select the ObjectPooler GameObject in your Hierarchy.

        In the Inspector, find the Pools list.

        Click the + button to add a new pool.

        Configure the new pool:

            Tag: Enter the exact same Pool Tag you used in the BuildingData asset (e.g., MyNewObject).

            Prefab: Drag your new prefab into this slot.

            Size: Set an initial size for the pool (e.g., 2).

Your new object is now fully integrated, cost-accounted, and optimized.
