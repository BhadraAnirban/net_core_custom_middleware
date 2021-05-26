using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnBoardConsultantWebApi.Utilities
{
    public class TrackUserMiddleware
    {
        public readonly RequestDelegate _next;
        public TrackUser _trackUser;
        public TrackUserMiddleware(RequestDelegate next, TrackUser trackUser)
        {
            _next = next;
            _trackUser = trackUser;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if(!context.Request.Path.Value.ToLower().Contains("api/login") && !context.Request.Path.Value.ToLower().EndsWith("/person"))
            {
                var isValid = _trackUser.VerifyTrackId(context.Request.Headers["user-id"], context.Request.Headers["track-id"]);
                if(!isValid)
                {
                    context.Response.StatusCode = 401;
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }

    public class TrackUser
    {
        private Dictionary<string, string> UserDictionary;
        public TrackUser()
        {
            if(UserDictionary == null)
            {
                UserDictionary = new Dictionary<string, string>();
            }
        }

        public string CreateTrackId(string userId)
        {
            var newId = Guid.NewGuid().ToString();
            if (!UserDictionary.ContainsKey(userId))
            {
                UserDictionary.Add(userId, newId);
            }
            else
            {
                UserDictionary.Remove(userId);
                UserDictionary.Add(userId, newId);
            }
            return newId;
        }
        public string GetTrackId(string userId)
        {
            if (UserDictionary.ContainsKey(userId))
            {
                return UserDictionary[userId];
            }
            else
            {
                return "";
            }
        }

        public bool VerifyTrackId(string userId, string trackId)
        {
            if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(trackId) && UserDictionary.ContainsKey(userId))
            {
                return UserDictionary[userId] == trackId;
            }
            return false;
        }

        public void RemoveTrackId(string userId)
        {
            if (UserDictionary.ContainsKey(userId))
            {
                UserDictionary.Remove(userId);
            }
        }
    }
}
