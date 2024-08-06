using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RalingAligner : MonoBehaviour
{
    [SerializeField] private List<Transform> ralings = new List<Transform>();
    [SerializeField] private float startingPosition;
    [SerializeField] private float offset;
    [SerializeField] private float randomizer = 5;
    [SerializeField] private float degreeAngle = 5;

    [ContextMenu("ALIGN")]
    public void ALIGN()
    {
        foreach (Transform raling in ralings)
        {
            Vector3 newPos;
            if (raling.GetSiblingIndex() == 0)
            {
                newPos.x = startingPosition;
            }
            else
            {
                float _randomNumber = Random.Range(0, randomizer);    
                newPos.x = ralings[raling.GetSiblingIndex() - 1].localPosition.x + offset + _randomNumber;
                
            }
            newPos.y = raling.localPosition.y;
            newPos.z = raling.localPosition.z;
            raling.localPosition = newPos;
            float randomAngle = Random.Range(-degreeAngle, degreeAngle);
            raling.rotation = Quaternion.Euler(0, randomAngle, 0);
        }
    }
}
