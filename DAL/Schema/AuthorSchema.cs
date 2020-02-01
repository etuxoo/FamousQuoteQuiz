using DAL.Dto;
using Dapper.FluentMap.Mapping;

namespace DAL.Schema
{
    public class AuthorSchema : EntityMap<AuthorDto>
    {
        public AuthorSchema()
        {
            Map(i => i.Id).ToColumn("Id");
            Map(i => i.Author).ToColumn("Author");
        }
    }
}
