using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using BrightIdeas.Models;


namespace BrightIdeas.Models
{
    public class User : BaseEntity{
 
        public int UserId {get;set;}
        public string Name{get;set;}              
        public string Alias {get;set;}
        public string Email{get;set;}        
        public string Password{get;set;}

        [InverseProperty("Creator")]
        public List<Post> MyPosts {get;set;}
        public List<Like> Likes {get;set;}   
        public User(){
        MyPosts = new List<Post>();    
        Likes = new List<Like>();    
        }
    }

    public class Post : BaseEntity{
        
        public int PostId {get;set;}
        public string Content {get;set;}

        [ForeignKey("Creator")]
        public int CreatorId {get;set;}
        public User Creator {get;set;}
        public List<Like> Likes {get;set;}
        public Post(){
            Likes = new List<Like>();
        }
    }

    public class Like : BaseEntity{
        
        public int UserId {get;set;}
        public int LikeId {get;set;}
        public int PostId {get;set;}
        public User User {get;set;}
        public Post Post{get;set;} 
    }

    public abstract class BaseEntity {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt {get;set;}
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt {get;set;}
    }
}