using AutoMapper;

namespace Hiwell.AddressBook.Core.Mappings
{
    public interface IMapTo<T>
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(GetType(), typeof(T));
        }
    }
}