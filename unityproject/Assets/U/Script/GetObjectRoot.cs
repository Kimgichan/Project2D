using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObjectRoot : MonoBehaviour
{
    

    void Start()
    {
       GameObject ob = transform.Find("UnitRoot/Root/BodySet/P_Body/ArmSet/ArmR/P_RArm/-20_R_Arm/P_Weapon/R_Weapon").gameObject;

       if(ob)
        {
            //Debug.Log("존재");
            //Debug.Log(ob);

            SpriteRenderer obS = ob.GetComponent<SpriteRenderer>();

            obS.enabled = false;
        }

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
