using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/*
 * Uses an L-system algorithm to generate a fractal tree
 * 
 */
public class TreeGenerator : MonoBehaviour {

    public bool debug;

    public Dictionary<char, string> rules = new Dictionary<char, string>();
    [Range(0, 6)]
    public int iterations = 4;
    public string input = "F"; // The axiom (sentence describing initial state of system)
    private string output;
    public string result;

    private List<Point> points = new List<Point>();
    private List<GameObject> branches = new List<GameObject>();

    public GameObject cylinder;

	// Use this for initialization
	void Start () {
        //DrawLine(new Vector3(0,0,0), new Vector3(1,1,1), Color.red);
        GrowTree();
	}

    void Update()
    {
        if (debug)
        {
            for (int i = 0; i < points.Count; i += 2)
            {
                Debug.DrawLine(points[i].point, points[i + 1].point, Color.black);
            }
        }
    }

    void GrowTree() {
        rules.Clear();
        points.Clear();
        foreach (GameObject obj in branches)
        {
            Object.DestroyImmediate(obj);
        }
        branches.Clear();
        // The L-system rules, F => F[-F]F[+F][F]
        rules.Add('F', "F[-F]F[+F][F]");

        // Apply rules
        output = input;
        for (int i = 0; i < iterations; i++)
        {
            output = applyRules(output);
        }
        result = output;
        DeterminPoints(output);
        CreateCylinders();

    }

    private string applyRules(string inputString)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (char c in inputString)
        {
            if (rules.ContainsKey(c))
                stringBuilder.Append(rules[c]);
            else
                stringBuilder.Append(c);
        }
        // Return new string with rules having been applied 
        return stringBuilder.ToString();
    }

    private void DeterminPoints(string inputString)
    {
        Stack<Point> returnValues = new Stack<Point>();
        Point lastPoint = new Point(Vector3.zero, Vector3.zero, 1f);
        returnValues.Push(lastPoint); // Push: add an object to the stack

        foreach (char c in inputString)
        {
            switch (c)
            {
                case 'F': //  Draw line of length lastBranchLength, in direction of lastAngle
                    points.Add(lastPoint);
                    Point newPoint = new Point(lastPoint.point + new Vector3(0, lastPoint.branchLength, 0),
                        lastPoint.angle, 1f);
                    newPoint.branchLength = lastPoint.branchLength - 0.02f;
                    if (newPoint.branchLength <= 0.0f)
                        newPoint.branchLength = 0.001f;
                    newPoint.angle.y = lastPoint.angle.y + UnityEngine.Random.Range(-30, 30);
                    newPoint.point = pivot(newPoint.point, lastPoint.point,
                        new Vector3(newPoint.angle.x, 0, 0));
                    newPoint.point = pivot(newPoint.point, lastPoint.point,
                        new Vector3(0, newPoint.angle.y, 0));
                    points.Add(newPoint);
                    lastPoint = newPoint;
                    break;

                case '+': // Rotate 30
                    lastPoint.angle.x += 30.0f;
                    break;

                case '[': // Save state
                   returnValues.Push(lastPoint);
                   break;
                   
                case '-': // Rotate -30
                    lastPoint.angle.x += -30.0f;
                    break;
                    
                case ']': // Load saved state
                    lastPoint = returnValues.Pop(); // Pop: remove and return the first object on the stack
                    break;
            }
        }
    }

    private void CreateCylinders()
    {
        for (int i = 0; i < points.Count; i += 2)
        {
            CreateCylinders(points[i], points[i + 1], 0.1f);
        }
    }

    private void CreateCylinders(Point point1, Point point2, float radius)
    {
        GameObject newCylinder = (GameObject)Instantiate(cylinder);
        newCylinder.SetActive(true);
        float length = Vector3.Distance(point2.point, point1.point);
        radius = radius * length;

        Vector3 scale = new Vector3(radius, length / 2.0f, radius);
        newCylinder.transform.localScale = scale;

        newCylinder.transform.position = point1.point;
        newCylinder.transform.Rotate(point2.angle);

        newCylinder.transform.parent = this.transform;

        branches.Add(newCylinder);
    }

    // Pivot point1 around point2 by angles
    private Vector3 pivot(Vector3 point1, Vector3 point2, Vector3 angles)
    {
        Vector3 dir = point1 - point2;
        dir = Quaternion.Euler(angles) * dir;
        point1 = dir + point2;
        return point1;
    }
    
    GameObject DrawLine(Vector3 start, Vector3 end, Color color) {
        GameObject line = new GameObject();
        line.transform.position = start;
        line.AddComponent<LineRenderer>();
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        return line;
    }

    struct Point
    {
        public Vector3 point;
        public Vector3 angle;
        public float branchLength;

        public Point(Vector3 point, Vector3 angle, float length)
        {
            this.point = point;
            this.angle = angle;
            this.branchLength = length;
        }
    }
}
