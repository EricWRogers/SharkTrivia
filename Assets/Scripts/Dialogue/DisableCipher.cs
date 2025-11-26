using UnityEngine;

public class DisableCipher : MonoBehaviour
{
    GameObject cipher;
    CipherDecode cipherDecode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Deprecated guy got called");
        cipher = GameObject.Find("usrDecode");
        cipherDecode = cipher.GetComponent<CipherDecode>();

        cipherDecode.encoding = false;
    }

    private void OnDestroy()
    {
        cipherDecode.encoding = true;
    }
}
