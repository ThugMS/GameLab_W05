using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bakeRuntime : MonoBehaviour
{
    #region PublicVariables
    public NavMeshSurface Surface2D;


    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    public void Start()
    {
        Surface2D = GetComponent<NavMeshSurface>();
        Surface2D.BuildNavMeshAsync();
    }

    public void updateMesh()
    {
        Surface2D.UpdateNavMesh(Surface2D.navMeshData);
    }

    #endregion

    #region PrivateMethod
    #endregion
}