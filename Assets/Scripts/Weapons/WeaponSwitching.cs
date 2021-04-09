using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [HideInInspector]
    public int selectedWeapon = 0;

    [HideInInspector]
    public bool canChange = true;

    [HideInInspector]
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        animator = gameObject.GetComponent<Animator>();


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
    }
    public void SelectWeapon ()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }

            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
