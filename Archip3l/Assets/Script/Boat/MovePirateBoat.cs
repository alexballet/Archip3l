using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using System.Collections.Generic;
using TouchScript;
using TouchScript.InputSources;
using TouchScript.Hit;

public class MovePirateBoat : InputSource
{
    public Vector2 speed = new Vector2(0.2f, 0.2f);
    public Vector2 direction = new Vector2(0, 1);
    private Vector2 movement;

    private float lifeTime;
    public ParticleSystem explosionEffect;
    public ParticleSystem sinkEffect;

    void Awake()
    {
        this.lifeTime = 20f;
        Destroy(gameObject, this.lifeTime);
        gameObject.SetActive(false);
    }

    public void init(Vector3 initPosition, Vector3 targetPosition)
    {
        System.Random rnd = new System.Random();

        //Debug.Log(initPosition.ToString());
        this.direction = targetPosition - initPosition;

        transform.position = initPosition;

        float alpha = -90 + (Mathf.Rad2Deg * Mathf.Atan2(targetPosition.y - initPosition.y, targetPosition.x - initPosition.x)); //, transform.position.y, targetPosition.x - transform.position.x));
        transform.rotation = Quaternion.Euler(0f, 0f, alpha);

        float boatSpeed = rnd.Next(3, 10) / 1000f;
        this.speed = new Vector2(boatSpeed, boatSpeed);

        float boatScale = 10 + rnd.Next(-3, 4);
        transform.localScale = new Vector3(boatScale, boatScale, 0);

        gameObject.SetActive(true);
    }

    void Update()
    {
        movement = new Vector2(transform.position.x + this.speed.x * direction.x, transform.position.y + this.speed.y * direction.y);
        if (System.Math.Abs(this.transform.position.x) > 700 || System.Math.Abs(this.transform.position.y) > 500)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(transform.position.ToString());
        this.transform.position = movement;
    }
    IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
    private Vector3 getNewBoatPosition()
    {
        System.Random rnd = new System.Random();
        switch (rnd.Next(1, 5))
        {
            case 1:
                return new Vector3(rnd.Next(-500, -430), rnd.Next(0, 400), 0);
            case 2:
                return new Vector3(rnd.Next(430, 500), rnd.Next(0, 400), 0);
            case 4:
                return new Vector3(rnd.Next(-500, -430), rnd.Next(-400, 0), 0);
            case 3:
                return new Vector3(rnd.Next(430, 500), rnd.Next(-400, 0), 0);
        }
        return new Vector3(0, 0);
    }
    private Vector2 getNewBoatDirection()
    {
        Vector2 direction = new Vector2();

        return direction;
    }
    void OnMouseDownSimulation()
    {
        this.destroyBoat(true);
    }

    public void destroyBoat(bool touched)
    {
        //PROBLEM: animation makes island disappear

        /*if(touched)
        {
            SoundPlayer.Instance.playBoatSinkSound();
            Instantiate(sinkEffect, transform.position, Quaternion.identity);
        }
        else
        {
            SoundPlayer.Instance.playExplosionOneSound();
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }*/
            
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.name.Contains("ExchangeResourceAnimation"))
        {
            this.destroyBoat(true);
        }
        if(collider.name.Contains("PirateBoat"))
        {
            this.destroyBoat(true);
        }
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
        if (TouchTime == 0 && !MinorIsland.exchangePerforming)
        {
            TouchTime = Time.time;
        }

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
        if (Time.time - TouchTime < 0.5)
        {
            TouchTime = 0;
            this.OnMouseDownSimulation();
        }
        else if (Time.time - TouchTime < 1.5)
        {
            TouchTime = 0;
        }
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