using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.Assertions.Must;

public class Store : MonoBehaviour
{
    public GameObject lettersParent;
    public Button[] letters;
    int baseCost = 100;
    int totalCost;

    void Start()
    {
        //puts all the buttons into an array and sets prices
        letters = lettersParent.GetComponentsInChildren<Button>().ToArray();
        RefreshStore();
    }
    public void Buy()
    {
        //gets the current button being pressed
        GameObject button = EventSystem.current.currentSelectedGameObject;

        //checks if they have the money
        if (TotalScore.instance.GetScore() >= totalCost)
        {
            //decreases the amount of points, changes it to 'SOLD!', and turns of button
            TotalScore.instance.AddPoints(-totalCost);
            button.GetComponentInChildren<TMP_Text>().text = "SOLD!";
            button.GetComponent<Button>().interactable = false;

            //passes the buttons name by turning the string into a char Array (look idk what else to do here)
            //all the buttons have been renamed to just be a letter: 'a', 'b', 'c' etc
            CipherDecode.instance.ConfirmedCharAssignment(button.name.ToArray()[0], button.name.ToArray()[0]);
            //Debug.Log($"You have bought the letter {button.name}");
            //reset prices
            RefreshStore();
        }
        else
        {
            return;
            //Debug.Log("YOURE POOR GET OUT OF MY SHOP");
        }
    }
    void RefreshStore()
    {
        if (CipherDecode.instance == null)
        {
            Debug.Log("no Cipher");
            return;
        }
        // this does loop through every letter to check if its confirmed or not
        //(yeah it does this everytime a button is pushed)
        //Probably going to leave it as it because it sets everything up when the Scene loads
        //it also can reset the prices when a letter is bought so eh its fine
        foreach (Button letter in letters)
        {


            if (CipherDecode.instance.GetConfirmedChars().Contains(letter.gameObject.name.ToCharArray()[0]))
            {
                //already confirmed make it sold and turn off the button
                letter.interactable = false;
                letter.GetComponentInChildren<TMP_Text>().text = "SOLD!";
                //Debug.Log("this is awful if it works");
            }
            else
            {
                //not sold so set the price to a baseCost * Total amount of confirmed chars
                totalCost = CipherDecode.instance.GetConfirmedChars().Count * baseCost;
                letter.gameObject.GetComponentInChildren<TMP_Text>().text = totalCost.ToString();
            }

        }
    }
}
