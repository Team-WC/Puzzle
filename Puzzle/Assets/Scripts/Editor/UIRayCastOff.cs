using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UIRayCastOff : MonoBehaviour
{
    public static void OffRayCastExceptButton()
    {
        GameObject[] selections = Selection.gameObjects;
        
        if(selections == null || selections.Length == 0)
        {
            Debug.LogError("선택된 Hierachy 오브젝트가 없습니다.");
            return;
        }

        foreach (var select in selections)
        {
            RayCastOffTargetsChilds(select.transform);

            //Debug.Log(select.name + " GameObject's childs raycast off complete.");
        }

        EditorUtility.DisplayDialog("Complete", "UI RayCast Off Process Completed!", "OK");
    }

    public static void RayCastOffTargetsChilds(Transform target)
    {
        foreach (var child in target.GetComponentsInChildren<Text>(true))
        {
            if (child.GetComponent<Button>())
            {
                continue;
            }
            else
            {
                child.GetComponent<Text>().raycastTarget = false;
            }
        }

        foreach (var child in target.GetComponentsInChildren<Image>(true))
        {
            if (child.GetComponent<Button>())
            {

            }
            else
            {
                child.GetComponent<Image>().raycastTarget = false;
            }
        }
    }

    private static void ShowErrorLog(System.Exception e)
    {
        Debug.LogError(e.Message);
    }
}
