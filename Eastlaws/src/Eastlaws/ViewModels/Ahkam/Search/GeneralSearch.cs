using Eastlaws.Entities;
using System.Collections.Generic;

namespace Eastlaws.ViewModels.Ahkam
{
    public class GeneralSearchVM : SimpleInputSearch
    {
        public IEnumerable<AhkamPresentation> SearchResault { get; set; }
    }
}
