using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public static class ExtensionMethods
{
    public static void Nudge(this Button button, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        button.transform.DOPunchPosition(position, .2f, 2, .2f);
        button.transform.DOPunchRotation(rotation, .2f, 2, .2f);
        button.transform.DOPunchScale(scale, .2f, 2, .2f);
    }
}