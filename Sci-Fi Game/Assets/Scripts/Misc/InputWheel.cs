using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;  
using static NewVehicleGround;
using static VehicleGround;

public static class InputWheel
{    
    static LogitechGSDK.LogiControllerPropertiesData properties;

    static LogitechGSDK.DIJOYSTATE2ENGINES rec;


    public static float Accelerator
    {
        get
        {
            if (CheckInitialised ())
            {
                return rec.lY.Map ( -32768, 32767, 1.0f, 0.0f );
            }
            else
            {
                Debug.LogError ( "Wheel not initialised. Returning default." );
                return 0.0f;
            }
        }
    }

    public static float Brake
    {
        get
        {
            if (CheckInitialised ())
            {
                return rec.lRz.Map ( -32768, 32767, 1.0f, 0.0f );
            }
            else
            {
                Debug.LogError ( "Wheel not initialised. Returning default." );
                return 0.0f;
            }
        }
    }

    public static float Clutch
    {
        get
        {
            if (CheckInitialised ())
            {
                return rec.rglSlider[0].Map ( -32768, 32767, 1.0f, 0.0f );
            }
            else
            {
                Debug.LogError ( "Wheel not initialised. Returning default." );
                return 0.0f;
            }
        }
    }

    public static float Steering
    {
        get
        {
            if (CheckInitialised ())
            {
                return rec.lX.Map ( -32768, 32767, -1.0f, 1.0f );
            }
            else
            {
                Debug.LogError ( "Wheel not initialised. Returning default." );
                return 0.0f;
            }
        }
    }

    public static bool ShiftDown
    {
        get
        {
            if (CheckInitialised ())
            {
                return rec.rgbButtons[5] == 128;
            }
            else
            {
                Debug.LogError ( "Wheel not initialised. Returning default." );
                return false;
            }
        }
    }

    public static bool ShiftUp
    {
        get
        {
            if (CheckInitialised ())
            {
                return rec.rgbButtons[4] == 128;
            }
            else
            {
                Debug.LogError ( "Wheel not initialised. Returning default." );
                return false;
            }
        }
    }

    public static int GearInt
    {
        get
        {
            if (CheckInitialised ())
            {
                if (rec.rgbButtons[12] == 128)
                {
                    return 1;
                }
                else if (rec.rgbButtons[13] == 128)
                {
                    return 2;
                }
                else if (rec.rgbButtons[14] == 128)
                {
                    return 3;
                }
                else if (rec.rgbButtons[15] == 128)
                {
                    return 4;
                }
                else if (rec.rgbButtons[16] == 128)
                {
                    return 5;
                }
                else if (rec.rgbButtons[17] == 128)
                {
                    return 6;
                }
                else if (rec.rgbButtons[18] == 128)
                {
                    return -1;
                }
                else 
                {
                    return 0;
                }
            }
            else
            {
                Debug.LogError ( "Wheel not initialised. Returning default." );
                return 0;
            }
        }
    }

    public static Gear GearEnum
    {
        get
        {
            if (CheckInitialised ())
            {
                if (rec.rgbButtons[12] == 128)
                {
                    return Gear.First;
                }
                else if (rec.rgbButtons[13] == 128)
                {
                    return Gear.Second;
                }
                else if (rec.rgbButtons[14] == 128)
                {
                    return Gear.Third;
                }
                else if (rec.rgbButtons[15] == 128)
                {
                    return Gear.Fourth;
                }
                else if (rec.rgbButtons[16] == 128)
                {
                    return Gear.Fifth;
                }
                else if (rec.rgbButtons[17] == 128)
                {
                    return Gear.Sixth;
                }
                else if (rec.rgbButtons[18] == 128)
                {
                    return Gear.Reverse;
                }
                else
                {
                    return Gear.Neutral;
                }
            }
            else
            {
                Debug.LogError ( "Wheel not initialised. Returning default." );
                return Gear.Neutral;
            }
        }
    }

    public static bool CheckInitialised ()
    {
        if (!Application.isPlaying) return false;

        if (LogitechGSDK.LogiUpdate () && LogitechGSDK.LogiIsConnected ( 0 ))
        {           
            rec = LogitechGSDK.LogiGetStateUnity ( 0 );
            return true;
        }

        return false;
    }
}
