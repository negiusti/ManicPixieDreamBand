using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "JobSystem", menuName = "Custom/JobSystem")]
public class JobSystem : ScriptableObject
{
    public enum PunkJob
    {
        Tattoo,
        Boba
    };

    private static PunkJob CurrentJob;
}
