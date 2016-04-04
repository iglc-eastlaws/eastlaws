

namespace Eastlaws.Infrastructure
{
    public static class UIHelpers
    {
        private static string m_TitlePrefix = "شبكة قوانين الشرق | ";
        public static string GetPageTitle(string Title)
        {
            return m_TitlePrefix + Title;
        }
        
    }
}
