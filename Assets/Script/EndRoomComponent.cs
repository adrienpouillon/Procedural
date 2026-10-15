using UnityEngine;

public class EndRoomComponent : EntityComponent
{
    protected override void OnLaunchGame()
    {
        RandomCell();
        color = Color.blue;
    }

    protected override void OnDraw()
    {

    }

    protected override void OnUpdateGrid()
    {

    }

    protected override void OnExitGrid()
    {
        RandomCell();
    }

    protected override void OnQuitGame()
    {

    }
}
