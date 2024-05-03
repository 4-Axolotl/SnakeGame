using UnityEngine;
using UnityEngine.SceneManagement;

public class HeadScript : MonoBehaviour
{
    Vector3 movement = new Vector3(0, 0, 0);
    Vector3 rotate = new Vector3(0, 0, 0);
    Vector3 oldrotate;
    Vector3[] oldrotatebody;
    Vector3 newfruitpos;
    Vector3 positionchanger;
    Vector3 actualposition = new Vector3((float)0.5, (float)0.5, 0);
    Vector3 auxiliarswap;
    Vector3 zero = new Vector3(0, 0, 0);
    float lastUpdate = 0;
    int snakesize = 2;
    bool Keyed = false;
    bool[] block;

    public Sprite Frt;
    public Sprite body;
    public Sprite turn;
    public Sprite tail;
    public Sprite darkGrass;
    GameObject FruitObject;
    GameObject[] snakebody;
    GameObject[] obstacles;

    int MaxX = 13;
    int MaxY = 6;
    [SerializeField] float speed = 40;
    [SerializeField] float speedfact = 20;
    [SerializeField] int requiredToWin = 25;
    [SerializeField] int obstaclesQuanty = 20;


    private float Abs(float val)
    {
        if (val < 0) return -val;
        else return val;
    }
    private void Start()
    {
        lastUpdate = 0;
        block = new bool[3*MaxX*3*MaxY];
        oldrotatebody = new Vector3[50];
        snakebody = new GameObject[100];
        obstacles = new GameObject[100];
        if (obstaclesQuanty > 49) obstaclesQuanty = 49;
        if (requiredToWin > 70) requiredToWin = 70;

        for(int i = 0; i < 50; i++)
        {
            snakebody[i] = new GameObject("Body", typeof(SpriteRenderer));
        }

        snakebody[0].name = "Tail";
        snakebody[0].GetComponent<SpriteRenderer>().sprite = tail;
        snakebody[0].transform.position = new Vector3((float)0.5, (float)0.5, 0);

        Vector3 obstaclepos;
        for (int i = 0; i < obstaclesQuanty; i++)
        {
            obstacles[i] = new GameObject("Obstacle", typeof(SpriteRenderer));
            do
            {
                obstaclepos = zero;
                do
                {
                    obstaclepos =
                        new Vector3(Random.Range(-MaxX - 1, MaxX), Random.Range(-MaxY - 1, MaxY), 0);
                } while (block[(int)obstaclepos.x + MaxX + 1 + ((int)obstaclepos.y + MaxY + 1) * (MaxX * 2 + 1)] == true);
                block[(int)obstaclepos.x + MaxX + 1 + ((int)obstaclepos.y + MaxY + 1) * (MaxX * 2 + 1)] = true;
                obstaclepos.x += (float)0.5;
                obstaclepos.y += (float)0.5;
            } while (Abs(obstaclepos.x - transform.position.x)<=(obstaclesQuanty/8) &&
                     Abs(obstaclepos.y - transform.position.y) <= (obstaclesQuanty/8));
            obstacles[i].GetComponent<SpriteRenderer>().sprite = darkGrass;
            obstacles[i].transform.position = obstaclepos;
            
        }
       
        FruitObject = new GameObject("Fruit", typeof(SpriteRenderer));
        FruitObject.GetComponent<SpriteRenderer>().sprite = Frt;
        do
        {
            do
            {
                newfruitpos =
                    new Vector3(Random.Range(-MaxX - 1, MaxX), Random.Range(-MaxY - 1, MaxY), 0);
            }while (block[(int)newfruitpos.x + MaxX + 1 + ((int)newfruitpos.y + MaxY + 1) * (MaxX * 2 + 1)] == true);
            newfruitpos.x += (float)0.5;
            newfruitpos.y += (float)0.5;
        } while ((newfruitpos.x - transform.position.x) == 0 &&
                 (newfruitpos.y - transform.position.y) == 0);
        newfruitpos.z = -1;
        FruitObject.transform.position = newfruitpos;

    }

    void Checkdeath()
    {
        bool die = false;
        for(int i = 0; i < snakesize - 1; i++)
        {
            if (snakebody[i].transform.position == transform.position) die = true;
        }
        for(int i = 0; i < obstaclesQuanty; i++)
        {
            if (obstacles[i].transform.position == transform.position) die = true;
        }
        if (transform.position.x > MaxX+.5 || transform.position.x < -MaxX-.5 || transform.position.y > MaxY+.5 || transform.position.y < -MaxY-.5) die = true;
        if (die)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
    private void BodyRotate()
    {
        Vector3 diference, prediference;
        Vector3 rotatebody = new Vector3(0, 0, 0);
        if (snakesize == 2) diference = transform.position - snakebody[0].transform.position;
        else diference = snakebody[1].transform.position - snakebody[0].transform.position;
        if (diference.x < 0) rotatebody.z = 180;
        if (diference.y > 0) rotatebody.z = 90;
        if (diference.y < 0) rotatebody.z = 270;
        snakebody[0].transform.Rotate(rotatebody - oldrotatebody[0]);
        oldrotatebody[0] = rotatebody;
        for (int i = 1; i < snakesize - 1; i++)
        {

            prediference = snakebody[i].transform.position - snakebody[i - 1].transform.position;
            if (i != snakesize - 2) diference = snakebody[i + 1].transform.position - snakebody[i].transform.position;
            else diference = transform.position - snakebody[i].transform.position;

            if (diference == prediference)
            {
                snakebody[i].GetComponent<SpriteRenderer>().sprite = body;
                rotatebody = zero;
                if (diference.x < 0) rotatebody.z = 180;
                if (diference.y > 0) rotatebody.z = 90;
                if (diference.y < 0) rotatebody.z = 270;
                snakebody[i].transform.Rotate(rotatebody - oldrotatebody[i]);
                oldrotatebody[i] = rotatebody;
            }
            else
            {
                snakebody[i].GetComponent<SpriteRenderer>().sprite = turn;
                diference += prediference;
                rotatebody = zero;
                if ((prediference.x > 0 && diference.y > 0) || (prediference.y < 0 && diference.x < 0)) rotatebody.z = 180;
                if ((prediference.x > 0 && diference.y < 0) || (prediference.y > 0 && diference.x < 0)) rotatebody.z = 270;
                if ((prediference.x < 0 && diference.y > 0) || (prediference.y < 0 && diference.x > 0)) rotatebody.z = 90;
                snakebody[i].transform.Rotate(rotatebody - oldrotatebody[i]);
                oldrotatebody[i] = rotatebody;
            }
        }
        Checkdeath();
    }
    private void GenerateFood()
    {
        do
        {
            do
            {
                newfruitpos =
                    new Vector3(Random.Range(-MaxX - 1, MaxX), Random.Range(-MaxY - 1, MaxY), 0);
            } while (block[(int)newfruitpos.x + MaxX + 1 + ((int)newfruitpos.y + MaxY + 1) * (MaxX * 2 + 1)] == true);
            newfruitpos.x += (float)0.5;
            newfruitpos.y += (float)0.5;
        } while ((newfruitpos.x - transform.position.x) == 0 &&
                 (newfruitpos.y - transform.position.y) == 0 );
        newfruitpos.z = -1;
        FruitObject.transform.position = newfruitpos;

        snakebody[snakesize - 1].name = "Tail";
        snakebody[snakesize - 1].transform.position = positionchanger;
        snakesize++;
        BodyRotate();
    }

    private void BodyMove()
    {
        for (int i = snakesize - 2; i >= 0; i--)
        {
            auxiliarswap = snakebody[i].transform.position;
            snakebody[i].transform.position = positionchanger;
            positionchanger = auxiliarswap;
        }
        BodyRotate();
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.W) && movement.y != -1) {
            movement.x = 0;
            movement.y = 1;
            rotate.z = 90;
            if (Input.GetKeyDown(KeyCode.W)) Keyed = true;
            //transform.Rotate(rotate);  
        }
        if (Input.GetKey(KeyCode.A) && movement.x != 1)
        {
            movement.x = -1;
            movement.y = 0;
            rotate.z = 180;
            if (Input.GetKeyDown(KeyCode.A)) Keyed = true;
            //transform.Rotate(rotate);
        }
        if (Input.GetKey(KeyCode.S) && movement.y != 1)
        {
            movement.x = 0;
            movement.y = -1;
            rotate.z = 270;
            if (Input.GetKeyDown(KeyCode.S)) Keyed = true;
            //transform.Rotate(rotate);
        }
        if (Input.GetKey(KeyCode.D) && movement.x != -1)
        {
            movement.x = 1;
            movement.y = 0;
            rotate.z = 0;
            if (Input.GetKeyDown(KeyCode.D))Keyed = true;
            //transform.Rotate(rotate);

        }

        if (Time.time >= speed/60f + lastUpdate || Keyed)
        {
            positionchanger = transform.position;
            transform.position = movement + positionchanger;
            actualposition = transform.position;
            actualposition.z = -1;

            transform.Rotate(rotate - oldrotate);
            oldrotate = rotate;
            lastUpdate = Time.time;
            Keyed = false;

            if (newfruitpos == actualposition)
            {
                GenerateFood();
                speed -= speed / speedfact;
            }
            else
            {
                if(movement != zero)BodyMove();
            }

        }
    }
}
