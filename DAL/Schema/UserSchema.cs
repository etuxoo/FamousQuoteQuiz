using DAL.Dto;
using Dapper.FluentMap.Mapping;

namespace DAL.Schema
{
    public class UserSchema : EntityMap<UserDto>
    {
        public UserSchema()
        {
            Map(i => i.Id).ToColumn("Id");
            Map(i => i.Username).ToColumn("Username");
            Map(i => i.Password).ToColumn("Password");
        }
    }
}
