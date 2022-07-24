using System;

namespace PopStudio.Constant
{
    internal static class Str
    {
        static ILanguage obj;

        public static ILanguage Obj => obj ?? LoadLanguage();

        static ILanguage LoadLanguage()
        {
            return null;
        }
    }
}