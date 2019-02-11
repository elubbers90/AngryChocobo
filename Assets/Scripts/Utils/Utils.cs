using System.Collections.Generic;

public class Utils {
    public static List<int> ToIntList(string text) {
        List<int> result = new List<int>();
        string[] split = text.Split(',');
        foreach(string number in split) {
            int x;
            if (int.TryParse(number, out x)) {
                result.Add(x);
            }
        }
        return result;
    }

    public static string IntListToString(List<int> list) {
        string result = "";
        int index = 0;
        foreach (int number in list) {
            if (index != 0) {
                result += ",";
            }
            index++;
            result += number;
        }
        return result;
    }
}