# UnitySimulatorClient
This is some example Unity code which can be used to communicate with Android Studio. 
## Usage
Put this folder into the Assets folder of your Unity Project.

Attach a SimulatorHandler component to the main camera. Then, attach the corresponding sensor and motor components to the simulated GameObjects. Some configuring may need to be done on the sensors.

Start the Android Studio program, then start the Unity project in the editor by pressing the play button (testing in build will not work). The Unity project will automatically pause. To begin the simulation, unpause Unity. 
