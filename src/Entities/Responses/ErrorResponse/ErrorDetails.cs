using System.Text.Json;

namespace Entities.Responses.ErrorResponse
{
    public class ErrorDetails
    {
        public bool Succeeded { get; } = false;
        public string? Message { get; set; }
        public override string ToString() => JsonSerializer.Serialize(this);
    }

}
