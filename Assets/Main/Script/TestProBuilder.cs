using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProBuilder2.Common;
using ProBuilder2.MeshOperations;

public class TestProBuilder : MonoBehaviour {

	// Use this for initialization
    private pb_Object obj;
    pb_Selection currentSelection;
    pb_Selection previousSelection;

    private pb_Object preview;
    public Material previewMaterial;

    public GameObject vertexControlObject;

    public enum editMode { vertex, edge, face, view};
    public editMode currentEditMode;
    public editMode previousEditMode;

    public GameObject[] contollers;

    void Awake()
    {
        SpawnCube();

        //temp init
        previousEditMode = editMode.view;
        currentEditMode = editMode.vertex;

    }
    void SpawnCube()
    {
        // This creates a basic cube with ProBuilder features enabled.  See the ProBuilder.Shape enum to 
        // see all possible primitive types.
        pb_Object pb = pb_ShapeGenerator.CubeGenerator(Vector3.one);


        // The runtime component requires that a concave mesh collider be present in order for face selection
        // to work.
        pb.gameObject.AddComponent<MeshCollider>().convex = false;

        // Now set it to the currentSelection
        currentSelection = new pb_Selection(pb, null, null, null);
    }

    // Update is called once per frame
    Vector2 mousePosition_initial = Vector2.zero;
    bool dragging = false;
    public float rotateSpeed = 100f;
    public void LateUpdate()
    {
        if (!currentSelection.HasObject())
            return;

        //if (Input.GetMouseButtonDown(1) || (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt)))
        //{
        //    mousePosition_initial = Input.mousePosition;
        //    dragging = true;
        //}

        //if (dragging)
        //{
        //    Vector2 delta = (Vector3)mousePosition_initial - (Vector3)Input.mousePosition;
        //    Vector3 dir = new Vector3(delta.y, delta.x, 0f);

        //    currentSelection.pb.gameObject.transform.RotateAround(Vector3.zero, dir, rotateSpeed * Time.deltaTime);

        //    // If there is a currently selected face, update the preview.
        //    if (currentSelection.IsValid())
        //        RefreshSelectedFacePreview();
        //}

        //if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
        //{
        //    dragging = false;
        //}

        //updateObjectWControllers();
    }

    public void Update()
    {
        //if (Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.LeftAlt))
        //{

        //    if (FaceCheck(Input.mousePosition))
        //    {
        //        if (currentSelection.IsValid())
        //        {
        //            // Check if this face has been previously selected, and if so, move the face.
        //            // Otherwise, just accept this click as a selection.
        //            if (!currentSelection.Equals(previousSelection))
        //            {
        //                previousSelection = new pb_Selection(currentSelection.pb, currentSelection.face, currentSelection.edge, currentSelection.vertex);
        //                RefreshSelectedFacePreview();
        //                return;
        //            }

        //            Vector3 localNormal = pb_Math.Normal(pbUtil.ValuesWithIndices(currentSelection.pb.vertices, currentSelection.face.distinctIndices));// currentSelection.pb.GetVertices(currentSelection.face.distinctIndices));

        //            if (Input.GetKey(KeyCode.LeftShift))
        //                currentSelection.pb.TranslateVertices(currentSelection.face.distinctIndices, localNormal.normalized * -.5f);
        //            else
        //                currentSelection.pb.TranslateVertices(currentSelection.face.distinctIndices, localNormal.normalized * .5f);

        //            currentSelection.pb.Refresh();  // Refresh will update the Collision mesh volume, face UVs as applicatble, and normal information.

        //            // this create the selected face preview
        //            RefreshSelectedFacePreview();
        //        }
        //    }
        //}

        //if (currentEditMode != previousEditMode)
        //    rePaintControllers();
        if (Input.GetMouseButtonUp(0))
        drawOnHover();
    }

    private void drawOnHover()
    {
        pb_Object obj = currentSelection.pb;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        pb_RaycastHit pbRay;
        pb_HandleUtility.FaceRaycast(ray, obj, out pbRay);
        
        //pbRay.
        List<pb_Face> faces = new List<pb_Face>();

        //obj.ToMesh();
        
        

        faces.Add(obj.faces[pbRay.face]);
        obj.SetSelectedFaces(faces);
        Debug.LogFormat("das{0}", obj.faces[pbRay.face].distinctIndices);
        obj.SetFaceMaterial(faces.ToArray(), previewMaterial);
        //obj.SetFaceColor(obj.faces[pbRay.face], new Color(0.5f, 0.5f, 0.5f));
        //pb_Face[] f = obj.SelectedFaces;
        //pb_Extrude.Extrude(obj, f, ExtrudeMethod.FaceNormal, 5f);
        //obj.ToMesh();

        int[] arr = obj.faces[pbRay.face].indices;
        //var i = obj.sharedIndices;
        //Debug.LogFormat("das{0}{1}{2}{3}, {4}", arr[0], arr[1], arr[2], arr[3], obj.sharedIndices[0]);
        //pb_IntArray s = new pb_IntArray(new int[] { 0,1,2});
        
        //obj.SetSharedVertexPosition(s[0], new Vector3(5f, 0, 0));

        
        //obj.ToMesh(); //needed after changing the mesh
        obj.Refresh();
    }

    private void updateObjectWControllers() {
        pb_Object obj = currentSelection.pb;
       
        Vector3[] newPos = new Vector3[contollers.Length];
        for (int i = 0; i < newPos.Length; i++) {
            newPos[i] = contollers[i].transform.position;
        }

        //obj.TranslateVertices(obj.obj.SelectedFaces, new Vector3(0.1f, 0, 0));
        obj.SetSharedVertexPosition(0, new Vector3(5f, 0, 0));
    
        obj.Refresh();
    }

    public bool FaceCheck(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            pb_Object hitpb = hit.transform.gameObject.GetComponent<pb_Object>();

            if (hitpb == null)
                return false;

            Mesh m = hitpb.msh;

            int[] tri = new int[3] {
                    m.triangles[hit.triangleIndex * 3 + 0],
                    m.triangles[hit.triangleIndex * 3 + 1],
                    m.triangles[hit.triangleIndex * 3 + 2]
                };

            currentSelection.pb = hitpb;

            return hitpb.FaceWithTriangle(tri, out currentSelection.face);
        }
        return false;
    }
    
    void RefreshSelectedFacePreview()
    {
        pb_Face face = new pb_Face(currentSelection.face);  // Copy the currently selected face
        face.ShiftIndicesToZero();                          // Shift the selected face indices to zero

        // Copy the currently selected vertices in world space.
        // World space so that we don't have to apply transforms
        // to match the current selection.
        Vector3[] verts = currentSelection.pb.VerticesInWorldSpace(currentSelection.face.distinctIndices);

        // Now go through and move the verts we just grabbed out about .1m from the original face.
        Vector3 normal = pb_Math.Normal(verts);

        for (int i = 0; i < verts.Length; i++)
            verts[i] += normal.normalized * .01f;

        if (preview)
            Destroy(preview.gameObject);

        preview = pb_Object.CreateInstanceWithVerticesFaces(verts, new pb_Face[1] { face });
        preview.SetFaceMaterial(preview.faces, previewMaterial);
        preview.ToMesh();
        preview.Refresh();
    }

    private void makeVertexPoints() {
        pb_Object obj = currentSelection.pb;
        foreach(Vector3 pos in obj.vertices){
            Instantiate(vertexControlObject, pos, Quaternion.Euler(0, 0, 0));
        }
    }

    private void removeControllers() {
        foreach (GameObject obj in contollers) {
            GameObject.Destroy(obj);
        }
    }

    private void rePaintControllers() {
        removeControllers();
        switch (currentEditMode) {
            case editMode.vertex:
                makeVertexPoints();
                break;
        }
    }

    class pb_Selection
    {
        public pb_Object pb;    ///< This is the currently selected ProBuilder object.	
        public pb_Face face;    ///< Keep a reference to the currently selected face.
        public pb_Vertex vertex;
        public pb_Edge edge;

        public pb_Selection(pb_Object _pb, pb_Face _face, pb_Edge _edge, pb_Vertex _vertex)
        {
            pb = _pb;
            face = _face;
            vertex = _vertex;
            edge = _edge;
        }

        public bool HasObject()
        {
            return pb != null;
        }

        public bool IsValid()
        {
            return (pb != null && face != null) || (pb != null && edge != null) || (pb != null && vertex != null);
        }

        public bool Equals(pb_Selection sel)
        {
            if (sel != null && sel.IsValid())
                return (pb == sel.pb && face == sel.face) || (pb == sel.pb && edge == sel.edge) || (pb == sel.pb && vertex == sel.vertex);
            else
                return false;
        }

        public void Destroy()
        {
            if (pb != null)
                GameObject.Destroy(pb.gameObject);
        }

        public override string ToString()
        {
            return "pb_Object: " + pb == null ? "Null" : pb.name +
                "\npb_Face: " + ((face == null) ? "Null" : face.ToString()) +
                "\npb_Face: " + ((edge == null) ? "Null" : edge.ToString()) +
                "\npb_Face: " + ((vertex == null) ? "Null" : vertex.ToString());
        }
    }
}
