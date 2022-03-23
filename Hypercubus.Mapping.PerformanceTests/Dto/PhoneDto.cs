using Hypercubus.Mapping.PerformanceTests.Entity;

namespace Hypercubus.Mapping.PerformanceTests.Dto
{
    public class PhoneDto
    {
        public long Number { get; set; }

        public static implicit operator PhoneDto(Phone phone)
        {
            return new PhoneDto()
            {
                Number = phone.Number
            };
        }
    }
}
