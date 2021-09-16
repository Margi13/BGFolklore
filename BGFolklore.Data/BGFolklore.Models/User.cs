using BGFolklore.Data.Models.Calendar;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BGFolklore.Data.Models
{
    public class User : IdentityUser
    {
        public ICollection<PublicEvent> PublicEvents { get; set; }
        public ICollection<Feedback> Reports { get; set; }

        public string Name { get; set; }
    }
}