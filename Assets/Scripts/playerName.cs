using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerName : MonoBehaviour
{
    public Text nameTag;

    [PunRPC]
    public void updateName(string name)
    {
        nameTag.text = name;
    }
}
