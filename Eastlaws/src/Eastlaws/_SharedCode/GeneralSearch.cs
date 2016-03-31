
namespace Eastlaws
{
    public class GeneralSearchVM
    {
        public string Filter { get; set; }

        public TextSearchMatch Match { get; set; }

        public bool isEmpty
        {
            get
            {
                if (string.IsNullOrEmpty(Filter) || Filter.Trim() == "")
                {
                    return false;
                }
                if(Filter.Length < 3)
                {
                    return false;
                }
                return true;
            }
        }

    }
}
