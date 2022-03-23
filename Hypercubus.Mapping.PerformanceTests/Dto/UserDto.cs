using Hypercubus.Mapping.PerformanceTests.Entity;

namespace Hypercubus.Mapping.PerformanceTests.Dto
{
    public class UserDto
    {
        public string UserName { get; set; }

        public static implicit operator UserDto(Person user)
        {
            return new UserDto()
            {
                UserName = "Dan Lima"
            };
        }
        
    }
}
