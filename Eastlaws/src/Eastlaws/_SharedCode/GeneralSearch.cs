
namespace Eastlaws.ViewModels.Ahkam
{

    public class GeneralSearchVM
    {
        public string Filter { get; set; }

        MatchType Match { get; set; }

        public bool isEmpty
        {
            get
            {
                if (string.IsNullOrEmpty(Filter) || Filter.Trim() == "")
                {
                    return false;
                }
                return true;
            }
        }

    }
}
