using UnityEngine;
using System;

public class TempCipherEncoder : MonoBehaviour
{
    // Toggle at runtime Frontstage: true, Backstage: false, this way backstage not encoded
    public static bool Enabled = true;

    // plug in the translator
    public static Func<string, string> CustomEncode;

    public static string Apply(string s)
    {
        if (!Enabled || string.IsNullOrEmpty(s)) return s;
        if (CustomEncode != null) return CustomEncode(s);

        // Fallback: simple pseudo-encoding for now
        // Replace with translator later
        string mapFrom = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        string mapTo   = "∀𐌁ḈĐɆҒǤĦłJḰŁMŊØPQЯSŦUVWX¥Z" + "αɓςđεғɠħιĵƙłɱŋøρφяʂŧυνωχγz";
        char[] arr = s.ToCharArray();
        for (int i = 0; i < arr.Length; i++)
        {
            int k = mapFrom.IndexOf(arr[i]);
            if (k >= 0 && k < mapTo.Length) arr[i] = mapTo[k];
        }
        return new string(arr);
    }
}
