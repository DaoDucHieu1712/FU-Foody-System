﻿using FFS.Application.Entities.Enum;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    public class ApplicationUser : IdentityUser
    {

		public ApplicationUser() {

			Locations = new HashSet<Location>();
			Reacts = new HashSet<React>();
			Comments = new HashSet<Comment>();	
			Orders = new HashSet<Order>();
			Notifications = new HashSet<Notification>();
			Posts = new HashSet<Post>();
			UserDiscounts = new HashSet<UserDiscount>();
			Chats = new HashSet<Chat>();
			Messages = new HashSet<Message>();
		}

        [Key]
        public override string Id { get; set; }
        public string? Avatar { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public bool? Allow { get; set; }
        public DateTime? BirthDay { get; set; }
        public StatusUser Status { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<React> Reacts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Order> Orders { get;set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
		public virtual ICollection<UserDiscount> UserDiscounts { get; set; }
		public virtual ICollection<Chat> Chats { get; set; }
		public virtual ICollection<Message> Messages { get; set; }
    }
}
