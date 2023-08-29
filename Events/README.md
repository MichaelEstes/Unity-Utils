using Scrim;

public delegate void ActorCallback(Actor actor);
public delegate void ActionCallback(Actor actor, Actor to, ActionData action);

Events<ActionCallback>.On("Test", OnAction);
Events<ActionCallback>.On("Test", OnAction);

Events<ActorCallback>.On("Test2", OnRoundStart);
Events<ActorCallback>.Once("Test2", OnTurnStart);

Events<ActionCallback>.Emit("Test", this, this, test.actionData);
Events<ActorCallback>.Emit("Test2", this);

 Events<ActionCallback>.RemoveAll("Test");
 Events<ActorCallback>.Remove("Test2", OnRoundStart);
