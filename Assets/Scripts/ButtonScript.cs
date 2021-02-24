using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public List<DoorScript> doors;
    MeshRenderer mRend;
    public Material unactiveButton;
    public Material activeButton;
    bool clicked = false;

    public void ClickButton()
    {
        if (!clicked)
        {
            clicked = true;
            foreach(DoorScript i in doors)
                i.Activate();
            GetComponent<MeshRenderer>().material = unactiveButton;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        clicked = false;
        GetComponent<MeshRenderer>().material = activeButton;
    }
}
