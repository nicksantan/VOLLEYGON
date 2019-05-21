using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]

public class ColliderToMesh : MonoBehaviour
{

    public PolygonCollider2D triangleCollider;
    public PolygonCollider2D trapezoidCollider;
    public PolygonCollider2D squareCollider;
    public PolygonCollider2D rectangleCollider;
    public PolygonCollider2D starCollider;
    public CircleCollider2D circleCollider;

    private bool isInnerShape = false;

    PolygonCollider2D pc2;
    void Start()
    {
        int whichShape = 0;

        if (gameObject.GetComponent<PlayerController>())
        {
            whichShape = gameObject.GetComponent<PlayerController>().playerType;
        }
        else
        {
            isInnerShape = true;
            Debug.Log("Using parent shape.. " + transform.parent.gameObject.GetComponent<PlayerController>().playerType.ToString());
            whichShape = transform.parent.gameObject.GetComponent<PlayerController>().playerType;
        }




        switch (whichShape)
        {

            case 0:
                pc2 = squareCollider;
                break;

            case 1:
                //pc2 = circleCollider;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                break;

            case 2:
                pc2 = triangleCollider;
                break;

            case 3:
                pc2 = trapezoidCollider;
                break;

            case 4:
                pc2 = rectangleCollider;
                break;

            case 5:
                pc2 = starCollider;
                break;
        }

        // don't act if type is circle 
        if (whichShape != 1)
        {
            //Render thing
            int pointCount = 0;
            pointCount = pc2.GetTotalPointCount();
            MeshFilter mf = GetComponent<MeshFilter>();
            Mesh mesh = new Mesh();
            Vector2[] points = pc2.points;
            Vector3[] vertices = new Vector3[pointCount];
            for (int j = 0; j < pointCount; j++)
            {
                Vector2 actual = points[j];
                vertices[j] = new Vector3(actual.x, actual.y, 0);
            }
            Triangulator tr = new Triangulator(points);
            int[] triangles = tr.Triangulate();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mf.mesh = mesh;
            //Render thing
        }
        else
        {
            // if circle AND innershape... activate child circle
            Transform circle = transform.Find("Circle");
            circle.gameObject.SetActive(true);
        }



    }
}