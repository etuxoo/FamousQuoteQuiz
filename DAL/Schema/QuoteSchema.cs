using DAL.Dto;
using Dapper.FluentMap.Mapping;

namespace DAL.Schema
{
    public class QuoteSchema : EntityMap<QuoteDto>
    {
        public QuoteSchema()
        {
            Map(i => i.Id).ToColumn("Id");
            Map(i => i.Quote).ToColumn("Quote");
            Map(i => i.AuthorId).ToColumn("AuthorId");
        }
    }
}
