//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
using UnityEngine;
//using static VehicleGround;

public class VehicleGround : MonoBehaviour
{
    //    public enum Gears { Reverse, Neutral, First, Second, Third, Fourth, Fifth, Sixth }

    //    public float maximumRevs = 7000.0f;
    //    public float currentRevs = 0.0f;
    //    public float targetRevs = 0.0f;
    //    public Gears CurrentGear = Gears.Neutral;
    //    public float engineTorque = 1500.0f;
    //    public float inGearDecelerationMultiplier = 0.2f;
    //    [Space]
    //    public float currentEngineTorque = 0.0f;   // TODO: I think this will eventually translate into a multiple between 0, 1 which will multiplty the maxEnginePower to give a torque to the wheels.
    //    public float MPH = 0.0f;
    //    public float MPHNormalised { get { return Mathf.Lerp ( 0.0f, 1.0f, MPH / maxMPH ); } }
    //    public float vehicleDrag = 10.0f;
    //    public float maxMPH = 128.0f;   
    //    //public float maxEngineTorque = 3500.0f;

    //    public Transform revLineUITransform;
    //    public Transform speedLineUITransform;
    //    public TextMeshProUGUI mphText;
    //    public TextMeshProUGUI gearText;
    //    public TextMeshProUGUI timeText;
    //    public TextMeshProUGUI actualMPH;
    //    public AudioSource audioSource;
    //    public Vector2 pitches = new Vector2 ();

    //    public bool automaticGearbox = false;
    //    public float automaticGearboxMultiplier = 0.65f;

    //    private Dictionary<Gears, VehicleGear> gears = new Dictionary<Gears, VehicleGear> ()
    //    {
    //        { Gears.Reverse, new VehicleGear ( Gears.Reverse, 0.200f, 0.0234f, 0.1f, 0.65f ) },
    //        { Gears.Neutral, new VehicleGear ( Gears.Neutral, 0.100f, 0.0f, 0.0f, 500.0f ) },

    //        { Gears.First, new VehicleGear   ( Gears.First,   0.200f, 0.0234f, 0.2f, 1.00f ) },
    //        { Gears.Second, new VehicleGear  ( Gears.Second,  0.185f, 0.0468f, 0.36f, 0.70f ) },

    //        { Gears.Third, new VehicleGear   ( Gears.Third,   0.170f, 0.0703f, 0.52f, 0.49f ) },
    //        { Gears.Fourth, new VehicleGear  ( Gears.Fourth,  0.155f, 0.0937f, 0.68f, 0.34f ) },
    //        { Gears.Fifth, new VehicleGear   ( Gears.Fifth,   0.140f, 0.1171f, 0.84f, 0.24f ) },
    //        { Gears.Sixth, new VehicleGear   ( Gears.Sixth,   0.125f, 0.1328f, 1.00f, 0.16f ) }
    //    };

    //    [SerializeField] private AnimationCurve gearBoxMaxSpeedRatios;

    //    private Vector3 prevPos = new Vector3 ();
    //    [SerializeField] private float mphCounterInterval = 0.05f;
    //    private float currentMphCounter = 0.0f;
    //    private float lastMphCounter = 0.0f;

    //    private bool count = false;
    //    private bool hasCounted = false;
    //    private float counter = 0.0f;

    //    private IEnumerator CalcSpeed ()
    //    {
    //        while (true)
    //        {
    //            yield return new WaitForSeconds ( 0.1f );
    //            MPH = GetComponent<Rigidbody> ().velocity.magnitude * 2.237f;
    //            actualMPH.text = MPH.ToString ( "00" ) + " mph";

    //        }
    //    }

    //    private void Start ()
    //    {
    //        StartCoroutine ( CalcSpeed () );

    //        prevPos = transform.position;

    //        float f = 0.1666f;
    //        gears[Gears.First].maxMPHMultipler = gearBoxMaxSpeedRatios.Evaluate ( f );

    //        f += 0.1666f;
    //        gears[Gears.Second].maxMPHMultipler = gearBoxMaxSpeedRatios.Evaluate ( f );

    //        f += 0.1666f;
    //        gears[Gears.Third].maxMPHMultipler = gearBoxMaxSpeedRatios.Evaluate ( f );

    //        f += 0.1666f;
    //        gears[Gears.Fourth].maxMPHMultipler = gearBoxMaxSpeedRatios.Evaluate ( f );

    //        f += 0.1666f;
    //        gears[Gears.Fifth].maxMPHMultipler = gearBoxMaxSpeedRatios.Evaluate ( f );

    //        f = 1.0f;
    //        gears[Gears.Sixth].maxMPHMultipler = gearBoxMaxSpeedRatios.Evaluate ( f );
    //    }

    //    private void OnGearChange ()
    //    {

    //    }

    //    private void ShiftDown ()
    //    {
    //        if ((int)CurrentGear == 0) return;
    //        CurrentGear = gears[(Gears)(((int)CurrentGear) - 1)].gear;
    //        OnGearChange ();
    //    }

    //    private void ShiftUp ()
    //    {
    //        if ((int)CurrentGear == 7) return;
    //        CurrentGear = gears[(Gears)(((int)CurrentGear) + 1)].gear;
    //        OnGearChange ();
    //    }

    //    private void Update ()
    //    {
    //        if (count && !hasCounted)
    //        {
    //            if (MPH >= 60.0f)
    //            {
    //                hasCounted = true;
    //                timeText.text = counter.ToString ( "00.00" ) +"  -  " + "0-60";
    //            }
    //            else
    //            {
    //                counter += Time.deltaTime;
    //                timeText.text = counter.ToString ( "00.00" );
    //            }            
    //        }
    //        //targetRevs = gears[CurrentGear].revMinimum * maximumRevs;

    //        if (Input.GetKeyDown ( KeyCode.A ))
    //        {
    //            ShiftDown ();
    //        }

    //        if (Input.GetKeyDown ( KeyCode.D ))
    //        {
    //            ShiftUp ();
    //        }

    //        gearText.text = CurrentGear.ToString ();

    //        if (Input.GetKey ( KeyCode.W ))
    //        {
    //            count = true;
    //            if (CurrentGear != Gears.Neutral)
    //            {
    //                if(currentEngineTorque >= gears[CurrentGear].maxMPHMultipler * maxMPH)
    //                {
    //                    currentEngineTorque -= Time.deltaTime * engineTorque * 5.0f;
    //                }
    //                else
    //                {

    //                    currentEngineTorque += Time.deltaTime * gears[CurrentGear].maxTorqueMultiplier * engineTorque;

    //                    if (currentEngineTorque >= gears[CurrentGear].maxMPHMultipler * maxMPH)
    //                    {
    //                        currentEngineTorque = gears[CurrentGear].maxMPHMultipler * maxMPH;
    //                    }
    //                }

    //                targetRevs = Mathf.Lerp ( 0.0f, maximumRevs, Mathf.InverseLerp ( 0.0f, gears[CurrentGear].maxMPHMultipler * maxMPH, currentEngineTorque ) );
    //            }
    //            else
    //            {
    //                currentEngineTorque -= Time.deltaTime * vehicleDrag;

    //                if (currentEngineTorque <= gears[CurrentGear].minMPHMultiplier * maxMPH)
    //                    currentEngineTorque = gears[CurrentGear].minMPHMultiplier * maxMPH;

    //                targetRevs += Time.deltaTime * engineTorque * gears[CurrentGear].maxTorqueMultiplier;

    //                if (targetRevs >= maximumRevs)
    //                    targetRevs = maximumRevs - (maximumRevs * 0.05f);
    //            }
    //        }
    //        else
    //        {

    //            if (CurrentGear != Gears.Neutral)
    //            {
    //                if (currentEngineTorque >= gears[CurrentGear].maxMPHMultipler * maxMPH)
    //                {
    //                    currentEngineTorque -= Time.deltaTime * engineTorque * 5.0f;
    //                }
    //                else
    //                {
    //                    currentEngineTorque -= Time.deltaTime * vehicleDrag;

    //                    if (currentEngineTorque <= gears[CurrentGear].minMPHMultiplier * maxMPH)
    //                        currentEngineTorque = gears[CurrentGear].minMPHMultiplier * maxMPH;
    //                }


    //                targetRevs = Mathf.Lerp ( 0.0f, maximumRevs, Mathf.InverseLerp ( 0.0f, gears[CurrentGear].maxMPHMultipler * maxMPH, currentEngineTorque ) );
    //            }
    //            else
    //            {
    //                currentEngineTorque -= Time.deltaTime * vehicleDrag;

    //                if (currentEngineTorque <= gears[CurrentGear].minMPHMultiplier * maxMPH)
    //                    currentEngineTorque = gears[CurrentGear].minMPHMultiplier * maxMPH;

    //                targetRevs -= Time.deltaTime * engineTorque * gears[CurrentGear].maxTorqueMultiplier;

    //                if (targetRevs <= Mathf.Lerp ( 0.0f, maximumRevs, Mathf.InverseLerp ( 0.0f, gears[Gears.First].maxMPHMultipler * maxMPH, gears[Gears.First].minMPHMultiplier * maxMPH ) ) / 2.0f)
    //                    targetRevs = Mathf.Lerp ( 0.0f, maximumRevs, Mathf.InverseLerp ( 0.0f, gears[Gears.First].maxMPHMultipler * maxMPH, gears[Gears.First].minMPHMultiplier * maxMPH ) / 2.0f );
    //            }
    //        }

    //        if (automaticGearbox)
    //        {
    //            if (CurrentGear != Gears.Neutral && CurrentGear != Gears.Reverse)
    //            {
    //                if (CurrentGear != Gears.Sixth)
    //                {
    //                    if (currentEngineTorque > gears[CurrentGear].maxMPHMultipler * maxMPH * automaticGearboxMultiplier)
    //                    {
    //                        ShiftUp ();
    //                    }
    //                }

    //                if (CurrentGear != Gears.First)
    //                {
    //                    if (currentEngineTorque < gears[(Gears)(int)CurrentGear - 1].maxMPHMultipler * maxMPH * automaticGearboxMultiplier)
    //                    {
    //                        ShiftDown ();
    //                    }
    //                }
    //            }
    //        }

    //        //if (Input.GetKey(KeyCode.W))
    //        //{
    //        //    targetRevs += gears[CurrentGear].accelerationMultiplier * Time.deltaTime * engineTorque;

    //        //    if(targetRevs >= maximumRevs)
    //        //    {
    //        //        targetRevs = maximumRevs - (maximumRevs * 0.05f);
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    if (CurrentGear == Gear.Neutral)
    //        //    {
    //        //        targetRevs -= Time.deltaTime * engineTorque * ((CurrentGear == Gear.Neutral) ? 1.0f : inGearDecelerationMultiplier);
    //        //    }
    //        //    else
    //        //    {
    //        //        // TODO: Decelerate revs by vehicle speed slowing down 
    //        //    }

    //        //    targetRevs -= Time.deltaTime * engineTorque * ((CurrentGear == Gear.Neutral) ? 1.0f : inGearDecelerationMultiplier);

    //        //    if (targetRevs <= gears[CurrentGear].revMinimum * maximumRevs)
    //        //        targetRevs = gears[CurrentGear].revMinimum * maximumRevs;
    //        //}

    //        currentRevs = targetRevs;
    //        audioSource.pitch = Mathf.Lerp ( pitches.x, pitches.y, currentRevs / maximumRevs );

    //        Debug.Log ( ((float)((int)CurrentGear - 1) * 0.8f) );

    //        float f = 1 - (0.05f * (float)((int)CurrentGear - 1));

    //        if((int)CurrentGear >= 2)
    //        {
    //            audioSource.pitch = audioSource.pitch * f;
    //        }

    //        revLineUITransform.localEulerAngles = new Vector3 ( 0.0f, 0.0f, Mathf.Lerp ( 0.0f, -225.0f, currentRevs / maximumRevs ) );
    //        speedLineUITransform.localEulerAngles = new Vector3 ( 0.0f, 0.0f, Mathf.Lerp ( 0.0f, -321.428f, currentEngineTorque / maxMPH ) );
    //        mphText.text = currentEngineTorque.ToString ( "00" ) + " mph";

    //        GetComponent<Rigidbody> ().velocity = new Vector3 ( GetComponent<Rigidbody> ().velocity.x, GetComponent<Rigidbody> ().velocity.y, currentEngineTorque );     
    //    }

}

//public class VehicleGear
//{
//    public Gears gear;
//    public float revMinimum;
//    public float maxTorqueMultiplier;
//    public float minMPHMultiplier;
//    public float maxMPHMultipler;

//    public VehicleGear (Gears gear, float revMinimum, float minMPHMultiplier, float maxMPHMultipler, float maxTorqueMultiplier)
//    {
//        this.gear = gear;
//        //this.revMinimum = revMinimum;
//        this.maxTorqueMultiplier = maxTorqueMultiplier;
//        this.minMPHMultiplier = minMPHMultiplier;
//        this.maxMPHMultipler = maxMPHMultipler;
//    }
//}
