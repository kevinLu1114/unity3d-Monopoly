﻿using System.Collections.Generic;
using UnityEngine;

class DisplayManager
{
    private GlobalManager globalManager;
    private GameObject nextPlayerText;
    private GameObject pathListEntity;
    private int timer;

    public Group currentPlayer
    {
        get
        {
            return globalManager.CurrentPlayer;
        }
    }



    public DisplayManager(GlobalManager globalManager)
    {
        this.globalManager = globalManager;

        nextPlayerText = Resources.Load<GameObject>("PreFab/Ui/NextPlayerText");
        nextPlayerText = GameObject.Instantiate(nextPlayerText ,new Vector3(33 ,-66.25f ,0) ,Quaternion.identity);
        nextPlayerText.transform.SetParent(GameObject.Find("Canvas").transform ,false);

        pathListEntity = new GameObject("pathListEntity"); //Empty GameObject
    }

    public void displayRollingDice()
    {
        //呼叫扔骰子

        globalManager.TotalStep = 6;//temp
        currentPlayer.State = PlayerState.SearchPath;//temp
    }
    public void displaySearchPath(Map map)
    {
        createScoutPathEntity(map);
        createInteractiveDot();
    }
    public void displayPlayerMovement()
    {
        currentPlayer.CurrentActor.run(currentPlayer.Location);

        if ( currentPlayer.Scout.totalStep == 0 )
        {
            currentPlayer.Scout.deleteDot(currentPlayer.Scout.choicePath[0]);//刪除走過的點
            currentPlayer.CurrentActor.stop();

            currentPlayer.State = PlayerState.End;
        }
    }

    public void displayNextPlayer()
    {
        if ( timer % 500 == 0 )
        {
            bool now = nextPlayerText.activeSelf;
            nextPlayerText.SetActive(!now);
            
        }
        timer = ( timer + 10 ) % 500;

        if ( Input.anyKey )
        {
            nextPlayerText.SetActive(false);
            globalManager.setNextPlayer();
            globalManager.GameState = GameState.GlobalEvent;
        }
    }

    /*==========private==========*/
    /*==========ScoutPathEntity==========*/
    private void createScoutPathEntity(Map map)
    {
        bool[] markMap = new bool[map.BlockList.Length];
        for ( int i = 0 ; i < markMap.Length ; i++ ) { markMap[i] = false; }//全部標記為 "未走過"

        Position onePos;       
        for ( int i = 0 ; i < currentPlayer.Scout.pathList.Count ; i++ )
        {
            for ( int j = 0 ; j < currentPlayer.Scout.pathList[i].Count ; j++ )
            {
                onePos = currentPlayer.Scout.pathList[i][j];
                if ( markMap[onePos.blockIndex] == false )//if沒有建過 "這一個點"
                {
                    markMap[onePos.blockIndex] = true;

                    buildEntity(onePos ,onePos.blockIndex);
                    onePos.entity.transform.parent = pathListEntity.transform;//統一放到一個Empty的GameObject下面
                }
                else
                {
                    onePos.entity = pathListEntity.transform.Find("dot" + onePos.blockIndex).gameObject;//如果有了就不要再建造一次 直接指定
                }
            }
        }
    }
    private void buildEntity(Position onePos ,int mapIndex)
    {
        onePos.entity = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        onePos.entity.transform.name = "dot" + mapIndex;
        onePos.entity.transform.localScale = new Vector3(0.4f ,0.1f ,0.4f);

        onePos.entity.GetComponent<Renderer>().material = Resources.Load<Material>("Texture/Orange");

        onePos.entity.transform.position = ( onePos.location + new Vector3(0 ,0.2f ,0) );
    }
    private void createInteractiveDot()
    {
        int count = 0;
        foreach ( List<Position> onePath in currentPlayer.Scout.pathList )
        {
            GameObject entity = onePath[onePath.Count - 1].entity;

            entity.AddComponent<PositionController>();
            entity.GetComponent<PositionController>().pathNo = count++;
            entity.GetComponent<PositionController>().CheckOut = currentPlayer.Scout.checkOutPath;
            entity.GetComponent<PositionController>().Select = currentPlayer.Scout.selectPath;
            entity.GetComponent<PositionController>().Leave = currentPlayer.Scout.leavePath;
            entity.transform.localScale = new Vector3(1.5f ,0.1f ,1.5f);
        }
        //if ( currentPlayer.Scout.pathList.Count == 1 )
        //{
        //    currentPlayer.Scout.checkOutPath(0);
        //}
    }
}

