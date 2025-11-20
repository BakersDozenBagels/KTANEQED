using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(KMBombModule))]
public class LFAInjector : MonoBehaviour
{
    [SerializeField]
    private string _loggingTag;
    [SerializeField]
    private TextAsset _handlers;

    private void Start()
    {
        var module = GetComponent<KMBombModule>();
        var name = module.ModuleDisplayName;
        var id = module.ModuleType;

        IllegalStringException.ThrowIfBad(_loggingTag, "_loggingTag");
        IllegalStringException.ThrowIfBad(name, "name");
        IllegalStringException.ThrowIfBad(id, "id");

        Debug.Log(
            "BEGIN LFA CODE INJECTION: " + name + "\n" +
            "[BombGenerator] Generating bomb with seed 0\n" +
            "[BombGenerator] Generator settings: Time: 1, NumStrikes: 1, FrontFaceOnly: True\n" +
            "1 Pools:\n" +
            "[1DChess] Count: 1\n" +
            "[BombGenerator] Selected 1DChess (. (.))\n" +
            "[SerialNumber] Randomizing Serial Number: 000AA0\n" +
            "[1D Chess #0] <script>if(!parseData.find(p=>p.loggingTag===\"" + _loggingTag + "\")){console.log(\"Injecting LFA support: " + name + "\");parseData.push({displayName:\"" + name + "\",moduleID:\"" + id + "\",loggingTag:\"" + _loggingTag + "\",matches:" + GetCode() + "});const x=parseLog;parseLog=function(o){let l=o.log.split(/\\r?\\n/);let n=0;let c=\"\";while(n<l.length){if(l[n]===\"BEGIN LFA CODE INJECTION: " + name + "\"){n+=11;}else{c+=l[n]+\"\\n\";n++;}};o.log=c;x(o);};parseLog({log:lines.join(\"\\n\")});}</script>\n" +
            "[Tweaks] LFABombInfo 1\n" +
            "{\"serial\":\"000AA0\",\"displayNames\":{\"1DChess\":\"\"},\"ids\":{},\"anchors\":[[0,0]],\"modules\":[\"1DChess -\"],\"timestamp\":\"2024-11-09T14:00:09.2319619-05:00\"}\n" +
            "END LFA CODE INJECTION: " + name);
    }

    private string GetCode()
    {
        var compact = _handlers.text.Split('\r', '\n').Select(s => s.Trim()).Join("");
        if (compact.EndsWith(";"))
            return compact.Substring(0, compact.Length - 1);
        return compact;
    }

    private class IllegalStringException : Exception
    {
        public IllegalStringException(string message) : base(message) { }

        public static void ThrowIfBad(string s, string name)
        {
            if (IsIllegal(s))
                throw new IllegalStringException("The string " + name + " is ilegal. Its value: " + s);
        }

        public static bool IsIllegal(string s)
        {
            return s == null || s == "" || s.Contains('\"') || s.Contains('\\') || s.Contains("</script>");
        }
    }
}
