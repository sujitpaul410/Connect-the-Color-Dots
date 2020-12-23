using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BoardSetup : MonoBehaviour
{

    TextAsset levelData;

    public GameObject[] grid = new GameObject[25];

    public GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    public EventSystem eventSystem;

    GameObject tmp;
    public Sprite GreenX;
    public Sprite GreenY;

    public Sprite RedX;
    public Sprite RedY;

    public Sprite BlueX;
    public Sprite BlueY;

    public Sprite OrangeX;
    public Sprite OrangeY;

    public Sprite PurpleX;
    public Sprite PurpleY;

    public Sprite BlueLT;
    public Sprite BlueLB;
    public Sprite BlueRT;
    public Sprite BlueRB;

    public Sprite Angle;

    Sprite currentSprite;
    string goal = "";

    List<GameObject> tmpList = new List<GameObject>();
    struct pos { public int posX, posY; }

    Sprite RedStart, GreenStart, BlueStart, OrangeStart, PurpleStart;
    private Sprite cell;

    void Awake()
    {
        InitialiseBoard();
    }


    void Update()
    {
        //Debug.LogWarning("Game Won: "+CheckWinCondition());
        if(CheckWinCondition())
        {
            Camera.main.GetComponent<SceneSelect>().ShowMessage();
        }

        if(Input.GetMouseButton(0))
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> res = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, res);

            if (res.Count > 0)
            {
                //Debug.Log("Hit: " + res[0].gameObject.name);
                int pos = Array.IndexOf(grid, res[0].gameObject);
                int posX = pos / 5;
                int posY = pos % 5;
                //Debug.Log("CurrentPos:"+pos+" "+posX+" "+posY);


                if (tmpList.Count == 0 && res[0].gameObject.GetComponent<Image>().sprite != cell)
                {
                    //res[0].gameObject.GetComponent<Image>().sprite = YellowX;
                    if (res[0].gameObject.GetComponent<Image>().sprite.name.Contains("Red"))
                    {
                        currentSprite = RedX;
                    }
                    if (res[0].gameObject.GetComponent<Image>().sprite.name.Contains("Green"))
                    {
                        currentSprite = GreenX;
                    }
                    if (res[0].gameObject.GetComponent<Image>().sprite.name.Contains("Blue"))
                    {
                        currentSprite = BlueX;
                    }
                    if (res[0].gameObject.GetComponent<Image>().sprite.name.Contains("Orange"))
                    {
                        currentSprite = OrangeX;
                    }
                    if (res[0].gameObject.GetComponent<Image>().sprite.name.Contains("Purple"))
                    {
                        currentSprite = PurpleY;
                    }

                    if (!tmpList.Contains(res[0].gameObject))
                    {
                        tmpList.Add(res[0].gameObject);
                    }
                }
                else if (tmpList.Count > 0)
                {
                    DrawAnglurarSprites();
                    pos p = new pos();
                    p = GetPos(tmpList[tmpList.Count - 1]);

                    if (posX == p.posX && (posY - p.posY == 1 || posY - p.posY == -1) && res[0].gameObject.GetComponent<Image>().sprite == cell)
                    {
                        ChooseSpriteX();
                        res[0].gameObject.GetComponent<Image>().sprite = currentSprite;

                        if (!tmpList.Contains(res[0].gameObject))
                            tmpList.Add(res[0].gameObject);
                    }
                    if (posY == p.posY && (posX - p.posX == 1 || posX - p.posX == -1) && res[0].gameObject.GetComponent<Image>().sprite == cell)
                    {
                        ChooseSpriteY();
                        res[0].gameObject.GetComponent<Image>().sprite = currentSprite;

                        if (!tmpList.Contains(res[0].gameObject))
                            tmpList.Add(res[0].gameObject);
                    }
                    if(res[0].gameObject.GetComponent<Image>().sprite.name.Equals("RedStart") || res[0].gameObject.GetComponent<Image>().sprite.name.Equals("GreenStart")|| res[0].gameObject.GetComponent<Image>().sprite.name.Equals("BlueStart") || res[0].gameObject.GetComponent<Image>().sprite.name.Equals("OrangeStart") || res[0].gameObject.GetComponent<Image>().sprite.name.Equals("PurpleStart"))
                    {
                        if ((currentSprite.name.Contains("Red") && res[0].gameObject.GetComponent<Image>().sprite.name.Contains("Red")) || (currentSprite.name.Contains("Blue") && res[0].gameObject.GetComponent<Image>().sprite.name.Contains("Blue")) || (currentSprite.name.Contains("Green") && res[0].gameObject.GetComponent<Image>().sprite.name.Contains("Green")) || (currentSprite.name.Contains("Orange") && res[0].gameObject.GetComponent<Image>().sprite.name.Contains("Orange")) || (currentSprite.name.Contains("Purple") && res[0].gameObject.GetComponent<Image>().sprite.name.Contains("Purple")))
                        {
                            if (!tmpList.Contains(res[0].gameObject))
                            {
                                tmpList.Add(res[0].gameObject);
                            }
                        }
                        else
                        {
                            CleanUp();
                        }
                    }
                }
            }
        }
        else
        {
            GameObject go=null;
            Sprite sp=null;
            bool wrongMove=false;
            //Debug.LogWarning("Last Node: "+ tmpList[tmpList.Count - 1].GetComponent<Image>().sprite.name+" And Goal: "+goal);
            if (tmpList.Count>0)
            {
                go = tmpList[0];
                sp = go.GetComponent<Image>().sprite;
                var tmpPos = tmpList[0].GetComponent<RectTransform>().position;
                for(int i=1; i<tmpList.Count-1; i++)
                {
                    if(tmpPos==tmpList[i].GetComponent<RectTransform>().position)
                    {
                        wrongMove = true;
                        break;
                    }
                }

                if (wrongMove || !tmpList[tmpList.Count - 1].GetComponent<Image>().sprite.name.Equals(goal))
                {
                    foreach (var item in tmpList)
                    {
                        if (!item.GetComponent<Image>().sprite.name.Contains("Start"))
                            item.GetComponent<Image>().sprite = cell;
                    }
                }
            }
            tmpList.Clear();
            if(go!=null)
            {
                go.GetComponent<Image>().sprite = sp;
            }
        }
        if(tmpList.Count==25)
        {
            Debug.LogWarning("Game Completed");
        }

    }

    private void CleanUp()
    {
        for(int i=1; i<tmpList.Count; i++)
        {
            if(!tmpList[i].GetComponent<Image>().sprite.name.Contains("Start"))
                tmpList[i].GetComponent<Image>().sprite = cell;
        }
        tmpList.Clear();
    }

    public void InitialiseBoard()
    {
        raycaster = gameObject.GetComponent<GraphicRaycaster>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        GreenX = Resources.Load<Sprite>("GreenX");
        GreenY = Resources.Load<Sprite>("GreenY");
        RedX = Resources.Load<Sprite>("RedX");
        RedY = Resources.Load<Sprite>("RedY");
        BlueX = Resources.Load<Sprite>("BlueX");
        BlueY = Resources.Load<Sprite>("BlueY");
        OrangeX = Resources.Load<Sprite>("OrangeX");
        OrangeY = Resources.Load<Sprite>("OrangeY");
        PurpleX = Resources.Load<Sprite>("PurpleX");
        PurpleY = Resources.Load<Sprite>("PurpleY");

        BlueLT = Resources.Load<Sprite>("BlueLT");
        BlueLB = Resources.Load<Sprite>("BlueLB");
        BlueRT = Resources.Load<Sprite>("BlueRT");
        BlueRB = Resources.Load<Sprite>("BlueRB");

        RedStart = Resources.Load<Sprite>("RedStart");
        BlueStart = Resources.Load<Sprite>("BlueStart");
        GreenStart = Resources.Load<Sprite>("GreenStart");
        OrangeStart = Resources.Load<Sprite>("OrangeStart");
        PurpleStart = Resources.Load<Sprite>("PurpleStart");
        cell=Resources.Load<Sprite>("cell");
        for (int i = 0; i < 25; i++)
        {
            grid[i] = GameObject.Find((i+1).ToString());
        }

        foreach (var item in grid)
        {
            item.GetComponent<Image>().sprite = cell;
        }

        tmpList.Clear();

        levelData = Resources.Load<TextAsset>("LevelData");
        LevelData ld = JsonUtility.FromJson<LevelData>(levelData.ToString());
        //Debug.LogWarning("From JSON, DataCount: "+ld.val.Length);
        switch (SceneSelect.levelID)
        {
            case "Level1":
                {
                    grid[ld.val[0].Red[0] - 1].GetComponent<Image>().sprite = RedStart;
                    grid[ld.val[0].Red[1] - 1].GetComponent<Image>().sprite = RedStart;

                    grid[ld.val[0].Green[0] - 1].GetComponent<Image>().sprite = GreenStart;
                    grid[ld.val[0].Green[1] - 1].GetComponent<Image>().sprite = GreenStart;

                    grid[ld.val[0].Blue[0] - 1].GetComponent<Image>().sprite = BlueStart;
                    grid[ld.val[0].Blue[1] - 1].GetComponent<Image>().sprite = BlueStart;

                    grid[ld.val[0].Orange[0] - 1].GetComponent<Image>().sprite = OrangeStart;
                    grid[ld.val[0].Orange[1] - 1].GetComponent<Image>().sprite = OrangeStart;

                    grid[ld.val[0].Purple[0] - 1].GetComponent<Image>().sprite = PurpleStart;
                    grid[ld.val[0].Purple[1] - 1].GetComponent<Image>().sprite = PurpleStart;
                }
                break;
            case "Level2":
                {
                    grid[ld.val[1].Red[0] - 1].GetComponent<Image>().sprite = RedStart;
                    grid[ld.val[1].Red[1] - 1].GetComponent<Image>().sprite = RedStart;

                    grid[ld.val[1].Green[0] - 1].GetComponent<Image>().sprite = GreenStart;
                    grid[ld.val[1].Green[1] - 1].GetComponent<Image>().sprite = GreenStart;

                    grid[ld.val[1].Blue[0] - 1].GetComponent<Image>().sprite = BlueStart;
                    grid[ld.val[1].Blue[1] - 1].GetComponent<Image>().sprite = BlueStart;

                    grid[ld.val[1].Orange[0] - 1].GetComponent<Image>().sprite = OrangeStart;
                    grid[ld.val[1].Orange[1] - 1].GetComponent<Image>().sprite = OrangeStart;

                    grid[ld.val[1].Purple[0] - 1].GetComponent<Image>().sprite = PurpleStart;
                    grid[ld.val[1].Purple[1] - 1].GetComponent<Image>().sprite = PurpleStart;
                }
                break;
            case "Level3":
                {
                    grid[ld.val[2].Red[0] - 1].GetComponent<Image>().sprite = RedStart;
                    grid[ld.val[2].Red[1] - 1].GetComponent<Image>().sprite = RedStart;

                    grid[ld.val[2].Green[0] - 1].GetComponent<Image>().sprite = GreenStart;
                    grid[ld.val[2].Green[1] - 1].GetComponent<Image>().sprite = GreenStart;

                    grid[ld.val[2].Blue[0] - 1].GetComponent<Image>().sprite = BlueStart;
                    grid[ld.val[2].Blue[1] - 1].GetComponent<Image>().sprite = BlueStart;

                    grid[ld.val[2].Orange[0] - 1].GetComponent<Image>().sprite = OrangeStart;
                    grid[ld.val[2].Orange[1] - 1].GetComponent<Image>().sprite = OrangeStart;

                    grid[ld.val[2].Purple[0] - 1].GetComponent<Image>().sprite = PurpleStart;
                    grid[ld.val[2].Purple[1] - 1].GetComponent<Image>().sprite = PurpleStart;
                }
                break;
            case "Level4":
                {
                    grid[ld.val[3].Red[0] - 1].GetComponent<Image>().sprite = RedStart;
                    grid[ld.val[3].Red[1] - 1].GetComponent<Image>().sprite = RedStart;

                    grid[ld.val[3].Green[0] - 1].GetComponent<Image>().sprite = GreenStart;
                    grid[ld.val[3].Green[1] - 1].GetComponent<Image>().sprite = GreenStart;

                    grid[ld.val[3].Blue[0] - 1].GetComponent<Image>().sprite = BlueStart;
                    grid[ld.val[3].Blue[1] - 1].GetComponent<Image>().sprite = BlueStart;

                    grid[ld.val[3].Orange[0] - 1].GetComponent<Image>().sprite = OrangeStart;
                    grid[ld.val[3].Orange[1] - 1].GetComponent<Image>().sprite = OrangeStart;

                    grid[ld.val[3].Purple[0] - 1].GetComponent<Image>().sprite = PurpleStart;
                    grid[ld.val[3].Purple[1] - 1].GetComponent<Image>().sprite = PurpleStart;
                }
                break;
            case "Level5":
                {
                    grid[ld.val[4].Red[0] - 1].GetComponent<Image>().sprite = RedStart;
                    grid[ld.val[4].Red[1] - 1].GetComponent<Image>().sprite = RedStart;

                    grid[ld.val[4].Green[0] - 1].GetComponent<Image>().sprite = GreenStart;
                    grid[ld.val[4].Green[1] - 1].GetComponent<Image>().sprite = GreenStart;

                    grid[ld.val[4].Blue[0] - 1].GetComponent<Image>().sprite = BlueStart;
                    grid[ld.val[4].Blue[1] - 1].GetComponent<Image>().sprite = BlueStart;

                    grid[ld.val[4].Orange[0] - 1].GetComponent<Image>().sprite = OrangeStart;
                    grid[ld.val[4].Orange[1] - 1].GetComponent<Image>().sprite = OrangeStart;

                    grid[ld.val[4].Purple[0] - 1].GetComponent<Image>().sprite = PurpleStart;
                    grid[ld.val[4].Purple[1] - 1].GetComponent<Image>().sprite = PurpleStart;
                }
                break;
            case "Level6":
                {
                    grid[ld.val[5].Red[0] - 1].GetComponent<Image>().sprite = RedStart;
                    grid[ld.val[5].Red[1] - 1].GetComponent<Image>().sprite = RedStart;

                    grid[ld.val[5].Green[0] - 1].GetComponent<Image>().sprite = GreenStart;
                    grid[ld.val[5].Green[1] - 1].GetComponent<Image>().sprite = GreenStart;

                    grid[ld.val[5].Blue[0] - 1].GetComponent<Image>().sprite = BlueStart;
                    grid[ld.val[5].Blue[1] - 1].GetComponent<Image>().sprite = BlueStart;

                    grid[ld.val[5].Orange[0] - 1].GetComponent<Image>().sprite = OrangeStart;
                    grid[ld.val[5].Orange[1] - 1].GetComponent<Image>().sprite = OrangeStart;

                    grid[ld.val[5].Purple[0] - 1].GetComponent<Image>().sprite = PurpleStart;
                    grid[ld.val[5].Purple[1] - 1].GetComponent<Image>().sprite = PurpleStart;
                }
                break;
            case "Level7":
                {
                    grid[ld.val[6].Red[0] - 1].GetComponent<Image>().sprite = RedStart;
                    grid[ld.val[6].Red[1] - 1].GetComponent<Image>().sprite = RedStart;

                    grid[ld.val[6].Green[0] - 1].GetComponent<Image>().sprite = GreenStart;
                    grid[ld.val[6].Green[1] - 1].GetComponent<Image>().sprite = GreenStart;

                    grid[ld.val[6].Blue[0] - 1].GetComponent<Image>().sprite = BlueStart;
                    grid[ld.val[6].Blue[1] - 1].GetComponent<Image>().sprite = BlueStart;

                    grid[ld.val[6].Orange[0] - 1].GetComponent<Image>().sprite = OrangeStart;
                    grid[ld.val[6].Orange[1] - 1].GetComponent<Image>().sprite = OrangeStart;

                    grid[ld.val[6].Purple[0] - 1].GetComponent<Image>().sprite = PurpleStart;
                    grid[ld.val[6].Purple[1] - 1].GetComponent<Image>().sprite = PurpleStart;
                }
                break;
            case "Level8":
                {
                    grid[ld.val[7].Red[0] - 1].GetComponent<Image>().sprite = RedStart;
                    grid[ld.val[7].Red[1] - 1].GetComponent<Image>().sprite = RedStart;

                    grid[ld.val[7].Green[0] - 1].GetComponent<Image>().sprite = GreenStart;
                    grid[ld.val[7].Green[1] - 1].GetComponent<Image>().sprite = GreenStart;

                    grid[ld.val[7].Blue[0] - 1].GetComponent<Image>().sprite = BlueStart;
                    grid[ld.val[7].Blue[1] - 1].GetComponent<Image>().sprite = BlueStart;

                    grid[ld.val[7].Orange[0] - 1].GetComponent<Image>().sprite = OrangeStart;
                    grid[ld.val[7].Orange[1] - 1].GetComponent<Image>().sprite = OrangeStart;

                    grid[ld.val[7].Purple[0] - 1].GetComponent<Image>().sprite = PurpleStart;
                    grid[ld.val[7].Purple[1] - 1].GetComponent<Image>().sprite = PurpleStart;
                }
                break;
            case "Level9":
                {
                    grid[ld.val[8].Red[0] - 1].GetComponent<Image>().sprite = RedStart;
                    grid[ld.val[8].Red[1] - 1].GetComponent<Image>().sprite = RedStart;

                    grid[ld.val[8].Green[0] - 1].GetComponent<Image>().sprite = GreenStart;
                    grid[ld.val[8].Green[1] - 1].GetComponent<Image>().sprite = GreenStart;

                    grid[ld.val[8].Blue[0] - 1].GetComponent<Image>().sprite = BlueStart;
                    grid[ld.val[8].Blue[1] - 1].GetComponent<Image>().sprite = BlueStart;

                    grid[ld.val[8].Orange[0] - 1].GetComponent<Image>().sprite = OrangeStart;
                    grid[ld.val[8].Orange[1] - 1].GetComponent<Image>().sprite = OrangeStart;

                    grid[ld.val[8].Purple[0] - 1].GetComponent<Image>().sprite = PurpleStart;
                    grid[ld.val[8].Purple[1] - 1].GetComponent<Image>().sprite = PurpleStart;
                }
                break;
            case "Level10":
                {
                    grid[ld.val[9].Red[0] - 1].GetComponent<Image>().sprite = RedStart;
                    grid[ld.val[9].Red[1] - 1].GetComponent<Image>().sprite = RedStart;

                    grid[ld.val[9].Green[0] - 1].GetComponent<Image>().sprite = GreenStart;
                    grid[ld.val[9].Green[1] - 1].GetComponent<Image>().sprite = GreenStart;

                    grid[ld.val[9].Blue[0] - 1].GetComponent<Image>().sprite = BlueStart;
                    grid[ld.val[9].Blue[1] - 1].GetComponent<Image>().sprite = BlueStart;

                    grid[ld.val[9].Orange[0] - 1].GetComponent<Image>().sprite = OrangeStart;
                    grid[ld.val[9].Orange[1] - 1].GetComponent<Image>().sprite = OrangeStart;

                    grid[ld.val[9].Purple[0] - 1].GetComponent<Image>().sprite = PurpleStart;
                    grid[ld.val[9].Purple[1] - 1].GetComponent<Image>().sprite = PurpleStart;
                }
                break;
            default:
                break;
        }
    }

    pos GetPos(GameObject go)
    {
        int position = Array.IndexOf(grid, go);
        int posX = position / 5;
        int posY = position % 5;

        pos p = new pos();
        p.posX = posX;
        p.posY = posY;

        //Debug.Log("LastPos:" + position + " " + posX + " " + posY);
        return p;
    }


    void ChooseSpriteY()
    {
        if (currentSprite.name.Contains("Red"))
        {
            currentSprite = RedY;
            goal = "RedStart";
        }
        if (currentSprite.name.Contains("Green"))
        {
            currentSprite = GreenY;
            goal = "GreenStart";
        }
        if (currentSprite.name.Contains("Blue"))
        {
            currentSprite = BlueY;
            goal = "BlueStart";
        }
        if (currentSprite.name.Contains("Orange"))
        {
            currentSprite = OrangeY;
            goal = "OrangeStart";
        }
        if (currentSprite.name.Contains("Purple"))
        {
            currentSprite = PurpleY;
            goal = "PurpleStart";
        }
    }

    void ChooseSpriteX()
    {
        if (currentSprite.name.Contains("Red"))
        {
            currentSprite = RedX;
            goal = "RedStart";
        }
        if (currentSprite.name.Contains("Green"))
        {
            currentSprite = GreenX;
            goal = "GreenStart";
        }
        if (currentSprite.name.Contains("Blue"))
        {
            currentSprite = BlueX;
            goal = "BlueStart";
        }
        if (currentSprite.name.Contains("Orange"))
        {
            currentSprite = OrangeX;
            goal = "OrangeStart";
        }
        if (currentSprite.name.Contains("Purple"))
        {
            currentSprite = PurpleX;
            goal = "PurpleStart";
        }
    }


    void DrawAnglurarSprites()
    {
        if(tmpList.Count-3>=0)
        {
            pos pNew = new pos();
            pos pOld = new pos();
            pos pMid = new pos();

            pNew = GetPos(tmpList[tmpList.Count - 1]);
            pMid = GetPos(tmpList[tmpList.Count - 2]);
            pOld = GetPos(tmpList[tmpList.Count - 3]);
            //Debug.LogWarning("Old X: "+pOld.posX+" Old Y: "+pOld.posY);
            //Debug.LogWarning("New X: "+pNew.posX+" New Y: "+pNew.posY);

            if (((pNew.posX - pOld.posX == -1 && pNew.posY - pOld.posY == 1) || (pNew.posX - pOld.posX == 1 && pNew.posY - pOld.posY == -1) || (pNew.posX - pOld.posX == -1 && pNew.posY - pOld.posY == -1) || (pNew.posX - pOld.posX == 1 && pNew.posY - pOld.posY == 1)))
            {
                if (pNew.posX < pOld.posX && pNew.posY > pOld.posY && pMid.posX == pNew.posX)
                {
                    Angle = BlueLT;
                    tmpList[tmpList.Count - 2].GetComponent<Image>().sprite = Angle;
                }
                if (pNew.posX > pOld.posX && pNew.posY < pOld.posY && pMid.posX == pOld.posX)
                {
                    Angle = BlueLT;
                    tmpList[tmpList.Count - 2].GetComponent<Image>().sprite = Angle;
                }
                if (pNew.posX > pOld.posX && pNew.posY > pOld.posY && pMid.posY != pNew.posY)
                {
                    Angle = BlueLB;
                    tmpList[tmpList.Count - 2].GetComponent<Image>().sprite = Angle;
                }
                if (pNew.posX < pOld.posX && pNew.posY < pOld.posY && pMid.posX == pOld.posX)
                {
                    Angle = BlueLB;
                    tmpList[tmpList.Count - 2].GetComponent<Image>().sprite = Angle;
                }
                if (pNew.posX > pOld.posX && pNew.posY > pOld.posY && pMid.posY==pNew.posY)
                {
                    Angle = BlueRT;
                    tmpList[tmpList.Count - 2].GetComponent<Image>().sprite = Angle;
                }
                if (pNew.posX < pOld.posX && pNew.posY < pOld.posY && pMid.posX!=pOld.posX)
                {
                    Angle = BlueRT;
                    tmpList[tmpList.Count - 2].GetComponent<Image>().sprite = Angle;
                }
                if (pNew.posX > pOld.posX && pNew.posY < pOld.posY && pMid.posY == pOld.posY)
                {
                    Angle = BlueRB;
                    tmpList[tmpList.Count - 2].GetComponent<Image>().sprite = Angle;
                }
                if (pNew.posX < pOld.posX && pNew.posY > pOld.posY && pMid.posX == pOld.posX)
                {
                    Angle = BlueRB;
                    tmpList[tmpList.Count - 2].GetComponent<Image>().sprite = Angle;
                }
            }
            else
            {
                //Debug.LogWarning("PrimaryCondition not set well..");
            }
        }
    }

    private bool CheckWinCondition()
    {
        foreach (var item in grid)
        {
            if(item.GetComponent<Image>().sprite==cell)
            {
                return false;
            }
        }
        return true;
    }
}
