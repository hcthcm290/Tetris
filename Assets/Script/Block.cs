using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class Block : MonoBehaviour
{
    [SerializeField] public Vector3 rotatePoint;
    float previousFallTime = 0;
    float previousMoveTime = 0;
    float fallTime = 0.5f;
    float moveTime = 0.06f;
    float maxTimeAtBtm = 6f;
    float maxTimeNoMoveAtBtm = 0.5f;
    float totalTimeNoMoveAtBtm = 0f;
    float totalTimeAtBtm = 0f;
    public static int height = 20;
    public static int width = 10;
    public static Transform[,] grid = new Transform[width, height + 3];
    float repeatMoveDelay = 0.2f;
    float totalDelay = 0.0f;
    KeyCode prevMoveKeyPress;
    GameObject projection;
    float clearLineTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // make projection for block
        projection = Instantiate(this.gameObject, transform.position, Quaternion.identity);
        foreach(Transform child in projection.transform)
        {
            Color childColor = child.GetComponent<SpriteRenderer>().color;
            child.GetComponent<SpriteRenderer>().color = new Color(childColor.r, childColor.g, childColor.b, 0.4f);
        }
        Destroy(projection.GetComponent<Block>());
        projection.AddComponent<ProjectionBlock>();
        projection.GetComponent<ProjectionBlock>().sourceBlock = this.transform;
        fallTime *= Mathf.Pow(0.93f, ScoreManager.level - 1);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] md = UnityEngine.Object.FindObjectsOfType<GameObject>();

        Vector3 moveVector = GetInputMoveVector();
        transform.position += moveVector;
        if(!ValidMove())
        {
            transform.position -= moveVector;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
            return;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            if(FindObjectOfType<HoldBlock>().Hold(this.gameObject))
            {
                Destroy(projection);
            }
            return;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotatePoint), new Vector3(0, 0, 1), -90);
            if(!ValidMove())
            {
                transform.position += new Vector3(1, 0, 0);
            }
            if(!ValidMove())
            {
                transform.position -= new Vector3(2, 0, 0);
            }
            if(!ValidMove())
            {
                transform.position += new Vector3(1, 0, 0);
                transform.position += new Vector3(0, 1, 0);
            }
            if(!ValidMove())
            {
                transform.position -= new Vector3(0, 1, 0);
                transform.RotateAround(transform.TransformPoint(rotatePoint), new Vector3(0, 0, 1), 90);
            }
        }

        if (Time.time - previousFallTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
            }

            previousFallTime = Time.time;
        }

        CheckBlockAtBottom();

        if (ReachTop())
        {
            Debug.Log("End");
            SceneManager.LoadScene("End", LoadSceneMode.Single);
        }
    }

    void HardDrop()
    {
        while(ValidMove())
        {
            transform.position += new Vector3(0, -1, 0);
        }
        transform.position -= new Vector3(0, -1, 0);
        this.enabled = false;
        AddToGrid();
        StartCoroutine(Post_Processing());
    }

    Vector3 GetInputMoveVector()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (prevMoveKeyPress != KeyCode.LeftArrow)
            {
                prevMoveKeyPress = KeyCode.LeftArrow;
                totalDelay = 0;
                return new Vector3(-1, 0, 0);
            }
            else if (totalDelay < repeatMoveDelay)
            {
                totalDelay += Time.deltaTime;
                return new Vector3(0, 0, 0);
            }
            else
            {
                if (Time.time - previousMoveTime > moveTime)
                {
                    previousMoveTime = Time.time;
                    return new Vector3(-1, 0, 0);
                }
            }
            prevMoveKeyPress = KeyCode.LeftArrow;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (prevMoveKeyPress != KeyCode.RightArrow)
            {
                totalDelay = 0;
                prevMoveKeyPress = KeyCode.RightArrow;
                return new Vector3(1, 0, 0);
            }
            else if (totalDelay < repeatMoveDelay)
            {
                totalDelay += Time.deltaTime;
                return new Vector3(0, 0, 0);
            }
            else
            {
                if (Time.time - previousMoveTime > moveTime)
                {
                    prevMoveKeyPress = KeyCode.RightArrow;
                    previousMoveTime = Time.time;
                    return new Vector3(1, 0, 0);
                }
            }
            prevMoveKeyPress = KeyCode.RightArrow;
        }
        else
        {
            prevMoveKeyPress = 0;
        }
        return new Vector3(0, 0, 0);
    }

    void CheckBlockAtBottom()
    {
        transform.position += new Vector3(0, -1, 0);

        if(!ValidMove())
        {
            transform.position -= new Vector3(0, -1, 0);
            totalTimeAtBtm += Time.deltaTime;
            if (!Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow))
            {
                totalTimeNoMoveAtBtm += Time.deltaTime;
            }
            else
            {
                totalTimeNoMoveAtBtm = 0;
            }
        }
        else
        {
            transform.position -= new Vector3(0, -1, 0);
            totalTimeAtBtm = 0;
            totalTimeNoMoveAtBtm = 0;
        }

        if (totalTimeAtBtm > maxTimeAtBtm || totalTimeNoMoveAtBtm > maxTimeNoMoveAtBtm)
        {
            this.enabled = false;
            
            AddToGrid();
            StartCoroutine(Post_Processing());
        }
    }
    bool ReachTop()
    {
        for(int x = 0; x < width; x++)
        {
            if(grid[x,19] != null)
            {
                return true;
            }
        }
        return false;
    }

    bool ValidMove()
    {
        foreach (Transform child in transform)
        {
            int xPos = Mathf.RoundToInt(child.transform.position.x);
            int yPos = Mathf.RoundToInt(child.transform.position.y);

            if (xPos < 0 || xPos >= width || yPos < 0)
            {
                return false;
            }

            if(grid[xPos,yPos] != null)
            {
                return false;
            }
        }

        return true;
    }

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            int xPos = Mathf.RoundToInt(child.transform.position.x);
            int yPos = Mathf.RoundToInt(child.transform.position.y);

            grid[xPos, yPos] = child;
        }
        FindObjectOfType<HoldBlock>().canHold = true;
        Destroy(projection);
    }

    /// <summary>
    /// Do all processing after a block is added to grid.
    /// Include: Clear all full rows, rows down, make new block
    /// </summary>
    /// <returns></returns>
    IEnumerator Post_Processing()
    {
        List<int> fullRows = new List<int>();

        // this part find all full rows in grid
        for(int y = 0; y < height; y++)
        {
            int count = 0;
            for(int x = 0; x < width; x++)
            {
                if(grid[x,y] != null)
                {
                    count++;
                }
            }
            if(count == width)
            {
                fullRows.Add(y);
            }
        }

        FindObjectOfType<LineClearEffect>().ClearLines(fullRows);

        // this part destroy all full rows and create destroy effect
        foreach (var y in fullRows)
        {
            for (int x = 0; x < width; x++)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }

        if (fullRows.Count > 0)
        {
            int x = ScoreManager.level;
            ScoreManager.AddScore(fullRows.Count);
        }

        if (fullRows.Count > 0) // if there is full rows
        {
            FindObjectOfType<Pause>().DoPause(clearLineTime); // pause the game for clear rows animation
            StartCoroutine(FindObjectOfType<SpawnBlock>().NewBlock(clearLineTime)); // born a new block after animation end
            yield return new WaitForSecondsRealtime(clearLineTime); // wait after anition end before make rows down
        }
        else // if there is no full rows
        {
            StartCoroutine(FindObjectOfType<SpawnBlock>().NewBlock(0)); // born new block immediately
        }

        RowsDown(fullRows); // make rows down
    }

    void RowsDown(List<int> deletedRows)
    {
        for(int i = deletedRows.Count - 1; i >= 0; i--)
        {
            for(int y = deletedRows[i] + 1; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    if(grid[x,y] != null)
                    {
                        grid[x, y].position = new Vector3(grid[x, y].position.x, grid[x, y].position.y - 1, 0);
                        grid[x, y - 1] = grid[x, y];
                        grid[x, y] = null;
                    }
                }
            }
        }
    }
}
