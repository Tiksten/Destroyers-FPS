using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadScript : MonoBehaviour
{
    public int maxAmmo = 45;
    public int startAmmo = 40;
    [HideInInspector]
    public int currentAmmo;
    public float reloadSpeed = 2f;
    public Text ammoText;
    [HideInInspector]
    public bool noAmmo;
    public bool reloading = false;
    public AudioSource reloadSound;
    public AudioSource reloadSound1;
    void Start()
    {
        currentAmmo = startAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAmmo <= 0)
            noAmmo = true;
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo && reloading != true)
            StartCoroutine(Reload());
        if(reloading)
        {
            ammoText.text = "Reload/" + maxAmmo;
        }
        else
        {
            ammoText.text = currentAmmo + "/" + maxAmmo;
        }
    }
    private IEnumerator Reload()
    {
        reloading = true;
        reloadSound.Play();
        yield return new WaitForSeconds(reloadSpeed);
        reloadSound1.Play();
        currentAmmo = maxAmmo;
        noAmmo = false;
        reloading = false;
    }
}
