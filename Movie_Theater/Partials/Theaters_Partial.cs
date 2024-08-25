using Microsoft.AspNetCore.Mvc;
using Movie_Theater.MetaDatas;

namespace Movie_Theater.Models
{
    [ModelMetadataType(typeof(Theaters_MetaData))]
    public partial class Theaters
    {
    }
}
