public class StartBtn : Button
{
    

    public override void HandleButton() {
        base.HandleButton();
        GameManager.instance.StartGame();
    }
}
