using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponScript : MonoBehaviour
{
    public Camera fpsCam;
    public GameObject hitPar;
    public int damage = 30;
    public int range = 100;
    public int ammo = 20;
    public int clipSize = 20;
    public int clipCount = 5;
    public float recoilPower = 50f;

    public Animation am;
    public AnimationClip shoot;
    public AnimationClip reload;
    public GameObject crosshairCanvas;

    public animManager animMan;

    void Start()
    {
        Instantiate(crosshairCanvas);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            fireShot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public void fireShot()
    {
        if (!am.IsPlaying(reload.name) && ammo >= 1)
        {
            if (!am.IsPlaying(shoot.name))
            {
                am.CrossFade(shoot.name, 0.1f);
                ammo--;

                // Recoil
                fpsCam.transform.Rotate(Vector3.right, -recoilPower * Time.deltaTime);

                RaycastHit hit;
                Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

                if (Physics.Raycast(ray, out hit, range))
                {
                    if (hit.transform.tag == "Player")
                    {
                        hit.transform.GetComponent<PhotonView>().RPC("applyDamage", PhotonTargets.AllBuffered, damage);
                    }
                    GameObject particleClone;
                    particleClone = PhotonNetwork.Instantiate(hitPar.name, hit.point, Quaternion.LookRotation(hit.normal), 0) as GameObject;
                    Destroy(particleClone, 2);
                    Debug.Log(hit.transform.name);

                }
            }
        }
        else if (!am.IsPlaying(reload.name) && ammo == 0)
        {
            Reload(); 
        }
    }

    public void Reload()
    {
        if (clipCount >= 1)
        {
            am.CrossFade(reload.name);
            ammo = clipSize;
            clipCount--;

            animMan.reload();
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(150, 20, 150, 40), "Ammo: " + ammo + "/" + clipSize + "/" + clipCount);
    }
}
