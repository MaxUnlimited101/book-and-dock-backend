namespace Backend.DTO;

/// <summary>
/// This is used to indicate the status of a request 
/// </summary>
/// <param name="Message">String message explaining result, or error</param>
/// <param name="AdditionalInformation">Additional object, depends on endpoint, explained in <paramref name="Message"/></param>
public record StatusReturnDto(string Message, object? AdditionalInformation);