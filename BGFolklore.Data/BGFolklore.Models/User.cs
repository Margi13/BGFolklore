using BGFolklore.Data.Models.Calendar;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BGFolklore.Data.Models
{
    public class User : IdentityUser
    {
        public IEnumerable<PublicEvent> PublicEvents { get; set; }
        public IEnumerable<Feedback> Reports { get; set; }

        public string Name { get; set; }
    }
}