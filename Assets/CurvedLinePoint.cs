using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CurvedLinePoint : MonoBehaviour 
{
	[HideInInspector] public bool showGizmo = true;
	[HideInInspector] public float gizmoSize = 0.1f;
	[HideInInspector] public Color gizmoColor = new Color(1,0,0,0.5f);
	[SerializeField]private BoxCollider2D m_col;

    private void Start()
    {
    }

    void OnDrawGizmos()
	{
		if( showGizmo == true )
		{
			Gizmos.color = gizmoColor;

			Gizmos.DrawSphere( this.transform.localPosition, gizmoSize );
		}
	}

	//update parent line when this point moved
	void OnDrawGizmosSelected()
	{
		CurvedLineRenderer curvedLine = this.transform.parent.GetComponent<CurvedLineRenderer>();

		if( curvedLine != null )
		{
			curvedLine.Update();
		}
	}

	public void SetCollider(GameObject _obj)
	{
		m_col.transform.position = new Vector3((transform.position.x + _obj.transform.position.x) / 2, (transform.position.y + _obj.transform.position.y) / 2, 0);
		
		float dis = Vector2.Distance(transform.position, _obj.transform.position);
		m_col.size = new Vector2(dis, 0.5f);

		float angle = Vector2.SignedAngle(Vector2.right, _obj.transform.position - transform.position);
		m_col.transform.rotation = Quaternion.Euler(0,0,angle);	
    }
}
