using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Utils{
    public static Vector3 GenerateRandomPosition(float distance, Vector3 centerPos) {
        Vector3 position = new Vector3();
        position.x = UnityEngine.Random.Range(0f, distance) * (UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1);
        position.y = Mathf.Sqrt(distance * distance - position.x * position.x) * (UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) + centerPos.y;
        position.x += centerPos.x;
        position.z = centerPos.z;
        return position;
    }

    public static float PositionDistance(Vector3 a, Vector3 b) {
        return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
    }

    public static void GetIntFromInputField(InputField inputField, out int outParam) {
        if (inputField.transform.Find("Text").GetComponent<Text>().text != "") {
            outParam = int.Parse(inputField.transform.Find("Text").GetComponent<Text>().text);
        }
        else {
            outParam = int.Parse(inputField.transform.Find("Placeholder").GetComponent<Text>().text);
        }
    }

    public static void GetFloatFromInputField(InputField inputField, out float outParam) {
        if (inputField.transform.Find("Text").GetComponent<Text>().text != "") {
            outParam = float.Parse(inputField.transform.Find("Text").GetComponent<Text>().text.Replace(".", ","), new CultureInfo("hu-HU").NumberFormat);
        }
        else {
            outParam = float.Parse(inputField.transform.Find("Placeholder").GetComponent<Text>().text.Replace(".", ","), new CultureInfo("hu-HU").NumberFormat);
        }
    }

    public static bool IsUI_Cilck() {
        return Input.mousePosition.x > Screen.width - 300 * Screen.height / 720 || !GameHandler.IsInMission();
    }
}
