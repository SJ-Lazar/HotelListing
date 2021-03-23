using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Models
{
    public class CreateCountryDTO
    {  
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage ="Country Name Is Too Long")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 6, ErrorMessage = "Short Country Name Is Too Long")]
        public string ShortName { get; set; }
    }

    public class CountryDTO: CreateCountryDTO
    {
        public int Id { get; set; }
        //Navigations
        public IList<HotelDTO> Hotels{ get; set; }
    }
}
