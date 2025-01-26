public class QuitBtn : Button
{
    public override void HandleButton()
    {
        base.HandleButton();
        GameManager.instance.QuitGame();
    }
}
