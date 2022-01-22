# Unity 2Dish Template

Made using Unity 2021.2.8f1

## CreateOnLoad

Multi-scene objects are created at runtime using the Assets/Template/Utility/CreateOnLoad.cs script. That script instantiates two prefabs (GlobalSystems and DialogueSystem) located at Assets/Template/Resources prior to any other script runs Awake() or Start(). To add objects that are created at load add to the GlobalSystems prefab or edit this script.

## GameEventChannel

Different parts of the program can communicate with each other through the GameEventChannel. This is how Dialogues or Transitions are started (amongst other things).  GameEventChannel is a ScriptableObject and can be dragged into prefabs or scene objects, allowing any sort of object access to it.

Scripts can register with the GameEventChannel to recieve a callback for specific events. Any object with a limited lifecycle that registers with the GameEventChannel should remove its listeners when finished.

For exmaple, to create a transition to another scene:

```
GameEventChannel.Broadcast(new TransitionGEM()
{
	TransitionOutEffect = TransitionEffectEnum.Scissor,
	NewSceneName = "ExampleScene",
	OnClose = new System.Action(()=>
	{
		GameDataChannel.StartNewGame(slot);
	}),
});
```