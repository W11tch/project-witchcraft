How to Add a New Buildable Object

This guide explains the complete, up-to-date process for adding a new placeable object to the game. The system is designed to be data-driven, making it easy to add new content without changing code.

Step 1: Create the Prefab ??

First, create the visual representation of your object.

    Create the GameObject: Design your object in the scene. Ensure its pivot point is at its base center, right where it should touch the ground.

    Add Essential Components: Add the following components to the root of your GameObject:

        Box Collider: Adjust its size and center to accurately match the object's shape.

        PlaceableObject script.

        VisualsController script.

    Create the Prefab: Drag the configured GameObject from the Hierarchy window into your Assets/Prefabs folder to create a prefab.

    Clean Up: You can now safely delete the original GameObject from the Hierarchy.

Step 2: Define the Object's Data (BuildingData) ??

This asset holds all the rules and information for your object.

    Navigate: Go to the Assets/Resources/BuildingSystem/BuildingData folder.

    Create Asset: Right-click and select Create > Building System > Building Data.

    Name It: Give it a descriptive name that matches your object (e.g., WoodenWall_Data).

    Configure in Inspector: Select the new asset and fill in its details:

        Prefab: Drag your new prefab from Assets/Prefabs into this slot.

        Pool Tag: A unique string to identify this object for pooling. A good practice is to use the prefab's name (e.g., WoodenWall).

        Category: This is the most important setting. Choose the object's fundamental type:

            Block: A foundational cube of material (like dirt or stone) that can be placed on the ground or stacked.

            Wall: An object that can only be placed on top of a Block or Bridge.

            Bridge: A walkable surface that can be placed over empty ground.

            Furniture: A decorative or functional item placed on a foundation.

        Requires Wall Behind: For Furniture, check this if the object must be placed adjacent to a Wall or an upper-level Block (e.g., a painting or wall shelf).

        Can Place On Top: Check this if you want other objects to be placeable on top of this one. Essential for Blocks and Bridges, and useful for things like tables.

        Is Walkable: Check this if characters should be able to walk on the tile this object occupies. Essential for Blocks and Bridges, and useful for things like rugs.

        Cost: Set the list Size, then for each element, assign the ResourceType asset and the required Amount.

Step 3: Link the Data to the Prefab ??

Now, we tell the prefab what data it represents.

    Select Prefab: Select your new prefab in the Assets/Prefabs folder.

    Assign Data: In the Inspector, find the Placeable Object (Script) component.

    Drag your new BuildingData asset (e.g., WoodenWall_Data) into the Building Data slot.

Step 4: Register the Object with Game Systems ??

Finally, tell the game's managers that your new object exists and is ready to be used.

    Register with the Building Hotbar:

        In the Hierarchy, select the BuildingManager GameObject.

        In the Inspector, find the Building Hotbar list. This list directly corresponds to the player's number keys.

        Element 0 is key "1", Element 1 is key "2", and so on.

        Drag your new BuildingData asset (e.g., WoodenWall_Data) into the hotbar slot you want it to occupy.

    Register with the Object Pooler:

        In the Hierarchy, select the ObjectPooler GameObject.

        In the Inspector, find the Pools list.

        Increase the list Size by one to add a new pool.

        Configure the new pool:

            Tag: Enter the exact same Pool Tag you used in the BuildingData asset (e.g., WoodenWall).

            Prefab: Drag your new prefab into this slot.

            Size: Set an initial size for how many of these objects to create at startup (e.g., 10).

Your new object is now fully integrated into the game with the new, powerful, and flexible system.
