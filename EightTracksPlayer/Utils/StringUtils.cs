using System;
using System.Collections.Generic;
using System.Linq;

namespace EightTracksPlayer.Utils
{
    public class StringUtils
    {
        public static string[] GetStringInBetween(string strBegin, string strEnd, string strSource, bool includeBegin,
                                                  bool includeEnd)
        {
            string[] result = {"", ""};
            int iIndexOfBegin = strSource.IndexOf(strBegin);
            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired
                if (includeBegin)
                    iIndexOfBegin -= strBegin.Length;
                strSource = strSource.Substring(iIndexOfBegin
                                                + strBegin.Length);
                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    // include the End string if desired
                    if (includeEnd)
                        iEnd += strEnd.Length;
                    result[0] = strSource.Substring(0, iEnd);
                    // advance beyond this segment
                    if (iEnd + strEnd.Length < strSource.Length)
                        result[1] = strSource.Substring(iEnd
                                                        + strEnd.Length);
                }
            }
            else
                // stay where we are
                result[1] = strSource;
            return result;
        }

        public static string TagListToString(List<string> list)
        {
            return TagListToString(list, " ");
        }

        public static string TagListToString(List<string> list, String tagSeparator)
        {
            return list.Aggregate("", (curr, next) => curr + tagSeparator + next).Trim(tagSeparator.ToCharArray());
        }
    }
}