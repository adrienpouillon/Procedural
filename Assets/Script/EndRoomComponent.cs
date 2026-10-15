using UnityEngine;

public class EndRoomComponent : EntityComponent
{
    protected override void OnLaunchGame()
    {
        RandomCell();
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnUpdateGrid()
    {

    }

    protected override void OnExitGrid()
    {
        RandomCell();
    }

    protected override void OnDraw()
    {

    }

    protected override void OnQuitGame()
    {

    }
}
