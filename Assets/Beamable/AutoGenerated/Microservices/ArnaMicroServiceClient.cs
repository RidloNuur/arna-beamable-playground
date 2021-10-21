//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Beamable.Server.Clients
{
    using System;
    using Beamable.Platform.SDK;
    using Beamable.Server;
    
    
    /// <summary> A generated client for <see cref="Beamable.Server.ArnaMicroService"/> </summary
    public sealed class ArnaMicroServiceClient : Beamable.Server.MicroserviceClient
    {
        
        /// <summary>
        /// Call the StartSession method on the ArnaMicroService microservice
        /// <see cref="Beamable.Server.ArnaMicroService.StartSession"/>
        /// </summary>
        public Beamable.Common.Promise<Beamable.Common.Unit> StartSession()
        {
            string[] serializedFields = new string[0];
            return this.Request<Beamable.Common.Unit>("ArnaMicroService", "StartSession", serializedFields);
        }
        
        /// <summary>
        /// Call the CreateRoom method on the ArnaMicroService microservice
        /// <see cref="Beamable.Server.ArnaMicroService.CreateRoom"/>
        /// </summary>
        public Beamable.Common.Promise<string> CreateRoom(long user, string name, bool isPrivate)
        {
            string serialized_user = this.SerializeArgument<long>(user);
            string serialized_name = this.SerializeArgument<string>(name);
            string serialized_isPrivate = this.SerializeArgument<bool>(isPrivate);
            string[] serializedFields = new string[] {
                    serialized_user,
                    serialized_name,
                    serialized_isPrivate};
            return this.Request<string>("ArnaMicroService", "CreateRoom", serializedFields);
        }
        
        /// <summary>
        /// Call the JoinRoomPublic method on the ArnaMicroService microservice
        /// <see cref="Beamable.Server.ArnaMicroService.JoinRoomPublic"/>
        /// </summary>
        public Beamable.Common.Promise<bool> JoinRoomPublic(string id)
        {
            string serialized_id = this.SerializeArgument<string>(id);
            string[] serializedFields = new string[] {
                    serialized_id};
            return this.Request<bool>("ArnaMicroService", "JoinRoomPublic", serializedFields);
        }
        
        /// <summary>
        /// Call the JoinRoom method on the ArnaMicroService microservice
        /// <see cref="Beamable.Server.ArnaMicroService.JoinRoom"/>
        /// </summary>
        public Beamable.Common.Promise<string> JoinRoom(string password)
        {
            string serialized_password = this.SerializeArgument<string>(password);
            string[] serializedFields = new string[] {
                    serialized_password};
            return this.Request<string>("ArnaMicroService", "JoinRoom", serializedFields);
        }
        
        /// <summary>
        /// Call the QuitRoom method on the ArnaMicroService microservice
        /// <see cref="Beamable.Server.ArnaMicroService.QuitRoom"/>
        /// </summary>
        public Beamable.Common.Promise<bool> QuitRoom(long user, string id)
        {
            string serialized_user = this.SerializeArgument<long>(user);
            string serialized_id = this.SerializeArgument<string>(id);
            string[] serializedFields = new string[] {
                    serialized_user,
                    serialized_id};
            return this.Request<bool>("ArnaMicroService", "QuitRoom", serializedFields);
        }
        
        /// <summary>
        /// Call the GetRooms method on the ArnaMicroService microservice
        /// <see cref="Beamable.Server.ArnaMicroService.GetRooms"/>
        /// </summary>
        public Beamable.Common.Promise<string[]> GetRooms()
        {
            string[] serializedFields = new string[0];
            return this.Request<string[]>("ArnaMicroService", "GetRooms", serializedFields);
        }
    }
}
