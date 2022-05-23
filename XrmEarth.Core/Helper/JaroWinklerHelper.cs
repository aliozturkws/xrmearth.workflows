using F23.StringSimilarity;

namespace XrmEarth.Core
{
    public class JaroWinklerHelper
    {
        public static double Similarity(string value1,string value2)
        {
            JaroWinkler jw = new JaroWinkler();

            return jw.Similarity(value1, value2);
        }
    }
}
