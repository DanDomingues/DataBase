using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevEnumField : DevEnumFieldBase
{
    //Swap for desired enum to be checked for at each case
    protected override Type enumType => typeof(Enum);
}
