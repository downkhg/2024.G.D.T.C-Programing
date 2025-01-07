using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Responner : MonoBehaviour
{
    public GameObject objPrefab;
    public GameObject objTarget;

    //public Transform trPatrolPoint;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(string.Format("{0}/{1}:Start()", this.gameObject.name, this.name));
    }

    // Update is called once per frame
    void Update()
    {
        if(objTarget == null)
        {
            objTarget = Instantiate(objPrefab, this.transform.position, Quaternion.identity);
            //Eagle eagle = objTarget.GetComponent<Eagle>();
            //if(eagle)
            //{
            //    eagle.trResponPoint = this.transform;
            //    eagle.trPatrolPoint = this.trPatrolPoint;
            //}
        }
    }
}
