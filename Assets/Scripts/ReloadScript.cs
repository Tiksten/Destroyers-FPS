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
    public Text ammoText;
    [HideInInspector]
    public bool noAmmo;
    [HideInInspector]
    public bool reloading = false;
    public AudioSource reloadSound;
    public AudioSource reloadSound1;
    public AudioSource reloadSound2;
    public Animator animator;
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
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(0.8f);
        reloadSound1.Play();
        yield return new WaitForSeconds(0.6f);
        reloadSound2.Play();
        yield return new WaitForSeconds(0.6f);
        currentAmmo = maxAmmo;
        noAmmo = false;
        reloading = false;
        animator.SetBool("Reloading", false);
    }
}
