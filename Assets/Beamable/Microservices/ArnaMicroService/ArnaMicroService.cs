using Beamable.Server;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Beamable.Server
{
    [Microservice("ArnaMicroService")]
    public class ArnaMicroService : Microservice
    {
        [System.Serializable]
        public class Room
        {
            public string id;
            public string name;
            public int maxPlayer;

            public bool IsPrivate => string.IsNullOrEmpty(_password);

            private string _password;

            public List<long> players;

            public Room(string id, string name, int maxPlayer, string password, long author)
            {
                this.id = id;
                this.name = name;
                this.maxPlayer = maxPlayer;
                _password = password;

                players = new List<long>();
                players.Add(author);
            }

            public bool TryEnter(string pw)
            {
                return pw == _password;
            }

            public bool TryEnter(long userId, string pw)
            {
                if (players.Contains(userId) || pw != _password)
                    return false;

                players.Add(userId);
                return true;
            }

            public bool TryQuit(long userId)
            {
                if (!players.Contains(userId))
                    return false;

                players.Remove(userId);
                return true;
            }
        }

        public Dictionary<string, Room> Rooms { get; private set; }
        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        [ClientCallable]
        public void StartSession()
        {
            Rooms = new Dictionary<string, Room>();
        }

        [ClientCallable]
        public string CreateRoom(long user, string name, bool isPrivate)
        {
            string roomId = GenerateRoomId(10);
            var room = new Room(roomId, name, 4, isPrivate ? "" : roomId, user);
            Rooms.Add(room.id, room);
            return room.id;
        }

        [ClientCallable]
        public bool JoinRoomPublic(string id)
        {
            return Rooms.ContainsKey(id) && Rooms[id].TryEnter("");
        }

        [ClientCallable]
        public string JoinRoom(string password)
        {
            var room = Rooms.Values.FirstOrDefault(e => e.TryEnter(password));
            return room == null ? null : room.id;
        }

        [ClientCallable]
        public bool QuitRoom(long user, string id)
        {
            if (Rooms.ContainsKey(id) && Rooms[id].TryQuit(user))
            {
                if (Rooms[id].players.Count == 0)
                    Rooms.Remove(id);
            }
            return false;
        }

        [ClientCallable]
        public string[] GetRooms()
        {
            return Rooms.Values.Where(e => !e.IsPrivate).Select(e => e.id).ToArray();
        }

        private string GenerateRoomId(int length)
        {
            string roomId;

            while (Rooms.ContainsKey(RandomizeString(length, out roomId)))
                continue;

            return roomId;
        }

        private string RandomizeString(int length, out string gString)
        {
            var stringChars = new char[length];
            var random = new Random();
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = CHARS[random.Next(CHARS.Length)];
            }
            gString = new string(stringChars);
            return gString;
        }
    }
}