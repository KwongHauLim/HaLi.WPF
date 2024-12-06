using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaLi.WPF.Helpers;

internal static class Helper
{
    public static void CopyProperties<T,U>(this T source, U target)
    {
        var PTs = typeof(T).GetProperties();
        var PUs = typeof(U).GetProperties();
        foreach (var PT in PTs)
        {
            var PU = PUs.FirstOrDefault(p => p.Name == PT.Name);
            if (PU == null)
                continue;
            PU.SetValue(target, PT.GetValue(source));
        }
    }
}
