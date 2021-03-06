﻿using UnityEngine;
using System.Collections;

using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;


public class UpgradeBuilding : InputSource
{

    public MinorIsland island;
    public Building building;

    private Client Client;

    void Awake()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
    }

    void OnMouseDownSimulation()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        building = GameObject.Find(island.nameBuildingTouchCanvas).GetComponent<Building>();

        if (this.name == "Upgrade")
        {
            if (building.level < 3)
            {
                //withdrawal of resources needed for the upgrading
                switch (building.level)
                {
                    case 0:
                        foreach (Tuple<TypeResource, int> tuple in building.upgrade1ResourceNeeded)
                        {
                            island.resourceManager.changeResourceStock(tuple.First, -tuple.Second);
                        }
                        break;
                    case 1:
                        foreach (Tuple<TypeResource, int> tuple in building.upgrade2ResourceNeeded)
                        {
                            island.resourceManager.changeResourceStock(tuple.First, -tuple.Second);
                        }
                        break;
                    case 2:
                        foreach (Tuple<TypeResource, int> tuple in building.upgrade3ResourceNeeded)
                        {
                            island.resourceManager.changeResourceStock(tuple.First, -tuple.Second);
                        }
                        break;
                }
                    
                island.buildingInfoPresent = false;
                island.createChallengeUpgrade(building);
                island.challengePresent = true;

                //To be cheked
                this.Client.sendData("@30505@" + (100*(building.level + 1)).ToString());
            }
            else
            {
                island.displayPopup("Ce bâtiment est déjà au niveau maximal !", 3);
                //StartCoroutine(island.destroyPopup(island.createPopup("Ce bâtiment est déjà au niveau maximal !"), 3));
            }
        }

        Destroy(GameObject.Find(this.transform.parent.parent.name));
        island.nameBuildingTouchCanvas = string.Empty;
    }


    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime;

    private MetaGesture gesture;
    private Dictionary<int, int> map = new Dictionary<int, int>();

    public override void CancelTouch(TouchPoint touch, bool @return)
    {
        base.CancelTouch(touch, @return);

        map.Remove(touch.Id);
        if (@return)
        {
            TouchHit hit;
            if (!gesture.GetTargetHitResult(touch.Position, out hit)) return;
            map.Add(touch.Id, beginTouch(processCoords(hit.RaycastHit.textureCoord), touch.Tags).Id);
        }
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<MetaGesture>();
        if (gesture)
        {
            gesture.TouchBegan += touchBeganHandler;
            gesture.TouchMoved += touchMovedhandler;
            gesture.TouchCancelled += touchCancelledhandler;
            gesture.TouchEnded += touchEndedHandler;
        }
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        if (gesture)
        {
            gesture.TouchBegan -= touchBeganHandler;
            gesture.TouchMoved -= touchMovedhandler;
            gesture.TouchCancelled -= touchCancelledhandler;
            gesture.TouchEnded -= touchEndedHandler;
        }
    }

    private Vector2 processCoords(Vector2 value)
    {
        return new Vector2(value.x * Width, value.y * Height);
    }

    private void touchBeganHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        map.Add(touch.Id, beginTouch(processCoords(touch.Hit.RaycastHit.textureCoord), touch.Tags).Id);
        //this.OnMouseDownSimulation();
        TouchTime = Time.time;
    }

    private void touchMovedhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        TouchHit hit;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        if (!gesture.GetTargetHitResult(touch.Position, out hit)) return;
        moveTouch(id, processCoords(hit.RaycastHit.textureCoord));
    }

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        endTouch(id);
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
    }

    private void touchCancelledhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        cancelTouch(id);
    }
}