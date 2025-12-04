using UnityEngine;

public class ChangeDifficulty : MonoBehaviour
{
    public void ChangeMode(int difficulty)
    {
        CipherDecode.instance.ChangeGameMode(difficulty);
    }

}
