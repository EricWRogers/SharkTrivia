using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// //Originally Programmed by Samuel (Scott)

public class CipherDecode : MonoBehaviour
{

    public List<char> keys = new List<char>();
    public List<char> values = new List<char>();

    private Dictionary<char, char> charAssignments = new Dictionary<char, char>
    {
        //tilde represents an english character which has not been assigned a ciphertext equivalent 
        {'a', '~'},{'b', '~'},{'c', '~'},{'d', '~'},
        {'e', '~'},{'f', '~'},{'g', '~'},{'h', '~'},
        {'i', '~'},{'j', '~'},{'k', '~'},{'l', '~'},
        {'m', '~'},{'n', '~'},{'o', '~'},{'p', '~'},
        {'q', '~'},{'r', '~'},{'s', '~'},{'t', '~'},
        {'u', '~'},{'v', '~'},{'w', '~'},{'x', '~'},
        {'y', '~'},{'z', '~'}
    };

    //pass in the key and value you are trying to edit
    public int CharAssignment(char key, char value)
    {
        for (int i = 97; i < 122; i++)
        {
            Debug.Log("Checking character: " + (char)i);

            //This if handles the case where the value that goes to the key in question is blank ('~') and the new value is not already found elsehwere
            if (charAssignments[(char)i] == '~' && (char)i == key && !charAssignments.ContainsValue(value))
            {
                charAssignments[key] = value;
            }

            //This if handles the case where the value that goes to the key in question is not blank but the new value also isn't found anywhere else in the journal
            else if (charAssignments[(char)i] != '~' && (char)i == key && !charAssignments.ContainsValue(value))
            {
                //Return an error code (some negative number) if you want this operation to be illegal, elsewise just overwrite the value if you're ok with it

                // return -1;

                //OR

                // charAssignments[key] = value;
            }

            //This if handles the case where the new value is already used somehwere else in the journal with some subcases
            else if ((char)i == key && charAssignments.ContainsValue(value))
            {
                //Return an error code (some other negative number) if you want this operation to be illegal, elsewise erase where the new value already was and put it here

                //Case where the value that goes to the key in question is blank
                // if (charAssignments[(char)i] == '~')
                // {
                //     // return -2;
                // }

                //Case where the value that goes to the key in question is blank
                // if (charAssignments[(char)i] != '~')
                // {
                //     // return -3;
                // }

                //OR

                // var firstKey = charAssignments.FirstOrDefault(kvp => kvp.Value == value).Key;
                // charAssignments[firstKey] = '~';
                // charAssignments[key] = value;
            }

        }
        return 0;

    }
}
