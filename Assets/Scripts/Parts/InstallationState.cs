/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>
using UnityEngine;

internal enum InstallationState
{
    NotInstalled,
    Step1Installed,
    Installed,
}