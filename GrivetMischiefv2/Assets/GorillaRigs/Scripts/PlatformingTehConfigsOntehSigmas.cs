using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingTehConfigsOntehSigmas : MonoBehaviour
{
    public ConfigurableJoint configurableJoint;
    public Transform targetObject;

    void Start()
    {
        if (configurableJoint == null)
            configurableJoint = GetComponent<ConfigurableJoint>();
    }

    void Update()
    {
        if (configurableJoint !=null && targetObject !=null)
        {
            configurableJoint.targetPosition = configurableJoint.transform.InverseTransformPoint(targetObject.position);
        }
    }
}
