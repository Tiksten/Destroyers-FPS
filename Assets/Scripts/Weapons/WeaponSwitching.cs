using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public Collider playerPickUpTrigger;
    public int selectedWeapon = 0;

    [HideInInspector]
    public bool canChange = true;

    [HideInInspector]
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        ReloadScript ammoScript = GetComponentInChildren<ReloadScript>();
        UniversalWeaponScript weaponScript = GetComponentInChildren<UniversalWeaponScript>();

        if (ammoScript != null)
        {
            if(ammoScript.reloading == false)
                canChange = true;
            else
                canChange = false;
        }


        if (weaponScript != null)
        {
            if (weaponScript.animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
                canChange = true;
            else
                canChange = false;
        }

        else
        {
            if (ammoScript == null)
                canChange = true;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f & (canChange))
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f & (canChange))
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
        animator.SetBool("RunAndGun", false);

        if (animator.GetBool("Running") && weaponScript != null)
            if (weaponScript.animator.GetCurrentAnimatorStateInfo(0).IsTag("Shoot"))
                animator.SetBool("RunAndGun", true);
    }
    public void SelectWeapon ()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                if (weapon.gameObject.GetComponent<UniversalWeaponScript>() != null)
                {
                    weapon.gameObject.GetComponent<UniversalWeaponScript>().animator.Rebind();
                }
            }

            else
            {
                if (weapon.gameObject.GetComponent<UniversalWeaponScript>() != null)
                {
                    weapon.gameObject.GetComponent<UniversalWeaponScript>().animator.Rebind();
                }
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
