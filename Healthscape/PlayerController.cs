using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform model;
    PlayerMovement playerMove;

    public static List<WeaponData> weapons;
    public int chosenWeapon;
    float shootTimer, invulnTimer, reloadTimer;
    bool reloading;

    public int maxHealth;
    public GameObject healthBar;
    [HideInInspector]
    public int health;

    private AudioSource[] audsrc;

    public Text ammoText;
    public Image weaponImg, weaponBackImg;
    public GameObject weaponWheel, weaponWheelSlot;
    public Transform weaponWheelParent;

    private void Start()
    {
        playerMove = GetComponent<PlayerMovement>();
        audsrc = GetComponents<AudioSource>();

        health = maxHealth;

        foreach(WeaponData weapon in weapons)
        {
            weapon.ammo = weapon.clipSize;
        }

        SwapWeapon(0);
    }

    private void Update()
    {
        if (!gameObject.GetComponent<PlayerMovement>().gameOver)
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = model.position.z;
            model.up = targetPos - transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            if(Time.time > shootTimer)
            {
                if (weapons[chosenWeapon].ammo > 0)
                    Shoot();
                else
                    StartReload();
            }
        }

        if(Input.GetKeyDown(KeyCode.R) && weapons[chosenWeapon].ammo < weapons[chosenWeapon].clipSize)
        {
            StartReload();
        }

        if (reloading)
        {
            weaponImg.fillAmount = 1 - (reloadTimer - Time.time) / weapons[chosenWeapon].reloadTime;
            if (Time.time > reloadTimer)
            {
                audsrc[4].Stop();
                weapons[chosenWeapon].ammo = weapons[chosenWeapon].clipSize;
                UpdateAmmoText();
                CancelReload();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetupWeaponWheel();
            weaponWheel.gameObject.SetActive(true);
            Time.timeScale = 0.1f;
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            CloseWeaponWheel();
            Time.timeScale = 1f;
        }
    }

    void Shoot()
    {
        audsrc[0].Play();
        for (int i = 0; i < weapons[chosenWeapon].projCount; i++)
        {
            GameObject newProj = Instantiate(weapons[chosenWeapon].proj, model.position, model.rotation);
            newProj.transform.Rotate(Vector3.forward * Random.Range(-weapons[chosenWeapon].spread, weapons[chosenWeapon].spread));
        }
        shootTimer = Time.time + weapons[chosenWeapon].shootCooldown;
        weapons[chosenWeapon].ammo--;
        UpdateAmmoText();
        CancelReload();
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        healthBar.GetComponent<RectTransform>().localScale = new Vector3((float)health / maxHealth, 1f, 1f);
        invulnTimer = Time.time + 0.1f;

        if(health <= 0)
        {
            gameObject.GetComponent<PlayerMovement>().GameOver();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnemyProj" && Time.time > invulnTimer && !playerMove.dashing)
        {
            if (collision.gameObject.name.Contains("CloseRange"))
            {
                audsrc[2].Play();
                audsrc[1].Play();
                TakeDamage(1);
            }
            else
            {
                audsrc[1].Play();
                TakeDamage(1);
                Destroy(collision.gameObject);
            }
        }
    }

    void UpdateAmmoText()
    {
        ammoText.text = weapons[chosenWeapon].ammo + "/" + weapons[chosenWeapon].clipSize;
    }

    void SwapWeapon(int i)
    {
        chosenWeapon = i;
        weaponImg.sprite = weapons[chosenWeapon].weaponSprite;
        weaponBackImg.sprite = weapons[chosenWeapon].weaponSprite;
        UpdateAmmoText();
        CancelReload();
    }

    void StartReload()
    {
        if (!reloading)
        {
            audsrc[4].Play();
            reloading = true;
            reloadTimer = Time.time + weapons[chosenWeapon].reloadTime;
        }
    }

    void SetupWeaponWheel()
    {
        foreach(Transform child in weaponWheelParent)
        {
            Destroy(child.gameObject);
        }

        foreach(WeaponData weapon in weapons)
        {
            GameObject newSlot = Instantiate(weaponWheelSlot, weaponWheelParent);
            newSlot.GetComponent<Image>().sprite = weapon.weaponSprite;
        }
    }

    void CloseWeaponWheel()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, hits);

        foreach(RaycastResult hit in hits)
        {
            if(hit.gameObject.tag == "WeaponWheel")
            {
                SwapWeapon(hit.gameObject.transform.GetSiblingIndex());
            }
        }

        weaponWheel.SetActive(false);
    }

    void CancelReload()
    {
        reloading = false;
        weaponImg.fillAmount = 1;
    }

}
