using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject crossHair;
    [SerializeField] private Transform duckTrans;
    [SerializeField] private float dist;
    public bool activeCrosshair;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activeCrosshair)
        {
            RaycastHit hit;
            Vector3 duck2dPos = duckTrans.forward;
            duck2dPos.y = 0f;
            duck2dPos.Normalize();
            Vector3 crossPos = duckTrans.position + dist * duck2dPos;
            crossPos.y += 5.0f;

            //Debug.DrawRay(crossPos, Vector3.down * 8.0f, Color.magenta);

            if (Physics.Raycast(crossPos, Vector3.down, out hit, 8.0f))
            {
                print("hit: " + hit.point + "\n");
                crossPos.y = hit.point.y + 0.1f;
            }
            crossHair.transform.position = crossPos;
        }

    }
    public void setState(bool state){
        crossHair.SetActive(state);
        activeCrosshair = state;
    }
}
