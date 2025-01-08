#  Introduction to Unity 3D
Unity Engine is a real-time 3D development software that can be used to collaborate and create immersive and interactive experiences like video games, virtual reality (VR), or augmented reality (AR) experiences, but also in the simulation of production processes; the possibilities are endless. 

The version of Unity we are using is Unity 2022.3.42f1. Using the wrong version can cause trouble in loading certain `Scenes` or `Assets` correctly. The correct version can be found at [Unity Download Archive](https://unity.com/releases/editor/archive).

## Unity's Interface

![image](https://github.com/user-attachments/assets/b0f98ab2-8c96-4dac-8774-6ffdc1c37062)

<sub>[This image has been extracted from [Unity Documentation](https://docs.unity3d.com/2022.3/Documentation/Manual/UsingTheEditor.html).]</sub>


The most common windows in their default positions: 

A. **The Toolbar** provides access to your Unity Account and Unity Cloud Services. It also contains controls for Play mode; Undo history; Unity Search; a layer visibility menu; and the Editor layout menu.

B. **The Hierarchy window** is a hierarchical text representation of every GameObject in the Scene. Each item in the Scene has an entry in the hierarchy, so the two windows are inherently linked. The hierarchy reveals the structure of how GameObjects attach to each other.

C. **The Game view** simulates what your final rendered game will look like through your Scene Cameras. When you click the Play button, the simulation begins.

D. **The Scene view** allows you to visually navigate and edit your Scene. The Scene view
 can display a 3D or 2D perspective, depending on the type of Project you are working on.

E. **Overlays** contain the basic tools for manipulating the Scene view and the GameObjects within it. You can also add custom Overlays to improve your workflow.

F. **The Inspector window** allows you to view and edit all the properties of the currently selected GameObject. Because different types of GameObjects have different sets of properties, the layout and contents of the Inspector window change each time you select a different GameObject.

G. **The Project window** displays your library of Assets that are available to use in your Project. When you import Assets into your Project, they appear here.

H. **The status bar** provides notifications about various Unity processes, and quick access to related tools and settings.
&nbsp;

>`Scenes` are where you work with content in Unity. They are assets that contain all or part of a game or application. `GameObjects` are the fundamental objects in Unity that represent characters, props, and scenery. However, a GameObject can’t do anything on its own unless it's given properties before it can become a character, an environment, or a special effect.<T>



 ## Base Libraries
Generally speaking, all C# programs are organized using namespaces. Namespaces are used for both “internal” and “external” organization systems for a program— a way of presenting program elements that are exposed to other programs

- `Unity Engine` is Unity’s core namespace, containing features such as game objects, components, physics, etc.

- `System` namespace is the core namespace of .NET and includes fundamental classes and base types for most C# programs.

- `System.Collections` and `System.Collections.Generic` are namespaces that allow one to use collections like ArrayList or List<T>.

- `System.IO` is a namespace that provides types for performing file and stream operations. It is commonly used for tasks such as reading and writing files.

- `System.Globalization` is a namespace that provides information about cultural-specific formats for dates, times, numbers, and strings. It is often used to handle application localization and internationalization (for converting strings to floats).

- `System.Linq` is a namespace that provides classes and methods that enable querying and manipulating data collections (arrays, lists, etc.) using Language Integrated Query.

> To easily access the list of namespaces in Visual Studio 2017, you can open the Solution Explorer window (View menu > Solution Explorer). Then, expand the Assembly-CSharp list and further expand the References list<T>. &nbsp;
 
## Basic Structure of a Unity C# Program 

    public class Process: MonoBehaviour
    
This defines a public class named Process. Classes in Unity inherit from ***MonoBehaviour*** which is the base class for all Unity scripts and attach them to ***GameObject*** and provide lifecycle methods. As a rule, the title of the ***MonoBehaviour*** class has to be the same as the name of the script making it a Unity script.

    Start()
    {
        //Code for initialization
    }
***Start()*** function is called when the script is first enabled, usually at the start of the game or when the GameObject becomes active. Used to initialize variables, set up states, or start coroutines.
    
    void Update()
    {
        //Code to be repeated  
    }
***Update()*** function is called once per frame. Used for handling frame-dependent logic like user input or movement updates.



### 1. Code Standards
Coding standards could keep a project consistent, making it easier for developers to swap between different areas in a project. But there are no rules set in stone and a standard can be set according to what works best for a team.

As an example, namespaces can organize a code more precisely as they allow for the separation of modules inside a project and avoid conflicts with third-party assets where class names could repeat.

>**Note:** When using namespaces in your code, break up your folder structure by namespace for better organization.<T>

A standard header is also recommended. Including a standard header in your code template helps you document the purpose of a class, the date it was created, and even who created it; essentially, all of the information could easily get lost in the long history of a project, even when using version control.

Unity employs a script template to read from whenever you create a new MonoBehaviour in the project. Every time you create a new script or shader, Unity uses a template stored in _%EDITOR_PATH%\Data\Resources\ScriptTemplates_:

- **Windows:** _C:\Program Files\Unity\Editor\Data\Resources\ScriptTemplates_
- **Mac:** _/Applications/Hub/Editor/[version]/Unity/Unity.app/Contents/ Resources/ScriptTemplates_

The default MonoBehaviour template is this one: **81-C# Script-NewBehaviourScript.cs.txt**. There are also templates for shaders, other behavior scripts, and assembly definitions. 

For project-specific script templates, an Assets/ScriptTemplates folder can be created, and the script templates are copied into this folder to override the defaults.



### 2. Structuring a Unity Project

####     2.1 Folder structure
&emsp; There isn't a set structure for the organization of folders in a Unity project but it is recommended to set a consistent standard and/or project template to keep things organized. Also, it is recommended to not deviate from the chosen naming conventions. If amendments are needed, parse and rename the affected assets all at once. If the changes affect a large number of files, consider automating the update using a script.

- **Don’t use spaces in file and folder names.** Unity’s command line tools have issues with path names that include spaces. Use _CamelCase_ as an alternative for spaces.
- **Separate testing or sandbox areas.** Create a separate folder for nonproduction scenes and experimentation. 
- **Avoid extra folders at the root level.** In general, content files should be stored within the Assets folder. Don’t create additional folders at the project’s root level unless it’s absolutely necessary.
- **Keep internal assets separate from third-party ones.** Assets from the Asset Store or other plug-ins likely have their own project structure so it is important to keep the local project assets separate.

> **Note:** If modifying a third-party asset or plug-in for a project, version control can help download the latest update for the plug-in. Once the update is imported, you can look through the diff to see where your modifications might have been overwritten, and reimplement them<T>.
####     2.2 Empty Folder

&emsp; Empty folders risk creating issues in version control, so try to only create folders for what you truly need. With Git and Perforce, empty folders are ignored by default. If such project folders are set up and someone attempts to commit them, it won’t actually work until something is placed into the folder.

>**Note:** A common workaround is to place a “.keep” file inside of an empty folder. This is enough for the folder to then be committed to the repository<T>.


####     2.3 The .meta file 
&emsp; Unity generates a .meta file for every other file inside the project. Normally, it is inadvisable to include auto-generated files in version control, but it is not the same for the .meta file. While it is an auto-generated file, it holds a lot of information about the file that it’s associated with. This is common for assets that have import settings, such as textures, meshes, audio clips, etc. When you change the import settings on those files, the changes are written into the .meta file (rather than the asset file). That’s why you commit the .meta files to your repository – so that everyone works with the same file settings. 

>**Note:** The Visible Meta Files mode should be turned on in the Version Control window (unless you’re using the built-in Plastic SCM or Perforce modes)<T>.

####     2.4 Splitting assets
&emsp; Individual, big Unity scenes do not tend to be good for collaboration so it is recommended to divide levels into several, smaller scenes to ensure smooth collaboration on a single level while minimizing the risk of conflicts.

At runtime, your project can load scenes additively using _SceneManager_ with _LoadSceneAsync_ passing the _LoadSceneMode.Additive_ parameter mode.

It’s best practice to break work up into `Prefabs` and whenever possible use Nested Prefabs. _Prefabs_ are a special type of component that allows fully configured GameObjects to be saved in the Project for reuse. These assets can then be shared between scenes, or even other projects without having to be configured again. To make changes later, the Prefab itself can be changed, rather than the scene that it’s in to avoid conflicts. Prefab changes are often easier to read when doing a diff under version control.

> Note: In the case that you end up with a scene conflict, Unity also has a built-in YAML (a human, readable, data-serialization language) tool used for merging scenes and Prefabs. For more information, refer to [Smart merge](https://docs.unity3d.com/Manual/SmartMerge.html) in Unity documentation.<T>

####     2.5 Presets
&emsp; Presets allow you to customize the default state of just about anything in your Inspector. Creating Presets allows us to copy the settings of selected components or assets, save them as assets, and then apply those same settings to other items later on. Presets can be used to enforce standards or apply reasonable defaults to new assets. Click the Preset icon at the top-right of the component. To save the Preset as an asset, click Save current to… then select one of the available Presets to load a set of values.

Here are some other handy ways to use Presets:

- Create a GameObject with defaults: Drag and drop a Preset asset into the Hierarchy to create a new GameObject with a corresponding component that includes Preset values.

- Associate a specific type with a Preset: In the Preset Manager (Project Settings > Preset Manager), specify one or more Presets per type. Creating a new component will then default to the specified Preset values.

     - Pro tip: Create multiple Presets per type, and rely on the filter to associate the correct Preset by name.

- Save and load Manager settings: Use Presets for a Manager window so that the settings can be reused. For example, if you plan to reapply the same tags and layers or physics settings, Presets can reduce setup time for your next project.


These were a few of the basics of the Unity interface and project organization in Unity. For more detailed information, refer to the [Unity User Manual](https://docs.unity3d.com/2022.2/Documentation/Manual/UnityManual.html).


# Unity - Machine Shop

This project consists of the simulation of the production planning problem of a Shipyard Block Assembly Line. This system combines a sequential flow production process (i.e. Flow shop) and a parallel machine production process (i.e. Parallel machine shop) with a buffer. 

- Block assembly line in which all blocks are processed according to the same sequence can be modeled as a sequential flow production process. 

     - All blocks can be moved to the next task only after,
        - The task currently in progress is completed.
        - The task of the previous block is completed and moved.

- The stage in which the pre-assembly process of the pre-assembled block is carried out is considered a parallel machine production process.

     - Blocks that have been assembled wait in the waiting area (BUFFER), then move to an empty workshop and undergo preliminary processes before being taken out (SINK).
     - Environmental variables such as work time and facility setup time change according to changes in work type.
     - For multiple waiting tasks, task sequencing and machine allocation have to be determined.

>The objective function considers production completion time (Makespan), workplace load, waiting space area, etc. Focus cannot be on maximizing the efficiency of one process as that may hinder the optimization of the entire system, so complex consideration at the system level is necessary.<T>

So, the two processes can be defined as, 

- **Flow production process:** Input order is the decision variable from a single process perspective, and minimizing task completion time is the goal.

- **Parallel machine production process:** Machine allocation is the deciding variable because load leveling is important.


## Elements 

![Untitled 3](https://github.com/user-attachments/assets/b75d2d28-66ab-4463-9ca9-1ab3eca6aedb)


### 1. Blocks
The block is the basic building element in our simulation flow model. It represents a part or product undergoing specific processing steps or functions in a shop. Generally speaking, blocks can have different purposes, like processing entities, controlling logic flow, or routing items. 

In this case, when simulating a process flow, the block is represented as a game object with a script attached to it that performs some action, such as changing block color and  or simulating movement. We use color to represent where the block is in the process flow, 
- Transparent Color: The block transitions from transparent to its original color when it's created.
- Original Color: The original color represents an active block.
- Darkened Color: A darker shade of the original color represents a block waiting.
- Black Color: Represents a block that has finished processing.

### 2. Machines

The machine is a specialized block that represents a specific task or operation on a block (representing a part or product) in a process. Machines typically have states like Idle, Busy, or Broken Down, and may require time to complete their tasks. In a production line simulation, a machine could be a conveyor belt or an assembly station where items are processed sequentially. Key attributes of a machine would be:

- Processing time: How long it takes to process an item.
- Capacity: How many items it can handle at a time.
- State: Whether it's currently available or in use.


### 3. Source

A source is the component where the entities or items are generated in a system. It's the starting point of the process, where new objects enter the system according to a schedule or preset rule.

### 4. Sink

A sink is the opposite of a source—it represents the endpoint where entities leave the system. It collects or disposes of completed items after they've gone through all the necessary processing steps. Here it can be considered storage for completed goods after production.

### 5. Buffer

A buffer is a block that temporarily holds entities before they proceed to the next stage of the process. Buffers are used to model storage areas, queues, or waiting points in a system. Key characteristics of a buffer are:

- Capacity: Maximum number of items it can hold.
- Flow control: Determines how items enter and leave the buffer. 

## Code

***Concept:*** 
The main logic used in this simulation is the use of `waypoints` for positional data handling. 
Waypoints are points or positions in space used to define a path that objects, or characters should follow. They are commonly used in AI navigation, patrol systems, pathfinding, and movement behaviors in games and simulations.

In Unity, waypoints are typically represented by empty GameObjects placed in the scene to mark specific locations. These points can be connected to form a path, and an object can move between them sequentially, randomly, or using specific logic. This is what we do in order to move blocks between the specific machines based on the data extracted from the CSV files.


### Block.cs

***Purpose:***
This code defines a Unity MonoBehaviour script for a Block object. The Block represents a moving object that follows a sequence of waypoints over time, changes color based on its state becomes transparent at the start, and darkens during movement.

***Code Explanation***

***Flow Summary***
#### 1.	Start Phase

   - The block starts at the first waypoint, becomes transparent, and waits until its created time before appearing.
     
#### 2.	Movement Phase
     
   - The block follows waypoints in sequence, moving smoothly and changing color as needed.

#### 3.	Completion Phase
     
   - Once the block reaches the last waypoint and exceeds the finish time, it turns black and stops moving.


### GameManager.cs

***Purpose:***
This code defines a Unity MonoBehaviour script to create and manage a job simulation. Jobs move through waypoints (positions) and interact with machines which are key positions that a job or block moves through during the simulation. 

***Code Explanation***

***Flow Summary***
#### 1.	Start Method

   - Load data from CSV files.
   - Generate source, sink, and buffer positions.
   - Create jobs and machines.
     
#### 2.	Update Method (Every Frame)
   - Continuously check for job completion
