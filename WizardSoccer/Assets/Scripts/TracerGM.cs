using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerGM : MonoBehaviour {

    public GameObject player;
    public Spell spell;
    public GameMaster gameMaster;

    public GameObject cursorAsset;
    public GameObject waypointAsset;
    public GameObject traceAsset;

    GameObject cursor;
    GameObject previousCursor;

    static int numWaypoints = 15;
    GameObject[] waypoints = new GameObject[numWaypoints];

    float[] x = new float[numWaypoints];
    float[] y = new float[numWaypoints];

    ArrayList traceList = new ArrayList();

    int[] color_progress = new int[numWaypoints];
    Color[] starting_colors = new Color[numWaypoints];

    bool isGameOver = false;

    int currentWaypointIndex = 0;

    float timeLimit = 10.0f;
    float startTime;
    float endTime;

    bool timedOut = false;

    float accuracy;

	// Use this for initialization
	void Start () {
        string spellName = spell.GetType().ToString();

        for (int j = 0; j < numWaypoints; j++)
        {
            color_progress[j] = 0;
        }

        startTime = Time.time;
        
        if (spellName == "CurseSpell")
        {
            //Curse (skull)
            for (int i = 0; i < numWaypoints - 4; i++)
            {
                x[i + 2] = 1.3f * Mathf.Sin((1.5f * Mathf.PI * (i - 5)) /(numWaypoints - 2)) + 10;
                y[i + 2] = 1.3f * Mathf.Cos((1.5f * Mathf.PI * (i - 5)) / (numWaypoints - 2)) + 3.7f;
            }

            x[numWaypoints - 2] = 3 * Mathf.Sin((2 * Mathf.PI * ((numWaypoints - 5) - 5)) / (numWaypoints - 2)) + 9;
            y[numWaypoints - 2] = 3 * Mathf.Cos((2 * Mathf.PI * ((numWaypoints - 5) - 5)) / (numWaypoints - 2)) + 4 + 1.2f;

            x[numWaypoints - 1] = 3 * Mathf.Sin((2 * Mathf.PI * ((numWaypoints - 5) - 5)) / (numWaypoints - 2)) + 9;
            y[numWaypoints - 1] = 3 * Mathf.Cos((2 * Mathf.PI * ((numWaypoints - 5) - 5)) / (numWaypoints - 2)) + 4 + 0.6f;

            x[1] = 3 * Mathf.Sin((2 * Mathf.PI * ((5 - numWaypoints) + 5)) / (numWaypoints - 2)) + 11;
            y[1] = 3 * Mathf.Cos((2 * Mathf.PI * ((5 - numWaypoints) + 5)) / (numWaypoints - 2)) + 4 + 1.2f;

            x[0] = 3 * Mathf.Sin((2 * Mathf.PI * ((5 - numWaypoints) + 5)) / (numWaypoints - 2)) + 11;
            y[0] = 3 * Mathf.Cos((2 * Mathf.PI * ((5 - numWaypoints) + 5)) / (numWaypoints - 2)) + 4 + 0.6f;


        } else if (spellName == "WallSpell")
        {
            //Wall
            for (int i = 0; i < (numWaypoints / 3); i++)
            {
                x[i] = -2 + 10;
                y[i] = 2 + 0.5f * i;
            }

            for (int i = (numWaypoints / 3); i < ((2 * numWaypoints) / 3); i++)
            {
                x[i] = (4.0f / 6.0f) + ((4.0f/6.0f) * (i - (numWaypoints / 3))) + 8;
                y[i] = 4;
            }

            for (int i = ((2 * numWaypoints) / 3); i < numWaypoints; i++)
            {
                x[i] = 2 + 10;
                y[i] = 4 - (0.5f * (i - ((2 * numWaypoints) / 3)));
            }
        } else if (spellName == "HexSpell")
        {
            //Hex (chicken)
            for (int i = 0; i < numWaypoints - 4; i++)
            {
                x[i] = 1.3f * Mathf.Sin((2 * Mathf.PI * (i - 6)) / (numWaypoints)) + 9.2f;
                y[i] = 1.3f * Mathf.Cos((2 * Mathf.PI * (i - 6)) / (numWaypoints)) + 1 + 2.5f;
            }

            x[numWaypoints - 4] = 1.3f * Mathf.Sin((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 0.5f + 9.2f;
            y[numWaypoints - 4] = 1.3f * Mathf.Cos((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 1 + .1f + 2.2f;

            x[numWaypoints - 3] = 1.3f * Mathf.Sin((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 1.2f + 9.2f;
            y[numWaypoints - 3] = 1.3f * Mathf.Cos((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 1 - .3f + 2.2f;

            x[numWaypoints - 2] = 1.3f * Mathf.Sin((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 0.8f + 9.2f;
            y[numWaypoints - 2] = 1.3f * Mathf.Cos((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 1 - .7f + 2.2f;

            x[numWaypoints - 1] = 1.3f * Mathf.Sin((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 0.4f + 9.2f;
            y[numWaypoints - 1] = 1.3f * Mathf.Cos((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 1 - 1.1f + 2.2f;

        } else if (spellName == "ProjectileSpell")
        {
            //Fire
            for (int i = 0; i < ((numWaypoints - 1) / 2 + 1); i++)
            {
                float theta = i * (Mathf.PI / 3) / ((numWaypoints - 1) / 2 + 2);
                x[i] = 2.5f * Mathf.Cos(theta) * (1 + Mathf.Cos(3 * theta)) + 7.5f;
                y[i] = 2.5f * Mathf.Sin(theta) * (1 + Mathf.Cos(3 * theta)) + 3;
            }

            x[(numWaypoints - 1) / 2 + 1] = 7.5f;
            y[(numWaypoints - 1) / 2 + 1] = 3;

            for (int i = ((numWaypoints - 1) / 2 + 2); i < numWaypoints; i++)
            {
                float theta = (i - ((numWaypoints - 1) / 2 + 2) + 2) * (Mathf.PI / 3) / ((numWaypoints - 1) / 2 + 2);
                x[numWaypoints + ((numWaypoints - 1) / 2 + 2) - i - 1] = 2.5f * Mathf.Cos(theta) * (1 + Mathf.Cos(3 * theta)) + 7.5f;
                y[numWaypoints + ((numWaypoints - 1) / 2 + 2) - i - 1] = -1 * (2.5f * Mathf.Sin(theta) * (1 + Mathf.Cos(3 * theta))) + 3;
            }
        } else if (spellName == "WindSpell")
        {
            //Wind (tornado)
            for (int i = 0; i < numWaypoints; i++)
            {
                x[i] = (2 * Mathf.PI / numWaypoints) * i + 7;
                y[i] = 1.4f * Mathf.Sin((2 * Mathf.PI / numWaypoints) * i) + 3.1f;
            }
        } else
        {
            for (int i = 0; i < numWaypoints; i++)
            {
                x[i] = 7.2f + 0.4f * i;
                y[i] = 0.35f * Mathf.Pow((7.2f + 0.4f * i - 10), 2) + 1.7f;
            }
        }

        for (int i = 0; i < numWaypoints; i++)
        {
            Vector2 waypointPos = new Vector2(x[i], y[i]);
            waypoints[i] = Instantiate(waypointAsset, waypointPos, waypointAsset.transform.rotation);

			if (player.GetComponent<Player>().playerID == 1)
			{
				waypoints[i].transform.position = new Vector3(waypoints[i].transform.position.x, waypoints[i].transform.position.y, 24.8f);
			}

            float colorGradient = 1.0f / (numWaypoints);

            Color c = new Color(0, (1 - i * colorGradient), 0);
            waypoints[i].GetComponent<Renderer>().material.color = c;
            starting_colors[i] = c;

            waypoints[i].transform.localScale = new Vector2(2.5f, 2.5f);
        }

    }
	
	// Update is called once per frame
	void Update () {
        float currentTime = Time.time;
        float timeElapsed = currentTime - startTime;

        accuracy = Mathf.RoundToInt((float)currentWaypointIndex / (float)numWaypoints * 100);

        if (!isGameOver && (timeElapsed >= timeLimit))
        {
            isGameOver = true;
            endTime = Time.time;
            timedOut = true;
        }

        if (!isGameOver)
        {
            //If left mouse button is clicked
            if (Input.GetMouseButton(0))
            {
                //Get mouse position in screen space
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4.9f);

                //Get mouse position in world space
                Vector3 mouseWorldPos = player.GetComponent<Player>().playerCamera.GetComponent<Camera>().ScreenToWorldPoint(mousePos);

                if (distance(mouseWorldPos, waypoints[currentWaypointIndex].transform.position) < 0.25)
                {
                    color_progress[currentWaypointIndex]++;
                    currentWaypointIndex++;

                    if (currentWaypointIndex >= numWaypoints)
                    {
                        isGameOver = true;
                        endTime = Time.time;
                    }

                }

                for (int k = 0; k < numWaypoints; k++)
                {
                    if (color_progress[k] > 0 && color_progress[k] < 25)
                    {
                        waypoints[k].GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(starting_colors[k], Color.magenta, color_progress[k]/25.0f));
                        color_progress[k]++;
                    }
                    
                }

                GameObject trace = Instantiate(traceAsset, mouseWorldPos, traceAsset.transform.rotation);
                trace.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                trace.transform.localScale = new Vector2(0.3f, 0.3f);

                traceList.Add(trace);

                if (previousCursor != null)
                {
                    Destroy(previousCursor);
                }

                cursor = Instantiate(cursorAsset, mouseWorldPos, cursorAsset.transform.rotation);
                cursor.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                cursor.transform.localScale = new Vector2(1.2f, 1.2f);

                previousCursor = cursor;

            }
        } else
        {
            if (!timedOut)
            {
                for (int k = 0; k < numWaypoints; k++)
                {
                    waypoints[k].GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
                }
            }
            
            if (currentTime - endTime > 3)
            {
                //Debug.Log("ACCURACY: " + accuracy + "%");

                Destroy(cursor);

                for (int p = 0; p < numWaypoints; p++)
                {
                    Destroy(waypoints[p]);
                }

                for (int q = 0; q < traceList.Count; q++)
                {
                    Destroy((GameObject)traceList[q]);
                }

                player.GetComponent<Player>().status.accuracy = accuracy;

                Aimer aimer = Instantiate(Resources.Load("Aimer") as GameObject).GetComponent<Aimer>();
                aimer.cam = player.GetComponent<Player>().playerCamera.GetComponent<Camera>();
                aimer.spell = spell;
                aimer.gameMaster = gameMaster;
                aimer.player = player.GetComponent<Player>();
                aimer.targetList = player.GetComponent<TargetList>();
                player.GetComponent<TargetList>().CmdClear();
                aimer.startFollowing = true;
                //

                gameMaster.spellButtonsEnabled = true;
                Destroy(gameObject);
            }
        }    
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), accuracy.ToString() + "%");

        if (timedOut)
        {
            GUI.Label(new Rect(0, 15, 100, 100), "Timed out");
        }
    }

    float distance(Vector3 v1, Vector3 v2)
    {
        return Mathf.Sqrt(Mathf.Pow((v2.x - v1.x), 2) + Mathf.Pow((v2.y - v1.y), 2));
    }
}
