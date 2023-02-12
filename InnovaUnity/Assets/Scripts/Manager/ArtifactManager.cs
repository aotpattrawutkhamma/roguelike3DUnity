using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    public static ArtifactManager ArtiFM;
    private void Awake()
    {
        ArtiFM = this;
    }
    public List<Artifacts> artifactList;
}
